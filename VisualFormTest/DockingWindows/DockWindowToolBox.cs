using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualFormTest.DockingWindows
{
    public partial class DockWindowToolBox : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowToolBox(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize =  toolBox1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowToolBox";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + toolBox1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return toolBox1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = toolBox1.Size;
        }
    }
}
