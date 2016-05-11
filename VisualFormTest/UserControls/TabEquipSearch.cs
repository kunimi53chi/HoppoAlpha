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

namespace VisualFormTest.UserControls
{
    public partial class TabEquipSearch : UserControl, ITabControl
    {
        public Control[] control_eqsearch;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        private ListViewItemComparer listviewSorter;

        public TabEquipSearch()
        {
            InitializeComponent();
        }

        public void Init()
        {
            control_eqsearch = new Control[]
            {
                comboBox_eqsearch_main, comboBox_eqsearch_sub, button_eqsearch_do, listView_eqsearch,
                label_eqsearch_time, button_eqsearch_status, checkBox_eqsearch,
            };

            InitFinished = true;
        }

        private void TabEquipSearch_Load(object sender, EventArgs e)
        {
            listviewSorter = new ListViewItemComparer();
            listviewSorter.ColumnModes = new ListViewItemComparer.ComparerMode[]
            {
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.Level,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.Integer,
            };
            listView_eqsearch.ListViewItemSorter = listviewSorter;
            //ソートのイベントハンドラー
            listView_eqsearch.ColumnClick += (ss, ee) =>
                {
                    listviewSorter.Column = ee.Column;
                    listView_eqsearch.Sort();
                };
        }

        public void Init2()
        {
            if (!(this.FindForm() as WeifenLuo.WinFormsUI.Docking.DockContent).IsHandleCreated) return;

            //Init
            KancolleInfoSlotitemSearch.Init(control_eqsearch);
            //イベントハンドラー
            //parent_combobox
            comboBox_eqsearch_main.SelectedIndexChanged +=
                new EventHandler(KancolleInfoSlotitemSearch.SlotitemSearch_ParentComboBox_SelectedIndexChanged);
            //sub_combobox
            comboBox_eqsearch_sub.SelectedIndexChanged +=
                new EventHandler(KancolleInfoSlotitemSearch.SlotitemSearch_SubComboBox_SelectedIndexChanged);
            //コンボボックスを反転しないように
            comboBox_eqsearch_main.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            comboBox_eqsearch_sub.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            //チェックボックス
            checkBox_eqsearch.CheckedChanged +=
                new EventHandler(KancolleInfoSlotitemSearch.SlotitemSearch_CheckBox_CheckedChanged);
            //button
            button_eqsearch_do.Click +=
                new EventHandler(KancolleInfoSlotitemSearch.SlotitemSearch_Button_Click);
            button_eqsearch_status.Click +=
                new EventHandler(KancolleInfoSlotitemSearch.SlotitemSearch_ShowButton_Click);
            //Listview
            listView_eqsearch.SelectedIndexChanged +=
                new EventHandler(KancolleInfoSlotitemSearch.SlotitemSearch_Listview_SelectedIndexChanged);

            Init2Finished = true;
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        #region 内部クラス（リストビューのソート
        public class ListViewItemComparer : IComparer
        {
            public enum ComparerMode
            {
                String, Integer, Level,
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
                    if (_column == value)
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
                if (this.Order == SortOrder.None)
                {
                    return 0;
                }

                int result = 0;
                ListViewItem itemx = (ListViewItem)x;
                ListViewItem itemy = (ListViewItem)y;

                //並べ替えモード
                if (ColumnModes != null && ColumnModes.Length > _column)
                {
                    Mode = ColumnModes[_column];
                }
                //並べ替え方法別にxとyを比較
                switch (Mode)
                {
                    case ComparerMode.String:
                        result = string.Compare(itemx.SubItems[_column].Text, itemy.SubItems[_column].Text);
                        break;
                    case ComparerMode.Integer:
                        int intx = GetLevel(itemx.SubItems[_column].Text);
                        int inty = GetLevel(itemy.SubItems[_column].Text);
                        result = intx.CompareTo(inty);
                        break;
                    case ComparerMode.Level:
                        int lx, ly;

                        int.TryParse(itemx.SubItems[_column].Text.Replace("◆", "").Replace("★", ""), out lx);
                        int.TryParse(itemy.SubItems[_column].Text.Replace("◆", "").Replace("★", ""), out ly);
                        if (itemx.SubItems[_column].Text.Contains("◆")) lx += 10;
                        if (itemy.SubItems[_column].Text.Contains("◆")) ly += 10;
                        result = lx.CompareTo(ly);
                        break;
                }

                //降順の場合は符号を逆にする
                if (Order == SortOrder.Descending) result = -result;

                return result;
            }

            //★や◆つきの文字からレベルを復元
            private int GetLevel(string levelstr)
            {
                //改修レベル
                int reinforce = 0;
                var reinreg = System.Text.RegularExpressions.Regex.Match(levelstr, "★[0-9]+");
                if(reinreg.Success)
                {
                    int.TryParse(reinreg.Value, out reinforce);
                }
                //熟練度
                int training = 0;
                var trainingreg = System.Text.RegularExpressions.Regex.Match(levelstr, "◆[0-9]+");
                if(trainingreg.Success)
                {
                    int.TryParse(trainingreg.Value, out training);
                }
                //改修レベル×10＋熟練度
                return reinforce * 10 + training;
            }
        }
        #endregion


    }
}
