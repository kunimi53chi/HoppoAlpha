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
            var listitems = Config.RankingSubmarinerList.Select(delegate(KeyValuePair<int, string> x)
            {
                var item = new ListViewItem(x.Key.ToString());
                item.SubItems.Add(x.Value);
                return item;
            }).ToArray();
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
            Dictionary<int, string> submariner = new Dictionary<int, string>();
            foreach(var i in listView_submariner.Items.OfType<ListViewItem>())
            {
                if (i.SubItems.Count < 2) continue;
                //提督ID
                int id;
                int.TryParse(i.SubItems[0].Text, out id);
                if(id != 0)
                {
                    submariner[id] = i.SubItems[1].Text;//提督名
                }
            }

            Config.RankingSubmarinerList = submariner;

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
