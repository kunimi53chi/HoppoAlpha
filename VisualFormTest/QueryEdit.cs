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
    public partial class QueryEdit : Form
    {

        //タイトルバーの名前
        private const string TitleName = "クエリ編集";
        //クエリアイテムの上限
        public static readonly int QueryItemMax = 20;
        //コントロールの配列
        public ComboBox[] Conditions { get; set; }
        public CheckBox[] IsAnd { get; set; }
        public CheckBox[] IsNot { get; set; }
        public TextBox[] Searches { get; set; }
        public Button[] Edit { get; set; }
        public Button[] Init { get; set; }
        //イベントハンドラーの停止
        public bool EventHandlerSuspended { get; set; }
        //保存するフラグ
        public bool SaveFlag{get; set;}

        //使っているクエリ
        public UnitQuery UsingQuery { get; set; }

        //タブコレクションのアクセサ
        public DockingWindows.DockWindowTabCollection TabCollection { get; set; }

        //コンストラクタ
        #region コンストラクタ
        public QueryEdit()
        {
            InitializeComponent();

            #region 変数の初期化
            //クエリ名
            ResetQueryName();
            //コントール配列
            Conditions = new ComboBox[]
            {
                comboBox1, comboBox2, comboBox3, comboBox4, comboBox5,
                comboBox6, comboBox7, comboBox8, comboBox9, comboBox10,
                comboBox11, comboBox12, comboBox13, comboBox14, comboBox15,
                comboBox16, comboBox17, comboBox18, comboBox19, comboBox20,
            };
            IsAnd = new CheckBox[]
            {
                checkBox1_1, checkBox2_1, checkBox3_1, checkBox4_1, checkBox5_1,
                checkBox6_1, checkBox7_1, checkBox8_1, checkBox9_1, checkBox10_1,
                checkBox11_1, checkBox12_1, checkBox13_1, checkBox14_1, checkBox15_1,
                checkBox16_1, checkBox17_1, checkBox18_1, checkBox19_1, checkBox20_1,
            };
            IsNot = new CheckBox[]
            {
                checkBox1_2, checkBox2_2, checkBox3_2, checkBox4_2, checkBox5_2,
                checkBox6_2, checkBox7_2, checkBox8_2, checkBox9_2, checkBox10_2,
                checkBox11_2, checkBox12_2, checkBox13_2, checkBox14_2, checkBox15_2,
                checkBox16_2, checkBox17_2, checkBox18_2, checkBox19_2, checkBox20_2,
            };
            Searches = new TextBox[]
            {
                textBox1, textBox2, textBox3, textBox4, textBox5,
                textBox6, textBox7, textBox8, textBox9, textBox10,
                textBox11, textBox12, textBox13, textBox14, textBox15,
                textBox16, textBox17, textBox18, textBox19, textBox20,
            };
            Edit = new Button[]
            {
                button1_1, button2_1, button3_1, button4_1, button5_1,
                button6_1, button7_1, button8_1, button9_1, button10_1,
                button11_1, button12_1, button13_1, button14_1, button15_1,
                button16_1, button17_1, button18_1, button19_1, button20_1,
            };
            Init = new Button[]
            {
                button1_2, button2_2, button3_2, button4_2, button5_2,
                button6_2, button7_2, button8_2, button9_2, button10_2,
                button11_2, button12_2, button13_2, button14_2, button15_2,
                button16_2, button17_2, button18_2, button19_2, button20_2,
            };
            //コンボボックスアイテム
            UnitQueryMode[] mode_enum = (UnitQueryMode[])Enum.GetValues(typeof(UnitQueryMode));
            var mode_str = (from x in mode_enum
                           select UnitQueryEnumExt.ToStr(x)).ToArray();
            //モードのコンボボックスにアイテムを追加
            foreach(ComboBox x in Conditions)
            {
                x.Items.AddRange(mode_str);
            }
            #endregion

            //イベントハンドラ
            comboBox_query.SelectedIndexChanged += comboBox_query_SelectedIndexChanged;
            textBox_queryname.TextChanged += new EventHandler(textBox_TextChanged);
            radioButton_cmode_and.CheckedChanged += new EventHandler(textBox_TextChanged);
            //クエリの変更
            foreach(int i in Enumerable.Range(0, QueryItemMax))
            {
                Edit[i].Click += new EventHandler(button_Edit_Click);
                Init[i].Click += new EventHandler(button_Init_Click);
                IsAnd[i].CheckedChanged += new EventHandler(textBox_TextChanged);
                IsNot[i].CheckedChanged += new EventHandler(textBox_TextChanged);
                Searches[i].TextChanged += new EventHandler(textBox_TextChanged);
            }

        }
        #endregion


        //クエリ名の取得
        public void ResetQueryName()
        {
            string[] name = new string[KancolleInfoUnitList.QueriesMax];
            foreach(int i in Enumerable.Range(0, name.Length))
            {
                UnitQuery query = KancolleInfoUnitList.Queries[i];
                name[i] = string.Format("({0}) {1}", query.ID, query.Name);
            }
            comboBox_query.Items.Clear();
            comboBox_query.Items.AddRange(name);

        }

        //N番目のクエリの読み込み
        public void ReadQuery(int id)
        {
            if (!KancolleInfoUnitList.IsInited) return;
            EventHandlerSuspended = true;
            //編集中の確認


            //読み込むクエリ
            UnitQuery query = KancolleInfoUnitList.Queries[id];
            //表示
            this.Text = string.Format("{0} : {1}", TitleName, query.ID);
            radioButton_cmode_or.Checked = query.IsOr;
            radioButton_cmode_and.Checked = !query.IsOr;
            textBox_queryname.Text = query.Name;
            textBox_queryjson.Text = query.ToJson();
            //Searchの追加
            foreach(int i in Enumerable.Range(0, Math.Min(query.Query.Count, QueryItemMax)))
            {
                UnitQueryItem search = query.Query[i];
                //モード
                Conditions[i].SelectedIndex = (int)search.TargetEnum;
                //AND
                IsAnd[i].Checked = search.IsAnd;
                //Not
                IsNot[i].Checked = search.IsNot;
                //値
                Searches[i].Text = search.ToSearchesJson();
            }
            //残りの初期化
            for (int i = query.Query.Count; i < QueryItemMax; i++ )
            {
                Conditions[i].SelectedIndex = -1;
                IsAnd[i].Checked = false; IsNot[i].Checked = false;
                Searches[i].Text = "";
            }
            //テンポラリ
            UsingQuery = KancolleInfoUnitList.Queries[id];

            //クエリのJSONの更新
            UpdateJsonTextBox();

            SaveFlag = false;
            EventHandlerSuspended = false;
        }

        //クエリの保存
        public void SaveQuery()
        {
            if (UsingQuery == null) return;
            int id = UsingQuery.ID;
            if (id < 0 || id >= KancolleInfoUnitList.QueriesMax) throw new IndexOutOfRangeException();
            KancolleInfoUnitList.Queries[id] = GetQueryFromControl();
            //変更済み表示の更新
            UpdateJsonTextBox();
            //親ウィンドウのクエリ名の更新
            TabCollection.UnitPage_RefreshAllQueryName();
            //このウィンドウのコンボボックスの更新
            ResetQueryName();
            comboBox_query.SelectedIndex = id;
        }

        //クエリJSONの更新
        private void UpdateJsonTextBox()
        {
            if (UsingQuery == null) return;
            textBox_queryjson.Text = GetQueryFromControl().ToJson();
            //変更されたかどうかのチェック
            string text = string.Format("{0} : {1}", TitleName, UsingQuery.ID);
            if (ModifyChecker()) this.Text = string.Format("{0} *", text);
            else this.Text = text;
        }

        //コントロールからクエリの取得
        public UnitQuery GetQueryFromControl()
        {
            UnitQuery query = new UnitQuery();
            if (UsingQuery == null) return query;
            //ID
            query.ID = UsingQuery.ID;
            //名前
            query.Name = textBox_queryname.Text;
            //IsOR
            query.IsOr = radioButton_cmode_or.Checked;
            //Searchs
            foreach(int i in Enumerable.Range(0, QueryItemMax))
            {
                //離脱
                if (Conditions[i].SelectedIndex == -1) break;
                //クエリアイテムの取得
                UnitQueryItem item = new UnitQueryItem((UnitQueryMode)Conditions[i].SelectedIndex);
                //IsAnd
                item.IsAnd = IsAnd[i].Checked;
                //IsNot
                item.IsNot = IsNot[i].Checked;
                //Searches
                if (Searches[i].Text == "") item.Searches = new List<UnitQueryItemSearchBase>();
                else item.Searches = UnitQueryItemSearchBaseExt.FromJson(Searches[i].Text);
                //追加
                query.Query.Add(item);
            }
            return query;
        }

        //編集チェック
        public bool ModifyChecker()
        {
            if (UsingQuery == null) return false;
            string original = UsingQuery.ToJson();
            string modified = GetQueryFromControl().ToJson();
            return original != modified;
        }

        //イベントハンドラー
        #region イベントハンドラー
        void comboBox_query_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = comboBox_query.SelectedIndex;
            if (n < 0 || n >= KancolleInfoUnitList.QueriesMax) return;
            ReadQuery(n);
        }

        void button_Edit_Click(object sender, EventArgs e)
        {
            int id = Array.IndexOf(Edit, sender as Button);
            //データのない列のチェック
            if (Conditions[id].SelectedIndex == -1) return;
            //ここを↓ベースのではなくTextBoxからパースさせる
            UnitQueryItem qui = new UnitQueryItem((UnitQueryMode)Conditions[id].SelectedIndex);
            qui.IsAnd = IsAnd[id].Checked;
            qui.IsNot = IsNot[id].Checked;
            if (Searches[id].Text.Length != 0) qui.Searches = UnitQueryItemExt.FromJsonToSearches(Searches[id].Text);
            else qui.Searches = new List<UnitQueryItemSearchBase>();
            QueryNestedEdit qne = new QueryNestedEdit(qui, KancolleInfoUnitList.Queries.IndexOf(UsingQuery), id);
            qne.FormClosed += new FormClosedEventHandler(qne_FormClosed);
            qne.ShowDialog();
        }

        void button_Init_Click(object sender, EventArgs e)
        {
            int id = Array.IndexOf(Init, sender as Button);
            EventHandlerSuspended = true;
            Conditions[id].SelectedIndex = -1;
            IsAnd[id].Checked = false;
            IsNot[id].Checked = false;
            //ここでイベントハンドラー再開
            EventHandlerSuspended = false;
            Searches[id].Text = "";
        }

        //サブフォームのFormClosed
        void qne_FormClosed(object sender, FormClosedEventArgs e)
        {
            QueryNestedEdit qne = sender as QueryNestedEdit;
            //上書きする場合
            if(qne.SaveFlag)
            {
                //テキストボックスの書き換え
                Searches[qne.QueryItemID].Text = qne.GetItemFormDataGridView().ToSearchesJson();
            }
        }

        //値の変更イベント
        void textBox_TextChanged(object sender, EventArgs e)
        {
            if (EventHandlerSuspended) return;
            UpdateJsonTextBox();
        }



        //閉じようとしたときのイベント
        private void QueryEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveFlag)
            {
                if (ModifyChecker())
                {
                    DialogResult result = MessageBox.Show("変更を保存しますか？", "保存", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        SaveFlag = true;
                        return;
                    }
                    else if (result == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //閉じるときのイベント
        private void QueryEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SaveFlag) SaveQuery();
        }

        //OKボタンのイベント
        private void button_ok_Click(object sender, EventArgs e)
        {
            SaveFlag = true;
            this.Close();
        }

        //キャンセルボタンのイベント
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //適用イベント
        private void button_apply_Click(object sender, EventArgs e)
        {
            SaveQuery();
        }
        #endregion

    }
}
