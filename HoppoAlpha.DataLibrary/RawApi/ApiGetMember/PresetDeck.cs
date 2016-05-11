using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiGetMember
{
    public class PresetDeck
    {
        public int api_preset_no { get; set; }
        public string api_name { get; set; }
        public string api_name_id { get; set; }
        public List<int> api_ship { get; set; }

        public PresetDeck()
        {
            this.api_ship = new List<int>();
        }
    }
}
