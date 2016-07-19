using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiReqRanking;

namespace VisualFormTest.UserControls
{
    public partial class RankingViewer_SelfEo : Form
    {
        CheckBox[] cbs;

        public RankingViewer.RankingListViewTag RankingTag { get; set; }
        public int EoAppend { get; set; }
        public bool IsOkPressed { get; set; }

        public RankingViewer_SelfEo(RankingViewer.RankingListViewTag tag)
        {
            InitializeComponent();

            cbs = new CheckBox[]
            {
                checkBox_15, checkBox_16, checkBox_25, checkBox_35, checkBox_45, checkBox_55,
            };

            this.RankingTag = tag;
        }

        private void RefreshCheckBoxState(object sender, EventArgs e)
        {
            EoAppend = 0;
            foreach(var c in cbs)
            {
                if (c.Checked) EoAppend += Convert.ToInt32(c.Tag);
            }
            numericUpDown_eo.Value = EoAppend;
        }

        #region イベントハンドラ
        private void RankingViewer_SelfEo_Load(object sender, EventArgs e)
        {
            foreach (var c in cbs) c.CheckedChanged += new EventHandler(RefreshCheckBoxState);
            
            //ハッシュ等の表示更新
            label_date.Text = RankingTag.CacheHash.Display2;
            label_name.Text = RankingTag.RankingList.api_nickname;
            label_senka.Text = RankingTag.RankingList.ViewSenka.ToString();
            if (RankingTag.RankingDiff.DiffSenka.HasValue) label_diff.Text = RankingTag.RankingDiff.DiffSenka.Value.ToString();
            else label_diff.Text = "?";
            label_addedeo.Text = RankingTag.SumEo.ToString();
        }

        private void numericUpDown_eo_ValueChanged(object sender, EventArgs e)
        {
            EoAppend = (int)numericUpDown_eo.Value;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            IsOkPressed = true;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            IsOkPressed = false;
            this.Close();
        }
        #endregion
    }
}
