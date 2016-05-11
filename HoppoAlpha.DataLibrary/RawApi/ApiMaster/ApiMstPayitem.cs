using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstPayitem
    {
        public int api_id { get; set; }
        public int api_type { get; set; }
        public string api_name { get; set; }
        public string api_description { get; set; }
        public List<int> api_item { get; set; }
        public int api_price { get; set; }
    }
}
