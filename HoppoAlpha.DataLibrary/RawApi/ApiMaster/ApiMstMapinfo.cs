using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstMapinfo
    {
        public int api_id { get; set; }
        public int api_maparea_id { get; set; }
        public int api_no { get; set; }
        public string api_name { get; set; }
        public int api_level { get; set; }
        public string api_opetext { get; set; }
        public string api_infotext { get; set; }
        public List<int> api_item { get; set; }
        public object api_max_maphp { get; set; } //double or null
        public object api_required_defeat_count { get; set; } //double or null
        public List<int> api_sally_flag { get; set; }
    }
}
