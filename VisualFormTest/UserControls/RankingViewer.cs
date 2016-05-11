using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using HoppoAlpha.DataLibrary;
using HoppoAlpha.DataLibrary.Const;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiReqRanking;

namespace VisualFormTest.UserControls
{
    public partial class RankingViewer : UserControl
    {
        //ファイルのキャッシュ
        private Dictionary<RankingCacheHash, SortedDictionary<int, ApiRanking.ApiList>> fileCache;
        //リストビューのソーター
        private ListViewItemComparer listViewSorter;

        public bool IsShown { get; set; }
        private bool inited = false;

        private RankingCacheHash selectedRanking = new RankingCacheHash();
        private static readonly RankingCacheHash zeroRankingHash = new RankingCacheHash();

        private string[] columnHeader = new string[]
        {
            "#", "順", "Lv", "提督名", "提督経験値", 
            "戦果", "増加", "EO増", "Rankコメント", "甲", 
            "EO潜水補正戦果", "破壊済EO", "月初戦果", "EO補正戦果", "月初データ", 
            "潜"
        };

        public RankingViewer()
        {
            InitializeComponent();

            //ListViewSorter
            listViewSorter = new ListViewItemComparer();
            listViewSorter.ColumnModes = new ListViewItemComparer.ComparerMode[]
            {
                ListViewItemComparer.ComparerMode.NonSortable,
                ListViewItemComparer.ComparerMode.Integer, ListViewItemComparer.ComparerMode.Integer, ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.Integer, ListViewItemComparer.ComparerMode.Integer, ListViewItemComparer.ComparerMode.NullableInt,
                ListViewItemComparer.ComparerMode.NullableInt, ListViewItemComparer.ComparerMode.String, ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.NullableInt, ListViewItemComparer.ComparerMode.NullableInt, ListViewItemComparer.ComparerMode.NullableInt,
                ListViewItemComparer.ComparerMode.NullableInt, ListViewItemComparer.ComparerMode.String, ListViewItemComparer.ComparerMode.String,
            };
            listView_ranking.ListViewItemSorter = listViewSorter;

            if (APIPort.Basic != null && !inited) Init(); 
        }

        //内部クラス
        #region 内部クラス
        //ランキングのハッシュ
        public struct RankingCacheHash : IEquatable<RankingCacheHash>
        {
            public int Year { get; private set; }
            public int Month { get; private set; }
            public int Day { get; private set; }
            public int Section { get; private set; }

            public string FilePath { get; private set; }

            public string Display
            {
                get { return string.Format("{0}_{1}", this.Year * 10000 + this.Month * 100 + this.Day, this.Section); }
            }

            public string Display2
            {
                get 
                {
                    string sectionName;
                    switch(Section)
                    {
                        case 1: sectionName = "A"; break;
                        case 2: sectionName = "B"; break;
                        case 3: sectionName = "C"; break;
                        default: sectionName = Section.ToString(); break;
                    }

                    return string.Format("{0}{1}{2}", this.Month.ToString("D2"), this.Day.ToString("D2"), sectionName);
                }
            }

            //値のフォーマットを確かめながらセット
            public bool SetValue(string filePath)
            {
                if (!filePath.Contains("ranking")) return false;
                string[] namesplit = Path.GetFileNameWithoutExtension(filePath).Replace("ranking", "").Split('_');
                if (namesplit.Length != 2) return false;

                this.FilePath = filePath;

                //時間部分
                DateTime date;
                if(DateTime.TryParseExact(namesplit[0], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    this.Year = date.Year;
                    this.Month = date.Month;
                    this.Day = date.Day;
                }
                else
                {
                    return false;
                }

                //セクション部分
                int section;
                if(int.TryParse(namesplit[1], out section))
                {
                    this.Section = section;
                }
                else
                {
                    return false;
                }

                return true;
            }

            //直前のセクションのハッシュを求める
            public RankingCacheHash GetPreviousHash()
            {
                //値が登録されていない場合
                if (FilePath == null || Year <= 0) return new RankingCacheHash();

                DateTime now = new DateTime(Year, Month, Day).AddHours(3);//時差対策のために2時間足しておく
                DateTime targetDate;
                int targetSection;
                //現在のセクション＝1の場合は昨日
                if(this.Section == 1)
                {
                    targetDate = now.AddDays(-1);
                    targetSection = 2;
                }
                //それ以外＝今日の前のセクション
                else
                {
                    targetDate = now;
                    targetSection = this.Section - 1;
                }

                //ファイル名
                string filepath = APIReqRanking.GetFileName(targetDate, targetSection);

                RankingCacheHash hash = new RankingCacheHash();
                hash.Year = targetDate.Year;
                hash.Month = targetDate.Month;
                hash.Day = targetDate.Day;
                hash.Section = targetSection;
                hash.FilePath = filepath;

                return hash;
            }

            //次のセクションのハッシュを求める
            public RankingCacheHash GetNextHash()
            {
                //値が登録されていない場合
                if (FilePath == null || Year <= 0) return new RankingCacheHash();

                DateTime now = new DateTime(Year, Month, Day).AddHours(3);//時差対策のために2時間足しておく
                DateTime targetDate;
                int targetSection;
                //現在のセクション＝1の場合は今日の次のセクション
                if (this.Section == 1)
                {
                    targetDate = now;
                    targetSection = 2;
                }
                //現在のセクション=2の場合は月末とそれ以外で異なる
                else if(this.Section == 2)
                {
                    //月末の場合は今日の第3セクション
                    if(now.Day == DateTime.DaysInMonth(now.Year, now.Month))
                    {
                        targetDate = now;
                        targetSection = 3;
                    }
                    //それ以外の場合は翌日の第1セクション
                    else
                    {
                        targetDate = now.AddDays(1);
                        targetSection = 1;
                    }
                }
                //それ以外＝翌日の第1セクション
                else
                {
                    targetDate = now.AddDays(1);
                    targetSection = 1;
                }

                //ファイル名
                string filepath = APIReqRanking.GetFileName(targetDate, targetSection);

                RankingCacheHash hash = new RankingCacheHash();
                hash.Year = targetDate.Year;
                hash.Month = targetDate.Month;
                hash.Day = targetDate.Day;
                hash.Section = targetSection;
                hash.FilePath = filepath;

                return hash;
            }

            //月初のセクションのハッシュを求める
            public RankingCacheHash GetMonthFirstHash()
            {
                //値が登録されていない場合
                if (FilePath == null || Year <= 0) return new RankingCacheHash();

                DateTime now = new DateTime(Year, Month, Day).AddHours(3);//時差対策のために2時間足しておく
                DateTime targetDate = new DateTime(now.Year, now.Month, 1).AddHours(3);//該当月の1日
                int targetSection = 1;

                //ファイル名
                string filepath = APIReqRanking.GetFileName(targetDate, targetSection);
                RankingCacheHash hash = new RankingCacheHash();
                hash.Year = targetDate.Year;
                hash.Month = targetDate.Month;
                hash.Day = targetDate.Day;
                hash.Section = targetSection;
                hash.FilePath = filepath;

                return hash;
            }

            #region IEquitrable関連
            public bool Equals(RankingCacheHash other)
            {
                return this.Year == other.Year && this.Month == other.Month && this.Day == other.Day && this.Section == other.Section;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is RankingCacheHash)) return false;
                else return Equals((RankingCacheHash)obj);
            }

            public override int GetHashCode()
            {
                return (this.Year * 100000 + this.Month * 1000 + this.Day * 10 + this.Section);
            }

            public static bool operator ==(RankingCacheHash hash1, RankingCacheHash hash2)
            {
                return hash1.Equals(hash2);
            }

            public static bool operator !=(RankingCacheHash hash1, RankingCacheHash hash2)
            {
                return !hash1.Equals(hash2);
            }
            #endregion
        }

        //リストビューのソーター
        public class ListViewItemComparer : IComparer
        {
            //比較する方法
            public enum ComparerMode
            {
                String,
                Integer,
                NullableInt,
                NonSortable,
            }

            private int _column;
            private SortOrder _order;
            private ComparerMode _mode;
            private ComparerMode[] _columnModes;

            //並べるListViewの列の番号
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
                        if (_order == SortOrder.Ascending) _order = SortOrder.Descending;
                        else if (_order == SortOrder.Descending) _order = SortOrder.Ascending;
                    }
                    _column = value;
                }
            }

            //昇順or降順
            public SortOrder Order
            {
                get { return _order; }
                set { _order = value; }
            }

            //並び替えの方法
            public ComparerMode Mode
            {
                get { return _mode; }
                set { _mode = value; }
            }

            //列ごとの並び替えの方法
            public ComparerMode[] ColumnModes
            {
                set { _columnModes = value; }
            }


            //コンストラクタ
            public ListViewItemComparer(int col, SortOrder ord, ComparerMode cmod)
            {
                _column = col;
                _order = ord;
                _mode = cmod;
            }
            public ListViewItemComparer()
            {
                _column = 0;
                _order = SortOrder.Ascending;
                _mode = ComparerMode.Integer;
            }

            static ListViewItemComparer()
            {
                NullableIntCast = (string str) =>
                    {
                        if (str == "?") return -1;
                        else
                        {
                            int x;
                            int.TryParse(str, NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, null, out x);
                            return x;
                        }
                    };
            }

            //比較する
            static Func<string, int> NullableIntCast;
            public int Compare(object x, object y)
            {
                //並び替えない場合
                if (_order == SortOrder.None) return 0;

                int result = 0;
                ListViewItem itemx = (ListViewItem)x;
                ListViewItem itemy = (ListViewItem)y;

                //並び替えの方法を決定
                if(_columnModes != null && _columnModes.Length > _column)
                {
                    _mode = _columnModes[_column];
                }

                //xとyの比較
                switch(_mode)
                {
                    case ComparerMode.String:
                        result = string.Compare(itemx.SubItems[_column].Text, itemy.SubItems[_column].Text);
                        break;
                    case ComparerMode.Integer:
                        int intx, inty;
                        int.TryParse(itemx.SubItems[_column].Text, NumberStyles.AllowThousands, null, out intx);
                        int.TryParse(itemy.SubItems[_column].Text, NumberStyles.AllowThousands, null, out inty);
                        result = intx.CompareTo(inty);
                        break;
                    case ComparerMode.NullableInt:
                        result = NullableIntCast(itemx.SubItems[_column].Text).CompareTo(NullableIntCast(itemy.SubItems[_column].Text));
                        break;
                    case ComparerMode.NonSortable:
                        result = 0;
                        break;
                }

                //降順の場合は反転する
                if(_order == SortOrder.Descending)
                {
                    result = -result;
                }

                return result;
            }
        }
        #endregion

        public void Init()
        {
            //フォームのハンドルが作られていない場合
            var form = treeView_files.FindForm();
            if (form == null) return;

            //ファイル一覧の更新
            RefreshFileTreeview();
            inited = true;
        }

        //更新関連のメソッド
        #region 更新関連のメソッド
        //ツリービューの更新
        delegate void TreeViewRefreshCallBack();
        public void RefreshFileTreeview()
        {
            if (APIPort.Basic == null) return;
            //ルートディレクトリ
            string dir = Environment.CurrentDirectory + @"\user\" + APIPort.Basic.api_member_id + @"\ranking";

            if (!Directory.Exists(dir))
            {
                if (IsShown)
                {
                    MessageBox.Show
                        ("ランキングデータが１つもないため、ランキング解析ができません\nランキングデータを取得後再起動することで解析が開始されます",
                        "ランキング解析", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }

            List<TreeNode> rootnodes = new List<TreeNode>();
            //ファイル一覧
            int cnt = 0;
            foreach(var d in Directory.GetDirectories(dir).OrderByDescending(x => x))
            {
                //ルートノード
                string[] rootstr = d.Split('\\');
                if (rootstr.Length == 0) continue;
                
                //子ノード
                List<TreeNode> subnodes = new List<TreeNode>();
                List<int> hashes = new List<int>();
                foreach(var f in Directory.GetFiles(d))
                {
                    RankingCacheHash key = new RankingCacheHash();
                    //キーに値をセット
                    if (!key.SetValue(f)) continue;
                    //ノード
                    TreeNode sub = new TreeNode(key.Display);
                    sub.Tag = key;
                    //コレクションに追加
                    hashes.Add(key.GetHashCode());
                    subnodes.Add(sub);
                }

                //最新のノード
                if (cnt == 0)
                {
                    DateTime now = DateTime.Now;
                    int section = SenkaRecord.GetSection(now);
                    RankingCacheHash latestKey = new RankingCacheHash();
                    latestKey.SetValue(APIReqRanking.GetFileName(now, section));
                    //最新のノードを作っていなければ
                    if(hashes.IndexOf(latestKey.GetHashCode()) == -1)
                    {
                        TreeNode latestNode = new TreeNode(latestKey.Display);
                        latestNode.Tag = latestKey;
                        //コレクションに追加
                        hashes.Add(latestKey.GetHashCode());
                        subnodes.Add(latestNode);
                    }
                }

                //ルートノードにセット
                TreeNode root = new TreeNode(rootstr[rootstr.Length - 1],
                    hashes.OrderByDescending(x => x).Select(x => subnodes[hashes.IndexOf(x)]).ToArray());
                if (cnt == 0) root.Expand();//最新のノードは展開

                cnt++;
                //ルートノードのコレクションに追加
                rootnodes.Add(root);
            }

            //ファイルキャッシュのクリア
            fileCache = new Dictionary<RankingCacheHash, SortedDictionary<int, ApiRanking.ApiList>>();

            //スレッドセーフなコールバック
            Action act = () =>
                {
                    treeView_files.SuspendLayout();
                    treeView_files.Nodes.Clear();
                    treeView_files.Nodes.AddRange(rootnodes.ToArray());
                    treeView_files.ResumeLayout();
                };

            //ノードのセット
            if(treeView_files.InvokeRequired)
            {
                TreeViewRefreshCallBack d = new TreeViewRefreshCallBack(act);
                treeView_files.Invoke(d);
            }
            else
            {
                act();
            }
        }

        //データの読み込み
        private SortedDictionary<int, ApiRanking.ApiList> LoadData(RankingCacheHash hash)
        {
            if (fileCache == null) throw new NullReferenceException("ファイルキャッシュがNullです");

            SortedDictionary<int, ApiRanking.ApiList> result = new SortedDictionary<int, ApiRanking.ApiList>();             
            //最新のデータのハッシュ
            DateTime now = DateTime.Now;
            int section = SenkaRecord.GetSection(now);
            RankingCacheHash latestKey = new RankingCacheHash();
            latestKey.SetValue(APIReqRanking.GetFileName(now, section));

            //最新データの場合
            if (hash == latestKey)
            {
                if (APIReqRanking.Rankings != null)
                {
                    foreach (var x in APIReqRanking.Rankings)
                    {
                        result[x.Key] = x.Value.DeepCopy();//ディープコピーしておく
                    }
                }
            }
            else
            {
                //キャッシュにない場合はロード
                if(!fileCache.TryGetValue(hash, out result))
                {
                    var loadResult = Files.TryLoad(hash.FilePath, DataType.Ranking);

                    if (loadResult.IsSuccess) result = (SortedDictionary<int, ApiRanking.ApiList>)loadResult.Instance;
                }
            }

            //キャッシュに追加
            fileCache[hash] = result;

            return result;
        }

        //リストビューの更新
        public void RefreshListView(RankingCacheHash selectedHash)
        {
            if (fileCache == null) return;
            if (selectedHash == zeroRankingHash) return;

            //現在のデータを取得
            var nowdata = LoadData(selectedHash);
            //1個前のデータを取得
            var prevhash = selectedHash.GetPreviousHash();
            SortedDictionary<int, ApiRanking.ApiList> prevdata = null;
            if (prevhash.Month == selectedHash.Month) prevdata = LoadData(prevhash);
            //月初のデータを取得3つ取得
            var firsthash = selectedHash.GetMonthFirstHash();
            Dictionary<string, SortedDictionary<int, ApiRanking.ApiList>> firstdatas = new Dictionary<string, SortedDictionary<int, ApiRanking.ApiList>>();
            while(firstdatas.Count < 3)
            {
                //選択ハッシュ≦月初ハッシュになってしまったら離脱
                if (selectedHash.GetHashCode() <= firsthash.GetHashCode()) break;
                //ファイルが存在すれば読み込み
                if(firsthash.FilePath != null && File.Exists(firsthash.FilePath))
                {
                    firstdatas[firsthash.Display2] = LoadData(firsthash);
                }
                //ハッシュの繰り下げ
                firsthash = firsthash.GetNextHash();
            }

            SortedDictionary<int, ApiRanking.ApiList> firstdata = null;
            if (firsthash.Month == selectedHash.Month) firstdata = LoadData(firsthash);

            //リストビューアイテムのコレクション
            List<ListViewItem> items = new List<ListViewItem>();
            if (nowdata != null)
            {
                int index = 1;
                foreach (var x in nowdata.Values)
                {
                    //インデックス
                    ListViewItem parent = new ListViewItem(index.ToString());

                    //潜水マンのハンデ取得
                    int handicap = 0;
                    if (Config.RankingSubmarinerList.ContainsKey(x.api_member_id))
                    {
                        handicap = Config.RankingSubmarinerEOHandicap;
                    }
                    
                    //潜水マンなら色を替える
                    if (handicap > 0) parent.BackColor = Color.Lavender;
                    //SubItemの表示変更を受け付ける
                    parent.UseItemStyleForSubItems = false;

                    //タグに提督IDを入れる
                    parent.Tag = x.api_member_id;

                    //順位
                    parent.SubItems.Add(x.api_no.ToString());
                    //増分
                    var diff = ApiRanking.RankingDiff.CalcDiff(x.api_member_id, nowdata, prevdata);
                    //EO計算
                    var eocalc = ApiRanking.RankingEOCalc.CalcEO(x.api_member_id, nowdata, firstdatas, handicap);

                    //提督Lv
                    parent.SubItems.Add(x.api_level.ToString(), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //提督名
                    parent.SubItems.Add(x.api_nickname, parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //提督経験値
                    parent.SubItems.Add(x.api_experience.ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //戦果
                    parent.SubItems.Add(x.api_rate.ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //戦果増分
                    if (diff.DiffSenka == null) parent.SubItems.Add("?", parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    else parent.SubItems.Add("+" + ((int)(diff.DiffSenka)).ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //EO増分
                    if (diff.DiffEO == null) parent.SubItems.Add("?", parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    else
                    {
                        int diffeo = (int)diff.DiffEO;
                        //EO破壊されている場合
                        if (diffeo >= 5)
                        {
                            parent.SubItems.Add("+" + diffeo.ToString("N0"), Color.Red, parent.BackColor, new Font(listView_ranking.Font, FontStyle.Bold));
                        }
                        else
                        {
                            parent.SubItems.Add("+" + diffeo.ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                        }
                    }
                    //Rankコメント
                    parent.SubItems.Add(x.api_comment, parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //甲種勲章
                    parent.SubItems.Add(x.api_medals.ToString(), parent.ForeColor, parent.BackColor, listView_ranking.Font);

                    //EO潜水補正戦果
                    if (eocalc.CorrelatedWithEOSubmarineSenka == null) parent.SubItems.Add("?", parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    else parent.SubItems.Add(((int)(eocalc.CorrelatedWithEOSubmarineSenka)).ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //破壊済みEO
                    if (eocalc.DestroiedEO == null) parent.SubItems.Add("?", parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    else
                    {
                        int desteo = (int)eocalc.DestroiedEO;
                        //EO破壊されている場合
                        if (desteo >= 5)
                        {
                            parent.SubItems.Add("+" + desteo.ToString("N0"), Color.Red, parent.BackColor, new Font(listView_ranking.Font, FontStyle.Bold));
                        }
                        else
                        {
                            parent.SubItems.Add("+" + desteo.ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                        }
                    }
                    //月初戦果
                    if (eocalc.FirstSenka == null) parent.SubItems.Add("?", parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    else parent.SubItems.Add(((int)(eocalc.FirstSenka)).ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //EO補正済み戦果
                    if (eocalc.CorrelatedWithEOSenka == null) parent.SubItems.Add("?", parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    else parent.SubItems.Add(((int)(eocalc.CorrelatedWithEOSenka)).ToString("N0"), parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //月初のデータ名
                    if (eocalc.FirstSenkaRecordName == null) parent.SubItems.Add("?", parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    else parent.SubItems.Add(eocalc.FirstSenkaRecordName, parent.ForeColor, parent.BackColor, listView_ranking.Font);
                    //潜水マンフラグ
                    parent.SubItems.Add(handicap > 0 ? "○" : "×", parent.ForeColor, parent.BackColor, listView_ranking.Font);

                    //コレクションに追加
                    items.Add(parent);

                    index++;
                }
            }

            //リストビューの更新
            listView_ranking.SuspendLayout();
            listView_ranking.Items.Clear();
            listView_ranking.Items.AddRange(items.ToArray());
            listView_ranking.ResumeLayout();

            //右上の表示更新
            string section_str;
            switch(selectedHash.Section)
            {
                case 1: section_str = "午前"; break;
                case 2: section_str = "午後"; break;
                case 3: section_str = "ロスタイム"; break;
                default: section_str = selectedHash.Section.ToString(); break;
            }
            int count = 0;
            if(nowdata != null) count = nowdata.Count;
            toolStripMenuItem_view.Text = string.Format("　　{0}/{1}/{2}({3}) N={4}", selectedHash.Year, selectedHash.Month, selectedHash.Day, section_str, count);

        }
        #endregion

        //イベントハンドラ
        private void treeView_files_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null || !(e.Node.Tag is RankingCacheHash)) return;

            selectedRanking = (RankingCacheHash)e.Node.Tag;
            RefreshListView(selectedRanking);

        }

        private void listView_ranking_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listView_ranking.SuspendLayout();

            //クリックされた列
            listViewSorter.Column = e.Column;
            //並び替える
            listView_ranking.Sort();
            //ヘッダーのタイトルを更新
            foreach(var x in listView_ranking.Columns.OfType<ColumnHeader>())
            {
                string sortmode;
                if(listViewSorter.Order == SortOrder.Descending) sortmode = "▼";
                else if(listViewSorter.Order == SortOrder.Ascending) sortmode = "▲";
                else sortmode = "";

                //ソート対象列
                if (x.DisplayIndex == e.Column) x.Text = sortmode + columnHeader[x.DisplayIndex];
                else x.Text = columnHeader[x.DisplayIndex];
            }
            //インデックスの振り直し
            int index = 1;
            foreach(var x in listView_ranking.Items.OfType<ListViewItem>())
            {
                x.Text = index.ToString();
                index++;
            }

            listView_ranking.ResumeLayout();
        }

        //更新ボタン
        private void toolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            //リストビューの消去
            listView_ranking.SuspendLayout();
            listView_ranking.Items.Clear();
            listView_ranking.ResumeLayout();

            RefreshFileTreeview();
        }

        //クリップボードにコピー
        private void toolStripMenuItem_copytoclipboard_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            List<string> row = new List<string>();
            //Header
            foreach(var h in listView_ranking.Columns.OfType<ColumnHeader>())
            {
                row.Add(h.Text);
            }
            sb.AppendLine(string.Join("\t", row));
            //アイテム
            foreach(var l in listView_ranking.Items.OfType<ListViewItem>())
            {
                row = new List<string>();

                foreach(var s in l.SubItems.OfType<ListViewItem.ListViewSubItem>())
                {
                    row.Add(s.Text);
                }

                sb.AppendLine(string.Join("\t", row));
            }

            //クリップボードにコピー
            Clipboard.SetText(sb.ToString());
        }

        //スクリーンショット
        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(listView_ranking, "ranking");
        }

        //潜水マンに追加
        private void toolStripMenuItem_addsubmariner_Click(object sender, EventArgs e)
        {
            var selected = listView_ranking.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if(selected != null)
            {
                //提督ID
                if (selected.Tag == null || !(selected.Tag is int)) return;
                int id = (int)selected.Tag;
                //提督名
                if (selected.SubItems.Count < 3) return;
                string name = selected.SubItems[2].Text;

                //追加
                Config.RankingSubmarinerList[id] = name;
            }

            RefreshListView(selectedRanking);
        }

        //潜水マン設定
        private void toolStripMenuItem_submariner_Click(object sender, EventArgs e)
        {
            var setting = new RankingViewer_SubmarinerSetting();
            setting.FormClosing += (ss, ee) =>
                {
                    if (setting.RefreshRequired) RefreshListView(selectedRanking);
                };
            setting.ShowDialog();
        }
    }
}
