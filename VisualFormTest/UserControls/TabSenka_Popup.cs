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
    public partial class TabSenka_Popup : Form
    {
        public TabSenka_Popup()
        {
            InitializeComponent();
        }

        private void TabSenka_Popup_Load(object sender, EventArgs e)
        {
            if (HistoricalData.LogSenka == null || HistoricalData.LogSenka.Count == 0) return;

            //今日のデータ
            int todayStartIndex = HistoricalData.LogSenka.Count - 1;
            for (int i = HistoricalData.LogSenka.Count - 2; i >= 0; i-- )
            {
                //日が同じなら
                if(HistoricalData.LogSenka[todayStartIndex].StartTime.AddHours(-2).Day == HistoricalData.LogSenka[i].StartTime.AddHours(-2).Day)
                {
                    todayStartIndex = i;
                }
                else
                {
                    break;
                }
            }
            //今週のデータ
            int thisWeekStartIndex = HistoricalData.LogSenka.Count - 1;
            for(int i=HistoricalData.LogSenka.Count - 2; i>=0; i--)
            {
                //週替りしたタイミングを探す
                DateTime nowOffsetTime = HistoricalData.LogSenka[thisWeekStartIndex].StartTime.AddHours(-2);//2時間マイナスして0時の時点をスタートにする
                DateTime prevOffsetTime = HistoricalData.LogSenka[i].StartTime.AddHours(-2);//nowの1個前を探す
                //曜日インデックス（月曜が0になるように+6mod7）
                int nowDayOfWeek = ((int)nowOffsetTime.DayOfWeek + 6) % 7;
                int prevDayOfWeek = ((int)prevOffsetTime.DayOfWeek + 6) % 7;
                //prev < nowなら週替りしていない、prev > nowなら週替りしてる
                if(prevDayOfWeek <= nowDayOfWeek)
                {
                    thisWeekStartIndex = i;
                }
                else
                {
                    break;
                }
            }
            //今月のデータ
            int thisMonthStartIndex = 0;

            //表示に反映
            var halfday = CalcExps(HistoricalData.LogSenka.Count - 1);
            var day = CalcExps(todayStartIndex);
            var week = CalcExps(thisWeekStartIndex);
            var month = CalcExps(thisMonthStartIndex);

            label_exp1.Text = halfday.DisplayExp();
            label_exp2.Text = day.DisplayExp();
            label_exp3.Text = week.DisplayExp();
            label_exp4.Text = month.DisplayExp();

            label_senka1.Text = halfday.DisplaySenka();
            label_senka2.Text = day.DisplaySenka();
            label_senka3.Text = week.DisplaySenka();
            label_senka4.Text = month.DisplaySenka();
        }

        private Result CalcExps(int startIndex)
        {
            Result result = new Result();

            //経験値
            result.Exp = APIPort.Basic.api_experience - HistoricalData.LogSenka[startIndex].StartExp;
            //EO戦果
            for(int i=startIndex; i<HistoricalData.LogSenka.Count; i++)
            {
                result.EOSenka += HistoricalData.LogSenka[i].SpecialSenka;
            }
            //戦果（引き継ぎ加味せず）
            result.Senka = (double)result.Exp / 10000.0 * 7.0 + (double)result.EOSenka;

            return result;
        }


        public class Result
        {
            public int Exp { get; set; }
            public double Senka { get; set; }
            public int EOSenka { get; set; } 

            public string DisplayExp()
            {
                return string.Format("{0} exp", this.Exp.ToString("N0"));
            }

            public string DisplaySenka()
            {
                return string.Format("{0} (EO:{1})", this.Senka.ToString("N1"), this.EOSenka.ToString());
            }
        }
    }
}
