using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqKaisou
{
    public class ApiSlotDeprive
    {
        public ApiShipData api_ship_data { get; set; }
        public ApiUnsetList api_unset_list { get; set; }
    }

    public class ApiShipData
    {
        public ApiShip api_set_ship { get; set; }
        public ApiShip api_unset_ship { get; set; }
    }

    public class ApiUnsetList
    {
        public int api_type3No { get; set; }
        public List<int> api_slot_list { get; set; }
    }
}
