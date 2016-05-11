using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstShipgraph
    {
        public int api_id { get; set; }
        public int api_sortno { get; set; }
        public string api_filename { get; set; }
        public string api_version { get; set; }
        public List<int> api_boko_n { get; set; }
        public List<int> api_boko_d { get; set; }
        public List<int> api_kaisyu_n { get; set; }
        public List<int> api_kaisyu_d { get; set; }
        public List<int> api_kaizo_n { get; set; }
        public List<int> api_kaizo_d { get; set; }
        public List<int> api_map_n { get; set; }
        public List<int> api_map_d { get; set; }
        public List<int> api_ensyuf_n { get; set; }
        public List<int> api_ensyuf_d { get; set; }
        public List<int> api_ensyue_n { get; set; }
        public List<int> api_battle_n { get; set; }
        public List<int> api_battle_d { get; set; }
        public List<int> api_weda { get; set; }
        public List<int> api_wedb { get; set; }
    }
}
