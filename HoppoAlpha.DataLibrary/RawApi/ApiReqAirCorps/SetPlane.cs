using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqAirCorps
{
    public class SetPlane
    {
        public int api_distance { get; set; }
        public List<ApiPlaneInfo> api_plane_info { get; set; }
        public int api_after_bauxite { get; set; }
    }
}
