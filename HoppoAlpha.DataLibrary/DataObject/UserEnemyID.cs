using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// ローカルな編成IDを計算するためのクラス
    /// </summary>
    public class UserEnemyID
    {
        /// <summary>
        /// 海域番号（ex.5-4-1の5）
        /// </summary>
        public int api_maparea_id { get; set; }
        /// <summary>
        /// マップ番号（ex.5-4-1の4）
        /// </summary>
        public int api_mapinfo_no { get; set; }
        /// <summary>
        /// セル番号（ex.5-4-1の1）
        /// </summary>
        public int api_cell_id { get; set; }
        /// <summary>
        /// 敵編成の型。戦闘APIのapi_ship_ke
        /// </summary>
        public List<int> api_ship_ke { get; set; }
        /// <summary>
        /// 敵の陣形。api_formationから敵の部分だけ抽出
        /// </summary>
        public int api_formation_enemy { get; set; }

        /// <summary>
        /// ハッシュコードをフルで返します。シリアル化してから比較するため値比較されます。
        /// </summary>
        /// <returns>ハッシュコード（フル）</returns>
        public int MakeLongHashCode()
        {
            //JSONでシリアル化　手動実装(ｷﾘｯ)
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"api_maparea_id\":");
            sb.Append(api_maparea_id);
            sb.Append(",\"api_mapinfo_no\":");
            sb.Append(api_mapinfo_no);
            sb.Append(",\"api_cell_id\":");
            sb.Append(api_cell_id);
            sb.Append(",\"api_ship_ke\":[");
            sb.Append(string.Join(",", api_ship_ke));
            sb.AppendFormat("],\"api_formation_enemy\":");
            sb.Append(api_formation_enemy);
            sb.Append("}");

            string json = sb.ToString();
            //UTF8でバイナリに変換
            byte[] binary = Encoding.UTF8.GetBytes(json);
            //MD5の計算
            MD5 cropto = new MD5CryptoServiceProvider();
            byte[] hashByte = cropto.ComputeHash(binary);
            //MD5を数値に変換
            BigInteger hashValue = new BigInteger(hashByte);

            //MD5をintに収まるようにMODを取る
            return (int)(hashValue % int.MaxValue);
        }

        /// <summary>
        /// 海域番号+マップ番号+ハッシュコードの数値を返します。シリアル化してから比較するため値比較されます。
        /// </summary>
        /// <param name="longhash">ハッシュコード（フル）</param>
        /// <returns>ハッシュコード（短縮版）</returns>
        public int MakeShortHashCode(int longhash)
        {
            int shorthash = Math.Abs(longhash % 1000);//LongHashの下3桁（マイナスなら絶対値で反転）
            shorthash += api_mapinfo_no * 1000;
            shorthash += api_maparea_id * 10000;

            return shorthash;
        }
    }
}
