using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstFurniture
    {
        public int api_id { get; set; }
        public int api_type { get; set; }
        public int api_no { get; set; }
        public string api_title { get; set; }
        public string api_description { get; set; }
        public int api_rarity { get; set; }
        public int api_price { get; set; }
        public int api_saleflg { get; set; }
        public int api_season { get; set; }
    }
}
