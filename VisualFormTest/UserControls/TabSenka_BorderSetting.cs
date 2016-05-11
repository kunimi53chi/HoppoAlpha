using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest.UserControls
{
    public partial class TabSenka_BorderSetting : Form
    {
        public bool RefreshRequired { get; set; }

        NumericUpDown[] updowns;

        public TabSenka_BorderSetting()
        {
            InitializeComponent();

            updowns = new NumericUpDown[]
            {
                numericUpDown1, numericUpDown2, numericUpDown3, numericUpDown4,
                numericUpDown5, numericUpDown6, numericUpDown7,
            };

            //順位の表示
            foreach (int i in Enumerable.Range(0, updowns.Length))
            {
                updowns[i].Value = Config.TabSenkaBorderDisplay[i];
            }
        }

        public void SaveSettings()
        {
            foreach(var i in Enumerable.Range(0, updowns.Length))
            {
                Config.TabSenkaBorderDisplay[i] = (int)updowns[i].Value;
            }

            RefreshRequired = true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_todefault_Click(object sender, EventArgs e)
        {
            foreach(var i in Enumerable.Range(0, updowns.Length))
            {
                updowns[i].Value = HoppoAlpha.DataLibrary.DataObject.SenkaRecord.DisplayRank[i];
            }
        }



    }
}
