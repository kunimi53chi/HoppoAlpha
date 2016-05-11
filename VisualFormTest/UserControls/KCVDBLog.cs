using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualFormTest.KancolleVerifyDb;

namespace VisualFormTest.UserControls
{
    public partial class KCVDBLog : UserControl
    {
        private Label[][] label_history;
        private PictureBox[] pbox_history;
        private System.Timers.Timer timer;

        public bool IsShown { get; set; }

        public KCVDBLog()
        {
            InitializeComponent();

            label_history = new Label[][]
            {
                new Label[]{label_h_time1, label_h_parent1, label_h_sub1},
                new Label[]{label_h_time2, label_h_parent2, label_h_sub2},
                new Label[]{label_h_time3, label_h_parent3, label_h_sub3},
                new Label[]{label_h_time4, label_h_parent4, label_h_sub4},
                new Label[]{label_h_time5, label_h_parent5, label_h_sub5},
                new Label[]{label_h_time6, label_h_parent6, label_h_sub6},
                new Label[]{label_h_time7, label_h_parent7, label_h_sub7},
            };
            pbox_history = new PictureBox[]
            {
                pictureBox_h_1, pictureBox_h_2, pictureBox_h_3, pictureBox_h_4,
                pictureBox_h_5, pictureBox_h_6, pictureBox_h_7,
            };

            timer = new System.Timers.Timer();
            timer.Interval = Config.KancolleVerifyScreenRefreshTimer;
            timer.SynchronizingObject = this;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!KCVDBObjects.ScreenRefreshRequired) return;
            if (!IsShown) return;

            //再描画の必要がある場合
            UIMethods.UIQueue.Enqueue(new UIMethods(new Action(() =>
            {
                //閉塞
                KCVDBObjects.AutoResetEventViewSwitcher(false);

                //--有効かどうか
                if (Config.KancolleVerifyPostEnable) pictureBox_enable.Image = Properties.Resources._091;//青
                else pictureBox_enable.Image = Properties.Resources._093;//赤

                //--概況
                if (KCVDBObjects.SessionID == new Guid())
                {
                    label_g_sessionid.Text = "The client has not been initialized yet.";
                    label_g_started.Text = "-";
                }
                else
                {
                    label_g_sessionid.Text = KCVDBObjects.SessionID.ToString();
                    label_g_started.Text = KCVDBObjects.SessionStarted.ToString();
                }
                label_g_sendapi.Text = KCVDBObjects.SendApis.ToString("N0");
                label_g_success.Text = KCVDBObjects.SendSuccess.ToString("N0");
                label_g_failed.Text = KCVDBObjects.SendFailure.ToString("N0");
                label_g_total.Text = KCVDBObjects.TotalKBytes.ToString("N0");
                label_g_kbperh.Text = KCVDBObjects.PerHourKBytes.Sum().ToString("N0");

                //--API履歴
                int i = pbox_history.Length - 1;
                foreach (var h in KCVDBObjects.ApiHistory)
                {
                    if (i < 0) break;

                    //画像
                    switch (h.SendStatus)
                    {
                        case KCVDBSendStatus.FatalError:
                        case KCVDBSendStatus.InternalError:
                            pbox_history[i].Image = Properties.Resources._093;//赤色
                            break;
                        case KCVDBSendStatus.Queuing:
                        case KCVDBSendStatus.Sending:
                            pbox_history[i].Image = Properties.Resources._091;//青色
                            break;
                        case KCVDBSendStatus.SendingError:
                            pbox_history[i].Image = Properties.Resources._094;//黄色
                            break;
                        case KCVDBSendStatus.Success:
                            pbox_history[i].Image = Properties.Resources._092;//緑色;
                            break;
                        default:
                            pbox_history[i].Image = null;//何も表示しない
                            break;
                    }

                    if (h.SendStatus == KCVDBSendStatus.None)
                    {
                        foreach (var l in label_history[i]) l.Text = "";
                    }
                    else
                    {
                        //時間
                        label_history[i][0].Text = h.Date.ToString("HH:mm:ss");

                        //パス
                        string first = "", second = "";
                        if (h.Url != null)
                        {
                            var split = h.Url.PathAndQuery.Split('/');
                            if (split.Length >= 3) first = split[2];
                            if (split.Length >= 4) second = split[3];
                        }
                        label_history[i][1].Text = first;
                        label_history[i][2].Text = second;
                    }

                    i--;
                }

                //閉塞解放
                KCVDBObjects.AutoResetEventViewSwitcher(true);

                //再描画のフラグをオフに
                KCVDBObjects.ScreenRefreshRequired = false;
            })));
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItem_enable.Checked = Config.KancolleVerifyPostEnable;
        }

        private void toolStripMenuItem_enable_Click(object sender, EventArgs e)
        {
            Config.KancolleVerifyPostEnable = !toolStripMenuItem_enable.Checked;//チェックの切り替え
            toolStripMenuItem_enable.Checked = Config.KancolleVerifyPostEnable;
            KancolleVerifyDb.KCVDBObjects.ScreenRefreshRequired = true;//再描画させる
        }

        public void SetTimerInterval()
        {
            timer.Interval = Config.KancolleVerifyScreenRefreshTimer;
        }
    }
}
