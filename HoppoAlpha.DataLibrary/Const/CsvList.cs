using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.Const
{
    /// <summary>
    /// CSV用のリスト
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CsvList<T> : System.Collections.Generic.List<string>
    {
        // List<t>のAddを隠蔽し再定義（orverrideできないため）
        public new void Add(string item)
        {
            // ダブルクォーテーションで文字列を囲む
            base.Add("\"" + item + "\"");
        }
    }
}
