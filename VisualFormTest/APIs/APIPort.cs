using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiReqKousyou;
using HoppoAlpha.DataLibrary.DataObject;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIPort
    {
        //APIデータの保存
        public static List<ApiMaterial> Materials { get; set; }
        public static List<ApiDeckPort> DeckPorts { get; set; }
        public static List<ApiNdock> Ndocks { get; set; }
        public static Dictionary<int, ApiShip> ShipsDictionary { get; set; }
        public static Basic Basic { get; set; }
        public static List<ApiLog> Logs { get; set; }
        public static DateTime LastUpdate { get; set; }
        public static int CombinedFlag { get; set; }

        //DeckPorts内の撤退している艦
        public static bool[,] IsWithdrawn { get; private set; }
        //出撃中かどうかどうのフラグ
        public static bool OnSortie { get; set; }

        //コンストラクタ
        static APIPort()
        {
            IsWithdrawn = new bool[4, 6];
        }

        //portの読み込み
        public static void ReadPort(string json)
        {
            //JSONオブジェクトの作成
            var ojson = DynamicJson.Parse(json);
            //日時の追加
            LastUpdate = DateTime.Now;
            //api_materialの読み込み
            Materials = ojson.api_data.api_material.Deserialize<List<ApiMaterial>>();
            //api_deck_port
            DeckPorts = ojson.api_data.api_deck_port.Deserialize<List<ApiDeckPort>>();
            //api_ndock
            Ndocks = ojson.api_data.api_ndock.Deserialize<List<ApiNdock>>();
            //api_ship
            List<ApiShip> ships = ojson.api_data.api_ship.Deserialize<List<ApiShip>>();
            ShipsDictionary = new Dictionary<int, ApiShip>();
            foreach(ApiShip s in ships)
            {
                ShipsDictionary[s.api_id] = s;
            }
            //api_basic
            Basic = ojson.api_data.api_basic.Deserialize<ApiBasic>();
            //api_log
            Logs = ojson.api_data.api_log.Deserialize<List<ApiLog>>();
            //api_combined_flag
            if (ojson.api_data.IsDefined("api_combined_flag"))
            {
                CombinedFlag = (int)ojson.api_data.api_combined_flag;
            }
            else
            {
                CombinedFlag = 0;
            }
            
            //出撃報告書に帰投報告
            SortieReportDataBase.SetEndSortie();
            
            //Battleオブジェクトの消去
            APIBattle.BattleView = new BattleView();
            //バトルのキューの初期化
            APIBattle.BattleQueue = new Queue<BattleInfo>();
            //出撃中の艦隊IDを0に
            APIReqMap.SallyDeckPort = 0;
            //○戦目カウンターのリセット
            APIReqMap.BattleCount = 0;
            //退避フラグのリセット
            WithdrawReset();
            //HistoricalData.LogSenkaのセット
            ToSenkaUpdate();
        }

        //shipの追加
        public static void AddShip(ApiShip ship, List<ApiSlotitem> sitem)
        {
            ShipsDictionary[ship.api_id] = ship;
            //装備の追加
            foreach(ApiSlotitem x in sitem)
            {
                SlotItem y = new SlotItem();
                y.api_id = x.api_id; y.api_slotitem_id = x.api_slotitem_id;
                //y.api_locked = 0;
                APIGetMember.AddSlotItem(y);
            }
        }

        //shipの削除
        public static void RemoveShip(ApiShip ship)
        {
            //shipの削除
            if (ShipsDictionary.ContainsKey(ship.api_id))
            {
                ShipsDictionary.Remove(ship.api_id);
            }
            //装備の削除
            foreach(int eqid in ship.api_slot)
            {
                if (eqid == -1) break;
                //装備オブジェクトの抽出
                SlotItem sitem;
                if(APIGetMember.SlotItemsDictionary.TryGetValue(eqid, out sitem))
                {
                    APIGetMember.RemoveSlotItem(sitem);
                }
            }
            //編成側の変更
            for(int i=0; i<DeckPorts.Count; i++)
            {
                for(int j=0; j<DeckPorts[i].api_ship.Count; j++)
                {
                    //編成に登録されている艦が削除対象ならば
                    if(ship.api_id == DeckPorts[i].api_ship[j])
                    {
                        DeckPorts[i].api_ship.RemoveAt(j);
                        DeckPorts[i].api_ship.Add(-1);
                        return;
                    }
                }
            }
        }

        //shipの置き換え（ステータス変更）
        public static void ReplaceShip(ApiShip newship)
        {
            ShipsDictionary[newship.api_id] = newship;
        }

        //shipに戦闘情報の反映（BattleInfoからの上書き）
        public static void SetShipBattleInfo(BattleInfo info)
        {
            //戦闘中の艦隊
            ApiDeckPort combat_deck = DeckPorts[info.DeckPortNumber - 1];
            //船のオブジェクトの抽出
            int cnt = 0;
            foreach(int shipid in combat_deck.api_ship)
            {
                if (shipid == -1) break;
                //HPの書き換え
                ShipsDictionary[shipid].api_nowhp = info.api_nowhps[cnt + 1];
                cnt++;
            }
        }

        //shipに戦闘情報の反映（上の連合艦隊版）
        public static void SetShipBattleCombinedInfo(BattleCombinedInfo cinfo)
        {
            //艦隊番号
            int ifleet_main = cinfo.DeckPortNumber - 1;
            int ifleet_combined = cinfo.DeckPortNumber;//随伴艦体はとりあえず+1
            for(int i=ifleet_main; i<=ifleet_combined; i++)
            {
                //艦隊のオブジェクト
                ApiDeckPort combat_deck = DeckPorts[i];
                int cnt = 0;
                //船オブジェクトの書き換え
                foreach(int shipid in combat_deck.api_ship)
                {
                    if (shipid == -1) break;
                    ApiShip ship = ShipsDictionary[shipid];
                    //HPの書き換え
                    if (i == ifleet_main) ship.api_nowhp = cinfo.api_nowhps[cnt + 1];
                    else if (i == ifleet_combined) ship.api_nowhp = cinfo.api_nowhps_combined[cnt + 1];
                    cnt++;
                }
            }
        }

        //Ndockの置き換え
        public static void SetNdock(List<ApiNdock> ndocks)
        {
            Ndocks = ndocks;
        }

        //Materialの置き換え
        public static void SetMaterial(int[] val)
        {
            int cnt = 0;
            foreach(ApiMaterial m in Materials)
            {
                if (cnt >= val.Length) break;
                m.api_value = val[cnt];
                cnt++;
            }
        }

        //Materialの加算
        public static void AddMaterial(int[] addval)
        {
            int cnt = 0;
            foreach(ApiMaterial m in Materials)
            {
                if (cnt >= addval.Length) break;
                m.api_value += addval[cnt];
                cnt++;
            }
        }

        //Basicの置き換え
        public static void SetBasic(Basic basic)
        {
            Basic = basic;
            //HistoricalData.LogSenkaのセット
            ToSenkaUpdate();
        }

        //提督経験値の置き換え
        public static void SetAdmiralExp(int val)
        {
            Basic.api_experience = val;
            LastUpdate = DateTime.Now;
            //HistoricalData.LogSenkaのセット
            ToSenkaUpdate();
        }

        //勝利数　敗北数の加算
        public static void AddBattleWin(string state)
        {
            if (state == "win") Basic.api_st_win++;
            else if (state == "lose") Basic.api_st_lose++;
        }

        //APIPort→HistoricalData.LogSenka
        private static void ToSenkaUpdate()
        {
            if (HistoricalData.LogSenka == null) return;//排他制御めんどいんで
            //HistoricalData.SetSenkaValue(DateTime.Now, Basic.api_experience, 0, Basic.api_rank);
            HistoricalData.SetSenkaValue(LastUpdate, Basic.api_experience, 0, Basic.api_rank);//極稀におかしくなるらしいんでLastUpdateに
        }

        //IsWithdrawのリセット
        public static void WithdrawReset()
        {
            for(int i=0; i<IsWithdrawn.GetLength(0); i++)
            {
                for(int j =0; j<IsWithdrawn.GetLength(1); j++)
                {
                    IsWithdrawn[i, j] = false;
                }
            }
        }

        //セット
        public static void WithdrawSet(APIBattle.EscapeItem escape)
        {
            IsWithdrawn[escape.FleetNo - 1, escape.ShipNo - 1] = true;
        }
    }

    //ライブラリ以外に依存する部分のヘルパー
    public static class ApiShipHelper
    {
        //完全なダミーデータ
        public static ApiShip MakeDummy()
        {
            ApiShip oship = new ApiShip();
            //初期値でNULLになるプロパティの初期化
            oship.api_exp = Enumerable.Repeat(0, 3).ToList();
            oship.api_slot = Enumerable.Repeat(-1, 5).ToList();
            oship.api_onslot = Enumerable.Repeat(0, 5).ToList();
            oship.api_kyouka = Enumerable.Repeat(0, 5).ToList();
            oship.api_ndock_item = Enumerable.Repeat(0, 2).ToList();

            oship.api_karyoku = Enumerable.Repeat(0, 2).ToList();
            oship.api_raisou = Enumerable.Repeat(0, 2).ToList();
            oship.api_taiku = Enumerable.Repeat(0, 2).ToList();
            oship.api_soukou = Enumerable.Repeat(0, 2).ToList();
            oship.api_kaihi = Enumerable.Repeat(0, 2).ToList();
            oship.api_taisen = Enumerable.Repeat(0, 2).ToList();
            oship.api_sakuteki = Enumerable.Repeat(0, 2).ToList();
            oship.api_lucky = Enumerable.Repeat(0, 2).ToList();
            //値を与えておかないと大変なことになるプロパティ
            oship.api_id = APIPort.ShipsDictionary.Keys.Max() + 100;//キーの最大値＋100 1だと轟沈したとき面倒なことになる可能性あり

            return oship;
        }

        //マスターIDを指定して新規艦の作成
        public static ApiShip MakeNewShip(int shipid)
        {
            ApiShip oship = MakeDummy();

            //マスターIDに入っているか
            ExMasterShip dship;
            if (!APIMaster.MstShips.TryGetValue(shipid, out dship)) return oship;
            //装備IDの最大値の取得
            int slotid = APIPort.ShipsDictionary.Values.SelectMany(x => x.api_slot).Max() + 1;
            //装備スロットの作成
            List<int> slots = oship.api_slot.Select(x => x).ToList();
            if(dship.DefaultSlotItem != null)
            {
                foreach (int i in Enumerable.Range(0, Math.Min(slots.Count, dship.DefaultSlotItem.Count)))
                {
                    if (dship.DefaultSlotItem[i] != -1)
                    {
                        slots[i] = slotid;
                        slotid++;
                    }
                }
            }

            //dshipからパラメーターのラッピング
            //api_idはダミーで初期化済み
            oship.api_sortno = dship.api_sortno;
            oship.api_ship_id = shipid;
            oship.api_lv = 1;
            //api_expはダミー　5
            oship.api_nowhp = dship.api_taik[0];
            oship.api_maxhp = oship.api_nowhp;
            oship.api_leng = dship.api_leng;
            oship.api_slot = slots;
            oship.api_onslot = dship.api_maxeq.Select(x => x).ToList();//そのままリスト指定だと参照渡しになってしまうため 10
            //api_kyoukaはそのまま
            oship.api_backs = dship.api_backs;
            oship.api_fuel = dship.api_fuel_max;
            oship.api_bull = dship.api_bull_max;
            oship.api_slotnum = dship.api_slot_num;//15
            //api_ndock_timeは0なのでそのまま
            //api_ndock_itemも初期値なのでそのまま
            //api_srateはよくわからないのでそのまま
            oship.api_cond = 40;//入手時のcond
            oship.api_karyoku = dship.api_houg.Select(x => x).ToList();//20
            oship.api_raisou = dship.api_raig.Select(x => x).ToList();
            oship.api_taiku = dship.api_tyku.Select(x => x).ToList();
            oship.api_soukou = dship.api_souk.Select(x => x).ToList();
            //api_kaihiは消えてるのでそのまま
            //api_taisenも消えてるのでそのまま　25
            //api_sakutekiも（ｒｙ
            oship.api_lucky = dship.api_luck.Select(x => x).ToList();
            //api_lockedも0なので
            //api_locked_equipも同様
            //api_sally_areaも同様 30

            return oship;
        }
    }
}
