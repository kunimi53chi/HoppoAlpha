using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiReqMember;

namespace VisualFormTest.UserControls
{
    public partial class PracticeInfo : UserControl
    {
        List<GetPracticeEnemyinfo> data = new List<GetPracticeEnemyinfo>();
        Label[] view_fleets;

        public bool IsShown { get; set; }


        public PracticeInfo()
        {
            InitializeComponent();

            view_fleets = new Label[]
            {
                label_view_fleet1, label_view_fleet2, label_view_fleet3, label_view_fleet4, label_view_fleet5, label_view_fleet6
            };
        }

        //リストビューの更新
        public void RefreshListview()
        {
            listView_result.SuspendLayout();

            var listitems = data.Select(delegate(GetPracticeEnemyinfo einfo)
                {
                    var item = new ListViewItem(einfo.GetDatetime.ToString());
                    item.SubItems.Add(einfo.api_nickname);

                    if (einfo.api_experience != null && einfo.api_experience.Count >= 1) item.SubItems.Add(einfo.api_experience[0].ToString("N0"));
                    else item.SubItems.Add("0");

                    item.SubItems.Add(einfo.api_member_id.ToString());

                    item.Tag = einfo;//タグに情報を紐付ける

                    return item;
                }).ToArray();

            listView_result.Items.Clear();
            listView_result.Items.AddRange(listitems);

            listView_result.ResumeLayout();
        }


        #region イベントハンドラ
        private void PracticeInfo_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        //検索ボタン
        private void button_search_Click(object sender, EventArgs e)
        {
            if(PracticeInfoDataBase.Collection == null || PracticeInfoDataBase.Collection.AllData == null) return;
            if (!checkBox_name.Checked && !checkBox_id.Checked && !checkBox_date.Checked) return;//検索条件のどれもチェックがはいってなければ

            int memberid;
            int.TryParse(textBox_id.Text, out memberid);

            data = new List<GetPracticeEnemyinfo>();

            foreach(var user in PracticeInfoDataBase.Collection.AllData)
            {
                if(user.Value == null || user.Value.MemberDataByDate == null) continue;

                //提督IDでチェック
                if(checkBox_id.Checked)
                {
                    if (user.Key != memberid) continue;
                }

                //提督名でチェック
                if(checkBox_name.Checked)
                {
                    var first = user.Value.MemberDataByDate.FirstOrDefault();
                    if(first.Value != null && first.Value.api_nickname != null)
                    {
                        if (!first.Value.api_nickname.Contains(textBox_name.Text)) continue;
                    }
                    else
                    {
                        continue;
                    }
                }

                //日付でチェック
                if (checkBox_date.Checked)
                {
                    foreach (var record in user.Value.MemberDataByDate)
                    {
                        if (record.Value == null) continue;

                        if (record.Value.GetDatetime.Date != dateTimePicker1.Value.Date) continue;
                        else data.Add(record.Value);
                    }
                }

                //リストに追加
                if (data.Count >= numericUpDown_num.Value) break;
                else
                {
                    //日付でチェックしない場合以外
                    if(!checkBox_date.Checked && user.Value.MemberDataByDate.Values != null)
                    {
                        data.AddRange(user.Value.MemberDataByDate.Values);
                    }
                }
            }

            RefreshListview();
        }

        //右側の表示更新
        private void listView_result_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_result.SelectedItems.Count == 0) return;

            var first = listView_result.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (first == null) return;
            var tag = first.Tag as GetPracticeEnemyinfo;
            if (tag == null) return;

            //表示更新
            //提督名
            label_view_name.Text = tag.api_nickname;
            //レベル～提督経験値
            label_view_lv.Text = tag.api_level.ToString();
            label_view_rank.Text = Helper.RankToString(tag.api_rank);
            label_view_date.Text = tag.GetDatetime.ToString();
            if (tag.api_experience != null && tag.api_experience.Count >= 2)
                label_view_exp.Text = string.Format("{0} / {1}", tag.api_experience[0].ToString("N0"), tag.api_experience[1].ToString("N0"));
            else
                label_view_exp.Text = "null";
            //コメント
            label_view_comment.Text = tag.api_cmt;

            //友軍艦隊～家具保有数
            label_view_friend.Text = tag.api_friend.ToString();
            label_view_chara.Text = ListComverter(tag.api_ship);
            label_view_equip.Text = ListComverter(tag.api_slotitem);
            label_view_furniture.Text = tag.api_furniture.ToString();
            
            //艦隊詳細
            foreach(var i in Enumerable.Range(0, view_fleets.Length))
            {
                if (i >= tag.api_deck.api_ships.Count) view_fleets[i].Text = "";
                else
                {
                    if (tag.api_deck.api_ships[i].ShipName == null) view_fleets[i].Text = "";
                    else view_fleets[i].Text = tag.api_deck.api_ships[i].ShipName + " Lv" + tag.api_deck.api_ships[i].api_level;
                }
            }
        }

        private string ListComverter(List<int> list)
        {
            if (list != null && list.Count >= 2) return string.Format("{0} / {1}", list[0], list[1]);
            else return "null";
        }

        private void button_byuser_Click(object sender, EventArgs e)
        {
            if (PracticeInfoDataBase.Collection == null || PracticeInfoDataBase.Collection.AllData == null) return;

            data = new List<GetPracticeEnemyinfo>();

            foreach(var member in PracticeInfoDataBase.Collection.AllData)
            {
                if (member.Value == null || member.Value.MemberDataByDate == null) continue;

                var first = member.Value.MemberDataByDate.Values.FirstOrDefault();
                if(first != null)
                {
                    data.Add(first);
                }
            }

            RefreshListview();
        }

        private void button_clipboard_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            //ヘッダー
            sb.AppendLine("日付\t名前\t提督経験値\tID");
            //中身
            foreach(var item in listView_result.Items.OfType<ListViewItem>())
            {
                var row = string.Join("\t", item.SubItems.OfType<ListViewItem.ListViewSubItem>().Select(x => x.Text));
                sb.AppendLine(row);
            }

            Clipboard.SetText(sb.ToString());
        }
        #endregion
    }
}
