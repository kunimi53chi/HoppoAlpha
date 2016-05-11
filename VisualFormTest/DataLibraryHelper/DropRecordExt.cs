using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    //ドロップレコードの拡張メソッド
    public static class DropRecordExt
    {
        public static string ToListViewString(this DropRecord record)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(record.MapAreaID).Append("-");
            sb.Append(record.MapInfoID).Append("-");
            sb.Append(record.BossFlag ? "★" : "").Append(record.MapCellID).Append("-");
            sb.Append(record.EnemyFleetLocalShortID);
            sb.Append(" ");
            sb.Append(record.WinRank).Append("勝利");
            sb.Append(" ");
            sb.Append("ドロップ:").Append(record.DropShipFlag ? DropDataBase.Collection.MasterDropShipHeader[record.DropShipID] : "なし");
            sb.Append("[").Append(record.DropItemAlreadyExists ? "既出" : "新規").Append("]");
            sb.Append(" ");
            sb.Append("司令部:Lv").Append(record.HQLevel);
            sb.Append(" ");
            sb.Append("難易度:").Append(record.MapDifficulty);
            sb.Append(" ");
            sb.Append("ドロップ:").Append(record.DropDisabled ? "無効" : "有効");
            sb.Append(" ");
            sb.Append("味方:").Append(string.Join(",", Enumerable.Range(0, Math.Min(record.FleetShipName.Length, record.FleetShipLevel.Length)).Select(x => string.Format("{0}(Lv{1})", record.FleetShipName[x], record.FleetShipLevel[x]))));
            sb.Append(" ");
            if (record.FleetCombinedShipName != null)
            {
                sb.Append("連合艦隊:");
                sb.Append(string.Join(",", Enumerable.Range(0, Math.Min(record.FleetCombinedShipName.Length, record.FleetCombinedShipLevel.Length))
                    .Select(x => string.Format("{0}(Lv{1})", record.FleetCombinedShipName[x], record.FleetCombinedShipLevel[x]))));
                sb.Append(" ");
            }
            sb.Append("[").Append(record.DropDate).Append("]");

            return sb.ToString();
        }
    }
}
