using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 戦果と簡易ランキングの記録
    /// </summary>
    [ProtoContract]
    public class SenkaRecord
    {
        /// <summary>
        /// セクション(午前=1, 午後=2, 月末22時～2時=3)を表します
        /// </summary>
        [ProtoMember(1)]
        public int Section { get; set; }
        /// <summary>
        /// 当日の午前の戦果を表します(参照形式)
        /// </summary>
        [ProtoMember(2, AsReference = true)]
        public SenkaRecord Morning { get; set; }
        /// <summary>
        /// 当日の午後の戦果を表します(参照形式)
        /// </summary>
        [ProtoMember(3, AsReference = true)]
        public SenkaRecord Afternoon { get; set; }
        /// <summary>
        /// 月末～月初午前までの戦果を表します(参照形式)
        /// </summary>
        [ProtoMember(4, AsReference = true)]
        public SenkaRecord Additional { get; set; }
        /// <summary>
        /// セクション開始日時を表します
        /// </summary>
        [ProtoMember(5)]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// セクション終了日時を表します
        /// </summary>
        [ProtoMember(6)]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// セクション開始時の提督経験値を表します
        /// </summary>
        [ProtoMember(7)]
        public int StartExp { get; set; }
        /// <summary>
        /// セクション終了時の提督経験値を表します
        /// </summary>
        [ProtoMember(8)]
        public int EndExp { get; set; }
        /// <summary>
        /// セクション開始時の戦果を表します
        /// </summary>
        [ProtoMember(9)]
        public int StartSenka { get; set; }
        /// <summary>
        /// セクション終了時の推定戦果を表します
        /// </summary>
        [ProtoMember(10)]
        public double EndSenkaEst { get; set; }
        /// <summary>
        /// セクション間の特別戦果の加算値を表します
        /// </summary>
        [ProtoMember(11)]
        public int SpecialSenka { get; set; }
        /// <summary>
        /// セクション開始時の順位を表します
        /// </summary>
        [ProtoMember(12)]
        public int Rank { get; set; }
        /// <summary>
        /// セクション開始時の階級を表します
        /// </summary>
        [ProtoMember(13)]
        public int Title { get; set; }
        /// <summary>
        /// ランキングデータからトップランカーの戦果値を表します
        /// </summary>
        [ProtoMember(14)]
        public int[] TopSenka { get; set; }
        /// <summary>
        /// ランキングデータからトップランカーのIDを表します
        /// </summary>
        [ProtoMember(15)]
        public int[] TopID { get; set; }
        /// <summary>
        /// ランキングデータからトップランカーの提督経験値を表します
        /// </summary>
        [ProtoMember(16)]
        public int[] TopExp { get; set; }
        /// <summary>
        /// ランキングデータからトップランカーの名前を表します
        /// </summary>
        [ProtoMember(17)]
        public string[] TopName { get; set; }
        /// <summary>
        /// 直前の連続するセクションを表します(参照形式)
        /// </summary>
        [ProtoMember(18, AsReference = true)]
        public SenkaRecord PrevContinuousSection { get; set; }

        /// <summary>
        /// トップランカーのデータ取得数を設定します(デフォルト:1000)
        /// </summary>
        public static int MaxArraySize = 1000;
        /// <summary>
        /// ボーダーで表示する順位を設定します
        /// </summary>
        public static int[] DisplayRank { get; set; }

        static SenkaRecord()
        {
            DisplayRank = new int[] { 1, 2, 3, 5, 20, 100, 500 };
        }

        public SenkaRecord()
        {
        }

        /// <param name="flag">初期値を与えるかどうかのフラグ</param>
        public SenkaRecord(bool flag)
        {
            if (flag)
            {
                this.StartExp = -1; this.EndExp = -1;
                this.StartSenka = -1; this.EndSenkaEst = -1;
                this.Rank = -1; this.Title = -1;
                this.TopExp = new int[MaxArraySize]; this.TopID = new int[MaxArraySize];
                this.TopName = new string[MaxArraySize]; this.TopSenka = new int[MaxArraySize];
                for (int i = 0; i < MaxArraySize; i++)
                {
                    this.TopExp[i] = -1;
                    this.TopID[i] = -1;
                    this.TopSenka[i] = -1;
                    this.TopName[i] = "";
                }
            }
        }

        /// <param name="date">セクション開始日時</param>
        /// <param name="startExp">セクション開始時の提督経験値</param>
        /// <param name="lastRecord">直前のセクション。存在しない場合はnull</param>
        public SenkaRecord(DateTime date, int startExp, SenkaRecord lastRecord)
            : this(true)
        {
            this.Section = SenkaRecord.GetSection(date);//セクション
            this.StartTime = date;
            //初期経験値
            this.StartExp = startExp;
            //初期化処理
            SenkaRecord last = lastRecord;
            //セクションの参照部分
            switch (this.Section)
            {
                case 1://午前の場合
                    this.Morning = this;
                    this.Afternoon = null;
                    if (last != null && last.Section == 3)
                    {
                        DateTime lastday = last.StartTime.AddHours(2);
                        if (lastday.Day == this.StartTime.Day) this.Additional = last;
                        else this.Additional = null;
                    }
                    else this.Additional = null;
                    break;
                case 2://午後の場合
                    if (last != null && last.Section == 1)
                    {
                        DateTime lastday = last.StartTime.AddHours(2);
                        if (lastday.Day == this.StartTime.Day) this.Morning = last;
                        else this.Morning = null;
                    }
                    else this.Morning = null;
                    this.Afternoon = this;
                    this.Additional = null;
                    break;
                case 3://Additionalの場合
                    this.Morning = null; this.Afternoon = null;
                    this.Additional = this;
                    break;
            }
            //直前のセクションが連続しているかどうかのチェック
            if (last == null)
            {
                this.PrevContinuousSection = null;
            }
            else
            {
                //午前→午後
                if (last.Section == 1 && this.Section == 2)
                {
                    if (this.StartTime - last.StartTime <= new TimeSpan(24, 0, 0)) this.PrevContinuousSection = last;
                    else this.PrevContinuousSection = null;
                }
                //午後→午前
                else if (last.Section == 2 && this.Section == 1)
                {
                    if (this.StartTime - last.StartTime <= new TimeSpan(24, 0, 0)) this.PrevContinuousSection = last;
                    else this.PrevContinuousSection = null;
                }
                //午後→ロスタイム
                else if (last.Section == 2 && this.Section == 3)
                {
                    if (this.StartTime - last.StartTime <= new TimeSpan(12, 0, 0)) this.PrevContinuousSection = last;
                    else this.PrevContinuousSection = null;
                }
                //ロスタイム→午前
                else if (last.Section == 3 && this.Section == 1)
                {
                    if (this.StartTime - last.StartTime <= new TimeSpan(12, 0, 0)) this.PrevContinuousSection = last;
                    else this.PrevContinuousSection = null;
                }
                else
                {
                    this.PrevContinuousSection = null;
                }
            }

        }

        #region セクション関連
        /// <summary>
        /// セクションの変更が必要かどうかを取得します
        /// </summary>
        public bool SectionSwitchRequired
        {
            get
            {
                return (!this.isThisSection(DateTime.Now));
            }
        }

        /// <summary>
        /// 指定した日時がこのセクションに属するかどうかを調べます
        /// </summary>
        /// <param name="date">チェックする日時</param>
        public bool isThisSection(DateTime date)
        {
            //日付のチェック
            DateTime date_m = date.AddHours(-2);
            DateTime start_m = this.StartTime.AddHours(-2);
            if (date_m.Day != start_m.Day) return false;
            //セクションのチェック
            int sec = SenkaRecord.GetSection(date);
            return (this.Section == sec);
        }

        /// <summary>
        /// 指定した日時がどのセクションに属するのかを調べます
        /// </summary>
        /// <param name="date">チェックする日時</param>
        /// <returns>1=午前, 2=午後, 3=月末～月始</returns>
        public static int GetSection(DateTime date)
        {
            DateTime secplus = date + new TimeSpan(2, 0, 0);//2時間進ませて判定
            //3の場合(月末か月頭の場合)
            if (secplus.Day == 1)
            {
                if (secplus.Hour / 4 == 0) return 3;//2時間進んだ状態で0時から4時まで
            }
            //1の場合
            DateTime ofs = date - new TimeSpan(2, 0, 0);//2時間マイナスして午前なら1、午後なら2
            if (ofs.Hour / 12 == 0) return 1;
            else return 2;
        }

        /// <summary>
        /// 指定したセクションの現在時刻に対する残り時間を取得します
        /// </summary>
        /// <param name="section">セクション(1=午前, 2=午後, 3=月末～月始)</param>
        /// <returns>セクション開始前は「待ち○○」、セクション中は「あと○○」、セクション終了後は「確定」※○○はh:mmとなります</returns>
        public static string GetTodaysLimitTimeString(int section)
        {
            DateTime now = DateTime.Now;
            DateTime start, limit;
            int nowsection = GetSection(now);
            if (now.Hour >= 0 && now.Hour < 2)
            {
                start = new DateTime(now.Year, now.Month, now.Day).AddDays(-1);
                limit = new DateTime(now.Year, now.Month, now.Day).AddDays(-1);
            }
            else
            {
                start = new DateTime(now.Year, now.Month, now.Day);
                limit = new DateTime(now.Year, now.Month, now.Day);
            }
            //これとは別にSection=3だった場合は1日進める
            if (nowsection == 3)
            {
                start = start.AddDays(1);
                limit = limit.AddDays(1);
            }
            switch (section)
            {
                case 1:
                    start += new TimeSpan(2, 0, 0);
                    limit += new TimeSpan(14, 0, 0);
                    break;
                case 2:
                    //最終日の場合
                    start += new TimeSpan(14, 0, 0);
                    if (start.AddDays(1).Day == 1)
                    {
                        limit += new TimeSpan(22, 0, 0);
                    }
                    limit += new TimeSpan(26, 0, 0);
                    break;
                case 3:
                    start += new TimeSpan(22, 0, 0);
                    limit += new TimeSpan(26, 0, 0);
                    break;
            }
            //セクション終わってた場合
            if (now >= limit) return "確定";
            //始まってない場合
            else if (nowsection != section) return "待ち" + (start - now).ToString(@"h\:mm");
            //セクション中の場合
            else return "あと" + (limit - now).ToString(@"h\:mm");
        }
        #endregion

        /// <summary>
        /// 推定戦果を再計算します
        /// </summary>
        public void CalcEstimateSenka()
        {
            if (this.StartSenka == -1) this.EndSenkaEst = -1;
            else
            {
                double expdiff = this.EndExp - this.StartExp;
                double earnsenka = expdiff * 7 / 10000;
                this.EndSenkaEst = this.StartSenka + earnsenka + this.SpecialSenka;
            }
        }

        //獲得経験値の文字列
        /// <summary>
        /// 現在のセクションの獲得経験値を文字列で取得します
        /// </summary>
        /// <returns></returns>
        public string GetEarnExpString()
        {
            if (this.EndExp == -1) return "NA";
            else return (this.EndExp - this.StartExp).ToString("N0");
        }

        //個人戦果画面に表示用のテキスト（午前・午後）
        public string[] ShowSectionExp()
        {

            string[] str = new string[2];
            switch (this.Section)
            {
                case 1:
                    str[0] = string.Format("{0} ({1})", this.GetEarnExpString(), SenkaRecord.GetTodaysLimitTimeString(1));
                    str[1] = string.Format("{0} ({1})", "-", SenkaRecord.GetTodaysLimitTimeString(2));
                    break;
                case 2:
                    if (this.Morning == null) str[0] = string.Format("{0} ({1})", "NA", SenkaRecord.GetTodaysLimitTimeString(1));
                    else str[0] = string.Format("{0} ({1})", this.Morning.GetEarnExpString(), SenkaRecord.GetTodaysLimitTimeString(1));
                    str[1] = string.Format("{0} ({1})", this.GetEarnExpString(), SenkaRecord.GetTodaysLimitTimeString(2));
                    break;
                case 3:
                    str[0] = string.Format("- ({0})", SenkaRecord.GetTodaysLimitTimeString(1));
                    str[1] = string.Format("- ({0})", SenkaRecord.GetTodaysLimitTimeString(2));
                    break;
            }
            return str;
        }
    }
}
