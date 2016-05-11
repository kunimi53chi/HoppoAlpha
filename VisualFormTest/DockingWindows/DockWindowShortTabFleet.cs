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
    public partial class DockWindowShortTabFleet : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }
        public DockWindowTabCollection Collection { get; set; }

        public DockWindowShortTabFleet(Form1 form1, DockWindowTabCollection collection)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.speech_balloon_orange_f;

            this.ClientSize = tabFleetShort1.Size;
            this.MainForm = form1;
            this.Collection = collection;

            tabFleetShort1.Init();
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowShortTabFleet";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + tabFleetShort1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return tabFleetShort1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = tabFleetShort1.Size;
        }

        //IsShownの切り替え
        private void DockWindowShortTabFleet_DockStateChanged(object sender, EventArgs e)
        {
            if (tabFleetShort1 == null) return;

            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    tabFleetShort1.IsShown = false;
                    return;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    tabFleetShort1.IsShown = false;
                    return;
                default:
                    tabFleetShort1.IsShown = true;
                    return;
            }
        }
    }
}
