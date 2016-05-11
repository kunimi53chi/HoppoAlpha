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
    public partial class DockWindowHoppoBrowser : DockContent, DockingWindows.IMyDockingWindow
    {
        public Form1 MainForm { get; set; }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + hoppoBrowser1.ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return hoppoBrowser1.ClientSize.Width;
            }
        }

        protected override string GetPersistString()
        {
            return "VisualFormTest.DockWindowHoppoBrowser";
        }

        public void Stretch()
        {
            this.ClientSize = hoppoBrowser1.Size;
        }

        public DockWindowHoppoBrowser(Form1 form1)
        {
            InitializeComponent();

            hoppoBrowser1.ClientSize = new Size((int)(Config.GameScreenSize.Width * Config.Ratio / 100), (int)(Config.GameScreenSize.Height * Config.Ratio / 100));
            this.ClientSize = hoppoBrowser1.Size;
            this.MainForm = form1;

            this.HideOnClose = true;

            hoppoBrowser1.ToolStrip = form1.ToolStrip;
            
        }
    }
}
