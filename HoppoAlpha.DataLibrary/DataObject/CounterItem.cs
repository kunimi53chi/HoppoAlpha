using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// カウンター1条件のアイテム
    /// </summary>
    [ProtoContract]
    public class CounterItem
    {
        /// <summary>
        /// 全マップを対象にするかどうかを表します
        /// </summary>
        [ProtoMember(1)]
        public bool AllMap { get; set; }
        /// <summary>
        /// 対象の海域番号を表します
        /// </summary>
        [ProtoMember(2)]
        public int AreaNo { get; set; }
        /// <summary>
        /// 対象のマップ番号を表します
        /// </summary>
        [ProtoMember(3)]
        public int MapNo { get; set; }
        /// <summary>
        /// カウントの値を表します
        /// </summary>
        [ProtoMember(4)]
        public int Value { get; set; }
        /// <summary>
        /// カウンターの条件を表します
        /// </summary>
        [ProtoMember(5)]
        public CounterMode Mode { get; set; }
        /// <summary>
        /// カウンターが有効かどうかを表します
        /// </summary>
        [ProtoMember(6)]
        public bool Enabled { get; set; }
        /// <summary>
        /// カウンターのIDを表します
        /// </summary>
        [ProtoMember(7)]
        public int ID { get; set; }
        /// <summary>
        /// カウンターの母数を表します
        /// </summary>
        [ProtoMember(8)]
        public int Trial { get; set; }
        /// <summary>
        /// カウンターの自動リセットの条件を表します
        /// </summary>
        [ProtoMember(9)]
        public CounterResetCondition AutoReset { get; set; }
        /// <summary>
        /// 最終更新日時を表します
        /// </summary>
        [ProtoMember(10)]
        public DateTime LastUpdated { get; set; }


        /// <summary>
        /// カウンターのToolTipを取得します
        /// </summary>
        /// <returns>ToolTipの文字列</returns>
        public string GetToolTipText()
        {
            string ratio;
            if (this.Trial == 0) ratio = "-";
            else ratio = ((double)this.Value / (double)this.Trial).ToString("F2");
            return string.Format("カウント:{0} / 出撃回数:{1} (平均:{2})\n最終更新:{3}", this.Value, this.Trial, ratio, this.LastUpdated.ToString());
        }
    }

    /// <summary>
    /// カウンターの条件を表す列挙体
    /// </summary>
    public enum CounterMode
    {
        /// <summary>
        /// 条件なし
        /// </summary>
        None,
        /// <summary>
        /// ボス勝利
        /// </summary>
        BossWin, 
        /// <summary>
        /// ボスS勝利
        /// </summary>
        BossRankS, 
        /// <summary>
        /// ボス到達
        /// </summary>
        Boss, 
        /// <summary>
        /// S勝利
        /// </summary>
        RankS, 
        /// <summary>
        /// 空母撃破数
        /// </summary>
        DestroyCarrier,
        /// <summary>
        /// 補給艦撃破数
        /// </summary>
        DestroySupplyShip, 
        /// <summary>
        /// 敗北数
        /// </summary>
        Lose, 
        /// <summary>
        /// ボス敗北数
        /// </summary>
        BossLose,
        /// <summary>
        /// ボスA勝利以上
        /// </summary>
        BossOverA,
    }

    /// <summary>
    /// カウンターのリセット条件を表します
    /// </summary>
    public enum CounterResetCondition
    {
        /// <summary>
        /// リセットなし
        /// </summary>
        None,
        /// <summary>
        /// 毎朝5時にリセット
        /// </summary>
        Daily,
        /// <summary>
        /// 月曜朝5時にリセット
        /// </summary>
        Weekly,
        /// <summary>
        /// 毎月1日朝5時にリセット
        /// </summary>
        Monthly,
    }

    /// <summary>
    /// カウンター条件の列挙体の拡張メソッド
    /// </summary>
    public static class CounterModeExt
    {
        /// <summary>
        /// カウンター条件を文字列に変換します
        /// </summary>
        /// <param name="m">カウンター条件の列挙体</param>
        /// <returns>カウンター条件の文字列</returns>
        public static string Display(this CounterMode m)
        {
            switch (m)
            {
                case CounterMode.None: return "NULL";
                case CounterMode.BossWin: return "ボス勝利回数";
                case CounterMode.BossRankS: return "ボスS勝利回数";
                case CounterMode.Boss: return "ボス到達回数";
                case CounterMode.RankS: return "S勝利回数";
                case CounterMode.DestroyCarrier: return "空母撃破数";
                case CounterMode.DestroySupplyShip: return "補給艦撃破数";
                case CounterMode.Lose: return "敗北数";
                case CounterMode.BossLose: return "ボス敗北数";
                case CounterMode.BossOverA: return "ボスA勝利以上";
                default: throw new ArgumentException();
            }
        }
        /// <summary>
        /// カウンターのリセット条件を短い文字列に変換します
        /// </summary>
        /// <param name="r">カウンターのリセット条件の列挙体</param>
        /// <returns>リセット条件の文字列</returns>
        public static string ToStrShort(this CounterResetCondition r)
        {
            switch(r)
            {
                case CounterResetCondition.None: return "なし";
                case CounterResetCondition.Daily: return "日";
                case CounterResetCondition.Weekly: return "週";
                case CounterResetCondition.Monthly: return "月";
                default: throw new ArgumentException();
            }
        }

        public static string ToStrLong(this CounterResetCondition r)
        {
            switch(r)
            {
                case CounterResetCondition.None: return "自動リセットなし";
                case CounterResetCondition.Daily: return "デイリー";
                case CounterResetCondition.Weekly: return "ウィークリー";
                case CounterResetCondition.Monthly: return "マンスリー";
                default: throw new ArgumentException();
            }
        }

        /// <summary>
        /// カウンターの自動リセットが必要かどうかを判定します
        /// </summary>
        /// <param name="r">カウンターのリセット条件の列挙体</param>
        /// <param name="target">判定する最終更新日時</param>
        /// <returns>自動リセットが必要かどうかのフラグ</returns>
        public static bool IsAutoResetActivated(this CounterResetCondition r, DateTime target)
        {
            DateTime now = DateTime.Now;
            //リセットされる基準時間を計算
            DateTime reset = DateTime.MinValue;
            DateTime timeshift = now.AddHours(-5);//午前5時リセットだから-5時しておく
            switch(r)
            {
                case CounterResetCondition.None:
                    reset = DateTime.MaxValue;//絶対リセットされない
                    break;
                case CounterResetCondition.Daily:
                    reset = new DateTime(timeshift.Year, timeshift.Month, timeshift.Day, 5, 0, 0);
                    break;
                case CounterResetCondition.Weekly:
                    //timeshiftから足す日 = MOD(8-DayOfWeek, 7) →　日曜（0）=1　月曜（1）＝0　火曜（2）＝6　…
                    reset = new DateTime(timeshift.Year, timeshift.Month, timeshift.Day, 5, 0, 0);
                    reset = reset.AddDays((8 - (int)timeshift.DayOfWeek) % 7);
                    //reset = new DateTime(timeshift.Year, timeshift.Month, timeshift.Day + (8 - (int)timeshift.DayOfWeek) % 7, 5, 0, 0); //これだと月末にクラッシュしてしまう
                    break;
                case CounterResetCondition.Monthly:
                    reset = new DateTime(timeshift.Year, timeshift.Month, 1, 5, 0, 0);
                    break;
            }
            //target < reset < nowならリセット
            return target < reset && reset < now;
        }
    }
}
