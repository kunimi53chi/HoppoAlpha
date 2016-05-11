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
    /// ほっぽアルファVer2.0.1索敵モデルのクラス
    /// </summary>
    public static class SearchHoppo201
    {
        /// <summary>
        /// 個別索敵値の計算（ほっぽアルファVer2.0.1索敵モデル）
        /// </summary>
        /// <param name="oship">船のオブジェクト</param>
        /// <param name="slotdata">装備データ</param>
        /// <returns>個別索敵値</returns>
        public static UnitSearchResult CalcUnitSearch(ApiShip oship, Dictionary<int, SlotItem> slotdata)
        {
            int status_search = oship.api_sakuteki[0];
            double pointe = 0;
            var oslots = oship.GetOSlotitems(slotdata);
            var dslots = oship.GetDSlotitems(slotdata);

            foreach (int i in Enumerable.Range(0, dslots.Count))
            {
                SlotItem oequip = oslots[i];
                var equip = dslots[i];
                //装備ポイントのある装備
                switch (equip.EquipType)
                {
                    case -1://ステータス索敵値から引く処理
                        status_search -= equip.api_saku;
                        break;
                    case 7://艦爆
                        pointe += 0.6 * (double)equip.api_saku;
                        goto case -1;
                    case 8://艦攻
                        pointe += 0.8 * (double)equip.api_saku;
                        goto case -1;
                    case 9://艦偵
                        pointe += 1.0 * (double)equip.api_saku;
                        goto case -1;
                    case 10://水偵
                        pointe += 1.2 * (double)equip.api_saku;
                        goto case -1;
                    case 11://水爆
                        pointe += 1.0 * (double)equip.api_saku;
                        goto case -1;
                    case 12://電探
                        pointe += 0.6 * (double)equip.api_saku;
                        goto case -1;
                    case 13://電探
                        goto case 12;
                    default://それ以外の索敵のある装備
                        if (equip.api_saku > 0)
                        {
                            pointe += 0.5 * (double)equip.api_saku;
                            status_search -= equip.api_saku;
                        }
                        break;
                }
            }
            //個別ポイントの計算
            double unitpoint = Math.Sqrt(status_search) + pointe;

            UnitSearchResult result = new UnitSearchResult()
            {
                UnitSearchInt = (int)unitpoint,
            };
            return result;
        }

        /// <summary>
        /// 艦隊全体の索敵値（ほっぽアルファVer2.0.1モデル）
        /// </summary>
        /// <param name="unitSearchResult">個別索敵値</param>
        /// <param name="hqLevel">司令部レベル</param>
        /// <returns>艦隊全体の索敵値</returns>
        public static int CalcFleetSearch(IEnumerable<UnitSearchResult> unitSearchResult, int hqLevel)
        {
            return unitSearchResult.Select(x => x.UnitSearchInt).Sum() - (int)Math.Floor((double)hqLevel * 0.4);
        }
    }
}
