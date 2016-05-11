using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class EnemyFleetDataBase
    {
        //データ
        public static Dictionary<int, EnemyFleetRecord> DataBase { get; set; }

        //追加をブロックする変数
        public static bool AddingBlock { get; set; }
        //安全にするためのフラグ
        public static bool IsInited { get; set; }

        //ファイル名
        public static readonly string OutputFileName = "enemyfleet.dat";
        public static readonly string OutputDirectory = "config";
        public static string OutputFullPath
        {
            get
            {
                return OutputDirectory + @"\" + OutputFileName;
            }
        }

        //コンストラクタ
        static EnemyFleetDataBase()
        {
            if(!File.Exists(OutputFullPath))
            {
                DataBase = new Dictionary<int, EnemyFleetRecord>();
            }
            else
            {
                var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(OutputFullPath, HoppoAlpha.DataLibrary.DataType.EnemyFleet);
                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.EnemyFleet, loadResult, false);
                DataBase = (Dictionary<int, EnemyFleetRecord>)loadResult.Instance;
            }
            IsInited = true;
        }

        //保存
        public static void Save()
        {
            if (!IsInited) return;

            AddingBlock = true;
            if (!System.IO.Directory.Exists(OutputDirectory))
            {
                System.IO.Directory.CreateDirectory(OutputDirectory);
            }
            var saveResult = HoppoAlpha.DataLibrary.Files.Save(OutputFullPath, HoppoAlpha.DataLibrary.DataType.EnemyFleet, DataBase);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.EnemyFleet, saveResult, true);

            AddingBlock = false;
        }

        //追加
        public static void AddDataBase()
        {
            //ブロックする場合
            if (AddingBlock) return;
            //記録しない場合
            BattleView view = APIBattle.BattleView;
            if (view.Situation == BattleSituation.EndBattle || view.Situation == BattleSituation.EndCombinedBattle
                || view.Situation == BattleSituation.BeforeBattle)
            {
                return;
            }
            if (APIBattle.BattleQueue.Count == 0)
            {
                return;
            }
            //記録する場合
            BattleInfo info = APIBattle.BattleQueue.Peek();
            //マスターデータへの記録
            for(int i=1; i<info.api_ship_ke.Length; i++)
            {
                int shipke = info.api_ship_ke[i];
                if (!APIMaster.MstShips.ContainsKey(shipke)) continue;

                //マスターデータの書き換え
                //敵装備
                if(APIMaster.MstShips[shipke].DefaultSlotItem == null) APIMaster.MstShips[shipke].DefaultSlotItem = info.api_eSlot[i - 1];
                //敵HP
                if(APIMaster.MstShips[shipke].api_taik == null) APIMaster.MstShips[shipke].api_taik = Enumerable.Repeat(info.api_maxhps[i + 6], 2).ToList();

                List<int> eparam = info.api_eParam[i - 1];//敵パラメーター
                //敵火力
                if (APIMaster.MstShips[shipke].api_houg == null) APIMaster.MstShips[shipke].api_houg = Enumerable.Repeat(eparam[0], 2).ToList();
                //敵雷撃
                if (APIMaster.MstShips[shipke].api_raig == null) APIMaster.MstShips[shipke].api_raig = Enumerable.Repeat(eparam[1], 2).ToList();
                //敵対空
                if (APIMaster.MstShips[shipke].api_tyku == null) APIMaster.MstShips[shipke].api_tyku = Enumerable.Repeat(eparam[2], 2).ToList();
                //敵装甲
                if (APIMaster.MstShips[shipke].api_souk == null) APIMaster.MstShips[shipke].api_souk = Enumerable.Repeat(eparam[3], 2).ToList();
            }

            //DBアイテムの取得
            EnemyFleetRecord record = HelperDataLibrary.EnemyFleetRecordHelper.CreateInstance(info, view);
            //DBに追加(もう少し丁寧に)
            //旧いデータがある場合
            var olddataquery = DataBase.Values.Where(x => x.LocalID == record.LocalID);
            if(olddataquery.Count() > 0)
            {
                var olditem = olddataquery.First();
                //古いデータとハッシュが同じで、海域・マップが等しいなら（偶然ハッシュが一致する問題対策）
                if(olditem.MapAreaID == record.MapAreaID && olditem.MapInfoNo == record.MapInfoNo)
                {
                    return;
                }
            }
            //データがない場合
            DataBase[record.LocalID] = record;
        }
    }
}
