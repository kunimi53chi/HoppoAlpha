using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HoppoAlpha.DataLibrary.RawApi.ApiPort
{
    [Serializable]
    public class ApiDeckPort
    {
        public int api_member_id { get; set; }
        public int api_id { get; set; }
        public string api_name { get; set; }
        public string api_name_id { get; set; }
        public List<long> api_mission { get; set; }
        public string api_flagship { get; set; }
        public List<int> api_ship { get; set; }
    }
}
