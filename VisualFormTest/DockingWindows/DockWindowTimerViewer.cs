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
    public partial class DockWindowTimerViewer : DockContent, DockingWindows.IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowTimerViewer(Form1 form1)
        {
            InitializeComponent();
            this.ClientSize = timerViewer1.Size;
            this.MainForm = form1;

            this.HideOnClose = true;
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + timerViewer1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return timerViewer1.ClientSize.Width;
            }
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowTimerViewer";
        }

        public void Stretch()
        {
            this.ClientSize = timerViewer1.Size;
        }

    }
}
