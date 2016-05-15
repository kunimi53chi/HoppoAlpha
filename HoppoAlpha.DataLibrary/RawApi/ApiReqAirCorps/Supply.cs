using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqAirCorps
{
    public class Supply
    {
        public int api_after_fuel { get; set; }
        public int api_after_bauxite { get; set; }
        public List<ApiPlaneInfo> api_plane_info { get; set; }
    }
}
