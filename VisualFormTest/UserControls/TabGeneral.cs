using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest.UserControls
{
    public partial class TabGeneral : UserControl, ITabControl
    {
        //コントロール
        public TextBox TaihaChecker { get; set; }
        public Label[] Exp { get; set; }
        public Label[] ShipNum { get; set; }
        public Label[][] Mission { get; set; }
        public Label[][] Ndock { get; set; }
        public Label[] Quest { get; set; }
        public Label[] Practice { get; set; }

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        public TabGeneral()
        {
            InitializeComponent();

        }

        //初期化
        public void Init()
        {
            //一般タブ
            TaihaChecker = textBox_general_taihachecker;
            Exp = new Label[] { label_general_exp1, label_general_exp2 };
            ShipNum = new Label[] { label_general_unit1, label_general_unit2 };
            Mission = new Label[][]{
                new Label[]{label_general_mission11, label_general_mission12},
                new Label[]{label_general_mission21, label_general_mission22},
                new Label[]{label_general_mission31, label_general_mission32},
            };
            Ndock = new Label[][]{
                new Label[]{label_general_ndock_11, label_general_ndock_12},
                new Label[]{label_general_ndock_21, label_general_ndock_22},
                new Label[]{label_general_ndock_31, label_general_ndock_32},
                new Label[]{label_general_ndock_41, label_general_ndock_42},
            };
            Quest = new Label[] { label_general_quest1, label_general_quest2, label_general_quest3, label_general_quest4, label_general_quest5, label_general_quest6 };
            Practice = new Label[] { label_general_practice1, label_general_practice2, label_general_practice3, label_general_practice4, label_general_practice5 };

            timer_second.Start();

            InitFinished = true;
        }

        public void Init2()
        {
            Init2Finished = true;
        }

        //大破チェッカーの更新
        public void TabGeneral_TaihaCheckerUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleBattle.SetSankWarning(TaihaChecker);
                }));
        }

        //簡易戦況の更新
        public void TabGeneral_BattleViewUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    try
                    {
                        KancolleBattle.SetBattleView_General(label_general_battleview, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "エラーが発生しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }));
        }

        //提督経験値の更新
        public void TabGeneral_AdmiralExpUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfoSenka.SetAdmiralExp_General(Exp);
                }));
        }

        //艦娘の数
        public void TabGeneral_ShipSlotitemNumUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfo.SetShipSlotitemNum_General(ShipNum);
                }));
        }

        //遠征の更新
        public void TabGeneral_MissionUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfo.SetMissionTime_General(Mission, toolTip1);
                }));
        }

        //入渠の更新
        public void TabGeneral_NdockUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfo.SetNdockTime_General(Ndock);
                }));
        }

        //任務の更新
        public void TabGeneral_QuestUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfoGeneral.ShowQuests(Quest, toolTip1);
                }));
        }


        //演習の更新
        public void TabGeneral_PracticeUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfoGeneral.SetPractice(Practice, toolTip1);
                }));
        }


        //デイリー用のタイマー
        private void timer_second_Tick(object sender, EventArgs e)
        {
            if (!InitFinished) return;
            DateTime now = DateTime.Now;
            //デイリー更新のタイマー
            if (label_general_missiontime.Tag == null || (DateTime)label_general_missiontime.Tag <= now)
            {
                //次の更新時間を計算
                DateTime mission_refresh;
                DateTime refresh1 = DateTime.Today + new TimeSpan(5, 0, 0);//今日の5時
                DateTime refresh2 = refresh1.AddDays(1);//次の日の5時
                if (now < refresh1) mission_refresh = refresh1;
                else mission_refresh = refresh2;
                //タグに設置
                label_general_missiontime.Tag = mission_refresh;
            }
            label_general_missiontime.Text = "デイリー更新まで " + ((DateTime)label_general_missiontime.Tag - now).ToString(@"hh\:mm");
            //遠征のタイマー
            foreach(int i in Enumerable.Range(0, Mission.Length))
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
            foreach(int i in Enumerable.Range(0, Ndock.Length))
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

        private void textBox_general_taihachecker_Enter(object sender, EventArgs e)
        {
            textBox_general_taihachecker.SelectionStart = textBox_general_taihachecker.Text.Length;
        }

    }
}
