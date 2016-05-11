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
    public partial class DockWindowBattleDetail : DockContent, IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowBattleDetail(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = battleDetail1.Size;
            this.MainForm = form1;

            battleDetail1.Init();
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowBattleDetail";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + battleDetail1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return battleDetail1.ClientSize.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = battleDetail1.Size;
        }
    }
}
