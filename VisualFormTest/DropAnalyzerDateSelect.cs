using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest
{
    public partial class DropAnalyzerDateSelect : Form
    {
        readonly DateTime newDate = new DateTime();

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsOk { get; set; }

        public DropAnalyzerDateSelect(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            if (startDate == newDate) dateTimePicker1.Enabled = false;
            if (endDate == newDate) dateTimePicker2.Enabled = false;

            SetValueToControls(startDate, endDate);
        }

        private void SetPropertiesFromControls()
        {
            if (!checkBox1.Checked) this.Start = newDate;
            else this.Start = dateTimePicker1.Value;

            if (!checkBox2.Checked) this.End = newDate;
            else this.End = dateTimePicker2.Value;
        }

        private void SetValueToControls(DateTime startDate, DateTime endDate)
        {
            if (startDate == newDate)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
                dateTimePicker1.Value = startDate;
            }

            if (endDate == newDate)
            {
                checkBox2.Checked = false;
            }
            else
            {
                checkBox2.Checked = true;
                dateTimePicker2.Value = endDate;
            }

            SetPropertiesFromControls();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            SetPropertiesFromControls();
            this.IsOk = true;
            this.Close();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            SetValueToControls(newDate, newDate);
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if(cb == checkBox1)
            {
                dateTimePicker1.Enabled = cb.Checked;
            }
            else if(cb == checkBox2)
            {
                dateTimePicker2.Enabled = cb.Checked;
            }
        }
    }
}
