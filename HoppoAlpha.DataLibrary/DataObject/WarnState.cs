using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 大破警告の状態を表す列挙体
    /// </summary>
    public enum WarnState
    {
        /// <summary>
        /// 0 : ConditionGreen（異常なし）を表します
        /// </summary>
        ConditionGreen = 0,
        /// <summary>
        /// 1 : 非ロックの大破艦ありを表します
        /// </summary>
        ShipUnlockedDamaged = 1,
        /// <summary>
        /// 2 : ダメコン装備艦の大破を表します
        /// </summary>
        HasDameconDamaged = 2,
        /// <summary>
        /// 3 : 旗艦大破を表します
        /// </summary>
        FlagshipDamaged = 3,
        /// <summary>
        /// 4 : 非ロックでロック済装備艦の大破
        /// </summary>
        ShipUnlockedAndEquipsLockedDamaged = 4,
        /// <summary>
        /// 5 : 主力艦の大破
        /// </summary>
        LockedShipDamagedWarning = 5,
    }
}
