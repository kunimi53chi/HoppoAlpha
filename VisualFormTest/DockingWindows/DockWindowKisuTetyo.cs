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
    public partial class DockWindowKisuTetyo : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowKisuTetyo(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = kisuTetyo1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowKisuTetyo";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + kisuTetyo1.ClientSize.Height;
            }
        }

        public int RealWidth
        {
            get
            {
                return kisuTetyo1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = kisuTetyo1.Size;
        }

        //IsShown切り替え
        private void DockWindowPracticeInfo_DockStateChanged(object sender, EventArgs e)
        {
            if (kisuTetyo1 == null) return;

            bool oldIsShown = kisuTetyo1.IsShown;

            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    kisuTetyo1.IsShown = false;
                    break;
                default:
                    kisuTetyo1.IsShown = true;
                    break;
            }

            //非表示→表示になった場合
            if(!oldIsShown && kisuTetyo1.IsShown)
            {
                kisuTetyo1.Navigate();
            }
        }
    }
}
