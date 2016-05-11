using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiGetMember
{
    public class ApiShipDeck
    {
        public List<ApiPort.ApiShip> api_ship_data { get; set; }
        public List<ApiPort.ApiDeckPort> api_deck_data { get; set; }
    }
}
