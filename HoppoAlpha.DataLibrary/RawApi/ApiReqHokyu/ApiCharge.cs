using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqHokyu
{
    public class ApiCharge
    {
        public List<ApiChargeShip> api_ship { get; set; }
        public List<int> api_material { get; set; }
        public int api_use_bou { get; set; }

        public class ApiChargeShip
        {
            public int api_id { get; set; }
            public int api_fuel { get; set; }
            public int api_bull { get; set; }
            public List<int> api_onslot { get; set; }
        }
    }
}
