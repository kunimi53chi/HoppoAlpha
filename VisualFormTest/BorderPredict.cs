using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.Const;

namespace VisualFormTest
{
    public partial class BorderPredict : Form
    {
        //計算結果
        List<BorderPredictItem> borders;

        public BorderPredict()
        {
            //レコードの準備が整っていなかったら
            if (!HistoricalData.IsInited) return;

            InitializeComponent();

            //取得するボーダー
            int[] getrank = new int[]
            {
                1, 2, 3, 5, 20, 100, 500,
            };
            //ボーダーのアイテム
            borders = new List<BorderPredictItem>();
            foreach(int x in getrank)
            {
                borders.Add(new BorderPredictItem(x));
            }
            //ラベルのコレクション
            Label[][] labels = new Label[][]
            {
                new Label[]{label_start_senka1, label_start_time1, label_end_senka1, label_end_time1, label_mean_am1, label_mean_pm1, label_sd_am1, label_sd_pm1, label_pred_2minus1, label_pred_1minus1, label_pred_zero1, label_pred_1plus1, label_pred_2plus1, label_remain_am1, label_remain_pm1,},
                new Label[]{label_start_senka2, label_start_time2, label_end_senka2, label_end_time2, label_mean_am2, label_mean_pm2, label_sd_am2, label_sd_pm2, label_pred_2minus2, label_pred_1minus2, label_pred_zero2, label_pred_1plus2, label_pred_2plus2, label_remain_am2, label_remain_pm2,},
                new Label[]{label_start_senka3, label_start_time3, label_end_senka3, label_end_time3, label_mean_am3, label_mean_pm3, label_sd_am3, label_sd_pm3, label_pred_2minus3, label_pred_1minus3, label_pred_zero3, label_pred_1plus3, label_pred_2plus3, label_remain_am3, label_remain_pm3,},
                new Label[]{label_start_senka4, label_start_time4, label_end_senka4, label_end_time4, label_mean_am4, label_mean_pm4, label_sd_am4, label_sd_pm4, label_pred_2minus4, label_pred_1minus4, label_pred_zero4, label_pred_1plus4, label_pred_2plus4, label_remain_am4, label_remain_pm4,},
                new Label[]{label_start_senka5, label_start_time5, label_end_senka5, label_end_time5, label_mean_am5, label_mean_pm5, label_sd_am5, label_sd_pm5, label_pred_2minus5, label_pred_1minus5, label_pred_zero5, label_pred_1plus5, label_pred_2plus5, label_remain_am5, label_remain_pm5,},
                new Label[]{label_start_senka6, label_start_time6, label_end_senka6, label_end_time6, label_mean_am6, label_mean_pm6, label_sd_am6, label_sd_pm6, label_pred_2minus6, label_pred_1minus6, label_pred_zero6, label_pred_1plus6, label_pred_2plus6, label_remain_am6, label_remain_pm6,},
                new Label[]{label_start_senka7, label_start_time7, label_end_senka7, label_end_time7, label_mean_am7, label_mean_pm7, label_sd_am7, label_sd_pm7, label_pred_2minus7, label_pred_1minus7, label_pred_zero7, label_pred_1plus7, label_pred_2plus7, label_remain_am7, label_remain_pm7,},
            };
            //セット
            for(int i=0; i<borders.Count; i++)
            {
                BorderPredictItem b = borders[i];
                //月初
                labels[i][0].Text = b.FirstValue >= 0 ? b.FirstValue.ToString() : "-";
                labels[i][1].Text = BorderPredictItem.DayToString(b.FirstDate, b.FirstSection);
                //現在
                labels[i][2].Text = b.LastValue >= 0 ? b.LastValue.ToString() : "-";
                labels[i][3].Text = BorderPredictItem.DayToString(b.LastDate, b.LastSection);
                //平均
                labels[i][4].Text = b.MeanMorning.ToString("F1");
                labels[i][5].Text = b.MeanAfternoon.ToString("F1");
                //標準偏差
                labels[i][6].Text = b.SdMorning.ToString("F2");
                labels[i][7].Text = b.SdAfternoon.ToString("F2");
                //予測
                labels[i][8].Text = b.ForcastMinus2 >= 0 ? b.ForcastMinus2.ToString("F0") : "-";
                labels[i][9].Text = b.ForcastMinus1 >= 0 ? b.ForcastMinus1.ToString("F0") : "-";
                labels[i][10].Text = b.ForcastMean >= 0 ? b.ForcastMean.ToString("F0") : "-";
                labels[i][11].Text = b.ForcastPlus1 >= 0 ? b.ForcastPlus1.ToString("F0") : "-";
                labels[i][12].Text = b.ForcastPlus2 >= 0 ? b.ForcastPlus2.ToString("F0") : "-";
                //残りセクション
                labels[i][13].Text = b.RemainSectionMorning.ToString();
                labels[i][14].Text = b.RemainSectionAfternoon.ToString();
            }
            //最終取得日時
            label101.Text = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1].EndTime.ToString();
            //ToolTip
            ToolTip tips = new ToolTip();
            tips.SetToolTip(label1, "その月で初めて観測された有効な戦果データの値と日時");
            tips.SetToolTip(label2, "現在取得された当月最新の有効な戦果データの値と日時");
            tips.SetToolTip(label3, "セクション間の上昇値の平均");
            tips.SetToolTip(label4, "月末戦果の予測：これ以降の戦果の上昇値を平均±標準偏差（σ）で場合分けしています");
            tips.SetToolTip(label5, "残セクション数：残り日数×2で計算されます");
        }

        private string MakeCsvString()
        {
            StringBuilder sb = new StringBuilder();
            //ヘッダー
            sb.AppendLine(BorderPredictItem.MakeCsvHeader());
            //行
            foreach(var x in borders)
            {
                sb.AppendLine(x.ConvertToCsvString());
            }
            //フッター
            sb.AppendLine();
            sb.AppendLine(BorderPredictItem.MakeCsvFooter(HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1].EndTime));

            return sb.ToString();
        }

        private void button_copytoclipboard_Click(object sender, EventArgs e)
        {
            string csv = MakeCsvString();
            Clipboard.SetText(csv);
        }

        private void button_savecsv_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Config.SenkaPredictOutputDirectory)) saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            else saveFileDialog1.InitialDirectory = Config.SenkaPredictOutputDirectory;

            var lastSenka = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
            saveFileDialog1.FileName = string.Format("senkapredict_{0}_{1}.csv", lastSenka.EndTime.ToString("yyMMdd"), lastSenka.Section);

            saveFileDialog1.Filter = "csvファイル(*.csv)|*.csv";
            saveFileDialog1.Title = "保存するファイルを選択してください";
            saveFileDialog1.RestoreDirectory = true;

            if(saveFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using(var fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using(var sw = new StreamWriter(fs, Encoding.GetEncoding("shift-jis")))
                        {
                            sw.WriteLine(MakeCsvString());
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("保存に失敗しました" + Environment.NewLine + ex.ToString(), "保存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    //戦果予想用のクラス
    public class BorderPredictItem
    {
        //月初の値
        public int FirstValue { get; set; }
        //月初の日付け
        public DateTime FirstDate { get; set; }
        //月初のセクション
        public int FirstSection { get; set; }
        //順位
        public int Rank { get; set; }
        //現在の値
        public int LastValue { get; set; }
        //現在の日付け
        public DateTime LastDate { get; set; }
        //現在のセクション
        public int LastSection { get; set; }
        //上昇平均・午前
        public double MeanMorning { get; set; }
        //上昇平均・午後
        public double MeanAfternoon { get; set; }
        //上昇標準偏差・午前
        public double SdMorning { get; set; }
        //上昇標準偏差・午後
        public double SdAfternoon { get; set; }
        //残りセクション・午前
        public int RemainSectionMorning { get; set; }
        //残りセクション・午後
        public int RemainSectionAfternoon { get; set; }
        //予測値
        public double ForcastMinus2 { get; set; }
        public double ForcastMinus1 { get; set; }
        public double ForcastMean { get; set; }
        public double ForcastPlus1 { get; set; }
        public double ForcastPlus2 { get; set; }

        public BorderPredictItem(int rank)
        {
            this.Rank = rank;
            int n = rank - 1;
            //頭からレコードを検索
            this.FirstValue = -1;
            for(int i=0; i<HistoricalData.LogSenka.Count; i++)
            {
                SenkaRecord r = HistoricalData.LogSenka[i];
                //見つかった場合
                if(r.TopSenka[n] != -1)
                {
                    this.FirstValue = r.TopSenka[n];
                    this.FirstDate = r.StartTime;
                    this.FirstSection = r.Section;
                    break;
                }
            }
            //お尻からレコードを検索
            this.LastValue = -1;
            for(int i=HistoricalData.LogSenka.Count-1; i>=0; i--)
            {
                SenkaRecord r = HistoricalData.LogSenka[i];
                //見つかった場合
                if (r.TopSenka[n] != -1)
                {
                    this.LastValue = r.TopSenka[n];
                    this.LastDate = r.StartTime;
                    this.LastSection = r.Section;
                    break;
                }
            }
            //有効なデータがあるノードに限定, sec1,2に限定
            var dataValid = HistoricalData.LogSenka.Where(x => x.TopSenka[n] != -1)
                .Where(x => x.Section == 1 || x.Section == 2).ToList();
            //レコードを午前と午後に分割
            List<ValuePairs<int>> morningValidDiff = new List<ValuePairs<int>>(), afternoonValidDiff = new List<ValuePairs<int>>();
            for (int i = 0; i < dataValid.Count - 1; i++ )
            {
                //このノードと次のノード
                var node = dataValid[i];
                var nodenext = dataValid[i + 1];
                //期間差分
                int termdiff = nodenext.StartTime.AddHours(-2).Day * 2 + nodenext.Section
                            - (node.StartTime.AddHours(-2).Day * 2 + node.Section);
                //アイテム
                var item = new ValuePairs<int>()
                {
                    ValueX = termdiff,
                    ValueY = nodenext.TopSenka[n] - node.TopSenka[n],
                };
                //今：1→次1or2 午前とみなす
                if (node.Section == 1) morningValidDiff.Add(item);
                //今：2→次1or2　午後とみなす
                else if (node.Section == 2) afternoonValidDiff.Add(item);
            }
            //残りセクション数
            DateTime zeroDate = new DateTime();
            if(this.FirstDate != zeroDate && this.LastDate != zeroDate)
            {
                int diffDay = EndOfMonth(DateTime.Today.AddHours(-2)).DayOfYear - StripTime(this.LastDate.AddHours(-2)).DayOfYear;
                bool isLastSectionMorning = this.LastSection == 1;
                this.RemainSectionMorning = diffDay + Convert.ToInt32(isLastSectionMorning);
                this.RemainSectionAfternoon = diffDay + 1;
            }
            //平均標準偏差を計算
            var resultMorning = CalcMeanSd(morningValidDiff);
            var resultAfternoon = CalcMeanSd(afternoonValidDiff);
            this.MeanMorning = resultMorning.ValueX; this.SdMorning = resultMorning.ValueY;
            this.MeanAfternoon = resultAfternoon.ValueX; this.SdAfternoon = resultAfternoon.ValueY;

            //予測増分の計算
            double[] forcDiffMorning = new double[5], forcDIffAfternoon = new double[5];
            foreach(int i in Enumerable.Range(0, forcDiffMorning.Length))
            {
                int k = i - 2;//-2,-1,0,1,2
                forcDiffMorning[i] = Math.Max(0.0, this.MeanMorning + k * this.SdMorning);
                forcDIffAfternoon[i] = Math.Max(0.0, this.MeanAfternoon + k * this.SdAfternoon);
            }

            //予測戦果
            if(this.LastValue == -1)
            {
                this.ForcastMinus2 = this.ForcastMinus1 = this.ForcastMean = this.ForcastPlus1 = this.ForcastPlus2 = -1;
            }
            else
            {
                this.ForcastMinus2 = this.LastValue + forcDiffMorning[0] * (double)RemainSectionMorning + forcDIffAfternoon[0] * (double)RemainSectionAfternoon;
                this.ForcastMinus1 = this.LastValue + forcDiffMorning[1] * (double)RemainSectionMorning + forcDIffAfternoon[1] * (double)RemainSectionAfternoon;
                this.ForcastMean = this.LastValue + forcDiffMorning[2] * (double)RemainSectionMorning + forcDIffAfternoon[2] * (double)RemainSectionAfternoon;
                this.ForcastPlus1 = this.LastValue + forcDiffMorning[3] * (double)RemainSectionMorning + forcDIffAfternoon[3] * (double)RemainSectionAfternoon;
                this.ForcastPlus2 = this.LastValue + forcDiffMorning[4] * (double)RemainSectionMorning + forcDIffAfternoon[4] * (double)RemainSectionAfternoon;
            }
        }

        //2つの値をもつペア（Dictionaryのインデクサーが使えないため）
        public class ValuePairs<T> where T : struct
        {
            public T ValueX { get; set; }
            public T ValueY { get; set; }
        }

        /// <summary>
        /// 時系列コレクションの平均、標準偏差を計算
        /// </summary>
        /// <param name="datas">データ列（日、値のペア）</param>
        /// <returns>平均、標準偏差のペア</returns>
        public static ValuePairs<double> CalcMeanSd(List<ValuePairs<int>> datas)
        {
            //期間を標準化する
            List<double> diff = new List<double>();
            foreach(var d in datas)
            {
                double standard_diff = (double)d.ValueY / (double)d.ValueX;
                diff.Add(standard_diff);
            }
            
            double mean = 0.0, sd = 0.0;
            if(diff.Count != 0)
            {
                //平均
                mean = diff.Sum() / (double)diff.Count;
                //分散
                double variance = 0;
                foreach (var x in diff) variance += Math.Pow(x - mean, 2.0);
                //標準偏差
                sd = Math.Sqrt(variance / diff.Count);
            }

            return new ValuePairs<double>() { ValueX = mean, ValueY = sd };
        }

        //CSVに変換用
        public string ConvertToCsvString()
        {
            var row = new CsvList<string>();
            //順位
            row.Add(this.Rank.ToString() + "位");
            //月初
            row.Add(this.FirstValue >= 0 ? this.FirstValue.ToString() : "-");
            row.Add(BorderPredictItem.DayToString(this.FirstDate, this.FirstSection));
            //現在
            row.Add(this.LastValue >= 0 ? this.LastValue.ToString() : "-");
            row.Add(BorderPredictItem.DayToString(this.LastDate, this.LastSection));
            //平均
            row.Add(this.MeanMorning.ToString());
            row.Add(this.MeanAfternoon.ToString());
            //標準偏差
            row.Add(this.SdMorning.ToString());
            row.Add(this.SdAfternoon.ToString());
            //予測
            row.Add(this.ForcastMinus2 >= 0 ? this.ForcastMinus2.ToString("F1") : "-");
            row.Add(this.ForcastMinus1 >= 0 ? this.ForcastMinus1.ToString("F1") : "-");
            row.Add(this.ForcastMean >= 0 ? this.ForcastMean.ToString("F1") : "-");
            row.Add(this.ForcastPlus1 >= 0 ? this.ForcastPlus1.ToString("F1") : "-");
            row.Add(this.ForcastPlus2 >= 0 ? this.ForcastPlus2.ToString("F1") : "-");
            //残りセクション
            row.Add(this.RemainSectionMorning.ToString());
            row.Add(this.RemainSectionAfternoon.ToString());

            return string.Join(",", row);
        }

        public static string MakeCsvHeader()
        {
            var rows = new List<string>();
            //1行目
            var row1 = new CsvList<string>();
            row1.Add("-");
            row1.Add("月初"); row1.Add("");
            row1.Add("現在"); row1.Add("");
            row1.Add("上昇平均"); row1.Add("");
            row1.Add("上昇標準偏差"); row1.Add("");
            row1.Add("予測"); for (int i = 0; i < 4; i++) row1.Add("");
            row1.Add("残りセクション"); row1.Add("");
            rows.Add(string.Join(",", row1));
            //2行目
            var row2 = new CsvList<string>();
            row2.Add("戦果"); row2.Add("日時");
            row2.Add("戦果"); row2.Add("日時");
            row2.Add("午前"); row2.Add("午後");
            row2.Add("午前"); row2.Add("午後");
            row2.Add("-2σ"); row2.Add("-1σ"); row2.Add("±0σ"); row2.Add("+1σ"); row2.Add("+2σ");
            row2.Add("午前"); row2.Add("午後");
            rows.Add(string.Join(",", row2));

            return string.Join(Environment.NewLine, rows);
        }

        public static string MakeCsvFooter(DateTime lastUpdatedTime)
        {
            return string.Format("\"最終更新日時 {0}\"", lastUpdatedTime.ToString());
        }


        /// <summary>
        /// 該当年月の日数を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        public static int DaysInMonth(DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month);
        }

        /// <summary>
        /// 月末日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime EndOfMonth(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DaysInMonth(dt));
        }

        /// <summary>
        /// 時刻を落として日付のみにする
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime StripTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        //日付けを文字列に変換
        public static string DayToString(DateTime d, int section)
        {
            string sec = "";
            switch(section)
            {
                case 1: sec = "AM"; break;
                case 2: sec = "PM"; break;
                case 3: sec = "etc"; break;
            }
            //日付けがない場合
            if (d == new DateTime()) return "-";
            else return string.Format("{0}{1}", d.ToString("M/d"), sec);
        }
    }
}
