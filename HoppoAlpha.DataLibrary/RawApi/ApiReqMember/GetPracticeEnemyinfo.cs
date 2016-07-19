using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using HoppoAlpha.DataLibrary.DataObject;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqMember
{
    /// <summary>
    /// 演習相手の艦の情報
    /// </summary>
    [ProtoContract]
    public class ApiPracticeShip
    {
        /// <summary>
        /// 艦のグローバルID？（存在しないなら-1）
        /// </summary>
        [ProtoMember(1)]
        public int api_id { get; set; }
        /// <summary>
        /// 艦のマスターID
        /// </summary>
        [ProtoMember(2)]
        public int api_ship_id { get; set; }
        /// <summary>
        /// 艦のレベル
        /// </summary>
        [ProtoMember(3)]
        public int api_level { get; set; }
        /// <summary>
        /// 艦の近代化改修
        /// </summary>
        [ProtoMember(4)]
        public int api_star { get; set; }
        /// <summary>
        /// 艦名（JSONで読み込まれないのであとでメソッドから取得）
        /// </summary>
        [ProtoMember(50)]
        public string ShipName { get; set; }
    }

    /// <summary>
    /// 演習相手の艦隊の情報
    /// </summary>
    [ProtoContract]
    public class ApiPracticeDeck
    {
        /// <summary>
        /// 演習相手の艦隊の艦一覧
        /// </summary>
        [ProtoMember(1)]
        public List<ApiPracticeShip> api_ships { get; set; }
    }

    /// <summary>
    /// 演習相手の情報
    /// </summary>
    [ProtoContract]
    public class GetPracticeEnemyinfo
    {
        /// <summary>
        /// 演習相手の提督ID
        /// </summary>
        [ProtoMember(1)]
        public int api_member_id { get; set; }
        /// <summary>
        /// 演習相手の提督名
        /// </summary>
        [ProtoMember(2)]
        public string api_nickname { get; set; }
        /// <summary>
        /// 演習相手の提督名のID
        /// </summary>
        [ProtoMember(3)]
        public string api_nickname_id { get; set; }
        /// <summary>
        /// 演習相手のランキングコメント
        /// </summary>
        [ProtoMember(4)]
        public string api_cmt { get; set; }
        /// <summary>
        /// 演習相手のランキングコメントのID
        /// </summary>
        [ProtoMember(5)]
        public string api_cmt_id { get; set; }
        /// <summary>
        /// 演習相手の司令部レベル
        /// </summary>
        [ProtoMember(6)]
        public int api_level { get; set; }
        /// <summary>
        /// 演習相手の階級
        /// </summary>
        [ProtoMember(7)]
        public int api_rank { get; set; }
        /// <summary>
        /// 演習相手の提督経験値（[0]Total, [1]次回レベルアップ時のTotal）
        /// </summary>
        [ProtoMember(8)]
        public List<int> api_experience { get; set; }
        /// <summary>
        /// 演習相手の友軍艦隊数（未実装）
        /// </summary>
        [ProtoMember(9)]
        public int api_friend { get; set; }
        /// <summary>
        /// 演習相手の艦娘保有数（[0]Now, [1]Max）
        /// </summary>
        [ProtoMember(10)]
        public List<int> api_ship { get; set; }
        /// <summary>
        /// 演習相手の装備保有数（[0]Now, [1]Max）
        /// </summary>
        [ProtoMember(11)]
        public List<int> api_slotitem { get; set; }
        /// <summary>
        /// 演習相手の家具保有数
        /// </summary>
        [ProtoMember(12)]
        public int api_furniture { get; set; }
        /// <summary>
        /// 演習相手の第一艦隊の艦隊名
        /// </summary>
        [ProtoMember(13)]
        public string api_deckname { get; set; }
        /// <summary>
        /// 演習相手の第一艦隊の艦隊名のID
        /// </summary>
        [ProtoMember(14)]
        public string api_deckname_id { get; set; }
        /// <summary>
        /// 演習相手の第一艦隊の情報
        /// </summary>
        [ProtoMember(15)]
        public ApiPracticeDeck api_deck { get; set; }
        /// <summary>
        /// データを取得した日時（取得時に追加）
        /// </summary>
        [ProtoMember(50)]
        public DateTime GetDatetime { get; set; }


        public void SetShipNames(ExMasterShipCollection exmasterShips)
        {
            if(this.api_deck == null || this.api_deck.api_ships == null) return;

            foreach(var s in api_deck.api_ships)
            {
                if (s.api_id < 0) continue;

                ExMasterShip dship;
                if(exmasterShips.TryGetValue(s.api_ship_id, out dship))
                {
                    s.ShipName = dship.api_name;
                }
            }
        }
    }
}
