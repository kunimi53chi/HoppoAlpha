using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 艦娘の状態を表す列挙体です
    /// </summary>
    [Flags]
    public enum HPCondition
    {
        /// <summary>
        /// HPが満タン状態
        /// </summary>
        Full = 0, 
        /// <summary>
        /// 小破未満の状態
        /// </summary>
        None = 1,
        /// <summary>
        /// 小破の状態
        /// </summary>
        SmallDamage = 2,
        /// <summary>
        /// 中破の状態
        /// </summary>
        MiddleDamage = 4,
        /// <summary>
        /// 大破の状態
        /// </summary>
        HeavyDamage = 8,
        /// <summary>
        /// 撃沈の状態
        /// </summary>
        Sank = 16,
        //------以下はフラグ------
        /// <summary>
        /// 入渠しているかどうか
        /// </summary>
        IsBathing = 256,
        /// <summary>
        /// 護衛退避しているかどうか
        /// </summary>
        IsWithdrawn = 512,
        /// <summary>
        /// 指定したHP以下かどうか
        /// </summary>
        IsUnderSelectedHPRatio = 1024,
        /// <summary>
        /// ダメコンを所持しているかどうか
        /// </summary>
        HasDamecon = 2048,
        /// <summary>
        /// ダメコン（女神）を所持しているかどうか
        /// </summary>
        HasDameconGoddess = 4096,
        /// ------以下はマジックナンバー------
        /// <summary>
        /// フラグを消すための数字。＆～で結合することでフラグを消去できます
        /// </summary>
        EraseFlagsMagicNumber = IsBathing | IsWithdrawn | IsUnderSelectedHPRatio | HasDamecon | HasDameconGoddess,
        /// <summary>
        /// HPが一定以下になったときに*印を表示するための数字。＆～で結合することでフラグを消去できます
        /// </summary>
        EraseUnderHPNotifyMagicNumber = None | SmallDamage | MiddleDamage | HeavyDamage | IsWithdrawn | HasDamecon | HasDameconGoddess,
    }

    /// <summary>
    /// 艦娘の列挙体の状態の拡張メソッド
    /// </summary>
    public static class HPConditionExt
    {
        public static string DisplayLong(HPCondition c)
        {
            string str = c.Display();
            if ((c & ~HPCondition.EraseUnderHPNotifyMagicNumber) == HPCondition.IsUnderSelectedHPRatio) str = str + "*";

            if (str == "") return "";
            else return "[" + str + "]";
        }

        public static string DisplayShort(HPCondition c)
        {
            string str = c.Display();
            if (str == "") return "";
            else if (str == "撃沈") return "[沈]";
            else return "[" + str.Substring(0, 1) + "]";
        }

        /*
        public static string DisplaySecret(HPCondition c)
        {
            string s;
            if (c.HasFlag(HPCondition.None) || c.HasFlag(HPCondition.Full)) s = "健在";
            else s = c.Display();
            return s;
        }*/

        public static string Display(this HPCondition c)
        {
            if (c.HasFlag(HPCondition.IsBathing)) return "入渠";
            if (c.HasFlag(HPCondition.IsWithdrawn)) return "退避";

            var erase = c & ~HPCondition.EraseFlagsMagicNumber;//フラグ部分を消去
            switch (erase)
            {
                case HPCondition.Full: return "";
                case HPCondition.None: return "";
                case HPCondition.SmallDamage: return "小破";
                case HPCondition.MiddleDamage: return "中破";
                case HPCondition.HeavyDamage: return "大破";
                case HPCondition.Sank: return "撃沈";
                default: throw new ArgumentException();
            }
        }
    }

}
