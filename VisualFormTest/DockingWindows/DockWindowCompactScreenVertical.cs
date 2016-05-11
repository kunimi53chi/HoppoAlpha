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
    public partial class DockWindowCompactScreenVertical : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowCompactScreenVertical(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = compactScreenVertical1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowCompactScreenVertical";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + compactScreenVertical1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return compactScreenVertical1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = compactScreenVertical1.Size;
        }

        //IsShownの切り替え
        private void DockWindowCompactScreenVertical_DockStateChanged(object sender, EventArgs e)
        {
            if (compactScreenVertical1 == null) return;

            bool oldval = compactScreenVertical1.IsShown;
            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    compactScreenVertical1.IsShown = false;
                    break;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    compactScreenVertical1.IsShown = false;
                    break;
                default:
                    compactScreenVertical1.IsShown = true;
                    break;
            }

            //現れた場合
            if (!oldval && compactScreenVertical1.IsShown)
            {
                compactScreenVertical1.Init();
            }
        }
    }
}
