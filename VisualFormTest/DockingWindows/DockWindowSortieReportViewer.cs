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
    public partial class DockWindowSortieReportViewer : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowSortieReportViewer(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = sortieReportViewer1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowSortieReportViewer";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + sortieReportViewer1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return sortieReportViewer1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = sortieReportViewer1.Size;
        }

        //IsShownの切り替え
        private void DockWindowSortieReportViewer_DockStateChanged(object sender, EventArgs e)
        {
            if (sortieReportViewer1 == null) return;

            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    sortieReportViewer1.IsShown = false;
                    return;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    sortieReportViewer1.IsShown = false;
                    return;
                default:
                    sortieReportViewer1.IsShown = true;
                    return;
            }
        }
    }
}
