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
    public partial class DockWindowBattleDetailSquare2 : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowBattleDetailSquare2(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = battleDetailSquare1.Size;
            this.MainForm = form1;

            battleDetailSquare1.Init();
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowBattleDetailSquare2";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + battleDetailSquare1.ClientSize.Height - 115;
            }
        }
        public int RealWidth
        {
            get
            {
                return battleDetailSquare1.ClientSize.Width - 7;
            }
        }

        public void Stretch()
        {
            this.ClientSize = battleDetailSquare1.Size;
        }

        //IsShownの切り替え
        private void DockWindowBattleDetailSquare_DockStateChanged(object sender, EventArgs e)
        {
            if (battleDetailSquare1 == null) return;

            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                    battleDetailSquare1.IsShown = false;
                    return;
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    battleDetailSquare1.IsShown = false;
                    return;
                default:
                    battleDetailSquare1.IsShown = true;
                    return;
            }
        }
    }
}
