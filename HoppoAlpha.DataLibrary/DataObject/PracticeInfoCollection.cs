using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using HoppoAlpha.DataLibrary.RawApi.ApiReqMember;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 演習情報の全データ
    /// </summary>
    [ProtoContract]
    public class PracticeInfoCollection
    {
        /// <summary>
        /// 提督IDをキーとした全データ
        /// </summary>
        [ProtoMember(1)]
        public SortedDictionary<int, PracticeInfoMemberData> AllData {get; set;}

        public PracticeInfoCollection()
        {
            this.AllData = new SortedDictionary<int, PracticeInfoMemberData>();
        }

        /// <summary>
        /// コレクションがNullもしくは空かどうかを判定します
        /// </summary>
        /// <returns>Nullまたは空ならTrue</returns>
        public bool IsNullOfEmpty()
        {
            if (this.AllData == null) return true;

            if (this.AllData.Count == 0) return true;

            return false;
        }

        /// <summary>
        /// 2つのコレクションについて、現在のコレクションがより多くのデータを持っているかどうかを比較します
        /// </summary>
        /// <param name="target">比較する古いコレクション</param>
        /// <returns>要素数が多ければTrue、同じまたは小さければFalse</returns>
        public bool IsIncreasing(PracticeInfoCollection target)
        {
            if (target == null) return false;
            if (target.AllData == null) return false;

            //1個でも多い要素があればTrue
            if (this.AllData.Count > target.AllData.Count) return true;

            var newdata_c = this.AllData.Values.SelectMany(x => x.MemberDataByDate).Count();
            var olddata_c = target.AllData.Values.SelectMany(x => x.MemberDataByDate).Count();
            if (newdata_c > olddata_c) return true;

            //どれも該当しない
            return false;
        }
    }

    /// <summary>
    /// メンバー単位のデータ
    /// </summary>
    [ProtoContract]
    public class PracticeInfoMemberData
    {
        /// <summary>
        /// 日付をキーとしてソートしたメンバー単位のデータ
        /// </summary>
        [ProtoMember(1)]
        public SortedDictionary<DateTime, GetPracticeEnemyinfo> MemberDataByDate { get; set; }


        public PracticeInfoMemberData()
        {
            this.MemberDataByDate = new SortedDictionary<DateTime, GetPracticeEnemyinfo>();
        }
    }
}
