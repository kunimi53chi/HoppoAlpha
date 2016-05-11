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
    public partial class DockWindowShortTabMaterial : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowShortTabMaterial(Form1 form1)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.speech_balloon_orange_m;

            this.ClientSize = tabMaterialShort1.Size;
            this.MainForm = form1;

            tabMaterialShort1.Init();
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowShortTabMaterial";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + tabMaterialShort1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return tabMaterialShort1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = tabMaterialShort1.Size;
        }

        //IsShownの切り替え
        private void DockWindowShortTabFleet_DockStateChanged(object sender, EventArgs e)
        {
            if (tabMaterialShort1 == null) return;

            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    tabMaterialShort1.IsShown = false;
                    return;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    tabMaterialShort1.IsShown = false;
                    return;
                default:
                    tabMaterialShort1.IsShown = true;
                    return;
            }
        }
    }
}
