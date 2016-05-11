using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;

namespace VisualFormTest
{
    public partial class UnitListViewer : Form
    {
        //クエリ
        public UnitQuery UsingQuery { get; set; }
        //フィルタ
        public UnitQueryFilter UsingFilter { get; set; }
        //条件にマッチした船のリスト
        public IEnumerable<ApiShip> QueriedShips { get; set; }

        public UnitListViewer()
        {
            InitializeComponent();

            KancolleInfoUnitList.Init(listView1);

            //プロパティの初期化
            ReadQuery(Config.SubWindowUnitListQueryNo);
            UsingFilter = new UnitQueryFilter();

        }

        private void UnitListViewer_Load(object sender, EventArgs e)
        {
            DoIt();
        }


        public class CsvList<T> : System.Collections.Generic.List<string>
        {
            // List<t>のAddを隠蔽し再定義（orverrideできないため）
            public new void Add(string item)
            {
                // ダブルクォーテーションで文字列を囲む
                base.Add("\"" + item + "\"");
            }
        }

        //実行部分
        public void DoIt()
        {
            if (UsingQuery == null) return;
            if (APIPort.ShipsDictionary == null) return;
            QueriedShips = KancolleInfoUnitList.DoQuery(UsingQuery, UsingFilter.NotShowFleetAssignFlag);
            KancolleInfoUnitList.DoIt(null, null, listView1, QueriedShips, UsingFilter);
            //合計経験値の表示
            UpdateTotalExp(false);
        }

        //経験値のアップデート
        public void UpdateTotalExp(bool isQueuing)
        {
            if (UsingQuery == null) return;
            //合計経験値の表示
            Action act = () =>
                {
                    var totalexp = QueriedShips.Select(x => x.api_exp != null ? x.api_exp[0] : 0).Sum();
                    CallBacks.SetToolStripMenuItemText(toolStripMenuItem_totalexp, menuStrip1, "合計経験値 : " + totalexp.ToString("N0"));
                };
            if (isQueuing)
            {
                Action act_query = () =>
                    {
                        QueriedShips = KancolleInfoUnitList.DoQuery(UsingQuery, UsingFilter.NotShowFleetAssignFlag);
                    };
                UIMethods.UIQueue.Enqueue(new UIMethods(act_query));
                UIMethods.UIQueue.Enqueue(new UIMethods(act));
            }
            else act();
        }

        //クエリの読み込み
        public void ReadQuery(int id)
        {
            if (KancolleInfoUnitList.Queries == null)
            {
                if(APIPort.Basic != null && APIPort.Basic.api_member_id != null)
                  KancolleInfoUnitList.InitQuery();//テスト
            }
            UsingQuery = KancolleInfoUnitList.Queries[id];
            this.Text = string.Format("艦娘リスト : ({0}){1}", UsingQuery.ID, UsingQuery.Name);
            Config.SubWindowUnitListQueryNo = id;
        }

        //クリップボードにコピー
        public void CopyToClipboad()
        {
            StringBuilder sb = new StringBuilder();
            //列
            CsvList<string> header = new CsvList<string>();
            foreach (ColumnHeader x in listView1.Columns)
            {
                header.Add(x.Text);
            }
            sb.AppendLine(string.Join("\t", header));            
            //中身
            foreach(ListViewItem x in listView1.Items)
            {
                CsvList<string> row = new CsvList<string>();
                for(int i=0; i<x.SubItems.Count; i++)
                {
                    row.Add(x.SubItems[i].Text);
                }
                //列の追加
                sb.AppendLine(string.Join("\t", row));
            }
            //クリップボードにコピー
            Clipboard.SetDataObject(sb.ToString(), true);
        }

        //メニューイベント
        private void toolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == toolStripMenuItem2)
            {
                DoIt();
            }
            else if(e.ClickedItem == toolStripMenuItem3)
            {
                CopyToClipboad();
            }
            else if(e.ClickedItem == toolStripMenuItem4)
            {
                UsingFilter.NotShowStar = !UsingFilter.NotShowStar;
                toolStripMenuItem4.Checked = !toolStripMenuItem4.Checked;
                DoIt();
            }
        }

        //クエリ番号の変更
        private void ChangeQuery()
        {
            //数字に変換できない場合
            int id;
            if (!int.TryParse(toolStripTextBox1.Text, out id)) return;
            //IDが範囲外の場合
            if (id < 0 || id >= KancolleInfoUnitList.QueriesMax) return;
            //クエリの読み込み
            ReadQuery(id);

            //実行
            DoIt();
        }

        //クエリ番号の変更
        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            ChangeQuery();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ChangeQuery();
            }
        }

        //グラフを表示
        private void toolStripMenuItem_graph_Click(object sender, EventArgs e)
        {
            KancolleInfoUnitList.CreateGoogleChart(QueriedShips);
        }


    }
}
