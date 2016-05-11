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
    public partial class ShipSlotitemNum : UserControl
    {
        public Label[] Labels { get; set; }
        public Panel[] Panels { get; set; }

        public ShipSlotitemNum()
        {
            InitializeComponent();
            this.ClientSize = panel2.Size;

            Labels = new Label[] { label2, label3, label5, label6 };
            Panels = new Panel[] { panel7, panel8 };
        }

        public void SetValue_Q()
        {
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    var dock = this.FindForm() as DockingWindows.DockWindowShipSlotitemNum;
                    KancolleInfo.SetShipSlotitemNum(Labels, Panels, dock.MainForm.toolStripStatusLabel6);
                }));
        }
    }
}
