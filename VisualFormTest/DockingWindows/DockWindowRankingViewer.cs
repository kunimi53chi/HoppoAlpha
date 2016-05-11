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
    public partial class DockWindowRankingViewer : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowRankingViewer(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = rankingViewer1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowRankingViewer";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + rankingViewer1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return rankingViewer1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = rankingViewer1.Size;
        }

        //IsShownの切り替え
        private void DockWindowRankingViewer_DockStateChanged(object sender, EventArgs e)
        {
            if (rankingViewer1 == null) return;

            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    rankingViewer1.IsShown = false;
                    return;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    rankingViewer1.IsShown = false;
                    return;
                default:
                    rankingViewer1.IsShown = true;
                    return;
            }
        }

    }
}
