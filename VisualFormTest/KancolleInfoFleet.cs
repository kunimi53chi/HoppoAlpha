using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.KancolleCalcConvert;
using HoppoAlpha.DataLibrary.Const;
using HoppoAlpha.DataLibrary.SearchModel;

namespace VisualFormTest
{
    public static class KancolleInfoFleet
    {
        //艦隊表示用のToolTip
        private static System.Windows.Forms.ToolTip FleetToolTip { get; set; }

        static KancolleInfoFleet()
        {
            FleetToolTip = new System.Windows.Forms.ToolTip();
        }


        //補給状態の色分け
        public static System.Drawing.Color GetSupplyColor(double supplyrate)
        {
            if (supplyrate >= 1.0) return KancolleInfo.DefaultStringColor;
            else if (supplyrate >= 0.8) return KancolleInfo.TiredStringColor;
            else if (supplyrate >= 0.4) return KancolleInfo.CautionStringColor;
            else if (supplyrate > 0) return KancolleInfo.WarningStringColor;
            else return KancolleInfo.DeadStringColor;
        }
        public static System.Drawing.Color GetSupplyBackColor(double supplyrate)
        {
            if (supplyrate >= 1.0) return KancolleInfo.Transparent;
            else if (supplyrate >= 0.8) return KancolleInfo.Transparent;
            else if (supplyrate >= 0.4) return KancolleInfo.CautionBackColor;
            else if (supplyrate > 0) return KancolleInfo.WarningBackColor;
            else return KancolleInfo.DeadBackColor;
        }


        public static AirSupResult CalcEnemyAirSup(int enemyshipid, List<int> eslot)
        {
            if (!APIMaster.MstShips.ContainsKey(enemyshipid)) return new AirSupResult();

            ExMasterShip dship = APIMaster.MstShips[enemyshipid];

            AirSupResult result = new AirSupResult();
            //制空値
            if(dship.IsEnemyShip) result.AirSupValueMax = dship.AirSuperiority;
            else
            {
                //味方＝演習の場合に計算
                List<ExMasterSlotitem> dslots = new List<ExMasterSlotitem>();
                foreach(var x in eslot)
                {
                    ExMasterSlotitem d;
                    if (APIMaster.MstSlotitems.TryGetValue(x, out d)) dslots.Add(d);
                }
                //制空値の計算
                int airsup = 0;
                foreach(int i in Enumerable.Range(0, Math.Min(dship.api_maxeq.Count, dslots.Count)))
                {
                    var d = dslots[i];
                    if(d.IsAirCombatable)
                    {
                        double unitas = (double)d.api_tyku * Math.Sqrt(dship.api_maxeq[i]);
                        airsup += (int)unitas;
                    }
                }
                //結果
                result.OriginalValue = airsup;
                result.AirSupValueMin = airsup;
                result.AirSupValueMax = airsup;
            }

            //制空値が正しいかどうか
            if (dship.api_maxeq != null) result.IsCorrect = true;
            else
            {
                //艦載機を装備できるかどうか
                ApiMstStype stype;
                if(!APIMaster.MstStypesDictionary.TryGetValue(dship.api_stype, out stype))
                {
                    result.IsCorrect = false;
                }
                else
                {
                    //艦載機を装備できる艦種の場合
                    if (stype.IsAirCombatable) result.IsCorrect = false;
                    else result.IsCorrect = true;
                }
            }

            result.OriginalValue = dship.AirSuperiority;
            result.TrainingValueMax = 0;

            return result;
        }

        //敵艦の加重対空値
        public static int CalcEnemyWeightedAntiAir(int enemyid, List<int> eslot)
        {
            ExMasterShip dship;
            if (!APIMaster.MstShips.TryGetValue(enemyid, out dship)) return 0;

            //敵艦の場合
            if (dship.IsEnemyShip) return dship.WeightedAntiAir;
            //味方艦の場合だけ計算（演習）
            else
            {
                double taiku = (int)(2.0 * Math.Sqrt(dship.api_tyku[0]));//敵艦のみ加重対空値は2√素対空で
                foreach (var i in Enumerable.Range(0, Math.Min(eslot.Count, dship.api_maxeq.Count)))
                {
                    ExMasterSlotitem dslotitem;
                    if (APIMaster.MstSlotitems.TryGetValue(eslot[i], out dslotitem))
                    {
                        taiku += dslotitem.WeightedAntiAirEquipmentRatio * dslotitem.api_tyku;
                    }
                }

                return (int)taiku;
            }
        }

        //空母の火力換算の計算
        public static int CalcCarrierFire(ApiShip oship)
        {
            double fire = oship.api_karyoku[0] * 1.5 + 55;
            foreach(var dslot in oship.GetDSlotitems(APIGetMember.SlotItemsDictionary))
            {
                fire += (1.5 * dslot.api_raig + 2.0 * dslot.api_baku);
            }
            return (int)fire;
        }

        //艦隊情報のアップデート
        delegate void FleetInfoUpdateCallBack(System.Windows.Forms.Label[][,] labels, System.Windows.Forms.Label[] fleetname, System.Windows.Forms.Panel[] panels);
        delegate void FleetInfoSeparateUpdateCallBack(int fleetId, System.Windows.Forms.Label[,] labels, System.Windows.Forms.Label fleetname, System.Windows.Forms.Panel panel, bool isShort);

        //TabFleetのアップデート
        public static void FleetInfoUpdate(UserControls.TabFleet tabFleetControl)
        {
            if(!tabFleetControl.IsHandleCreated)
            {
                var form = tabFleetControl.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }

            if(tabFleetControl.InvokeRequired)
            {
                FleetInfoUpdateCallBack cb = (labels, fleetname, panels) => FleetInfoUpdateLogic(labels, fleetname, panels);
                tabFleetControl.Invoke(cb, new object[]{tabFleetControl.LabelFleet, tabFleetControl.LabelFleetName, tabFleetControl.PanelFleet});
            }
            else
            {
                FleetInfoUpdateLogic(tabFleetControl.LabelFleet, tabFleetControl.LabelFleetName, tabFleetControl.PanelFleet);
            }
        }

        public static void FleetInfoShortUpdate(UserControls.TabFleetShort tabFleetShort)
        {
            if (!tabFleetShort.IsHandleCreated)
            {
                var form = tabFleetShort.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }

            if (tabFleetShort.InvokeRequired)
            {
                FleetInfoSeparateUpdateCallBack cb = new FleetInfoSeparateUpdateCallBack(FleetInfoUpdateLogicSeparate);
                tabFleetShort.Invoke(cb, new object[] {tabFleetShort.FleetIndex, tabFleetShort.LabelFleet, tabFleetShort.LabelFleetName, tabFleetShort.PanelFleet, true});
            }
            else
            {
                FleetInfoUpdateLogicSeparate(tabFleetShort.FleetIndex, tabFleetShort.LabelFleet, tabFleetShort.LabelFleetName, tabFleetShort.PanelFleet, true);
            }
        }

        private static void FleetInfoUpdateLogicSeparate(int fleetId, System.Windows.Forms.Label[,] labels, System.Windows.Forms.Label fleetname, System.Windows.Forms.Panel panel, bool isShortWindow)
        {
            //NULLなら帰投
            if (APIPort.DeckPorts == null) return;
            //ToolTipをHide
            if (FleetToolTip != null)
            {
                for (int j = 0; j < labels.GetLength(0); j++)
                {
                    FleetToolTip.Hide(labels[j, 0]);
                }
            }
            //Tipsの初期化
            if (FleetToolTip == null) FleetToolTip = new System.Windows.Forms.ToolTip();
            //艦隊オブジェクトの取得
            var fleet = APIPort.DeckPorts[fleetId];

            //--ロジック部分
            //文字色
            System.Drawing.Color color1, color2, color3, color4, color5;
            System.Drawing.Color bcolor1, bcolor2, bcolor3, bcolor4, bcolor5;
            //レベル合計
            int sumlv = 0;
            int flaglv = 0;
            int fleet_as = 0;//艦隊合計制空値
            int fleet_as_min = 0;
            List<UnitSearchResult> unit_sakuteki = new List<UnitSearchResult>();//個別索敵値
            int drumnum = 0; int drumshipnum = 0;//ドラム缶総数、装備している船の数
            int radarnum = 0;//電探総数
            var daihatsu = new List<ApiShip.DaihatsuResult>();//大発
            int i = 0;
            //入渠ドックの船のID
            int[] ndock_id = (from d in APIPort.Ndocks
                              select d.api_ship_id).ToArray();
            //補給が終わっているか
            bool supplyfinished = true;
            //艦隊の大破警告
            WarnState fleetWarnState = WarnState.ConditionGreen;
            //簡易モードか詳細モードか
            bool fullshow = isShortWindow || Config.FleetShowFull[fleetId];//ShortWindowならTrue
            //ラベルのテキスト
            string[,] label_text = new string[labels.GetLength(0), labels.GetLength(1)];
            System.Drawing.Color[,] label_forecolor = new System.Drawing.Color[label_text.GetLength(0), label_text.GetLength(1)];
            System.Drawing.Color[,] label_backcolor = new System.Drawing.Color[label_text.GetLength(0), label_text.GetLength(1)];
            foreach (int sid in fleet.api_ship)
            {
                //sid=-1なら初期化
                if (sid == -1)
                {
                    for (int j = 0; j < labels.GetLength(1); j++)
                    {
                        labels[i, j].Text = "";
                        labels[i, j].ForeColor = KancolleInfo.DefaultStringColor2;
                        labels[i, j].BackColor = KancolleInfo.Transparent;
                    }
                    labels[i, 0].Tag = null;
                    i++;
                    continue;
                }
                //船の取得
                ApiShip oship = APIPort.ShipsDictionary[sid];
                //入渠しているかどうか
                bool isbath = Array.IndexOf(ndock_id, oship.api_id) != -1;
                //損傷状態
                HPCondition hpcond = oship.GetHPCondition(isbath, APIPort.IsWithdrawn[fleetId, i], Config.BucketHPRatio, APIGetMember.SlotItemsDictionary);
                //レベル合計
                if (!hpcond.HasFlag(HPCondition.IsWithdrawn)) sumlv += oship.api_lv;
                //旗艦Lv
                if (i == 0) flaglv = oship.api_lv;
                //最大燃料と弾薬(ケッコンカッコカリの燃費向上)→これは考慮しなくていい
                //燃料と弾薬の目盛り
                double fuelrate = Math.Ceiling((double)oship.api_fuel / (double)oship.DShip.api_fuel_max * 10) / 10;
                double bullrate = Math.Ceiling((double)oship.api_bull / (double)oship.DShip.api_bull_max * 10) / 10;
                supplyfinished = supplyfinished && oship.IsBullMax && oship.IsFuelMax;
                //ドラム缶
                var drumpership = oship.GetDrumNum(APIGetMember.SlotItemsDictionary);
                if(drumpership > 0)
                {
                    drumnum += drumpership;
                    drumshipnum++;
                }
                //電探
                radarnum += oship.GetRadarNum(APIGetMember.SlotItemsDictionary);
                //大発
                daihatsu.Add(oship.GetDaihatsuNum(APIGetMember.SlotItemsDictionary));
                //制空値
                var unit_as = oship.GetAirSupValue(APIGetMember.SlotItemsDictionary, APIPort.IsWithdrawn, APIPort.DeckPorts);//ユニット制空
                bool isWithDrawn = hpcond.HasFlag(HPCondition.IsWithdrawn);
                if (!isWithDrawn)
                {
                    fleet_as += unit_as.AirSupValueMax;
                    fleet_as_min += unit_as.AirSupValueMin;
                }
                //索敵値
                UnitSearchResult us = UnitSearchCenter(oship);
                if (!isWithDrawn)
                {
                    unit_sakuteki.Add(us);
                }
                //大破警告
                fleetWarnState = (WarnState)Math.Max((int)fleetWarnState,
                    (int)oship.GetWarnState(i == 0, isWithDrawn, Config.BucketHPRatio, APIGetMember.SlotItemsDictionary));
                //--艦隊情報
                //FullModeの場合
                if (fullshow)
                {
                    //文字色の設定
                    //Name, Level, Exp = color1
                    //fuel = color2
                    //bull = color3
                    //sirsup, search = color4
                    //HP = color5
                    //撃沈されている場合
                    if (hpcond.HasFlag(HPCondition.Sank))
                    {
                        color1 = color2 = color3 = color4 = color5 = HPCondition.Sank.GetColor();
                        bcolor1 = bcolor2 = bcolor3 = bcolor4 = bcolor5 = HPCondition.Sank.GetBackColor();
                    }
                    //退避している場合
                    else if (hpcond.HasFlag(HPCondition.IsWithdrawn))
                    {
                        color1 = color2 = color3 = color4 = color5 = HPCondition.IsWithdrawn.GetColor();
                        bcolor1 = bcolor2 = bcolor3 = bcolor4 = bcolor5 = HPCondition.IsWithdrawn.GetBackColor();
                    }
                    //撃沈されていない場合
                    else
                    {
                        //color1 キラキラついている場合→キラキラ　それ以外白
                        if (oship.api_cond >= 50)
                        {
                            color1 = KancolleInfo.GoodStringColor;
                            bcolor1 = KancolleInfo.Transparent;
                        }
                        else if (oship.api_cond >= 40)
                        {
                            color1 = KancolleInfo.DefaultStringColor2;
                            bcolor1 = KancolleInfo.Transparent;
                        }
                        else if (oship.api_cond >= 30)
                        {
                            color1 = KancolleInfo.TiredStringColor;
                            bcolor1 = KancolleInfo.Transparent;
                        }
                        else if (oship.api_cond >= 20)
                        {
                            color1 = KancolleInfo.CautionStringColor;
                            bcolor1 = KancolleInfo.CautionBackColor;
                        }
                        else
                        {
                            color1 = KancolleInfo.WarningStringColor;
                            bcolor1 = KancolleInfo.WarningBackColor;
                        }
                        //color2 燃料が10→白　8-9→黄色　4-7→オレンジ　1-3→赤　0→黒
                        color2 = GetSupplyColor(fuelrate);
                        bcolor2 = GetSupplyBackColor(fuelrate);
                        //color3 弾薬で2と同様
                        color3 = GetSupplyColor(bullrate);
                        bcolor3 = GetSupplyBackColor(bullrate);
                        //color4 撃沈されていない限り白
                        color4 = KancolleInfo.DefaultStringColor2;
                        bcolor4 = KancolleInfo.Transparent;
                        //color5 無傷・小破→白　中破→オレンジ　大破→赤
                        color5 = hpcond.GetColor();
                        bcolor5 = hpcond.GetBackColor();
                    }
                    //文字の設定
                    //[0]:名前 color1
                    label_text[i, 0] = i + 1 + " " + oship.ShipName;
                    label_forecolor[i, 0] = color1;
                    label_backcolor[i, 0] = bcolor1;
                    //[1]:Level(cond) color1
                    label_text[i, 1] = "Lv" + oship.api_lv + "(" + oship.api_cond + ")";
                    label_forecolor[i, 1] = color1;
                    label_backcolor[i, 1] = bcolor1;
                    //[2]:次:Exp color1
                    label_text[i, 2] = "次:" + oship.api_exp[1];
                    label_forecolor[i, 2] = color1;
                    label_backcolor[i, 2] = bcolor1;
                    //[3]:燃 color2
                    label_text[i, 3] = "燃";
                    label_forecolor[i, 3] = color2;
                    label_backcolor[i, 3] = bcolor2;
                    //[4]:弾 color3
                    label_text[i, 4] = "弾";
                    label_forecolor[i, 4] = color3;
                    label_backcolor[i, 4] = bcolor3;
                    //[5]:制 color4
                    label_text[i, 5] = string.Format("制{0}(+{1})", unit_as.AirSupValueMax, unit_as.TrainingValueMax);
                    label_forecolor[i, 5] = color4;
                    label_backcolor[i, 5] = bcolor4;
                    //[6]:索 color5
                    label_text[i, 6] = "索" + us.DisplayUnitSearch();
                    label_forecolor[i, 6] = color4;
                    label_backcolor[i, 6] = bcolor4;
                    //[7]:nowHP/maxHP[hpcond]
                    label_text[i, 7] = oship.api_nowhp + "/" + oship.api_maxhp + HPConditionExt.DisplayLong(hpcond);
                    label_forecolor[i, 7] = color5;
                    label_backcolor[i, 7] = bcolor5;
                }
                //簡易モードの場合
                else
                {
                    //文字色（簡易）
                    //Name, cond : color1
                    //fuel : color2
                    //bull : color3
                    //hpcond : color4
                    //撃沈の場合
                    if (hpcond.HasFlag(HPCondition.Sank))
                    {
                        color1 = color2 = color3 = color4 = HPCondition.Sank.GetColor();
                        bcolor1 = bcolor2 = bcolor3 = bcolor4 = HPCondition.Sank.GetBackColor();
                    }
                    else
                    {
                        color1 = oship.api_cond >= 50 ? KancolleInfo.GoodStringColor : KancolleInfo.DefaultStringColor2;
                        color2 = GetSupplyColor(fuelrate);
                        color3 = GetSupplyColor(bullrate);
                        color4 = hpcond.GetColor();

                        bcolor1 = KancolleInfo.Transparent;
                        bcolor2 = GetSupplyBackColor(fuelrate);
                        bcolor3 = GetSupplyBackColor(bullrate);
                        bcolor4 = hpcond.GetBackColor();
                    }
                    //ラベルの設定
                    //[0] 名前
                    string short_name = oship.ShipName;
                    short_name = short_name.Length < 3 ? short_name : short_name.Substring(0, 3);
                    label_text[i, 0] = short_name;
                    label_forecolor[i, 0] = color1;
                    label_backcolor[i, 0] = bcolor1;
                    //[1] (cond)
                    label_text[i, 1] = "(" + oship.api_cond + ")";
                    label_forecolor[i, 1] = color1;
                    label_backcolor[i, 1] = bcolor1;
                    //[3]:燃 color2
                    label_text[i, 2] = "燃";
                    label_forecolor[i, 2] = color2;
                    label_backcolor[i, 2] = bcolor2;
                    //[4]:弾 color3
                    label_text[i, 3] = "弾";
                    label_forecolor[i, 3] = color3;
                    label_backcolor[i, 3] = bcolor3;
                    //[5]:[hpcond]
                    label_text[i, 4] = HPConditionExt.DisplayShort(hpcond);
                    label_forecolor[i, 4] = color4;
                    label_backcolor[i, 4] = bcolor4;
                }
                //--ToolTip用のテキスト→ロジックはHelperに移動
                //Tipsをタグに
                labels[i, 0].Tag = Helper.MakeUnitToolTip(oship);
                //カウンター増加
                i++;
            }
            //--艦隊名
            System.Drawing.Color headercol;
            if ((int)fleetWarnState >= (int)WarnState.FlagshipDamaged) headercol = KancolleInfo.DamagedLabelHeaderColor;
            else if (fleetWarnState == WarnState.HasDameconDamaged) headercol = KancolleInfo.DameconLabelHeaderColor;
            else if (supplyfinished) headercol = KancolleInfo.DefaultLabelHeaderColor;
            else headercol = KancolleInfo.NotSupplyLabelHeaderColor;
            //索敵値
            double total_saku = FleetSearchCenter(unit_sakuteki);
            //艦隊大発
            var total_daihatsu = ApiShip.DaihatsuResult.ToFleetResult(daihatsu);
            //艦隊のToolTip
            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine(string.Format("艦隊索敵値[{0}] : {1}", Config.SearchUsingModel.GetName(), total_saku.ToString("F2")));
            sb2.AppendLine(string.Format("艦隊制空値(上限) : {0}", fleet_as));
            sb2.AppendLine(string.Format("艦隊制空値(下限) : {0}", fleet_as_min));
            sb2.AppendLine("----------------");
            sb2.AppendLine(string.Format("電探装備数 : {0}", radarnum));
            sb2.AppendLine(string.Format("ドラム缶総数 : {0}", drumnum));
            sb2.AppendLine(string.Format("ドラム缶装備船数 : {0}", drumshipnum));
            sb2.AppendLine(string.Format("大発動艇総数 : {0}", total_daihatsu.FleetNumDaihatsu));
            sb2.AppendLine(string.Format("大発動艇補正 : {0}{1}", total_daihatsu.FleetCappedBonusRatio.ToString("P1"),
                total_daihatsu.IsCapped ? "(" + total_daihatsu.FleetNonCappedBonusRatio.ToString("P1") + ")" : ""));
            FleetToolTip.SetToolTip(fleetname, sb2.ToString());
            if (fullshow)
            {
                string kantai;
                if (APIPort.CombinedFlag == 0) kantai = "第" + (fleetId + 1) + "艦隊 : ";
                else
                {
                    if (fleetId == 0) kantai = "連合本隊 : ";
                    else kantai = "連合水雷 : ";
                }
                fleetname.Text = kantai + fleet.api_name + " (Lv計" + sumlv + " / 制" + fleet_as + " / 索" + total_saku.ToString("F2") + " / 電" + radarnum + ")";
                fleetname.BackColor = headercol;
            }
            else
            {
                fleetname.Text = "第" + (fleetId + 1) + " 旗" + flaglv + "/計" + sumlv;
                fleetname.BackColor = headercol;
            }
            //ラベルの変更
            for (int k = 0; k < labels.GetLength(0); k++)
            {
                for (int l = 0; l < labels.GetLength(1); l++)
                {
                    float fontSize = 9;
                    if (l == 0 && label_text[k, l] != null && label_text[k, l].Length >= 9) fontSize = 8;
                    if (k % 2 == 0 && label_backcolor[k, l] == KancolleInfo.Transparent)
                    {
                        labels[k, l].Text = label_text[k, l];
                        labels[k, l].ForeColor = label_forecolor[k, l];
                        labels[k, l].Font = new System.Drawing.Font(labels[k, l].Font.FontFamily, fontSize);
                        if((int)fleetWarnState >= (int)WarnState.FlagshipDamaged)
                        {
                            labels[k, l].BackColor = KancolleInfo.DamagedBackColor2;
                        }
                        else if(fleetWarnState == WarnState.HasDameconDamaged)
                        {
                            labels[k, l].BackColor = KancolleInfo.DameconBackColor2;
                        }
                        else if (supplyfinished)
                        {
                            labels[k, l].BackColor = KancolleInfo.DefaultBackColor2;
                        }
                        else
                        {
                            labels[k, l].BackColor = KancolleInfo.NotSupplyBackColor2;
                        }
                    }
                    else
                    {
                        labels[k, l].Text = label_text[k, l];
                        labels[k, l].ForeColor = label_forecolor[k, l];
                        labels[k, l].BackColor = label_backcolor[k, l];
                        labels[k, l].Font = new System.Drawing.Font(labels[k, l].Font.FontFamily, fontSize);
                    }
                }
            }
            //奇数行の色（パネル側で操作）
            if ((int)fleetWarnState >= (int)WarnState.FlagshipDamaged)
            {
                panel.BackColor = KancolleInfo.DamagedBackColor;
            }
            else if (fleetWarnState == WarnState.HasDameconDamaged)
            {
                panel.BackColor = KancolleInfo.DameconBackColor;
            }
            else if(supplyfinished)
            {
                panel.BackColor = KancolleInfo.DefaultBackColor;
            }
            else
            {
                panel.BackColor = KancolleInfo.NotSupplyBackColor;
            }
        }

        public static UnitSearchResult UnitSearchCenter(ApiShip oship)
        {
            switch(Config.SearchUsingModel)
            {
                case Models.Old25: return Old25Model.CalcUnitSearchOld25(oship, APIGetMember.SlotItemsDictionary);
                case Models.Hoppo201: return SearchHoppo201.CalcUnitSearch(oship, APIGetMember.SlotItemsDictionary);
                case Models.Model33: return Model33.CalcUnitSearch(oship, APIGetMember.SlotItemsDictionary);
                default: throw new NotImplementedException("索敵モデルが実装されていません");
            }
        }

        public static double FleetSearchCenter(IEnumerable<UnitSearchResult> searches)
        {
            switch(Config.SearchUsingModel)
            {
                case Models.Old25: return Old25Model.CalcFleetSearchOld25(searches, APIPort.Basic.api_level);
                case Models.Hoppo201: return SearchHoppo201.CalcFleetSearch(searches, APIPort.Basic.api_level);
                case Models.Model33: return Model33.CalcFleetSearch(searches, APIPort.Basic.api_level);
                default: throw new NotImplementedException("索敵モデルが実装されていません");
            }
        }

        public static string DisplayUnitSearch(this UnitSearchResult ur)
        {
            switch(Config.SearchUsingModel)
            {
                case Models.Hoppo201: return ur.UnitSearchInt.ToString();
                case Models.Model33: return ur.UnitSearchDouble.ToString("F1");
                case Models.Old25: return ur.UnitSearchInt.ToString();
                default: throw new NotImplementedException("索敵モデルが実装されていません");
            }
        }

        private static void FleetInfoUpdateLogic(System.Windows.Forms.Label[][,] labels, System.Windows.Forms.Label[] fleetname,
            System.Windows.Forms.Panel[] panels)
        {
            if (APIPort.DeckPorts == null) return;
            foreach (int i in Enumerable.Range(0, Math.Min(labels.Length, APIPort.DeckPorts.Count)))
            {
                FleetInfoUpdateLogicSeparate(i, labels[i], fleetname[i], panels[i], false);
            }
        }

        //艦隊表示用のマウスホバー
        public static void FleetLabel_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.Label label = (System.Windows.Forms.Label)sender;
            string text = (string)label.Tag;
            if(text != null) CallBacks.SetLabelToolTipShow(label, text, FleetToolTip);
        }
        //艦隊表示用のマウスリーブ
        public static void FleetLabel_MouseLeave(object sender, EventArgs e)
        {
            System.Windows.Forms.Label label = (System.Windows.Forms.Label)sender;
            CallBacks.SetLabelToolTipHide(label, FleetToolTip);
        }

        //コンテクストメニューの右クリック
        //艦隊パネルの右クリック
        public static void contextMenuStrip_fleet_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            //艦隊IDの取得
            System.Windows.Forms.ContextMenuStrip context = (System.Windows.Forms.ContextMenuStrip)sender;
            System.Windows.Forms.Panel source = (System.Windows.Forms.Panel)context.SourceControl;
            DockingWindows.DockWindowTabCollection tab = DockingWindows.DockWindowTabCollection.GetCollection(source);
            int idx = Array.IndexOf(tab.Fleet.PanelFleet, source);
            //クリックの処理
            if (e.ClickedItem.Name == "toolStripMenuItem_fleet_copy")
            {
                context_Fleet_CopyToClipboard(idx);
            }
            else if(e.ClickedItem.Name == "toolStripMenuItem_fleet_query")
            {
                context_Fleet_MakeQuery(idx, tab);
            }
            else if(e.ClickedItem.Name == "toolStripMenuItem_fleet_convertdeck")
            {
                context_ConvertDeckBuilder(idx);
            }
        }

        //編成のコピー
        public static void context_Fleet_CopyToClipboard(int fleetidx)
        {
            if (APIPort.DeckPorts == null || APIPort.ShipsDictionary == null || APIGetMember.SlotItemsDictionary == null) return;
            //編成のコピー
            List<List<string>> fleettext = new List<List<string>>();
            int totallv = 0; int totalseiku = 0;
            List<UnitSearchResult> searches = new List<UnitSearchResult>();
            int cnt = 0;
            foreach (int shipid in APIPort.DeckPorts[fleetidx].api_ship)
            {
                cnt++;
                if (shipid == -1) continue;
                ApiShip oship = APIPort.ShipsDictionary[shipid];
                //船がある場合
                List<string> shiptext = new List<string>();
                //番号
                shiptext.Add(cnt.ToString());
                //名前
                shiptext.Add(oship.ShipName);
                //レベル
                shiptext.Add(string.Format("Lv{0}", oship.api_lv));
                totallv += oship.api_lv;
                //制/索
                var seiku = oship.GetAirSupValue(APIGetMember.SlotItemsDictionary, APIPort.IsWithdrawn, APIPort.DeckPorts);
                totalseiku += seiku.AirSupValueMax;
                var us = UnitSearchCenter(oship);
                searches.Add(us);
                shiptext.Add(string.Format("制{0}(+{1})/索{2}", seiku.AirSupValueMax, seiku.TrainingValueMax, us.DisplayUnitSearch()));
                //装備
                var dslots = oship.GetDSlotitems(APIGetMember.SlotItemsDictionary);
                var oslots = oship.GetOSlotitems(APIGetMember.SlotItemsDictionary);
                foreach (var i in Enumerable.Range(0, Math.Min(dslots.Count, oslots.Count)))
                {
                    string reinforce;
                    if (oslots[i].api_alv > 0) reinforce = "◆" + oslots[i].api_alv;
                    else reinforce = "★" + oslots[i].api_level;

                    shiptext.Add(dslots[i].api_name + "(" + reinforce + ")");
                }
                //追加
                fleettext.Add(shiptext);
            }
            //StringBuilderに
            StringBuilder sb = new StringBuilder();
            //ヘッダー
            sb.AppendFormat("第{0}艦隊\tLv合計{1}\t制空{2}\t索敵{3}({4})", fleetidx + 1, totallv,
                totalseiku, FleetSearchCenter(searches).ToString("N1"), Config.SearchUsingModel.GetName()).AppendLine();
            //船のアイテム
            foreach (List<string> sitem in fleettext)
            {
                sb.AppendLine(string.Join("\t", sitem));
            }
            //クリップボードに
            System.Windows.Forms.Clipboard.SetDataObject(sb.ToString(), true);
        }

        //クエリの作成
        public static void context_Fleet_MakeQuery(int fleetidx, DockingWindows.DockWindowTabCollection tabcollection)
        {
            if (APIPort.DeckPorts == null || APIPort.ShipsDictionary == null || APIGetMember.SlotItemsDictionary == null) return;
            //クエリ
            UnitQuery query = new UnitQuery();
            query.Name = string.Format("{0} 第{1}艦隊", DateTime.Now.ToString(), fleetidx + 1);
            query.Query = new List<UnitQueryItem>();
            //クエリアイテム
            UnitQueryItem item = new UnitQueryItem(UnitQueryMode.ID);
            //船IDの追加
            foreach(int x in APIPort.DeckPorts[fleetidx].api_ship)
            {
                if (x == -1) continue;
                item.SearchesAdd(x, UnitQueryRangeMode.Equals);
            }
            query.Query.Add(item);
            //確認ダイアログの起動
            UserControls.TabFleet_MakeQuery mquery = new UserControls.TabFleet_MakeQuery(query);
            mquery.TabCollection = tabcollection;
            mquery.ShowDialog();
        }

        //デッキビルダーにコンバート
        public static void context_ConvertDeckBuilder(int fleetId)
        {
            if (APIPort.DeckPorts == null) return;
            var dialog = new KancolleInfoFleet_ConvertDeckBuilder(APIPort.DeckPorts.Count, fleetId);
            dialog.ShowDialog();
        }

        //デッキビルダーのロジック部分
        public static string context_ConvertDeckBuilderLogic(List<bool> deckFlags)
        {
            if (APIPort.DeckPorts == null || APIPort.ShipsDictionary == null || APIGetMember.SlotItemsDictionary == null) return null;
            //コンバートアイテム
            ToDeckBuilder conv = new ToDeckBuilder();
            foreach(int i in Enumerable.Range(0, deckFlags.Count))
            {
                if (i >= APIPort.DeckPorts.Count) break;
                //範囲内の場合
                var deck = APIPort.DeckPorts[i];
                var convDeck = new ToDeckBuilder.DeckItem();
                if (!deckFlags[i])
                {
                    conv.Decks.Add(convDeck);
                    continue;//FlagがFlaseの場合
                }
                foreach (var j in deck.api_ship)
                {
                    ApiShip oship;
                    if (j == -1) continue;
                    if (!APIPort.ShipsDictionary.TryGetValue(j, out oship)) continue;
                    //船のオブジェクトが見つかった場合
                    var ship = new ToDeckBuilder.ShipItem();
                    ship.ShipID = oship.api_ship_id;
                    ship.Level = oship.api_lv;
                    if (oship.api_lucky[0] != oship.DShip.api_luck[0]) ship.Luck = oship.api_lucky[0];//運が異なっていた場合のみ
                    //装備のオブジェクト
                    var slots = new ToDeckBuilder.InternalList<ToDeckBuilder.SlotItem>();
                    foreach (var s in oship.api_slot)
                    {
                        SlotItem oslot;
                        if (s == -1) continue;
                        if (!APIGetMember.SlotItemsDictionary.TryGetValue(s, out oslot)) continue;
                        //装備オブジェクトがあった場合
                        var slotobj = new ToDeckBuilder.SlotItem()
                        {
                            Id = oslot.api_slotitem_id,
                            ReinforcedLevel = oslot.api_level,
                            MasterLevel = oslot.api_alv,
                        };
                        slots.Add(slotobj);
                    }
                    ship.Items = slots;
                    //拡張スロットのオブジェクト
                    SlotItem exslot;
                    if(APIGetMember.SlotItemsDictionary.TryGetValue(oship.api_slot_ex, out exslot))
                    {
                        var exobj = new ToDeckBuilder.SlotItem()
                        {
                            Id = exslot.api_slotitem_id,
                            ReinforcedLevel = exslot.api_level,
                            MasterLevel = exslot.api_alv,
                        };
                        ship.ExItem = exobj;
                    }
                    //キャラアイテムの追加
                    convDeck.Add(ship);
                }
                //デッキアイテムの追加
                conv.Decks.Add(convDeck);
            }
            return conv.ToString();
        }
    }
}
