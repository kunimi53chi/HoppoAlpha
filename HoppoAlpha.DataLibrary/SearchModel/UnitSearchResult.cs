using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.SearchModel
{
    /// <summary>
    /// 個別索敵値の計算用のクラス
    /// </summary>
    public class UnitSearchResult
    {
        /// <summary>
        /// 旧2-5式用：索敵に関係する装備の索敵値ポイント（彩雲、水上偵察機は２倍）
        /// </summary>
        public int EquipSearchesPoint { get; set; }
        /// <summary>
        /// 旧2-5式用：索敵装備を抜いた素の索敵値
        /// </summary>
        public int StatusSearches { get; set; }
        /// <summary>
        /// 汎用：個別索敵値（int）
        /// </summary>
        public int UnitSearchInt { get; set; }
        /// <summary>
        /// 汎用：個別索敵値（double）
        /// </summary>
        public double UnitSearchDouble { get; set; }
    }
}
