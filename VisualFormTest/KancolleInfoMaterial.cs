using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class KancolleInfoMaterial
    {
        //資源表示
        public static void SetMaterial(System.Windows.Forms.Label[] labels, System.Windows.Forms.Label[] label_diff)
        {
            //現在の資源量
            int[] now = (from m in APIPort.Materials
                         select m.api_value).ToArray();
            //[0]-[3]資源　[4]高速建造　[5]バケツ [6]開発資源 [7]改修資材
            for (int i = 0; i < labels.Length; i++)
            {
                CallBacks.SetLabelTextAndColor(labels[i], now[i].ToString("N0"), KancolleInfo.DefaultStringColor);
            }
            //資材の差分
            if (HistoricalData.LogMaterial.Count == 0)
            {
                foreach (var x in label_diff) CallBacks.SetLabelTextAndColor(x, "-", KancolleInfo.DefaultStringColor);
            }
            else
            {
                Dictionary<string, int> last = HistoricalData.LogMaterial[HistoricalData.LogMaterial.Count - 1].Value;
                int cnt = 0;
                foreach (var x in label_diff)
                {
                    //差分
                    int diff = now[cnt] - last[MaterialRecord.Keys[cnt]];
                    if (diff == 0)
                    {
                        CallBacks.SetLabelTextAndColor(x, "-", KancolleInfo.DefaultStringColor);
                    }
                    else if (diff > 0)
                    {
                        CallBacks.SetLabelTextAndColor(x, "+" + diff, KancolleInfo.MaterialPlusStringColor);
                    }
                    else
                    {
                        CallBacks.SetLabelTextAndColor(x, diff.ToString(), KancolleInfo.MaterialMinusStringColor);
                    }
                    cnt++;
                }
            }
        }

        //資源のグラフ表示
        public static void DrawMaterialGraph(System.Windows.Forms.DataVisualization.Charting.Chart chart, GraphInfo info)
        {
            if (chart.InvokeRequired)
            {
                CallBacks.SetChartCallBack d = new CallBacks.SetChartCallBack(DrawMaterialGraph);
                chart.Invoke(d, new object[] { chart, info });
            }
            else
            {
                //初期化が必要な場合
                if (info != HistoricalData.GraphInfoMaterial) MaterialGraphInit(chart, info, true);
                //最新のデータを追加するだけの場合
                else
                {
                    //古い値の消去
                    if (HistoricalData.GraphInfoMaterial.Term != GraphInfoTerm.All)
                    {
                        DateTime datemin = HistoricalData.GraphInfoMaterial.Term.GetMinDate();
                        //消去する必要がある場合
                        bool isdelete = DateTime.FromOADate(chart.Series[0].Points[0].XValue) < datemin;
                        if (isdelete)
                        {
                            //消去する必要ありかつ差分表示の場合初期化
                            if (info.IsDiff)
                            {
                                MaterialGraphInit(chart, info, true);
                            }
                            //その他の消去する必要がある場合
                            for (int i = 0; i < Math.Min(5, chart.Series[0].Points.Count); i++)
                            {
                                DateTime date = DateTime.FromOADate(chart.Series[0].Points[i].XValue);
                                if (date >= datemin) break;//消す必要がなくなったら離脱
                                foreach (var x in chart.Series) x.Points.RemoveAt(i);
                            }
                        }
                    }
                    //最新の値の追加
                    int offset = info.Mode == 1 ? 0 : 4;
                    MaterialRecord latest = HistoricalData.LogMaterial[HistoricalData.LogMaterial.Count - 1];
                    MaterialRecord first = HistoricalData.LogMaterial[0];
                    int cnt = 0;
                    foreach (var x in chart.Series)
                    {
                        int val = latest.Value[MaterialRecord.Keys[cnt + offset]]
                            - Convert.ToInt32(info.IsDiff) * first.Value[MaterialRecord.Keys[cnt + offset]];
                        x.Points.AddXY(latest.Date, val);
                        cnt++;
                    }
                }
            }
        }

        //資源グラフの初期化
        public static void MaterialGraphInit(System.Windows.Forms.DataVisualization.Charting.Chart chart, GraphInfo info, bool infooverwrite)
        {
            chart.Series.Clear();
            //系列の追加
            string[] series_str = new string[]
            {
                "燃料", "弾薬", "鋼材", "ボーキサイト", "高速建造材", "高速修復材", "開発資材", "改修資材"
            };
            if (info.Mode == 1)
            {
                for (int i = 0; i < 4; i++) chart.Series.Add(series_str[i]);
            }
            else if (info.Mode == 2)
            {
                for (int i = 4; i < series_str.Length; i++) chart.Series.Add(series_str[i]);
            }
            //グラフの種類
            foreach (var x in chart.Series)
            {
                x.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                x.ToolTip = "#SERIESNAME \nX=#VALX{M/d H:mm} \nY=#VALY";
            }
            //期間の調整
            DateTime mindate = info.Term.GetMinDate();
            var data = from p in HistoricalData.LogMaterial
                       where p.Date >= mindate
                       select p;
            if (data.Count() == 0) return;
            //最初のレコード
            MaterialRecord first_record = data.First();
            //データの追加
            foreach (var d in data)
            {
                //カウンター
                int i_start = info.Mode == 1 ? 0 : 4;
                int i_end = info.Mode == 1 ? 4 : series_str.Length;
                //データ
                for (int i = i_start; i < i_end; i++)
                {
                    int val = d.Value[MaterialRecord.Keys[i]]
                        - Convert.ToInt32(info.IsDiff) * first_record.Value[MaterialRecord.Keys[i]];
                    chart.Series[series_str[i]].Points.AddXY(d.Date, val);
                }
            }
            //Infoの更新
            if (infooverwrite) HistoricalData.GraphInfoMaterial = info;
        }
    }
}
