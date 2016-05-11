using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 出撃報告書のコレクションを表すクラス
    /// </summary>
    [ProtoContract]
    public class SortieReportCollection
    {
        /// <summary>
        /// 出撃報告書のコレクション
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<SortieReportMapHash, SortieReport> Collection { get; set; }
        /// <summary>
        /// この週の開始時間（5時間時差がある）
        /// </summary>
        [ProtoMember(2)]
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// この週の終了時間（5時間時差がある）
        /// </summary>
        [ProtoMember(3)]
        public DateTime EndTime { get; private set; }
        /// <summary>
        /// 週番号
        /// </summary>
        [ProtoMember(4)]
        public int WeeklyIndex { get; private set; }

        //コンストラクタ
        public SortieReportCollection()
        {
        }

        public SortieReportCollection(DateTime now)
        {
            int weeklyindex = SortieReportCollection.Helper.GetNowWeeklyIndex(now);
            Collection = new Dictionary<SortieReportMapHash, SortieReport>();
            StartTime = SortieReportCollection.Helper.GetWeeklyMinDate(now.Year, weeklyindex);
            EndTime = SortieReportCollection.Helper.GetWeeklyMaxDate(now.Year, weeklyindex);
            WeeklyIndex = weeklyindex;
        }

        /// <summary>
        /// ２つ以上の出撃報告書を統合します
        /// </summary>
        /// <param name="targets">統合する出撃報告書</param>
        /// <returns>統合された出撃報告書</returns>
        public SortieReportCollection MergeMany(IEnumerable<SortieReportCollection> targets)
        {
            //自分自身をディープコピー
            var result = this.DeepCopy();

            //ターゲットを統合していく
            foreach(var t in targets)
            {
                //出撃報告書コレクションの統合
                foreach(var c in t.Collection)
                {
                    SortieReport report;
                    if(result.Collection.TryGetValue(c.Key, out report))
                    {
                        report = report.Integrate(c.Value);
                    }
                    else
                    {
                        report = c.Value.DeepCopy();
                    }
                    result.Collection[c.Key] = report;
                }

                //開始日時は小さい所をとる
                if (t.StartTime < result.StartTime) result.StartTime = t.StartTime;
                //終了日時は大きい所をとる
                if (t.EndTime > result.EndTime) result.EndTime = t.EndTime;
                //週番号は（複数あるなら）-1を代入しておく？
                result.WeeklyIndex = -1;
            }

            //コレクションをソートにかける
            result.Collection.OrderBy(x => x.Key.GetHashCode()).ToDictionary(k => k.Key, v => v.Value);

            return result;
        }

        //内部クラス
        #region 内部クラス
        /// <summary>
        /// 週番号を計算するためのヘルパークラス
        /// </summary>
        public static class Helper
        {
            /// <summary>
            /// 現在の週番号を計算
            /// </summary>
            /// <param name="date">計算対象の日付け</param>
            /// <returns>週番号</returns>
            public static int GetNowWeeklyIndex(DateTime date)
            {
                /// 曜日のインデックス = {DayOfWeek(日曜日＝0、土曜日＝6) + 6} mod 7 →月曜日＝0、日曜日＝6
                ///　x = 1/1からの経過日数（1/1は1）　＋　1/1の曜日のインデックス
                ///　週番号n = INT[ (x+6)/7 ]
                DateTime delay = date.AddHours(-5);//デイリーにあわせる

                int newyear_dayofweek = ((int)(new DateTime(delay.Year, 1, 1).DayOfWeek) + 6) % 7;//1/1の曜日インデックス
                int x = delay.DayOfYear + newyear_dayofweek;//曜日補正込みの積算日数
                int weekindex = (x + 6) / 7;

                return weekindex;
            }

            /// <summary>
            /// 現在の週番号、年を一意に求めるハッシュを取得
            /// </summary>
            /// <param name="date">計算対象の日付け</param>
            /// <returns>週番号</returns>
            public static int GetNowWeeklyIndexHash(DateTime date)
            {
                DateTime delay = date.AddHours(-5);

                return delay.Year * 100 + GetNowWeeklyIndex(date);
            }

            /// <summary>
            /// 週番号からその週のはじまる日付けの計算
            /// </summary>
            /// <param name="year">年の指定</param>
            /// <param name="weeklyindex">週番号</param>
            /// <returns>週の開始日時</returns>
            public static DateTime GetWeeklyMinDate(int year, int weeklyindex)
            {
                // 開始x = 週番号*7-6  / 終了x = 週番号*7
                if (year < 0 || weeklyindex <= 0) throw new ArgumentException("yearまたはweeklyindexが不正です");
                // 1/1の曜日インデックスを引いて、1/1からのDayOfYearを計算する
                int newyear_dayofweek = ((int)(new DateTime(year, 1, 1).DayOfWeek + 6)) % 7;
                int dayofyear = Math.Max((weeklyindex * 7 - 6) - newyear_dayofweek, 1);//昨年の場合は打ち切り
                //DayOfYearからDateTimeの変換
                DateTime startdate = new DateTime(year, 1, 1).AddDays(dayofyear - 1);

                return startdate;
            }

            /// <summary>
            /// 週番号からその週の終わる日付けの計算
            /// </summary>
            /// <param name="year">年の指定</param>
            /// <param name="weeklyindex">週番号</param>
            /// <returns>週の終了日時</returns>
            public static DateTime GetWeeklyMaxDate(int year, int weeklyindex)
            {
                // 開始x = 週番号*7-6  / 終了x = 週番号*7
                if (year < 0 || weeklyindex <= 0) throw new ArgumentException("yearまたはweeklyindexが不正です");
                //1年の日数の取得
                int daysinyear = DateTime.IsLeapYear(year) ? 366 : 365;
                // 1/1の曜日インデックスを引いて、1/1からのDayOfYearを計算する
                int newyear_dayofweek = ((int)(new DateTime(year, 1, 1).DayOfWeek + 6)) % 7;
                int dayofyear = Math.Min((weeklyindex * 7) - newyear_dayofweek, daysinyear);//翌年の場合は打ち切り
                //DayOfYearからDateTimeに変換
                DateTime enddate = new DateTime(year, 1, 1).AddDays(dayofyear - 1);

                return enddate;
            }

            /// <summary>
            /// ファイル集計用のstring作成
            /// </summary>
            /// <param name="year">年の指定</param>
            /// <param name="weeklyIndex">週番号</param>
            /// <param name="termMode">期間指定モード</param>
            /// <returns>ファイル集計用のstring</returns>
            public static IEnumerable<string> IntegrateYearAndWeeklyIndex(int year, int weeklyIndex, SortieReportTermIntegrateMode termMode)
            {
                switch(termMode)
                {
                    case SortieReportTermIntegrateMode.None:
                        throw new ArgumentException("SortieReportTermIntegrateMode.Noneでは計算できません");
                    case SortieReportTermIntegrateMode.All:
                        yield return "全て";
                        break;
                    case SortieReportTermIntegrateMode.Month:
                        var start = GetWeeklyMinDate(year, weeklyIndex);
                        var end = GetWeeklyMaxDate(year, weeklyIndex);
                        for(int i =start.Month; i<=end.Month; i++)
                        {
                            yield return string.Format("{0}年{1}月", year, i);
                        }
                        break;
                    case SortieReportTermIntegrateMode.Week:
                        yield return string.Format("{0}年{1}週", year, weeklyIndex);
                        break;
                    case SortieReportTermIntegrateMode.Year:
                        yield return string.Format("{0}年", year);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion

        /// <summary>
        /// コレクションがNullもしくは空かどうかを判定します
        /// </summary>
        /// <returns>Nullまたは空ならTrue</returns>
        public bool IsNullOrEmpty()
        {
            return this.Collection == null || this.Collection.Count == 0;
        }

        /// <summary>
        /// 2つのコレクションについて、現在のコレクションがより多くのデータを持っているかどうかを比較します
        /// </summary>
        /// <param name="target">比較する古いコレクション</param>
        /// <returns>要素数が多ければTrue、同じまたは小さければFalse</returns>
        public bool IsIncreasing(SortieReportCollection target)
        {
            if (target == null) return false;
            if (target.Collection == null) return false;

            return this.Collection.Count >= target.Collection.Count;
        }

        /// <summary>
        /// インスタンスをディープコピーをします
        /// </summary>
        /// <returns>複製されたインスタンス</returns>
        public SortieReportCollection DeepCopy()
        {
            SortieReportCollection collection = new SortieReportCollection();

            collection.Collection = new Dictionary<SortieReportMapHash,SortieReport>();
            foreach(var x in this.Collection)
            {
                collection.Collection[x.Key] = x.Value.DeepCopy();
            }

            collection.StartTime = this.StartTime;
            collection.EndTime = this.EndTime;
            collection.WeeklyIndex = this.WeeklyIndex;

            return collection;
        }
    }
}
