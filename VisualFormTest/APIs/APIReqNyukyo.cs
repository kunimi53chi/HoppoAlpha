using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;

namespace VisualFormTest
{
    static class APIReqNyukyo
    {
        //api_req_nyukyo/start
        public static void ReadStart(string requestbody)
        {
            //船のID
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fship%5Fid=([0-9]+)");
            int ship_id = Convert.ToInt32(match.Groups[1].Value);
            //入渠ドックのID
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fndock%5Fid=([0-9])");
            int ndock_id = Convert.ToInt32(match.Groups[1].Value);
            //高速修復かどうか
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fhighspeed=([0-9])");
            int highspeed = Convert.ToInt32(match.Groups[1].Value);
            //高速修復の場合、該当する船を即座に直す
            ApiShip ship = APIPort.ShipsDictionary[ship_id];
            if (highspeed == 1)
            {
                ship.api_nowhp = ship.api_maxhp;
                ship.api_cond = Math.Max(ship.api_cond, 40);
            }
            //出撃報告書にセット（前）
            SortieReportDataBase.SetRepair(false);
            //修復に必要な資材を探す
            //入渠で使う資材
            int[] usematerial = new int[]{-1*ship.api_ndock_item[0], 0, -1*ship.api_ndock_item[1], 0, 
                                            0, -1*highspeed, 0, 0};
            APIPort.AddMaterial(usematerial);
            //出撃報告書にセット（後）
            SortieReportDataBase.SetRepair(true);

            //バルーンに
            if(highspeed != 1) NotifyBalloon.AddNdock(ship_id, ndock_id);
        }

        //speedchange
        public static void ReadSpeedchange(string requestbody)
        {
            //入渠のID
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fndock%5Fid=([0-9])");
            int ndock_id = Convert.ToInt32(match.Groups[1].Value);
            //船を直す
            int shipid = APIPort.Ndocks[ndock_id - 1].api_ship_id;
            ApiShip target_ship = APIPort.ShipsDictionary[shipid];
            target_ship.api_nowhp = target_ship.api_maxhp;
            //入渠ドックを戻す
            ApiNdock ndock = APIPort.Ndocks[ndock_id -1];
            int buf_memberid = ndock.api_member_id;
            int buf_id = ndock.api_id;
            ndock = new ApiNdock();
            ndock.api_member_id = buf_memberid;
            ndock.api_id = buf_id;
            ndock.api_complete_time_str = "0";
            APIPort.Ndocks[ndock_id - 1] = ndock;
            //出撃報告書にセット（前）
            SortieReportDataBase.SetRepair(false);
            //入渠に使う資材（バケツ消費）
            APIPort.AddMaterial(new int[] { 0, 0, 0, 0, 0, -1, 0, 0 });
            //出撃報告書にセット（後）
            SortieReportDataBase.SetRepair(true);
            //バルーンの削除
            NotifyBalloon.RemoveNdock(ndock_id);
        }
    }
}
