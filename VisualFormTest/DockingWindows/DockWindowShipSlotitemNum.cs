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
    public partial class DockWindowShipSlotitemNum : DockContent, DockingWindows.IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public DockWindowShipSlotitemNum(Form1 form1)
        {
            InitializeComponent();
            this.ClientSize = shipSlotitemNum1.Size;
            this.MainForm = form1;

            this.HideOnClose = true;
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + shipSlotitemNum1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return shipSlotitemNum1.ClientSize.Width;
            }
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowShipSlotitemNum";
        }
        public void Stretch()
        {
            this.ClientSize = shipSlotitemNum1.Size;
        }

    }
}
