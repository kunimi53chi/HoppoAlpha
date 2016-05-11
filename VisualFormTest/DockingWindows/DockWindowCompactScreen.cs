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
    public partial class DockWindowCompactScreen :  DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowCompactScreen(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = compactScreen1.Size;
            this.MainForm = form1;

        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowCompactScreen";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + compactScreen1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return compactScreen1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = compactScreen1.Size;
        }

        //IsShownの切り替え
        private void DockWindowCompactScreen_DockStateChanged(object sender, EventArgs e)
        {
            if (compactScreen1 == null) return;

            bool oldval = compactScreen1.IsShown;
            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    compactScreen1.IsShown = false;
                    break;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    compactScreen1.IsShown = false;
                    break;
                default:
                    compactScreen1.IsShown = true;
                    break;
            }

            //現れた場合
            if(!oldval && compactScreen1.IsShown)
            {
                compactScreen1.Init();
            }
        }
    }
}
