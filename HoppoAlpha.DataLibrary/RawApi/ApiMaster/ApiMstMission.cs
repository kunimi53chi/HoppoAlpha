using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstMission
    {
        public int api_id { get; set; }
        public int api_maparea_id { get; set; }
        public string api_name { get; set; }
        public string api_details { get; set; }
        public int api_time { get; set; }
        public int api_difficulty { get; set; }
        public double api_use_fuel { get; set; }
        public double api_use_bull { get; set; }
        public List<int> api_win_item1 { get; set; }
        public List<int> api_win_item2 { get; set; }
        public int api_return_flag { get; set; }
    }
}
