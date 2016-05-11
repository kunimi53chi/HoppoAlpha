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
    public partial class DockWindowKCVDBLog : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowKCVDBLog(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = kcvdbLog1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowKCVDBLog";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + kcvdbLog1.Height;
            }
        }

        public int RealWidth
        {
            get
            {
                return kcvdbLog1.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = kcvdbLog1.Size;
        }

        private void DockWindowKCVDBLog_DockStateChanged(object sender, EventArgs e)
        {
            if (kcvdbLog1 == null) return;

            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    kcvdbLog1.IsShown = false;
                    return;
                default:
                    kcvdbLog1.IsShown = true;
                    return;
            }
        }


    }
}
