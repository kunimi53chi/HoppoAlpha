using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class KancolleInfoSenka
    {
        //個人戦果
        public static void SetAdmiralExp(System.Windows.Forms.Label[] label_exp, System.Windows.Forms.Label[] label_win,
            System.Windows.Forms.Label[] label_border)
        {
            //ベースとなるレコード
            ExpRecord expbase = HistoricalData.LogExperience[HistoricalData.LogExperience.Count - 1];
            //現在の提督経験値
            int nowexp = APIPort.Basic.api_experience;
            //提督経験値
            CallBacks.SetLabelTextAndColor(label_exp[0], string.Format("{0} ({1})", nowexp.ToString("N0"), APIPort.LastUpdate.ToString("HH:mm")),
                KancolleInfo.DefaultStringColor);
            //格納するテキスト
            string[] text = new string[6];
            //6時間前
            if (expbase.Before6H == null) text[0] = "-";
            else text[0] = string.Format("{0} ({1})", (nowexp - expbase.Before6H.Value).ToString("N0"),
                expbase.Before6H.Date.ToString("HH:mm"));
            //12時間前
            if (expbase.Before12H == null) text[1] = "-";
            else text[1] = string.Format("{0} ({1})", (nowexp - expbase.Before12H.Value).ToString("N0"),
                expbase.Before12H.Date.ToString("HH:mm"));
            //24時間前
            if (expbase.Before24H == null) text[2] = "-";
            else text[2] = string.Format("{0} ({1})", (nowexp - expbase.Before24H.Value).ToString("N0"),
                expbase.Before24H.Date.ToString("HH:mm"));
            //戦果ベース
            SenkaRecord senkabase = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
            //午前・午後
            string[] str_senka = senkabase.ShowSectionExp();
            text[3] = str_senka[0]; text[4] = str_senka[1];
            //推定戦果
            if (senkabase.EndSenkaEst < 0) text[5] = "NA";
            else text[5] = senkabase.EndSenkaEst.ToString("N1");
            //テキストのセット
            for (int i = 0; i < text.Length; i++)
            {
                CallBacks.SetLabelTextAndColor(label_exp[i + 1], text[i], KancolleInfo.DefaultStringColor);
            }
            //勝ち負け
            CallBacks.SetLabelTextAndColor(label_win[0], APIPort.Basic.api_st_win.ToString("N0"), KancolleInfo.DefaultStringColor);
            CallBacks.SetLabelTextAndColor(label_win[1], APIPort.Basic.api_st_lose.ToString("N0"), KancolleInfo.DefaultStringColor);
            double winratio = (double)APIPort.Basic.api_st_win / (double)(APIPort.Basic.api_st_win + APIPort.Basic.api_st_lose);
            CallBacks.SetLabelTextAndColor(label_win[2], winratio.ToString("P4"), KancolleInfo.DefaultStringColor);
            //ボーダー
            foreach (var x in label_border)
            {
                //取得する順位
                int rank = Convert.ToInt32(x.Tag) - 1;
                //現在のボーダー
                int nowborder = senkabase.TopSenka[rank];
                //戦果の差分
                int diffborder = -1;
                //直前のセクション
                if (senkabase.PrevContinuousSection != null)
                {
                    int prevborder = senkabase.PrevContinuousSection.TopSenka[rank];
                    if (nowborder != -1 && prevborder != -1) diffborder = nowborder - prevborder;
                }
                //文字の置き換え
                CallBacks.SetLabelTextAndColor(x, string.Format("{0} ({1})", (nowborder < 0 ? "NA" : nowborder.ToString("N0")), (diffborder < 0 ? "-" : "+" + diffborder)),
                    KancolleInfo.DefaultStringColor);
            }
        }

        //一般タブの提督経験値
        public static void SetAdmiralExp_General(System.Windows.Forms.Label[] labels)
        {
            //提督経験値
            int nowexp = APIPort.Basic.api_experience;
            CallBacks.SetLabelText(labels[0], nowexp.ToString("N0"));
            //階級とレベル
            CallBacks.SetLabelText(labels[1], string.Format("Lv{0}　{1}", APIPort.Basic.api_level, Helper.RankToString(APIPort.Basic.api_rank)));
        }

        public static void SetAdmiralExp_Short(System.Windows.Forms.Label[] label_section)
        {
            //戦果ベース
            SenkaRecord senkabase = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
            string[] text = new string[3];
            //午前・午後
            string[] str_senka = senkabase.ShowSectionExp();
            text[0] = str_senka[0]; text[1] = str_senka[1];
            //推定戦果
            if (senkabase.EndSenkaEst < 0) text[2] = "NA";
            else text[2] = senkabase.EndSenkaEst.ToString("N1");
            //テキストのセット
            foreach(int i in Enumerable.Range(0, text.Length))
            {
                CallBacks.SetLabelText(label_section[i], text[i]);
            }
        }

        //戦果のタイマーリフレッシュ
        public static void TimerRefreshAdmiralExp(System.Windows.Forms.Label[] labels)
        {
            if (!HistoricalData.IsInited) return;
            string[] str = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1].ShowSectionExp();
            for (int i = 0; i < labels.Length; i++)
            {
                CallBacks.SetLabelTextAndColor(labels[i], str[i], KancolleInfo.DefaultStringColor);
            }
        }

        //経験値グラフの表示
        public static void DrawExperienceGraph(System.Windows.Forms.DataVisualization.Charting.Chart chart, GraphInfo info)
        {
            if (chart.InvokeRequired)
            {
                CallBacks.SetChartCallBack d = new CallBacks.SetChartCallBack(DrawExperienceGraph);
                chart.Invoke(d, new object[] { chart, info });
            }
            else
            {
                //初期化が必要な場合
                if (HistoricalData.GraphInfoExperience != info) ExperienceGraphInit(chart, info, true);
                //そうでない場合
                else
                {
                    //古い値の消去
                    if (HistoricalData.GraphInfoExperience.Term != GraphInfoTerm.All)
                    {
                        DateTime mindate = HistoricalData.GraphInfoExperience.Term.GetMinDate();
                        //消去する必要がある場合
                        bool isdelete = DateTime.FromOADate(chart.Series[0].Points[0].XValue) < mindate;
                        if (isdelete)
                        {
                            //消去
                            for (int i = 0; i < Math.Min(5, chart.Series[0].Points.Count); i++)
                            {
                                DateTime date = DateTime.FromOADate(chart.Series[0].Points[i].XValue);
                                if (date >= mindate) break;//消す必要がなくなったら離脱
                                foreach (var x in chart.Series) x.Points.RemoveAt(i);
                            }
                        }
                    }
                    //最新の値の追加
                    if (info.Mode == 1)
                    {
                        ExpRecord latest = HistoricalData.LogExperience[HistoricalData.LogExperience.Count - 1];
                        chart.Series[0].Points.AddXY(latest.Date, latest.Value);
                    }
                    else if (info.Mode == 2)
                    {
                        SenkaRecord latest = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
                        int[] display_rank = SenkaRecord.DisplayRank;
                        //ボーダー
                        for (int i = 0; i < display_rank.Length; i++)
                        {
                            int senkaval = latest.TopSenka[display_rank[i] - 1];
                            if (senkaval >= 0) chart.Series[i].Points.AddXY(latest.StartTime, senkaval);
                        }
                        //自分の戦果
                        int mysenka = latest.StartSenka;
                        if (mysenka >= 0) chart.Series[display_rank.Length].Points.AddXY(latest.StartTime, mysenka);
                    }
                }
            }
        }

        //経験値のグラフの初期化
        public static void ExperienceGraphInit(System.Windows.Forms.DataVisualization.Charting.Chart chart, GraphInfo info, bool infooverwrite)
        {
            chart.Series.Clear();
            string[] ser_label = null;
            int[] display_rank = SenkaRecord.DisplayRank;
            int n = display_rank.Length;
            //系列の初期化
            if (info.Mode == 1)
            {
                ser_label = new string[] { "提督経験値" };
                chart.Series.Add(ser_label[0]);
                chart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                //x軸のスタイル
                chart.ChartAreas[0].AxisX.LabelStyle.Format = "\\\'d H:mm";
                chart.Series[0].ToolTip = "#SERIESNAME \nX=#VALX{M/d H:mm} \nY=#VALY";
            }
            else if (info.Mode == 2)
            {
                ser_label = new string[display_rank.Length + 1];
                //ボーダー
                for (int i = 0; i < n; i++)
                {
                    ser_label[i] = display_rank[i] + "位";
                    chart.Series.Add(ser_label[i]);
                    chart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart.Series[i].BorderWidth = 2;
                    chart.Series[i].ToolTip = "#SERIESNAME \nX=#VALX{M/d H:mm} \nY=#VALY";
                }
                //自分の戦果
                ser_label[n] = "自分";
                chart.Series.Add(ser_label[n]);
                chart.Series[n].Color = System.Drawing.Color.FromArgb(250, 104, 0);
                chart.Series[n].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                chart.Series[n].BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent50;
                chart.Series[n].ToolTip = "#SERIESNAME \nX=#VALX{M/d H:mm} \nY=#VALY";
                //x軸のスタイル
                chart.ChartAreas[0].AxisX.LabelStyle.Format = "M/d";
            }
            //データ
            DateTime mindate = info.Term.GetMinDate();
            //提督経験値の場合
            if (info.Mode == 1)
            {
                var data = from p in HistoricalData.LogExperience
                           where p.Date >= mindate
                           select p;
                if (data.Count() == 0) return;
                //データのプロット
                foreach (var x in data)
                {
                    chart.Series[0].Points.AddXY(x.Date, x.Value);
                }
            }
            else if (info.Mode == 2)
            {
                var data = from p in HistoricalData.LogSenka
                           where p.StartTime >= mindate
                           select p;
                if (data.Count() == 0) return;
                //プロット
                foreach (var x in data)
                {
                    //ボーダー部分
                    for (int i = 0; i < display_rank.Length; i++)
                    {
                        int senkaval = x.TopSenka[display_rank[i] - 1];
                        if (senkaval >= 0) chart.Series[i].Points.AddXY(x.StartTime, senkaval);
                    }
                    //自分の戦果
                    int mysenka = x.StartSenka;
                    if (mysenka >= 0) chart.Series[n].Points.AddXY(x.StartTime, mysenka);
                }
            }
            //Infoの更新
            if (infooverwrite) HistoricalData.GraphInfoExperience = info;
        }

        //戦果タブの別窓ボタン
        public static void button_senka_openchart_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem item = sender as System.Windows.Forms.ToolStripMenuItem;
            //親タブの取得
            UserControls.TabSenka ucntr = item.Tag as UserControls.TabSenka;
            DockingWindows.DockWindowTabPage tabpage = ucntr.FindForm() as DockingWindows.DockWindowTabPage;
            //親フォームの取得
            Form1 parentForm = tabpage.MainScreen;
            //別窓表示
            KancolleInfo.ShowChartViewer(1, parentForm);
        }

        //戦果タブの右クリックメニュー
        public static void contextMenuStrip_senka_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            //親コレクションの取得
            System.Windows.Forms.ContextMenuStrip item = sender as System.Windows.Forms.ContextMenuStrip;
            //親フォームの取得
            UserControls.TabSenka ucntr = item.Tag as UserControls.TabSenka;
            Form1 parentForm = (ucntr.FindForm() as DockingWindows.DockWindowTabPage).MainScreen;
            if (e.ClickedItem == ucntr.toolStripMenuItem_senka_predict)
            {
                parentForm.ShowSenkaPredict();
            }
        }

    }
}
