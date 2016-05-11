using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 資材推移の記録
    /// </summary>
    [ProtoContract]
    public class MaterialRecord
    {
        /// <summary>
        /// 日付を表します
        /// </summary>
        [ProtoMember(1)]
        public DateTime Date { get; set; }
        /// <summary>
        /// 資源の値を表します
        /// </summary>
        /// これDictionaryじゃなくてListで書けばよかった(´・ω・｀)
        [ProtoMember(2)]
        public Dictionary<string, int> Value { get; set; }

        /// <summary>
        /// 資源のDictionaryのキー配列を表します
        /// </summary>
        public static string[] Keys { get; set; }
        static MaterialRecord()
        {
            Keys = new string[] { "fuel", "ammo", "steel", "bauxite", "build", "repair", "develop", "no8" };
        }

        /// <summary>
        /// 2つの資源レコードの数値が等しいかどうか比較します
        /// </summary>
        /// <param name="target">比較対象の資源レコード</param>
        public bool IsEquials(MaterialRecord target)
        {
            var m1v = this.Value.Values.ToArray();
            var m2v = target.Value.Values.ToArray();

            foreach (int i in Enumerable.Range(0, Math.Min(m1v.Length, m2v.Length)))
            {
                if (m1v[i] != m2v[i]) return false;
            }

            return true;
        }
    }
}
