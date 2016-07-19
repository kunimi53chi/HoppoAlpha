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
    public partial class DockWindowPracticeInfo : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowPracticeInfo(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = practiceInfo1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowPracticeInfo";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + practiceInfo1.ClientSize.Height;
            }
        }

        public int RealWidth
        {
            get
            {
                return practiceInfo1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = practiceInfo1.Size;
        }

        //IsShown切り替え
        private void DockWindowPracticeInfo_DockStateChanged(object sender, EventArgs e)
        {
            if (practiceInfo1 == null) return;

            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    practiceInfo1.IsShown = false;
                    return;
                default:
                    practiceInfo1.IsShown = true;
                    return;
            }
        }


    }
}
