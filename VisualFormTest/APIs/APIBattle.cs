using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.DataObject;
using Codeplex.Data;

namespace VisualFormTest
{
    public static class APIBattle
    {
        //戦闘情報のキュー
        public static Queue<BattleInfo> BattleQueue { get; set; }
        //戦闘形勢
        public static BattleView BattleView { get; set; }
        //護衛退避を準備している船
        public static List<EscapeItem> PrepareWithdrawing { get; set; }

        static APIBattle()
        {
            BattleQueue = new Queue<BattleInfo>();
            BattleView = new BattleView();
        }

        //航空戦・雷撃戦・夜戦のダメージテンプレートクラス
        public class DamageBasic
        {
            public List<int> api_fdam { get; set; }//味方の被ダメージ
            public List<int> api_edam { get; set; }//敵の被ダメージ
            public List<int> api_fydam { get; set; }//味方の与ダメ
            public List<int> api_eydam { get; set; }//敵の与ダメ

            public int[] GetFdam()
            {
                if (api_fdam == null || api_fdam.Count < 7) return new int[6];
                int[] farray = (from x in api_fdam
                               where x != -1
                               select x).ToArray();
                return farray;
            }

            public int[] GetEdam()
            {
                if (api_edam == null || api_edam.Count < 7) return new int[6];
                int[] earray = (from x in api_edam
                                where x != -1
                                select x).ToArray();
                return earray;
            }

            public int[] GetFYdam()
            {
                if (api_fydam == null || api_fydam.Count < 7) return new int[6];
                int[] farray = (from x in api_fydam
                                where x != -1
                                select x).ToArray();
                return farray;
            }

            public int[] GetEYdam()
            {
                if (api_eydam == null || api_eydam.Count < 7) return new int[6];
                int[] earray = (from x in api_eydam
                                where x != -1
                                select x).ToArray();
                return earray;
            }
        }

        //砲撃戦のダメージクラス
        public class DamageHougeki
        {
            public List<int> api_at_list { get; set; }//攻撃側
            public List<List<int>> api_df_list { get; set; }//防御側
            public List<List<double>> api_damage { get; set; }//ダメージ
            public int[] Index { get; set; }
            public int[] Damage { get; set; }
            public int[] FYdam { get; set; }//味方の与ダメ
            public int[] EYdam { get; set; }//敵の与ダメ

            //計算するメソッド（これを実行しないとエラー）
            public void Calc()
            {
                //防御側のインデックスの展開
                this.Index = (from p in api_df_list
                         from q in p
                         where q != -1
                         select q).ToArray();
                //ダメージの展開
                this.Damage = (from p in api_damage
                               from q in p
                               where q != -1
                               select (int)q).ToArray();
                //与ダメ
                FYdam = new int[6]; EYdam = new int[6];
                foreach(int i in Enumerable.Range(0, api_at_list.Count))
                {
                    int id = api_at_list[i];
                    if (id == -1) continue;
                    //味方の場合
                    if(id <= 6)
                    {
                        FYdam[id - 1] += (int)api_damage[i].Sum();
                    }
                    //敵の場合
                    else
                    {
                        EYdam[id - 7] += (int)api_damage[i].Sum();
                    }
                }
            }
        }

        //護衛退避のためのアイテム
        public class EscapeItem
        {
            public int FleetNo { get; set; }
            public int ShipNo { get; set; }
        }

        //昼戦を読むためのメソッド
        public static void ReadSortieBattle(string json)
        {
            string str = json.Replace("[-1]", "[[-1]]").Replace("-1,[", "[-1],[").Replace(@"api_at_list"":[[-1]]", @"api_at_list"":[-1]");//二重配列になっている-1修正
            var ojson = DynamicJson.Parse(str).api_data;
            //オブジェクトの作成
            BattleInfo binfo = ojson.Deserialize<BattleInfo>();
            //基地航空戦
            if(ojson.IsDefined("api_air_base_attack") && ojson.api_air_base_attack != null)
            {
                foreach(var ab in ojson.api_air_base_attack)
                {
                    if(ab.IsDefined("api_stage3") && ab.api_stage3 != null)
                    {
                        DamageBasic d_ab = ab.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_ab);
                    }
                    ((BattleInfo.ApiAirBaseAttack)ab).UpdatePlaneCount();//機数の修正(キャストしないとダメ)？
                }
            }
            //航空戦
            if(ojson.api_kouku.api_stage3 != null)
            {
                DamageBasic d_kouku = ojson.api_kouku.api_stage3.Deserialize<DamageBasic>();
                binfo.AddAirStage3Damage(d_kouku);
            }
            //支援艦隊
            if (ojson.IsDefined("api_support_info") && ojson.api_support_info != null)
            {
                //航空支援
                if(ojson.api_support_info.api_support_airatack != null)
                {
                    if(ojson.api_support_info.api_support_airatack.api_stage3 != null)
                    {
                        DamageBasic d_support_air = ojson.api_support_info.api_support_airatack.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_support_air);
                    }
                }
                //雷撃・砲撃支援
                if(ojson.api_support_info.api_support_hourai != null)
                {
                    List<int> support_hourai_damage = ojson.api_support_info.api_support_hourai.api_damage.Deserialize<List<int>>();
                    DamageBasic d_support_hourai = new DamageBasic()
                    {
                         api_edam = support_hourai_damage, api_fdam = null,
                    };
                    binfo.AddDamage(d_support_hourai);
                }
            }
            //開幕対潜
            if(ojson.api_opening_taisen != null)
            {
                DamageHougeki d_opening_taisen = ojson.api_opening_taisen.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_opening_taisen);
            }
            //開幕雷撃 : atackというスペルミスがデフォルトなので注意
            if(ojson.api_opening_atack != null)
            {
                DamageBasic d_opening = ojson.api_opening_atack.Deserialize<DamageBasic>();
                binfo.AddDamage(d_opening);
            }
            //砲撃戦1回目
            if(ojson.api_hougeki1 != null)
            {
                DamageHougeki d_hou1 = ojson.api_hougeki1.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_hou1);
            }
            //砲撃戦2回目
            if(ojson.api_hougeki2 != null)
            {
                DamageHougeki d_hou2 = ojson.api_hougeki2.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_hou2);
            }
            //砲撃戦3回目（多分使ってない）
            if(ojson.api_hougeki3 != null)
            {
                DamageHougeki d_hou3 = ojson.api_hougeki3.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_hou3);
            }
            //閉幕雷撃
            if(ojson.api_raigeki != null)
            {
                DamageBasic d_raigeki = ojson.api_raigeki.Deserialize<DamageBasic>();
                binfo.AddDamage(d_raigeki);
            }
            bool ispractice = BattleView.Situation == BattleSituation.Practice;
            //APIPort側の書き換え
            if(!ispractice) APIPort.SetShipBattleInfo(binfo);
            //キューに格納
            BattleQueue = new Queue<BattleInfo>();
            BattleQueue.Enqueue(binfo);
            //形勢の記録
            BattleView view = new BattleView(binfo, BattleView);
            if (!ispractice) view.Situation = BattleSituation.Battle;
            else view.Situation = BattleSituation.Practice;
            BattleView = view;
            //DBに追加
            if(!ispractice) EnemyFleetDataBase.AddDataBase();
        }

        //昼戦（航空戦）
        public static void ReadAirbattle(string json)
        {
            string str = json.Replace("[-1]", "[[-1]]").Replace("-1,[", "[-1],[").Replace(@"api_at_list"":[[-1]]", @"api_at_list"":[-1]");//二重配列になっている-1修正
            var ojson = DynamicJson.Parse(str).api_data;
            //オブジェクトの作成
            BattleInfo binfo = ojson.Deserialize<BattleInfo>();
            //航空戦
            if (ojson.api_kouku.api_stage3 != null)
            {
                DamageBasic d_kouku = ojson.api_kouku.api_stage3.Deserialize<DamageBasic>();
                binfo.AddAirStage3Damage(d_kouku);
            }
            //支援艦隊
            if (ojson.IsDefined("api_support_info") && ojson.api_support_info != null)
            {
                //航空支援
                if (ojson.api_support_info.api_support_airatack != null)
                {
                    if (ojson.api_support_info.api_support_airatack.api_stage3 != null)
                    {
                        DamageBasic d_support_air = ojson.api_support_info.api_support_airatack.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_support_air);
                    }
                }
                //雷撃・砲撃支援
                if (ojson.api_support_info.api_support_hourai != null)
                {
                    List<int> support_hourai_damage = ojson.api_support_info.api_support_hourai.api_damage.Deserialize<List<int>>();
                    DamageBasic d_support_hourai = new DamageBasic()
                    {
                        api_edam = support_hourai_damage,
                        api_fdam = null,
                    };
                    binfo.AddDamage(d_support_hourai);
                }
            }
            //2回目の航空戦
            if (ojson.IsDefined("api_kouku2") && ojson.api_kouku2 != null)
            {
                if (ojson.api_kouku2.api_stage3 != null)
                {
                    DamageBasic d_kouku2 = ojson.api_kouku2.api_stage3.Deserialize<DamageBasic>();
                    binfo.AddAirStage3Damage(d_kouku2);
                }

                if (binfo.api_kouku2 != null)
                {
                    if (binfo.api_kouku2.api_stage1 != null)
                    {
                        //総数は多いとこ取り、損失数は和
                        binfo.api_kouku.api_stage1.api_f_count = Math.Max(binfo.api_kouku.api_stage1.api_f_count, binfo.api_kouku2.api_stage1.api_f_count);
                        binfo.api_kouku.api_stage1.api_e_count = Math.Max(binfo.api_kouku.api_stage1.api_e_count, binfo.api_kouku2.api_stage1.api_e_count);

                        binfo.api_kouku.api_stage1.api_f_lostcount += binfo.api_kouku2.api_stage1.api_f_lostcount;
                        binfo.api_kouku.api_stage1.api_e_lostcount += binfo.api_kouku2.api_stage1.api_e_lostcount;
                    }

                    if (binfo.api_kouku2.api_stage2 != null)
                    {
                        //総数は多いとこ取りで
                        binfo.api_kouku.api_stage2.api_f_count = Math.Max(binfo.api_kouku.api_stage2.api_f_count, binfo.api_kouku2.api_stage2.api_f_count);
                        binfo.api_kouku.api_stage2.api_e_count = Math.Max(binfo.api_kouku.api_stage2.api_e_count, binfo.api_kouku2.api_stage2.api_e_count);
                        //損失数は和で
                        binfo.api_kouku.api_stage2.api_f_lostcount += binfo.api_kouku2.api_stage2.api_f_lostcount;
                        binfo.api_kouku.api_stage2.api_e_lostcount += binfo.api_kouku2.api_stage2.api_e_lostcount;
                    }
                }
            }

            //APIPort側の書き換え
            APIPort.SetShipBattleInfo(binfo);
            //キューに格納
            BattleQueue = new Queue<BattleInfo>();
            BattleQueue.Enqueue(binfo);
            //形勢の記録
            BattleView view = new BattleView(binfo, BattleView);
            view.Situation = BattleSituation.BattleAir;
            BattleView = view;
            //DBに追加
            EnemyFleetDataBase.AddDataBase();
        }

        //空襲戦のメソッド
        public static void ReadLdAirBattle(string json)
        {
            //面倒なので航空戦を読む
            ReadAirbattle(json);
            //戦闘形態と勝敗の再判定
            BattleView.Situation = BattleSituation.LdAirBattle;
            BattleView.WinRankEstimated = BattleView.EstimateWinRank();
        }

        //夜戦（追撃）を読むためのメソッド
        public static void ReadBattleMidnight(string json)
        {
            string str = json.Replace("[-1]", "[[-1]]").Replace("-1,[", "[-1],[").Replace(@"api_at_list"":[[-1]]", @"api_at_list"":[-1]");//二重配列になっている-1修正
            var ojson = DynamicJson.Parse(str).api_data;
            //昼戦からのデータ引き継ぎ
            BattleInfo binfo = ojson.Deserialize<BattleInfo>();
            if(BattleQueue.Count > 0) binfo.Overwrap(BattleQueue.Dequeue());
            //ダメージ
            if (ojson.api_hougeki != null)
            {
                DamageHougeki d_mid = ojson.api_hougeki.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_mid);
            }
            //APIPort側の書き換え
            if(BattleView.Situation != BattleSituation.Practice) APIPort.SetShipBattleInfo(binfo);
            //形勢の記録
            BattleView view = new BattleView(binfo, BattleView);
            if (BattleView.Situation == BattleSituation.Battle || BattleView.Situation == BattleSituation.BattleAir) view.Situation = BattleSituation.NightBattleChase;
            else if (BattleView.Situation == BattleSituation.BeforeBattle) view.Situation = BattleSituation.NightBattleSP;
            else if (BattleView.Situation == BattleSituation.Practice) view.Situation = BattleSituation.PracticeNightBattle;
            BattleView = view;
            //キューに記録
            BattleQueue.Enqueue(binfo);
            //DBに追加
            if(BattleView.Situation == BattleSituation.NightBattleSP) EnemyFleetDataBase.AddDataBase();
        }

        //開幕夜戦を読むためのメソッド
        public static void ReadSpMidnight(string json)
        {
            ReadBattleMidnight(json);
        }

        //戦闘結果の内部クラス
        #region 戦闘結果の内部クラス
        public class ApiBattleResultCombined
        {
            public class ApiEnemyInfo
            {
                public string api_level { get; set; }
                public string api_rank { get; set; }
                public string api_deck_name { get; set; }
            }

            public class ApiGetShip
            {
                public int api_ship_id { get; set; }
                public string api_ship_type { get; set; }
                public string api_ship_name { get; set; }
                public string api_ship_getmes { get; set; }
            }

            public class ApiGetUseitem
            {
                public int api_useitem_id { get; set; }
                public string api_useitem_name { get; set; }
            }

            public class ApiEscape
            {
                public List<int> api_escape_idx { get; set; }
                public List<int> api_tow_idx { get; set; }
            }

            public List<int> api_ship_id { get; set; }
            public string api_win_rank { get; set; }
            public int api_get_exp { get; set; }
            public int api_mvp { get; set; }
            public int api_mvp_combined { get; set; }
            public int api_member_lv { get; set; }
            public int api_member_exp { get; set; }
            public int api_get_base_exp { get; set; }
            public List<int> api_get_ship_exp { get; set; }
            public List<int> api_get_ship_exp_combined { get; set; }
            public List<List<int>> api_get_exp_lvup { get; set; }
            public List<List<int>> api_get_exp_lvup_combined { get; set; }
            public int api_dests { get; set; }
            public int api_destsf { get; set; }
            public string api_quest_name { get; set; }
            public int api_quest_level { get; set; }
            public ApiEnemyInfo api_enemy_info { get; set; }
            public int api_first_clear { get; set; }
            public List<int> api_get_flag { get; set; }
            public ApiGetUseitem api_get_useitem { get; set; }
            public ApiGetShip api_get_ship { get; set; }
            public int api_get_eventflag { get; set; }
            public object api_get_exmap_rate { get; set; }
            public int api_get_exmap_useitem_id { get; set; }
            public int api_escape_flag { get; set; }
            public ApiEscape api_escape { get; set; }
        }
        #endregion
        //api_req_sortie/battleresult
        public static void ReadBattleResult(string json, UserControls.TabCounter counter)
        {
            //提督経験値
            var ojson = DynamicJson.Parse(json).api_data;
            ApiBattleResultCombined bresult = ojson.Deserialize<ApiBattleResultCombined>();
            APIPort.SetAdmiralExp(bresult.api_member_exp);
            //戦闘結果の取得（演習を除く）
            if (APIBattle.BattleView.Situation == BattleSituation.Practice || APIBattle.BattleView.Situation == BattleSituation.PracticeNightBattle) { }
            else
            {
                BattleView.WinRank = bresult.api_win_rank;
                if (bresult.api_win_rank == "S" || bresult.api_win_rank == "A" || bresult.api_win_rank == "B") APIPort.AddBattleWin("win");
                else APIPort.AddBattleWin("lose");
                //敵の船のID
                BattleView.EnemyShipID = bresult.api_ship_id.ToArray();
                //カウンターの更新
                KancolleInfoCounter.CheckAllAndCount(counter, BattleView);
                //特別戦果
                int exrate = 0;
                if (bresult.api_get_exmap_rate is double || bresult.api_get_exmap_rate is int) exrate = Convert.ToInt32(bresult.api_get_exmap_rate);
                else if (ojson.api_get_exmap_rate is string) exrate = Convert.ToInt32(bresult.api_get_exmap_rate);
                if(exrate > 0)
                {
                    HistoricalData.SetSenkaValue(DateTime.Now, bresult.api_member_exp, exrate, APIPort.Basic.api_rank);
                }
                //ドロップ
                int dropshipid = -1; bool isgetship = false;
                int dropitemid = -1; bool isgetitem = false;
                if(bresult.api_get_ship != null)
                {
                    //ドロップした艦
                    dropshipid = bresult.api_get_ship.api_ship_id;
                    isgetship = true;
                }
                if(bresult.api_get_useitem != null)
                {
                    //ドロップアイテム
                    dropitemid = bresult.api_get_useitem.api_useitem_id;
                    isgetitem = true;
                }
                //ドロップDBへの記録
                switch(isgetship)
                {
                    case true:
                        if(isgetitem) DropDataBase.AddDataBase(dropshipid, dropitemid, bresult.api_quest_name, bresult.api_enemy_info.api_deck_name);
                        else DropDataBase.AddDataBase(dropshipid, -1, bresult.api_quest_name, bresult.api_enemy_info.api_deck_name);
                        break;
                    case false:
                        if (isgetitem) DropDataBase.AddDataBase(-1, dropitemid, bresult.api_quest_name, bresult.api_enemy_info.api_deck_name);
                        else DropDataBase.AddDataBase(-1, -1, bresult.api_quest_name, bresult.api_enemy_info.api_deck_name);
                        break;
                }
                //ドロップ艦を母港に追加
                if(bresult.api_get_ship != null)
                {
                    //母港への追加（5/18から）
                    ApiShip dropship = ApiShipHelper.MakeNewShip(dropshipid);
                    APIPort.ShipsDictionary[dropship.api_id] = dropship;
                }
                //レベルの取得
                List<int> level = new List<int>();
                foreach(var shipid in APIPort.DeckPorts[APIReqMap.SallyDeckPort -1 ].api_ship)
                {
                    ApiShip oship;
                    if(APIPort.ShipsDictionary.TryGetValue(shipid, out oship))
                    {
                        level.Add(oship.api_lv);
                    }
                    else
                    {
                        level.Add(0);
                    }
                }
                //連合艦隊のレベル
                List<int> levelCombined = null;
                int mvpCombined = -1;//連合艦隊のMVP
                if(bresult.api_get_ship_exp_combined != null && APIPort.DeckPorts.Count >= 2)
                {
                    //レベル
                    levelCombined = new List<int>();
                    foreach(var shipid in APIPort.DeckPorts[1].api_ship)
                    {
                        ApiShip oshipCombined;
                        if(APIPort.ShipsDictionary.TryGetValue(shipid, out oshipCombined)) levelCombined.Add(oshipCombined.api_lv);
                        else levelCombined.Add(0);
                    }
                    //連合艦隊のMVP
                    mvpCombined = bresult.api_mvp_combined;
                }
                //出撃報告書に送信
                SortieReportDataBase.SetBattleResult(bresult.api_win_rank, bresult.api_get_ship_exp, bresult.api_mvp, bresult.api_get_exp, level,
                    bresult.api_get_ship_exp_combined, mvpCombined, levelCombined);
            }
            //形勢をいじるだけ
            APIBattle.BattleView.Situation = BattleSituation.EndBattle;
            //キューを初期化
            BattleQueue = new Queue<BattleInfo>();
        }

        //api_req_combined_battle/battle_water　：　連合艦隊水上打撃戦
        public static void ReadCombinedBattleWater(string json)
        {
            string str = json.Replace("[-1]", "[[-1]]").Replace("-1,[", "[-1],[").Replace(@"api_at_list"":[[-1]]", @"api_at_list"":[-1]");//二重配列になっている-1修正
            var ojson = DynamicJson.Parse(str).api_data;
            //オブジェクトの作成
            BattleCombinedInfo binfo = ojson.Deserialize<BattleCombinedInfo>();
            //基地航空戦
            if (ojson.IsDefined("api_air_base_attack") && ojson.api_air_base_attack != null)
            {
                foreach (var ab in ojson.api_air_base_attack)
                {
                    if (ab.IsDefined("api_stage3") && ab.api_stage3 != null)
                    {
                        DamageBasic d_ab = ab.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_ab);
                    }
                    ((BattleInfo.ApiAirBaseAttack)ab).UpdatePlaneCount();//機数の修正(キャストしないとダメ)？
                }
            }
            //航空戦（本隊側）
            if (ojson.api_kouku.api_stage3 != null)
            {
                DamageBasic d_kouku = ojson.api_kouku.api_stage3.Deserialize<DamageBasic>();
                //binfo.AddDamage(d_kouku);
                binfo.AddAirStage3Damage(d_kouku);
            }
            //航空戦（水雷戦隊）
            if(ojson.api_kouku.api_stage3_combined != null)
            {
                DamageBasic d_kouku_combined = ojson.api_kouku.api_stage3_combined.Deserialize<DamageBasic>();
                binfo.AddCombinedDamage(d_kouku_combined);
            }
            //支援艦隊
            if (ojson.IsDefined("api_support_info") && ojson.api_support_info != null)
            {
                //航空支援
                if (ojson.api_support_info.api_support_airatack != null)
                {
                    if (ojson.api_support_info.api_support_airatack.api_stage3 != null)
                    {
                        DamageBasic d_support_air = ojson.api_support_info.api_support_airatack.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_support_air);
                    }
                }
                //雷撃・砲撃支援
                if (ojson.api_support_info.api_support_hourai != null)
                {
                    List<int> support_hourai_damage = ojson.api_support_info.api_support_hourai.api_damage.Deserialize<List<int>>();
                    DamageBasic d_support_hourai = new DamageBasic()
                    {
                        api_edam = support_hourai_damage,
                        api_fdam = null,
                    };
                    binfo.AddDamage(d_support_hourai);
                }
            }
            //開幕対潜
            if (ojson.api_opening_taisen != null)
            {
                DamageHougeki d_opening_taisen = ojson.api_opening_taisen.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_opening_taisen);
            }
            //開幕雷撃（第2艦隊） : atackというスペルミスがデフォルトなので注意
            if (ojson.IsDefined("api_opening_atack") && ojson.api_opening_atack != null)
            {
                DamageBasic d_opening = ojson.api_opening_atack.Deserialize<DamageBasic>();
                binfo.AddCombinedDamage(d_opening);//第2vs敵の構図で考える
            }
            //砲撃戦1回目
            if (ojson.IsDefined("api_hougeki1") && ojson.api_hougeki1 != null)
            {
                DamageHougeki d_hou1 = ojson.api_hougeki1.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_hou1);
            }
            //砲撃戦2回目
            if (ojson.IsDefined("api_hougeki2") && ojson.api_hougeki2 != null)
            {
                DamageHougeki d_hou2 = ojson.api_hougeki2.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_hou2);
            }
            //砲撃戦3回目（第2艦隊）
            if (ojson.IsDefined("api_hougeki3") && ojson.api_hougeki3 != null)
            {
                DamageHougeki d_hou3 = ojson.api_hougeki3.Deserialize<DamageHougeki>();
                binfo.AddCombinedDamage(d_hou3);
            }
            //閉幕雷撃（第2艦隊）
            if (ojson.IsDefined("api_raigeki") && ojson.api_raigeki != null)
            {
                DamageBasic d_raigeki = ojson.api_raigeki.Deserialize<DamageBasic>();
                binfo.AddCombinedDamage(d_raigeki);
            }
            //APIPort側の書き換え
            APIPort.SetShipBattleCombinedInfo(binfo);
            //キューに格納
            BattleQueue = new Queue<BattleInfo>();
            BattleQueue.Enqueue(binfo);
            //形勢の記録
            BattleView view = new BattleView(binfo, BattleView);
            view.Situation = BattleSituation.CombinedBattleWater;
            BattleView = view;
            //DB
            EnemyFleetDataBase.AddDataBase();
        }

        //api_req_combined_battle/battle : 連合艦隊機動部隊
        public static void ReadCombinedBattle(string json)
        {
            string str = json.Replace("[-1]", "[[-1]]").Replace("-1,[", "[-1],[").Replace(@"api_at_list"":[[-1]]", @"api_at_list"":[-1]");//二重配列になっている-1修正
            var ojson = DynamicJson.Parse(str).api_data;
            //オブジェクトの作成
            BattleCombinedInfo binfo = ojson.Deserialize<BattleCombinedInfo>();
            //基地航空戦
            if (ojson.IsDefined("api_air_base_attack") && ojson.api_air_base_attack != null)
            {
                foreach (var ab in ojson.api_air_base_attack)
                {
                    if (ab.IsDefined("api_stage3") && ab.api_stage3 != null)
                    {
                        DamageBasic d_ab = ab.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_ab);
                    }
                    ((BattleInfo.ApiAirBaseAttack)ab).UpdatePlaneCount();//機数の修正(キャストしないとダメ)？
                }
            }
            //航空戦（本隊側）
            if (ojson.api_kouku.api_stage3 != null)
            {
                DamageBasic d_kouku = ojson.api_kouku.api_stage3.Deserialize<DamageBasic>();
                //binfo.AddDamage(d_kouku);
                binfo.AddAirStage3Damage(d_kouku);
            }
            //航空戦（水雷戦隊）
            if (ojson.api_kouku.api_stage3_combined != null)
            {
                DamageBasic d_kouku_combined = ojson.api_kouku.api_stage3_combined.Deserialize<DamageBasic>();
                binfo.AddCombinedDamage(d_kouku_combined);
            }
            //支援艦隊
            if (ojson.IsDefined("api_support_info") && ojson.api_support_info != null)
            {
                //航空支援
                if (ojson.api_support_info.api_support_airatack != null)
                {
                    if (ojson.api_support_info.api_support_airatack.api_stage3 != null)
                    {
                        DamageBasic d_support_air = ojson.api_support_info.api_support_airatack.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_support_air);
                    }
                }
                //雷撃・砲撃支援
                if (ojson.api_support_info.api_support_hourai != null)
                {
                    List<int> support_hourai_damage = ojson.api_support_info.api_support_hourai.api_damage.Deserialize<List<int>>();
                    DamageBasic d_support_hourai = new DamageBasic()
                    {
                        api_edam = support_hourai_damage,
                        api_fdam = null,
                    };
                    binfo.AddDamage(d_support_hourai);
                }
            }
            //開幕対潜
            if (ojson.api_opening_taisen != null)
            {
                DamageHougeki d_opening_taisen = ojson.api_opening_taisen.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_opening_taisen);
            }
            //開幕雷撃（第2艦隊） : atackというスペルミスがデフォルトなので注意
            if (ojson.IsDefined("api_opening_atack") && ojson.api_opening_atack != null)
            {
                DamageBasic d_opening = ojson.api_opening_atack.Deserialize<DamageBasic>();
                binfo.AddCombinedDamage(d_opening);//第2vs敵の構図で考える
            }
            //砲撃戦1回目（第2艦隊）
            if (ojson.IsDefined("api_hougeki1") && ojson.api_hougeki1 != null)
            {
                DamageHougeki d_hou1 = ojson.api_hougeki1.Deserialize<DamageHougeki>();
                binfo.AddCombinedDamage(d_hou1);
            }
            //第2艦隊閉幕雷撃（第2艦隊）
            if (ojson.IsDefined("api_raigeki") && ojson.api_raigeki != null)
            {
                DamageBasic d_raigeki = ojson.api_raigeki.Deserialize<DamageBasic>();
                binfo.AddCombinedDamage(d_raigeki);
            }
            //砲撃戦2回目（第1艦隊）
            if (ojson.IsDefined("api_hougeki2") && ojson.api_hougeki2 != null)
            {
                DamageHougeki d_hou2 = ojson.api_hougeki2.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_hou2);
            }
            //砲撃戦3回目（第1艦隊）
            if (ojson.IsDefined("api_hougeki3") && ojson.api_hougeki3 != null)
            {
                DamageHougeki d_hou3 = ojson.api_hougeki3.Deserialize<DamageHougeki>();
                binfo.AddDamage(d_hou3);
            }
            //APIPort側の書き換え
            APIPort.SetShipBattleCombinedInfo(binfo);
            //キューに格納
            BattleQueue = new Queue<BattleInfo>();
            BattleQueue.Enqueue(binfo);
            //形勢の記録
            BattleView view = new BattleView(binfo, BattleView);
            view.Situation = BattleSituation.CombinedBattle;
            BattleView = view;
            //DB
            EnemyFleetDataBase.AddDataBase();
        }

        //api_req_combined_battle/airbattle : 連合艦隊機動部隊　航空戦
        public static void ReadCombinedAirbattle(string json)
        {
            string str = json.Replace("[-1]", "[[-1]]").Replace("-1,[", "[-1],[").Replace(@"api_at_list"":[[-1]]", @"api_at_list"":[-1]");//二重配列になっている-1修正
            var ojson = DynamicJson.Parse(str).api_data;
            //オブジェクトの作成
            BattleCombinedInfo binfo = ojson.Deserialize<BattleCombinedInfo>();
            //航空戦（本隊側）
            if (ojson.api_kouku.api_stage3 != null)
            {
                DamageBasic d_kouku = ojson.api_kouku.api_stage3.Deserialize<DamageBasic>();
                //binfo.AddDamage(d_kouku);
                binfo.AddAirStage3Damage(d_kouku);
            }
            //航空戦（水雷戦隊）
            if (ojson.api_kouku.api_stage3_combined != null)
            {
                DamageBasic d_kouku_combined = ojson.api_kouku.api_stage3_combined.Deserialize<DamageBasic>();
                binfo.AddCombinedDamage(d_kouku_combined);
            }
            //支援艦隊
            if (ojson.IsDefined("api_support_info") && ojson.api_support_info != null)
            {
                //航空支援
                if (ojson.api_support_info.api_support_airatack != null)
                {
                    if (ojson.api_support_info.api_support_airatack.api_stage3 != null)
                    {
                        DamageBasic d_support_air = ojson.api_support_info.api_support_airatack.api_stage3.Deserialize<DamageBasic>();
                        binfo.AddDamage(d_support_air);
                    }
                }
                //雷撃・砲撃支援
                if (ojson.api_support_info.api_support_hourai != null)
                {
                    List<int> support_hourai_damage = ojson.api_support_info.api_support_hourai.api_damage.Deserialize<List<int>>();
                    DamageBasic d_support_hourai = new DamageBasic()
                    {
                        api_edam = support_hourai_damage,
                        api_fdam = null,
                    };
                    binfo.AddDamage(d_support_hourai);
                }
            }
            if (ojson.IsDefined("api_kouku2") && ojson.api_kouku2 != null)
            {
                //航空戦2回目
                if (ojson.IsDefined("api_kouku2") && ojson.api_kouku2.api_stage3 != null)
                {
                    DamageBasic d_kouku2 = ojson.api_kouku2.api_stage3.Deserialize<DamageBasic>();
                    binfo.AddAirStage3Damage(d_kouku2);
                }
                if (ojson.IsDefined("api_kouku2") && ojson.api_kouku2.api_stage3_combined != null)
                {
                    DamageBasic d_kouku2_combined = ojson.api_kouku2.api_stage3_combined.Deserialize<DamageBasic>();
                    binfo.AddCombinedDamage(d_kouku2_combined);
                }
                //2回の航空戦の結果のマージ
                if (binfo.api_kouku2 != null)
                {
                    if (binfo.api_kouku2.api_stage1 != null)
                    {
                        //総数は多いとこ取り、損失数は和
                        binfo.api_kouku.api_stage1.api_f_count = Math.Max(binfo.api_kouku.api_stage1.api_f_count, binfo.api_kouku2.api_stage1.api_f_count);
                        binfo.api_kouku.api_stage1.api_e_count = Math.Max(binfo.api_kouku.api_stage1.api_e_count, binfo.api_kouku2.api_stage1.api_e_count);

                        binfo.api_kouku.api_stage1.api_f_lostcount += binfo.api_kouku2.api_stage1.api_f_lostcount;
                        binfo.api_kouku.api_stage1.api_e_lostcount += binfo.api_kouku2.api_stage1.api_e_lostcount;
                    }

                    if (binfo.api_kouku2.api_stage2 != null)
                    {
                        //総数は多いとこ取りで
                        binfo.api_kouku.api_stage2.api_f_count = Math.Max(binfo.api_kouku.api_stage2.api_f_count, binfo.api_kouku2.api_stage2.api_f_count);
                        binfo.api_kouku.api_stage2.api_e_count = Math.Max(binfo.api_kouku.api_stage2.api_e_count, binfo.api_kouku2.api_stage2.api_e_count);
                        //損失数は和で
                        binfo.api_kouku.api_stage2.api_f_lostcount += binfo.api_kouku2.api_stage2.api_f_lostcount;
                        binfo.api_kouku.api_stage2.api_e_lostcount += binfo.api_kouku2.api_stage2.api_e_lostcount;
                    }
                }
            }
            //APIPort側の書き換え
            APIPort.SetShipBattleInfo(binfo);
            //キューに格納
            BattleQueue = new Queue<BattleInfo>();
            BattleQueue.Enqueue(binfo);
            //形勢の記録
            BattleView view = new BattleView(binfo, BattleView);
            view.Situation = BattleSituation.CombinedBattleAir;
            BattleView = view;
            //DBに追加
            EnemyFleetDataBase.AddDataBase();
        }

        //api_req_combined_battle/ld_airbattle : 連合艦隊空襲戦
        public static void ReadCombinedLdAirbattle(string json)
        {
            //面倒なので航空戦を読む
            ReadCombinedAirbattle(json);
            //戦闘形態と勝敗の再判定
            BattleView.Situation = BattleSituation.CombinedLdAirBattle;
            BattleView.WinRankEstimated = BattleView.EstimateWinRank();
        }

        //api_req_combined_battle/midnight_battle : 連合艦隊追撃夜戦
        public static void ReadCombinedMidnightBattle(string json)
        {
            string str = json.Replace("[-1]", "[[-1]]").Replace("-1,[", "[-1],[").Replace(@"api_at_list"":[[-1]]", @"api_at_list"":[-1]");//二重配列になっている-1修正
            var ojson = DynamicJson.Parse(str).api_data;
            //昼戦からのデータ引き継ぎ
            BattleCombinedInfo binfo = ojson.Deserialize<BattleCombinedInfo>();
            if (BattleQueue.Count > 0) binfo.Overwrap(BattleQueue.Dequeue());
            //ダメージ
            if (ojson.IsDefined("api_hougeki") && ojson.api_hougeki != null)
            {
                DamageHougeki d_mid = ojson.api_hougeki.Deserialize<DamageHougeki>();
                binfo.AddCombinedDamage(d_mid);
            }
            //APIPort側の書き換え
            APIPort.SetShipBattleCombinedInfo(binfo);
            //形勢の記録
            BattleView view = new BattleView(binfo, BattleView);
            if (BattleView.Situation == BattleSituation.BeforeBattle) view.Situation = BattleSituation.CombinedNightBattleSP;
            else view.Situation = BattleSituation.CombinedNightBattleChase;
            BattleView = view;
            //キューに記録
            BattleQueue.Enqueue(binfo);
        }

        public static void ReadCombinedMidnightBattleSP(string json)
        {
            ReadCombinedMidnightBattle(json);
        }

        //api_req_combined_battle/battleresult : 連合艦隊戦闘結果
        public static void ReadCombinedBattleResult(string json, UserControls.TabCounter counter)
        {
            //通常の戦闘終了と同じ処理
            ReadBattleResult(json, counter);
            //形勢を連合艦隊用に
            APIBattle.BattleView.Situation = BattleSituation.EndCombinedBattle;
            //護衛退避のチェック
            var ojson = DynamicJson.Parse(json).api_data;
            if(ojson.IsDefined("api_escape") && ojson.api_escape != null)
            {
                ApiEscape escape = ojson.api_escape.Deserialize<ApiEscape>();
                PrepareWithdrawing = new List<EscapeItem>();
                //退却する艦隊のインデックス
                int[] withdraw = new int[] { escape.api_escape_idx[0], escape.api_tow_idx[0] };
                foreach(int x in withdraw)
                {
                    EscapeItem item = new EscapeItem();
                    //艦隊番号
                    if (x > 6) item.FleetNo = APIReqMap.SallyDeckPort + 1;
                    else item.FleetNo = APIReqMap.SallyDeckPort;
                    //艦隊での番号
                    item.ShipNo = (x - 1) % 6 + 1;
                    //追加
                    PrepareWithdrawing.Add(item);
                }
            }
        }

        //api_req_combined_battle/goback_port : 護衛退避
        public static void ReadCombinedGobackPort()
        {
            foreach(EscapeItem x in PrepareWithdrawing)
            {
                APIPort.WithdrawSet(x);
            }
        }
    }

    //戦闘情報をストックするためのクラス
    #region 戦闘情報のストッククラス
    public class BattleInfo
    {
        //味方の被ダメ合計
        public int[] Fdamage { get; set; }
        //敵の被ダメ合計
        public int[] Edamage { get; set; }
        //削ったHP
        public int Fscore { get; set; }
        public int[] FscoreArray { get; set; }
        public int Escore { get; set; }
        //MAXHP
        public int FscoreMax { get; set; }
        public int EscoreMax { get; set; }
        public int[] FStartHP { get; set; }
        public int[] EStartHP { get; set; }
        //最大得点を計算し終わったか
        public bool MaxScoreCalcEnd { get; set; }
        //推定値かどうか
        public bool[] IsPredicted { get; set; }

        //デシリアライズで読む部分
        public int api_dock_id { get; set; }//出撃している艦隊 第1艦隊＝1
        public int api_deck_id { get; set; }//dockとdeck両方あるため
        public int[] api_nowhps { get; set; }//戦闘開始前のHP
        public int[] api_maxhps { get; set; }//各船のMaxHP
        public int[] api_ship_ke { get; set; }//敵艦隊のShipID
        public int[] api_ship_lv { get; set; }//艦隊のレベル
        public List<List<int>> api_eSlot { get; set; }//敵艦隊の装備アイテム
        public List<List<int>> api_eParam { get; set; }//敵艦隊のパラメーター
        public int[] api_search { get; set; }//索敵
        public object[] api_formation { get; set; }//陣形
        public ApiKouku api_kouku { get; set; }//航空戦
        public ApiKouku api_kouku2 { get; set; }//航空戦Phase2
        public List<ApiAirBaseAttack> api_air_base_attack { get; set; }//基地航空戦

        //内部クラス
        #region 内部クラス
        //航空戦
        public class ApiKouku
        {
            public ApiStage1 api_stage1 { get; set; }
            public ApiStage2 api_stage2 { get; set; }
        }

        public class ApiAirBaseAttack : ApiKouku
        {
            public int api_base_id { get; set; }
            public List<ApiSquadronPlane> api_squadron_plane { get; set; }

            public void UpdatePlaneCount()
            {
                if(api_squadron_plane != null && APIGetMember.BaseAirCorps != null && api_base_id <= APIGetMember.BaseAirCorps.Count)
                {
                    var baseobj = APIGetMember.BaseAirCorps[api_base_id - 1];
                    foreach(var i in Enumerable.Range(0, Math.Min(baseobj.api_plane_info.Count, api_squadron_plane.Count)))
                    {
                        //機数の修正
                        baseobj.api_plane_info[i].api_count = api_squadron_plane[i].api_count;
                    }
                }
            }
        }

        public class ApiSquadronPlane
        {
            public int api_mst_id { get; set; }
            public int api_count { get; set; }
        }

        //Stage1
        public class ApiStage1
        {
            public int api_f_count { get; set; }
            public int api_f_lostcount { get; set; }
            public int api_e_count { get; set; }
            public int api_e_lostcount { get; set; }
            public int api_disp_seiku { get; set; }
            public List<int> api_touch_plane { get; set; }
        }

        //Stage2
        public class ApiStage2
        {
            public int api_f_count { get; set; }
            public int api_f_lostcount { get; set; }
            public int api_e_count { get; set; }
            public int api_e_lostcount { get; set; }
            public int api_disp_seiku { get; set; }
        }

        public class DameconHandler
        {
            public int Flag { get; set; }
            public int OnSlot { get; set; }
            public bool ExSlot { get; set; }
        }
        #endregion

        //プロパティ
        #region プロパティ
        public int DeckPortNumber
        {
            get
            {
                return this.api_deck_id + this.api_dock_id;
            }
        }
        public int[] Formation
        {
            get
            {
                List<int> form_int = new List<int>();
                foreach(object x in api_formation)
                {
                    if (x is string) form_int.Add(Convert.ToInt32(x));
                    else if (x is int) form_int.Add((int)x);
                    else if (x is double) form_int.Add((int)((double)x));
                }
                return form_int.ToArray();
            }
        }
        #endregion

        //コンストラクタ
        public BattleInfo()
        {
            Fdamage = new int[6];
            Edamage = new int[6];
            FscoreArray = new int[6];
            FStartHP = new int[6];
            EStartHP = new int[6];
            IsPredicted = new bool[6];
        }

        //戦闘情報を引き継いでオーバーラップ
        public void Overwrap(BattleInfo info)
        {
            this.Fscore += info.Fscore;
            this.Escore += info.Escore;
            this.FscoreMax = info.FscoreMax;
            this.EscoreMax = info.EscoreMax;
            this.FscoreArray = info.FscoreArray;
            this.FStartHP = info.FStartHP; this.EStartHP = info.EStartHP;
            this.MaxScoreCalcEnd = true;
            this.IsPredicted = info.IsPredicted;
            //シリアル化でも引き継ぐ部分
            //this.api_ship_ke = info.api_ship_ke;
            this.api_search = info.api_search;
            this.api_formation = info.api_formation;
            this.api_kouku = info.api_kouku;
        }

        //ダメージ計算の共通部分
        protected void AddDamage()
        {
            if (this.MaxScoreCalcEnd) return;
            //ScoreMaxの計算
            for(int i=0; i<api_nowhps.Length; i++)
            {
                int hp = api_nowhps[i];
                if (hp == -1) continue;
                if (i <= 6) EscoreMax += hp;
                else FscoreMax += hp;
            }
            //初期HPの記録
            for (int i = 0; i < FStartHP.Length; i++) FStartHP[i] = Math.Max(api_nowhps[i + 1], 0);
            for (int i = 0; i < EStartHP.Length; i++) EStartHP[i] = Math.Max(api_nowhps[i + 7], 0);
            this.MaxScoreCalcEnd = true;
        }

        //ダメージの加算
        public void AddDamage(APIBattle.DamageBasic db)
        {
            AddDamage();
            int[] f_dam = db.GetFdam();//味方のダメージ
            int[] e_dam = db.GetEdam();
            List<DameconHandler> dameconflag = HasDamecon();
            for(int i=0; i<Fdamage.Length; i++)
            {
                //味方の被ダメージ
                if(api_nowhps[i+1] != -1)
                {
                    Fdamage[i] += f_dam[i];
                    //実際に削られたHPの計算
                    int oldhp = api_nowhps[i + 1];
                    int df_f_hp = Math.Min(f_dam[i], api_nowhps[i + 1]);
                    api_nowhps[i + 1] -= df_f_hp;
                    //沈没処理
                    bool sank = oldhp > 0 && api_nowhps[i + 1] <= 0;
                    if (sank && dameconflag[i].Flag > 0)
                    {
                        int id = APIPort.DeckPorts[DeckPortNumber - 1].api_ship[i];
                        if (APIPort.ShipsDictionary.ContainsKey(id))
                        {
                            //HPの回復
                            switch (dameconflag[i].Flag)
                            {
                                case 1://応急修理要員
                                    api_nowhps[i + 1] = (int)((double)api_maxhps[i + 1] * (double)0.2);
                                    break;
                                case 2://応急修理女神
                                    api_nowhps[i + 1] = api_maxhps[i + 1];
                                    break;
                            }
                            //装備消費
                            ApiShip oship = APIPort.ShipsDictionary[id];
                            if (!dameconflag[i].ExSlot) oship.api_slot[dameconflag[i].OnSlot] = -1;
                            else oship.api_slot_ex = -1;
                            //燃料弾薬補給
                            if (dameconflag[i].Flag == 2)
                            {
                                oship.api_fuel = oship.DShip.api_fuel_max;
                                oship.api_bull = oship.DShip.api_bull_max;
                            }
                        }
                    }
                    //Scoreは交差させる（被ダメージの引数を与ダメージに替えるため）
                    Escore += df_f_hp;
                }
                //敵の被ダメージ
                if (api_nowhps[i + 7] != -1)
                {
                    Edamage[i] += e_dam[i];
                    int df_e_hp = Math.Min(e_dam[i], api_nowhps[i + 7]);
                    api_nowhps[i + 7] -= df_e_hp;
                    Fscore += df_e_hp;
                }
            }
            //与ダメ
            int[] f_ydam = db.GetFYdam();
            for(int i=0; i<FscoreArray.Length; i++)
            {
                FscoreArray[i] += f_ydam[i];
            }
        }

        public void AddDamage(APIBattle.DamageHougeki hou)
        {
            AddDamage();
            hou.Calc();
            List<DameconHandler> dameconflag = HasDamecon();
            for(int i=0; i<hou.Index.Length; i++)
            {
                int idx = hou.Index[i];
                int damage = hou.Damage[i];
                //味方のダメージの場合
                if (idx <= 6 && api_nowhps[idx] != -1)
                {
                    Fdamage[idx - 1] += damage;
                    int oldhp = api_nowhps[idx];
                    int df_f_hp = Math.Min(damage, api_nowhps[idx]);//実際に削られたHP
                    api_nowhps[idx] -= df_f_hp;
                    //ダメコン処理
                    //沈没処理
                    bool sank = oldhp > 0 && api_nowhps[idx] <= 0;
                    if (sank && dameconflag[idx-1].Flag > 0)
                    {
                        int id = APIPort.DeckPorts[DeckPortNumber - 1].api_ship[idx - 1];
                        if (APIPort.ShipsDictionary.ContainsKey(id))
                        {
                            //HPの回復
                            switch (dameconflag[idx-1].Flag)
                            {
                                case 1://応急修理要員
                                    api_nowhps[idx] = (int)((double)api_maxhps[idx] * (double)0.2);
                                    break;
                                case 2://応急修理女神
                                    api_nowhps[idx] = api_maxhps[idx];
                                    break;
                            }
                            //装備消費
                            ApiShip oship = APIPort.ShipsDictionary[id];
                            if (!dameconflag[idx - 1].ExSlot) oship.api_slot[dameconflag[idx - 1].OnSlot] = -1;
                            else oship.api_slot_ex = -1;
                            //燃料弾薬補給
                            if (dameconflag[idx-1].Flag == 2)
                            {
                                oship.api_fuel = oship.DShip.api_fuel_max;
                                oship.api_bull = oship.DShip.api_bull_max;
                            }
                        }
                    }
                    //敵のスコアに加算
                    Escore += df_f_hp;//Scoreは交差
                }
                else if(idx >= 7 && api_nowhps[idx] != -1)
                {
                    Edamage[idx - 7] += damage;
                    int df_e_hp = Math.Min(damage, api_nowhps[idx]);//実際に削られたHP
                    api_nowhps[idx] -= df_e_hp;
                    Fscore += df_e_hp;
                }
            }
            //与ダメ
            for(int i=0; i<FscoreArray.Length; i++)
            {
                FscoreArray[i] += hou.FYdam[i];
            }
        }

        //開幕航空戦のAddDamage＋与ダメの推定
        public void AddAirStage3Damage(APIBattle.DamageBasic db)
        {
            //ベースのAddDamage
            AddDamage(db);
            //敵味方の被ダメを取得
            int[] f_dam = db.GetFdam();
            int[] e_dam = db.GetEdam();
            //与ダメウェイト
            int[] fyweight = new int[6]; int[] eyweight = new int[6];
            //味方の与ダメウェイトの計算
            int k = 0;
            foreach(int shipid in APIPort.DeckPorts[DeckPortNumber-1].api_ship)
            {
                if (shipid == -1) continue;
                ApiShip oship = APIPort.ShipsDictionary[shipid];
                foreach(var dslotitem in oship.GetDSlotitems(APIGetMember.SlotItemsDictionary))
                {
                    //装備の種類ごとにウェイト付
                    switch (dslotitem.EquipType)
                    {
                        case 8://艦攻なら+2
                            fyweight[k] += 2;
                            break;
                        case 7://艦爆なら+1
                            fyweight[k] += 1;
                            break;
                        case 11://水爆なら+1
                            fyweight[k] += 1;
                            break;
                    }
                }
                k++;
            }
            //敵の与ダメウェイトの計算
            k = 0;
            foreach(List<int> eqlist in api_eSlot)
            {
                foreach(int eqid in eqlist)
                {
                    if (eqid == -1) continue;
                    var dslotitem = APIMaster.MstSlotitems[eqid];
                    switch (dslotitem.EquipType)
                    {
                        case 8://艦攻なら+2
                            eyweight[k] += 2;
                            break;
                        case 7://艦爆なら+1
                            eyweight[k] += 1;
                            break;
                        case 11://水爆なら+1
                            eyweight[k] += 1;
                            break;
                    }
                }
                k++;
            }
            //ウェイトにしたがって分配
            if(fyweight.Sum() != 0)
            {
                double fdamageunit = (double)e_dam.Sum() / (double)fyweight.Sum();
                foreach(int i in Enumerable.Range(0, e_dam.Length))
                {
                    FscoreArray[i] += (int)(fdamageunit * (double)fyweight[i]);
                    if (fyweight[i] > 0) IsPredicted[i] = true;
                }
            }
            /*
             * 敵のスコアの配列が必要ならここに実装
             */ 
        }

        //MVPの取得
        public int GetMVPIndex()
        {
            //全て0の場合
            bool allzero = FscoreArray[0] == 0 && FscoreArray[1] == 0 && FscoreArray[2] == 0 &&
                            FscoreArray[3] == 0 && FscoreArray[4] == 0 && FscoreArray[5] == 0;
            if (allzero) return 0;
            //誰かはダメージを与えている場合
            int maxscore = FscoreArray.Max();
            foreach (int i in Enumerable.Range(0, FscoreArray.Length))
            {
                if (FscoreArray[FscoreArray.Length - i - 1] == maxscore) return (FscoreArray.Length - i - 1);
            }
            //例外
            throw new IndexOutOfRangeException();
        }

        //与えるダメの表示用
        public string[] GetFYdamDisplay()
        {
            string[] disp = new string[FscoreArray.Length];
            foreach(int i in Enumerable.Range(0, FscoreArray.Length))
            {
                string str = FscoreArray[i].ToString();
                if (IsPredicted[i]) str += "?";
                disp[i] = str;
            }
            return disp;
        }

        //ダメコンを持っているかどうか  : 0なし、1応急修理、2女神
        public List<DameconHandler> HasDamecon()
        {
            return APIPort.DeckPorts[DeckPortNumber - 1].api_ship.Select(delegate(int shipid)
            {
                if (shipid == -1) return new DameconHandler();
                if (!APIPort.ShipsDictionary.ContainsKey(shipid)) return new DameconHandler();
                //slotitemid = 42 ダメコン、43　女神
                DameconHandler handler = new DameconHandler();

                //通常のスロット
                foreach (var i in Enumerable.Range(0, APIPort.ShipsDictionary[shipid].api_slot.Count))
                {
                    var x = APIPort.ShipsDictionary[shipid].api_slot[i];

                    SlotItem oslot;
                    if (APIGetMember.SlotItemsDictionary.TryGetValue(x, out oslot))
                    {
                        int masterid = oslot.api_slotitem_id;
                        if (masterid == 42)
                        {
                            handler.Flag = 1;
                            handler.OnSlot = i;
                        }
                        else if (masterid == 43)
                        {
                            handler.Flag = 2;
                            handler.OnSlot = i;
                        }
                    }
                }

                //拡張スロット
                var exslotid = APIPort.ShipsDictionary[shipid].api_slot_ex;
                SlotItem exslot;
                if(APIGetMember.SlotItemsDictionary.TryGetValue(exslotid, out exslot))
                {
                    int masterexid = exslot.api_slotitem_id;
                    if(masterexid == 42)
                    {
                        handler.Flag = 1;
                        handler.ExSlot = true;
                    }
                    else if(masterexid == 43)
                    {
                        handler.Flag = 2;
                        handler.ExSlot = true;
                    }
                }

                return handler;
            }).ToList();
        }
    }

    //連合艦隊のストック
    public class BattleCombinedInfo : BattleInfo
    {
        public int[] FdamageCombined { get; set; }
        public int[] FScoreArrayCombined { get; set; }//与ダメ
        public int EscoreCombined { get; set; }//第2vs敵
        public int EscoreMaxCombined { get; set; }
        public int[] FStartHPCombined { get; set; }//開戦時のHP

        //デシリアライズ
        public int[] api_nowhps_combined { get; set; }
        public int[] api_maxhps_combined { get; set; }

        public bool IsCombinedMaxScoreCalcEnd { get; set; }


        //コンストラクタ
        public BattleCombinedInfo()
            : base()
        {
            this.FdamageCombined = new int[6];
            this.FStartHPCombined = new int[6];
            this.FScoreArrayCombined = new int[6];
        }

        //オーバーラップ（隠蔽して実装）
        public new void Overwrap(BattleInfo info)
        {
            base.Overwrap(info);//アップキャストして通常戦闘のオーバーラップを呼び出し

            if (!(info is BattleCombinedInfo)) throw new InvalidCastException();
            BattleCombinedInfo cinfo = info as BattleCombinedInfo;
            this.FScoreArrayCombined = cinfo.FScoreArrayCombined;
            this.FStartHPCombined = cinfo.FStartHPCombined;
            this.IsCombinedMaxScoreCalcEnd = true;//よくわからないけど

            this.EscoreCombined += cinfo.EscoreCombined;
            this.EscoreMaxCombined = cinfo.EscoreMaxCombined;
        }

        //ダメージ追加の共通処理
        protected void AddCombinedDamage()
        {
            if (this.IsCombinedMaxScoreCalcEnd) return;

            //EscoreMaxCombinedの計算
            this.EscoreMaxCombined = api_nowhps_combined.Where(x => x >= 0).Sum();
            //初期HPの記録
            for (int i = 0; i < FStartHPCombined.Length; i++) FStartHPCombined[i] = Math.Max(api_nowhps_combined[i + 1], 0);

            this.IsCombinedMaxScoreCalcEnd = true;
        }

        //第2艦隊側のダメージ
        public void AddCombinedDamage(APIBattle.DamageBasic db)
        {
            AddDamage();
            AddCombinedDamage();
            int[] f_dam = db.GetFdam();//味方のダメージ
            int[] e_dam = db.GetEdam();
            List<DameconHandler> dameconflag = HasDameconCombined();
            for (int i = 0; i < FdamageCombined.Length; i++)
            {
                //味方
                if (api_nowhps_combined[i + 1] != -1)
                {
                    FdamageCombined[i] += f_dam[i];
                    //実際に削られたHPの計算
                    int oldhp = api_nowhps_combined[i + 1];
                    int df_f_hp = Math.Min(f_dam[i], api_nowhps_combined[i + 1]);
                    api_nowhps_combined[i + 1] -= df_f_hp;
                    //ダメコン処理
                    bool sank = oldhp > 0 && api_nowhps_combined[i + 1] <= 0;
                    if (sank && dameconflag[i].Flag > 0)
                    {
                        int id = APIPort.DeckPorts[Math.Min(DeckPortNumber - 1 + 1, APIPort.DeckPorts.Count - 1)].api_ship[i];
                        if (APIPort.ShipsDictionary.ContainsKey(id))
                        {
                            //HPの回復
                            switch (dameconflag[i].Flag)
                            {
                                case 1://応急修理要員
                                    api_nowhps_combined[i + 1] = (int)((double)api_maxhps_combined[i + 1] * (double)0.2);
                                    break;
                                case 2://応急修理女神
                                    api_nowhps_combined[i + 1] = api_maxhps_combined[i + 1];
                                    break;
                            }
                            //装備消費
                            ApiShip oship = APIPort.ShipsDictionary[id];
                            if (!dameconflag[i].ExSlot) oship.api_slot[dameconflag[i].OnSlot] = -1;
                            else oship.api_slot_ex = -1;
                            //燃料弾薬補給
                            if(dameconflag[i].Flag == 2)
                            {
                                oship.api_fuel = oship.DShip.api_fuel_max;
                                oship.api_bull = oship.DShip.api_bull_max;
                            }
                        }
                    }
                    //第2艦隊はスコアの計算
                    EscoreCombined += df_f_hp;
                }
                //敵
                if (api_nowhps[i + 7] != -1)
                {
                    Edamage[i] += e_dam[i];
                    int df_e_hp = Math.Min(e_dam[i], api_nowhps[i + 7]);
                    api_nowhps[i + 7] -= df_e_hp;
                    Fscore += df_e_hp;
                }
            }
            //与ダメ
            int[] f_ydam = db.GetFYdam();
            for(int i=0; i<FScoreArrayCombined.Length; i++)
            {
                FScoreArrayCombined[i] += f_ydam[i];
            }
        }

        //第2艦隊側のダメージ(砲撃戦)
        public void AddCombinedDamage(APIBattle.DamageHougeki hou)
        {
            AddDamage();
            AddCombinedDamage();
            hou.Calc();
            List<DameconHandler> dameconflag = HasDameconCombined();
            for (int i = 0; i < hou.Index.Length; i++)
            {
                int idx = hou.Index[i];
                int damage = hou.Damage[i];
                //味方のダメージの場合
                if (idx <= 6 && api_nowhps_combined[idx] != -1)
                {
                    int oldhp = api_nowhps_combined[idx];
                    FdamageCombined[idx - 1] += damage;
                    int df_f_hp = Math.Min(damage, api_nowhps_combined[idx]);//実際に削られたHP
                    api_nowhps_combined[idx] -= df_f_hp;
                    //ダメコン処理
                    bool sank = oldhp > 0 && api_nowhps_combined[idx] <= 0;
                    if (sank && dameconflag[idx-1].Flag > 0)
                    {
                        int id = APIPort.DeckPorts[Math.Min(DeckPortNumber - 1 + 1, APIPort.DeckPorts.Count - 1)].api_ship[idx - 1];
                        if (APIPort.ShipsDictionary.ContainsKey(id))
                        {
                            //HPの回復
                            switch (dameconflag[idx-1].Flag)
                            {
                                case 1://応急修理要員
                                    api_nowhps_combined[idx] = (int)((double)api_maxhps_combined[idx] * (double)0.2);
                                    break;
                                case 2://応急修理女神
                                    api_nowhps_combined[idx] = api_maxhps_combined[idx];
                                    break;
                            }
                            //装備消費
                            ApiShip oship = APIPort.ShipsDictionary[id];
                            if (!dameconflag[idx - 1].ExSlot) oship.api_slot[dameconflag[idx - 1].OnSlot] = -1;
                            else oship.api_slot_ex = -1;
                            //燃料弾薬補給
                            if (dameconflag[idx-1].Flag == 2)
                            {
                                oship.api_fuel = oship.DShip.api_fuel_max;
                                oship.api_bull = oship.DShip.api_bull_max;
                            }
                        }
                    }
                    //第2相手のスコア
                    EscoreCombined += df_f_hp;
                }
                //敵のダメージの場合
                else if (idx >= 7 && api_nowhps[idx] != -1)
                {
                    Edamage[idx - 7] += damage;
                    int df_e_hp = Math.Min(damage, api_nowhps[idx]);//実際に削られたHP
                    api_nowhps[idx] -= df_e_hp;
                    Fscore += df_e_hp;
                }
            }
            //与ダメ
            for (int i = 0; i < FScoreArrayCombined.Length; i++)
            {
                FScoreArrayCombined[i] += hou.FYdam[i];
            }
        }

        //MVPの取得
        public int GetMVPIndexCombined()
        {
            //全て0の場合
            bool allzero = FScoreArrayCombined[0] == 0 && FScoreArrayCombined[1] == 0 && FScoreArrayCombined[2] == 0 &&
                            FScoreArrayCombined[3] == 0 && FScoreArrayCombined[4] == 0 && FScoreArrayCombined[5] == 0;
            if (allzero) return 0;
            //誰かはダメージを与えている場合
            int maxscore = FScoreArrayCombined.Max();
            foreach(int i in Enumerable.Range(0, FScoreArrayCombined.Length))
            {
                if (FScoreArrayCombined[FScoreArrayCombined.Length - i - 1] == maxscore) return (FScoreArrayCombined.Length - i - 1);
            }
            //例外
            throw new IndexOutOfRangeException();
        }

        //表示用
        //絶対航空戦に参加しないので実装しなくて良い

        //ダメコンを持っているかどうか  : 0なし、1応急修理、2女神
        public List<DameconHandler> HasDameconCombined()
        {
            return APIPort.DeckPorts[Math.Min(DeckPortNumber - 1 + 1, APIPort.DeckPorts.Count - 1)].api_ship.Select(delegate(int shipid)
            {
                if (shipid == -1) return new DameconHandler();
                if (!APIPort.ShipsDictionary.ContainsKey(shipid)) return new DameconHandler();
                //slotitemid = 42 ダメコン、43　女神
                DameconHandler handler = new DameconHandler();

                //通常のスロット
                foreach (var i in Enumerable.Range(0, APIPort.ShipsDictionary[shipid].api_slot.Count))
                {
                    var x = APIPort.ShipsDictionary[shipid].api_slot[i];

                    SlotItem oslot;
                    if (APIGetMember.SlotItemsDictionary.TryGetValue(x, out oslot))
                    {
                        int masterid = oslot.api_slotitem_id;
                        if (masterid == 42)
                        {
                            handler.Flag = 1;
                            handler.OnSlot = i;
                        }
                        else if (masterid == 43)
                        {
                            handler.Flag = 2;
                            handler.OnSlot = i;
                        }
                    }
                }

                //拡張スロット
                var exslotid = APIPort.ShipsDictionary[shipid].api_slot_ex;
                SlotItem exslot;
                if (APIGetMember.SlotItemsDictionary.TryGetValue(exslotid, out exslot))
                {
                    int masterexid = exslot.api_slotitem_id;
                    if (masterexid == 42)
                    {
                        handler.Flag = 1;
                        handler.ExSlot = true;
                    }
                    else if (masterexid == 43)
                    {
                        handler.Flag = 2;
                        handler.ExSlot = true;
                    }
                }

                return handler;
            }).ToList();
        }

    }
    #endregion

    //形勢をストックするためのクラス
    #region 形勢
    public class BattleView
    {
        public BattleSituation Situation { get; set; }
        //味方のゲージ
        public int Fscore { get; set; }
        public int FscoreMax { get; set; }
        public double FscoreRatio { get; set; }
        //敵のゲージ
        public int Escore { get; set; }
        public int EscoreMax { get; set; }
        public double EscoreRatio { get; set; }
        //敵の第2艦隊相手のゲージ
        public int EscoreCombined { get; set; }
        public int EscoreMaxCombined { get; set; }
        public double EscoreRatioCombined { get; set; }
        //ゲージ比率
        public double GaugeRatio { get; set; }
        public string GaugeString { get; set; }
        public double GaugeRatioCombined { get; set; }
        public string GaugeStringCombined { get; set; }
        //連合艦隊かどうか
        public bool IsCombined { get; set; }
        //沈んでいるか
        public bool[] IsFriendSank { get; set; }
        public bool[] IsFriendCombinedSank { get; set; }
        public bool[] IsEnemySank { get; set; }
        //船の総数
        public int NumShipFriend { get; set; }
        public int NumShipEnemy { get; set; }
        public int NumShipFriendCombined { get; set; }
        //敵のid
        /// <summary>
        /// 2015/7/17のメンテで消えたんで使用しない
        /// </summary>
        [Obsolete]
        public int EnemyID { get; set; }
        public int EnemyLocalID { get; set; }
        public int EnemyLocalShortID { get; set; }
        public int EnemyPracticeID { get; set; }
        //敵のShipID
        public int[] EnemyShipID { get; set; }
        //戦闘評価
        public string WinRank { get; set; }
        public WinRankEnum WinRankEstimated { get; set; }
        //MAPID
        public int AreaID { get; set; }
        public int MapID { get; set; }
        public int CellID { get; set; }
        public int BossFlag { get; set; }

        //コンストラクタ
        public BattleView()
        {
            this.Situation = BattleSituation.None;
            this.WinRankEstimated = BattleView.WinRankEnum.Unknown;
        }
        
        //列挙体
        public enum WinRankEnum
        {
            Unknown, Perfect, S, A, B, C, D, E,
        }

        public BattleView(BattleInfo info, BattleView view)
            : this()
        {
            //インプット
            this.Fscore = info.Fscore; this.FscoreMax = info.FscoreMax;
            this.FscoreRatio = (double)this.Fscore / (double)this.FscoreMax;
            this.Escore = info.Escore; this.EscoreMax = info.EscoreMax;
            this.EscoreRatio = (double)this.Escore / (double)this.EscoreMax;            
            //定数など
            //this.EnemyID = view.EnemyID; 
            this.AreaID = view.AreaID;
            this.MapID = view.MapID; this.BossFlag = view.BossFlag;
            this.CellID = view.CellID;
            this.EnemyPracticeID = view.EnemyPracticeID;

            UserEnemyID uid = new UserEnemyID()
            {
                api_maparea_id = AreaID,
                api_mapinfo_no = MapID,
                api_cell_id = CellID,
                api_ship_ke = info.api_ship_ke.ToList(),
                api_formation_enemy = info.Formation[1],
            };
            this.EnemyLocalID = uid.MakeLongHashCode();
            this.EnemyLocalShortID = uid.MakeShortHashCode(this.EnemyLocalID);
            if (this.EnemyPracticeID != 0) this.EnemyLocalShortID = this.EnemyPracticeID;//演習時の特例

            //ゲージ比
            //敵のゲージが0の場合
            if(this.Escore == 0)
            {
                //無条件で勝つ場合
                if (this.Fscore > 0)
                {
                    this.GaugeRatio = -1;
                    this.GaugeString = "∞";
                }
                else
                {
                    this.GaugeRatio = 0;
                    this.GaugeString = "0";
                }
            }
            else
            {
                this.GaugeRatio = Math.Floor(this.FscoreRatio * 100) / Math.Floor(this.EscoreRatio * 100);
                this.GaugeString = (Math.Floor(this.GaugeRatio * 100) / 100).ToString("F2");
            }
            //敵：沈んでいるか
            IsEnemySank = new bool[info.api_nowhps.Length - 6];//6-6（Length=13）で7
            IsEnemySank[0] = false;
            for(int i=1; i<IsEnemySank.Length; i++)
            {
                IsEnemySank[i] = info.api_nowhps[i+6] == 0;
            }
            //味方：沈んでいるか
            IsFriendSank = new bool[Math.Min(7, info.api_nowhps.Length)];
            IsFriendSank[0] = false;
            for (int i = 1; i < IsFriendSank.Length; i++ )
            {
                IsFriendSank[i] = info.api_nowhps[i] == 0;
            }
            //船の数
            NumShipFriend = Enumerable.Range(1, 6).Select(x => info.api_maxhps[x] > 0 ? 1 : 0).Sum();
            NumShipEnemy = Enumerable.Range(7, IsEnemySank.Length - 1).Select(x => info.api_maxhps[x] > 0 ? 1 : 0).Sum();

            //--連合艦隊
            this.IsCombined = info is BattleCombinedInfo;
            if (!IsCombined)
            {
                this.WinRankEstimated = EstimateWinRank();
                return;
            }

            BattleCombinedInfo cinfo = info as BattleCombinedInfo;
            //インプット
            this.EscoreCombined = cinfo.EscoreCombined; this.EscoreMaxCombined = cinfo.EscoreMaxCombined;
            this.EscoreRatioCombined = (double)this.EscoreCombined / (double)this.EscoreMaxCombined;
            //ゲージ比率
            if(this.EscoreMaxCombined == 0)
            {
                if(this.Fscore > 0)
                {
                    this.GaugeRatioCombined = -1;
                    this.GaugeStringCombined = "∞";
                }
                else
                {
                    this.GaugeRatioCombined = 0;
                    this.GaugeStringCombined = "0";
                }
            }
            else
            {
                this.GaugeRatioCombined = Math.Floor(this.FscoreRatio * 100) / Math.Floor(this.EscoreRatioCombined * 100);
                this.GaugeStringCombined = (Math.Floor(this.GaugeRatioCombined * 100) / 100).ToString("F2");
            }
            //味方連合：沈んでいるか
            IsFriendCombinedSank = new bool[cinfo.api_nowhps_combined.Length];
            IsFriendCombinedSank[0] = false;
            for(int i=0; i<IsFriendCombinedSank.Length; i++)
            {
                IsFriendCombinedSank[i] = cinfo.api_nowhps_combined[i] == 0;
            }
            //連合艦隊の船の数
            NumShipFriendCombined = Enumerable.Range(0, cinfo.api_maxhps_combined.Length).Select(x => cinfo.api_maxhps_combined[x] > 0 ? 1 : 0).Sum();
            //ゲージ判定
            this.WinRankEstimated = EstimateWinRank();
        }

        //戦闘結果の推定
        public WinRankEnum EstimateWinRank()
        {
            //敵、味方のスコア
            int enemy = this.Escore; int friend = this.Fscore;
            int maxenemy = this.EscoreMax; int maxfriend = this.FscoreMax;
            //連合艦隊の場合（＋1の倍率でとりあえず加算）
            if(IsCombined)
            {
                enemy += this.EscoreCombined;
                maxenemy += this.EscoreMaxCombined;
            }

            //--航海日誌拡張版からのコピペ
            //轟沈・撃沈数
            int friendSunk = IsFriendSank.Where(x => x).Select(x => 1).Sum();
            if (IsCombined && IsFriendCombinedSank != null) friendSunk += IsFriendCombinedSank.Where(x => x).Select(x => 1).Sum();
            int enemySunk = IsEnemySank.Where(x => x).Select(x => 1).Sum();

            double[] damageRate = new double[]
            {
                (double)friend / (double)maxfriend,
                (double)enemy / (double)maxenemy,
            };

            double friendGaugeRate = Math.Floor(damageRate[0] * 100);
            double enemyGaugeRate = Math.Floor(damageRate[1] * 100);
            bool equalOrMore = friendGaugeRate > (0.9 * enemyGaugeRate);
            bool superior = (friendGaugeRate > 0) && (friendGaugeRate > 2.5 * enemyGaugeRate);

            //生き残っている船
            int enemyNowShips = this.NumShipEnemy - enemySunk;
            //敵旗艦轟沈させたか
            bool isEnemyFlagshipSank = IsEnemySank[1];

            //追加（空襲戦）
            if(this.Situation == BattleSituation.CombinedLdAirBattle || this.Situation == BattleSituation.LdAirBattle)
            {
                //1%未満なら完全勝利
                if (enemyGaugeRate < 1) return WinRankEnum.Perfect;
                //1%～10%ならA勝利
                else if (enemyGaugeRate < 10) return WinRankEnum.A;
                //11%～20％ならB勝利
                else if (enemyGaugeRate < 20) return WinRankEnum.B;
                //21%～50%ならC敗北
                else if (enemyGaugeRate < 50) return WinRankEnum.C;
                //51%～80%ならD敗北
                else if (enemyGaugeRate < 80) return WinRankEnum.D;
                //それ以外ならE敗北
                else return WinRankEnum.E;
            }

            if (friendSunk == 0)
            { // 味方轟沈数ゼロ
                if (enemyNowShips == 0)
                { // 敵を殲滅した
                    if (enemy == 0)
                    { // 味方ダメージゼロ
                        return WinRankEnum.Perfect;
                    }
                    return WinRankEnum.S;
                }
                else
                {
                    // 6隻の場合のみ4隻以上撃沈？
                    if (this.NumShipEnemy == 6)
                    {
                        if (enemySunk >= 4)
                        {
                            return WinRankEnum.A;
                        }
                    }
                    // 半数以上撃沈？
                    else if ((enemySunk * 2) >= this.NumShipEnemy)
                    {
                        return WinRankEnum.A;
                    }
                    // 敵旗艦を撃沈
                    if (isEnemyFlagshipSank)
                    {
                        return WinRankEnum.B;
                    }
                    // 戦果ゲージが2.5倍以上
                    if (superior)
                    {
                        return WinRankEnum.B;
                    }
                }
            }
            else
            {
                // 敵を殲滅した
                if (enemyNowShips == 0)
                {
                    return WinRankEnum.B;
                }
                // 敵旗艦を撃沈 and 味方轟沈数 < 敵撃沈数
                if ((isEnemyFlagshipSank) && (friendSunk < enemySunk))
                {
                    return WinRankEnum.B;
                }
                // 戦果ゲージが2.5倍以上
                if (superior)
                {
                    return WinRankEnum.B;
                }
                // 敵旗艦を撃沈
                // TODO: 味方の轟沈艦が２隻以上ある場合、敵旗艦を撃沈してもDになる場合がある
                if (isEnemyFlagshipSank)
                {
                    return WinRankEnum.C;
                }
            }
            // 敵に与えたダメージが一定以上 and 戦果ゲージが1.0倍以上
            if (friend > 0)
            {
                if (equalOrMore)
                {
                    return WinRankEnum.C;
                }
            }
            // 轟沈艦があり かつ 残った艦が１隻のみ
            if ((friendSunk > 0) && ((this.NumShipFriend - friendSunk) == 1))
            {
                return WinRankEnum.E;
            }
            // 残りはD
            return WinRankEnum.D;
        }
    }
    #endregion

    //戦闘状態の列挙体
    #region 戦闘状態
    public enum BattleSituation
    {
        None,
        BeforeBattle,
        Battle,
        NightBattleChase,
        NightBattleSP,
        EndBattle,
        Practice,
        PracticeNightBattle,
        CombinedBattle,
        CombinedBattleWater,
        CombinedNightBattleChase,
        CombinedNightBattleSP,
        EndCombinedBattle,
        BattleAir,
        CombinedBattleAir,
        LdAirBattle,
        CombinedLdAirBattle,
    }

    public static class BattleEnumExt
    {
        public static string ToStr(this BattleSituation s)
        {
            switch(s)
            {
                case BattleSituation.BeforeBattle: return "交戦見込";
                case BattleSituation.Battle: return "交戦中";
                case BattleSituation.NightBattleChase: return "追撃夜戦";
                case BattleSituation.NightBattleSP: return "開幕夜戦";
                case BattleSituation.EndBattle: return "戦闘終了";
                case BattleSituation.Practice: return "演習中";
                case BattleSituation.PracticeNightBattle: return "演習夜戦";
                case BattleSituation.CombinedBattle: return "(連)機動戦";
                case BattleSituation.CombinedBattleWater: return "(連)水上戦";
                case BattleSituation.CombinedNightBattleChase: return "(連)追撃夜戦";
                case BattleSituation.CombinedNightBattleSP: return "(連)開幕夜戦";
                case BattleSituation.EndCombinedBattle: return "(連)戦闘終了";
                case BattleSituation.BattleAir: return "航空戦";
                case BattleSituation.CombinedBattleAir: return "(連)航空戦";
                case BattleSituation.LdAirBattle: return "空襲戦";
                case BattleSituation.CombinedLdAirBattle: return "(連)空襲戦";
                default: return "";
            }
        }

        public static string ToStr(this BattleView.WinRankEnum e)
        {
            switch(e)
            {
                case BattleView.WinRankEnum.Unknown: return "?";
                case BattleView.WinRankEnum.Perfect: return "SS";
                case BattleView.WinRankEnum.S: return "S";
                case BattleView.WinRankEnum.A: return "A";
                case BattleView.WinRankEnum.B: return "B";
                case BattleView.WinRankEnum.C: return "C";
                case BattleView.WinRankEnum.D: return "D";
                case BattleView.WinRankEnum.E: return "E";
                default: return "";
            }
        }

        static System.Drawing.Color winColor = System.Drawing.Color.FromArgb(255, 225, 223);
        static System.Drawing.Color loseColor = System.Drawing.Color.FromArgb(223, 234, 225);
        public static System.Drawing.Color GetBackColor(this BattleView.WinRankEnum e)
        {
            switch(e)
            {
                case BattleView.WinRankEnum.Unknown: return System.Drawing.Color.White;
                case BattleView.WinRankEnum.Perfect: return winColor;
                case BattleView.WinRankEnum.S: return winColor;
                case BattleView.WinRankEnum.A: return winColor;
                case BattleView.WinRankEnum.B: return winColor;
                case BattleView.WinRankEnum.C: return loseColor;
                case BattleView.WinRankEnum.D: return loseColor;
                case BattleView.WinRankEnum.E: return loseColor;
                default: throw new ArgumentException();
            }
        }
    }
    #endregion

    //護衛退避
    public class ApiEscape
    {
        public List<int> api_escape_idx { get; set; }
        public List<int> api_tow_idx { get; set; }
    }
}
