using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest.UserControls
{
    public partial class SortieReportViewer : UserControl
    {
        //public SortieReportShipHashIntegrateMode integrate = SortieReportShipHashIntegrateMode.MaskShipTypeDescending;
        //--期間のツリービュー
        //ALL
        private List<FileMainItem> terms = new List<FileMainItem>();
        //term依存
        private SortieReportCollection collection = null;
        private List<SortieReportMapHash> maps = null;
        //map依存
        private SortieReport report = null;
        private Dictionary<SortieReportHash, SortieReportItem> sortieIntegrated = null;
        //fleet依存
        private SortieReportItem reportitem = null;

        private bool inited = false;

        //色一覧
        private List<Color> menuColors = null;

        //ボタンのデフォルトの文字
        private string butStrShiptype = "艦種でまとめる";
        private string butStrChara = "キャラでまとめる";
        private string butStrSlotitem = "装備を区別する";
        private string butStrOrder = "並び順を区別しない";
        private string butStrWeek = "週単位でまとめる";
        private string butStrMonth = "月単位でまとめる";
        private string butStrYear = "年単位でまとめる";
        private string butStrAll = "全ての期間を表示";

        public bool IsShown { get; set; }

        public SortieReportViewer()
        {
            InitializeComponent();

            if (APIPort.Basic != null && !inited) Init();

            //統合モードのボタンの表示
            button_integrate_button_update();
            //期間のモードのボタンの表示
            button_term_update();
            //前景色切り替えのメニューアイテム
            toolStripMenuItem_foreColor.DropDownItems.AddRange(ColorMenuFactory());
            toolStripMenuItem_backColor.DropDownItems.AddRange(ColorMenuFactory());
            toolStripMenuItem_foreColor.DropDownItemClicked += new ToolStripItemClickedEventHandler(menuColor_DropDownItemClicked);
            toolStripMenuItem_backColor.DropDownItemClicked += new ToolStripItemClickedEventHandler(menuColor_DropDownItemClicked);
            //表示色の変更
            Color foreColor = Color.FromName(Config.SortieReportViewForeColor);
            Color backColor = Color.FromName(Config.SortieReportViewBackColor);
            toolStripMenuItem_backColor.DropDownItems[Math.Max(menuColors.IndexOf(backColor), 0)].PerformClick();
            toolStripMenuItem_foreColor.DropDownItems[Math.Max(menuColors.IndexOf(foreColor), 0)].PerformClick();
        }

        public void Init()
        {
            //フォームのハンドルが作られていない場合
            var form = treeView_file.FindForm();
            if (form == null) return;

            //ファイル一覧の更新
            RefreshTermTreeView();
            inited = true;
        }

        //メニューアイテム
        #region メニューアイテム関連
        private ToolStripMenuItem[] ColorMenuFactory()
        {
            //色一覧の取得
            if (menuColors == null)
            {
                menuColors = new List<Color>();
                foreach(var info in typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static))
                {
                    menuColors.Add((Color)info.GetValue(null, null));
                }
            }

            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
            //メニューアイテムを作る
            foreach(var c in menuColors)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = c.Name;
                item.Tag = c;

                //アイコンの作成
                Bitmap img = new Bitmap(16, 16);
                using(var g = Graphics.FromImage(img))
                {
                    using(var b = new SolidBrush(c))
                    {
                        g.FillRectangle(b, 0, 0, img.Width, img.Height);
                    }
                }
                item.Image = img;

                items.Add(item);
            }

            return items.ToArray();
        }

        //メニューアイテムが選択されたときのイベント
        private void menuColor_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            Color selectedColor = (Color)e.ClickedItem.Tag;
            if (selectedColor == Color.Transparent) return;
            
            if(item.Name == "toolStripMenuItem_foreColor")
            {
                textBox_report.ForeColor = selectedColor;
                Config.SortieReportViewForeColor = selectedColor.Name;
            }
            else if(item.Name == "toolStripMenuItem_backColor")
            {
                textBox_report.BackColor = selectedColor;
                Config.SortieReportViewBackColor = selectedColor.Name;
            }

            foreach(var i in item.DropDownItems.OfType<ToolStripMenuItem>())
            {
                if (i == e.ClickedItem) i.Checked = true;
                else i.Checked = false;
            }
        }
        #endregion

        //内部クラス
        #region 内部クラス
        //期間ノードに格納するアイテム（新）
        public class FileSubItem
        {
            public string FilePath { get; private set; }
            public int Year { get; private set; }
            public int WeeklyIndex { get; private set; }
            public DateTime StartDate { get; private set; }
            public DateTime EndDate { get; private set; }
            public int Hash
            {
                get
                {
                    return Year * 100 + WeeklyIndex;
                }
            }

            public bool SetValues(string filePath)
            {
                if (!filePath.Contains("sortie")) return false;

                //年と週番号の取得
                string[] split = Path.GetFileNameWithoutExtension(filePath).Replace("sortie", "").Split('_');
                if (split.Length != 2) return false;
                int year;
                if (!int.TryParse(split[0], out year)) return false;
                int weeklyindex;
                if (!int.TryParse(split[1], out weeklyindex)) return false;

                this.FilePath = filePath;
                this.Year = year;
                this.WeeklyIndex = weeklyindex;

                this.StartDate = SortieReportCollection.Helper.GetWeeklyMinDate(year, weeklyindex);
                this.EndDate = SortieReportCollection.Helper.GetWeeklyMaxDate(year, weeklyindex);
                return true;
            }
        }

        public class FileMainItem
        {
            public List<FileSubItem> Files { get; private set; }
            public DateTime StartDate { get; private set; }
            public DateTime EndDate { get; private set; }
            public string Display {get; private set;}
            
            public FileMainItem()
            {
                Files = new List<FileSubItem>();
            }

            //ソート用のハッシュ
            public int SortHash
            {
                get
                {
                    return StartDate.Year * 1000 + StartDate.DayOfYear;
                }
            }

            public bool SetValues(List<FileSubItem> subItems, string displayKey)
            {
                this.Files = subItems;
                this.Display = displayKey;

                if(Files.Count > 0)
                {
                    this.StartDate = Files[0].StartDate;
                    this.EndDate = Files[this.Files.Count - 1].EndDate;

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void AddValue(FileSubItem subItem)
            {
                this.Files.Add(subItem);

                if (subItem.StartDate < this.StartDate) this.StartDate = subItem.StartDate;
                if (subItem.EndDate > this.EndDate) this.EndDate = subItem.EndDate;
            }
        }
        #endregion

        //更新関連のメソッド
        #region 更新関連のメソッド
        //期間のツリービューの更新
        public void RefreshTermTreeView()
        {
            if (APIPort.Basic == null) return;
            //ルートディレクトリ
            string dir = Environment.CurrentDirectory + @"/user/" + APIPort.Basic.api_member_id + @"/sortie";

            if (!Directory.Exists(dir)) return;

            //ファイル一覧の取得
            terms = new List<FileMainItem>();
            collection = null; maps = null;
            report = null; sortieIntegrated = null;
            reportitem = null;
            //とりあえずファイルリストを作る
            var files = new List<string>();
            foreach (var x in Directory.GetDirectories(dir))
            {
                foreach(var f in Directory.GetFiles(x))
                {
                    if (f.Contains("sortie")) files.Add(f);
                }
            }
            //期間分けに応じて分類する
            var filedic = new Dictionary<string, List<FileSubItem>>();
            foreach(var f in files)
            {
                var sub = new FileSubItem();
                if (!sub.SetValues(f)) continue;

                var keys = SortieReportCollection.Helper.IntegrateYearAndWeeklyIndex(sub.Year, sub.WeeklyIndex, Config.SortieReportTermMode);
                foreach(var k in keys)
                {
                    List<FileSubItem> flist;
                    if (!filedic.TryGetValue(k, out flist)) flist = new List<FileSubItem>();
                    flist.Add(sub);

                    filedic[k] = flist;
                }
            }
            //MainItemへの変換
            foreach(var d in filedic)
            {
                var main = new FileMainItem();
                if (!main.SetValues(d.Value, d.Key)) continue;

                terms.Add(main);
            }

            //最新のファイルが含まれていない場合
            DateTime now = DateTime.Now;
            int latesthash = SortieReportCollection.Helper.GetNowWeeklyIndexHash(now);
            int[] hashs = terms.SelectMany(x => x.Files).Select(x => x.Hash).ToArray();
            if (Array.IndexOf(hashs, latesthash) == -1)
            {
                var latest_sub = new FileSubItem();
                latest_sub.SetValues(SortieReportDataBase.GetFileName(now));
                var latest_file = new List<FileSubItem>();
                latest_file.Add(latest_sub);

                var latest_keys = SortieReportCollection.Helper.IntegrateYearAndWeeklyIndex(now.Year, SortieReportCollection.Helper.GetNowWeeklyIndex(now), Config.SortieReportTermMode);
                foreach(var key in latest_keys)
                {
                    var latest_main = terms.Where(x => x.Display == key).FirstOrDefault();
                    if(latest_main == null)
                    {
                        latest_main = new FileMainItem();
                        latest_main.SetValues(latest_file, key);
                        terms.Add(latest_main);
                    }
                    else
                    {
                        latest_main.AddValue(latest_sub);
                    }
                }
            }

            //降順ソート
            terms = terms.OrderByDescending(x => x.SortHash).ToList();

            //ツリービューの更新
            //ここだけ別スレッドで叩かれるのでコールバックにする
            Action act = () =>
                {
                    treeView_file.SuspendLayout();
                    treeView_file.Nodes.Clear();
                    var nodes = terms.Select(x => new TreeNode(x.Display)).ToArray();
                    treeView_file.Nodes.AddRange(nodes);
                    treeView_file.ResumeLayout();
                    //子ツリーの消去
                    treeView_map.Nodes.Clear();
                    treeView_fleet.Nodes.Clear();
                };
            if(treeView_file.InvokeRequired)
            {
                treeView_file.Invoke(act);
            }
            else
            {
                act();
            }
        }

        //マップのツリービューの更新
        public void RefreshMapTreeView(FileMainItem selectedTerm)
        {
            if (selectedTerm == null) return;

            collection = null;
            var mergeTargets = new List<SortieReportCollection>();
            int latest = SortieReportCollection.Helper.GetNowWeeklyIndexHash(DateTime.Now);
            foreach(var f in selectedTerm.Files)
            {
                //最新の場合は？
                if(f.Hash == latest)
                {
                    if (SortieReportDataBase.Reports == null) return;

                    if (collection == null) collection = SortieReportDataBase.Reports.DeepCopy();
                    else mergeTargets.Add(SortieReportDataBase.Reports);
                }

                //最新でない場合、ファイルを読み込み
                var loadResult = Files.TryLoad(f.FilePath, DataType.SortieReport);
                if (!loadResult.IsSuccess) continue;

                if (collection == null) collection = (SortieReportCollection)loadResult.Instance;
                else mergeTargets.Add((SortieReportCollection)loadResult.Instance);
            }
            if (collection == null) return;
            if(mergeTargets.Count > 0)
            {
                //複数ファイルをマージする
                collection = collection.MergeMany(mergeTargets);
            }

            //子ツリーの消去
            report = null; sortieIntegrated = null;
            reportitem = null;
            treeView_fleet.Nodes.Clear();
            //昇順ソート
            maps = collection.Collection.Keys.OrderBy(x => x.GetHashCode()).ToList();

            //ツリービューの更新
            treeView_map.SuspendLayout();
            treeView_map.Nodes.Clear();
            var nodes = maps.Select(x => new TreeNode(x.Display())).ToArray();
            treeView_map.Nodes.AddRange(nodes);
            treeView_map.ResumeLayout();

            //選択テキストの更新
            RefreshSelectTextBox();
        }

        //編成のツリービューの更新
        public void RefreshFleetTreeView(SortieReportMapHash selectedMap)
        {
            if (collection == null) return;
            //出撃ログの取得
            if (!collection.Collection.TryGetValue(selectedMap, out report)) return;
            //出撃ログを統合する
            reportitem = null;
            sortieIntegrated = new Dictionary<SortieReportHash,SortieReportItem>();
            foreach(var s in report.Sorties.Values)
            {
                //キーをマスクする
                var masked_key = s.Deck.Mask(Config.SortieReportIntegrateMode);
                //マスクしたキーで登録
                SortieReportItem item;
                if(sortieIntegrated.TryGetValue(masked_key, out item))
                {
                    item = item.Integrate(s);
                }
                else
                {
                    item = s;
                }

                sortieIntegrated[masked_key] = item;
            }

            //ツリービューの更新
            treeView_fleet.SuspendLayout();
            treeView_fleet.Nodes.Clear();
            var nodes = sortieIntegrated.Keys.Select(x => new TreeNode(x.Fleet.DisplayTreeView(Config.SortieReportIntegrateMode, APIMaster.MstShips, APIMaster.MstStypesDictionary))).ToArray();
            treeView_fleet.Nodes.AddRange(nodes);
            treeView_fleet.ResumeLayout();

            //選択テキストの更新
            RefreshSelectTextBox();
        }

        //出撃報告書のテキストボックスの更新
        public void RefreshReportTextBox(SortieReportHash hash)
        {
            if (sortieIntegrated == null || report == null) return;
            //報告書アイテムの設定
            if (!sortieIntegrated.TryGetValue(hash, out reportitem)) return;

            //テキストボックスの更新
            textBox_report.Text = reportitem.Display(Config.SortieReportIntegrateMode, APIMaster.MstShips, APIMaster.MstStypesDictionary, APIMaster.MstSlotitems, report.Map);

            //選択テキストの更新
            RefreshSelectTextBox();
        }

        //ツリービューの選択状態を表すテキストボックスの更新
        public void RefreshSelectTextBox()
        {
            StringBuilder sb = new StringBuilder();
            //選択されたファイル
            if (collection == null)
            {
                textBox_selected.Text = sb.ToString();
                return;
            }
            switch(Config.SortieReportTermMode)
            {
                case SortieReportTermIntegrateMode.Week:
                    sb.AppendFormat("{0}～{1} ({2}年{3}週)", collection.StartTime.ToShortDateString(), collection.EndTime.ToShortDateString(),
                        collection.StartTime.Year, collection.WeeklyIndex);
                    break;
                case SortieReportTermIntegrateMode.Month:
                    string keydate;
                    //9月：8/31～10/2のように2ヶ月またいでいる場合は中間の月
                    if (collection.StartTime.AddMonths(2).Month == collection.EndTime.Month)
                    {
                        keydate = string.Format("{0}年{1}月", collection.StartTime.Year, collection.StartTime.Month);
                    }
                    //それ以外の場合はより多い日数の月を選択
                    else
                    {
                        var basedate = new DateTime(collection.StartTime.Year, collection.StartTime.Month, DateTime.DaysInMonth(collection.StartTime.Year, collection.StartTime.Month));//計算機準備
                        var first = basedate - collection.StartTime;//前月として認識した場合の日数
                        var second = collection.EndTime - basedate;//当月として認識した場合の日数
                        if (first.Days > second.Days) keydate = string.Format("{0}年{1}月", collection.StartTime.Year, collection.StartTime.Month);
                        else keydate = string.Format("{0}年{1}月", collection.EndTime.Year, collection.EndTime.Month);
                    }
                    sb.AppendFormat("{0}～{1}（{2}）", collection.StartTime.ToShortDateString(), collection.EndTime.ToShortDateString(), keydate);
                    break;
                case SortieReportTermIntegrateMode.Year:
                    sb.AppendFormat("{0}～{1}（{2}年）", collection.StartTime.ToShortDateString(), collection.EndTime.ToShortDateString(), collection.StartTime.Year);
                    break;
                case SortieReportTermIntegrateMode.All:
                    sb.AppendFormat("{0}～{1}（全て）", collection.StartTime.ToShortDateString(), collection.EndTime.ToShortDateString());
                    break;
            }
            sb.AppendLine();
            //選択されたマップ
            if(report == null)
            {
                textBox_selected.Text = sb.ToString();
                return;
            }
            sb.Append("-> " + report.Map.Display());
            sb.AppendLine();
            //選択された編成
            if (reportitem == null)
            {
                textBox_selected.Text = sb.ToString();
                return;
            }
            sb.AppendFormat("-> {0}", reportitem.Deck.Fleet.DisplayTreeView(Config.SortieReportIntegrateMode, APIMaster.MstShips, APIMaster.MstStypesDictionary));
            sb.AppendLine();
            if(reportitem.Deck.FleetCombined.Ship1.MasterShipID != 0)
            {
                //連合艦隊の場合
                sb.AppendFormat("  {0}", reportitem.Deck.FleetCombined.DisplayTreeView(Config.SortieReportIntegrateMode, APIMaster.MstShips, APIMaster.MstStypesDictionary));
                sb.AppendLine();
            }

            textBox_selected.Text = sb.ToString();
        }
        #endregion

        #region イベントハンドラ
        //ファイル一覧のツリービューのセレクトイベント
        private void treeView_file_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = treeView_file.Nodes.IndexOf(e.Node);
            if (index < 0 || index >= terms.Count) return;

            RefreshMapTreeView(terms[index]);
        }

        //マップ一覧のツリービューのセレクトイベント
        private void treeView_map_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = treeView_map.Nodes.IndexOf(e.Node);
            if (maps == null || index < 0 || index >= maps.Count) return;

            RefreshFleetTreeView(maps[index]);
        }

        //艦隊一覧のツリービューのセレクトイベント
        private void treeView_fleet_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = treeView_fleet.Nodes.IndexOf(e.Node);
            if (sortieIntegrated == null || index < 0 || index >= sortieIntegrated.Count) return;

            RefreshReportTextBox(sortieIntegrated.Keys.ElementAt(index));
        }
        
        //まとめるボタンのテキストのアップデート
        private void button_integrate_button_update()
        {
            if (Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._ShipType)) button_integrate_shiptype.Text = Helper.CheckString + butStrShiptype;
            else button_integrate_shiptype.Text = butStrShiptype;

            if (Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._ShipId)) button_integrate_chara.Text = Helper.CheckString + butStrChara;
            else button_integrate_chara.Text = butStrChara;

            if (Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._Slotitem)) button_integrate_slotitem.Text = Helper.CheckString + butStrSlotitem;
            else button_integrate_slotitem.Text = butStrSlotitem;

            if (Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._Descending)) button_integrate_orderby.Text = Helper.CheckString + butStrOrder;
            else button_integrate_orderby.Text = butStrOrder;
        }

        //艦種でまとめるのチェックイベント
        private void button_integrate_shiptype_Click(object sender, EventArgs e)
        {
            //ボタンの表示の更新
            if(Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._Descending))
            {
                Config.SortieReportIntegrateMode = SortieReportShipHashIntegrateMode._ShipType | SortieReportShipHashIntegrateMode._Descending;
            }
            else
            {
                Config.SortieReportIntegrateMode = SortieReportShipHashIntegrateMode._ShipType;
            }
            button_integrate_button_update();
            //艦隊表示の更新
            if (collection != null && treeView_map.SelectedNode != null)
            {
                treeView_map_AfterSelect(treeView_map, new TreeViewEventArgs(treeView_map.SelectedNode));
            }
        }

        //キャラでまとめるのチェックイベント
        private void button_integrate_chara_Click(object sender, EventArgs e)
        {
            //ボタンの表示の更新
            if (Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._Descending))
            {
                Config.SortieReportIntegrateMode = SortieReportShipHashIntegrateMode._ShipId | SortieReportShipHashIntegrateMode._Descending;
            }
            else
            {
                Config.SortieReportIntegrateMode = SortieReportShipHashIntegrateMode._ShipId;
            }
            button_integrate_button_update();
            //艦隊表示の更新
            if (collection != null && treeView_map.SelectedNode != null)
            {
                treeView_map_AfterSelect(treeView_map, new TreeViewEventArgs(treeView_map.SelectedNode));
            }
        }

        //装備を区別するのチェックイベント
        private void button_integrate_slotitem_Click(object sender, EventArgs e)
        {
            //ボタンの表示の更新
            if (Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._Descending))
            {
                Config.SortieReportIntegrateMode = SortieReportShipHashIntegrateMode._Slotitem | SortieReportShipHashIntegrateMode._Descending;
            }
            else
            {
                Config.SortieReportIntegrateMode = SortieReportShipHashIntegrateMode._Slotitem;
            }
            button_integrate_button_update();
            //艦隊表示の更新
            if (collection != null && treeView_map.SelectedNode != null)
            {
                treeView_map_AfterSelect(treeView_map, new TreeViewEventArgs(treeView_map.SelectedNode));
            }
        }

        //並び順を区別しないチェックイベント
        private void button_integrate_orderby_Click(object sender, EventArgs e)
        {
            //ボタンの表示の更新
            if (Config.SortieReportIntegrateMode.HasFlag(SortieReportShipHashIntegrateMode._Descending))
            {
                Config.SortieReportIntegrateMode = Config.SortieReportIntegrateMode & ~SortieReportShipHashIntegrateMode._Descending;//取り除く
            }
            else
            {
                Config.SortieReportIntegrateMode = Config.SortieReportIntegrateMode | SortieReportShipHashIntegrateMode._Descending;
            }
            button_integrate_button_update();
            //艦隊表示の更新
            if (collection != null && treeView_map.SelectedNode != null)
            {
                treeView_map_AfterSelect(treeView_map, new TreeViewEventArgs(treeView_map.SelectedNode));
            }
        }

        //メニューの更新ボタン
        private void toolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            treeView_fleet.Nodes.Clear();
            treeView_map.Nodes.Clear();
            treeView_file.Nodes.Clear();

            RefreshTermTreeView();
        }

        //期間の更新ボタン
        private void button_term_Click(object sender, EventArgs e)
        {
            if (sender == button_term_week) Config.SortieReportTermMode = SortieReportTermIntegrateMode.Week;
            else if (sender == button_term_month) Config.SortieReportTermMode = SortieReportTermIntegrateMode.Month;
            else if (sender == button_term_year) Config.SortieReportTermMode = SortieReportTermIntegrateMode.Year;
            else if (sender == button_term_all) Config.SortieReportTermMode = SortieReportTermIntegrateMode.All;
            //期間のボタン一覧更新
            button_term_update();
            //ファイル一覧の更新
            RefreshTermTreeView();
            //toolStripMenuItem_refresh.PerformClick();
        }
        private void button_term_update()
        {
            switch(Config.SortieReportTermMode)
            {
                case SortieReportTermIntegrateMode.Week:
                    button_term_week.Text = Helper.CheckString + butStrWeek;

                    button_term_month.Text = butStrMonth;
                    button_term_year.Text = butStrYear;
                    button_term_all.Text = butStrAll;
                    break;
                case SortieReportTermIntegrateMode.Month:
                    button_term_month.Text = Helper.CheckString + butStrMonth;

                    button_term_week.Text = butStrWeek;
                    button_term_year.Text = butStrYear;
                    button_term_all.Text = butStrAll;
                    break;
                case SortieReportTermIntegrateMode.Year:
                    button_term_year.Text = Helper.CheckString + butStrYear;

                    button_term_week.Text = butStrWeek;
                    button_term_month.Text = butStrMonth;
                    button_term_all.Text = butStrAll;
                    break;
                case SortieReportTermIntegrateMode.All:
                    button_term_all.Text = Helper.CheckString + butStrAll;

                    button_term_week.Text = butStrWeek;
                    button_term_month.Text = butStrMonth;
                    button_term_year.Text = butStrYear;
                    break;
            }
        }

        #endregion



    }
}
