using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstShipupgrade
    {
        public int api_id { get; set; }
        public int api_original_ship_id { get; set; }
        public int api_upgrade_type { get; set; }
        public int api_upgrade_level { get; set; }
        public int api_drawing_count { get; set; }
        public int api_sortno { get; set; }
    }
}
