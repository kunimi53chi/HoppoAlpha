using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualFormTest.UserControls
{
    public interface ITabControl
    {
        void Init();
        void Init2();

        bool InitFinished { get; set; }
        bool Init2Finished { get; set; }
    }
}
