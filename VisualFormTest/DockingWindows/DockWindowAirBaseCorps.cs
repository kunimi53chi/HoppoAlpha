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
    public partial class DockWindowAirBaseCorps : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowAirBaseCorps(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = airBaseCorps1.Size;
            this.MainForm = form1;

            airBaseCorps1.Init();//こうしないとデザイナーでエラーになる
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowAirBaseCorps";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + airBaseCorps1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return airBaseCorps1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = airBaseCorps1.Size;
        }

        //IsShown切り替え
        private void DockWindowAirBaseCorps_DockStateChanged(object sender, EventArgs e)
        {
            if (airBaseCorps1 == null) return;

            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    airBaseCorps1.IsShown = false;
                    return;
                default:
                    airBaseCorps1.IsShown = true;
                    return;
            }
        }
    }
}
