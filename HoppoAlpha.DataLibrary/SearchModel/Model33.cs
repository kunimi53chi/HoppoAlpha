using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;

namespace HoppoAlpha.DataLibrary.SearchModel
{
    /// <summary>
    /// 33式の索敵モデルのクラス
    /// </summary>
    public static class Model33
    {
        /// <summary>
        /// 個別索敵値の計算（33式）
        /// </summary>
        /// <param name="oship">船のオブジェクト</param>
        /// <param name="itemdata">装備データ</param>
        /// <returns>Σ[係数×装備ステ]＋√ステ索敵</returns>
        public static UnitSearchResult CalcUnitSearch(ApiShip oship, Dictionary<int, SlotItem> itemdata)
        {
            var oslots = oship.GetOSlotitems(itemdata);
            var dslots = oship.GetDSlotitems(itemdata);

            int status_saku = oship.api_sakuteki[0];//装備なしの索敵
            double equip_saku = 0.0;//装備索敵ポイント
            foreach(var i in Enumerable.Range(0, Math.Min(oslots.Count, dslots.Count)))
            {
                var o = oslots[i];
                var d = dslots[i];

                if (d.api_saku > 0)
                {
                    //係数
                    double ratio;
                    switch (d.EquipType)
                    {
                        case 8://艦上攻撃機
                            ratio = 0.8;
                            break;
                        case 9://艦上偵察機
                            ratio = 1.0;
                            break;
                        case 10://水上偵察機
                            ratio = 1.2;
                            break;
                        case 11://水上爆撃機
                            ratio = 1.1;
                            break;
                        default:
                            ratio = 0.6;
                            break;
                    }

                    //改修効果
                    double kaishu;
                    switch(d.EquipType)
                    {
                        case 10://水上偵察機
                            kaishu = 1.2 * Math.Sqrt(o.api_level);
                            break;
                        case 12://小型電探
                        case 13://大型電探
                            kaishu = 1.25 * Math.Sqrt(o.api_level);
                            break;
                        default:
                            kaishu = 0.0;
                            break;
                    }

                    //装備索敵の合計をプラスし、素索敵から引く
                    equip_saku += ratio * ((double)d.api_saku + kaishu);
                    status_saku -= d.api_saku;
                }
            }

            //Σ[係数×装備ステ]＋√ステ索敵 : 返り値
            double unitSearch = equip_saku + Math.Sqrt(status_saku);

            UnitSearchResult result = new UnitSearchResult()
            {
                UnitSearchDouble = unitSearch,
            };
            return result;
        }

        /// <summary>
        /// 艦隊全体の索敵値（33式）
        /// </summary>
        /// <param name="unitSearchResult">個別索敵値</param>
        /// <param name="hqLevel">司令部レベル</param>
        /// <returns>艦隊全体の索敵値</returns>
        public static double CalcFleetSearch(IEnumerable<UnitSearchResult> unitSearchResult, int hqLevel)
        {
            int m = 6;
            double phi = 0.0;//判定値
            foreach(var u in unitSearchResult)
            {
                m--;
                phi += u.UnitSearchDouble;
            }

            //Σ[個別] - Ceil[0.4*司令部] + 2M
            return phi - Math.Ceiling(0.4 * (double)hqLevel) + 2 * m;
        }
    }
}
