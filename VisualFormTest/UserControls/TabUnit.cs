using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;

namespace VisualFormTest.UserControls
{
    public partial class TabUnit : UserControl, ITabControl
    {
        Label[] label_unit_num, label_unit_time;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        //イベントハンドラを動かさないフラグ
        public bool EventHandlerSuspend { get; set; }

        //クエリ
        public UnitQuery UsingQuery { get; set; }
        //フィルタ
        public UnitQueryFilter UsingFilter { get; set; }
        //条件にマッチした船のリスト
        public IEnumerable<ApiShip> QueriedShips { get; set; }
        //自動更新
        public bool AutoRefresh { get; set; }

        //ToolTipsの一覧
        public ToolStripMenuItem[][] MenuItemQuery { get; set; }

        //クエリが読まれたか
        public bool IsQueryLoaded { get; set; }

        //検索オプション
        public string ThresholdHour
        {
            get
            {
                int i = toolStripComboBox_hour.SelectedIndex;
                if (i == -1) return null;
                else return toolStripComboBox_hour.Items[i].ToString();
            }
        }

        public TabUnit()
        {
            InitializeComponent();

            CallBacks.EnableDoubleBuffering(listView_unit);
        }

        //艦娘タブの初期化
        public void Init()
        {
            if (!(this.FindForm() as WeifenLuo.WinFormsUI.Docking.DockContent).IsHandleCreated)
            {
                return;
            }

            //変数の設定
            label_unit_num = new Label[]
            {
                label_unit_num_1, label_unit_num_2, label_unit_num_3
            };
            label_unit_time = new Label[]
            {
                label_unit_time_1, label_unit_time_2, label_unit_time_3, label_unit_time_4
            };
            //メニューアイテムの作成
            MenuItemQuery = new ToolStripMenuItem[8][];
            foreach (int i in Enumerable.Range(0, MenuItemQuery.Length)) MenuItemQuery[i] = new ToolStripMenuItem[KancolleInfoUnitList.QueriesMax / MenuItemQuery.Length];
            foreach(int i in Enumerable.Range(0, KancolleInfoUnitList.QueriesMax))
            {
                ToolStripMenuItem item = new ToolStripMenuItem()
                {
                    Text = string.Format("({0})", i), Tag = i,
                };
                MenuItemQuery[i / MenuItemQuery[0].Length][i % MenuItemQuery[0].Length] = item;
            }
            toolStripMenuItem_q1.DropDownItems.AddRange(MenuItemQuery[0]); toolStripMenuItem_q2.DropDownItems.AddRange(MenuItemQuery[1]);
            toolStripMenuItem_q3.DropDownItems.AddRange(MenuItemQuery[2]); toolStripMenuItem_q4.DropDownItems.AddRange(MenuItemQuery[3]);
            toolStripMenuItem_q5.DropDownItems.AddRange(MenuItemQuery[4]); toolStripMenuItem_q6.DropDownItems.AddRange(MenuItemQuery[5]);
            toolStripMenuItem_q7.DropDownItems.AddRange(MenuItemQuery[6]); toolStripMenuItem_q8.DropDownItems.AddRange(MenuItemQuery[7]);
            //自動更新
            toolStripMenuItem_autorefresh.Click += new EventHandler(toolStripMenuItem_autorefresh_Click);
            if(!Config.TabKanmusuAutoRefreshDisable) toolStripMenuItem_autorefresh.PerformClick();
            //クエリの初期化
            UsingQuery = new UnitQuery();
            //フィルターの初期化
            UsingFilter = new UnitQueryFilter();

            InitFinished = true;
        }

        public void Init2()
        {
            if (!(this.FindForm() as WeifenLuo.WinFormsUI.Docking.DockContent).IsHandleCreated)
            {
                return;
            }

            //初期化
            KancolleInfoUnitList.Init(
                label_unit_num,
                label_unit_time,
                listView_unit
            );
            if (KancolleInfoUnitList.Queries == null)
            {
                if (APIPort.Basic != null && APIPort.Basic.api_member_id != null)
                    KancolleInfoUnitList.InitQuery();//テスト
            }
            //--イベントハンドラー
            //クエリ
            ToolStripMenuItem[] menuitems = new ToolStripMenuItem[]
            {
                toolStripMenuItem_q1, toolStripMenuItem_q2, toolStripMenuItem_q3, toolStripMenuItem_q4,
                toolStripMenuItem_q5, toolStripMenuItem_q6, toolStripMenuItem_q7, toolStripMenuItem_q8,
            };
            foreach(ToolStripMenuItem x in menuitems)
            {
                x.Tag = this;//イベントハンドラーからこのフォームを参照できないのでタグで引き渡す
                x.DropDownItemClicked += new ToolStripItemClickedEventHandler(KancolleInfoUnitList.ToolStripMenuItem_Query_DropDownItemClicked);
            }
            //フィルター
            toolStripComboBox_hour.Tag = this;
            toolStripMenuItem_filter.DropDownItemClicked += new ToolStripItemClickedEventHandler(KancolleInfoUnitList.ToolStripMenuItem_Filter_DropDownItemClicked);
            toolStripComboBox_hour.SelectedIndexChanged += new EventHandler(KancolleInfoUnitList.ToolStripComboBox_Filter_SelectedIndexChanged);
            //編集
            toolStripMenuItem_edit.Click += new EventHandler(toolStripMenuItem_edit_Click);
            //メニューストリップの名前の更新
            StripNameRefresh();

            Init2Finished = true;
            
        }

        //クエリ0を読ませる
        /*
        public void ReadQueryZero()
        {
            if (!this.IsHandleCreated) return;
            KancolleInfoUnitList.ToolStripMenuItem_Query_DropDownItemClicked(toolStripMenuItem_q1, new ToolStripItemClickedEventArgs(toolStripMenuItem_q1.DropDownItems[0]));
            IsQueryLoaded = true;
        }*/

        //任意の番号のクエリを読ませる
        public void ReadQueryByNumber(int queryNumber)
        {
            if (!this.IsHandleCreated) return;
            int queryRow = queryNumber / MenuItemQuery[0].Length;
            int queryColumn = queryNumber % MenuItemQuery[0].Length;
            ToolStripMenuItem toolStrip = null;
            switch(queryRow)
            {
                case 0: toolStrip = toolStripMenuItem_q1; break;
                case 1: toolStrip = toolStripMenuItem_q2; break;
                case 2: toolStrip = toolStripMenuItem_q3; break;
                case 3: toolStrip = toolStripMenuItem_q4; break;
                case 4: toolStrip = toolStripMenuItem_q5; break;
                case 5: toolStrip = toolStripMenuItem_q6; break;
                case 6: toolStrip = toolStripMenuItem_q7; break;
                case 7: toolStrip = toolStripMenuItem_q8; break;
            }
            KancolleInfoUnitList.ToolStripMenuItem_Query_DropDownItemClicked(toolStrip, new ToolStripItemClickedEventArgs(toolStrip.DropDownItems[queryColumn]));
            IsQueryLoaded = true;

        }

        //名前の更新
        public void StripNameRefresh()
        {
            if (!KancolleInfoUnitList.IsInited) return;
            List<ToolStripMenuItem> strips = new List<ToolStripMenuItem>();
            strips.AddRange(toolStripMenuItem_q1.DropDownItems.OfType<ToolStripMenuItem>());
            strips.AddRange(toolStripMenuItem_q2.DropDownItems.OfType<ToolStripMenuItem>());
            strips.AddRange(toolStripMenuItem_q3.DropDownItems.OfType<ToolStripMenuItem>());
            strips.AddRange(toolStripMenuItem_q4.DropDownItems.OfType<ToolStripMenuItem>());
            strips.AddRange(toolStripMenuItem_q5.DropDownItems.OfType<ToolStripMenuItem>());
            strips.AddRange(toolStripMenuItem_q6.DropDownItems.OfType<ToolStripMenuItem>());
            strips.AddRange(toolStripMenuItem_q7.DropDownItems.OfType<ToolStripMenuItem>());
            strips.AddRange(toolStripMenuItem_q8.DropDownItems.OfType<ToolStripMenuItem>());
            //名前の更新
            foreach(int i in Enumerable.Range(0, strips.Count))
            {
                UnitQuery q = KancolleInfoUnitList.Queries[i];
                CallBacks.SetToolStripMenuItemText(strips[i], menuStrip1, string.Format("({0}){1}", q.ID, q.Name));
            }
        }

        //JSONの更新
        public void JsonUpdate()
        {
            //クエリ名
            CallBacks.SetLabelText(label_unit_queryname, string.Format("({0}){1}", UsingQuery.ID, UsingQuery.Name));
            //JSON
            CallBacks.SetTextBoxText(textBox_unit_json, UsingQuery.ToJson());
        }

        //結果の表示
        public void SetResult(int num, long ms)
        {
            CallBacks.SetLabelText(label_unit_result, string.Format("{0} 件検索 ({1}ms)", num, ms));
        }

        //クエリのチェックの切り替え
        public void MenuItemQuery_CheckedChange(int id)
        {
            foreach(int i in Enumerable.Range(0, MenuItemQuery.Length))
            {
                foreach(int j in Enumerable.Range(0, MenuItemQuery[i].Length))
                {
                    int x = i * MenuItemQuery[i].Length + j;
                    if(x != id)
                    {
                        //チェックを外す
                        if (MenuItemQuery[i][j].Checked) CallBacks.SetToolStripMenuItemChecked(
                            MenuItemQuery[i][j], MenuItemQuery[i][j].GetCurrentParent(), false);
                    }
                    else
                    {
                        //チェックをオン
                        CallBacks.SetToolStripMenuItemChecked(MenuItemQuery[i][j], MenuItemQuery[i][j].GetCurrentParent(), true);
                    }
                }
            }
        }

        //艦娘タブの更新
        public void TabUnit_ListViewUpdate_Q()
        {
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    Cursor.Current = Cursors.AppStarting;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    //クエリの実行
                    QueriedShips = KancolleInfoUnitList.DoQuery(UsingQuery, UsingFilter.NotShowFleetAssignFlag);
                    //リストビューの変更
                    KancolleInfoUnitList.DoIt(
                        label_unit_num,
                        label_unit_time,
                        listView_unit, QueriedShips, UsingFilter
                    );
                    //JSONの更新
                    JsonUpdate();
                    sw.Stop();
                    //結果の表示
                    SetResult(QueriedShips.Count(), (long)sw.ElapsedMilliseconds);
                    Cursor.Current = Cursors.Default;
                }));
        }

        //フィルターの更新
        public void TabUnit_FilterUpdate()
        {
            UsingFilter.NotShowFleetAssignFlag = toolStripMenuItem_assign.Checked;
            UsingFilter.NotShowOverThresholdHour = toolStripMenuItem_time.Checked;
            UsingFilter.NotShowSmallDamage = toolStripMenuItem_small.Checked;
            string hour = ThresholdHour;
            if(hour != null)
            {
                UsingFilter.ThresholdHour = Convert.ToDouble(hour.Replace("時間", ""));
            }
        }

        //艦娘タブの計算
        public void TabUnit_TimeUpdate()
        {
            KancolleInfoUnitList.RefreshRepairTime(
                label_unit_num,
                label_unit_time,
                QueriedShips, UsingFilter
            );
        }

        //イベントハンドラー
        private void toolStripMenuItem_edit_Click(object sender, EventArgs e)
        {
            QueryEdit edit = new QueryEdit();
            //アクセサの付与
            edit.TabCollection = (this.FindForm() as DockingWindows.DockWindowTabPage).Collection;
            edit.ShowDialog();
        }

        private void toolStripMenuItem_autorefresh_Click(object sender, EventArgs e)
        {
            AutoRefresh = !AutoRefresh;
            Config.TabKanmusuAutoRefreshDisable = !AutoRefresh;
            if (AutoRefresh) toolStripMenuItem_autorefresh.Text = Helper.CheckString + "自動更新";
            else toolStripMenuItem_autorefresh.Text = "自動更新";
        }

        private void TabUnit_DoubleClick(object sender, EventArgs e)
        {
            if (!IsQueryLoaded) return;
            TabUnit_ListViewUpdate_Q();
        }
    }
}
