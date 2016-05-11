using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.Const;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// ドロップデータの記録
    /// </summary>
    [ProtoContract]
    public class DropRecord
    {
        /// <summary>
        /// 海域番号を表します
        /// </summary>
        [ProtoMember(1)]
        public int MapAreaID { get; set; }
        /// <summary>
        /// マップ番号を表します
        /// </summary>
        [ProtoMember(2)]
        public int MapInfoID { get; set; }
        /// <summary>
        /// セル番号を表します
        /// </summary>
        [ProtoMember(3)]
        public int MapCellID { get; set; }

        /// <summary>
        /// ボスかどうかのフラグを表します
        /// </summary>
        [ProtoMember(4)]
        public bool BossFlag { get; set; }
        /// <summary>
        /// 戦闘評価を表します
        /// </summary>
        [ProtoMember(5)]
        public string WinRank { get; set; }

        /// <summary>
        /// 司令部レベルを表します
        /// </summary>
        [ProtoMember(6)]
        public int HQLevel { get; set; }
        /// <summary>
        /// イベントでの作戦難易度を表します
        /// 0=なし、1=丙、2=乙、3=甲
        /// </summary>
        [ProtoMember(7)]
        public int MapDifficulty { get; set; }

        /// <summary>
        /// ドロップ艦があったかどうかのフラグを表します
        /// </summary>
        [ProtoMember(8)]
        public bool DropShipFlag { get; set; }
        /// <summary>
        /// ドロップ艦のマスターIDを表します
        /// </summary>
        [ProtoMember(9)]
        public int DropShipID { get; set; }
        /// <summary>
        /// ドロップ艦が母港に存在するかどうかを表します
        /// </summary>
        [ProtoMember(10)]
        public bool DropShipAlreadyExists { get; set; }

        /// <summary>
        /// ドロップアイテムがあったどうかのフラグを表します（未実装）
        /// </summary>
        [ProtoMember(11)]
        public bool DropItemFlag { get; set; }
        /// <summary>
        /// ドロップアイテムのIDを表します（未実装）
        /// </summary>
        [ProtoMember(12)]
        public int DropItemID { get; set; }
        /// <summary>
        /// ドロップアイテムが既に存在するかどうかを表します（未実装）
        /// </summary>
        [ProtoMember(13)]
        public bool DropItemAlreadyExists { get; set; }

        /// <summary>
        /// ドロップ時の艦隊編成を表します
        /// </summary>
        [ProtoMember(14)]
        public string[] FleetShipName { get; set; }
        /// <summary>
        /// ドロップ時の艦隊のLvを表します
        /// </summary>
        [ProtoMember(15)]
        public int[] FleetShipLevel { get; set; }
        /// <summary>
        /// ドロップ時の連合艦隊第2の編成を表します
        /// </summary>
        [ProtoMember(16)]
        public string[] FleetCombinedShipName { get; set; }
        /// <summary>
        /// ドロップ時の連合艦隊第2のLvを表します
        /// </summary>
        [ProtoMember(17)]
        public int[] FleetCombinedShipLevel { get; set; }

        /// <summary>
        /// ドロップしたときの日時を表します
        /// </summary>
        [ProtoMember(18)]
        public DateTime DropDate { get; set; }
        /// <summary>
        /// ドロップしたときの敵編成IDを表します（15/7/17のメンテで消えたのでローカルIDを使用してください）
        /// </summary>
        [ProtoMember(19)]
        [Obsolete]
        public int EnemyFleetID { get; set; }
        /// <summary>
        /// ドロップカットしたかどうかのフラグを表します
        /// </summary>
        [ProtoMember(20)]
        public bool DropDisabled { get; set; }
        /// <summary>
        /// ドロップしたときの敵編成LIDを表します
        /// </summary>
        [ProtoMember(21)]
        public int EnemyFleetLocalID { get; set; }
        /// <summary>
        /// ドロップしたときの敵編成LID(短縮版)を表します
        /// </summary>
        [ProtoMember(22)]
        public int EnemyFleetLocalShortID { get; set; }
        /// <summary>
        /// ドロップした艦が存在するかどうか。このフラグは改造の前後を加味します。（2016/2追加）
        /// </summary>
        [ProtoMember(23)]
        public bool DropShipAlreadyExistsContainsRemodeling { get; set; }
        /// <summary>
        /// ドロップした艦を既に保持している個数。このフラグは改造の前後を加味します。（2016/2追加）
        /// </summary>
        [ProtoMember(24)]
        public int DropShipNumOfExistsContainsRemodeling { get; set; }

        public static string[] LogConvertHeader { get; set; }

        static DropRecord()
        {
            LogConvertHeader = new string[]
            {
                "日付", "エリア", "マップ", "セル", "ボスフラグ", 
                "旧ID", "ローカルID", "ハッシュ", "勝利", "司令部レベル",   
                "難易度", "ドロップ封じ","ドロップ有無", "ドロップ艦ID", "ドロップ艦名",   
                "ドロップ艦の所持有無", "ドロップ艦の所持有無(改造)", "ドロップ艦の所持数", "アイテム獲得有無", "アイテムのID", 
                "アイテム名", "アイテムの所持有無", "味方艦隊", "連合艦隊",
            };
        }

        /// <summary>
        /// 古い形式のデータをコンバートします
        /// </summary>
        /// <param name="header">ドロップレコードのヘッダー</param>
        public void ConvertOldData(DropRecordCollection.MasterHeader header)
        {
            //2015/7/17以前のデータをインポート
            if(this.EnemyFleetLocalID == 0 && this.EnemyFleetLocalShortID == 0 && this.EnemyFleetID != 0)
            {
                DropRecordCollection.MasterHeaderArea area;
                if(header.ChildrenAreas.TryGetValue(this.MapAreaID, out area))
                {
                    DropRecordCollection.MasterHeaderMap map;
                    if(area.ChildrenMaps.TryGetValue(this.MapInfoID, out map))
                    {
                        DropRecordCollection.MasterHeaderCell cell;
                        if(map.ChildrenCells.TryGetValue(this.MapCellID, out cell))
                        {
                            //旧編成IDを含んでいるか
                            int oldindex = Array.IndexOf(cell.ChildrenFleets.ToArray(), this.EnemyFleetID);
                            if(oldindex >= 0)
                            {
                                var newarray = cell.ChildrenLocalFleets.ToArray();
                                if(oldindex < newarray.Length)
                                {
                                    var newvalue = newarray[oldindex];
                                    this.EnemyFleetLocalID = newvalue.LocalID;
                                    this.EnemyFleetLocalShortID = newvalue.LocalShortID;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ログのコンバートで出力する行単位のテキスト
        /// </summary>
        /// <param name="dropshipheader">ドロップ艦名のマスターヘッダー</param>
        /// <param name="dropitemheader">ドロップアイテムのマスターヘッダー</param>
        /// <returns>行のテキスト</returns>
        public string MakeLogConvertText(Dictionary<int, string> dropshipheader, Dictionary<int, string> dropitemheader)
        {
            //行の作成
            CsvList<string> row = new CsvList<string>();
            
            //1 : 日付
            row.Add(this.DropDate.ToString());
            //2 : エリア
            row.Add(this.MapAreaID.ToString());
            //3 : マップ
            row.Add(this.MapInfoID.ToString());
            //4 : セル
            row.Add(this.MapCellID.ToString());
            //5 : ボスフラグ
            row.Add(this.BossFlag.ToString());
            //6 : 旧ID
            row.Add(this.EnemyFleetID.ToString());
            //7 : ローカルID(Short)
            row.Add(this.EnemyFleetLocalShortID.ToString());
            //8 : ハッシュ（ローカルID）
            row.Add(this.EnemyFleetLocalID.ToString());
            //9 : 勝利
            row.Add(this.WinRank);
            //10 : 司令部レベル
            row.Add(this.HQLevel.ToString());
            //11 : 難易度
            row.Add(this.MapDifficulty.ToString());//とりあえず0で
            //12 : ドロップ封じ
            row.Add(this.DropDisabled.ToString());
            //13 : ドロップ有無
            row.Add(this.DropShipFlag.ToString());
            //14 : ドロップID
            row.Add(this.DropShipID.ToString());
            //15 : ドロップ艦名
            row.Add(this.DropShipID == -1 ? "なし" : (dropshipheader.ContainsKey(this.DropShipID) ? dropshipheader[this.DropShipID] : "不明"));
            //16 : ドロップ艦の所持有無
            row.Add(this.DropShipAlreadyExists.ToString());
            //23 : ドロップ艦の所持有無（改造）
            row.Add(this.DropShipAlreadyExistsContainsRemodeling.ToString());
            //24 : ドロップ艦の所持数（改造）
            row.Add(this.DropShipNumOfExistsContainsRemodeling.ToString());
            //17 : アイテムの獲得有無
            row.Add(this.DropItemFlag.ToString());
            //18 : アイテムのID
            row.Add(this.DropItemID.ToString());
            //19 : アイテム名
            row.Add(this.DropItemID == -1 ? "なし" : (dropitemheader.ContainsKey(this.DropItemID) ? dropitemheader[this.DropItemID] : "不明"));
            //20 : アイテムの所持有無
            row.Add(this.DropItemAlreadyExists.ToString());
            //21 : 味方艦隊
            row.Add(this.FleetShipName == null ? "" :
                string.Join(" ", Enumerable.Range(0, Math.Min(this.FleetShipName.Length, this.FleetShipLevel.Length))
                    .Select(i => string.Format("{0}(Lv{1})", this.FleetShipName[i], this.FleetShipLevel[i]))));
            //22 : 連合艦隊
            row.Add(this.FleetCombinedShipName == null ? "" :
                string.Join(" ", Enumerable.Range(0, Math.Min(this.FleetCombinedShipName.Length, this.FleetCombinedShipLevel.Length))
                    .Select(i => string.Format("{0}(Lv{1})", this.FleetCombinedShipName[i], this.FleetCombinedShipLevel[i]))));

            //返り値
            return string.Join(",", row.ToArray());
        }
    }
}
