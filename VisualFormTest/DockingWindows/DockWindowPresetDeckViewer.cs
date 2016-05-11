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
    public partial class DockWindowPresetDeckViewer : DockContent, IMyDockingWindow
    {
        public Form1 MainForm {get; set;}

        public DockWindowPresetDeckViewer(Form1 form1)
        {
            InitializeComponent();

            this.ClientSize = presetDeckViewer1.Size;
            this.MainForm = form1;
        }

        protected override string GetPersistString()
        {
 	         return "VisualFormTest.DockWindowPresetDeckViewer";
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + presetDeckViewer1.Height;
            }
        }

        public int RealWidth
        {
            get
            {
                return presetDeckViewer1.Width;
            }
        }

        public void Stretch()
        {
            this.ClientSize = presetDeckViewer1.Size;
        }

        private void DockWindowPresetDeckViewer_DockStateChanged(object sender, EventArgs e)
        {
            if(presetDeckViewer1 == null) return;

            switch(this.DockState)
            {
                case WeifenLuo.WinFormsUI.Docking.DockState.Hidden:
                case WeifenLuo.WinFormsUI.Docking.DockState.Unknown:
                    presetDeckViewer1.IsShown = false;
                    return;
                default:
                    presetDeckViewer1.IsShown = true;
                    return;
            }
        }
    }
}
