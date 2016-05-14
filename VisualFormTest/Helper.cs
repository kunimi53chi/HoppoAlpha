using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class Helper
    {
        //チェックマーク
        public static readonly string CheckString = Convert.ToChar(Convert.ToInt32("2713", 16)).ToString();

        //階級の定数
        public static string RankToString(int rank)
        {
            switch(rank)
            {
                case 1: return "元帥";
                case 2: return "大将";
                case 3: return "中将";
                case 4: return "少将";
                case 5: return "大佐";
                case 6: return "中佐";
                case 7: return "新米中佐";
                case 8: return "少佐";
                case 9: return "中堅少佐";
                case 10: return "新米少佐";
                default: return rank.ToString();
            }
        }

        //装備の種別の略称
        public static string MstSlotitemEquiptypeToString(int slot_eqtype_id)
        {
            switch(slot_eqtype_id)
            {
                case 1: return "主砲";
                case 2: return "主砲";
                case 3: return "主砲";
                case 4: return "副砲";
                case 5: return "魚雷";
                case 6: return "艦戦";
                case 7: return "艦爆";
                case 8: return "艦攻";
                case 9: return "艦偵";
                case 10: return "水偵";
                case 11: return "水爆";
                case 12: return "電探";
                case 13: return "電探";
                case 14: return "ソナ";
                case 15: return "爆雷";
                case 16: return "バル";
                case 17: return "機関";
                case 18: return "三式";
                case 19: return "徹甲";
                case 20: return "VT";
                case 21: return "機銃";
                case 22: return "特潜";
                case 23: return "応急";
                case 24: return "大発";
                case 25: return "ジャ";
                case 26: return "潜哨";
                case 27: return "バル";
                case 28: return "バル";
                case 29: return "探照";
                case 30: return "ドラ";
                case 31: return "修理";
                case 32: return "潜雷";
                case 33: return "照明";
                case 34: return "司令";
                case 35: return "航空";
                case 36: return "高射";
                case 37: return "対地";
                case 38: return "主II";
                case 39: return "見張";
                case 40: return "大ソ";
                case 41: return "大艇";
                case 42: return "大灯";
                case 43: return "糧食";
                case 44: return "洋補";
                case 45: return "水戦";
                case 46: return "内火";
                case 47: return "陸攻";
                case 48: return "局戦";
                default: return slot_eqtype_id.ToString();
            }
        }

        //速力
        public static string MstShipSpeedToString(int soku)
        {
            if (soku < 10) return "低速";
            else return "高速";
        }

        //装備の射程
        public static string MstSlotitemLengthToString(int lengthnum)
        {
            switch (lengthnum)
            {
                case 0: return "なし";
                case 1: return "短";
                case 2: return "中";
                case 3: return "長";
                case 4: return "超長";
                case 5: return "超超長";
                default: return lengthnum.ToString();
            }
        }

        //装備の詳細
        public static string MstSlotitemDetailToString(ApiMstSlotitem dslot)
        {
            StringBuilder sb = new StringBuilder();
            //装備名
            sb.AppendLine(dslot.api_name);
            //ステータス
            if (dslot.api_taik > 0) sb.AppendLine(string.Format("耐久:{0}", dslot.api_taik));
            if (dslot.api_souk > 0) sb.AppendLine(string.Format("装甲:{0}", dslot.api_souk));
            if (dslot.api_houm > 0) sb.AppendLine(string.Format("命中:{0}", dslot.api_houm));
            if (dslot.api_houk > 0) sb.AppendLine(string.Format("回避:{0}", dslot.api_houk));
            if (dslot.api_soku > 0) sb.AppendLine("速力:高速");
            if (dslot.api_leng > 0) sb.AppendLine(string.Format("射程:{0}", MstSlotitemLengthToString(dslot.api_leng)));
            //
            if (dslot.api_houg > 0) sb.AppendLine(string.Format("火力:{0}", dslot.api_houg));
            if (dslot.api_raig > 0) sb.AppendLine(string.Format("雷撃:{0}", dslot.api_raig));
            if (dslot.api_tyku > 0) sb.AppendLine(string.Format("対空:{0}", dslot.api_tyku));
            if (dslot.api_baku > 0) sb.AppendLine(string.Format("爆装:{0}", dslot.api_baku));
            if (dslot.api_tais > 0) sb.AppendLine(string.Format("対潜:{0}", dslot.api_tais));
            if (dslot.api_saku > 0) sb.AppendLine(string.Format("索敵:{0}", dslot.api_saku));
            if (dslot.api_luck > 0) sb.AppendLine(string.Format("運:{0}", dslot.api_luck));
            //返り値
            return sb.ToString();
        }


        //陣形
        public static string BattleFormationToString(int formation)
        {
            switch(formation)
            {
                case 1: return "単縦";
                case 2: return "複縦";
                case 3: return "輪形";
                case 4: return "梯形";
                case 5: return "単横";
                case 11: return "第一";
                case 12: return "第二";
                case 13: return "第三";
                case 14: return "第四";
                default: return formation.ToString();
            }
        }

        //交戦
        public static string BattleEngagementToString(int engage)
        {
            switch(engage)
            {
                case 1: return "同航戦";
                case 2: return "反航戦";
                case 3: return "T有利";
                case 4: return "T不利";
                default: return engage.ToString();
            }
        }

        //索敵
        public static string BattleSearchToString(int search)
        {
            switch(search)
            {
                case 1: return "成功";
                case 2: return "成功";
                case 4: return "失敗";
                case 5: return "成功";
                case 6: return "失敗";
                default: return search.ToString();
            }
        }

        //制空権
        public static string BattleAirConditionToString(int aircond)
        {
            switch(aircond)
            {
                case 0: return "拮抗";
                case 1: return "確保";
                case 2: return "優勢";
                case 3: return "劣勢";
                case 4: return "喪失";
                default: return aircond.ToString();
            }
        }

        //ユニットのToolTip
        public static string MakeUnitToolTip(ApiShip friendShip)
        {
            if (friendShip == null) return null;

            StringBuilder sb = new StringBuilder();
            //Tips1行目
            double hprate = (double)friendShip.api_nowhp / (double)friendShip.api_maxhp;
            sb.AppendFormat("{0} Lv{1} cond:{2} HP:{3}", friendShip.ShipName, friendShip.api_lv, friendShip.api_cond, hprate.ToString("P0")).AppendLine();
            //Tips2行目
            TimeSpan ndocktime = TimeSpan.FromMilliseconds(friendShip.api_ndock_time);
            sb.AppendFormat("燃:{0}/{1} 弾:{2}/{3} 入:{4}",
                friendShip.api_fuel, friendShip.DShip.api_fuel_max, friendShip.api_bull, friendShip.DShip.api_bull_max, ndocktime.ToString(@"h\hmm\m"))
                .AppendLine();
            //Tips3行目
            sb.AppendFormat("昼火:{0} 夜火:{1} 対潜:{2}",
                friendShip.api_karyoku[0], friendShip.api_karyoku[0] + friendShip.api_raisou[0], friendShip.api_taisen[0])
                .AppendLine();
            //Tips3.5行目
            sb.AppendFormat("加重対空:{0}", friendShip.GetWeightedAntiAirValue(APIGetMember.SlotItemsDictionary)).AppendLine();
            //Tips4行目～8行目(装備)
            var friend_oslot = friendShip.GetOSlotitems(APIGetMember.SlotItemsDictionary);
            var friend_dslot = friendShip.GetDSlotitems(APIGetMember.SlotItemsDictionary);
            if (friend_dslot.Count == 0)
            {
                sb.AppendLine("装備なし");
            }
            else
            {
                for (int j = 0; j < friend_dslot.Count; j++)
                {
                    var oslotitem = friend_oslot[j];
                    var dslotitem = friend_dslot[j];
                    //Tips n行目
                    string itemlevel = oslotitem.api_alv == 0 ? "★" + oslotitem.api_level : "◆" + oslotitem.api_alv;
                    sb.AppendFormat("装備{0} : [{1}] {2}({3})", j + 1, friendShip.api_onslot[j], dslotitem.api_name, itemlevel).AppendLine();
                }
            }
            //Tips9行目（拡張スロット）
            if(friendShip.api_slot_ex != 0)
            {
                SlotItem exslotitem;
                if(APIGetMember.SlotItemsDictionary.TryGetValue(friendShip.api_slot_ex, out exslotitem))
                {
                    sb.AppendFormat("拡張:{0}", APIMaster.MstSlotitems[exslotitem.api_slotitem_id].api_name).AppendLine();
                }
                else
                {
                    sb.AppendLine("拡張:装備なし");
                }
            }

            //Tips10行目
            sb.AppendFormat("Exp/Next {0}/{1}", friendShip.api_exp[0].ToString("N0"), friendShip.api_exp[1].ToString("N0")).AppendLine();
            //Tips11行目
            sb.Append("改造まで　");
            if (friendShip.DShip.api_afterlv != 0)
            {
                sb.AppendFormat("あとLv{0}", friendShip.DShip.api_afterlv - friendShip.api_lv);
            }
            else
            {
                sb.AppendFormat("-");
            }
            sb.AppendLine();

            return sb.ToString();
        }

        public static string MakeUnitToolTip(ExMasterShip enemyShip, int nowhp,List<int> onslotitem)
        {
            StringBuilder sb = new StringBuilder();

            if (enemyShip.api_taik == null) return sb.ToString();

            //Tips1行目
            double hprate = (double)nowhp / (double)enemyShip.api_taik[0];
            string shipname = string.Format("{0}{1}", enemyShip.api_name, enemyShip.api_yomi != "-" ? enemyShip.api_yomi : "");
            if (!enemyShip.IsEnemyShip) shipname = enemyShip.api_name;//演習の場合

            sb.AppendFormat("{0} HP:{1}/{2}({3})", shipname, nowhp, enemyShip.api_taik[0], hprate.ToString("P0")).AppendLine();
            //Tips2行目→なし
            //dslotの配列
            var dslotlist = new List<ApiMstSlotitem>();
            for (int j = 0; j < onslotitem.Count; j++)
            {
                int eid = onslotitem[j];//APIMasterの番号に対応
                if (eid == -1 || !APIMaster.MstShips.ContainsKey(eid))
                {
                    dslotlist.Add(null);
                    continue;
                }
                ApiMstSlotitem dslotitem = APIMaster.MstSlotitems[eid];
                dslotlist.Add(dslotitem);
            }
            //スロット数の配列
            string[] slotnum;
            if (enemyShip.api_maxeq == null) slotnum = Enumerable.Repeat("?", 5).ToArray();
            else slotnum = enemyShip.api_maxeq.Select(x => x.ToString()).ToArray();

            if (enemyShip.api_houg == null || enemyShip.api_raig == null || enemyShip.api_souk == null) return sb.ToString();

            //装備込み火力・装備込み雷装の計算
            int fire = enemyShip.api_houg[0] + dslotlist.Select(x => x == null ? 0 : x.api_houg).Sum();
            int torpedo = enemyShip.api_raig[0] + dslotlist.Select(x => x == null ? 0 : x.api_raig).Sum();
            //Tips3行目
            sb.AppendFormat("昼火:{0} 夜火:{1} 装甲:{2} ",
                fire, fire + torpedo, enemyShip.api_souk[0]);
            if (enemyShip.api_luck != null) sb.AppendFormat("運:{0}", enemyShip.api_luck[0]);
            sb.AppendLine();
            //Tips3.5行目
            sb.AppendFormat("加重対空:{0}", KancolleInfoFleet.CalcEnemyWeightedAntiAir(enemyShip.api_id, onslotitem)).AppendLine();

            //Tips4行目～8行目(装備)
            for (int j = 0; j < Math.Min(dslotlist.Count, slotnum.Length); j++)
            {
                if(dslotlist[j] == null)
                {
                    //装備なしの場合
                    if (j == 0) sb.AppendLine("装備なし");
                    break;
                }
                //Tips n行目
                sb.AppendFormat("装備{0} : [{1}] {2}", j + 1, slotnum[j], dslotlist[j].api_name).AppendLine();
            }

            return sb.ToString();
        }

        //マップの難易度
        public static string MapRankToString(int api_selected_rank)
        {
            switch(api_selected_rank)
            {
                case 0: return "なし";
                case 1: return "丙";
                case 2: return "乙";
                case 3: return "甲";
                default: return "";
            }
        }
    }
}
