using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest.UserControls
{
    public partial class SankWarning : UserControl
    {
        public SankWarning()
        {
            InitializeComponent();
            this.ClientSize = this.textBox_sankwarning.Size;
        }
    }
}
