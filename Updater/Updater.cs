using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Updater
{
    public partial class Updater : Form
    {

        public Updater()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string downloadpage = @"http://nekokan333.web.fc2.com/hoppoalpha.html";
            //最新バージョン
            string latest_version = webBrowser1.Document.GetElementById("latestversion").GetAttribute("value");
            //本体のバージョン
            System.Diagnostics.FileVersionInfo ver =
                System.Diagnostics.FileVersionInfo.GetVersionInfo("hoppoalpha.exe");
            string now_version = ver.FileVersion;

            //分割
            var currentSplit = now_version.Trim().Split('.');
            var latestSplit = latest_version.Trim().Split('.');

            //数字に変換
            int currentVersionLong = 0; int latestVersionLong = 0;
            foreach(var i in Enumerable.Range(0, Math.Min(currentSplit.Length, latestSplit.Length)))
            {
                int current, latest;
                int.TryParse(currentSplit[i], out current);
                int.TryParse(latestSplit[i], out latest);

                currentVersionLong = currentVersionLong * 100 + current;
                latestVersionLong = latestVersionLong * 100 + latest;
            }

            //バージョンが最新版だったら
            if(latestVersionLong <= currentVersionLong)
            {
                label1.Text += "最新版です";
            }
            else
            {
                label1.Text += string.Format("ver{0}が検出されました", latest_version);
                //ダウンロードページを開く場合
                if (DialogResult.Yes == MessageBox.Show(string.Format("最新版 {0} が公開されています\nダウンロードページを開きますか？", latest_version),
                    "アップデート", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    System.Diagnostics.Process.Start(downloadpage);
                }
            }
            
            //レジストリの作成
            Microsoft.Win32.RegistryKey regkey_render = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                 @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
            string keyname = "hoppoalpha.exe";
            if (regkey_render.GetValue(keyname) == null)
            {
                regkey_render.SetValue(keyname, 11000, Microsoft.Win32.RegistryValueKind.DWord);
            }
            regkey_render.Close();

            //閉じる
            this.Close();
        }
    }
}
