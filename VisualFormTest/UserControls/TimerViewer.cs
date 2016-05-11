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
    public partial class TimerViewer : UserControl
    {
        public Label[] Labels { get; set; }
        public Label[] MissionLabels { get; set; }
        public Label[] NdockLabels { get; set; }

        public TimerViewer()
        {
            InitializeComponent();
            this.ClientSize = panel3.Size;
            //タイマー
            Labels = new Label[]{ label_quest_timer_1, label_quest_timer_2, label_quest_timer_3,
                                label_ndock_timer_1, label_ndock_timer_2, label_ndock_timer_3, label_ndock_timer_4,
                                label_ndock_name_1, label_ndock_name_2, label_ndock_name_3, label_ndock_name_4};//11,l13,15→遠征 21,23,25,27→入渠 タイマー, 20,22,24,26→名前と％
            MissionLabels = new Label[]{label_quest_name_1, label_quest_name_2, label_quest_name_3,
                                        label_quest_timer_1, label_quest_timer_2, label_quest_timer_3};
            NdockLabels = new Label[]{label_ndock_name_1, label_ndock_name_2, label_ndock_name_3, label_ndock_name_4,
                                        label_ndock_timer_1, label_ndock_timer_2, label_ndock_timer_3, label_ndock_timer_4};
            missionNdockTimer.Start();
        }

        //遠征情報のセット
        public void SetMissionTime_Q()
        {
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfo.SetMissionTime(MissionLabels);
                }));
        }

        //入渠情報のセット
        public void SetNdockTime_Q()
        {
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfo.SetNdockTime(NdockLabels);
                }));
        }

        //遠征・入渠表示のタイマーイベント
        private void missionNdockPanelTimer_Tick(object sender, EventArgs e)
        {
            foreach (Label x in Labels)
            {
                //タグがなかったら何もしない
                if (x.Tag == null)
                {
                    continue;
                }
                else
                {
                    //遠征と入渠のタイマーだったら
                    if (x.Tag is DateTime)
                    {
                        DateTime endTime = (DateTime)x.Tag;
                        TimeSpan ts = endTime - DateTime.Now;
                        if (ts < new TimeSpan()) ts = new TimeSpan();//マイナスの時間を打ち切り
                        x.Text = KancolleInfo.TimerRemainTime(ts);
                    }
                    //入渠の名前だったら
                    else if (x.Tag is object[])
                    {
                        object[] tags = (object[])x.Tag;
                        DateTime endtime = (DateTime)tags[0];
                        TimeSpan estimatetime = (TimeSpan)tags[1];
                        string headertext = (string)tags[2];
                        x.Text = string.Format("{0} ({1}%)", headertext, KancolleInfo.NdockRemainPercent(endtime, estimatetime));
                    }
                }
            }
        }
    }
}
