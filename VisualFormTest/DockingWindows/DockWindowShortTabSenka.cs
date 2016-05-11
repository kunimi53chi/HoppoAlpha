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
    public partial class DockWindowShortTabSenka : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowShortTabSenka(Form1 form1)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.speech_balloon_orange_s;

            this.ClientSize = tabSenkaShort1.Size;
            this.MainForm = form1;

            tabSenkaShort1.Init();
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowShortTabSenka";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + tabSenkaShort1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return tabSenkaShort1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = tabSenkaShort1.Size;
        }

        //IsShownの切り替え
        private void DockWindowShortTabSenka_DockStateChanged(object sender, EventArgs e)
        {
            if (tabSenkaShort1 == null) return;

            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    tabSenkaShort1.IsShown = false;
                    return;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    tabSenkaShort1.IsShown = false;
                    return;
                default:
                    tabSenkaShort1.IsShown = true;
                    return;
            }
        }
    }
}
