using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstMapbgm
    {
        public int api_id { get; set; }
        public int api_maparea_id { get; set; }
        public int api_no { get; set; }
        public List<int> api_map_bgm { get; set; }
        public List<int> api_boss_bgm { get; set; }
    }
}
