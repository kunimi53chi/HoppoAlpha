using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DLLのバージョン
            if (CheckDataLibraryVersion())
            {
                MessageBox.Show("HoppoAlpha.DataLibrary.dllのバージョンと、本体のバージョンが一致しません\nバージョンを確認してください");
                Environment.Exit(0);
            }
            //アップデートのチェック
            CheckUpdates();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        //アップデートのチェック
        static void CheckUpdates()
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("Updater.exe");
            p.WaitForExit();
        }

        //DLLのバージョンチェック
        static bool CheckDataLibraryVersion()
        {
            //DLLのサポートバージョン
            string maxVersionStr = HoppoAlpha.DataLibrary.Version.SupportedHoppoAlphaMaxVersion;
            //自分自身のバージョン情報を取得する
            System.Diagnostics.FileVersionInfo ver =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            //数値に比較
            int maxVersion = Convert.ToInt32(maxVersionStr.Replace(".", ""));
            int version = Convert.ToInt32(ver.FileVersion.Replace(".", ""));
            return version > maxVersion;
        }
    }
}
