using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.Const;

namespace VisualFormTest
{
    public static class KancolleBattle
    {
        //色
        //public static System.Drawing.Color FormBackColor = System.Drawing.Color.FromArgb(214, 221, 240);
        public static System.Drawing.Color FormBackColor = System.Drawing.Color.White;
        public static System.Drawing.Color myWhite = System.Drawing.Color.FromArgb(209, 209, 209);
        public static System.Drawing.Color myBlack = System.Drawing.Color.FromArgb(51, 51, 51);


        //左上の戦闘ビューの更新
        delegate void SetBattleViewCallBack(UserControls.BattleState uBattleState);
        public static void SetBattleView(UserControls.BattleState uBattleState)
        {
            if(!uBattleState.IsHandleCreated)
            {
                var form = uBattleState.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if(uBattleState.InvokeRequired)
            {
                SetBattleViewCallBack d = new SetBattleViewCallBack(SetBattleView);
                uBattleState.Invoke(d, new object[] { uBattleState });
            }
            else
            {
                SetBattleViewUnsafe(uBattleState);
            }
        }

        //View更新のロジック
        private static void SetBattleViewUnsafe(UserControls.BattleState uBattleState)
        {
            /* labels
             * [0]:enemy_id [1]:編成 [2]:スコア [3]倍率
             */
            //ラベルの取得
            System.Windows.Forms.Label[] labels = uBattleState.Labels;
            //戦闘状態ではない場合
            if (APIBattle.BattleView.Situation == BattleSituation.None || !Config.ShowBattleInfo)
            {
                uBattleState.BackColor = BattleView.WinRankEnum.Unknown.GetBackColor();
                foreach (var x in labels) x.Text = "";
                return;
            }
            //enemy_id
            if (APIBattle.BattleView.Situation != BattleSituation.BeforeBattle)
            {
                labels[0].Text = string.Format("【{0}】と{1}",
                    APIBattle.BattleView.EnemyLocalShortID, APIBattle.BattleView.Situation.ToStr());
            }
            else
            {
                labels[0].Text = string.Format("{0}(cid:{1})", APIBattle.BattleView.Situation.ToStr(), APIBattle.BattleView.CellID);
            }
            //編成（これをマップとボスのフラグにする）
            if (APIBattle.BattleView.AreaID != 0)
            {
                string bossstr = "";
                if (APIBattle.BattleView.BossFlag == 2) bossstr = "ボス戦";
                else if (APIBattle.BattleView.BossFlag == 1) bossstr = "雑魚戦";
                labels[1].Text = string.Format("{0}-{1}　{2}　{3}戦目",
                    APIBattle.BattleView.AreaID, APIBattle.BattleView.MapID, bossstr, APIReqMap.BattleCount);
            }
            else
            {
                labels[1].Text = "";//未実装？
            }

            //(交戦見込みの場合)
            if (APIBattle.BattleView.Situation == BattleSituation.BeforeBattle)
            {
                uBattleState.BackColor = BattleView.WinRankEnum.Unknown.GetBackColor();
                for (int i = 2; i < labels.Length; i++) labels[i].Text = "";
                return;
            }
            //スコア
            labels[2].Text = string.Format("味方={0} 敵={1}{2}",
                (Math.Floor(APIBattle.BattleView.FscoreRatio * 100) / 100).ToString("P0"),
                (Math.Floor(APIBattle.BattleView.EscoreRatio * 100) / 100).ToString("P0"),
                (APIBattle.BattleView.IsCombined ? string.Format(",{0}", (Math.Floor(APIBattle.BattleView.EscoreRatioCombined * 100) / 100).ToString("P0")) : ""));
            //倍率
            labels[3].Text = string.Format("{0}{1}倍", APIBattle.BattleView.GaugeString,
                (APIBattle.BattleView.IsCombined ? string.Format(",{0}", APIBattle.BattleView.GaugeStringCombined) : ""));
            //MVP
            if (APIBattle.BattleQueue.Count == 0) return;
            BattleInfo info = APIBattle.BattleQueue.Peek();
            int mvpshipid = APIPort.DeckPorts[info.DeckPortNumber - 1].api_ship[info.GetMVPIndex()];
            if (APIPort.ShipsDictionary.ContainsKey(mvpshipid))
            {
                ApiShip oship = APIPort.ShipsDictionary[mvpshipid];
                bool isallcorrect = true;
                foreach (bool b in info.IsPredicted)
                {
                    if (b)
                    {
                        isallcorrect = false;
                        break;
                    }
                }
                labels[4].Text = string.Format("MVP{0} : {1}(Lv{2})", isallcorrect ? "" : "(?)", oship.ShipName, oship.api_lv);
            }
            //勝利判定
            uBattleState.BackColor = APIBattle.BattleView.WinRankEstimated.GetBackColor();
            labels[5].Text = APIBattle.BattleView.WinRankEstimated.ToStr();
        }



        //BattleView
        public static void SetBattleView_General(System.Windows.Forms.Label label, bool isVertical)
        {
            /*
             * 【537】と交戦見込み　/ 5-4ボス戦　3戦目
                2.38倍（味方38％ 敵12％）/ MVP(?) : 鳥海改(Lv35)*/
            //区切り記号
            string separator = isVertical ? Environment.NewLine : " / ";
            //戦闘状態ではない場合
            if (APIBattle.BattleView.Situation == BattleSituation.None || !Config.ShowBattleInfo)
            {
                CallBacks.SetLabelTextAndColorDouble(label, "", label.ForeColor, System.Drawing.Color.White);
                return;
            }
            StringBuilder sb = new StringBuilder();
            //enemy_id
            if (APIBattle.BattleView.Situation != BattleSituation.BeforeBattle)
            {
                sb.Append(string.Format("【{0}】と{1}",
                    APIBattle.BattleView.EnemyLocalShortID, APIBattle.BattleView.Situation.ToStr()));
            }
            else
            {
                sb.Append(APIBattle.BattleView.Situation.ToStr());
            }
            sb.Append(separator);
            //編成（これをマップとボスのフラグにする）
            if (APIBattle.BattleView.AreaID != 0)
            {
                string bossstr = "";
                if (APIBattle.BattleView.BossFlag == 2) bossstr = "ボス戦";
                else if (APIBattle.BattleView.BossFlag == 1) bossstr = "雑魚戦";
                sb.AppendLine(string.Format("{0}-{1}　{2}　{3}戦目 {4}",
                    APIBattle.BattleView.AreaID, APIBattle.BattleView.MapID, bossstr, APIReqMap.BattleCount, APIBattle.BattleView.WinRankEstimated.ToStr().Replace("?", "")));
            }
            else
            {
                sb.AppendLine();
            }

            //スコア・倍率
            if (APIBattle.BattleView.Situation != BattleSituation.BeforeBattle)
            {
                //倍率
                sb.AppendFormat("{0}{1}倍", APIBattle.BattleView.GaugeString, (APIBattle.BattleView.IsCombined ? string.Format(",{0}", APIBattle.BattleView.GaugeStringCombined) : ""));
                //スコア
                sb.AppendFormat("(味方={0} 敵={1}{2})",
                    (Math.Floor(APIBattle.BattleView.FscoreRatio * 100) / 100).ToString("P0"),
                    (Math.Floor(APIBattle.BattleView.EscoreRatio * 100) / 100).ToString("P0"),
                    (APIBattle.BattleView.IsCombined ? string.Format(",{0}", (Math.Floor(APIBattle.BattleView.EscoreRatioCombined * 100) / 100).ToString()) : ""));
            }
            //MVP
            if (APIBattle.BattleQueue.Count > 0)
            {
                BattleInfo info = APIBattle.BattleQueue.Peek();
                int mvpshipid = APIPort.DeckPorts[info.DeckPortNumber - 1].api_ship[info.GetMVPIndex()];
                if (APIPort.ShipsDictionary.ContainsKey(mvpshipid))
                {
                    ApiShip oship = APIPort.ShipsDictionary[mvpshipid];
                    bool isallcorrect = true;
                    foreach (bool b in info.IsPredicted)
                    {
                        if (b)
                        {
                            isallcorrect = false;
                            break;
                        }
                    }
                    sb.Append(separator);
                    sb.AppendFormat("MVP{0} : {1}(Lv{2})", isallcorrect ? "" : "(?)", oship.ShipName, oship.api_lv);
                }
            }
            //Labelに反映
            CallBacks.SetLabelTextAndColorDouble(label, sb.ToString(), label.ForeColor, APIBattle.BattleView.WinRankEstimated.GetBackColor());
        }

        public static WarnState GetWarnState()
        {
            //0 : ConditionGreen
            //1 : 非ロックの大破（黄色）
            //2 : ダメコン装備の大破（ピンク）
            //3 : 旗艦大破（赤）
            //4 : 非ロックでロック済装備艦の大破
            //5 : 主力艦隊の大破
            //警告状態
            int warn_state_int = 0;
            //連合艦隊用のループ
            int istart, iend;
            if(APIPort.CombinedFlag != 0 && APIReqMap.SallyDeckPort == 1)
            {
                //連合艦隊組んでいて、かつ第1艦隊で出撃時のみ
                istart = 0; iend = Math.Min(1, APIPort.DeckPorts.Count - 1);
            }
            else
            {
                istart = APIReqMap.SallyDeckPort - 1; iend = istart;
            }

            for (int i = istart; i <= iend; i++)
            {
                //艦隊情報
                ApiDeckPort deck = APIPort.DeckPorts[i];
                //船情報
                foreach (int x in deck.api_ship)
                {
                    if (x == -1) continue;
                    if (warn_state_int == (int)WarnState.LockedShipDamagedWarning) break;
                    //船のオブジェクト
                    ApiShip ship = APIPort.ShipsDictionary[x];
                    //退避しているか
                    int index = deck.api_ship.IndexOf(x);
                    bool iswithdraw = APIPort.IsWithdrawn[i, index];
                    if (iswithdraw) continue;//退避している場合は無視
                    //船ごとの大破警告の取得
                    var ship_warnstate = ship.GetWarnState(index == 0, iswithdraw, Config.BucketHPRatio, APIGetMember.SlotItemsDictionary);
                    //艦隊別の大破警告に統合
                    warn_state_int = Math.Max((int)ship_warnstate, warn_state_int);
                }
            }
            //返り値
            return (WarnState)warn_state_int;
        }

        //大破警告
        public static void SetSankWarning(System.Windows.Forms.TextBox textBox)
        {
            //警告のリセット
            if(APIReqMap.SallyDeckPort == 0)
            {
                CallBacks.SetTextBoxTextColorDoubleAndBorderStyle(textBox, "", 
                    FormBackColor, System.Drawing.Color.White, System.Windows.Forms.BorderStyle.None);
                return;
            }
            //警告状態
            WarnState warn_state = GetWarnState();
            //警告状態に応じて場合分け
            switch (warn_state)
            {
                case WarnState.ConditionGreen:
                    CallBacks.SetTextBoxTextColorDoubleAndBorderStyle(textBox, "Condition Green",
                        myBlack, System.Drawing.Color.Green, System.Windows.Forms.BorderStyle.None);
                    break;
                case WarnState.ShipUnlockedDamaged:
                    CallBacks.SetTextBoxTextColorDoubleAndBorderStyle(textBox, "非ロックの大破艦あり",
                        myBlack, System.Drawing.Color.Orange, System.Windows.Forms.BorderStyle.None);
                    break;
                case WarnState.HasDameconDamaged:
                    CallBacks.SetTextBoxTextColorDoubleAndBorderStyle(textBox, "ダメコン装備艦の大破",
                        System.Drawing.Color.White, System.Drawing.Color.Pink, System.Windows.Forms.BorderStyle.None);
                    break;
                case WarnState.FlagshipDamaged:
                    CallBacks.SetTextBoxTextColorDoubleAndBorderStyle(textBox, "旗艦大破",
                        System.Drawing.Color.Black, System.Drawing.Color.Red, System.Windows.Forms.BorderStyle.None);
                    break;
                case WarnState.ShipUnlockedAndEquipsLockedDamaged:
                    CallBacks.SetTextBoxTextColorDoubleAndBorderStyle(textBox, "装備ロック艦の大破",
                        System.Drawing.Color.Black, System.Drawing.Color.Red, System.Windows.Forms.BorderStyle.None);
                    break;
                case WarnState.LockedShipDamagedWarning:
                    CallBacks.SetTextBoxTextColorDoubleAndBorderStyle(textBox, "主力艦の大破警告",
                        System.Drawing.Color.White, System.Drawing.Color.Red, System.Windows.Forms.BorderStyle.None);
                    break;
            }
        }

        //戦闘詳細
        delegate void SetBattleDetailInvokerCallBack(UserControls.BattleDetailLabelHandler handler, System.Windows.Forms.ToolTip toolTip, bool isSquare);

        public static void SetBattleDetail(UserControls.BattleDetail battleDetail, System.Windows.Forms.ToolTip toolTip)
        {
            if(battleDetail.InvokeRequired)
            {
                SetBattleDetailInvokerCallBack cb = (handler, tooltip, issquare) => SetBattleDetailInvokerLogic(handler, tooltip, issquare);
                battleDetail.Invoke(cb, new object[] { battleDetail.LabelHandler, toolTip, false });
            }
            else
            {
                SetBattleDetailInvokerLogic(battleDetail.LabelHandler, toolTip, false);
            }
        }
        public static void SetBattleDetail(UserControls.BattleDetailSquare battleDetailSquare, System.Windows.Forms.ToolTip toolTip)
        {
            if (battleDetailSquare.InvokeRequired)
            {
                SetBattleDetailInvokerCallBack cb = (handler, tooltip, issquare) => SetBattleDetailInvokerLogic(handler, tooltip, issquare);
                battleDetailSquare.Invoke(cb, new object[] { battleDetailSquare.LabelHandler, toolTip, true });
            }
            else
            {
                SetBattleDetailInvokerLogic(battleDetailSquare.LabelHandler, toolTip, true);
            }
        }



        private static void SetBattleDetailInvokerLogic(UserControls.BattleDetailLabelHandler handler, System.Windows.Forms.ToolTip toolTip, bool isSquare)
        {
            if(!Config.ShowBattleInfo)
            {
                ResetBattleDetail(handler, toolTip, isSquare);
                return;
            }
            BattleView view = APIBattle.BattleView;
            if(view.Situation == BattleSituation.EndBattle || view.Situation == BattleSituation.EndCombinedBattle)
            {
                string endsituation = view.Situation.ToStr();
                if (!string.IsNullOrEmpty(endsituation)) handler.Overview.Header.Text = string.Format("概況 : {0}", endsituation);
                return;
            }
            else if(view.Situation == BattleSituation.BeforeBattle && !Config.BattleDetailViewKeeping)
            {
                //交戦見込みの場合の処理
                ResetBattleDetail(handler, toolTip, isSquare);
                handler.Overview.Header.Text = string.Format("概況 : {0}", view.Situation.ToStr());
                handler.Overview.MapCell.Text = string.Format("{0}-{1}-{2}", view.AreaID, view.MapID, view.CellID);
                handler.Overview.BattleCount.Text = APIReqMap.BattleCount.ToString();

                return;
            }
            if (APIBattle.BattleQueue.Count == 0)
            {
                if (!Config.BattleDetailViewKeeping) ResetBattleDetail(handler, toolTip, isSquare);
                return;
            }
            //バトルビュー
            BattleInfo info = APIBattle.BattleQueue.Peek();
            if (Config.BattleDetailViewKeeping) ResetBattleDetail(handler, toolTip, isSquare);//キープする場合でもここでリセット
            //概況
            string situation = view.Situation.ToStr();
            if (situation != "") handler.Overview.Header.Text = string.Format("概況 : {0}", situation);
            else handler.Overview.Header.Text = "概況";
            if (view.Situation == BattleSituation.Practice || view.Situation == BattleSituation.PracticeNightBattle)
            {
                handler.Overview.MapCell.Text = "";
                handler.Overview.BattleCount.Text = "";
            }
            else
            {
                handler.Overview.MapCell.Text = string.Format("{0}-{1}-{2}", view.AreaID, view.MapID, view.CellID);
                handler.Overview.BattleCount.Text = APIReqMap.BattleCount.ToString();
            }
            handler.Overview.ID.Text = view.EnemyLocalShortID.ToString();
            if (view.EnemyLocalShortID.ToString().Length > 8) toolTip.SetToolTip(handler.Overview.ID, view.EnemyLocalShortID.ToString());//IDが5桁以上の場合
            handler.Overview.GaugeRatio.Text = string.Format("{0}{1}倍", view.GaugeString, (view.IsCombined ? string.Format(", {0}", view.GaugeStringCombined) : ""));
            //handler.Overview.GaugeRatio.Text = string.Format("{0} 倍", view.GaugeString);
            handler.Overview.GaugeDetail.Text = string.Format(
                "({0} - {1}{2})",
                    (Math.Floor(view.FscoreRatio * 100) / 100).ToString("P0"),
                    (Math.Floor(view.EscoreRatio * 100) / 100).ToString("P0"),
                    (view.IsCombined ? string.Format(", {0}", (Math.Floor(view.EscoreRatioCombined * 100) / 100).ToString("P0")) : ""));
            /*handler.Overview.GaugeDetail.Text =
                string.Format("({0} - {1})", (Math.Floor(view.FscoreRatio * 100) / 100).ToString("P0"),
                (Math.Floor(view.EscoreRatio * 100) / 100).ToString("P0"));*/
            //会戦
            if(info.api_formation != null)
            {
                int[] formation = info.Formation;
                handler.Situation.FormationFriend.Text = Helper.BattleFormationToString(formation[0]);
                handler.Situation.FormationEnemy.Text = Helper.BattleFormationToString(formation[1]);
                handler.Situation.Engagement.Text = Helper.BattleEngagementToString(formation[2]);
            }
            else
            {
                handler.Situation.FormationFriend.Text = "";
                handler.Situation.FormationEnemy.Text = "";
                handler.Situation.Engagement.Text = "";
            }
            if(info.api_kouku != null && info.api_kouku.api_stage1 != null)
            {
                handler.Situation.AirSup.Text = Helper.BattleAirConditionToString(info.api_kouku.api_stage1.api_disp_seiku);
            }
            else
            {
                handler.Situation.AirSup.Text = "";
            }
            if(info.api_search != null)
            {
                handler.Situation.SearchFriend.Text = Helper.BattleSearchToString(info.api_search[0]);
                handler.Situation.SearchEnemy.Text = Helper.BattleSearchToString(info.api_search[1]);
            }
            else
            {
                handler.Situation.SearchFriend.Text = "";
                handler.Situation.SearchEnemy.Text = "";
            }
            if (info.api_kouku != null && info.api_kouku.api_stage1 != null && info.api_kouku.api_stage1.api_touch_plane != null && info.api_kouku.api_stage1.api_touch_plane.Count == 2)
            {
                ExMasterSlotitem friendTouch; ExMasterSlotitem enemyTouch;
                //味方触接
                if (APIMaster.MstSlotitems.TryGetValue(info.api_kouku.api_stage1.api_touch_plane[0], out friendTouch))
                {
                    handler.Situation.AttachFriend.Text = "+" + friendTouch.api_houm;
                    toolTip.SetToolTip(handler.Situation.AttachFriend, friendTouch.api_name);
                }
                else
                {
                    handler.Situation.AttachFriend.Text = "×"; toolTip.SetToolTip(handler.Situation.AttachFriend, null);
                }
                //敵触接
                if (APIMaster.MstSlotitems.TryGetValue(info.api_kouku.api_stage1.api_touch_plane[1], out enemyTouch))
                {
                    handler.Situation.AttachEnemy.Text = "+" + enemyTouch.api_houm;
                    toolTip.SetToolTip(handler.Situation.AttachEnemy, enemyTouch.api_name);
                }
                else
                {
                    handler.Situation.AttachEnemy.Text = "×"; toolTip.SetToolTip(handler.Situation.AttachEnemy, null);
                }
            }
            else
            {
                handler.Situation.AttachFriend.Text = "×"; toolTip.SetToolTip(handler.Situation.AttachFriend, null);
                handler.Situation.AttachEnemy.Text = "×"; toolTip.SetToolTip(handler.Situation.AttachEnemy, null);
            }
            //基地航空戦
            if (handler.AirBattle.AirBaseEnemy != null && handler.AirBattle.AirBaseFriend != null)
            {
                if (info.api_air_base_attack != null)
                {
                    int friend_total = 0, friend_lost = 0, enemy_total = 0, enemy_lost = 0;
                    foreach (var ab in info.api_air_base_attack)
                    {
                        if (ab.api_stage1 != null)
                        {
                            //味方の基地航空戦の総数は加算(Stage1で加算する)
                            friend_total += ab.api_stage1.api_f_count;
                            //敵、味方とも損失数は随時加算(Stage1, Stage2とも)
                            friend_lost += ab.api_stage1.api_f_lostcount;
                            enemy_lost += ab.api_stage1.api_e_lostcount;
                            //敵の総数は最大の値を取る
                            enemy_total = Math.Max(enemy_total, ab.api_stage1.api_e_count);
                        }
                        if (ab.api_stage2 != null)
                        {
                            //Stage2の総数は無視する
                            //敵、味方の損失数を加算する
                            friend_lost += ab.api_stage2.api_f_lostcount;
                            enemy_lost += ab.api_stage2.api_e_lostcount;
                        }
                    }
                    handler.AirBattle.AirBaseFriend.Text =
                        string.Format("{0}/{1}", friend_lost, friend_total);
                    handler.AirBattle.AirBaseEnemy.Text =
                        string.Format("{0}/{1}", enemy_lost, enemy_total);
                }
                else
                {
                    handler.AirBattle.AirBaseFriend.Text = "";
                    handler.AirBattle.AirBaseEnemy.Text = "";
                }
            }
            //航空戦
            if (info.api_kouku != null)
            {
                if (info.api_kouku.api_stage1 != null)
                {
                    handler.AirBattle.Stage1Friend.Text =
                        string.Format("{0}/{1}", info.api_kouku.api_stage1.api_f_lostcount, info.api_kouku.api_stage1.api_f_count);
                    handler.AirBattle.Stage1Enemy.Text =
                        string.Format("{0}/{1}", info.api_kouku.api_stage1.api_e_lostcount, info.api_kouku.api_stage1.api_e_count);
                    //制空値の計算
                    //味方制空値
                    int f_airsup_min = 0, f_airsup_max = 0;
                    foreach (int x in APIPort.DeckPorts[info.DeckPortNumber - 1].api_ship)
                    {
                        if (x == -1) continue;
                        ApiShip oship = APIPort.ShipsDictionary[x];
                        var airsupresult = oship.GetAirSupValue(APIGetMember.SlotItemsDictionary, APIPort.IsWithdrawn, APIPort.DeckPorts);
                        f_airsup_min += airsupresult.AirSupValueMin;
                        f_airsup_max += airsupresult.AirSupValueMax;
                    }
                    //敵制空値
                    int e_airsup = 0;
                    bool e_airsup_correct = true;
                    foreach (int i in Enumerable.Range(0, info.api_eSlot.Count))
                    {
                        if (info.api_ship_ke[i + 1] == -1) continue;
                        var airsupresult = KancolleInfoFleet.CalcEnemyAirSup(info.api_ship_ke[i + 1], info.api_eSlot[i]);
                        e_airsup += airsupresult.AirSupValueMax;
                        e_airsup_correct = e_airsup_correct && airsupresult.IsCorrect;
                    }
                    handler.AirBattle.AirSupValueFriend.Text = f_airsup_min + "-" + f_airsup_max;
                    handler.AirBattle.AirSupValueEnemy.Text = e_airsup.ToString() + (e_airsup_correct ? "" : "+");//不正確な場合も対応
                }
                else
                {
                    handler.AirBattle.Stage1Friend.Text = "";
                    handler.AirBattle.Stage1Enemy.Text = "";
                    handler.AirBattle.AirSupValueFriend.Text = "";
                    handler.AirBattle.AirSupValueEnemy.Text = "";
                }
                if (info.api_kouku.api_stage2 != null)
                {
                    handler.AirBattle.Stage2Friend.Text =
                        string.Format("{0}/{1}", info.api_kouku.api_stage2.api_f_lostcount, info.api_kouku.api_stage2.api_f_count);
                    handler.AirBattle.Stage2Enemy.Text =
                        string.Format("{0}/{1}", info.api_kouku.api_stage2.api_e_lostcount, info.api_kouku.api_stage2.api_e_count);
                }
                else
                {
                    handler.AirBattle.Stage2Friend.Text = "";
                    handler.AirBattle.Stage2Enemy.Text = "";
                }
            }
            //--通常戦闘
            //ダメージ
            //ダメージ（非タブ画面の場合）
            if(!isSquare)
            {
                string[,] damage_str = new string[6, 5];
                System.Drawing.Color[,] damage_fore = new System.Drawing.Color[6, 5];
                System.Drawing.Color[,] damage_back = new System.Drawing.Color[6, 5];
                for(int i=0; i<damage_fore.GetLength(0); i++)
                {
                    for(int j=0; j<damage_fore.GetLength(1); j++)
                    {
                        damage_fore[i, j] = KancolleInfo.DefaultStringColor;
                        damage_back[i, j] = System.Drawing.Color.Transparent;
                    }
                }
                //MVPのID
                int mvp = info.GetMVPIndex();
                //値の取得
                for(int i=0; i<damage_str.GetLength(0); i++)
                {
                    //味方の状態
                    HPCondition f_cond_before = HPCondition.None;
                    HPCondition f_cond_after = HPCondition.None;
                    if(info.api_maxhps[i+1] >= 0)
                    {
                        f_cond_before = ApiShip.GetHPCondtionCore(info.FStartHP[i], info.api_maxhps[i + 1], false, APIPort.IsWithdrawn[info.DeckPortNumber - 1, i], Config.BucketHPRatio);
                        f_cond_after = ApiShip.GetHPCondtionCore(info.api_nowhps[i+1], info.api_maxhps[i + 1], false, APIPort.IsWithdrawn[info.DeckPortNumber - 1, i], Config.BucketHPRatio);
                    }
                    //敵の状態
                    HPCondition e_cond_before = HPCondition.None;
                    HPCondition e_cond_after = HPCondition.None;
                    if(info.api_maxhps[i+7] >= 0)
                    {
                        e_cond_before = ApiShip.GetHPCondtionCore(info.EStartHP[i], info.api_maxhps[i + 7], false, false, 0.0);
                        e_cond_after = ApiShip.GetHPCondtionCore(info.api_nowhps[i + 7], info.api_maxhps[i + 7], false, false, 0.0);
                    }
                    //--パラメーター
                    //味方開戦前のHP
                    damage_str[i, 0] = info.FStartHP[i] > 0 ? info.FStartHP[i].ToString() : "";
                    damage_fore[i, 0] = f_cond_before.GetColorBattleDetail();
                    damage_back[i, 0] = f_cond_before.GetBackColorBattleDetail();
                    //味方の与えたダメージ
                    string[] fydam = info.GetFYdamDisplay();
                    damage_str[i, 1] = info.FStartHP[i] > 0 ? fydam[i] : "";
                    if (i == mvp) damage_back[i, 1] = System.Drawing.Color.FromArgb(248, 224, 114);
                    //味方の残りのHP
                    damage_str[i, 2] = info.api_nowhps[i + 1] >= 0 ? info.api_nowhps[i + 1].ToString() : "";
                    damage_fore[i, 2] = f_cond_after.GetColorBattleDetail();
                    damage_back[i, 2] = f_cond_after.GetBackColorBattleDetail();
                    //敵の開戦前のHP
                    damage_str[i, 3] = info.EStartHP[i] > 0 ? info.EStartHP[i].ToString() : "";
                    damage_fore[i, 3] = e_cond_before.GetColorBattleDetail();
                    damage_back[i, 3] = e_cond_before.GetBackColorBattleDetail();
                    //敵の残りのHP
                    if (i + 7 < info.api_nowhps.Length)
                    {
                        damage_str[i, 4] = info.api_nowhps[i + 7] >= 0 ? info.api_nowhps[i + 7].ToString() : "";
                        damage_fore[i, 4] = e_cond_after.GetColorBattleDetail();
                        damage_back[i, 4] = e_cond_after.GetBackColorBattleDetail();
                    }
                    else
                    {
                        damage_str[i, 4] = "";
                        damage_fore[i, 4] = KancolleInfo.DefaultStringColor;
                        damage_back[i, 4] = KancolleInfo.DefaultBackColor;
                    }
                }
                //ラベルにセット
                foreach(int i in Enumerable.Range(0, damage_str.GetLength(0)))
                {
                    foreach(int j in Enumerable.Range(0, damage_str.GetLength(1)))
                    {
                        handler.Damage[i][j].Text = damage_str[i, j];
                        handler.Damage[i][j].ForeColor = damage_fore[i, j];
                        handler.Damage[i][j].BackColor = damage_back[i, j];
                    }
                }
            }
            //ダメージ（正方形の場合）
            else
            {
                string[,] damage_str = new string[6, 7];
                System.Drawing.Color[,] damage_fore = new System.Drawing.Color[6, 7];
                System.Drawing.Color[,] damage_back = new System.Drawing.Color[6, 7];
                for (int i = 0; i < damage_fore.GetLength(0); i++)
                {
                    for (int j = 0; j < damage_fore.GetLength(1); j++)
                    {
                        damage_fore[i, j] = KancolleInfo.DefaultStringColor;
                        damage_back[i, j] = System.Drawing.Color.Transparent;
                    }
                }
                //MVPのID
                int mvp = info.GetMVPIndex();
                //味方の艦名
                var oshiparray = APIPort.DeckPorts[info.DeckPortNumber - 1].api_ship.Select(delegate(int shipid)
                    {
                        if (shipid < 0) return null;
                        if (!APIPort.ShipsDictionary.ContainsKey(shipid)) return null;
                        return APIPort.ShipsDictionary[shipid];
                    }).ToArray();
                //値の取得
                for (int i = 0; i < damage_str.GetLength(0); i++)
                {
                    //味方の状態
                    HPCondition f_cond_before = HPCondition.None;
                    HPCondition f_cond_after = HPCondition.None;
                    if (info.api_maxhps[i + 1] >= 0)
                    {
                        f_cond_before = ApiShip.GetHPCondtionCore(info.FStartHP[i], info.api_maxhps[i + 1], false, APIPort.IsWithdrawn[info.DeckPortNumber - 1, i], Config.BucketHPRatio);
                        f_cond_after = ApiShip.GetHPCondtionCore(info.api_nowhps[i + 1], info.api_maxhps[i + 1], false, APIPort.IsWithdrawn[info.DeckPortNumber - 1, i], Config.BucketHPRatio);
                    }
                    //敵の状態
                    HPCondition e_cond_before = HPCondition.None;
                    HPCondition e_cond_after = HPCondition.None;
                    if (info.api_maxhps[i + 7] >= 0)
                    {
                        e_cond_before = ApiShip.GetHPCondtionCore(info.EStartHP[i], info.api_maxhps[i + 7], false, false, 0.0);
                        e_cond_after = ApiShip.GetHPCondtionCore(info.api_nowhps[i + 7], info.api_maxhps[i + 7], false, false, 0.0);
                    }
                    //--パラメーター
                    //艦名
                    damage_str[i, 0] = oshiparray[i] != null ? 
                        string.Format("{0}(Lv{1}:{2})", oshiparray[i].ShipName, oshiparray[i].api_lv, oshiparray[i].api_cond) : "";
                    CallBacks.SetControlToolTip(toolTip, handler.Damage[i][0], Helper.MakeUnitToolTip(oshiparray[i]));
                    //味方開戦前のHP
                    damage_str[i, 1] = info.FStartHP[i] > 0 ? info.FStartHP[i].ToString() : "";
                    damage_fore[i, 1] = f_cond_before.GetColorBattleDetail();
                    damage_back[i, 1] = f_cond_before.GetBackColorBattleDetail();
                    //味方の与えたダメージ
                    string[] fydam = info.GetFYdamDisplay();
                    damage_str[i, 2] = info.FStartHP[i] > 0 ? fydam[i] : "";
                    if (i == mvp) damage_back[i, 2] = System.Drawing.Color.FromArgb(248, 224, 114);
                    //味方の残りのHP
                    damage_str[i, 3] = info.api_nowhps[i + 1] >= 0 ? info.api_nowhps[i + 1].ToString() : "";
                    damage_fore[i, 3] = f_cond_after.GetColorBattleDetail();
                    damage_back[i, 3] = f_cond_after.GetBackColorBattleDetail();
                    //味方の残りHPのパーセント
                    if(info.api_nowhps[i+1]>=0 && info.api_maxhps[i+1]>0)
                    {
                        damage_str[i, 4] = ((double)info.api_nowhps[i + 1] / (double)info.api_maxhps[i + 1]).ToString("P0");
                    }
                    else
                    {
                        damage_str[i, 4] = "";
                    }
                    damage_fore[i, 4] = KancolleInfo.DefaultStringColor;
                    damage_back[i, 4] = f_cond_after.GetBackColorBattleDetailPercent();
                    //敵の開戦前のHP
                    damage_str[i, 5] = info.EStartHP[i] > 0 ? info.EStartHP[i].ToString() : "";
                    damage_fore[i, 5] = e_cond_before.GetColorBattleDetail();
                    damage_back[i, 5] = e_cond_before.GetBackColorBattleDetail();
                    //敵の残りのHP
                    if (i + 7 < info.api_nowhps.Length)
                    {
                        damage_str[i, 6] = info.api_nowhps[i + 7] >= 0 ? info.api_nowhps[i + 7].ToString() : "";
                        damage_fore[i, 6] = e_cond_after.GetColorBattleDetail();
                        damage_back[i, 6] = e_cond_after.GetBackColorBattleDetail();
                    }
                    else
                    {
                        damage_str[i, 6] = "";
                        damage_fore[i, 6] = KancolleInfo.DefaultStringColor;
                        damage_fore[i, 6] = KancolleInfo.DefaultBackColor;
                    }
                }
                //ラベルにセット
                foreach (int i in Enumerable.Range(0, damage_str.GetLength(0)))
                {
                    foreach (int j in Enumerable.Range(0, damage_str.GetLength(1)))
                    {
                        handler.Damage[i][j].Text = damage_str[i, j];
                        handler.Damage[i][j].ForeColor = damage_fore[i, j];
                        handler.Damage[i][j].BackColor = damage_back[i, j];
                    }
                }
            }
            //敵編成
            handler.EnemyFleet.Header.Text = "敵編成";
            for(int i=1; i<info.api_ship_ke.Length; i++)
            {
                if (info.api_ship_ke[i] == -1)
                {
                    handler.EnemyFleet.Name[i - 1].Text = "";
                    if(isSquare) toolTip.SetToolTip(handler.EnemyFleet.Name[i - 1], null);
                    continue;
                }
                //船の名前
                ExMasterShip dship = APIMaster.MstShips[info.api_ship_ke[i]];
                if (view.Situation == BattleSituation.Practice || view.Situation == BattleSituation.PracticeNightBattle)
                {
                    string text = dship.api_name;
                    if(info.api_ship_lv != null)
                    {
                        text += string.Format(" Lv{0}", info.api_ship_lv[i]);
                    }
                    handler.EnemyFleet.Name[i - 1].Text = text;
                }
                else
                {
                    handler.EnemyFleet.Name[i - 1].Text = 
                        string.Format("{0} {1}", dship.api_name, dship.api_yomi.Replace("-", ""));
                }

                //船のToolTips
                if (isSquare) toolTip.SetToolTip(handler.EnemyFleet.Name[i - 1],
                     i - 1 < info.api_eSlot.Count ? Helper.MakeUnitToolTip(dship, info.EStartHP[i - 1], info.api_eSlot[i - 1]) : null);
            }
            //装備
            foreach(int i in Enumerable.Range(0, info.api_eSlot.Count))
            {
                List<int> eunit = info.api_eSlot[i];
                foreach(int j in Enumerable.Range(0, Math.Min(eunit.Count, 4)))
                {
                    if(eunit[j] == -1)
                    {
                        handler.EnemyFleet.Equips[i][j].Text = "";
                        continue;
                    }
                    //装備
                    ExMasterSlotitem dslotitem;
                    if(APIMaster.MstSlotitems.TryGetValue(eunit[j], out dslotitem))
                    {
                        int slotitemtype = dslotitem.EquipType;
                        handler.EnemyFleet.Equips[i][j].Text = Helper.MstSlotitemEquiptypeToString(slotitemtype);
                        //装備のToolTips
                        toolTip.SetToolTip(handler.EnemyFleet.Equips[i][j], Helper.MstSlotitemDetailToString(dslotitem));
                    }
                }
            }
            //--連合艦隊
            if (!(info is BattleCombinedInfo)) return;
            BattleCombinedInfo combined_info = info as BattleCombinedInfo;
            int combined_id = Math.Min(combined_info.DeckPortNumber, APIPort.DeckPorts.Count - 1);
            //連合艦隊（非タブ画面の場合）
            if(!isSquare)
            {
                string[,] combined_damage_str = new string[6, 3];
                var combined_damage_fore = new System.Drawing.Color[6, 3];
                var combined_damage_back = new System.Drawing.Color[6, 3];
                foreach(int i in Enumerable.Range(0, combined_damage_fore.GetLength(0)))
                {
                    foreach(int j in Enumerable.Range(0, combined_damage_fore.GetLength(1)))
                    {
                        combined_damage_fore[i, j] = KancolleInfo.DefaultStringColor;
                        combined_damage_back[i, j] = System.Drawing.Color.Transparent;
                    }
                }
                //MVPのID
                int mvp_combined = combined_info.GetMVPIndexCombined();
                //値の取得
                foreach(int i in Enumerable.Range(0, combined_damage_str.GetLength(0)))
                {
                    //味方の状態
                    var f_cond_before = HPCondition.None;
                    var f_cond_after = HPCondition.None;
                    if(combined_info.api_maxhps_combined[i+1] >= 0)
                    {
                        f_cond_before = ApiShip.GetHPCondtionCore(combined_info.FStartHPCombined[i], combined_info.api_maxhps_combined[i+1], 
                            false, APIPort.IsWithdrawn[combined_id, i], Config.BucketHPRatio);
                        f_cond_after = ApiShip.GetHPCondtionCore(combined_info.api_nowhps_combined[i+1], combined_info.api_maxhps_combined[i+1],
                            false, APIPort.IsWithdrawn[combined_id, i], Config.BucketHPRatio);
                    }
                    //--パラメーター
                    //開戦前のHP
                    combined_damage_str[i, 0] = combined_info.FStartHPCombined[i] > 0 ? combined_info.FStartHPCombined[i].ToString() : "";
                    combined_damage_fore[i, 0] = f_cond_before.GetColorBattleDetail();
                    combined_damage_back[i, 0] = f_cond_before.GetBackColorBattleDetail();
                    //与えたダメージ
                    combined_damage_str[i, 1] = combined_info.FStartHPCombined[i] > 0 ? combined_info.FScoreArrayCombined[i].ToString() : "";
                    if (i == mvp_combined) combined_damage_back[i, 1] = System.Drawing.Color.FromArgb(248, 224, 114);
                    //味方の残りHP
                    combined_damage_str[i, 2] = combined_info.api_nowhps_combined[i + 1] >= 0 ? combined_info.api_nowhps_combined[i + 1].ToString() : "";
                    combined_damage_fore[i, 2] = f_cond_after.GetColorBattleDetail();
                    combined_damage_back[i, 2] = f_cond_after.GetBackColorBattleDetail();
                }
                //ラベルにセット
                foreach(int i in Enumerable.Range(0, combined_damage_str.GetLength(0)))
                {
                    foreach(int j in Enumerable.Range(0, combined_damage_str.GetLength(1)))
                    {
                        handler.DamageCombined[i][j].Text = combined_damage_str[i, j];
                        handler.DamageCombined[i][j].ForeColor = combined_damage_fore[i, j];
                        handler.DamageCombined[i][j].BackColor = combined_damage_back[i, j];
                    }
                }
            }
            //連合艦隊（正方形の場合）
            else
            {
                string[,] combined_damage_str = new string[6, 5];
                var combined_damage_fore = new System.Drawing.Color[6, 5];
                var combined_damage_back = new System.Drawing.Color[6, 5];
                foreach (int i in Enumerable.Range(0, combined_damage_fore.GetLength(0)))
                {
                    foreach (int j in Enumerable.Range(0, combined_damage_fore.GetLength(1)))
                    {
                        combined_damage_fore[i, j] = KancolleInfo.DefaultStringColor;
                        combined_damage_back[i, j] = System.Drawing.Color.Transparent;
                    }
                }
                //MVPのID
                int mvp_combined = combined_info.GetMVPIndexCombined();
                //連合艦隊の艦名
                var oshiparray = APIPort.DeckPorts[combined_id].api_ship.Select(delegate(int shipid)
                {
                    if (shipid < 0) return null;
                    if (!APIPort.ShipsDictionary.ContainsKey(shipid)) return null;
                    return APIPort.ShipsDictionary[shipid];
                }).ToArray();
                //値の取得
                foreach (int i in Enumerable.Range(0, combined_damage_str.GetLength(0)))
                {
                    //味方の状態
                    var f_cond_before = HPCondition.None;
                    var f_cond_after = HPCondition.None;
                    if (combined_info.api_maxhps_combined[i + 1] >= 0)
                    {
                        f_cond_before = ApiShip.GetHPCondtionCore(combined_info.FStartHPCombined[i], combined_info.api_maxhps_combined[i + 1],
                            false, APIPort.IsWithdrawn[combined_id, i], Config.BucketHPRatio);
                        f_cond_after = ApiShip.GetHPCondtionCore(combined_info.api_nowhps_combined[i + 1], combined_info.api_maxhps_combined[i + 1],
                            false, APIPort.IsWithdrawn[combined_id, i], Config.BucketHPRatio);
                    }
                    //--パラメーター
                    //連合艦隊の艦名
                    combined_damage_str[i, 0] = oshiparray[i] != null ?
                        string.Format("{0}(Lv{1}:{2})", oshiparray[i].ShipName, oshiparray[i].api_lv, oshiparray[i].api_cond) : "";
                    CallBacks.SetControlToolTip(toolTip, handler.DamageCombined[i][0], Helper.MakeUnitToolTip(oshiparray[i]));
                    //開戦前のHP
                    combined_damage_str[i, 1] = combined_info.FStartHPCombined[i] > 0 ? combined_info.FStartHPCombined[i].ToString() : "";
                    combined_damage_fore[i, 1] = f_cond_before.GetColorBattleDetail();
                    combined_damage_back[i, 1] = f_cond_before.GetBackColorBattleDetail();
                    //与えたダメージ
                    combined_damage_str[i, 2] = combined_info.FStartHPCombined[i] > 0 ? combined_info.FScoreArrayCombined[i].ToString() : "";
                    if (i == mvp_combined) combined_damage_back[i, 2] = System.Drawing.Color.FromArgb(248, 224, 114);
                    //味方の残りHP
                    combined_damage_str[i, 3] = combined_info.api_nowhps_combined[i + 1] >= 0 ? combined_info.api_nowhps_combined[i + 1].ToString() : "";
                    combined_damage_fore[i, 3] = f_cond_after.GetColorBattleDetail();
                    combined_damage_back[i, 3] = f_cond_after.GetBackColorBattleDetail();
                    //味方のHPのパーセント
                    if(combined_info.api_nowhps_combined[i+1]>=0 && combined_info.api_maxhps_combined[i+1]>0)
                    {
                        combined_damage_str[i, 4] = ((double)combined_info.api_nowhps_combined[i + 1] / (double)combined_info.api_maxhps_combined[i + 1]).ToString("P0");
                    }
                    else
                    {
                        combined_damage_str[i, 4] = "";
                    }
                    combined_damage_fore[i, 4] = KancolleInfo.DefaultStringColor;
                    combined_damage_back[i, 4] = f_cond_after.GetBackColorBattleDetailPercent();
                }
                //ラベルにセット
                foreach (int i in Enumerable.Range(0, combined_damage_str.GetLength(0)))
                {
                    foreach (int j in Enumerable.Range(0, combined_damage_str.GetLength(1)))
                    {
                        handler.DamageCombined[i][j].Text = combined_damage_str[i, j];
                        handler.DamageCombined[i][j].ForeColor = combined_damage_fore[i, j];
                        handler.DamageCombined[i][j].BackColor = combined_damage_back[i, j];
                    }
                }
            }
        }

        //戦況詳細のリセット
        private static void ResetBattleDetail(UserControls.BattleDetailLabelHandler handler, System.Windows.Forms.ToolTip toolTip, bool isSquare)
        {
            //戦況
            handler.Overview.Header.Text = "概況";
            handler.Overview.MapCell.Text = "";
            handler.Overview.ID.Text = ""; handler.Overview.BattleCount.Text = "";
            toolTip.SetToolTip(handler.Overview.ID, null);
            handler.Overview.GaugeRatio.Text = "";
            handler.Overview.GaugeDetail.Text = "";
            //会戦
            handler.Situation.FormationFriend.Text = ""; handler.Situation.FormationEnemy.Text = "";
            handler.Situation.SearchFriend.Text = ""; handler.Situation.SearchEnemy.Text = "";
            handler.Situation.Engagement.Text = ""; handler.Situation.AirSup.Text = "";
            handler.Situation.AttachFriend.Text = ""; toolTip.SetToolTip(handler.Situation.AttachFriend, null);
            handler.Situation.AttachEnemy.Text = ""; toolTip.SetToolTip(handler.Situation.AttachEnemy, null);
            //航空戦
            handler.AirBattle.Stage1Friend.Text = ""; handler.AirBattle.Stage1Enemy.Text = "";
            handler.AirBattle.Stage2Friend.Text = ""; handler.AirBattle.Stage2Enemy.Text = "";
            handler.AirBattle.AirSupValueFriend.Text = ""; handler.AirBattle.AirSupValueEnemy.Text = "";
            if(handler.AirBattle.AirBaseFriend != null) handler.AirBattle.AirBaseFriend.Text = ""; 
            if(handler.AirBattle.AirBaseEnemy != null) handler.AirBattle.AirBaseEnemy.Text = "";
            //ダメージ
            foreach(var x in handler.Damage)
            {
                if (isSquare) CallBacks.SetControlToolTip(toolTip, x[0], null);
                foreach(var y in x)
                {
                    y.Text = "";
                    y.ForeColor = KancolleInfo.DefaultStringColor;
                    y.BackColor = KancolleInfo.DefaultBackColor;
                }
            }
            //敵編成
            handler.EnemyFleet.Header.Text = "敵編成";
            foreach(var x in handler.EnemyFleet.Name)
            {
                x.Text = "";
                if (isSquare) toolTip.SetToolTip(x, null);
            }
            foreach(var x in handler.EnemyFleet.Equips)
            {
                foreach(var y in x)
                {
                    y.Text = "";
                    toolTip.SetToolTip(y, null);
                }
            }
            //連合艦隊
            foreach(var x in handler.DamageCombined)
            {
                if (isSquare) toolTip.SetToolTip(x[0], null);
                foreach (var y in x)
                {
                    y.Text = "";
                    y.ForeColor = KancolleInfo.DefaultStringColor;
                    y.BackColor = KancolleInfo.DefaultBackColor;
                }
            }
        }
    }
}
