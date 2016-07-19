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
    public partial class RankingViewer_SubmarinerSetting : Form
    {
        public bool RefreshRequired { get; set; }

        public RankingViewer_SubmarinerSetting()
        {
            InitializeComponent();

            //潜水マンハンデ
            numericUpDown_eohandicap.Value = Config.RankingSubmarinerEOHandicap;
            //潜水マン一覧
            listView_submariner.SuspendLayout();
            var listitems = Config.RankingSubmarinerList.Select(x => new ListViewItem(x)).ToArray();
            listView_submariner.Items.Clear();
            listView_submariner.Items.AddRange(listitems);
            listView_submariner.ResumeLayout();
        }

        //設定を保存
        public void SaveSetting()
        {
            //潜水マンハンデ
            Config.RankingSubmarinerEOHandicap = (int)numericUpDown_eohandicap.Value;
            //潜水マンリスト
            var submariner = listView_submariner.Items.OfType<ListViewItem>().Where(x => x.SubItems.Count >= 1).Select(x => x.SubItems[0].Text);
            var hashset = new HashSet<string>();
            foreach(var s in submariner)
            {
                hashset.Add(s);
            }
            Config.RankingSubmarinerList = hashset;

            RefreshRequired = true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            SaveSetting();
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_deleteselected_Click(object sender, EventArgs e)
        {
            var selected = listView_submariner.SelectedItems.OfType<ListViewItem>();

            listView_submariner.SuspendLayout();
            foreach(var s in selected)
            {
                listView_submariner.Items.Remove(s);
            }
            listView_submariner.ResumeLayout();
        }
    }
}
