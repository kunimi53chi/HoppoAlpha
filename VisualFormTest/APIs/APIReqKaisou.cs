using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqKaisou
    {
        public static bool OnRemodeling { get; set; }

        public static void ReadPowerup(string json, string requestbody)
        {
            var ojson = DynamicJson.Parse(json);
            //近代化成功時
            if(ojson.api_data.api_powerup_flag == 1)
            {
                //パワーアップした船のステータスの書き換え
                ApiShip ship = ojson.api_data.api_ship.Deserialize<ApiShip>();
                APIPort.ReplaceShip(ship);
            }
            //艦隊情報のリセット
            List<ApiDeckPort> decks = ojson.api_data.api_deck.Deserialize<List<ApiDeckPort>>();
            APIPort.DeckPorts = decks;
            //近代化の餌になった船の削除
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fid%5Fitems=([0-9%2C]+)");
            string itemids = match.Groups[1].Value;//37807%2C37806　こんな文字列
            string[] strids = itemids.Replace("%2C", ",").Split(',');
            //餌の船の抽出
            foreach(string s in strids)
            {
                int shipid = Convert.ToInt32(s);
                ApiShip item = APIPort.ShipsDictionary[shipid];
                //削除
                APIPort.RemoveShip(item);
            }
        }

        //lock 装備のロック
        public static void ReadLock(string requestbody, string json)
        {
            //アイテムのID
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fslotitem%5Fid=([0-9]+)");
            int slotitemid = Convert.ToInt32(match.Groups[1].Value);
            //Lockのフラグ
            var ojson = DynamicJson.Parse(json);
            int locked = (int)ojson.api_data.api_locked;
            //装備アイテムの置き換え
            if (APIGetMember.SlotItemsDictionary.ContainsKey(slotitemid))
            {
                APIGetMember.SlotItemsDictionary[slotitemid].api_locked = locked;
            }
        }

        //slot_exchange_index : D&Dで装備の交換
        public static void ReadSlotExchangeIndex(string requestbody, string json)
        {
            //艦のID
            var match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fid=([0-9]+)");
            int shipid = Convert.ToInt32(match.Groups[1].Value);
            //スロットのリストを取得
            var ojson = DynamicJson.Parse(json).api_data;
            List<int> slots = (List<int>)ojson.api_slot;
            //船のオブジェクトの変更
            if(APIPort.ShipsDictionary.ContainsKey(shipid))
            {
                APIPort.ShipsDictionary[shipid].api_slot = slots;
            }
        }

        //remodeling : 改装開始
        public static void ReadRemodeling(string requestbody)
        {
            //改造する艦のID
            var match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fid=([0-9]+)");
            int shipid = Convert.ToInt32(match.Groups[1].Value);
            //改装開始
            OnRemodeling = true;
            DefaultSlotitemDataBase.StartRemodeling(shipid);
        }
    }
}
