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
    public partial class DockWindowBattleState : DockContent, DockingWindows.IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowBattleState(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = battleState1.Size;
            this.MainForm = form1;

            this.HideOnClose = true;
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowBattleState";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + battleState1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return battleState1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = battleState1.Size;
        }

    }
}
