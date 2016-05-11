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
    public partial class TabMaterial : UserControl, ITabControl
    {
        Label[] label_material, label_material_diff;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        public TabMaterial()
        {
            InitializeComponent();
        }

        //変数の設定
        public void Init()
        {
            //資材タブ
            label_material = new Label[]
            {
                label_material_1, label_material_2, label_material_3, label_material_4,
                label_material_5, label_material_6, label_material_7, label_material_8,
            };
            label_material_diff = new Label[]
            {
                label_material_diff_1, label_material_diff_2, label_material_diff_3, label_material_diff_4,
                label_material_diff_5, label_material_diff_6, label_material_diff_7, label_material_diff_8,
            };
            //コンテキストメニュー
            toolStripMenuItem_mode.DropDownItemClicked += new ToolStripItemClickedEventHandler(contextSubMenu_DropDownItemClicked);
            toolStripMenuItem_diff.DropDownItemClicked += new ToolStripItemClickedEventHandler(contextSubMenu_DropDownItemClicked);
            toolStripMenuItem_term.DropDownItemClicked += new ToolStripItemClickedEventHandler(contextSubMenu_DropDownItemClicked);

            InitFinished = true;
        }

        public void Init2()
        {
            Init2Finished = true;
        }

        //資材タブの更新
        public Task TabMaterialUpdate()
        {
            if (!InitFinished) return Task.Factory.StartNew(() => { });
            return Task.Factory.StartNew(() =>
            {
                KancolleInfoMaterial.SetMaterial(label_material, label_material_diff);
            });
        }
        public void TabMaterialUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
            {
                KancolleInfoMaterial.SetMaterial(label_material, label_material_diff);
            }));
        }


        //資材グラフの更新
        public void TabMaterial_GraphUpdate_Q(object sender, EventArgs e)
        {
            if (!InitFinished) return;
            if (!HistoricalData.IsInited) return;
            int mode = 1 + Convert.ToInt32(toolStripMenuItem12.Checked);
            bool isdiff = toolStripMenuItem22.Checked;
            //期間
            GraphInfoTerm term;
            if (toolStripMenuItem31.Checked) term = GraphInfoTerm.All;
            else if (toolStripMenuItem32.Checked) term = GraphInfoTerm.Week;
            else term = GraphInfoTerm.Day;

            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfoMaterial.DrawMaterialGraph(chart_material, new GraphInfo { Mode = mode, IsDiff = isdiff, Term = term });
                }));
        }

        //コンテキストメニュー
        void contextSubMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolStripMenuItem11)
            {
                toolStripMenuItem11.Checked = true;
                toolStripMenuItem12.Checked = false;
            }
            else if (e.ClickedItem == toolStripMenuItem12)
            {
                toolStripMenuItem12.Checked = true;
                toolStripMenuItem11.Checked = false;
            }
            else if (e.ClickedItem == toolStripMenuItem21)
            {
                toolStripMenuItem21.Checked = true;
                toolStripMenuItem22.Checked = false;
            }
            else if (e.ClickedItem == toolStripMenuItem22)
            {
                toolStripMenuItem22.Checked = true;
                toolStripMenuItem21.Checked = false;
            }
            else if (e.ClickedItem == toolStripMenuItem31)
            {
                toolStripMenuItem31.Checked = true;
                toolStripMenuItem32.Checked = false;
                toolStripMenuItem33.Checked = false;
            }
            else if (e.ClickedItem == toolStripMenuItem32)
            {
                toolStripMenuItem32.Checked = true;
                toolStripMenuItem31.Checked = false;
                toolStripMenuItem33.Checked = false;
            }
            else if (e.ClickedItem == toolStripMenuItem33)
            {
                toolStripMenuItem33.Checked = true;
                toolStripMenuItem31.Checked = false;
                toolStripMenuItem32.Checked = false;
            }
            //グラフの再描画
            TabMaterial_GraphUpdate_Q(new object(), new EventArgs());
        }

        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(tableLayoutPanel1, "material");
        }

    }
}
