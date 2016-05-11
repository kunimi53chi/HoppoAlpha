using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace VisualFormTest.KancolleVerifyDb
{
    public partial class NotifyDialog : Form
    {
        DockingWindows.DockWindowTabCollection collection;

        public NotifyDialog(DockingWindows.DockWindowTabCollection tabs)
        {
            InitializeComponent();

            collection = tabs;
        }

        private void button_yes_Click(object sender, EventArgs e)
        {
            Config.KancolleVerifyPostEnable = true;
            Config.KancolleVerifyNotifyDialogNotShow = true;
            collection.ToolStripHandler.TKCVDBLog.PerformClick();//はいがクリックされたら通信ログ画面表示する

            this.Close();
        }

        private void button_no_Click(object sender, EventArgs e)
        {
            Config.KancolleVerifyPostEnable = false;
            Config.KancolleVerifyNotifyDialogNotShow = true;//ダイアログは初回のみ表示
            this.Close();
        }

        private void linkLabel_url_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel_url.Text);
        }
    }
}
