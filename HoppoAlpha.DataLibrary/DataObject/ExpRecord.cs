using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 提督経験値の推移の記録
    /// </summary>
    [ProtoContract]
    public class ExpRecord
    {
        /// <summary>
        /// 日付けを表します
        /// </summary>
        [ProtoMember(1)]
        public DateTime Date { get; set; }
        /// <summary>
        /// 提督経験値を表します
        /// </summary>
        [ProtoMember(2)]
        public int Value { get; set; }
        /// <summary>
        /// 6時間前の提督経験値を表します(参照形式)
        /// </summary>
        [ProtoMember(3, AsReference = true)]
        public ExpRecord Before6H { get; set; }
        /// <summary>
        /// 12時間前の提督経験値を表します(参照形式)
        /// </summary>
        [ProtoMember(4, AsReference = true)]
        public ExpRecord Before12H { get; set; }
        /// <summary>
        /// 24時間前の提督経験値を表します(参照形式)
        /// </summary>
        [ProtoMember(5, AsReference = true)]
        public ExpRecord Before24H { get; set; }
    }
}
