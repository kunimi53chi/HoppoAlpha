using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary
{
    /// <summary>
    /// データの種類を表す列挙体
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 経験値レコードを表します
        /// </summary>
        Experience,
        /// <summary>
        /// カウンターのアイテムを表します
        /// </summary>
        Counter,
        /// <summary>
        /// クエリデータを表します
        /// </summary>
        Query,
        /// <summary>
        /// 任務データを表します
        /// </summary>
        Quest,
        /// <summary>
        /// 資源レコードを表します
        /// </summary>
        Material,
        /// <summary>
        /// ランキングレコードを表します
        /// </summary>
        Ranking,
        /// <summary>
        /// 戦果レコードを表します
        /// </summary>
        Senka,
        /// <summary>
        /// 初期装備のDBを表します
        /// </summary>
        DefaultSlotItem,
        /// <summary>
        /// ドロップレコードを表します
        /// </summary>
        DropRecord,
        /// <summary>
        /// 敵艦隊DBを表します
        /// </summary>
        EnemyFleet,
        /// <summary>
        /// 設定ファイルを表します
        /// </summary>
        Config,
        /// <summary>
        /// 船のマスターデータを表します
        /// </summary>
        ExMasterShip,
        /// <summary>
        /// 装備のマスターデータを表します
        /// </summary>
        ExMasterSlotitem,
        /// <summary>
        /// 出撃報告書を表します
        /// </summary>
        SortieReport,
    }
}
