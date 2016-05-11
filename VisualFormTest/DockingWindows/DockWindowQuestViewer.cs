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
    public partial class DockWindowQuestViewer : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowQuestViewer(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = questViewer1.Size;
            this.MainForm = form1;

            this.HideOnClose = true;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowQuestViewer";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + questViewer1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return questViewer1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = questViewer1.Size;
        }

        private void DockWindowQuestViewer_DockStateChanged(object sender, EventArgs e)
        {
            if (questViewer1 == null) return;

            bool oldval = questViewer1.IsShown;
            switch (this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    questViewer1.IsShown = false;
                    break;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    questViewer1.IsShown = false;
                    break;
                default:
                    questViewer1.IsShown = true;
                    break;
            }

            //現れた場合
            if (!oldval && questViewer1.IsShown)
            {
                questViewer1.Init();
            }
        }

    }
}
