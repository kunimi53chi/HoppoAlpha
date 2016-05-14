using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiReqQuest;
using HoppoAlpha.DataLibrary.RawApi.ApiReqPractice;
using HoppoAlpha.DataLibrary.DataObject;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIGetMember
    {
        public static List<Furniture> Furnitures { get; set; }
        public static Dictionary<int, SlotItem> SlotItemsDictionary { get; set; }
        public static List<Useitem> Useitems { get; set; }
        public static List<Kdock> Kdocks { get; set; }
        public static Unsetslot Unsetslots { get; set; }
        public static List<ApiMapInfo> MapInfo { get; set; }
        public static List<BaseAirCorp> BaseAirCorps { get; set; }

        //プリセットデータ
        public static List<PresetDeck> Preset { get; set; }
        //選択したプリセット
        public static int SelectedPresetNo { get; set; }


        //basicからのアップデート
        public static void ReadBasic(string json)
        {
            //JSONオブジェクトの作成
            var ojson = DynamicJson.Parse(json);
            //Basicの読み込み
            Basic basic = ojson.api_data.Deserialize<Basic>();
            //置き換え
            APIPort.SetBasic(basic);
        }

        //furnitureからのアップデート
        public static void ReadFurniture(string json)
        {
            //JSONオブジェクトの作成
            var ojson = DynamicJson.Parse(json);
            //Furnitureの読み込み
            Furnitures = ojson.api_data.Deserialize<List<Furniture>>();
        }

        //slot_itemからのアップデート
        public static void ReadSlotItem(string json)
        {
            //JSONオブジェクトの作成
            var ojson = DynamicJson.Parse(json);
            //slot_itemの読み込み
            List<SlotItem> slotitem = ojson.api_data.Deserialize<List<SlotItem>>();
            SlotItemsDictionary = new Dictionary<int, SlotItem>();
            foreach (SlotItem s in slotitem)
            {
                SlotItemsDictionary[s.api_id] = s;
            }
            //改装中に呼ばれたならば
            if (APIReqKaisou.OnRemodeling)
            {
                DefaultSlotitemDataBase.EndRemodeling_Slotitem();
                APIReqKaisou.OnRemodeling = false;
            }
        }

        //useitemからのアップデート
        public static void ReadUseitem(string json)
        {
            //JSONオブジェクトの作成
            var ojson = DynamicJson.Parse(json);
            //useitemの読み込み
            Useitems = ojson.api_data.Deserialize<List<Useitem>>();
        }

        //kdockからのアップデート
        public static void ReadKdock(string json)
        {
            var ojson = DynamicJson.Parse(json);
            Kdocks = ojson.api_data.Deserialize<List<Kdock>>();
        }

        //unsetslotからのアップデート
        public static void ReadUnsetslot(string json)
        {
            //二重配列化
            var ojson = DynamicJson.Parse(json).api_data;
            string reg_str = System.Text.RegularExpressions.Regex.Replace(ojson.ToString().Replace("\"", ""), @"api_slottype[0-9]{1,2}:", "");
            string str = reg_str.Replace("{", "[").Replace("}", "]").Replace("-1", "[]");
            str = "{\"slottype\":" + str + "}";
            var rjson = DynamicJson.Parse(str);
            Unsetslots = rjson.Deserialize<Unsetslot>();
            //出撃中のチェックを外す
            if (APIPort.OnSortie) APIPort.OnSortie = false;
            //母港の差分チェック
            DefaultSlotitemDataBase.AfterReturnProcess();
        }

        //deckのアップデート
        public static void ReadDeck(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            List<ApiDeckPort> decks = ojson.Deserialize<List<ApiDeckPort>>();
            APIPort.DeckPorts = decks;
        }

        //ndockのアップデート
        public static void ReadNdock(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            List<ApiNdock> ndock = ojson.Deserialize<List<ApiNdock>>();
            APIPort.SetNdock(ndock);

            foreach (var n in ndock)
            {
                //入渠開始したことを通知させる
                ApiShip oship;
                if (APIPort.ShipsDictionary.TryGetValue(n.api_ship_id, out oship)) oship.RaiseConditionChanged();
            }
        }

        //ship2のアップデート
        public static void ReadShip2(string json)
        {
            var ojson = DynamicJson.Parse(json);
            List<ApiShip> ships = ojson.api_data.Deserialize<List<ApiShip>>();
            List<ApiDeckPort> decks = ojson.api_data_deck.Deserialize<List<ApiDeckPort>>();
            Dictionary<int, ApiShip> newdic = new Dictionary<int, ApiShip>();
            foreach (ApiShip s in ships)
            {
                newdic[s.api_id] = s;
            }
            APIPort.ShipsDictionary = newdic;
            APIPort.DeckPorts = decks;
        }

        //ship3のアップデート
        public static void ReadShip3(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            Ship3Partial ship3 = ojson.Deserialize<Ship3Partial>();
            //ApiShipの置き換え
            foreach (ApiShip s in ship3.api_ship_data) APIPort.ReplaceShip(s);
            //APIDeckPortの置き換え
            APIPort.DeckPorts = ship3.api_deck_data;
            //Unsetslotの置き換え
            string reg_str = System.Text.RegularExpressions.Regex.Replace(ojson.api_slot_data.ToString().Replace("\"", ""), @"api_slottype[0-9]{1,2}:", "");
            string str = reg_str.Replace("{", "[").Replace("}", "]").Replace("-1", "[]");
            str = "{\"slottype\":" + str + "}";
            var rjson = DynamicJson.Parse(str);
            Unsetslots = rjson.Deserialize<Unsetslot>();
            //改装中に呼ばれたならば
            if (APIReqKaisou.OnRemodeling) DefaultSlotitemDataBase.MiddleRemodeling_Ship3();
        }

        //require_infoのアップデート
        public static void ReadReuireInfo(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            RequireInfo requireinfo = ojson.Deserialize<RequireInfo>();

            //--Basic
            if (APIPort.Basic == null) APIPort.Basic = new Basic();
            APIPort.Basic.api_member_id = requireinfo.api_basic.api_member_id;
            APIPort.Basic.api_firstflag = requireinfo.api_basic.api_firstflag;
            //APIPort.SetBasic(requireinfo.api_basic);

            //--SlotItem
            SlotItemsDictionary = new Dictionary<int, SlotItem>();
            foreach (SlotItem s in requireinfo.api_slot_item)
            {
                SlotItemsDictionary[s.api_id] = s;
            }
            //改装中に呼ばれたならば
            if (APIReqKaisou.OnRemodeling)
            {
                DefaultSlotitemDataBase.EndRemodeling_Slotitem();
                APIReqKaisou.OnRemodeling = false;
            }

            //--UnsetSlot
            if (ojson.IsDefined("api_unsetslot"))
            {
                string reg_str = System.Text.RegularExpressions.Regex.Replace(ojson.api_unsetslot.ToString().Replace("\"", ""), @"api_slottype[0-9]{1,2}:", "");
                string str = reg_str.Replace("{", "[").Replace("}", "]").Replace("-1", "[]");
                str = "{\"slottype\":" + str + "}";
                var rjson = DynamicJson.Parse(str);
                Unsetslots = rjson.Deserialize<Unsetslot>();
                //出撃中のチェックを外す
                if (APIPort.OnSortie) APIPort.OnSortie = false;
                //母港の差分チェック
                DefaultSlotitemDataBase.AfterReturnProcess();
            }

            //-Kdock
            Kdocks = requireinfo.api_kdock;

            //-Useitem
            Useitems = requireinfo.api_useitem;

            //--Furniture
            Furnitures = requireinfo.api_furniture;
        }

        //questlistのアップデート
        public static void ReadQuestlist(string json)
        {
            var ojson = DynamicJson.Parse(json.Replace("\"api_get_material\":-1", "\"api_get_material\":[0,0,0,0]").Replace
                (",-1", "")).api_data.api_list;
            if (ojson == null) return;
            //日またぎチェック
            APIReqQuest.CheckDaysOver();
            List<ApiQuest> raw_quest = ojson.Deserialize<List<ApiQuest>>();
            if (APIReqQuest.Quests == null) APIReqQuest.Quests = new SortedDictionary<int, ApiQuest>();
            foreach (ApiQuest q in raw_quest)
            {
                //受注していれば追加
                if (q.api_state >= 2) APIReqQuest.Quests[q.api_no] = q;
                //中止なりで外れれば削除
                else APIReqQuest.Quests.Remove(q.api_no);
            }
            //時間の記録
            APIReqQuest.LastUpdated = DateTime.Now;
        }

        //practice 演習リスト
        public static void ReadPractice(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            List<ApiPractice> prac = ojson.Deserialize<List<ApiPractice>>();
            APIReqPractice.Practice = prac;
            APIReqPractice.LastUpdateTime = DateTime.Now;
        }

        //material 資源
        public static void ReadMaterial(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            List<ApiMaterial> material = ojson.Deserialize<List<ApiMaterial>>();
            APIPort.Materials = material;
        }

        //mapinfo :　マップの情報
        public static void ReadMapInfo(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            MapInfo = ojson.Deserialize<List<ApiMapInfo>>();
        }

        //ship_deck : 2015/5/18追加
        public static void ReadShipDeck(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            ApiShipDeck api = ojson.Deserialize<ApiShipDeck>();
            //艦娘データの更新
            foreach (var x in api.api_ship_data)
            {
                if (!APIPort.ShipsDictionary.ContainsKey(x.api_id)) continue;
                //オブジェクトの書き換え
                APIPort.ShipsDictionary[x.api_id] = x;
            }
            //艦隊所属艦のバッファリング
            List<int[]> tempfleet = new List<int[]>();
            foreach (var x in APIPort.DeckPorts)
            {
                tempfleet.Add(x.api_ship.Where(k => k != -1).ToArray());
            }
            //デッキのデータの更新
            List<int> sankship = new List<int>();//轟沈艦
            foreach (var x in api.api_deck_data)
            {
                //オブジェクトの取得
                if (x.api_id > APIPort.DeckPorts.Count) continue;
                APIPort.DeckPorts[x.api_id - 1] = x;
                //轟沈艦の取得
                if (x.api_id > tempfleet.Count) continue;
                int[] lastfleet = tempfleet[x.api_id - 1];
                sankship.AddRange(lastfleet.Except(x.api_ship.Where(k => k != -1)));
            }
            //轟沈艦の取得
            foreach (var x in sankship)
            {
                APIPort.ShipsDictionary.Remove(x);
            }
        }

        //preset_deck : プリセット情報の編成画面時の取得
        public static void ReadPresetDeck(string json)
        {
            var replace = System.Text.RegularExpressions.Regex.Replace(json, "(\")([0-9]+\":)", "$1_$2");

            var ojson = DynamicJson.Parse(replace);
            Preset = new List<PresetDeck>();
            foreach (var p in ojson.api_data.api_deck.GetDynamicMemberNames())
            {
                Preset.Add(new PresetDeck()
                    {
                        api_preset_no = (int)ojson.api_data.api_deck[p].api_preset_no,
                        api_name = ojson.api_data.api_deck[p].api_name,
                        api_name_id = ojson.api_data.api_deck[p].api_name_id,
                        api_ship = ((double[])ojson.api_data.api_deck[p].api_ship).Select(x => (int)x).ToList(),
                    });
            }
        }

        //base_air_corps : 基地航空隊の情報
        public static void ReadBaseAirCorps(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            BaseAirCorps = ojson.Deserialize<List<BaseAirCorp>>();
        }


        //装備の追加
        public static void AddSlotItem(SlotItem item)
        {
            //追加される場合→装備開発、艦ゲット（ドロップor建造）、マップクリア報酬orランカー報酬
            //SlotItemsの追加
            SlotItemsDictionary[item.api_id] = item;
        }

        //Unsetslotの編集
        public static void SetUnsetslot(List<int> unsetlist, int index)
        {
            Unsetslots.slottype[index] = unsetlist;
        }

        //装備の削除
        public static void RemoveSlotItem(SlotItem item)
        {
            //SlotItemの削除
            SlotItemsDictionary.Remove(item.api_id);
            //アイテムデータ取得
            ExMasterSlotitem ditem = APIMaster.MstSlotitems[item.api_slotitem_id];
            //装備種類のNo
            int typeid = ditem.EquipType;
            //Unsetslotの削除
            Unsetslots.slottype[typeid - 1].Remove(item.api_id);
        }

        //SlotItemの書き換え
        public static void RewriteSlotItem(SlotItem item)
        {
            SlotItemsDictionary[item.api_id] = item;
        }

        //Slotitemの取得
        public static SlotItem GetSlotitem(int slotitemid)
        {
            if (!SlotItemsDictionary.ContainsKey(slotitemid)) return null;
            else return SlotItemsDictionary[slotitemid];
        }

        //戦闘中の装備数の取得
        public static int GetSlotitemNumOnSortie()
        {
            int nowslotitem = 0;
            foreach (var s in APIPort.ShipsDictionary)
            {
                //装備中のスロット
                foreach (var slot in s.Value.api_slot)
                {
                    if (slot > 0) nowslotitem++;
                }
                //拡張スロット
                if (s.Value.api_slot_ex > 0) nowslotitem++;
            }

            //未装備の装備
            nowslotitem += APIGetMember.Unsetslots.slottype.Select(x => x.Count).Sum();

            //基地航空隊
            if (APIGetMember.BaseAirCorps != null) nowslotitem += APIGetMember.BaseAirCorps.SelectMany(x => x.api_plane_info).Where(x => x.api_slotid > 0).Count();


            return nowslotitem;
        }

        public class Unsetslot
        {
            public List<List<int>> slottype { get; set; }
        }

        public class Ship3Partial
        {
            //api_slot_data以外
            public List<ApiShip> api_ship_data { get; set; }
            public List<ApiDeckPort> api_deck_data { get; set; }
        }
    }
}
