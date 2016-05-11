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
    public partial class TabSenka : UserControl, ITabControl
    {
        Label[] label_senka_admiralexp, label_senka_winlose, label_senka_border, label_senka_timertick, label_senka_border_header;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        ToolStripDropDown popup = new ToolStripDropDown();

        public TabSenka()
        {
            InitializeComponent();

            //タイマー
            admiralExpTimer.Start();
        }

        //戦果タブの初期化
        public void Init()
        {
            //戦果タブ
            label_senka_admiralexp = new Label[]
            {
                label_senka_admiral_1, label_senka_admiral_2, label_senka_admiral_3, label_senka_admiral_4,
                label_senka_admiral_5, label_senka_admiral_6, label_senka_admiral_7,
            };
            label_senka_winlose = new Label[]
            {
                label_senka_winlose_1, label_senka_winlose_2, label_senka_winlose_3,
            };
            label_senka_border = new Label[]
            {
                label_senka_border_1, label_senka_border_2, label_senka_border_3, label_senka_border_4,
                label_senka_border_5, label_senka_border_6, label_senka_border_7,
            };
            label_senka_timertick = new Label[]
            {
                label_senka_admiral_5, label_senka_admiral_6,
            };
            label_senka_border_header = new Label[]
            {
                label_senka_border_header_1, label_senka_border_header_2, label_senka_border_header_3, label_senka_border_header_4,
                label_senka_border_header_5, label_senka_border_header_6, label_senka_border_header_7
            };

            BorderDisplayChange();

            //ポップアップの設定
            popup.Margin = Padding.Empty;
            popup.Padding = Padding.Empty;

            InitFinished = true;
        }

        public void Init2()
        {
            if (!(this.FindForm() as WeifenLuo.WinFormsUI.Docking.DockContent).IsHandleCreated) return;

            //Tagの設定
            toolStripMenuItem_window.Tag = this;
            contextMenuStrip_senka.Tag = this;
            //イベントハンドラー
            contextMenuStrip_senka.ItemClicked += new ToolStripItemClickedEventHandler(KancolleInfoSenka.contextMenuStrip_senka_ItemClicked);
            toolStripMenuItem_mode.DropDownItemClicked += new ToolStripItemClickedEventHandler(contextSubMenu_DropDownItemCliceked);
            toolStripMenuItem_term.DropDownItemClicked += new ToolStripItemClickedEventHandler(contextSubMenu_DropDownItemCliceked);
            toolStripMenuItem_window.Click += new EventHandler(KancolleInfoSenka.button_senka_openchart_Click);

            Init2Finished = true;
        }

        //ボーダーの表示順位の更新
        public void BorderDisplayChange()
        {
            foreach(int i in Enumerable.Range(0, label_senka_border_header.Length))
            {
                label_senka_border_header[i].Text = Config.TabSenkaBorderDisplay[i] + "位";
                label_senka_border[i].Tag = Config.TabSenkaBorderDisplay[i];
            }
        }

        //戦果タブの更新
        public void TabSenkaUpdate()
        {
            if (!InitFinished) return;
            Task.Factory.StartNew(() =>
            {
                KancolleInfoSenka.SetAdmiralExp(label_senka_admiralexp, label_senka_winlose, label_senka_border);
            });
        }

        //戦果グラフ
        public void TabSenka_GraphUpdate(object sender, EventArgs e)
        {
            if (!InitFinished) return;
            if (!HistoricalData.IsInited) return;
            int mode = 1 + Convert.ToInt32(toolStripMenuItem12.Checked);
            GraphInfoTerm term;
            if (toolStripMenuItem21.Checked) term = GraphInfoTerm.All;
            else if (toolStripMenuItem22.Checked) term = GraphInfoTerm.Week;
            else term = GraphInfoTerm.Day;

            Task.Factory.StartNew(() =>
                {
                    KancolleInfoSenka.DrawExperienceGraph(chart_senka, new GraphInfo { Mode = mode, Term = term });
                });
        }

        //タイマー
        private void admiralExpTimer_Tick(object sender, EventArgs e)
        {
            if (!InitFinished) return;
            KancolleInfoSenka.TimerRefreshAdmiralExp(label_senka_timertick);
        }

        //ToolStrip
        private void contextSubMenu_DropDownItemCliceked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == toolStripMenuItem11)
            {
                toolStripMenuItem11.Checked = true;
                toolStripMenuItem12.Checked = false;
            }
            else if(e.ClickedItem == toolStripMenuItem12)
            {
                toolStripMenuItem12.Checked = true;
                toolStripMenuItem11.Checked = false;
            }
            else if(e.ClickedItem == toolStripMenuItem21)
            {
                toolStripMenuItem21.Checked = true;
                toolStripMenuItem22.Checked = false;
                toolStripMenuItem23.Checked = false;
            }
            else if(e.ClickedItem == toolStripMenuItem22)
            {
                toolStripMenuItem22.Checked = true;
                toolStripMenuItem21.Checked = false;
                toolStripMenuItem23.Checked = false;
            }
            else if(e.ClickedItem == toolStripMenuItem23)
            {
                toolStripMenuItem23.Checked = true;
                toolStripMenuItem21.Checked = false;
                toolStripMenuItem22.Checked = false;
            }
            //グラフの描画
            TabSenka_GraphUpdate(new object(), new EventArgs());
        }

        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(this, "senka");
        }

        private void toolStripMenuItem_border_Click(object sender, EventArgs e)
        {
            TabSenka_BorderSetting setting = new TabSenka_BorderSetting();
            setting.FormClosing += (ss, ee) =>
                {
                    if (setting.RefreshRequired)
                    {
                        BorderDisplayChange();
                        TabSenkaUpdate();
                    }
                };

            setting.ShowDialog();
        }

        //戦果ポップアップ
        private void label_senka_admiral_1_Click(object sender, EventArgs e)
        {
            TabSenka_Popup frm = new TabSenka_Popup();
            frm.TopLevel = false;

            ToolStripControlHost host = new ToolStripControlHost(frm);
            host.Margin = Padding.Empty;
            host.Padding = Padding.Empty;

            popup.Items.Clear();
            popup.Items.Add(host);
            popup.Show(new Point(MousePosition.X, MousePosition.Y));
        }
    }
}
