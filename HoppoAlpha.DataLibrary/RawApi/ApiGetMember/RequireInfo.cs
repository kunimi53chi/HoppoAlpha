using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiGetMember
{
    public class RequireInfo
    {
        public Basic api_basic { get; set; }
        public List<SlotItem> api_slot_item { get; set; }
        //public ApiUnsetslot api_unsetslot { get; set; }//unsetslotだけ自前で
        public List<Kdock> api_kdock { get; set; }
        public List<Useitem> api_useitem { get; set; }
        public List<Furniture> api_furniture { get; set; }
    }
}
