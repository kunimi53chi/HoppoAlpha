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
    public partial class DockWindowDropAnalyzer : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowDropAnalyzer(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = dropAnalyzer1.Size;
            this.MainForm = form1;

            this.HideOnClose = true;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowDropAnalyzer";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + dropAnalyzer1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return dropAnalyzer1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = dropAnalyzer1.Size;
        }

        private void DockWindowDropAnalyzer_DockStateChanged(object sender, EventArgs e)
        {
            if (dropAnalyzer1 == null) return;

            bool oldstate = dropAnalyzer1.IsShown;
            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    dropAnalyzer1.IsShown = false;
                    break;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    dropAnalyzer1.IsShown = false;
                    break;
                default:
                    dropAnalyzer1.IsShown = true;
                    break;
            }

            //最初に表示された場合
            if(dropAnalyzer1.IsShown && !oldstate)
            {
                dropAnalyzer1.ResetHeader();
            }
        }
    }
}
