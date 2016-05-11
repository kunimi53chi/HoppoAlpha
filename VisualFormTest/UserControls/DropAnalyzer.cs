using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.Const;

namespace VisualFormTest.UserControls
{
    public partial class DropAnalyzer : UserControl
    {
        private int _area = -1;
        private int _map = -1;
        private int _cell = -1;
        private int _localid = -1;
        private int _ship = -1;
        private int _difficulty = -1;
        private int _item = -1;
        private DateTime startDate = new DateTime();
        private DateTime endDate = new DateTime();

        private ListViewItemComparer listViewItemSorter;

        public bool IsShown { get; set; }

        public DropAnalyzer()
        {
            InitializeComponent();
            
            //リストビュー
            listViewItemSorter = new ListViewItemComparer();
            listViewItemSorter.ColumnModes = new ListViewItemComparer.ComparerMode[]
            {
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.Percent,
            };
            listView_output.ListViewItemSorter = listViewItemSorter;

            DateSelectButtonUpdate();
        }

        //内部クラス
        #region 内部クラス
        public class ListViewItemComparer : IComparer
        {
            public enum ComparerMode
            {
                String, Integer, Percent,
            }

            private int _column;

            public int Column
            {
                get
                {
                    return _column;
                }
                set
                {
                    if(_column == value)
                    {
                        if (Order == SortOrder.Ascending) Order = SortOrder.Descending;
                        else if (Order == SortOrder.Descending) Order = SortOrder.Ascending;
                    }
                    _column = value;
                }
            }
            public SortOrder Order { get; set; }
            public ComparerMode Mode { get; set; }
            public ComparerMode[] ColumnModes { get; set; }

            //コンストラクタ
            public ListViewItemComparer(int col, SortOrder order, ComparerMode mode)
            {
                this._column = col;
                this.Order = order;
                this.Mode = mode;
            }
            public ListViewItemComparer()
            {
                this.Column = 0;
                this.Order = SortOrder.Ascending;
                this.Mode = ComparerMode.String;
            }

            //xがyより小さいときはマイナスの数、大きいときはプラスの数、
            //同じときは0を返す
            public int Compare(object x, object y)
            {
                if(this.Order == SortOrder.None)
                {
                    return 0;
                }

                int result = 0;
                ListViewItem itemx = (ListViewItem)x;
                ListViewItem itemy = (ListViewItem)y;

                //並べ替えモード
                if(ColumnModes != null && ColumnModes.Length > _column)
                {
                    Mode = ColumnModes[_column];
                }
                //並べ替え方法別にxとyを比較
                switch(Mode)
                {
                    case ComparerMode.String:
                        result = string.Compare(itemx.SubItems[_column].Text, itemy.SubItems[_column].Text);
                        break;
                    case ComparerMode.Integer:
                        int intx, inty;
                        int.TryParse(itemx.SubItems[_column].Text, out intx);
                        int.TryParse(itemy.SubItems[_column].Text, out inty);
                        result = intx.CompareTo(inty);
                        break;
                    case ComparerMode.Percent:
                        double doublex, doubley;
                        double.TryParse(itemx.SubItems[_column].Text.Replace("%", ""), out doublex);
                        double.TryParse(itemy.SubItems[_column].Text.Replace("%", ""), out doubley);
                        result = doublex.CompareTo(doubley);
                        break;
                }

                //降順の場合は符号を逆にする
                if (Order == SortOrder.Descending) result = -result;

                return result;
            }
        }
        #endregion

        //ボタンの切り替え
        private void ButtonSwitch(bool flag)
        {
            button_readrecent.Enabled = flag;
            button_refreshheader.Enabled = flag;
            button_search1.Enabled = flag;
            button_search2.Enabled = flag;
            button_search3.Enabled = flag;
        }

        //リストビューのアップデート
        private void UpdateListView(DropDataBase.DropDataSummarize summary)
        {
            var listitem = summary.Rows.Select(delegate(DropDataBase.DropDataSummarizeRow row)
            {
                ListViewItem item = new ListViewItem();
                var rowtext = row.MakeListViewRow();
                item.Text = rowtext[0];
                item.SubItems.AddRange(rowtext.Skip(1).ToArray());
                item.Tag = row.CorrespondingRecord;
                return item;
            });
            //リストビューの操作部分
            listView_output.BeginUpdate();
            foreach (var i in Enumerable.Range(0, Math.Min(summary.Headers.Length, listView_output.Columns.Count))) listView_output.Columns[i].Text = summary.Headers[i];//ヘッダー
            listView_output.Items.Clear();
            listView_output.Items.AddRange(listitem.ToArray());
            listView_output.EndUpdate();
        }

        //初期化用とかいうかヘッダー全部リセット
        public void ResetHeader()
        {
            //条件1 : エリアだけでOK
            _area = -1; _map = -1; _cell = -1; _localid = -1;
            var items = DropDataBase.Collection.MasterMapHeader.HeaderAreaFactory();
            comboBox_11.BeginUpdate();
            comboBox_11.Items.Clear();
            comboBox_11.Items.AddRange(items.Select(x => x.Value).ToArray());
            comboBox_11.Tag = items.Select(x => x.Key).ToArray();
            comboBox_11.SelectedIndex = 0;
            comboBox_11.EndUpdate();
            //条件2 : 船をリセット
            _ship = -1;
            comboBox_21.BeginUpdate();
            comboBox_21.Items.Clear();
            comboBox_21.Items.AddRange(Enumerable.Repeat("全て", 1).Concat(DropDataBase.Collection.MasterDropShipHeader.Values).ToArray());
            comboBox_21.Tag = Enumerable.Repeat(-1, 1).Concat(DropDataBase.Collection.MasterDropShipHeader.Keys).ToArray();
            comboBox_21.SelectedIndex = 0;
            comboBox_21.EndUpdate();
            //条件3 : アイテムをリセット
            _item = -1;
            comboBox_22.BeginUpdate();
            comboBox_22.Items.Clear();
            comboBox_22.Items.AddRange(Enumerable.Repeat("全て", 1).Concat(DropDataBase.Collection.MasterDropItemHeader.Values).ToArray());
            comboBox_22.Tag = Enumerable.Repeat(-1, 1).Concat(DropDataBase.Collection.MasterDropItemHeader.Keys).ToArray();
            comboBox_22.SelectedIndex = 0;
            comboBox_22.EndUpdate();
            //難易度のセット
            _difficulty = -1;
            var difitem = DropRecordCollection.MasterHeader.HeaderDifficultyFactory();
            comboBox_difficulty.BeginUpdate();
            comboBox_difficulty.Items.Clear();
            comboBox_difficulty.Items.AddRange(difitem.Select(x => x.Value).ToArray());
            comboBox_difficulty.Tag = difitem.Select(x => x.Key).ToArray();
            comboBox_difficulty.SelectedIndex = 0;
            comboBox_difficulty.EndUpdate();
        }

        //WinRankを作る
        private DropDataBase.WinRank MakeWinrank()
        {
            DropDataBase.WinRank winrank = DropDataBase.WinRank.None;
            if (checkBox_wins.Checked) winrank = winrank | DropDataBase.WinRank.S;
            if (checkBox_wina.Checked) winrank = winrank | DropDataBase.WinRank.A;
            if (checkBox_winb.Checked) winrank = winrank | DropDataBase.WinRank.B;
            if (checkBox_losec.Checked) winrank = winrank | DropDataBase.WinRank.C | DropDataBase.WinRank.D | DropDataBase.WinRank.E;
            return winrank;
        }

        //DateTimeのボタンへのアップデート
        private void DateSelectButtonUpdate()
        {
            DateTime newDate = new DateTime();

            if(startDate == newDate && endDate == newDate)
            {
                button_timeselect.Text = "期間指定 [なし]";
            }
            else
            {
                button_timeselect.Text = string.Format("期間指定 [{0}-{1}]",
                    startDate == newDate ? "" : startDate.ToString("M/d"),
                    endDate == newDate ? "" : endDate.ToString("M/d"));
            }
        }

        //イベントハンドラ
        private void comboBox_11_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選択されているインデックス
            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            if (idx == -1) return;
            //選択されている海域
            int area = (cb.Tag as int[])[idx];
            _area = area; _map = -1; _cell = -1; _localid = -1;
            //コンボボックスのアイテム
            IEnumerable<KeyValuePair<int, string>> items;
            if (area == -1) items = DropRecordCollection.MasterHeader.HeaderAllOnlyFactory();
            else items = DropDataBase.Collection.MasterMapHeader.ChildrenAreas[area].HeaderMapFactory();
            //コンボボックスに反映
            comboBox_12.BeginUpdate();
            comboBox_12.Items.Clear();
            comboBox_12.Items.AddRange(items.Select(x => x.Value).ToArray());
            comboBox_12.Tag = items.Select(x => x.Key).ToArray();
            comboBox_12.SelectedIndex = 0;
            comboBox_12.EndUpdate();
        }

        private void comboBox_12_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選択されているインデックス
            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            if (idx == -1) return;
            //選択されているマップ
            int map = (cb.Tag as int[])[idx];
            _map = map; _cell = -1; _localid = -1;
            //コンボボックスのアイテム
            IEnumerable<KeyValuePair<int, string>> items;
            if (_area == -1 || map == -1) items = DropRecordCollection.MasterHeader.HeaderAllOnlyFactory();
            else items = DropDataBase.Collection.MasterMapHeader.ChildrenAreas[_area].ChildrenMaps[map].HeaderCellFactory();
            //コンボボックスに反映
            comboBox_13.BeginUpdate();
            comboBox_13.Items.Clear();
            comboBox_13.Items.AddRange(items.Select(x => x.Value).ToArray());
            comboBox_13.Tag = items.Select(x => x.Key).ToArray();
            comboBox_13.SelectedIndex = 0;
            comboBox_13.EndUpdate();
        }

        private void comboBox_13_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選択されているインデックス
            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            if (idx == -1) return;
            //選択されているセル
            int cell = (cb.Tag as int[])[idx];
            _cell = cell; _localid = -1;
            //コンボボックスのアイテム
            IEnumerable<KeyValuePair<int, string>> items;
            if (_area == -1 || _map == -1 || cell == -1) items = DropRecordCollection.MasterHeader.HeaderAllOnlyFactory();
            else items = DropDataBase.Collection.MasterMapHeader.ChildrenAreas[_area].ChildrenMaps[_map].ChildrenCells[cell].HeaderFleetFactory();
            //コンボボックスに反映
            comboBox_14.BeginUpdate();
            comboBox_14.Items.Clear();
            comboBox_14.Items.AddRange(items.Select(x => x.Value).ToArray());
            comboBox_14.Tag = items.Select(x => x.Key).ToArray();
            comboBox_14.SelectedIndex = 0;
            comboBox_14.EndUpdate();
        }

        private void comboBox_14_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選択されているインデックス
            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            if (idx == -1) return;
            _localid = (cb.Tag as int[])[idx];
        }

        private void comboBox_21_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            if (idx == -1) return;
            _ship = (cb.Tag as int[])[idx];
        }

        private void comboBox_22_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            if (idx == -1) return;
            _item = (cb.Tag as int[])[idx];
        }

        private void comboBox_difficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            if(idx == -1) return;
            _difficulty = (cb.Tag as int[])[idx];
        } 


        //マップで検索
        private async void button_search1_Click(object sender, EventArgs e)
        {
            //ボタンを無効化
            ButtonSwitch(false);
            //結果の待機
            var summary = await Task.Factory.StartNew(() => DropDataBase.SummarizeByMap(_area, _map, _cell, _localid, checkBox_exceptdropcut.Checked, _difficulty, MakeWinrank(), startDate, endDate ));
            //件数の表示
            label_result.Text = string.Format("{0}/{1} 件がヒットしました", summary.Rows.Count, summary.Rows.Select(x => x.NumTotal).Sum());
            //ボタンの有効化
            ButtonSwitch(true);
            //リストビューに反映
            UpdateListView(summary);
        }

        //船で検索
        private async void button_search2_Click(object sender, EventArgs e)
        {
            //ボタンを無効化
            ButtonSwitch(false);
            //結果の待機
            var summary = await Task.Factory.StartNew(() => DropDataBase.SummarizeByShip(_ship, -1, checkBox_mergebycell.Checked, checkBox_exceptdropcut.Checked, _difficulty, MakeWinrank(), startDate, endDate));
            //件数の表示
            label_result.Text = string.Format("{0}/{1} 件がヒットしました", summary.Rows.Count, summary.Rows.Select(x => x.NumTotal).Sum());
            //ボタンの有効化
            ButtonSwitch(true);
            //リストビューに反映
            UpdateListView(summary);
        }

        //アイテムで検索
        private async void button_search3_Click(object sender, EventArgs e)
        {
            //ボタンを無効化
            ButtonSwitch(false);
            //結果の待機
            var summary = await Task.Factory.StartNew(() => DropDataBase.SummarizeByShip(-1, _item, checkBox_mergebycell.Checked, checkBox_exceptdropcut.Checked, _difficulty, MakeWinrank(), startDate, endDate));
            //件数の表示
            label_result.Text = string.Format("{0}/{1} 件がヒットしました", summary.Rows.Count, summary.Rows.Select(x => x.NumTotal).Sum());
            //ボタンの有効化
            ButtonSwitch(true);
            //リストビューに反映
            UpdateListView(summary);
        }

        private void button_refreshheader_Click(object sender, EventArgs e)
        {
            ResetHeader();
        }

        //直近100件
        private void button_readrecent_Click(object sender, EventArgs e)
        {
            var query = DropDataBase.Collection.DataBase.OrderByDescending(x => x.DropDate).Take(100);
            textBox_support.Text = string.Join(Environment.NewLine, query.Select(x => x.ToListViewString()));
            textBox_support.SelectionLength = 0;
        }

        //選択したアイテムの表示
        private void listView_output_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_output.SelectedItems.Count <= 0) return;
            var selected = listView_output.SelectedItems[0];
            var correspond = selected.Tag as List<DropRecord>;
            //右上テキストに表示
            textBox_support.Text = string.Join(Environment.NewLine, correspond.Select(x => x.ToListViewString()));
            textBox_support.SelectionLength = 0;
        }

        //列をクリック
        private void listView_output_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listViewItemSorter == null) return;

            listViewItemSorter.Column = e.Column;
            listView_output.Sort();
        }

        //リストビューをコピーするだけ
        private void button_copy_Click(object sender, EventArgs e)
        {
            if (listView_output.Items.Count == 0) return;

            StringBuilder sb = new StringBuilder();
            //ヘッダー
            var header = new CsvList<string>();
            foreach(int i in Enumerable.Range(0, listView_output.Columns.Count))
            {
                header.Add(listView_output.Columns[i].Text);
            };
            sb.AppendLine(string.Join("\t", header));
            //アイテム
            foreach(int i in Enumerable.Range(0, listView_output.Items.Count))
            {
                var row = new CsvList<string>();
                var item = listView_output.Items[i];
                foreach(int j in Enumerable.Range(0, item.SubItems.Count))
                {
                    row.Add(item.SubItems[j].Text);
                }
                sb.AppendLine(string.Join("\t", row));
            }
            //クリップボードにコピー
            Clipboard.SetText(sb.ToString());
        }

        //展開してコピー
        private void button_extract_Click(object sender, EventArgs e)
        {
            if (listView_output.Items.Count == 0) return;

            var records = Enumerable.Range(0, listView_output.Items.Count).Select(x => listView_output.Items[x].Tag)
                            .Cast<List<DropRecord>>().SelectMany(x => x)
                            .OrderBy(x => x.DropDate);
            var header = DropDataBase.Collection.MasterDropShipHeader;
            var itemheader = DropDataBase.Collection.MasterDropItemHeader;
            //テキストの取得
            string text = DropDataBase.MakeConvertText(records, header, itemheader);
            //MemoryStreamに変換
            byte[] bs = System.Text.Encoding.Default.GetBytes(text);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bs))
            {
                DataObject data = new DataObject(DataFormats.CommaSeparatedValue, ms);
                Clipboard.SetDataObject(data, true);
            }
        }

        //期間指定
        private void button_timeselect_Click(object sender, EventArgs e)
        {
            DropAnalyzerDateSelect ds = new DropAnalyzerDateSelect(startDate, endDate);

            //期間指定のフォームが閉じられたときのイベント
            FormClosingEventHandler closing = (ss, ee) =>
                {
                    var select = (DropAnalyzerDateSelect)ss;

                    //OKボタンが押されたときのみ
                    if (select.IsOk)
                    {
                        startDate = select.Start;
                        endDate = select.End;
                    }

                    DateSelectButtonUpdate();
                };

            ds.FormClosing += closing;

            ds.ShowDialog();
        }

        //編成詳細
        private void button_fleetdetail_Click(object sender, EventArgs e)
        {
            if (EnemyFleetDataBase.DataBase == null || APIMaster.MstShips == null) return;

            DropRecordCollection.MasterHeaderArea area;
            if(DropDataBase.Collection.MasterMapHeader.ChildrenAreas.TryGetValue(_area, out area))
            {
                DropRecordCollection.MasterHeaderMap map;
                if(area.ChildrenMaps.TryGetValue(_map, out map))
                {
                    DropRecordCollection.MasterHeaderCell cell;
                    if(map.ChildrenCells.TryGetValue(_cell, out cell))
                    {
                        var query = cell.ChildrenLocalFleets.Where(x => x.LocalID == _localid);
                        if(query.Count() > 0)
                        {
                            var item = query.First();
                            string text = item.EnemyFleetDetail(EnemyFleetDataBase.DataBase, APIMaster.MstShips, _area, _map, _cell, cell.EnemyFleetName, cell.IsBoss);
                            textBox_support.Text = text;
                        }
                    }
                }
            }
        }




    }
}
