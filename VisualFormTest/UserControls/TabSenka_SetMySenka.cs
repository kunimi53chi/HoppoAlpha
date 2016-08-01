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
    public partial class TabSenka_SetMySenka : Form
    {
        Form1 _form1;

        public TabSenka_SetMySenka(Form1 form1)
        {
            InitializeComponent();

            this._form1 = form1;
        }

        private bool Apply()
        {
            int val;
            if (!int.TryParse(textBox1.Text, out val)) return false;
            if (val < -1) return false;

            if (HistoricalData.LogSenka == null || HistoricalData.LogSenka.Count == 0) return false;
            //セクションが異なる場合は閉じる
            if (APIPort.Basic == null) return false;
            if (APIReqRanking.GetFileName(DateTime.Now) != APIReqRanking.LastSavedFileName)
            {
                if (MessageBox.Show("セクションが変更されため終了します", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                {
                    this.Close();
                    return false;
                }
            }

            var senka = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
            senka.StartSenka = val;
            senka.CalcEstimateMySenka();

            _form1.UpdateSenka();
            return true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (Apply()) this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            Apply();
        }
    }
}
