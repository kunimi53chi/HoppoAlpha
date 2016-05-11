using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiPort
{
    public class ApiLog
    {
        public int api_no { get; set; }
        public string api_type { get; set; }
        public string api_state { get; set; }
        public string api_message { get; set; }
    }
}
