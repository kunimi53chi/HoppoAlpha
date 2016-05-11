using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.SearchModel
{
    /// <summary>
    /// 索敵モデルの列挙体
    /// </summary>
    public enum Models
    {
        /// <summary>
        /// 33式モデル（デフォルト）
        /// </summary>
        Model33,
        /// <summary>
        /// ほっぽアルファVer2.0.1モデル
        /// </summary>
        Hoppo201,
        /// <summary>
        /// 旧2-5式
        /// </summary>
        Old25,
    }

    /// <summary>
    /// 索敵モデルの列挙体の拡張メソッド
    /// </summary>
    public static class ModelsExt
    {
        public static string GetName(this Models m)
        {
            switch(m)
            {
                case Models.Hoppo201: return "ほっぽVer2.0.1モデル";
                case Models.Model33: return "33式";
                case Models.Old25: return "旧2-5式";
                default: return "";
            }
        }
    }
}
