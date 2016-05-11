using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class HPConditionViewExt
    {
        public static System.Drawing.Color GetColor(this HPCondition c)
        {
            if (c.HasFlag(HPCondition.IsWithdrawn)) return KancolleInfo.WithdrawingColor;
            if (c.HasFlag(HPCondition.IsBathing)) return KancolleInfo.DefaultStringColor;

            var erase = c & ~HPCondition.EraseFlagsMagicNumber;
            switch (erase)
            {
                case HPCondition.HeavyDamage: return KancolleInfo.WarningStringColor;
                case HPCondition.MiddleDamage: return KancolleInfo.CautionStringColor;
                case HPCondition.SmallDamage: return KancolleInfo.TiredStringColor;
                case HPCondition.None: return KancolleInfo.TiredStringColor;
                case HPCondition.Sank: return KancolleInfo.DeadStringColor;
                default: return KancolleInfo.DefaultStringColor;
            }
        }

        //BattleDetail用のGetColor
        public static System.Drawing.Color GetColorBattleDetail(this HPCondition c)
        {
            if (c.HasFlag(HPCondition.SmallDamage)) return KancolleInfo.DefaultStringColor;
            else return c.GetColor();
        }

        public static System.Drawing.Color GetBackColor(this HPCondition c)
        {
            if (c.HasFlag(HPCondition.IsWithdrawn)) return KancolleInfo.WithdrawBackColor;
            if (c.HasFlag(HPCondition.IsBathing)) return KancolleInfo.Transparent;

            var erase = c & ~HPCondition.EraseFlagsMagicNumber;
            switch (erase)
            {
                case HPCondition.HeavyDamage: return KancolleInfo.WarningBackColor;
                case HPCondition.MiddleDamage: return KancolleInfo.CautionBackColor;
                case HPCondition.Sank: return KancolleInfo.DeadBackColor;
                default: return KancolleInfo.Transparent;
            }
        }

        public static System.Drawing.Color GetBackColorBattleDetail(this HPCondition c)
        {
            if (c.HasFlag(HPCondition.SmallDamage)) return System.Drawing.Color.FromArgb(223, 223, 223);//(248, 224, 114)
            else return c.GetBackColor();
        }

        public static System.Drawing.Color GetBackColorBattleDetailPercent(this HPCondition c)
        {
            if ((c & ~HPCondition.EraseUnderHPNotifyMagicNumber) == HPCondition.IsUnderSelectedHPRatio) return System.Drawing.Color.Aquamarine;
            else return KancolleInfo.Transparent;
        }
    }
}
