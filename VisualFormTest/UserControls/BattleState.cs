using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest.UserControls
{
    public partial class BattleState : UserControl
    {
        public Label[] Labels { get; private set; }

        public BattleState()
        {
            InitializeComponent();

            this.Labels = new Label[] { label_bs1, label_bs2, label_bs3, label_bs4, label_bs5, label_bs6 };
        }

        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(this, "battlestate");
        }
    }
}
