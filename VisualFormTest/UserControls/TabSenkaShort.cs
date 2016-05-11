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
    public partial class TabSenkaShort : UserControl
    {
        Label[] label_senka_admiralexp, label_senka_timertick;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }
        public bool IsShown { get; set; }

        public TabSenkaShort()
        {
            InitializeComponent();

            //タイマー
            admiralExpTimer.Start();
        }

        //戦果タブの初期化
        public void Init()
        {
            //戦果タブ
            label_senka_admiralexp = new Label[]
            {
                label_senka_admiral_5, label_senka_admiral_6, label_senka_admiral_7,
            };
            label_senka_timertick = new Label[]
            {
                label_senka_admiral_5, label_senka_admiral_6,
            };

            InitFinished = true;
        }

        public void Init2()
        {
            Init2Finished = true;
        }

        //戦果タブの更新
        public void TabSenkaUpdate()
        {
            if (!IsShown || !InitFinished) return;
            Task.Factory.StartNew(() =>
            {
                KancolleInfoSenka.SetAdmiralExp_Short(label_senka_admiralexp);
            });
        }

        //タイマー
        private void admiralExpTimer_Tick(object sender, EventArgs e)
        {
            if (!IsShown || !InitFinished) return;
            KancolleInfoSenka.TimerRefreshAdmiralExp(label_senka_timertick);
        }

        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(this, "ssenka");
        }
    }
}
