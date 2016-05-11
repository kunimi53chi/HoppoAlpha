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
    /// 旧2-5式のモデルを表すクラス
    /// </summary>
    public static class Old25Model
    {
        /// <summary>
        /// 個別索敵値の計算（旧2-5式）
        /// </summary>
        /// <param name="oship">計算対象の船のオブジェクト</param>
        /// <param name="itemdata">装備のデータ</param>
        /// <returns>個別索敵値</returns>
        public static UnitSearchResult CalcUnitSearchOld25(ApiShip oship, Dictionary<int, SlotItem> itemdata)
        {
            //返り値：[0] 索敵装備値、[1]装備を引いたステ索敵値
            int status_search = oship.api_sakuteki[0];
            int equip_search = 0;
            var oslots = oship.GetOSlotitems(itemdata);
            var dslots = oship.GetDSlotitems(itemdata);

            foreach (int i in Enumerable.Range(0, dslots.Count))
            {
                SlotItem oequip = oslots[i];
                var equip = dslots[i];
                //索敵に関係する装備：艦戦(6), 艦爆(7), 艦攻(8), 艦偵{9}, 水上機{10} api_type[3]
                if (equip.api_type[3] >= 6 && equip.api_type[3] <= 10)
                {
                    equip_search += (equip.api_saku * 2);
                    status_search -= equip.api_saku;
                }
                //電探{11} 探照灯[24]
                else if (equip.api_type[3] == 11 || equip.api_type[3] == 24)
                {
                    equip_search += equip.api_saku;
                    status_search -= equip.api_saku;
                }
            }

            UnitSearchResult result = new UnitSearchResult
            {
                EquipSearchesPoint = equip_search,
                StatusSearches = status_search,
            };
            return result;
        }

        /// <summary>
        /// 艦隊索敵値の計算（旧2-5式）
        /// </summary>
        /// <param name="unitResults">個別索敵値</param>
        /// <param name="hqLevel">司令部レベル（このパラメーターは使いません）</param>
        /// <returns>艦隊索敵値</returns>
        public static int CalcFleetSearchOld25(IEnumerable<UnitSearchResult> unitResults, int hqLevel)
        {
            int point = 0, status = 0;
            foreach(var u in unitResults)
            {
                point += u.EquipSearchesPoint;
                status += u.StatusSearches;
            }

            return point + (int)Math.Sqrt(status);
        }
    }
}
