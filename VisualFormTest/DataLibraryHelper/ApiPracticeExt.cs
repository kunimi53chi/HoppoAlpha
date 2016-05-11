using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiReqPractice;

namespace VisualFormTest
{
    public static class ApiPracticeExt
    {
        //ToolTips用
        public static string DisplayToolTips(this ApiPractice item)
        {
            if (item.api_id == 0) return null;
            //船の名前
            string shipname = APIMaster.MstShips[item.api_enemy_flag_ship].api_name;
            return string.Format("旗艦:{0} / ID:{1} / 階級:{2}", shipname, item.api_enemy_id, item.api_enemy_rank);
        }

    }
}
