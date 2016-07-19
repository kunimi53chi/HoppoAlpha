using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest
{
    public partial class ChartViewer : Form
    {
        //表示する対象　0なら資源　1なら経験値
        public int Target { get; set; }
        //グラフインフォ
        public GraphInfo Info { get; set; }

        //メニューアイテムの配列
        ToolStripMenuItem[] item_subject, item_diff, item_term, item_palette, item_series;

        //カラーパレット
        static System.Windows.Forms.DataVisualization.Charting.ChartColorPalette[] Palette
            = (System.Windows.Forms.DataVisualization.Charting.ChartColorPalette[])Enum.GetValues(typeof(System.Windows.Forms.DataVisualization.Charting.ChartColorPalette));

        public ChartViewer(int target)
        {
            if (target >= 2) throw new ArgumentException();

            InitializeComponent();

            //対象
            this.Target = target;
            //インフォの整理
            this.Info = new GraphInfo();
            if(target == 0)
            {
                if (HistoricalData.GraphInfoMaterial.Mode != 0)
                {
                    //資材モードの場合
                    this.Info.IsDiff = HistoricalData.GraphInfoMaterial.IsDiff;
                    this.Info.Mode = HistoricalData.GraphInfoMaterial.Mode;
                    this.Info.Term = HistoricalData.GraphInfoMaterial.Term;
                }
                else
                {
                    this.Info.IsDiff = true;
                    this.Info.Mode = 1;
                    this.Info.Term = GraphInfoTerm.All;
                }
            }
            else
            {
                if (HistoricalData.GraphInfoMaterial.Mode != 0)
                {
                    //経験値モードの場合
                    this.Info.IsDiff = HistoricalData.GraphInfoExperience.IsDiff;
                    this.Info.Mode = HistoricalData.GraphInfoExperience.Mode;
                    this.Info.Term = HistoricalData.GraphInfoExperience.Term;
                }
                else
                {
                    this.Info.IsDiff = true;
                    this.Info.Mode = 1;
                    this.Info.Term = GraphInfoTerm.All;
                }
            }
            //メニューアイテムの整理
            item_subject = new ToolStripMenuItem[]
            {
                toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6, toolStripMenuItem7
            };
            item_diff = new ToolStripMenuItem[]
            {
                toolStripMenuItem8, toolStripMenuItem9,
            };
            item_term = new ToolStripMenuItem[]
            {
                toolStripMenuItem10, toolStripMenuItem11, toolStripMenuItem12,
            };
            item_series = new ToolStripMenuItem[]
            {
                toolStripMenuItem_s1, toolStripMenuItem_s2, toolStripMenuItem_s3, toolStripMenuItem_s4, toolStripMenuItem_s5,
                toolStripMenuItem_s6, toolStripMenuItem_s7, toolStripMenuItem_s8, toolStripMenuItem_s9, toolStripMenuItem_s10,
            };
            //パレットの追加
            item_palette = new ToolStripMenuItem[Palette.Length];
            for (int i = 0; i < item_palette.Length; i++ )
            {
                ToolStripMenuItem pitem = new ToolStripMenuItem(Palette[i].ToString());
                toolStripMenuItem13.DropDownItems.Add(pitem);
                item_palette[i] = pitem;
            }

            //メニューアイテムのチェック
            MenuItemCheckRefresh();
            //チャートを描く
            DrawChart();
        }

        //チャートを描く
        public void DrawChart()
        {
            if(this.Target == 0)
            {
                KancolleInfoMaterial.MaterialGraphInit(this.chart1, this.Info, false);
            }
            else
            {
                KancolleInfoSenka.ExperienceGraphInit(this.chart1, this.Info, false);
            }
        }

        //メニューアイテムの名前変更
        public void MenuItemCheckRefresh()
        {
            //項目の表示
            bool[] subject_flag = new bool[item_subject.Length];
            switch(this.Target)
            {
                case 0:
                    if (this.Info.Mode == 1) subject_flag[0] = true;//資源の前半
                    else subject_flag[1] = true;//資源の後半
                    break;
                case 1:
                    if(this.Info.Mode == 1) subject_flag[2] = true;//提督経験値
                    else subject_flag[3] = true;//ボーダー
                    break;
            }
            for(int i=0; i<subject_flag.Length; i++)
            {
                item_subject[i].Checked = subject_flag[i];
            }
            //計算方法
            bool[] diff_flag = new bool[item_diff.Length];
            if (this.Info.IsDiff) diff_flag[1] = true;
            else diff_flag[0] = true;
            for(int i=0; i<diff_flag.Length; i++)
            {
                item_diff[i].Checked = diff_flag[i];
            }
            //期間
            bool[] term_flag = new bool[item_term.Length];
            switch(this.Info.Term)
            {
                case GraphInfoTerm.All:
                    term_flag[0] = true;
                    break;
                case GraphInfoTerm.Week:
                    term_flag[1] = true;
                    break;
                case GraphInfoTerm.Day:
                    term_flag[2] = true;
                    break;
            }
            for(int i=0; i<term_flag.Length; i++)
            {
                item_term[i].Checked = term_flag[i];
            }
            //パレット
            int palette_index = Array.IndexOf(Palette, chart1.Palette);
            for(int i=0; i<item_palette.Length; i++)
            {
                if (i == palette_index) item_palette[i].Checked = true;
                else item_palette[i].Checked = false;
            }
            //系列
            for(int i=0; i<item_series.Length; i++)
            {
                if (this.Info.ExceptSeries.ContainsExceptSeries(i + 1)) item_series[i].Checked = false;
                else item_series[i].Checked = true;
            }
        }

        //イベントハンドラー
        //項目
        private void toolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //インデックスの取得
            int index = Array.IndexOf(item_subject, e.ClickedItem);
            //ターゲットの更新
            this.Target = index / 2;
            //モードの更新
            this.Info.Mode = 1 + (index % 2);
            //除外系列のリセット
            this.Info.ExceptSeries = (GraphExceptSeries)0;
            //チェックボタンの更新
            MenuItemCheckRefresh();
            //グラフの再描画
            DrawChart();
        }

        //計算方法
        private void toolStripMenuItem2_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //インデックスの取得
            int index = Array.IndexOf(item_diff, e.ClickedItem);
            //計算方法
            this.Info.IsDiff = index == 1;
            //チェックボタンの更新
            MenuItemCheckRefresh();
            //グラフの再描画
            DrawChart();        
        }

        //期間
        private void toolStripMenuItem3_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //インデックスの取得
            int index = Array.IndexOf(item_term, e.ClickedItem);
            //計算方法
            switch(index)
            {
                case 0:
                    this.Info.Term = GraphInfoTerm.All;
                    break;
                case 1:
                    this.Info.Term = GraphInfoTerm.Week;
                    break;
                case 2:
                    this.Info.Term = GraphInfoTerm.Day;
                    break;
            }
            //チェックボタンの更新
            MenuItemCheckRefresh();
            //グラフの再描画
            DrawChart();
        }

        //パレット
        private void toolStripMenuItem13_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int index = Array.IndexOf(item_palette, e.ClickedItem);
            var newpalette = Palette[index];
            chart1.Palette = newpalette;
            //チェックボタンの更新
            MenuItemCheckRefresh();
        }

        //除外系列のクリック
        private void toolStripMenuItem14_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int index = Array.IndexOf(item_series, e.ClickedItem);
            bool newflag = !item_series[index].Checked;
            item_series[index].Checked = newflag;

            var target = (GraphExceptSeries)((int)Math.Pow(2, index));
            //除外設定する場合
            if(!newflag)
            {
                this.Info.ExceptSeries = this.Info.ExceptSeries | target;
            }
            //フラグを解除する場合
            else
            {
                this.Info.ExceptSeries = this.Info.ExceptSeries & ~target;
            }

            //グラフの描画
            DrawChart();
        }
    }
}
