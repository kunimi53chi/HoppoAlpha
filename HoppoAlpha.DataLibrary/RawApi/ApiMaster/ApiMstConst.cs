using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstConst
    {
        public BokoMaxShips api_boko_max_ships { get; set; }
        public ApiParallelQuestMax api_parallel_quest_max { get; set; }
        public ApiDpflagQuest api_dpflag_quest { get; set; }

        public class BokoMaxShips
        {
            public string api_string_value { get; set; }
            public int api_int_value { get; set; }
        }
        public class ApiDpflagQuest
        {
            public string api_string_value { get; set; }
            public int api_int_value { get; set; }
        }
        public class ApiParallelQuestMax
        {
            public string api_string_value { get; set; }
            public int api_int_value { get; set; }
        }
    }
}
