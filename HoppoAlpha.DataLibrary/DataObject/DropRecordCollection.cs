using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// ドロップデータのコレクション
    /// </summary>
    [ProtoContract]
    public class DropRecordCollection
    {
        /// <summary>
        /// ドロップデータのリストを表します
        /// </summary>
        [ProtoMember(1)]
        public List<DropRecord> DataBase { get; set; }
        /// <summary>
        /// ドロップデータのマップヘッダーのツリーを表します
        /// </summary>
        [ProtoMember(2)]
        public MasterHeader MasterMapHeader { get; set; }
        /// <summary>
        /// ドロップ艦のマスターIDと艦名の対応を表します
        /// </summary>
        [ProtoMember(3)]
        public Dictionary<int, string> MasterDropShipHeader { get; set; }
        /// <summary>
        /// ドロップアイテムのマスターIDとアイテム名の対応を表します
        /// </summary>
        [ProtoMember(4)]
        public Dictionary<int, string> MasterDropItemHeader { get; set; }

        #region 内部クラス
        /// <summary>
        /// ドロップヘッダーのトップノード(深さ0)
        /// </summary>
        [ProtoContract]
        public class MasterHeader
        {
            /// <summary>
            /// 海域番号の子ノードを表します
            /// </summary>
            [ProtoMember(1)]
            public Dictionary<int, MasterHeaderArea> ChildrenAreas { get; set; }//ex.1-Object

            [ProtoAfterDeserialization]
            protected void OnDserialized()
            {
                if (ChildrenAreas.Count > 0)
                {
                    foreach (var x in ChildrenAreas.Values) x.ParentMaster = this;
                }
            }

            static int[] operationDifficulty = new int[] { -1, 0, 1, 2, 3 };
            static string[] operationDifficultyStr = new string[] { "全て", "なし", "丙", "乙", "甲" };

            public MasterHeader()
            {
                this.ChildrenAreas = new Dictionary<int, MasterHeaderArea>();
            }

            /// <summary>
            /// 表示用の海域ヘッダーを作成します
            /// </summary>
            /// <returns>内部値とヘッダー名のペア</returns>
            public IEnumerable<KeyValuePair<int, string>> HeaderAreaFactory()
            {
                foreach (var x in Enumerable.Repeat(-1, 1).Concat(ChildrenAreas.Keys.OrderBy(x => x)))
                {
                    if (x == -1) yield return new KeyValuePair<int, string>(x, "全て");
                    else yield return new KeyValuePair<int, string>(x, x.ToString());
                }
            }

            /// <summary>
            /// 「全て」だけのヘッダーを作成します
            /// </summary>
            /// <returns>内部値とヘッダー名のペア</returns>
            public static IEnumerable<KeyValuePair<int, string>> HeaderAllOnlyFactory()
            {
                foreach (var x in Enumerable.Repeat(-1, 1))
                    yield return new KeyValuePair<int, string>(x, "全て");
            }

            /// <summary>
            /// 表示用の難易度別ヘッダーを作成します
            /// </summary>
            /// <returns>内部値とヘッダー名のペア</returns>
            public static IEnumerable<KeyValuePair<int, string>> HeaderDifficultyFactory()
            {
                foreach(int i in Enumerable.Range(0, operationDifficulty.Length))
                {
                    yield return new KeyValuePair<int, string>(operationDifficulty[i], operationDifficultyStr[i]);
                }
            }
        }

        /// <summary>
        /// ドロップヘッダーを海域で集計(深さ1)
        /// </summary>
        [ProtoContract]
        public class MasterHeaderArea
        {
            /// <summary>
            /// マップ番号の子ノードを表します
            /// </summary>
            [ProtoMember(1)]
            public Dictionary<int, MasterHeaderMap> ChildrenMaps { get; set; }//ex. 1-Object
            /// <summary>
            /// 親ノードを表します
            /// ※シリアル化しないでください(Possible recursion detectedエラーでログが破壊されます)
            /// </summary>
            public MasterHeader ParentMaster { get; set; }

            [ProtoAfterDeserialization]
            protected void OnDserialized()
            {
                if (ChildrenMaps.Count > 0)
                {
                    foreach (var x in ChildrenMaps.Values) x.ParentArea = this;
                }
            }

            public MasterHeaderArea()
            {
                this.ChildrenMaps = new Dictionary<int, MasterHeaderMap>();
            }

            /// <summary>
            /// 表示用のマップヘッダーを作成します
            /// </summary>
            /// <returns>内部値とヘッダー名のペア</returns>
            public IEnumerable<KeyValuePair<int, string>> HeaderMapFactory()
            {
                foreach (var x in Enumerable.Repeat(-1, 1).Concat(ChildrenMaps.Keys.OrderBy(x => x)))
                {
                    if (x == -1) yield return new KeyValuePair<int, string>(x, "全て");
                    else yield return new KeyValuePair<int, string>(x, string.Format("{0}:{1}", x, ChildrenMaps[x].MapName));
                }
            }
        }

        /// <summary>
        /// ドロップヘッダーをマップで集計(深さ2)
        /// </summary>
        [ProtoContract]
        public class MasterHeaderMap
        {
            /// <summary>
            /// セル番号の子ノードを表します
            /// </summary>
            [ProtoMember(1)]
            public Dictionary<int, MasterHeaderCell> ChildrenCells { get; set; }//ex. セル番-Object
            /// <summary>
            /// 親ノードを表します
            /// ※シリアル化しないでください(Possible recursion detectedエラーでログが破壊されます)
            /// </summary>
            public MasterHeaderArea ParentArea { get; set; }
            /// <summary>
            /// マップ名を表します
            /// </summary>
            [ProtoMember(3)]
            public string MapName { get; set; }

            [ProtoAfterDeserialization]
            protected void OnDserialized()
            {
                if (ChildrenCells.Count > 0)
                {
                    foreach (var x in ChildrenCells.Values) x.ParentMap = this;
                }
            }

            public MasterHeaderMap()
            {
                this.ChildrenCells = new Dictionary<int, MasterHeaderCell>();
            }

            /// <summary>
            /// 表示用のセルヘッダーを作成します
            /// </summary>
            /// <returns>内部値とヘッダー名のペア</returns>
            public IEnumerable<KeyValuePair<int, string>> HeaderCellFactory()
            {
                foreach (var x in Enumerable.Repeat(-1, 1).Concat(ChildrenCells.Keys.OrderBy(x => x)))
                {
                    if (x == -1) yield return new KeyValuePair<int, string>(x, "全て");
                    else yield return new KeyValuePair<int, string>(x, string.Format("{0}{1}:{2}", ChildrenCells[x].IsBoss ? "★" : "", x, ChildrenCells[x].EnemyFleetName));
                }
            }
        }

        /// <summary>
        /// ドロップヘッダーをセルで集計(深さ3)
        /// </summary>
        [ProtoContract]
        public class MasterHeaderCell
        {
            /// <summary>
            /// 敵編成IDの子ノードを表します（2015/7/17のメンテで消えたのでローカルIDを使用してください）
            /// </summary>
            [ProtoMember(1)]
            [Obsolete]
            public HashSet<int> ChildrenFleets { get; set; }//ex. 526,527,528
            /// <summary>
            /// 親ノードを表します
            /// ※シリアル化しないでください(Possible recursion detectedエラーでログが破壊されます)
            /// </summary>
            public MasterHeaderMap ParentMap { get; set; }
            /// <summary>
            /// ボスかどうかのフラグを表します
            /// </summary>
            [ProtoMember(3)]
            public bool IsBoss { get; set; }
            /// <summary>
            /// 敵艦隊名を表します
            /// </summary>
            [ProtoMember(4)]
            public string EnemyFleetName { get; set; }
            /// <summary>
            /// 敵編成のローカルIDの子ノードを表します
            /// </summary>
            [ProtoMember(5)]
            public HashSet<EnemyFleetTable> ChildrenLocalFleets { get; set; }

            public MasterHeaderCell()
            {
                this.ChildrenFleets = new HashSet<int>();
                this.ChildrenLocalFleets = new HashSet<EnemyFleetTable>();
            }

            /// <summary>
            /// 表示用の敵艦隊ヘッダーを作成します
            /// </summary>
            /// <returns>内部値とヘッダー名のペア</returns>
            public IEnumerable<KeyValuePair<int, string>> HeaderFleetFactory()
            {

                foreach (var x in Enumerable.Repeat(EnemyFleetTable.MakeDummy(), 1).Concat(ChildrenLocalFleets.OrderBy(x => x.LocalID)))
                {
                    string mes = string.Format("{0}{1}",
                        x.LocalShortID,
                        x.OldID == 0 ? "" : string.Format("({0})", x.OldID));

                    if (x.LocalID == -1) yield return new KeyValuePair<int, string>(x.LocalID, "全て");
                    else yield return new KeyValuePair<int, string>(x.LocalID, mes);
                }
            }

            /// <summary>
            /// 古い形式のデータをコンバートします
            /// </summary>
            /// <param name="erecord">敵編成のDB</param>
            public void ConvertOldData(Dictionary<int, EnemyFleetRecord> erecord)
            {
                //2015/7/17以前のデータを引き継ぐ用
                if(this.ChildrenLocalFleets.Count == 0 && this.ChildrenFleets.Count != 0)
                {
                    foreach(int x in this.ChildrenFleets)
                    {
                        EnemyFleetRecord val;
                        if(erecord.TryGetValue(x, out val))
                        {
                            //敵編成データにあった場合
                            EnemyFleetTable table = new EnemyFleetTable()
                            {
                                LocalID = val.LocalID,
                                LocalShortID = val.LocalShortID,
                                OldID = val.ID,
                            };
                            this.ChildrenLocalFleets.Add(table);
                        }
                    }
                }
            }
        }

        [ProtoContract]
        public class EnemyFleetTable
        {
            /// <summary>
            /// ローカルIDを表します
            /// </summary>
            [ProtoMember(1)]
            public int LocalID { get; set; }
            /// <summary>
            /// ローカルIDの短縮版を表します
            /// </summary>
            [ProtoMember(2)]
            public int LocalShortID { get; set; }
            /// <summary>
            /// 旧IDを表します
            /// </summary>
            [ProtoMember(3)]
            public int OldID { get; set; }

            /// <summary>
            /// ダミーデータを作るためのメソッド
            /// </summary>
            /// <returns>ダミーデータ</returns>
            public static EnemyFleetTable MakeDummy()
            {
                EnemyFleetTable dummy = new EnemyFleetTable()
                {
                    LocalID = -1,
                    LocalShortID = -1,
                    OldID = 0,
                };
                return dummy;
            }

            /// <summary>
            /// 敵編成の詳細を表示します
            /// </summary>
            /// <param name="erecord">敵編成レコード</param>
            /// <param name="mstshipdata">船のマスターデータ</param>
            /// <param name="areaid">海域番号</param>
            /// <param name="mapid">マップ番号</param>
            /// <param name="cellid">セル番号</param>
            /// <param name="cellname">艦隊名</param>
            /// <param name="isBoss">ボスフラグ</param>
            /// <returns></returns>
            public string EnemyFleetDetail(Dictionary<int, EnemyFleetRecord> erecord,  ExMasterShipCollection mstshipdata,
                int areaid, int mapid, int cellid, string cellname, bool isBoss)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}{1}-{2}-{3}:{4}", isBoss ? "★" : "", areaid, mapid, cellid, cellname);
                sb.AppendLine();
                sb.AppendFormat("LID:{0}", LocalShortID).AppendLine();
                sb.AppendFormat("編成ハッシュ:{0}", LocalID).AppendLine();
                sb.AppendFormat("旧編成ID:{0}", OldID).AppendLine();
                //敵編成
                sb.AppendLine("-敵編成詳細-");
                EnemyFleetRecord record;
                //LIDで取ってみる
                if(!erecord.TryGetValue(LocalID, out record))
                {
                    //LIDで取れない場合、旧IDで取ってみる
                    erecord.TryGetValue(OldID, out record);
                }
                //成功した場合
                if(record != null)
                {
                    foreach(var x in record.ShipKe)
                    {
                        ExMasterShip dship;
                        if(mstshipdata.TryGetValue(x, out dship))
                        {
                            sb.AppendFormat("{0}{1}", dship.api_name, dship.api_yomi.Replace("-", "")).AppendLine();
                        }
                    }
                }
                //失敗した場合
                else
                {
                    sb.AppendLine("不明");
                }

                return sb.ToString();
            }

        }
        #endregion

        public DropRecordCollection()
        {
            this.DataBase = new List<DropRecord>();
            this.MasterMapHeader = new MasterHeader();
            this.MasterDropShipHeader = new Dictionary<int, string>();
            this.MasterDropItemHeader = new Dictionary<int, string>();
        }

        /// <summary>
        /// コレクションがNullもしくは空かどうかを判定します
        /// </summary>
        /// <returns>Nullまたは空ならTrue</returns>
        public bool IsNullOrEmpty()
        {
            if (this.DataBase == null || this.MasterMapHeader == null || this.MasterDropShipHeader == null
                || this.MasterDropItemHeader == null) return true;

            if (this.DataBase.Count == 0) return true;
            if (MasterMapHeader.ChildrenAreas.Count == 0) return true;
            if (MasterDropShipHeader.Count == 0) return true;

            return false;
        }

        /// <summary>
        /// 2つのコレクションについて、現在のコレクションがより多くのデータを持っているかどうかを比較します
        /// </summary>
        /// <param name="target">比較する古いコレクション</param>
        /// <returns>要素数が多ければTrue、同じまたは小さければFalse</returns>
        public bool IsIncreasing(DropRecordCollection target)
        {
            if (target == null) return false;
            if (target.DataBase == null || target.MasterDropShipHeader == null || target.MasterMapHeader == null
                || target.MasterDropItemHeader == null) return false;

            //どれか1つでも多い要素があればTrue
            if (this.DataBase.Count > target.DataBase.Count) return true;

            if (this.MasterDropShipHeader.Count > target.MasterDropShipHeader.Count) return true;
            if (this.MasterDropItemHeader.Count > target.MasterDropItemHeader.Count) return true;

            var newheader = this.MasterMapHeader.ChildrenAreas.Values
                            .SelectMany(x => x.ChildrenMaps.Values)
                            .SelectMany(x => x.ChildrenCells.Values)
                            .SelectMany(x => x.ChildrenLocalFleets).Count();
            var oldheader = target.MasterMapHeader.ChildrenAreas.Values
                            .SelectMany(x => x.ChildrenMaps.Values)
                            .SelectMany(x => x.ChildrenCells.Values)
                            .SelectMany(x => x.ChildrenLocalFleets).Count();
            if (newheader > oldheader) return true;

            //どれも該当しない
            return false;
        }
    }
}
