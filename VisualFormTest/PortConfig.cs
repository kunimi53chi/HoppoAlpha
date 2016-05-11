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
    public partial class PortConfig : Form
    {
        public PortConfig()
        {
            InitializeComponent();

            //現在の値のセット
            numericUpDown1.Value = Config.PortNumber;

            //現在の待受ポート
            label2.Text = string.Format("現在の待ち受けポート : {0}", Fiddler.FiddlerApplication.oProxy.ListenPort);

            //プロキシ
            if(Config.ProxyAddress == "")
            {
                checkBox1.Checked = true;
                textBox1.Enabled = false;
            }
            else
            {
                checkBox1.Checked = false;
                textBox1.Text = Config.ProxyAddress;
            }
            //受信専用
            checkBox2.Checked = Config.ListeningMode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ポート番号の保存
            Config.PortNumber = (int)numericUpDown1.Value;
            //プロキシの保存
            if (textBox1.Enabled) Config.ProxyAddress = textBox1.Text;
            else Config.ProxyAddress = "";
            this.Close();
            //受信専用
            Config.ListeningMode = checkBox2.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //利用しないになった場合
            if(checkBox1.Checked)
            {
                textBox1.Enabled = false;
            }
            //利用するになった場合
            else
            {
                textBox1.Enabled = true;
            }
        }
    }
}
