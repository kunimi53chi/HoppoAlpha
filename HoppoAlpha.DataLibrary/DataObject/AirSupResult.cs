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
        /// 制空値の熟練度ボーナスを計算します
        /// </summary>
        /// <param name="dslot">装備のマスターデータ</param>
        /// <param name="alevel">熟練度レベル</param>
        /// <param name="isMax">内部熟練度で上限の値を取るか。falseなら下限の値</param>
        /// <returns>熟練度による制空値のボーナス</returns>
        public static double AircraftTrainingBonus(ExMasterSlotitem dslot, int alevel, bool isMax)
        {
            /*
            //暫定的に熟練度MAXのみ加算
            switch (dslot.EquipType)
            {
                case 6://艦戦
                    if (alevel == 7) return 25;
                    else return 0;
                case 7://艦爆
                    if (alevel == 7) return 3;
                    else return 0;
                case 8://艦攻
                    goto case 7;
                case 11://水爆
                    if (alevel == 7) return 9;
                    else return 0;
                default:
                    return 0;
            }*/
            //--装備種類ごとの制空ボーナス
            double seikuEquipBonus = 0.0;
            //艦戦の場合
            if(dslot.EquipType == 6)
            {
                if (alevel >= 0 && alevel <= 7) seikuEquipBonus = (double)kansen_seiku_bonus[alevel];
            }
            //水爆の場合
            else if(dslot.EquipType == 11)
            {
                if (alevel >= 0 && alevel <= 7) seikuEquipBonus = (double)suibaku_seiku_bonus[alevel];
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
    }
}
