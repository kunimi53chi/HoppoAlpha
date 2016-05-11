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
    public partial class DockWindowMapInfo : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowMapInfo(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = mapInfo1.Size;
            this.MainForm = form1;

            this.HideOnClose = true;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowMapInfo";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + mapInfo1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return mapInfo1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = mapInfo1.Size;
        }

        private void DockWindowMapInfo_DockStateChanged(object sender, EventArgs e)
        {
            if (mapInfo1 == null) return;

            bool oldstate = mapInfo1.IsShown;
            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    mapInfo1.IsShown = false;
                    break;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    mapInfo1.IsShown = false;
                    break;
                default:
                    mapInfo1.IsShown = true;
                    break;
            }
        }
    }
}
