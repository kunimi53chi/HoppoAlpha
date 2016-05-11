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

namespace VisualFormTest
{
    public partial class QueryNestedEdit : Form
    {
        //SearchItem
        public UnitQueryItem Item { get; set; }
        //親フォームに渡すプロパティ
        public bool SaveFlag { get; set; }
        public int QueryID { get; set; }
        public int QueryItemID { get; set; }

        public static readonly string FormName = "ネストクエリ";


        public QueryNestedEdit(UnitQueryItem item, int queryid, int queryitemid)
        {
            InitializeComponent();

            Item = item;

            //上の表示
            label_condition.Text = item.TargetEnum.ToStr();
            toolTip1.SetToolTip(label_condition, item.TargetEnum.GetToolTipMessage());
            label_and.Text = item.IsAnd ? "○" : "×";
            label_not.Text = item.IsNot ? "○" : "×";
            string tips_type = "";
            if (item.TargetEnum.GetInstanceType() == typeof(string))
            {
                label_type.Text = "文字列";
                dataGridView1.Columns[0].DefaultCellStyle.Format = "g";
                tips_type = "文字列で判定します。\n検索モードは「文字列の検索モード」を使用します。";
            }
            else if (item.TargetEnum.GetInstanceType() == typeof(int))
            {
                label_type.Text = "整数";
                dataGridView1.Columns[0].DefaultCellStyle.Format = "d";
                tips_type = "整数で判定します。\n検索モードは「数値の検索モード」を使用します。";
            }
            else if (item.TargetEnum.GetInstanceType() == typeof(double))
            {
                label_type.Text = "小数";
                dataGridView1.Columns[0].DefaultCellStyle.Format = "f";
                tips_type = "小数で判定します。\n検索モードは「数値の検索モード」を使用します。";
            }
            toolTip1.SetToolTip(label_type, tips_type);
           

            //Searchesの追加
            (dataGridView1.Columns[1] as DataGridViewComboBoxColumn).ValueMember = "常にNO";
            dataGridView1.RowCount = item.Searches.Count + 1;
            foreach(int i in Enumerable.Range(0, item.Searches.Count))
            {
                var unitbase = item.Searches[i];
                //行のデータ
                dataGridView1.Rows[i].Cells[0].Value = unitbase.Value.ToString();
                dataGridView1.Rows[i].Cells[1].Value = (dataGridView1.Rows[i].Cells[1] as DataGridViewComboBoxCell).Items[unitbase.Range].ToString();
                dataGridView1.Rows[i].Cells[2].Value = (dataGridView1.Rows[i].Cells[2] as DataGridViewComboBoxCell).Items[unitbase.Match].ToString();
            }

            //ReadOnlyの切り替え
            Type insttype = item.TargetEnum.GetInstanceType();
            if(insttype == typeof(string))
            {
                dataGridView1.Columns[2].ReadOnly = false;
                dataGridView1.Columns[1].ReadOnly = true;
            }
            else
            {
                dataGridView1.Columns[1].ReadOnly = false;
                dataGridView1.Columns[2].ReadOnly = true;
            }

            //JSONテキストボックスのアップデート
            QueryID = queryid;
            QueryItemID = queryitemid;
            UpdateJsonTextBox(Item);

            //イベントハンドラー
            //DGV
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            dataGridView1.CellValidated += new DataGridViewCellEventHandler(dataGridView1_CellValidated);
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
        }




        
        //クエリJSONの更新
        private void UpdateJsonTextBox(UnitQueryItem item)
        {
            textBox_queryjson.Text = item.ToSearchesJson();
            //変更されたかどうかのチェック
            string text = string.Format("{0}-{1}", QueryID, QueryItemID);
            if (ModifyChecker()) this.Text = string.Format("{0} * - {1}", text, FormName);
            else this.Text = string.Format("{0} - {1}", text, FormName);
        }

        //コントールからの取得
        public UnitQueryItem GetItemFormDataGridView()
        {
            UnitQueryItem uqi = new UnitQueryItem(Item.TargetEnum);
            //変更不可のプロパティのコピー
            uqi.IsAnd = Item.IsAnd;
            uqi.IsNot = Item.IsNot;
            //列挙体のアイテム一覧
            string[] enum_match = (from x in (UnitQueryMatchMode[])Enum.GetValues(typeof(UnitQueryMatchMode))
                                    select x.ToStr()).ToArray();
            string[] enum_range = (from x in (UnitQueryRangeMode[])Enum.GetValues(typeof(UnitQueryRangeMode))
                                    select x.ToStr()).ToArray();
            //DataGridViewのコピー
            foreach(DataGridViewRow r in dataGridView1.Rows)
            {
                //入力されていないセルだったらスキップ
                if (r.Cells[0].FormattedValue.ToString() == "") continue;
                UnitQueryItemSearchBase sb = new UnitQueryItemSearchBase();
                //値のキャスト
                Type uqi_type = uqi.TargetEnum.GetInstanceType();
                if (uqi_type == typeof(string)) sb.Value = r.Cells[0].FormattedValue.ToString();
                else if (uqi_type == typeof(int)) sb.Value = Convert.ToInt32(r.Cells[0].FormattedValue);
                else if (uqi_type == typeof(double)) sb.Value = Convert.ToDouble(r.Cells[0].FormattedValue);
                //マッチモードの探索
                int range = Array.IndexOf(enum_range, r.Cells[1].FormattedValue);
                int match = Array.IndexOf(enum_match, r.Cells[2].FormattedValue);
                if(match == -1 || range == -1) throw new IndexOutOfRangeException();
                sb.Match = match;
                sb.Range = range;
                //追加
                uqi.Searches.Add(sb);
            }
            //返り値
            return uqi;
        }

        //編集されたかどうかのチェッカー
        public bool ModifyChecker()
        {
            string original = Item.ToJson();
            string modified = GetItemFormDataGridView().ToJson();
            return original != modified;
        }

        //終了を取り消すかどうかのチェック
        public bool FormCloseAborter()
        {
            if (SaveFlag) return false;
            //編集されていた場合
            if(ModifyChecker())
            {
                DialogResult result = MessageBox.Show("変更を保存しますか？", "保存", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch(result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveFlag = true;
                        return false;
                    case System.Windows.Forms.DialogResult.No:
                        SaveFlag = false;
                        return false;
                    case System.Windows.Forms.DialogResult.Cancel:
                        SaveFlag = false;
                        return true;
                    default:
                        throw new ArgumentException();
                }
            }
            else
            {
                return false;
            }
        }

        //イベントハンドラー
        #region イベントハンドラー
        //DataGridViewのセル内容変更
        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //テキストボックスの変更
            UnitQueryItem uqi = GetItemFormDataGridView();
            UpdateJsonTextBox(uqi);
        }

        //キャストチェック
        void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //新しい行のセルでなく、セルの内容が変更されている時だけ検証する
            if (e.RowIndex == dataGridView1.NewRowIndex || !dataGridView1.IsCurrentCellDirty)
            {
                return;
            }
            //1列目以外のチェック
            object val = e.FormattedValue;
            if(e.ColumnIndex != 0)
            {
                //自動入力
                if(dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString() == "")
                {
                    if (Item.TargetEnum.GetInstanceType() == typeof(string)) dataGridView1.Rows[e.RowIndex].Cells[0].Value = "dummy";
                    else dataGridView1.Rows[e.RowIndex].Cells[0].Value = 0;
                }
                return;
            }
            //Nullチェック
            if(val.ToString() == "")
            {
                dataGridView1.Rows[e.RowIndex].ErrorText = "値が入力されていません";
                dataGridView1.CancelEdit();
                e.Cancel = true;
                return;
            }
            //型のチェック
            Type item_type = Item.TargetEnum.GetInstanceType();
            if(item_type == typeof(int))
            {
                int intoutval;
                if(!int.TryParse(val.ToString(), out intoutval))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "値は 整数 でなくてはいけません";
                    dataGridView1.CancelEdit();
                    e.Cancel = true;
                }
            }
            else if(item_type == typeof(double))
            {
                double doubleoutval;
                if (!double.TryParse(val.ToString(), out doubleoutval))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "値は 小数 でなくてはいけません";
                    dataGridView1.CancelEdit();
                    e.Cancel = true;
                }
            }
        }

        void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //エラーテキストを消す
            dataGridView1.Rows[e.RowIndex].ErrorText = null;
        }


        //削除イベント
        void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3) return;
            //消せる場合
            if (e.RowIndex < dataGridView1.RowCount-1)
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
            UpdateJsonTextBox(GetItemFormDataGridView());
        }



        //OKボタン
        private void button_ok_Click(object sender, EventArgs e)
        {
            SaveFlag = true;
            this.Close();
        }

        //キャンセル
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //フォームを閉じようとした時のイベント
        private void QueryNestedEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームを閉じるのを中断する必要性がある場合
            e.Cancel = FormCloseAborter();
        }
        #endregion



    }
}
