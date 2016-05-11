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

namespace VisualFormTest.UserControls
{
    public partial class TabFleet_MakeQuery : Form
    {
        //クエリのテンポラリ
        public UnitQuery TemporaryQuery { get; set; }
        //コレクション
        public DockingWindows.DockWindowTabCollection TabCollection { get; set; }

        public TabFleet_MakeQuery(UnitQuery query)
        {
            InitializeComponent();

            TemporaryQuery = query;
            //コンボボックスの初期化
            if (!KancolleInfoUnitList.IsInited) KancolleInfoUnitList.InitQuery();
            string[] queryname = (from x in KancolleInfoUnitList.Queries
                                  select string.Format("({0}){1}", x.ID, x.Name)).ToArray();
            comboBox1.Items.AddRange(queryname);
            //名前
            textBox1.Text = query.Name;
            textBox1.SelectionStart = 0;

            //クエリIDの自動探索
            TemporaryQuery.ID = 16;//デフォルトの値
            for(int i=16; i<KancolleInfoUnitList.QueriesMax; i++)
            {
                if(KancolleInfoUnitList.Queries[i].IsEmpty())
                {
                    TemporaryQuery.ID = i;
                    break;
                }
            }
            comboBox1.SelectedIndex = TemporaryQuery.ID;
        }

        //イベントハンドラー
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TemporaryQuery.ID = comboBox1.SelectedIndex;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TemporaryQuery.Name = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KancolleInfoUnitList.Queries[TemporaryQuery.ID] = TemporaryQuery;
            TabCollection.UnitPage_RefreshAllQueryName();
            this.Close();
        }

    }
}
