using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 制空値の計算結果を表します
    /// </summary>
    public class AirSupResult
    {
        /// <summary>
        /// 制空値（上限）
        /// </summary>
        public int AirSupValueMax { get; set; }
        /// <summary>
        /// 制空値（下限）
        /// </summary>
        public int AirSupValueMin { get; set; }
        /// <summary>
        /// 制空値が正確かどうか
        /// </summary>
        public bool IsCorrect { get; set; }
        /// <summary>
        /// 艦載機本来の値
        /// </summary>
        public int OriginalValue { get; set; }
        /// <summary>
        /// 熟練度由来の値（上限）
        /// </summary>
        public int TrainingValueMax { get; set; }
        /// <summary>
        /// 熟練度由来の値（下限）
        /// </summary>
        public int TrainigValueMin { get; set; }

        //内部熟練度の下限
        static int[] naibu_seiku_min = new int[] { 0, 10, 25, 40, 55, 70, 85, 100 };
        //内部熟練度の上限
        static int[] naibu_seiku_max = new int[] { 9, 24, 39, 54, 69, 84, 99, 120 };
        //艦戦制空ボーナス
        static int[] kansen_seiku_bonus = new int[] { 0, 0, 2, 5, 9, 14, 14, 22 };
        //水爆制空ボーナス
        static int[] suibaku_seiku_bonus = new int[] { 0, 0, 1, 1, 1, 3, 3, 6 };
 
        /// <summary>
        /// 制空値をマージして別インスタンスで返します
        /// </summary>
        /// <param name="target">マージ対象の制空値</param>
        /// <returns>マージされた制空値</returns>
        public AirSupResult Merge(AirSupResult target)
        {
            var result = new AirSupResult()
            {
                AirSupValueMax = this.AirSupValueMax + target.AirSupValueMax,
                AirSupValueMin = this.AirSupValueMin + target.AirSupValueMin,
                IsCorrect = this.IsCorrect & target.IsCorrect,
                OriginalValue = this.OriginalValue + target.OriginalValue,
                TrainigValueMin = this.TrainigValueMin + target.TrainigValueMin,
                TrainingValueMax = this.TrainingValueMax + target.TrainingValueMax,
            };

            return result;
        }


        /// <summary>
        /// 制空値の熟練度ボーナスを計算します
        /// </summary>
        /// <param name="dslot">装備のマスターデータ</param>
        /// <param name="alevel">熟練度レベル</param>
        /// <param name="isMax">内部熟練度で上限の値を取るか。falseなら下限の値</param>
        /// <returns>熟練度による制空値のボーナス</returns>
        public static double AircraftTrainingBonus(ExMasterSlotitem dslot, int alevel, bool isMax)
        {
            //--装備種類ごとの制空ボーナス
            double seikuEquipBonus = 0.0;
            switch(dslot.EquipType)
            {
                //艦戦の場合（水戦、局地戦闘機を追加）
                case 6:
                case 45:
                case 48:
                    if (alevel >= 0 && alevel <= 7) seikuEquipBonus = (double)kansen_seiku_bonus[alevel];
                    break;
                //水爆の場合
                case 11:
                    if (alevel >= 0 && alevel <= 7) seikuEquipBonus = (double)suibaku_seiku_bonus[alevel];
                    break;
            }

            //--内部熟練度
            double trainval = 0.0;
            if(alevel >= 0 && alevel <= 7)
            {
                //最大値を取る場合
                if (isMax) trainval = (double)naibu_seiku_max[alevel];
                //最小値を取る場合
                else trainval = (double)naibu_seiku_min[alevel];
            }
            //--合計制空ボーナス
            return Math.Sqrt(trainval / 10.0) + seikuEquipBonus;
        }

        /// <summary>
        /// 1スロットあたりの制空値を計算します
        /// </summary>
        /// <param name="equip">装備マスターデータ</param>
        /// <param name="onSlotNum">スロット数</param>
        /// <param name="trainingLevel">熟練度レベル</param>
        /// <param name="reinforcedLevel">改修レベル</param>
        /// <returns>制空値</returns>
        public static AirSupResult SingleSlotitemAirSup(ExMasterSlotitem equip, double onSlotNum, int trainingLevel, int reinforcedLevel)
        {
            if (!equip.IsAirCombatable) return new AirSupResult();

            //1スロットあたりの艦載機由来の制空値
            //迎撃ステータスの制空値の追加
            double slotas = ((double)equip.api_tyku + (double)equip.Geigeki * 1.5) * Math.Sqrt(onSlotNum);
            //改修レベルの制空値
            double reinas = 0.0;
            if (equip.EquipType == 6) reinas = (double)reinforcedLevel * 0.2 * Math.Sqrt(onSlotNum);//艦戦のみ

            //熟練度ボーナス
            double trainmin = AircraftTrainingBonus(equip, trainingLevel, false);
            double trainmax = AircraftTrainingBonus(equip, trainingLevel, true);

            //このスロットの制空値
            int sum_min = (int)(slotas + reinas + trainmin);
            int sum_max = (int)(slotas + reinas + trainmax);

            //ボーナス部分をintに変換
            int trainmin_int = sum_min - (int)slotas;
            int trainmax_int = sum_max - (int)slotas; ;

            var result = new AirSupResult()
            {
                AirSupValueMin = sum_min,
                AirSupValueMax = sum_max,
                IsCorrect = true,
                TrainigValueMin = trainmin_int,
                TrainingValueMax = trainmax_int,
                OriginalValue = (int)slotas,
            };

            return result;
        }
    }
}
