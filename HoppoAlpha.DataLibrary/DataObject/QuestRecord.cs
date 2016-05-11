using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiReqQuest;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 任務の記録用
    /// </summary>
    [ProtoContract]
    public class QuestRecord
    {
        /// <summary>
        /// 最後にチェックした日時を表します
        /// </summary>
        [ProtoMember(1)]
        public DateTime LastCheckDate { get; set; }
        /// <summary>
        /// 任務一覧を表します
        /// </summary>
        [ProtoMember(2)]
        public SortedDictionary<int, ApiQuest> Records { get; set; }

        public QuestRecord()
        {
            this.Records = new SortedDictionary<int, ApiQuest>();
        }

        /// <summary>
        /// コレクションがNullもしくは空かどうかを判定します
        /// </summary>
        /// <returns>Nullまたは空ならTrue</returns>
        public bool IsNullOrEmpty()
        {
            if (this.Records == null) return true;

            if (this.LastCheckDate == new DateTime()) return true;
            if (this.Records.Count == 0) return true;

            return false;
        }

        /// <summary>
        /// 2つのコレクションについて、現在のコレクションがより多くのデータを持っているかどうかを比較します
        /// </summary>
        /// <param name="target">比較する古いコレクション</param>
        /// <returns>要素数が多ければTrue、同じまたは小さければFalse</returns>
        public bool IsIncreasing(QuestRecord target)
        {
            if (this.Records == null || target.Records == null) return false;

            if (this.LastCheckDate > target.LastCheckDate) return true;

            return false;
        }

    }
}
