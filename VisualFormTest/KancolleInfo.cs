using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class KancolleInfo
    {
        delegate void SetChartCallBack(System.Windows.Forms.DataVisualization.Charting.Chart chart, GraphInfo info);

        //文字色
        //public static System.Drawing.Color DefaultStringColor = System.Drawing.Color.White;
        public static System.Drawing.Color DefaultStringColor = System.Drawing.Color.FromArgb(51, 51, 51);
        public static System.Drawing.Color DefaultStringColor2 = System.Drawing.Color.FromArgb(25, 25, 25);
        public static System.Drawing.Color DefaultStringColorWhite = System.Drawing.Color.FromArgb(209, 209, 209);
        public static System.Drawing.Color Transparent = System.Drawing.Color.Transparent;
        public static System.Drawing.Color GoodStringColor = System.Drawing.Color.FromArgb(0, 113, 181);//(0, 163, 181)→(0, 130, 145)
        public static System.Drawing.Color TiredStringColor = System.Drawing.Color.FromArgb(127, 127, 127);
        public static System.Drawing.Color CautionStringColor = System.Drawing.Color.FromArgb(221, 100, 0);
        public static System.Drawing.Color CautionBackColor = System.Drawing.Color.FromArgb(252, 224, 201);
        public static System.Drawing.Color WarningStringColor = System.Drawing.Color.FromArgb(242, 242, 242);
        public static System.Drawing.Color WarningBackColor = System.Drawing.Color.FromArgb(255, 32, 32);
        public static System.Drawing.Color DeadStringColor = System.Drawing.Color.FromArgb(242, 242, 242);
        public static System.Drawing.Color DeadBackColor = System.Drawing.Color.FromArgb(39, 39, 39);
        public static System.Drawing.Color WithdrawingColor = System.Drawing.Color.FromArgb(242, 242, 242);
        public static System.Drawing.Color WithdrawBackColor = System.Drawing.Color.FromArgb(83, 142, 213);
        //public static System.Drawing.Color DefaultBackColor = System.Drawing.Color.FromArgb(0x83, 0x4e, 0x62);
        public static System.Drawing.Color DefaultLabelHeaderColor = System.Drawing.Color.FromArgb(75, 228, 137);
        public static System.Drawing.Color DefaultBackColor = System.Drawing.Color.White;
        public static System.Drawing.Color DefaultBackColor2 = System.Drawing.Color.FromArgb(242, 242, 242);

        public static System.Drawing.Color NotSupplyLabelHeaderColor = System.Drawing.Color.FromArgb(248, 224, 114);
        public static System.Drawing.Color NotSupplyBackColor = System.Drawing.Color.FromArgb(252, 243, 205);
        public static System.Drawing.Color NotSupplyBackColor2 = System.Drawing.Color.FromArgb(231, 223, 188);

        public static System.Drawing.Color DamagedLabelHeaderColor = System.Drawing.Color.FromArgb(248, 144, 144);
        public static System.Drawing.Color DamagedBackColor = System.Drawing.Color.FromArgb(239, 213, 215);
        public static System.Drawing.Color DamagedBackColor2 = System.Drawing.Color.FromArgb(231, 188, 188);

        public static System.Drawing.Color DameconLabelHeaderColor = System.Drawing.Color.FromArgb(248, 114, 224);
        public static System.Drawing.Color DameconBackColor = System.Drawing.Color.FromArgb(252, 205, 244);
        public static System.Drawing.Color DameconBackColor2 = System.Drawing.Color.FromArgb(231, 188, 223);

        public static System.Drawing.Color BlackbackNormalStringColor = System.Drawing.Color.FromArgb(209, 209, 209);
        public static System.Drawing.Color MaterialPlusStringColor = System.Drawing.Color.Red;//229, 20, 0
        public static System.Drawing.Color MaterialMinusStringColor = System.Drawing.Color.FromArgb(0, 80, 239);

        //public static System.Drawing.Color ControlTextColor = System.Drawing.SystemColors.ControlText;
        public static System.Drawing.Color ControlTextColor = System.Drawing.Color.FromArgb(51, 51, 51);

        //public static System.Drawing.Color PanelDefaultBackColor = System.Drawing.Color.FromArgb(130, 192, 205);
        public static System.Drawing.Color PanelDefaultBackColor = System.Drawing.Color.White;
        public static System.Drawing.Color PanelCautionBackColor = System.Drawing.Color.FromArgb(219, 188, 134);
        public static System.Drawing.Color PanelWarningBackColor = System.Drawing.Color.FromArgb(160, 78, 144);

        //艦隊表示用のToolTip
        private static System.Windows.Forms.ToolTip FleetToolTip { get; set; }

        //別窓用のアイテム
        public static ChartViewer chartViewer;

        //エポック秒
        public static DateTime EPOCH_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0);


        //艦娘の数と装備の数の表示
        public static void SetShipSlotitemNum(System.Windows.Forms.Label[] labels, System.Windows.Forms.Panel[] panels,
            System.Windows.Forms.ToolStripStatusLabel status)
        {
            if (APIPort.ShipsDictionary == null) return;
            //艦娘数の文字とパネルの色
            System.Drawing.Color charalabelcol, charapanelcol;
            bool charabool;
            int nowchara = APIPort.ShipsDictionary.Count; int maxchara = APIPort.Basic.api_max_chara;
            if (nowchara == maxchara)
            {
                //満杯の場合
                charalabelcol = DefaultStringColor; charabool = true;
                charapanelcol = PanelWarningBackColor;
            }
            else if (nowchara > maxchara - 5)
            {
                //イベントの出撃要件である-5
                charalabelcol = ControlTextColor; charabool = true;
                charapanelcol = PanelCautionBackColor;
            }
            else
            {
                charalabelcol = ControlTextColor; charabool = false;
                charapanelcol = PanelDefaultBackColor;
            }
            //艦娘の数
            CallBacks.SetShipNumLabel(labels[0], nowchara.ToString(), charalabelcol, charabool);
            CallBacks.SetLabelTextAndColor(labels[1], "/ " + maxchara, charalabelcol);
            CallBacks.SetShipNumPanel(panels[0], charapanelcol);
            //装備の文字の色
            System.Drawing.Color slotitemlabelcol, slotitempanelcol;
            bool slotitembool;
            int maxslotitem = APIPort.Basic.api_max_slotitem + 3;
            //装備の数は出撃中は別計算
            int nowslotitem;
            if (APIReqMap.SallyDeckPort == 0)
            {
                nowslotitem = APIGetMember.SlotItemsDictionary.Count;
            }
            else
            {
                nowslotitem = APIGetMember.GetSlotitemNumOnSortie();
            }
            if (nowslotitem >= maxslotitem - 3)
            {
                //満杯
                slotitemlabelcol = DefaultStringColor; slotitembool = true;
                slotitempanelcol = PanelWarningBackColor;
            }
            else if (nowslotitem > maxslotitem - 20)
            {
                //イベント要件である-20
                slotitemlabelcol = ControlTextColor; slotitembool = true;
                slotitempanelcol = PanelCautionBackColor;
            }
            else
            {
                slotitemlabelcol = ControlTextColor; slotitembool = false;
                slotitempanelcol = PanelDefaultBackColor;
            }
            //装備の数
            CallBacks.SetShipNumLabel(labels[2], nowslotitem.ToString(), slotitemlabelcol, slotitembool);
            CallBacks.SetLabelTextAndColor(labels[3], "/ " + maxslotitem.ToString(), slotitemlabelcol);
            CallBacks.SetShipNumPanel(panels[1], slotitempanelcol);
            //---
            //下への表示
            if (Config.StatusStripShowShipNum)
            {
                string status_text = string.Format("船:{0}/{1}, 装:{2}/{3}", nowchara, maxchara, nowslotitem, maxslotitem);
                status.Text = status_text;
            }
            else
            {
                status.Text = "";
            }
        }

        //一般タブの装備数の表示
        public static void SetShipSlotitemNum_General(System.Windows.Forms.Label[] labels)
        {
            if (APIPort.ShipsDictionary == null) return;
            //艦娘数の文字とパネルの色
            System.Drawing.Color charabackcol;
            int nowchara = APIPort.ShipsDictionary.Count; int maxchara = APIPort.Basic.api_max_chara;
            if (nowchara == maxchara)
            {
                //満杯の場合
                charabackcol = PanelWarningBackColor;
            }
            else if (nowchara > maxchara - 5)
            {
                //イベントの出撃要件である-5
                charabackcol = PanelCautionBackColor;
            }
            else
            {
                charabackcol = PanelDefaultBackColor;
            }
            //艦娘の数
            CallBacks.SetLabelTextAndColorDouble(labels[0], string.Format("{0} / {1}", nowchara, maxchara),
                KancolleInfo.DefaultStringColor, charabackcol);
            //装備の文字の色
            System.Drawing.Color slotitembackcol;
            int maxslotitem = APIPort.Basic.api_max_slotitem + 3;
            //装備の数は出撃中は別計算
            int nowslotitem;
            if (APIReqMap.SallyDeckPort == 0)
            {
                nowslotitem = APIGetMember.SlotItemsDictionary.Count;
            }
            else
            {
                nowslotitem = APIGetMember.GetSlotitemNumOnSortie();
            }
            if (nowslotitem >= maxslotitem - 3)
            {
                //満杯
                slotitembackcol = PanelWarningBackColor;
            }
            else if (nowslotitem > maxslotitem - 20)
            {
                //イベント要件である-20
                slotitembackcol = PanelCautionBackColor;
            }
            else
            {
                slotitembackcol = PanelDefaultBackColor;
            }
            //装備の数
            CallBacks.SetLabelTextAndColorDouble(labels[1], string.Format("{0} / {1}", nowslotitem, maxslotitem),
                KancolleInfo.DefaultStringColor, slotitembackcol);
        }

        //遠征帰投時間の下表示
        public static string GetToolStripMissionStatus()
        {
            Dictionary<int, List<long>> dic = new Dictionary<int,List<long>>();
            for(int i=0; i<APIPort.DeckPorts.Count; i++)
            {
                List<long> mission = APIPort.DeckPorts[i].api_mission;
                if (mission[2] != 0)//遠征帰投時間
                {
                    dic.Add(i, mission);
                }
            }
            //帰投が早い順にソート
            var query = from x in dic
                        orderby x.Value[2] ascending
                        let z = EpochmsToDate(x.Value[2])
                        let k = APIMaster.GetMissionName((int)x.Value[1])
                        select new { FleetNo = x.Key, MissionID = x.Value[1], MissionName = k, BackTime = z };
            //文字列化
            StringBuilder sb = new StringBuilder();
            int cnt = 1;
            foreach(var x in query)
            {
                if (cnt == 1) sb.Append("遠征 - ");
                string date = (x.BackTime.Day == DateTime.Now.Day) ? x.BackTime.ToString("H:mm") : x.BackTime.ToString("M/d H:mm");
                sb.AppendFormat("[{0}]:{1} {2}", x.FleetNo, x.MissionName, date);
                if (cnt != query.Count()) sb.Append(" - ");
                cnt++;
            }
            return sb.ToString();
        }
        //遠征の左表示
        public static void SetMissionTime(System.Windows.Forms.Label[] mission)
        {
            //遠征名[3], 帰投時間[3]
            //--遠征部分
            //色
            System.Drawing.Color col1 = System.Drawing.Color.FromArgb(41, 98, 79);
            System.Drawing.Color col2 = System.Drawing.Color.FromArgb(105, 34, 79);
            System.Drawing.Color col3 = System.Drawing.Color.FromArgb(105, 98, 15);
            System.Drawing.Color col4 = System.Drawing.Color.FromArgb(169, 34, 15);
            System.Drawing.Color col5 = System.Drawing.Color.FromArgb(41, 162, 15);
            System.Drawing.Color col6 = System.Drawing.Color.FromArgb(41, 34, 143);
            //遠征のデータ取得
            int cnt = 0;
            //第2～4艦隊
            var fleets = from idx in Enumerable.Range(1, APIPort.DeckPorts.Count-1)
                         let x = APIPort.DeckPorts[idx]
                         select x;
            foreach(var x in fleets)
            {
                //遠征のID
                int misid = (int)x.api_mission[1];
                //遠征に出ていない場合
                if(misid <= 0)
                {
                    SetMissionFleetState(cnt + 1, mission[cnt], mission[cnt + 3], null);
                    cnt++;
                    continue;
                }
                //遠征のオブジェクト取得
                ApiMstMission oMission = (from p in APIMaster.MstMissions
                                          where p.api_id == misid
                                          select p).First();
                //エリアの特定と色
                System.Drawing.Color drawColor;
                switch(oMission.api_maparea_id)
                {
                    case 1://鎮守府海域
                        drawColor = col1;
                        break;
                    case 2://南西
                        drawColor = col2;
                        break;
                    case 3://北方
                        drawColor = col3;
                        break;
                    case 4://西方
                        drawColor = col4;
                        break;
                    case 5://南方
                        drawColor = col5;
                        break;
                    default://その他
                        drawColor = col6;
                        break;
                }
                //遠征名のセット
                CallBacks.SetLabelTextAndColor(mission[cnt], oMission.api_name, drawColor);
                //帰投時間のセット
                DateTime endTime = EpochmsToDate(x.api_mission[2]);
                CallBacks.SetLabelTextAndColor(mission[cnt + 3], TimerRemainTime(endTime - DateTime.Now), ControlTextColor);
                CallBacks.SetLabelTag(mission[cnt + 3], endTime);
                cnt++;
            }
        }

        //遠征の遠征に出ていない艦隊のキラ、ドラム数の内部処理
        private static void SetMissionFleetState(int fleetid, System.Windows.Forms.Label missionName, System.Windows.Forms.Label missionTime, System.Windows.Forms.ToolTip tooltip)
        {
            if (fleetid < 0 || fleetid >= APIPort.DeckPorts.Count) return;
            var fleet = APIPort.DeckPorts[fleetid];
            //キラ数・ドラム数・補給状態
            int kira = 0; int totalship = 0;
            int drumnum = 0; int drumsum = 0;
            bool hokyu = true;
            var daihatsu = new List<ApiShip.DaihatsuResult>();
            foreach(var x in fleet.api_ship)
            {
                ApiShip oship;
                if(APIPort.ShipsDictionary.TryGetValue(x, out oship))
                {
                    totalship++;
                    //補給状態
                    hokyu = hokyu & oship.IsFuelMax & oship.IsBullMax;
                    //キラ数
                    if (oship.api_cond >= 50) kira++;
                    //ドラム数
                    var drum = oship.GetDrumNum(APIGetMember.SlotItemsDictionary);
                    if(drum > 0)
                    {
                        drumnum++;
                        drumsum += drum;
                    }
                    //大発
                    daihatsu.Add(oship.GetDaihatsuNum(APIGetMember.SlotItemsDictionary));
                }
            }
            string hokyumes = hokyu ? "OK" : "NG";
            var bonus = ApiShip.DaihatsuResult.ToFleetResult(daihatsu).FleetCappedBonusRatio;
            //表示に反映　キラ4/6 ドラム12/5 補給:OK
            if(tooltip == null)
            {
                //タイマー表示の場合
                CallBacks.SetLabelTextAndColor(missionName, string.Format("キ{0}/{1} ド{2}/{3} 大:{4} 補:{5}", kira, totalship, drumsum, drumnum, bonus.ToString("P0"), hokyumes), ControlTextColor);
            }
            else
            {
                //一般・省スペースの場合
                CallBacks.SetLabelTextAndColor(missionName, string.Format("キ{0}/{1} ド{2}/{3} 補:{4}", kira, totalship, drumsum, drumnum, hokyumes), ControlTextColor);
                CallBacks.SetControlToolTip(tooltip, missionName, null);
            }
            CallBacks.SetLabelTextAndColor(missionTime, "", ControlTextColor);
            CallBacks.SetLabelTag(missionTime, null);
        }

        //一般タブの遠征
        public static void SetMissionTime_General(System.Windows.Forms.Label[][] labels, System.Windows.Forms.ToolTip tooltip)
        {
            //[][0] 遠征名、[][1]時間
            //--遠征部分
            //遠征のデータ取得
            int cnt = 0;
            //第2～4艦隊
            var fleets = from idx in Enumerable.Range(1, APIPort.DeckPorts.Count - 1)
                         let x = APIPort.DeckPorts[idx]
                         select x;
            foreach (var x in fleets)
            {
                //遠征のID
                int misid = (int)x.api_mission[1];
                //遠征に出ていない場合
                if (misid <= 0)
                {
                    SetMissionFleetState(cnt + 1, labels[cnt][0], labels[cnt][1], tooltip);
                    cnt++;
                    continue;
                }
                //遠征のオブジェクト取得
                ApiMstMission oMission = (from p in APIMaster.MstMissions
                                          where p.api_id == misid
                                          select p).First();
                //遠征名のセット
                CallBacks.SetLabelText(labels[cnt][0], oMission.api_name);
                CallBacks.SetControlToolTip(tooltip, labels[cnt][0], oMission.api_details);
                //帰投時間のセット
                DateTime endTime = EpochmsToDate(x.api_mission[2]);
                string endtime_str;
                if(endTime.Date != DateTime.Now.Date) endtime_str = endTime.ToString("M/d ")+endTime.ToShortTimeString();
                else endtime_str = endTime.ToShortTimeString();
                CallBacks.SetLabelText(labels[cnt][1], string.Format("({0}) {1}" ,endtime_str, TimerRemainTime(endTime - DateTime.Now)));
                CallBacks.SetLabelTag(labels[cnt][1], endTime);
                cnt++;
            }
        }

        //入渠の左の時間の表示
        public static void SetNdockTime(System.Windows.Forms.Label[] labels)
        {
            int cnt = 0;
            foreach(ApiNdock ndock in APIPort.Ndocks)
            {
                int shipname_idx = cnt;
                int time_idx = cnt + 4;
                //船が入ってなかったら
                if(ndock.api_state <= 0)
                {
                    CallBacks.SetLabelTextAndColor(labels[shipname_idx], "", ControlTextColor);
                    CallBacks.SetLabelTag(labels[shipname_idx], null);
                    CallBacks.SetLabelTextAndColor(labels[time_idx], "", ControlTextColor);
                    CallBacks.SetLabelTag(labels[time_idx], null);
                    cnt++;
                    continue;
                }
                //船の特定
                ApiShip ship = APIPort.ShipsDictionary[ndock.api_ship_id];
                string name = ship.ShipName;
                //正味所要時間
                TimeSpan estimateTime = TimeSpan.FromMilliseconds(ship.api_ndock_time);
                //完了時間
                DateTime endTime = EpochmsToDate(ndock.api_complete_time);
                //進捗率
                int perc = NdockRemainPercent(endTime, estimateTime);
                //船の名前のセット
                string headertext = string.Format("{0}:Lv{1}", name, ship.api_lv);
                string str = string.Format("{0}:Lv{1} ({2}%)", name, ship.api_lv, perc);
                CallBacks.SetLabelTextAndColor(labels[shipname_idx], str, ControlTextColor);
                CallBacks.SetLabelTag(labels[shipname_idx], new object[] { endTime, estimateTime, headertext }); 
                //時間のセット
                CallBacks.SetLabelTextAndColor(labels[time_idx], TimerRemainTime(endTime - DateTime.Now), ControlTextColor);
                CallBacks.SetLabelTag(labels[time_idx], endTime);
                cnt++;
            }
        }

        //一般タブの入渠表示
        public static void SetNdockTime_General(System.Windows.Forms.Label[][] labels)
        {
            int cnt = 0;
            foreach (ApiNdock ndock in APIPort.Ndocks)
            {
                //船が入ってなかったら
                if (ndock.api_state <= 0)
                {
                    CallBacks.SetLabelText(labels[cnt][0], "");
                    CallBacks.SetLabelTag(labels[cnt][0], null);
                    CallBacks.SetLabelText(labels[cnt][1], "");
                    CallBacks.SetLabelTag(labels[cnt][1], null);
                    cnt++;
                    continue;
                }
                //船の特定
                ApiShip ship = APIPort.ShipsDictionary[ndock.api_ship_id];
                string name = ship.ShipName;
                //正味所要時間
                TimeSpan estimateTime = TimeSpan.FromMilliseconds(ship.api_ndock_time);
                //完了時間
                DateTime endTime = EpochmsToDate(ndock.api_complete_time);
                //進捗率
                int perc = NdockRemainPercent(endTime, estimateTime);
                //船の名前のセット
                string headertext = string.Format("{0}", name);
                string str = string.Format("{0} ({1}%)", name, perc);
                CallBacks.SetLabelText(labels[cnt][0], str);
                CallBacks.SetLabelTag(labels[cnt][0], new object[] { endTime, estimateTime, headertext });
                //時間のセット
                string endtime_str;
                if (endTime.Date != DateTime.Now.Date) endtime_str = endTime.ToString("M/d ") + endTime.ToShortTimeString();
                else endtime_str = endTime.ToShortTimeString();
                CallBacks.SetLabelText(labels[cnt][1], string.Format("({0}) {1}", endtime_str, TimerRemainTime(endTime - DateTime.Now)));
                CallBacks.SetLabelTag(labels[cnt][1], endTime);
                cnt++;
            }
        }


        //HPの損傷度合い
        /*
        public enum HPCondition
        {
            Full, None, SmallDamage, MiddleDamage, HeavyDamage, Sank, Bathing, Withdrawn,
        }*/


        //エポックミリ秒→DateTime
        public static DateTime EpochmsToDate(long epoch)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(epoch);
            DateTime dt = EPOCH_TIME + ts + new TimeSpan(9, 0, 0);//UTCとの時差
            return dt;
        }

        //遠征の帰投時間の表示用
        public static string TimerRemainTime(TimeSpan ts)
        {
            return string.Format("{0}:{1}:{2}", (ts.Days * 24 + ts.Hours).ToString("D2"), ts.Minutes.ToString("D2"), ts.Seconds.ToString("D2"));
        }

        //入渠時間のパーセンテージ
        public static int NdockRemainPercent(DateTime endTime, TimeSpan estimateTime)
        {
            //残り時間
            TimeSpan remainTime = endTime - DateTime.Now - new TimeSpan(0, 1, 0);
            double remainSec = Math.Max(remainTime.TotalSeconds, 0.0);
            //残り％
            double percent = (1 - remainSec / estimateTime.TotalSeconds) * 100;
            return (int)percent;
        }

        //別ウィンドウでグラフ表示
        public static void ShowChartViewer(int target, Form1 mainScreen)
        {
            if (!HistoricalData.IsInited) return;
            //if (HistoricalData.GraphInfoMaterial.Mode == 0) return;
            //if (HistoricalData.GraphInfoExperience.Mode == 0) return;
            //表示されていない場合
            if (!Config.ShowSubWindowGraph)
            {
                chartViewer = new ChartViewer(target);
                chartViewer.FormClosed += new System.Windows.Forms.FormClosedEventHandler(chartViewer_FormClosed);
                chartViewer.Owner = mainScreen;
                chartViewer.Show();
                CallBacks.SetToolStripMenuItemChecked(mainScreen.toolStripMenuItem19, mainScreen.toolStripMenuItem19.GetCurrentParent(), true);
                Config.ShowSubWindowGraph = true;
            }
            else
            {
                chartViewer.Close();
            }
        }

        private static void chartViewer_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Config.ShowSubWindowGraph = false;
            Form1 mainscreen = (sender as ChartViewer).Owner as Form1;
            if(mainscreen != null)
            {
                CallBacks.SetToolStripMenuItemChecked(mainscreen.toolStripMenuItem19, mainscreen.toolStripMenuItem19.GetCurrentParent(), false);
            }
        }

    }

}
