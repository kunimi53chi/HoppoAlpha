using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using GoogleChartSharp;

namespace VisualFormTest
{
    public class KancolleInfoUnitList
    {
        delegate void SetListViewItemsCallBack(System.Windows.Forms.ListView listview, string[][] items, IEnumerable<ApiShip> queried);
        delegate void SetListViewEventHandlerCallBack(System.Windows.Forms.ListView listview, ListViewItemComparer comparer,
            System.Windows.Forms.ColumnClickEventHandler handler);
        delegate void SetListViewSorterCallBack(System.Windows.Forms.ListView listview, int col);

        //クエリ全体のデータ
        public static List<UnitQuery> Queries { get; set; }
        //クエリ最大数
        public static readonly int QueriesMax = 128;
        //初期化フラグ
        public static bool IsInited { get; set; }


        //ファイル名
        #region ファイル名
        public static string DirectoryName
        {
            get
            {
                return @"user/" + APIPort.Basic.api_member_id + @"/general/";
            }
        }

        public static string FileName
        {
            get
            {
                return DirectoryName + "query.dat";
            }
        }
        #endregion

        //別窓
        public static UnitListViewer unitListViewer;

        //コールバック
        #region コールバック
        public static void SetListViewItems(System.Windows.Forms.ListView listview, string[][] items, IEnumerable<ApiShip> queried)
        {
            if (listview.InvokeRequired)
            {
                SetListViewItemsCallBack d = new SetListViewItemsCallBack(SetListViewItems);
                listview.Invoke(d, new object[] { listview, items, queried });
            }
            else
            {
                listview.BeginUpdate();
                listview.Items.Clear();
                int cnt = 0;
                int n = queried.Count();
                System.Windows.Forms.ListViewItem[] item_array = new System.Windows.Forms.ListViewItem[n];
                //入渠ドックの船のID
                int[] ndock_id = (from d in APIPort.Ndocks
                                  select d.api_ship_id).ToArray();
                foreach (var s in queried)
                {
                    System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(items[cnt]);
                    item.Tag = s;
                    //リストビューの色
                    var hpcond = s.GetHPCondition(Array.IndexOf(ndock_id, s.api_id) != -1, false, Config.BucketHPRatio, APIGetMember.SlotItemsDictionary);
                    System.Drawing.Color bcol = System.Drawing.SystemColors.Window;
                    if (s.api_cond >= 50) bcol = System.Drawing.Color.FromArgb(255, 255, 160);

                    if (hpcond.HasFlag(HPCondition.IsBathing)) bcol = System.Drawing.Color.FromArgb(160, 160, 255);
                    else if (hpcond.HasFlag(HPCondition.MiddleDamage)) bcol = System.Drawing.Color.FromArgb(255, 221, 160);
                    else if (hpcond.HasFlag(HPCondition.HeavyDamage)) bcol = System.Drawing.Color.FromArgb(255, 160, 160);

                    item.BackColor = bcol;
                    //追加
                    item_array[cnt] = item;
                    cnt++;
                }
                listview.Items.AddRange(item_array);
                listview.EndUpdate();
            }
        }

        private static void SetListViewEventHandler(System.Windows.Forms.ListView listview, ListViewItemComparer comparer,
            System.Windows.Forms.ColumnClickEventHandler handler)
        {
            if (listview.InvokeRequired)
            {
                SetListViewEventHandlerCallBack d = new SetListViewEventHandlerCallBack(SetListViewEventHandler);
                listview.Invoke(d, new object[] { listview, comparer, handler });
            }
            else
            {
                listview.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(handler);
                listview.ListViewItemSorter = comparer;
            }
        }

        private static void SetListViewSorter(System.Windows.Forms.ListView listview, int col)
        {
            if (listview.InvokeRequired)
            {
                SetListViewSorterCallBack d = new SetListViewSorterCallBack(SetListViewSorter);
                listview.Invoke(d, new object[] { listview, col });
            }
            else
            {
                ListViewItemComparer comparer = (ListViewItemComparer)listview.ListViewItemSorter;
                comparer.SetColumn(col);
                //選択されているColに昇順・降順ボタン
                int cnt = 0;
                foreach (System.Windows.Forms.ColumnHeader citem in listview.Columns)
                {
                    string str = citem.Text;
                    str = str.Replace("▼", "").Replace("▲", "");
                    //選択されている列
                    if (cnt == col)
                    {
                        if (comparer.Order == SortOrder.Descending) str = "▼" + str;
                        else if (comparer.Order == SortOrder.Ascending) str = "▲" + str;
                    }
                    //名前のセット
                    citem.Text = str;
                    cnt++;
                }

                listview.Sort();
            }
        }
        #endregion

        //内部クラス
        #region 内部クラス


        //ソートオーダー
        public enum SortOrder
        {
            None, Descending, Ascending,
        }

        //ソートモード
        public enum SortMode
        {
            ID, Name, Level, ShipType, Cond,
            NdockTime, HP, Exp, NdockItem, Karyoku,
            Raisou, NightKaryoku, Taisen, Taiku, Soukou,
            Kaihi, Tousai, Length, Sakuteki, Luck,
            SallyArea,
        }

        //ListViewSorter
        public class ListViewItemComparer : IComparer
        {
            public int Column { get; private set; }
            public SortOrder Order { get; private set; }

            public ListViewItemComparer(int col)
            {
                SetColumn(col);
            }

            //列のセット
            public void SetColumn(int col)
            {
                //列が同じ場合
                if (this.Column == col)
                {
                    if (this.Order == SortOrder.Ascending) this.Order = SortOrder.Descending;
                    else if (this.Order == SortOrder.Descending) this.Order = SortOrder.Ascending;
                }
                //違う列の場合
                else
                {
                    this.Order = SortOrder.Descending;
                }
                this.Column = col;
            }

            public int Compare(object x, object y)
            {
                //ListViewItemの取得
                System.Windows.Forms.ListViewItem itemx = (System.Windows.Forms.ListViewItem)x;
                System.Windows.Forms.ListViewItem itemy = (System.Windows.Forms.ListViewItem)y;
                //ApiShipの取得
                ApiShip shipx = (ApiShip)itemx.Tag;
                ApiShip shipy = (ApiShip)itemy.Tag;
                //モード
                SortMode mode = (SortMode)this.Column;
                //場合分け
                int result;
                switch(mode)
                {
                    case SortMode.Cond:
                        result = shipx.api_cond.CompareTo(shipy.api_cond);
                        break;
                    case SortMode.Exp:
                        result = shipx.api_exp[0].CompareTo(shipy.api_exp[0]);
                        break;
                    case SortMode.HP:
                        double hpratio_x = 1 - (double)shipx.api_nowhp / (double)shipx.api_maxhp;
                        double hpratio_y = 1 - (double)shipy.api_nowhp / (double)shipy.api_maxhp;
                        result = hpratio_x.CompareTo(hpratio_y);
                        break;
                    case SortMode.ID:
                        result = shipx.api_id.CompareTo(shipy.api_id);
                        break;
                    case SortMode.Kaihi://5
                        result = shipx.api_kaihi[0].CompareTo(shipy.api_kaihi[0]);
                        break;
                    case SortMode.Karyoku:
                        result = shipx.api_karyoku[0].CompareTo(shipy.api_karyoku[0]);
                        break;
                    case SortMode.Length:
                        result = shipx.api_leng.CompareTo(shipy.api_leng);
                        break;
                    case SortMode.Level:
                        result = shipx.api_lv.CompareTo(shipy.api_lv);
                        break;
                    case SortMode.Luck:
                        int luck_buf = shipx.api_lucky[0].CompareTo(shipy.api_lucky[0]) * 10
                            + shipx.api_lucky[1].CompareTo(shipy.api_lucky[1]);//現在値の比較×10+MAX値の比較
                        result = Math.Sign(luck_buf);
                        break;
                    case SortMode.Name://10
                        result = shipx.api_ship_id.CompareTo(shipy.api_ship_id);//船のIDで比較
                        break;
                    case SortMode.NdockItem:
                        int ndock_item_buf = shipx.api_ndock_item.Sum().CompareTo(shipy.api_ndock_item.Sum()) * 10
                            + shipx.api_ndock_item[0].CompareTo(shipy.api_ndock_item[0]);//合計×10+燃料
                        result = Math.Sign(ndock_item_buf);
                        break;
                    case SortMode.NdockTime:
                        result = shipx.api_ndock_time.CompareTo(shipy.api_ndock_time);
                        break;
                    case SortMode.NightKaryoku:
                        result = (shipx.api_karyoku[0] + shipx.api_raisou[0]).CompareTo(
                            shipy.api_karyoku[0] + shipy.api_raisou[0]);
                        break;
                    case SortMode.Raisou:
                        result = shipx.api_raisou[0].CompareTo(shipy.api_raisou[0]);
                        break;
                    case SortMode.Sakuteki://15
                        result = shipx.api_sakuteki[0].CompareTo(shipy.api_sakuteki[0]);
                        break;
                    case SortMode.ShipType:
                        int shiptype_x = shipx.DShip.api_stype;
                        int shiptype_y = shipy.DShip.api_stype;
                        result = shiptype_x.CompareTo(shiptype_y);
                        break;
                    case SortMode.Soukou:
                        result = shipx.api_soukou[0].CompareTo(shipy.api_soukou[0]);
                        break;
                    case SortMode.Taiku:
                        result = shipx.api_taiku[0].CompareTo(shipy.api_taiku[0]);
                        break;
                    case SortMode.Taisen:
                        result = shipx.api_taisen[0].CompareTo(shipy.api_taisen[0]);
                        break;
                    case SortMode.Tousai://20
                        result = shipx.api_onslot.Sum().CompareTo(shipy.api_onslot.Sum());
                        break;
                    case SortMode.SallyArea://
                        result = shipx.api_sally_area.CompareTo(shipy.api_sally_area);
                        break;
                    default:
                        result = 0;
                        break;
                }
                //降順の場合は逆転
                if (this.Order == SortOrder.Descending) result = -result;
                else if (this.Order == SortOrder.None) result = 0;
                //結果を返す
                return result;
            }
        }
        #endregion

        //クエリデータの関係
        #region クエリデータ
        //クエリデータの初期化
        public static void InitQuery()
        {
            //ユーザークエリの読み込み
            var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(FileName, HoppoAlpha.DataLibrary.DataType.Query);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Query, loadResult, false);

            if (loadResult.IsSuccess) Queries = (List<UnitQuery>)loadResult.Instance;
            else Queries = MakeNewQueries();

            //初期化終了
            IsInited = true;
        }

        //クエリデータを新規作成
        #region クエリの新規作成
        public static List<UnitQuery> MakeNewQueries()
        {
            List<UnitQuery> queries = new List<UnitQuery>();
            foreach (int i in Enumerable.Range(0, QueriesMax))
            {
                UnitQuery q = new UnitQuery();
                q.ID = i;
                q.Name = "なし";
                queries.Add(q);
            }
            //クエリプリセット
            //0 : 全て選択
            queries[0].Name = "全て表示";
            //1 : 駆逐軽巡
            queries[1].Name = "駆逐軽巡";
            UnitQueryItem q1 = new UnitQueryItem(UnitQueryMode.ShipType);
            q1.SearchesAdd(2, UnitQueryRangeMode.Equals);//駆逐
            q1.SearchesAdd(3, UnitQueryRangeMode.Equals);//軽巡
            queries[1].Query.Add(q1);
            //2  : 潜水艦のみ
            queries[2].Name = "潜水・潜水空母";
            UnitQueryItem q2 = new UnitQueryItem(UnitQueryMode.ShipType);
            q2.SearchesAdd(13, UnitQueryRangeMode.Equals);//潜水
            q2.SearchesAdd(14, UnitQueryRangeMode.Equals);//潜水空母
            queries[2].Query.Add(q2);
            //3 : キラキラのみ
            queries[3].Name = "全て : キラキラ艦";
            UnitQueryItem q3 = new UnitQueryItem(UnitQueryMode.Cond);
            q3.SearchesAdd(50, UnitQueryRangeMode.MoreThanEquals);//Cond値50以上
            queries[3].Query.Add(q3);
            //4 : ロック済み小破以下
            queries[4].Name = "主力艦 : 小破以下";
            UnitQueryItem q41 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q41.SearchesAdd(1, UnitQueryRangeMode.Equals);//ロック
            UnitQueryItem q42 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q42.AddHPCondition_NoneOrSmall();//小破以下
            queries[4].Query.Add(q41);
            queries[4].Query.Add(q42);
            //---
            //5 : ロック済み+中破大破
            queries[5].Name = "主力艦 : 中破・大破";
            UnitQueryItem q51 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q51.SearchesAdd(1, UnitQueryRangeMode.Equals);//ロック済み
            UnitQueryItem q52 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q52.AddHPCondition_MiddleOver();//中破以上
            queries[5].Query.Add(q51);
            queries[5].Query.Add(q52);
            //6 : 駆逐：キラキラなし＋ロック済＋小破以下
            queries[6].Name = "主力駆逐：キラキラなし＋小破以下";
            UnitQueryItem q61 = new UnitQueryItem(UnitQueryMode.ShipType);
            q61.SearchesAdd(2, UnitQueryRangeMode.Equals);//駆逐艦
            UnitQueryItem q62 = new UnitQueryItem(UnitQueryMode.Cond);
            q62.SearchesAdd(49, UnitQueryRangeMode.LessThanEquals);//cond49以下
            UnitQueryItem q63 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q63.SearchesAdd(1, UnitQueryRangeMode.Equals);//ロック済み
            UnitQueryItem q64 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q64.AddHPCondition_NoneOrSmall();//小破以下
            queries[6].Query.AddRange(new UnitQueryItem[] { q61, q62, q63, q64 });
            // 7 : 主力駆軽雷 : 中破・大破
            queries[7].Name = "主力駆軽雷 : 中破・大破";
            UnitQueryItem q71 = new UnitQueryItem(UnitQueryMode.ShipType);
            q71.SearchesAdd(2, UnitQueryRangeMode.Equals);//駆逐艦
            q71.SearchesAdd(3, UnitQueryRangeMode.Equals);//軽巡洋艦
            q71.SearchesAdd(4, UnitQueryRangeMode.Equals);//雷巡
            UnitQueryItem q72 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q72.SearchesAdd(1, UnitQueryRangeMode.Equals);//ロック済み            
            UnitQueryItem q73 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q73.AddHPCondition_MiddleOver();//中破以上
            queries[7].Query.AddRange(new UnitQueryItem[] { q71, q72, q73 });
            //8 : 主力巡洋艦、戦艦・空母系 : 小破
            queries[8].Name = "主力巡洋艦、戦艦・空母系 : 小破";
            UnitQueryItem q81 = new UnitQueryItem(UnitQueryMode.ShipTypeName);
            q81.SearchesAdd("巡洋艦", UnitQueryMatchMode.Contains);//巡洋艦
            q81.SearchesAdd("戦艦", UnitQueryMatchMode.Contains);//戦艦
            q81.SearchesAdd("軽空母", UnitQueryMatchMode.ExactlyMatch);
            q81.SearchesAdd("正規空母", UnitQueryMatchMode.ExactlyMatch);
            q81.SearchesAdd("装甲空母", UnitQueryMatchMode.ExactlyMatch);
            UnitQueryItem q82 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q82.SearchesAdd(1, UnitQueryRangeMode.Equals);//ロック済み            
            UnitQueryItem q83 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q83.AddHPCondition_Small();//小破のみ
            queries[8].Query.AddRange(new UnitQueryItem[] { q81, q82, q83 });
            //9 : 非ロック重巡 : 燃弾あり
            queries[9].Name = "非ロック重巡 : 燃弾あり";
            UnitQueryItem q91 = new UnitQueryItem(UnitQueryMode.ShipType);
            q91.SearchesAdd(5, UnitQueryRangeMode.Equals);//重巡
            UnitQueryItem q92 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q92.SearchesAdd(0, UnitQueryRangeMode.Equals);//非ロック            
            UnitQueryItem q93 = new UnitQueryItem(UnitQueryMode.FuelNow);
            q93.SearchesAdd(1, UnitQueryRangeMode.MoreThanEquals);//燃料1以上
            UnitQueryItem q94 = new UnitQueryItem(UnitQueryMode.BullNow);
            q94.SearchesAdd(1, UnitQueryRangeMode.MoreThanEquals);//弾薬1以上
            queries[9].Query.AddRange(new UnitQueryItem[] { q91, q92, q93, q94 });
            //--
            //10 : 非ロック戦艦 : 燃弾あり
            queries[10].Name = "非ロック戦艦 : 燃弾あり";
            UnitQueryItem q101 = new UnitQueryItem(UnitQueryMode.ShipTypeName);
            q101.SearchesAdd("戦艦", UnitQueryMatchMode.Contains);//戦艦
            UnitQueryItem q102 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q102.SearchesAdd(0, UnitQueryRangeMode.Equals);//非ロック
            UnitQueryItem q103 = new UnitQueryItem(UnitQueryMode.FuelNow);
            q103.SearchesAdd(1, UnitQueryRangeMode.MoreThanEquals);//燃料1以上
            UnitQueryItem q104 = new UnitQueryItem(UnitQueryMode.BullNow);
            q104.SearchesAdd(1, UnitQueryRangeMode.MoreThanEquals);//弾薬1以上
            queries[10].Query.AddRange(new UnitQueryItem[] { q101, q102, q103, q104 });
            //11 : 非ロック駆逐 : 燃弾あり
            queries[11].Name = "非ロック駆逐 : 燃弾あり";
            UnitQueryItem q111 = new UnitQueryItem(UnitQueryMode.ShipType);
            q111.SearchesAdd(2, UnitQueryRangeMode.Equals);//駆逐
            UnitQueryItem q112 = new UnitQueryItem(UnitQueryMode.IsLocked);
            q112.SearchesAdd(0, UnitQueryRangeMode.Equals);//非ロック 
            UnitQueryItem q113 = new UnitQueryItem(UnitQueryMode.FuelNow);
            q113.SearchesAdd(1, UnitQueryRangeMode.MoreThanEquals);//燃料1以上
            UnitQueryItem q114 = new UnitQueryItem(UnitQueryMode.BullNow);
            q114.SearchesAdd(1, UnitQueryRangeMode.MoreThanEquals);//弾薬1以上
            queries[11].Query.AddRange(new UnitQueryItem[] { q111, q112, q113, q114 });
            //12 : 潜水・潜水空母 : 小破以下
            queries[12].Name = "潜水・潜水空母 : 小破以下";
            UnitQueryItem q121 = new UnitQueryItem(UnitQueryMode.ShipType);
            q121.SearchesAdd(13, UnitQueryRangeMode.Equals);//潜水艦
            q121.SearchesAdd(14, UnitQueryRangeMode.Equals);//潜水空母
            UnitQueryItem q122 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q122.AddHPCondition_NoneOrSmall();//小破以下
            queries[12].Query.Add(q121);
            queries[12].Query.Add(q122);
            //13 : 潜水・潜水空母 : 中破・大破
            queries[13].Name = "潜水・潜水空母 : 中破・大破";
            UnitQueryItem q131 = new UnitQueryItem(UnitQueryMode.ShipType);
            q131.SearchesAdd(13, UnitQueryRangeMode.Equals);//潜水艦
            q131.SearchesAdd(14, UnitQueryRangeMode.Equals);//潜水空母
            UnitQueryItem q132 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q132.AddHPCondition_MiddleOver();//中破大破
            queries[13].Query.Add(q131);
            queries[13].Query.Add(q132);
            //14 : 潜水空母 : 中破・大破
            queries[14].Name = "潜水空母 : 中破・大破";
            UnitQueryItem q141 = new UnitQueryItem(UnitQueryMode.ShipType);
            q141.SearchesAdd(14, UnitQueryRangeMode.Equals);//潜水空母
            UnitQueryItem q142 = new UnitQueryItem(UnitQueryMode.HPRatio);
            q142.AddHPCondition_MiddleOver();//中破大破
            queries[14].Query.Add(q141);
            queries[14].Query.Add(q142);
            //--
            //15 : 潜水艦 : キラキラあり
            queries[15].Name = "潜水艦 : キラキラあり";
            UnitQueryItem q151 = new UnitQueryItem(UnitQueryMode.ShipType);
            q151.SearchesAdd(13, UnitQueryRangeMode.Equals);//潜水艦
            UnitQueryItem q152 = new UnitQueryItem(UnitQueryMode.Cond);
            q152.SearchesAdd(50, UnitQueryRangeMode.MoreThanEquals);//キラキラあり
            queries[15].Query.Add(q151);
            queries[15].Query.Add(q152);

            //返り値
            return queries;
        }
        #endregion

        //クエリの保存
        public static void SaveQueries()
        {
            //初期化されていなかったら
            if (!IsInited || Queries == null) return;
            //ディレクトリの作成
            if (!System.IO.Directory.Exists(DirectoryName))
            {
                System.IO.Directory.CreateDirectory(DirectoryName);
            }
            //保存
            var saveResult = HoppoAlpha.DataLibrary.Files.Save(FileName, HoppoAlpha.DataLibrary.DataType.Query, Queries);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Query, saveResult, true);
        }
        #endregion

        //本体の動作
        #region 本体の動作
        //初期化
        public static void Init(System.Windows.Forms.Label[] label_num,
            System.Windows.Forms.Label[] label_repair, System.Windows.Forms.ListView listview)
        {
            //イベントハンドラ
            ListViewItemComparer comp = new ListViewItemComparer(-1);
            SetListViewEventHandler(listview, comp, ListView_ColumnClick);
            //ListViewのダブルバッファ
            CallBacks.EnableDoubleBuffering(listview);

            //クエリデータの初期化
            if (!IsInited) InitQuery();
        }

        public static void Init(System.Windows.Forms.ListView listview)
        {
            //イベントハンドラ
            ListViewItemComparer comp = new ListViewItemComparer(-1);
            SetListViewEventHandler(listview, comp, ListView_ColumnClick);
            //ListViewのダブルバッファ
            CallBacks.EnableDoubleBuffering(listview);
        }

        //実行
        public static void DoIt(System.Windows.Forms.Label[] label_num,
            System.Windows.Forms.Label[] label_repair, System.Windows.Forms.ListView listview, 
            IEnumerable<ApiShip> queried, UnitQueryFilter filter)
        {
            //メインかどうかのフラグ
            bool mainflag = label_num != null && label_repair != null;
            if (APIPort.ShipsDictionary == null || (!mainflag && queried == null)) return;
            //艦隊配置のID
            var fleetassignships = from d in APIPort.DeckPorts
                                   from s in d.api_ship
                                   where s != -1
                                   select s;
            //リストビューのアイテムの取得
            int n = queried.Count();
            string[][] item_str = new string[n][];
            int cnt = 0;
            foreach(var s in queried)
            {
                bool assign_flag;
                if (filter.NotShowStar) assign_flag = false;
                else assign_flag = Enumerable.Contains(fleetassignships, s.api_id);
                item_str[cnt] = ToListViewItem(s, assign_flag);
                cnt++;
            }
            //リストビューのアイテムの更新
            SetListViewItems(listview, item_str, queried);
            //ラベルがNULLの場合はここで終わり
            if (!mainflag) return;
            //キラキラの艦数
            var query_cond = from s in queried
                             where s.api_cond >= 50
                             select s;
            CallBacks.SetLabelText(label_num[0], query_cond.Count().ToString());
            //中破以上の艦数
            var conditions = queried.Select(x => x.GetHPCondition(false, false, Config.BucketHPRatio, APIGetMember.SlotItemsDictionary) & ~HPCondition.EraseFlagsMagicNumber);
            var query_middleover = conditions.Where(x => (x == HPCondition.HeavyDamage || x == HPCondition.MiddleDamage));
            CallBacks.SetLabelText(label_num[1], query_middleover.Count().ToString());
            //小破の艦数
            var query_small = conditions.Where(x => x == HPCondition.SmallDamage);
            CallBacks.SetLabelText(label_num[2], query_small.Count().ToString());
            //件数

            //入渠時間の計算
            RefreshRepairTime(label_num, label_repair, queried, filter);
        }

        //
        public static void RefreshRepairTime(System.Windows.Forms.Label[] label_num,
            System.Windows.Forms.Label[] label_repair, IEnumerable<ApiShip> queried, UnitQueryFilter filter)
        {
            if (queried == null || queried.Count() == 0) return; 
            //入渠時間の計算
            //クエリのさらなる絞り込み
            var query_ndock = from s in queried
                              where Filtering(s, filter)
                              select s;
            //入渠時間の合計
            long sum_bath = 0;
            foreach (ApiShip s in query_ndock) sum_bath += s.api_ndock_time;
            //出力アイテム
            TimeSpan[] result = new TimeSpan[4];
            result[0] = TimeSpan.FromMilliseconds(sum_bath);
            result[1] = TimeSpan.FromMilliseconds(sum_bath / 2);
            result[2] = TimeSpan.FromMilliseconds(sum_bath / 3);
            result[3] = TimeSpan.FromMilliseconds(sum_bath / 4);
            //入渠時間のセット
            for (int i = 0; i < result.Length; i++)
            {
                CallBacks.SetLabelText(label_repair[i], TimeSpanToString(result[i]));
            }
        }

        //クエリの実行
        public static IEnumerable<ApiShip> DoQuery(UnitQuery query, bool notshowassign)
        {
            if (APIPort.DeckPorts == null || APIPort.ShipsDictionary == null) return Enumerable.Empty<ApiShip>();

            //艦隊所属の艦
            var fleetassignships = from d in APIPort.DeckPorts
                                   from s in d.api_ship
                                   where s != -1
                                   select s;
            var ships = from s in APIPort.ShipsDictionary.Values
                        where query.CheckAll(s)
                        where (!notshowassign) || !Enumerable.Contains(fleetassignships, s.api_id)
                        select s;
            return ships;
        }

        //フィルタリング（入渠時間計算用）
        private static bool Filtering(ApiShip oship, UnitQueryFilter filter)
        {
            //入渠時間が一定以上か
            bool ndock_timeover = filter.NotShowOverThresholdHour &&
                TimeSpan.FromMilliseconds(oship.api_ndock_time).TotalHours >= filter.ThresholdHour;
            //小破以上か
            var hpcond = oship.GetHPCondition(false, false, Config.BucketHPRatio, APIGetMember.SlotItemsDictionary) & ~HPCondition.EraseFlagsMagicNumber;
            bool small_damage = filter.NotShowSmallDamage && ((int)hpcond <= (int)HPCondition.SmallDamage);
            return (!ndock_timeover) && (!small_damage);
        }

        //ListViewイベントハンドラ
        private static void ListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            SetListViewSorter((System.Windows.Forms.ListView)sender, e.Column);
        }
        #endregion

        //ヘルパー
        #region ヘルパー
        //1日以上のTimeSpanをToString
        private static string TimeSpanToString(int ndocktime)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ndocktime);
            string str = string.Format("{0}:{1}", ts.Days * 24 + ts.Hours, ts.ToString(@"mm\:ss"));
            return str;
        }
        private static string TimeSpanToString(TimeSpan ts)
        {
            return string.Format("{0}:{1}", ts.Days * 24 + ts.Hours, ts.ToString(@"mm\:ss"));
        }

        //ApiShip→リストビューのアイテム用
        private static string[] ToListViewItem(ApiShip oship, bool assignFlag)
        {
            //Enumを基準になるように書き換え
            SortMode[] mode = (SortMode[])Enum.GetValues(typeof(SortMode));
            //列のオブジェクト
            string[] row = new string[mode.Length];
            foreach (SortMode m in mode)
            {
                int i = (int)m;
                switch(m)
                {
                    //0個目
                    case SortMode.ID:
                        row[i] = oship.api_id.ToString();//ID
                        break;
                    case SortMode.Name:
                        row[i] = (assignFlag ? "★" : "") + oship.ShipName;//名前
                        break;
                    case SortMode.Level:
                        row[i] = oship.api_lv.ToString();//レベル
                        break;
                    case SortMode.ShipType:
                        row[i] = oship.ShipTypeName;//艦種
                        break;
                    case SortMode.Cond:
                        row[i] = oship.api_cond.ToString();//cond値
                        break;
                    //5個目完了
                    case SortMode.NdockTime:
                        row[i] = TimeSpanToString(oship.api_ndock_time);//入渠時間
                        break;
                    case SortMode.HP:
                        row[i] = string.Format("{0}/{1}", oship.api_nowhp, oship.api_maxhp);//HP
                        break;
                    case SortMode.Exp:
                        row[i] = oship.api_exp[0].ToString("N0");//TotalExp
                        break;
                    case SortMode.NdockItem:
                        row[i] = string.Join(", ", oship.api_ndock_item);//修理資材
                        break;
                    case SortMode.Karyoku:
                        row[i] = oship.api_karyoku[0].ToString();//火力
                        break;
                    //10個目完了
                    case SortMode.Raisou:
                        row[i] = oship.api_raisou[0].ToString();//雷装
                        break;
                    case SortMode.NightKaryoku:
                        row[i] = (oship.api_karyoku[0] + oship.api_raisou[0]).ToString();//夜戦火力
                        break;
                    case SortMode.Taisen:
                        row[i] = oship.api_taisen[0].ToString();//対潜
                        break;
                    case SortMode.Taiku:
                        row[i] = oship.api_taiku[0].ToString();//対空
                        break;
                    case SortMode.Soukou:
                        row[i] = oship.api_soukou[0].ToString();//装甲
                        break;
                    //15個目完了
                    case SortMode.Kaihi:
                        row[i] = oship.api_kaihi[0].ToString();//回避
                        break;
                    case SortMode.Tousai:
                        row[i] = oship.api_onslot.Sum().ToString();//搭載
                        break;
                    case SortMode.Length:
                        row[i] = Helper.MstSlotitemLengthToString(oship.api_leng);//射程
                        break;
                    case SortMode.Sakuteki:
                        row[i] = oship.api_sakuteki[0].ToString();//索敵
                        break;
                    case SortMode.Luck:
                        row[i] = string.Join("/", oship.api_lucky);//運
                        break;
                    //20個目完了
                    case SortMode.SallyArea:
                        row[i] = oship.api_sally_area.ToString();//出撃エリア
                        break;
                }
            }
            return row;
        }
        #endregion

        //別窓
        public static void SubWindow_UnitList_Switch(Form1 parentForm)
        {
            //もともと表示されていない場合
            if (!Config.ShowUnitList)
            {
                unitListViewer = new UnitListViewer();
                unitListViewer.FormClosed += new System.Windows.Forms.FormClosedEventHandler(unitListViewer_FormClosed);
                unitListViewer.Owner = parentForm;
                unitListViewer.DoIt();
                unitListViewer.Show();
                CallBacks.SetToolStripMenuItemChecked(parentForm.toolStripMenuItem24, parentForm.toolStripMenuItem24.GetCurrentParent(), true);
                Config.ShowUnitList = true;
            }
            //表示されている場合
            else
            {
                unitListViewer.Close();
            }
        }
        //サブウィンドウのフォームクローズイベント
        private static void unitListViewer_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            UnitListViewer form = sender as UnitListViewer;
            Form1 parentForm = (Form1)form.Owner;
            Config.ShowUnitList = false;
            CallBacks.SetToolStripMenuItemChecked(parentForm.toolStripMenuItem24, parentForm.toolStripMenuItem24.GetCurrentParent(), false);
        }

        //クエリからGoogleChartを作成
        public static void CreateGoogleChart(IEnumerable<ApiShip> queriedShips)
        {
            //艦種別に経験値を集計
            var summaryexp = new SortedDictionary<int, long>();
            foreach(var q in queriedShips)
            {
                var shiptype = q.DShip.api_stype;
                long exp;
                summaryexp.TryGetValue(shiptype, out exp);
                exp += q.api_exp != null ? q.api_exp[0] : 0;

                summaryexp[shiptype] = exp;
            }
            //キャラ別に集計するか
            var isSummariesByChara = summaryexp.Count == 1;
            if(isSummariesByChara)
            {
                summaryexp = new SortedDictionary<int, long>();
                foreach(var q in queriedShips)
                {
                    var ship = q.DShip.api_id;
                    long exp;
                    summaryexp.TryGetValue(ship, out exp);
                    exp += q.api_exp != null ? q.api_exp[0] : 0;

                    summaryexp[ship] = exp;
                }
            }
            //円グラフの割合に変更
            var sum = summaryexp.Values.Sum();
            if (sum == 0) return;
            var data = summaryexp.Values.Select(x => (float)(x * 100 / sum)).ToArray();
            //ラベル
            string[] labels;
            if (isSummariesByChara)
            {
                labels = summaryexp.Keys.Select(delegate(int x)
                {
                    ExMasterShip mstship;
                    if (APIMaster.MstShips.TryGetValue(x, out mstship))
                    {
                        if (mstship.api_name != null) return mstship.api_name;
                        else return "";
                    }
                    else return "";
                }).ToArray();
            }
            else
            {
                labels = summaryexp.Keys.Select(delegate(int x)
                    {
                        ApiMstStype mststype;
                        if (APIMaster.MstStypesDictionary.TryGetValue(x, out mststype))
                        {
                            if (mststype.api_name != null)
                            {
                                switch(mststype.api_id)
                                {
                                    case 8: return "高速戦艦";
                                    case 9: return "低速戦艦";
                                    default: return mststype.api_name;
                                }
                            }
                            else return "";
                        }
                        else
                        {
                            return "";
                        }
                    }).ToArray();
            }
            //グラフサイズ
            var log = Math.Log10(sum)-5;//10^6が最低で1、10^9が最高で4
            var ratio = Math.Max(Math.Min(log, 4.0), 1.0);//解像度の倍率
            var pixely = (int)(96.0 * ratio);
            //グラフを作成
            var pieChart = new PieChart(pixely * 2, pixely, PieChartType.ThreeD);
            pieChart.SetTitle(sum.ToString("N0") + " Exp");
            pieChart.SetData(data);
            pieChart.SetPieChartLabels(labels);
            //色
            pieChart.SetDatasetColors(new string[] { "FF66FF", "FFFF66", "66FFFF" });
            //URLを取得
            var url = pieChart.GetUrl();
            Process.Start(url);
        }

        //更新処理のイベントハンドラー
        #region イベントハンドラー
        //イベントハンドラーの共通処理
        private static void EventHandlerCommon(System.Windows.Forms.Control sender, bool fullrefresh)
        {
            //親のタブ
            DockingWindows.DockWindowTabCollection tab = DockingWindows.DockWindowTabCollection.GetCollection(sender);
            //タブの元フォーム
            Form1 parent = tab.MainScreen;
            if (fullrefresh)
            {
                //親のタブのリストビューの更新
                tab.Unit.TabUnit_ListViewUpdate_Q();
            }
            else
            {
                //親のタブの計算部分のみ更新
                tab.Unit.TabUnit_TimeUpdate();
            }
        }

        //ToolStripMenuItem関連
        //クエリの切り替え
        public static void ToolStripMenuItem_Query_DropDownItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            //親のタブ
            UserControls.TabUnit tab = (sender as System.Windows.Forms.ToolStripMenuItem).Tag as UserControls.TabUnit;
            //クエリの切り替え
            int id = (int)e.ClickedItem.Tag;
            tab.UsingQuery = Queries[id];
            //チェックの切り替え
            tab.MenuItemQuery_CheckedChange(id);
            //クエリ読み込みフラグ
            tab.IsQueryLoaded = true;
            //親のタブのリストビューの更新
            tab.TabUnit_ListViewUpdate_Q();
        }

        //フィルターの切り替え
        public static void ToolStripMenuItem_Filter_DropDownItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            //親のタブ
            System.Windows.Forms.ToolStrip strip = (sender as System.Windows.Forms.ToolStripMenuItem).GetCurrentParent();
            DockingWindows.DockWindowTabCollection tab = DockingWindows.DockWindowTabCollection.GetCollection(strip);
            //選択されたItemのチェックの変更
            if (e.ClickedItem is System.Windows.Forms.ToolStripMenuItem)
            {
                System.Windows.Forms.ToolStripMenuItem menuitem = e.ClickedItem as System.Windows.Forms.ToolStripMenuItem;
                CallBacks.SetToolStripMenuItemChecked(menuitem, strip, !menuitem.Checked);
            }
            //フィルターのアップデート
            tab.Unit.TabUnit_FilterUpdate();
            //所属艦の非表示だったら
            if (e.ClickedItem.Text.Contains("所属艦"))
            {
                //親のタブのリストビューの更新
                tab.Unit.TabUnit_ListViewUpdate_Q();
            }
            else
            {
                //タブの計算部分
                tab.Unit.TabUnit_TimeUpdate();
            }
        }

        //コンボボックス切り替え
        public static void ToolStripComboBox_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //親のタブ
            UserControls.TabUnit unit = (sender as System.Windows.Forms.ToolStripComboBox).Tag as UserControls.TabUnit;
            //フィルターのアップデート
            unit.TabUnit_FilterUpdate();
            //タブの計算部分
            unit.TabUnit_TimeUpdate();
        }

        //アイテム選択時に艦名を表示する
        private void ListViewUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView list = sender as System.Windows.Forms.ListView;
            if (list.SelectedItems.Count <= 0) return;
            System.Windows.Forms.ListViewItem selected = list.SelectedItems[0];
            ApiShip oship = (ApiShip)selected.Tag;
            //親フォームの取得
            Form1 parent = DockingWindows.DockWindowTabCollection.GetCollection(list).MainScreen;
            parent.SetTextToLabelStatusTimer(string.Format("{0} Lv{1}", oship.ShipName, oship.api_lv), 5000);
        }
        #endregion
    }
}
