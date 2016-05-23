using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;
using HoppoAlpha.DataLibrary.RawApi.ApiReqAirCorps;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;

namespace VisualFormTest
{
    public static class ApiReqAirCorps
    {
        //api_req_aircorps/set_plane : 交換や配置
        public static void ReadSetPlane(string requestBody, string responseBody)
        {
            if (APIGetMember.BaseAirCorps == null) return;
            //航空隊IDを取得
            var match = System.Text.RegularExpressions.Regex.Match(requestBody, @"api%5Fbase%5Fid=([0-9])");
            var airbase_id_str = match.Groups[1].Value;
            int airbase_id;
            if (!int.TryParse(airbase_id_str, out airbase_id)) return;
            //JSONパース
            var ojson = DynamicJson.Parse(responseBody).api_data;
            SetPlane setPlane = ojson.Deserialize<SetPlane>();
            //データの書き換え
            var airbase = APIGetMember.BaseAirCorps.Where(x => x.api_rid == airbase_id).FirstOrDefault();
            if (airbase == null) return;

            airbase.api_distance = setPlane.api_distance;
            var toUnsetSlotid = new List<int>();
            var removeUnsetSlotid = new List<int>();
            foreach(var pinfo in setPlane.api_plane_info)
            {
                if(pinfo.api_squadron_id <= airbase.api_plane_info.Count)
                {
                    var oldid = airbase.api_plane_info[pinfo.api_squadron_id - 1].api_slotid;
                    if (oldid > 0) toUnsetSlotid.Add(oldid);//あとでUnsetslotに追加
                    airbase.api_plane_info[pinfo.api_squadron_id - 1] = pinfo;

                    if (pinfo.api_slotid > 0) removeUnsetSlotid.Add(pinfo.api_slotid);//あとでUnsetslotから削除
                }
            }
            //ボーキの置き換え
            if (APIPort.Materials == null) return;
            var bauxite = APIPort.Materials.Where(x => x.api_id == 4).FirstOrDefault();//ボーキ
            if (bauxite != null) bauxite.api_value = setPlane.api_after_bauxite;
            //Unsetslotに追加
            foreach(var id in toUnsetSlotid)
            {
                SlotItem slot;
                if(APIGetMember.SlotItemsDictionary.TryGetValue(id, out slot))
                {
                    var mstslot = APIMaster.MstSlotitems.Where(x => x.Key == slot.api_slotitem_id).Select(x => x.Value).FirstOrDefault();
                    if(mstslot != null && mstslot.api_type != null)
                    {
                        //Unsetslotに追加
                        APIGetMember.Unsetslots.slottype[mstslot.api_type[2]].Add(id);
                    }
                }
            }
            //Unsetslotから削除
            foreach(var id in removeUnsetSlotid)
            {
                SlotItem slot;
                if(APIGetMember.SlotItemsDictionary.TryGetValue(id, out slot))
                {
                    var mstslot = APIMaster.MstSlotitems.Where(x => x.Key == slot.api_slotitem_id).Select(x => x.Value).FirstOrDefault();
                    if(mstslot != null && mstslot.api_type != null)
                    {
                        if(APIGetMember.Unsetslots.slottype[mstslot.api_type[2]].Contains(id))
                        {
                            APIGetMember.Unsetslots.slottype[mstslot.api_type[2]].Remove(id);
                        }
                    }
                }
            }
        }

        public static void ReadSupply(string requestBody, string responseBody)
        {
            if (APIGetMember.BaseAirCorps == null) return;
            //航空隊IDを取得
            var match = System.Text.RegularExpressions.Regex.Match(requestBody, @"api%5Fbase%5Fid=([0-9])");
            var airbase_id_str = match.Groups[1].Value;
            int airbase_id;
            if (!int.TryParse(airbase_id_str, out airbase_id)) return;
            //JSONパース
            var ojson = DynamicJson.Parse(responseBody).api_data;
            Supply supply = ojson.Deserialize<Supply>();
            //データの置き換え
            var airbase = APIGetMember.BaseAirCorps.Where(x => x.api_rid == airbase_id).FirstOrDefault();
            if (airbase == null) return;

            foreach(var pinfo in supply.api_plane_info)
            {
                if(pinfo.api_squadron_id <= airbase.api_plane_info.Count)
                {
                    airbase.api_plane_info[pinfo.api_squadron_id - 1] = pinfo;
                }
            }
            //ボーキと燃料の置き換え
            if (APIPort.Materials == null) return;
            var fuel = APIPort.Materials.Where(x => x.api_id == 1).FirstOrDefault();//燃料
            if (fuel != null && fuel.api_value != 0) fuel.api_value = supply.api_after_fuel;
            var bauxite = APIPort.Materials.Where(x => x.api_id == 4).FirstOrDefault();//ボーキ
            if (bauxite != null && bauxite.api_value != 0) bauxite.api_value = supply.api_after_bauxite;
        }
    }
}
