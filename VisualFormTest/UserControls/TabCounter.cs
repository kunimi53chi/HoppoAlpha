using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest.UserControls
{
    public partial class TabCounter : UserControl, ITabControl
    {
        public Control[,] control_counter;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        public TabCounter()
        {
            InitializeComponent();
        }

        //カウンターの初期化
        public void Init()
        {
            //counter配列が変わったのでIndexの変更
            //コントロールの配列
            control_counter = new Control[,]{
                {checkBox_counter_1, comboBox_counter_11, comboBox_counter_12, comboBox_counter_13, button_counter_modify_1,
                    textBox_counter_1, button_counter_11, button_counter_12, button_counter_13, button_counter_14,
                    label_counter1},
                {checkBox_counter_2, comboBox_counter_21, comboBox_counter_22, comboBox_counter_23, button_counter_modify_2,
                    textBox_counter_2, button_counter_21, button_counter_22, button_counter_23, button_counter_24,
                    label_counter2},
                {checkBox_counter_3, comboBox_counter_31, comboBox_counter_32, comboBox_counter_33, button_counter_modify_3,
                    textBox_counter_3, button_counter_31, button_counter_32, button_counter_33, button_counter_34,
                    label_counter3},
            };
            contextMenuStrip_counterTime.Tag = control_counter;
            //データソースのセットとコントロールのハンドラー
            for (int i = 0; i < control_counter.GetLength(0); i++)
            {
                //イベントハンドラーのセット
                CheckBox check = (CheckBox)control_counter[i, 0];
                check.CheckedChanged += new EventHandler(KancolleInfoCounter.counterCheckBoxCheckedChanged);
                //コンボボックス（全マップ）
                ComboBox combo = (ComboBox)control_counter[i, 1];
                combo.SelectedIndexChanged += new EventHandler(KancolleInfoCounter.counterComboBoxSelectedIndexChanged);
                //ボタン
                Button but = (Button)control_counter[i, 4];
                but.Click += new EventHandler(KancolleInfoCounter.counterConditionModifyButtonClick);
                but = (Button)control_counter[i, 6];
                but.Click += new EventHandler(KancolleInfoCounter.counterPlusButtonClick);
                but = (Button)control_counter[i, 7];
                but.Click += new EventHandler(KancolleInfoCounter.counterMinusButtonClick);
                but = (Button)control_counter[i, 8];
                but.Click += new EventHandler(KancolleInfoCounter.counterZeroButtonClick);
                but = (Button)control_counter[i, 9];
                but.Click += new EventHandler(KancolleInfoCounter.counterResetButtonClick);
            }
            //期間変更のイベントハンドラ
            contextMenuStrip_counterTime.ItemClicked += new ToolStripItemClickedEventHandler(KancolleInfoCounter.contextMenuStrip_counterTime_ItemClicked);


            InitFinished = true;
        }



        public void Init2()
        {
            if (!(this.FindForm() as WeifenLuo.WinFormsUI.Docking.DockContent).IsHandleCreated) return;

            KancolleInfoCounter.Init(control_counter);

            Init2Finished = true;
        }
    }
}
