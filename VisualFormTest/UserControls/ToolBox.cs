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
    public partial class ToolBox : UserControl
    {
        public Button[] Buttons { get; set; }

        public ToolBox()
        {
            InitializeComponent();

            Buttons = new Button[]
            {
                button_tool_black, button_tool_black2, button_tool_trimfleet, button_tool_trimcombine, button_tool_combine,
            };

            foreach(var x in Buttons)
            {
                x.DragEnter += new DragEventHandler(ImageEdit.ToolButton_DragEnter);
                x.DragDrop += new DragEventHandler(ImageEdit.ToolButton_DragDrop);
            }
            button_tool_combine.Tag = 2;
        }

        private void numericUpDown_tool_combine_ValueChanged(object sender, EventArgs e)
        {
            button_tool_combine.Tag = (int)numericUpDown_tool_combine.Value;
        }


    }
}
