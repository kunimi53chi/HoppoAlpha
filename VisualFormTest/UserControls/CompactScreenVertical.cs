using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;

namespace VisualFormTest.UserControls
{
    public partial class CompactScreenVertical : UserControl
    {
        public Label[][] Mission { get; set; }
        public Label[][] Ndock { get; set; }
        public Label[] Material { get; set; }
        public Label[] Num { get; set; }
        public Label[] Senka { get; set; }
        public Label BattleView { get; set; }
        public Label[][] Fleet { get; set; }
        public Label[] Condition { get; set; }

        public bool IsShown { get; set; }

        private bool IsInited { get; set; }

        public CompactScreenVertical()
        {
            InitializeComponent();
        }

        #region CompactScreenからのコピペ
                public void Init()
        {
            if (IsInited) return;

            //--ハンドラーのセット
            Mission = new Label[][]
            {
                new Label[]{label_mission_11, label_mission_12},
                new Label[]{label_mission_21, label_mission_22},
                new Label[]{label_mission_31, label_mission_32},
            };
            Ndock = new Label[][]
            {
                new Label[]{label_ndock_11, label_ndock_12},
                new Label[]{label_ndock_21, label_ndock_22},
                new Label[]{label_ndock_31, label_ndock_32},
                new Label[]{label_ndock_41, label_ndock_42},
            };
            Material = new Label[] { label_material_1, label_material_2, label_material_3, label_material_4, label_material_6, label_material_5, label_material_7 };//開発資材とバケツが逆らしいので入れ替える
            Num = new Label[] { label_num_1, label_num_2 };
            Senka = new Label[] { label_senka_1, label_senka_2 };
            BattleView = label_battleview;
            Fleet = new Label[][]
            {
                new Label[]{label_fleet_11, label_fleet_12},
                new Label[]{label_fleet_21, label_fleet_22},
                new Label[]{label_fleet_31, label_fleet_32},
                new Label[]{label_fleet_41, label_fleet_42},
                new Label[]{label_fleet_51, label_fleet_52},
                new Label[]{label_fleet_61, label_fleet_62},
            };
            Condition = new Label[] { label_cond_1, label_cond_2, label_cond_3 };

            //DoubleBuffered
            foreach (var x in Mission) LocalDoubleBuffering(x[1]);
            foreach (var x in Ndock) LocalDoubleBuffering(x[1]);
            LocalDoubleBuffering(tableLayoutPanel1);
            LocalDoubleBuffering(tableLayoutPanel2);

            timer1.Start();

            IsInited = true;
        }

        private void LocalDoubleBuffering(Control control)
        {
            control.GetType().InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               control,
               new object[] { true });
        }

        //遠征のアップデート
        public void MissionUpdate_Q()
        {
            if (!IsShown) return;
            //一般タブのを流用
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfo.SetMissionTime_General(Mission, toolTip1);
                }));
        }

        //入渠のアップデート
        public void NdockUpdate_Q()
        {
            if (!IsShown) return;
            //一般タブのを流用
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
            {
                KancolleInfo.SetNdockTime_General(Ndock);
            }));
        }

        //資源のアップデート
        public void MaterialUpdate_Q()
        {
            if (!IsShown) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    //ロジック
                    MethodInvoker logic = delegate()
                    {
                        //4,6の高速建造を外す
                        int[] now = Enumerable.Range(0, APIPort.Materials.Count).Where(x => x != 4).Select(x => APIPort.Materials[x].api_value).ToArray();
                        foreach (var i in Enumerable.Range(0, Material.Length)) Material[i].Text = now[i].ToString("N0");
                    };
                    if (this.InvokeRequired) this.Invoke(logic);
                    else logic.Invoke();
                }));
        }

        //艦娘の数
        public void NumUpdate_Q()
        {
            if (!IsShown) return;
            //一般タブの流用
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfo.SetShipSlotitemNum_General(Num);
                    foreach (var x in Num) if (x.BackColor == System.Drawing.Color.White) x.BackColor = System.Drawing.Color.Transparent;
                }));
        }

        //戦果のアップデート
        public void SenkaUpdate_Q()
        {
            if (!IsShown) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    //ロジック
                    MethodInvoker logic = delegate()
                    {
                        Senka[0].Text = APIPort.Basic.api_experience.ToString("N0");
                        if (HistoricalData.LogSenka.Count > 0)
                        {
                            SenkaRecord senkabase = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
                            if (senkabase.EndSenkaEst < 0) Senka[1].Text = "NA";
                            else Senka[1].Text = senkabase.EndSenkaEst.ToString("N1");
                        }
                        else Senka[1].Text = "NA";
                    };
                    if (this.InvokeRequired) this.Invoke(logic);
                    else logic.Invoke();
                }));
        }

        //戦況のアップデート
        public void BattleUpdate_Q()
        {
            if (!IsShown) return;
            //一般タブの流用
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
            {
                try
                {
                    KancolleBattle.SetBattleView_General(BattleView, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "エラーが発生しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));
        }

        //艦隊のアップデート
        delegate void FleetUpdateCallBack();
        public void FleetUpdate_Q()
        {
            if (!IsShown) return;
            if (APIPort.Ndocks == null) return;

            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
            {
                if (this.InvokeRequired)
                {
                    FleetUpdateCallBack d = new FleetUpdateCallBack(FleetUpdate_Q);
                    this.Invoke(d);
                }
                else
                {
                    int airsup = 0;
                    List<HoppoAlpha.DataLibrary.SearchModel.UnitSearchResult> searches = new List<HoppoAlpha.DataLibrary.SearchModel.UnitSearchResult>();
                    bool issupply = true;
                    int[] baths = APIPort.Ndocks.Select(x => x.api_ship_id).ToArray();
                    //第1艦隊
                    foreach (var i in Enumerable.Range(0, APIPort.DeckPorts[0].api_ship.Count))
                    {
                        ApiShip oship = APIPort.ShipsDictionary.ContainsKey(APIPort.DeckPorts[0].api_ship[i]) ? APIPort.ShipsDictionary[APIPort.DeckPorts[0].api_ship[i]] : null;
                        if (oship != null)
                        {
                            var hpcond = oship.GetHPCondition(Array.IndexOf(baths, oship.api_id) != -1, APIPort.IsWithdrawn[0, i], Config.BucketHPRatio, APIGetMember.SlotItemsDictionary);
                            //var dship = APIMaster.MstShips[oship.api_ship_id];
                            string strcond = hpcond.Display();
                            System.Drawing.Color backs;
                            if (oship.api_cond >= 50) backs = Color.SpringGreen;
                            else if (oship.api_cond >= 40) backs = Color.Transparent;
                            else if (oship.api_cond >= 30) backs = Color.Yellow;
                            else if (oship.api_cond >= 20) backs = Color.LightSalmon;
                            else backs = Color.Red;

                            Fleet[i][0].Text = string.Format("{0}({1})", oship.ShipName, oship.api_cond);
                            Fleet[i][0].BackColor = backs;
                            toolTip1.SetToolTip(Fleet[i][0], Helper.MakeUnitToolTip(oship));

                            Fleet[i][1].Text = string.Format("{0}/{1} {2}", oship.api_nowhp, oship.api_maxhp, strcond != "" ? string.Format("[{0}]", strcond) : strcond);
                            Fleet[i][1].ForeColor = hpcond.GetColor();
                            Fleet[i][1].BackColor = hpcond.GetBackColor() == System.Drawing.Color.White ? System.Drawing.Color.Transparent : hpcond.GetBackColor();

                            var airsupresult = oship.GetAirSupValue(APIGetMember.SlotItemsDictionary, APIPort.IsWithdrawn, APIPort.DeckPorts);
                            airsup += airsupresult.AirSupValueMax;
                            searches.Add(KancolleInfoFleet.UnitSearchCenter(oship));
                            issupply = issupply && (oship.IsFuelMax && oship.IsBullMax);
                        }
                        else
                        {
                            toolTip1.SetToolTip(Fleet[i][0], null);
                            Fleet[i][0].Text = "";
                            Fleet[i][0].BackColor = Color.Transparent;

                            Fleet[i][1].Text = "";
                            Fleet[i][1].ForeColor = Label.DefaultForeColor; Fleet[i][1].BackColor = Label.DefaultBackColor;
                        }
                    }
                    //制空等
                    Condition[0].Text = issupply ? "OK" : "NG";
                    Condition[1].Text = airsup.ToString();
                    Condition[2].Text = KancolleInfoFleet.FleetSearchCenter(searches).ToString("N1");
                }
            }));
        }

        //タイマー
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!IsShown) return;
            if (Mission == null || Ndock == null) return;
            //タイマーの更新
            //遠征のタイマー
            foreach (int i in Enumerable.Range(0, Mission.Length))
            {
                //遠征に出ていない場合
                if (Mission[i][1].Tag == null) continue;
                else
                {
                    DateTime endTime = (DateTime)Mission[i][1].Tag;
                    TimeSpan ts = endTime - DateTime.Now;
                    if (ts < new TimeSpan()) ts = new TimeSpan();//マイナスの時間を打ち切り
                    string endtime_str;
                    if (endTime.Date != DateTime.Now.Date) endtime_str = endTime.ToString("M/d ") + endTime.ToShortTimeString();
                    else endtime_str = endTime.ToShortTimeString();
                    Mission[i][1].Text = string.Format("({0}) {1}", endtime_str, KancolleInfo.TimerRemainTime(ts));
                }
            }
            //入渠のタイマー
            foreach (int i in Enumerable.Range(0, Ndock.Length))
            {
                //入渠してない場合
                if (Ndock[i][0].Tag == null) continue;
                else
                {
                    //船の名前
                    object[] tags = (object[])Ndock[i][0].Tag;
                    DateTime endtime = (DateTime)tags[0];
                    TimeSpan estimatetime = (TimeSpan)tags[1];
                    string headertext = (string)tags[2];
                    Ndock[i][0].Text = string.Format("{0} ({1}%)", headertext, KancolleInfo.NdockRemainPercent(endtime, estimatetime));
                    //入渠時間
                    if (Ndock[i][1].Tag == null) continue;
                    DateTime endTime = (DateTime)Ndock[i][1].Tag;
                    TimeSpan ts = endTime - DateTime.Now;
                    if (ts < new TimeSpan()) ts = new TimeSpan();//マイナスの時間を打ち切り
                    string endtime_str;
                    if (endTime.Date != DateTime.Now.Date) endtime_str = endTime.ToString("M/d ") + endTime.ToShortTimeString();
                    else endtime_str = endTime.ToShortTimeString();
                    Ndock[i][1].Text = string.Format("({0}) {1}", endtime_str, KancolleInfo.TimerRemainTime(ts));
                }
            }
        }
        #endregion

    }
}
