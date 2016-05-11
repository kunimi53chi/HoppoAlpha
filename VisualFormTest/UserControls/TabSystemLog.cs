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
    public partial class TabSystemLog : UserControl, ITabControl
    {
        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        private int Caret { get; set; }

        public TabSystemLog()
        {
            InitializeComponent();
        }

        public void Init()
        {
            InitFinished = true;
        }

        public void Init2()
        {
            Init2Finished = true;
        }

        public void UpdateSystemLog()
        {
            if (!InitFinished) return;
            //追加するログの取得
            var applogs = Enumerable.Range(0, LogSystem.LogMessage.Count).Skip(this.Caret).Select(x => LogSystem.LogMessage[x]);
            if (applogs.Count() == 0) return;

            string appendtext = string.Join(Environment.NewLine, applogs);
            
            //キャレットの設定
            this.Caret = LogSystem.LogMessage.Count;

            Task.Factory.StartNew(() =>
            {
                CallBacks.SetTextBoxTextAppend(textBox_systemlog, Environment.NewLine + appendtext);
            });
        }

        //苦し紛れで（システムログの追加のタイマーイベント）
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (LogSystem.LogMessage == null) return;
            if (LogSystem.LogMessage.Count <= this.Caret) return;

            UpdateSystemLog();
        }
    }
}
