using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 敵編成の記録
    /// </summary>
    [ProtoContract]
    public class EnemyFleetRecord
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
        public int MapInfoNo { get; set; }
        /// <summary>
        /// セルの番号を表します
        /// </summary>
        [ProtoMember(3)]
        public int CellNo { get; set; }
        /// <summary>
        /// 敵編成IDを表します(2015/7/17で消えたので使用しないでください)
        /// </summary>
        [ProtoMember(4)]
        [Obsolete]
        public int ID { get; set; }
        /// <summary>
        /// 敵艦船のマスターIDを表します
        /// </summary>
        [ProtoMember(5)]
        public List<int> ShipKe { get; set; }
        /// <summary>
        /// 敵艦船のレベルを表します
        /// </summary>
        [ProtoMember(6)]
        public List<int> ShipLv { get; set; }
        /// <summary>
        /// 敵艦船の装備のマスターIDを表します
        /// </summary>
        [ProtoMember(7)]
        public Dictionary<int, List<int>> ESlot { get; set; }
        /// <summary>
        /// 敵艦船のパラメーターを表します
        /// </summary>
        [ProtoMember(8)]
        public Dictionary<int, List<int>> EParam { get; set; }
        /// <summary>
        /// 敵の陣形を表します
        /// 1=単縦、2=複縦、3=輪形、4=梯形、5=単横
        /// </summary>
        [ProtoMember(9)]
        public int Formation { get; set; }
        /// <summary>
        /// ローカルIDを表します
        /// </summary>
        [ProtoMember(10)]
        public int LocalID { get; set; }
        /// <summary>
        /// ローカルID(短縮版)を表します
        /// </summary>
        [ProtoMember(11)]
        public int LocalShortID { get; set; }

        /// <summary>
        /// Excelのヘッダーを表します
        /// </summary>
        public static string[] ExcelHeader { get; set; }

        //コンストラクタ
        public EnemyFleetRecord()
        {

        }

        static EnemyFleetRecord()
        {
            ExcelHeader = new string[]
            {
                "海域", "マップ", "セル", "旧ID", "ローカルID", "ハッシュ",  "旗艦ID", "旗艦名", "僚艦1_ID", "僚艦1_名前", "僚艦2_ID", "僚艦2_名前", "僚艦3_ID", "僚艦3_名前", "僚艦4_ID", "僚艦4_名前", "僚艦5_ID", "僚艦5_名前", "陣形", "敵制空値", "優勢", "確保",
                //0     1           2       3          4          5            6        7         8            9             10   　　    11     　　　 12          13           14    　　　　15　　　　　16　　　　　　　17       18          19       20        21
            };
        }

        //シリアル化後に読まれるもの
        [ProtoAfterDeserialization]
        protected void OnDeserialized()
        {
            //2015/7/17以前のデータがある場合
            if(this.LocalID == 0 && this.LocalShortID == 0 && this.ID != 0)
            {
                UserEnemyID uid = new UserEnemyID()
                {
                    api_maparea_id = this.MapAreaID,
                    api_mapinfo_no = this.MapInfoNo,
                    api_cell_id = this.CellNo,
                    api_ship_ke = this.ShipKe,
                    api_formation_enemy = this.Formation,
                };

                this.LocalID = uid.MakeLongHashCode();
                this.LocalShortID = uid.MakeShortHashCode(this.LocalID);
            }
        }

        //Excelにコンバート用
        public string[] ConvertToExcelText(string[] shipname, int[] shipid, int enemyAirSup, string enemyFormation)
        {
            //eSlotのダミー用
            List<int> edummy = Enumerable.Repeat(-1, 5).ToList();

            //優勢・確保
            int air1 = (int)Math.Ceiling((double)enemyAirSup * (double)1.5);//端数切り上げ
            int air2 = enemyAirSup * 3;
            //データに格納
            return new string[]
                        {
                            MapAreaID.ToString(), MapInfoNo.ToString(), CellNo.ToString(), ID.ToString(), LocalShortID.ToString(), LocalID.ToString(), //ここまで5
                            shipid[0].ToString(), shipname[0], shipid[1].ToString(), shipname[1], shipid[2].ToString(), shipname[2],//11
                            shipid[3].ToString(), shipname[3], shipid[4].ToString(), shipname[4], shipid[5].ToString(), shipname[5],//17
                            enemyFormation, enemyAirSup.ToString(), air1.ToString(), air2.ToString(),//21
                        };

        }
    }
}
