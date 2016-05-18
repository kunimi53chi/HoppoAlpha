using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class KancolleInfoAirBaseCorps
    {
        #region 基地航空隊ステータス
        delegate void SetAirBaseStatusCallBack(UserControls.AirBaseCorpsLabelHandler handler);
        //基地航空隊のステータスの更新
        public static void SetAirBaseStatus(UserControls.AirBaseCorps uAirBaseCorps)
        {
            if(!uAirBaseCorps.IsHandleCreated)
            {
                var form = uAirBaseCorps.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if(uAirBaseCorps.InvokeRequired)
            {
                var d = new SetAirBaseStatusCallBack(SetAirBaseStatusInvokerLogic);
                uAirBaseCorps.Invoke(d, new object[] { uAirBaseCorps.Handler });
            }
            else
            {
                SetAirBaseStatusInvokerLogic(uAirBaseCorps.Handler);
            }
        }

        //基地航空隊のステータスの更新（非スレッドセーフ）
        private static void SetAirBaseStatusInvokerLogic(UserControls.AirBaseCorpsLabelHandler handler)
        {
            if (handler.AirBases == null) return;
            if (APIGetMember.BaseAirCorps == null) return;

            var n = Math.Min(handler.AirBases.Length, APIGetMember.BaseAirCorps.Count);
            //航空隊別の表示
            foreach(var i in Enumerable.Range(0, n))
            {
                var base_clasobj = APIGetMember.BaseAirCorps[i];
                var base_view = handler.AirBases[i];

                if(base_clasobj.api_plane_info == null) return;

                var base_viewobj = base_clasobj.GetViewStatus(APIGetMember.SlotItemsDictionary);

                //中隊別の表示
                foreach(var j in Enumerable.Range(0, Math.Min(base_clasobj.api_plane_info.Count, base_view.Squadrons.Length)))
                {
                    var sq_clsobj = base_clasobj.api_plane_info[j];
                    var sq_view = base_view.Squadrons[j];

                    var viewobj = base_viewobj.SquadronView[j];

                    if (viewobj != null)
                    {
                        sq_view.PlaneName.Text = viewobj.PlaneName;
                        sq_view.PlaneNum.Text = sq_clsobj.api_count.ToString();
                        sq_view.Training.Text = viewobj.Training;
                        sq_view.Cost.Text = viewobj.Cost.ToString();
                        sq_view.Radius.Text = viewobj.Radius.ToString();
                        sq_view.Dispatch.Text = viewobj.Dispatch.ToString();
                        sq_view.AirSupValue.Text = viewobj.AirSup.AirSupValueMin + "-" + viewobj.AirSup.AirSupValueMax;
                    }
                    else
                    {
                        sq_view.PlaneName.Text = "";
                        sq_view.PlaneNum.Text = "";
                        sq_view.Training.Text = "";
                        sq_view.Cost.Text = "";
                        sq_view.Radius.Text = "";
                        sq_view.Dispatch.Text = "";
                        sq_view.AirSupValue.Text = "";
                    }
                }

                //航空隊の合計値
                base_view.TotalNum.Text = base_viewobj.TotalNum.ToString();
                base_view.TotalCost.Text = base_viewobj.TotalCost.ToString();
                base_view.TotalRadius.Text = base_viewobj.TotalRedius.ToString();
                base_view.TotalDispatch.Text = base_viewobj.TotalDispatch.ToString();
                base_view.TotalAirSup.Text = base_viewobj.TotalAirSup.AirSupValueMin + "-" + base_viewobj.TotalAirSup.AirSupValueMax;
            }
            //ない航空隊はリセット
            for(int i=n; i<BaseAirCorp.NumOfAirBase; i++)
            {
                ResetAirBaseStatus(handler, i);
            }
        }

        //リセット
        private static void ResetAirBaseStatus(UserControls.AirBaseCorpsLabelHandler handler, int indexAirBase)
        {
            var b = handler.AirBases[indexAirBase];

            foreach(var s in b.Squadrons)
            {
                s.PlaneName.Text = "";
                s.PlaneNum.Text = "";
                s.Training.Text = "";
                s.Cost.Text = "";
                s.Radius.Text = "";
                s.Dispatch.Text = "";
                s.AirSupValue.Text = "";
            }

            b.TotalNum.Text = "";
            b.TotalCost.Text = "";
            b.TotalRadius.Text = "";
            b.TotalDispatch.Text = "";
            b.TotalAirSup.Text = "";
        }
        #endregion

        #region 基地航空戦
        delegate void SetAirBaseBattleCallBack(UserControls.AirBaseCorpsLabelHandler handler, System.Windows.Forms.ToolTip tooltip);
        //基地航空隊の戦闘の更新
        public static void SetAirBaseBattle(UserControls.AirBaseCorps uAirBaseCorps)
        {
            if (!uAirBaseCorps.IsHandleCreated)
            {
                var form = uAirBaseCorps.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if(uAirBaseCorps.InvokeRequired)
            {
                var d = new SetAirBaseBattleCallBack(SetAirBaseBattleInvokerLogic);
                uAirBaseCorps.Invoke(d, new object[] { uAirBaseCorps.Handler, uAirBaseCorps.ToolTip });
            }
            else
            {
                SetAirBaseBattleInvokerLogic(uAirBaseCorps.Handler, uAirBaseCorps.ToolTip);
            }

        }


        //基地航空隊の戦闘の更新（非スレッドセーフ）
        private static void SetAirBaseBattleInvokerLogic(UserControls.AirBaseCorpsLabelHandler handler, System.Windows.Forms.ToolTip tooltip)
        {
            if (handler.AirCombats == null) return;
            if (APIBattle.BattleQueue == null || APIBattle.BattleView == null) return;
            if (APIGetMember.BaseAirCorps == null) return;

            if(!Config.ShowBattleInfo)
            {
                foreach(var i in Enumerable.Range(0, handler.AirCombats.Length))
                    ResetAirBaseBattle(handler, tooltip, i);
                return;
            }
            var view = APIBattle.BattleView;
            if(view.Situation == BattleSituation.EndBattle || view.Situation == BattleSituation.EndCombinedBattle)
            {
                return;
            }

            if(APIBattle.BattleQueue.Count == 0)
            {
                foreach(var i in Enumerable.Range(0, handler.AirCombats.Length))
                    if (!Config.BattleDetailViewKeeping) ResetAirBaseBattle(handler, tooltip, i);
                return;
            }


            var info = APIBattle.BattleQueue.Peek();
            if (info.api_air_base_attack == null) return;

            bool[] isSecond = new bool[BaseAirCorp.NumOfSquadron];

            foreach(var attack in info.api_air_base_attack)
            {
                if (handler.AirCombats.Length < attack.api_base_id) continue;

                var index = attack.api_base_id - 1;

                //1回目か2回目のターゲット判定
                var target = isSecond[index] ? handler.AirCombats[index].Second : handler.AirCombats[index].First;
                isSecond[index] = true;

                if(attack.api_stage1 != null)
                {
                    target.Stage1Player.Text = attack.api_stage1.api_f_lostcount + "/" + attack.api_stage1.api_f_count;
                    target.Stage1Enemy.Text = attack.api_stage1.api_e_lostcount + "/" + attack.api_stage1.api_e_count;

                    //触接
                    if(attack.api_stage1.api_touch_plane != null && attack.api_stage1.api_touch_plane.Count >= 2)
                    {
                        ExMasterSlotitem playerAttach;
                        if(APIMaster.MstSlotitems.TryGetValue(attack.api_stage1.api_touch_plane[0], out playerAttach))
                        {
                            target.AttachPlayer.Text = "+" + playerAttach.api_houm;
                            tooltip.SetToolTip(target.AttachPlayer, playerAttach.api_name);
                        }
                        else
                        {
                            target.AttachPlayer.Text = "×";
                            tooltip.SetToolTip(target.AttachPlayer, null);
                        }

                        ExMasterSlotitem enemyAttach;
                        if(APIMaster.MstSlotitems.TryGetValue(attack.api_stage1.api_touch_plane[1], out enemyAttach))
                        {
                            target.AttachEnemy.Text = "+" + enemyAttach.api_houm;
                            tooltip.SetToolTip(target.AttachEnemy, enemyAttach.api_name);
                        }
                        else
                        {
                            target.AttachEnemy.Text = "×";
                            tooltip.SetToolTip(target.AttachEnemy, null);
                        }
                    }
                    else
                    {
                        target.AttachPlayer.Text = "×";
                        tooltip.SetToolTip(target.AttachPlayer, null);
                    }

                    //制空権
                    target.AirSupCondition.Text = Helper.BattleAirConditionToString(attack.api_stage1.api_disp_seiku);
                }

                if(attack.api_stage2 != null)
                {
                    target.Stage2Player.Text = attack.api_stage2.api_f_lostcount + "/" + attack.api_stage2.api_f_count;
                    target.Stage2Enemy.Text = attack.api_stage2.api_e_lostcount + "/" + attack.api_stage2.api_e_count;
                }
            }
        }

        //リセット
        private static void ResetAirBaseBattle(UserControls.AirBaseCorpsLabelHandler handler, System.Windows.Forms.ToolTip tooltip, int index)
        {
            var reset = new Action<UserControls.AirBaseCorpsLabelHandler.AirCombatItem>(item =>
            {
                item.Stage1Enemy.Text = "";
                item.Stage1Player.Text = "";
                item.Stage2Enemy.Text = "";
                item.Stage2Player.Text = "";

                item.AttachPlayer.Text = "";
                tooltip.SetToolTip(item.AttachPlayer, null);
                item.AttachEnemy.Text = "";
                tooltip.SetToolTip(item.AttachEnemy, null);

                item.AirSupCondition.Text = "";
            });

            var b = handler.AirCombats[index];
            reset(b.First);
            reset(b.Second);
        }
        #endregion
    }
}
