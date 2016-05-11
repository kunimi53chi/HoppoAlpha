using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiReqHokyu;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqHokyu
    {
        public static void ReadCharge(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            ApiCharge charge = ojson.Deserialize<ApiCharge>();
            //補給する船を探す
            foreach(ApiCharge.ApiChargeShip x in charge.api_ship)
            {
                ApiShip ship = APIPort.ShipsDictionary[x.api_id];
                //ステータスの変更
                ship.api_fuel = x.api_fuel;
                ship.api_bull = x.api_bull;
                ship.api_onslot = x.api_onslot;
            }
            //最初に補給した船のID
            int firstid = -1;
            if (charge.api_ship.Count > 0) firstid = charge.api_ship[0].api_id;
            //出撃報告書にセット（前）
            SortieReportDataBase.SetSupply(false, firstid);
            //資源の更新
            APIPort.SetMaterial(charge.api_material.ToArray());
            //出撃報告書にセット（後）
            SortieReportDataBase.SetSupply(true, firstid);
        }
    }
}
