using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiReqKousyou;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqKousyou
    {
        //createitem
        public static void ReadCreateitem(string json)
        {
            var ojson = DynamicJson.Parse(json);
            //アイテムの開発成功時
            if(ojson.api_data.api_create_flag == 1)
            {
                SlotItem item = new SlotItem();
                item.api_id = (int)ojson.api_data.api_slot_item.api_id;
                item.api_slotitem_id = (int)ojson.api_data.api_slot_item.api_slotitem_id;
                item.api_locked = 0;
                //Unsetslot部分の取得
                int idx = (int)ojson.api_data.api_type3 - 1;
                List<int> val = ojson.api_data.api_unsetslot.Deserialize<List<int>>();
                //アイテムの追加
                APIGetMember.AddSlotItem(item);
                //Unsetslot部分にも追加
                APIGetMember.SetUnsetslot(val, idx);
            }
            //資材
            int[] material = ojson.api_data.api_material.Deserialize<int[]>();
            APIPort.SetMaterial(material);
        }

        //destroyitem2 : RequestBodyに注意
        public static void ReadDestroyItem2(string requestbody, string json)
        {
            //slotitem_idの抽出
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"slotitem%5Fids=([0-9%2C]+)");
            string itemids = match.Groups[1].Value;//56975%2C56822　こんな文字列
            string[] strids = itemids.Replace("%2C", ",").Split(',');
            //SlotItemの抽出（複数廃棄にも対応）
            foreach (string s in strids)
            {
                int slotitem_id = Convert.ToInt32(s);
                SlotItem item = APIGetMember.SlotItemsDictionary[slotitem_id];
                APIGetMember.RemoveSlotItem(item);
            }
            //資源の抽出
            int[] resource = DynamicJson.Parse(json).api_data.api_get_material.Deserialize<int[]>();
            APIPort.AddMaterial(resource);
        }

        //GetShip
        public static void ReadGetShip(string json)
        {
            var ojson = DynamicJson.Parse(json);
            ApiShip ship = ojson.api_data.api_ship.Deserialize<ApiShip>();
            List<ApiSlotitem> sitem;
            if (ojson.api_data.IsDefined("api_slotitem") && ojson.api_data.api_slotitem != null)
            {
                sitem = ojson.api_data.api_slotitem.Deserialize<List<ApiSlotitem>>();
            }
            else
            {
                sitem = new List<ApiSlotitem>();
            }
            //デフォルト装備DBへの記録
            List<int> sid = sitem.Select(x => x.api_slotitem_id).Concat(Enumerable.Repeat(-1, 5)).Take(5).ToList();//サイズ数5のリストへ
            DefaultSlotitemDataBase.AddCollection(ship.api_ship_id, sid);

            APIPort.AddShip(ship, sitem);
        }

        //destroyship
        public static void ReadDestroyship(string requestbody, string json)
        {
            //ship_idの抽出
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fship%5Fid=([0-9]+)");
            int ship_id = Convert.ToInt32(match.Groups[1].Value);
            //Shipの抽出
            ApiShip ship = APIPort.ShipsDictionary[ship_id];
            //Shipの削除
            APIPort.RemoveShip(ship);
            //資源
            int[] material = DynamicJson.Parse(json).api_data.api_material.Deserialize<int[]>();
            APIPort.SetMaterial(material);
        }

        //remodel_slot
        public static void ReadRemodelSlot(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            //成功失敗問わずあるもの
            //資源
            int[] material = (int[])ojson.api_after_material;
            APIPort.SetMaterial(material);
            //あるかどうかわからないもの
            //api_after_slot : 成功時のみ出現
            if(ojson.IsDefined("api_after_slot"))
            {
                SlotItem s = ojson.api_after_slot.Deserialize<SlotItem>();
                APIGetMember.RewriteSlotItem(s);
            }
            //api_use_slot_id : 高レベルで出現
            if(ojson.IsDefined("api_use_slot_id"))
            {
                List<int> useitem = ojson.api_use_slot_id.Deserialize<List<int>>();
                foreach(int x in useitem)
                {
                    //装備の削除
                    APIGetMember.RemoveSlotItem(APIGetMember.SlotItemsDictionary[x]);
                }
            }
        }
    }
}
