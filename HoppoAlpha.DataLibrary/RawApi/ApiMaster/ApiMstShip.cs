using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstShip
    {
        public int api_id { get; set; }
        public int api_sortno { get; set; }
        public string api_name { get; set; }
        public string api_yomi { get; set; }
        public int api_stype { get; set; }
        public int api_afterlv { get; set; }
        public string api_aftershipid { get; set; }
        public List<int> api_taik { get; set; }
        public List<int> api_souk { get; set; }
        public List<int> api_houg { get; set; }
        public List<int> api_raig { get; set; }
        public List<int> api_tyku { get; set; }
        public List<int> api_luck { get; set; }
        public int api_soku { get; set; }
        public int api_leng { get; set; }
        public int api_slot_num { get; set; }
        public List<int> api_maxeq { get; set; }
        public int api_buildtime { get; set; }
        public List<int> api_broken { get; set; }
        public List<int> api_powup { get; set; }
        public int api_backs { get; set; }
        public string api_getmes { get; set; }
        public int api_afterfuel { get; set; }
        public int api_afterbull { get; set; }
        public int api_fuel_max { get; set; }
        public int api_bull_max { get; set; }
        public int api_voicef { get; set; }
    }
}
