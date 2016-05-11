using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    //カウンターの拡張メソッド
    public static class CounterItemExt
    {
        //条件を満たしたかのチェック
        public static int Check(this CounterItem item, BattleView view)
        {
            if (!item.Enabled) return 0;
            if (!item.AllMap)
            {
                if (item.AreaNo != view.AreaID) return 0;
                if (item.MapNo != view.MapID) return 0;
            }
            switch (item.Mode)
            {
                case CounterMode.None:
                    return 0;
                case CounterMode.BossWin:
                    bool iswin = view.WinRank == "S" || view.WinRank == "A" || view.WinRank == "B";
                    if (iswin && (view.BossFlag == 2)) return 1;
                    else return 0;
                case CounterMode.BossRankS:
                    if ((view.WinRank == "S") && (view.BossFlag == 2)) return 1;
                    else return 0;
                case CounterMode.Boss:
                    if (view.BossFlag == 2) return 1;
                    else return 0;
                case CounterMode.RankS:
                    if (view.WinRank == "S") return 1;
                    else return 0;
                case CounterMode.DestroyCarrier:
                    //空母リスト
                    var query = from p in APIMaster.MstShips
                                where p.Value.IsCarrier
                                select p.Value.api_id;
                    int cnt = 0;
                    for (int i = 0; i < view.EnemyShipID.Length; i++)
                    {
                        if (view.IsEnemySank == null) continue;//NULLチェック
                        if (!view.IsEnemySank[i]) continue;//沈んでない場合
                        if (query.Contains(view.EnemyShipID[i])) cnt++;//沈んでいてかつ空母の場合
                    }
                    return cnt;
                case CounterMode.DestroySupplyShip:
                    var querysupp = from p in APIMaster.MstShips
                                    where p.Value.IsSupplier
                                    select p.Value.api_id;
                    int cntsup = 0;
                    for (int i = 0; i < view.EnemyShipID.Length; i++)
                    {
                        if (view.IsEnemySank == null) continue;//NULLチェック
                        if (!view.IsEnemySank[i]) continue;//沈んでない場合
                        if (querysupp.Contains(view.EnemyShipID[i])) cntsup++;
                    }
                    return cntsup;
                case CounterMode.Lose:
                    if (view.WinRank == "C" || view.WinRank == "D" || view.WinRank == "E") return 1;
                    else return 0;
                case CounterMode.BossLose:
                    if (view.BossFlag != 2) return 0;
                    if (view.WinRank == "C" || view.WinRank == "D" || view.WinRank == "E") return 1;
                    else return 0;
                case CounterMode.BossOverA:
                    if (view.BossFlag != 2) return 0;
                    if (view.WinRank == "S" || view.WinRank == "A") return 1;
                    else return 0;
                default:
                    throw new ArgumentException();
            }
        }

        //チェックしてカウント
        public static void CheckAndCount(this CounterItem item, BattleView view)
        {
            if (item.AutoReset.IsAutoResetActivated(item.LastUpdated)) item.Reset();
            item.Value += Check(item, view);
            item.LastUpdated = DateTime.Now;
        }

        //このカウントのリセット
        public static void Reset(this CounterItem item)
        {
            item.Value = 0;
            item.Trial = 0;
        }

        //試行回数のチェック
        public static int TrialCheck(this CounterItem item, BattleView view)
        {
            //有効ではない場合
            if (!item.Enabled) return 0;
            //全マップモードの場合
            if (item.AllMap) return 1;
            //個別マップモードの場合
            else
            {
                if (item.AreaNo != view.AreaID) return 0;
                if (item.MapNo != view.MapID) return 0;
                return 1;
            }
        }

        public static void TrialCheckAndCount(this CounterItem item, BattleView view)
        {
            if (item.AutoReset.IsAutoResetActivated(item.LastUpdated)) item.Reset();
            item.Trial += TrialCheck(item, view);
            item.LastUpdated = DateTime.Now;
        }
    }
}
