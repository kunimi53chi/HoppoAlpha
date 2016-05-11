using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest
{
    public partial class LogConvert : Form
    {
        private bool isExecute = false;
        private System.Threading.CancellationTokenSource token = null;

        public LogConvert()
        {
            InitializeComponent();

            ButtonExecuteEnable(isExecute);

            //アウトプットディレクトリ
            if (Config.LogExportOutputDirectory != null && Directory.Exists(Config.LogExportOutputDirectory)) textBox_folder.Text = Config.LogExportOutputDirectory;
            else
            {
                textBox_folder.Text = Environment.CurrentDirectory;
                Config.LogExportOutputDirectory = Environment.CurrentDirectory;
            }
            textBox_folder.SelectionStart = textBox_folder.Text.Length;
        }


        //ファイルの追加チェック
        private void CheckFileAndAppendToListView(string[] files)
        {
            List<ListViewItem> items = new List<ListViewItem>();
            //ファイルチェック
            foreach (string x in files ?? new string[0])
            {
                //フォルダだったら
                if (!File.Exists(x)) continue;

                //datファイルじゃなかったら
                string extension = Path.GetExtension(x).Replace(".", "");
                if (extension != "dat") continue;

                //タイプを識別
                string filename = Path.GetFileName(x);
                ListViewItem lvi = new ListViewItem(filename);

                if (filename.Contains("experience")) lvi.SubItems.Add("経験値");
                else if (filename.Contains("material")) lvi.SubItems.Add("資材");
                else if (filename.Contains("ranking")) lvi.SubItems.Add("ランキング");
                else if (filename.Contains("senka")) lvi.SubItems.Add("戦果");
                else if (filename.Contains("enemyfleet")) lvi.SubItems.Add("敵編成");
                else if (filename.Contains("droprecord")) lvi.SubItems.Add("ドロップ");
                else continue;

                //フルパスを追加
                lvi.SubItems.Add(x);

                items.Add(lvi);
            }

            if (items.Count > 0)
            {
                listView_files.Items.AddRange(items.ToArray());
            }
        }

        //サブディレクトリのファイルの取得
        private List<string> GetSubDirectory(string dir)
        {
            List<string> filelist = new List<string>();
            string[] files = Directory.GetFiles(dir);
            filelist.AddRange(files);

            string[] dirs = Directory.GetDirectories(dir);
            foreach (string s in dirs)
            {
                filelist.AddRange(GetSubDirectory(s)); ;
            }
            return filelist;
        }

        //処理中のボタンの切り替え
        private void ButtonExecuteEnable(bool isExecuting)
        {
            button_folderef.Enabled = !isExecuting;

            button_selectedclear.Enabled = !isExecuting;
            button_appendall.Enabled = !isExecuting;
            button_allclear.Enabled = !isExecuting;

            checkBox_fullmode.Enabled = !isExecuting;

            button_start.Enabled = !isExecuting;
            button_cancel.Enabled = isExecuting;//これだけ違う
        }

        //イベントハンドラー
        #region イベントハンドラー
        //出力フォルダの選択
        private void button_folderef_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Config.LogExportOutputDirectory)) folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;
            else folderBrowserDialog1.SelectedPath = Config.LogExportOutputDirectory;

            if(folderBrowserDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_folder.Text = folderBrowserDialog1.SelectedPath;
                Config.LogExportOutputDirectory = folderBrowserDialog1.SelectedPath;
            }
        }

        //リストビューにD&D
        private void listView_files_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            }
        }

        private void listView_files_DragDrop(object sender, DragEventArgs e)
        {
            string[] originalfiles = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
            CheckFileAndAppendToListView(originalfiles);
        }

        //リストビューの選択をクリア
        private void button_selectedclear_Click(object sender, EventArgs e)
        {
            if(listView_files.SelectedItems.Count > 0)
            {
                listView_files.BeginUpdate();

                foreach(ListViewItem item in listView_files.SelectedItems)
                {
                    listView_files.Items.Remove(item);
                }

                listView_files.EndUpdate();
            }
        }

        //フォルダごと追加
        private void button_appendall_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;

            if (folderBrowserDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                var filelist = GetSubDirectory(folderBrowserDialog1.SelectedPath);
                CheckFileAndAppendToListView(filelist.ToArray());
            }
        }

        //リストビューの全てクリア
        private void button_allclear_Click(object sender, EventArgs e)
        {
            listView_files.Items.Clear();
        }

        //出力開始
        private void button_start_Click(object sender, EventArgs e)
        {
            if(listView_files.Items.Count == 0) return;
            //ソースファイルの作成
            string[] srcfile = listView_files.Items.OfType<ListViewItem>().Select(x => x.SubItems[2].Text).ToArray();

            var logic = new LogConvertLogic(srcfile, textBox_folder.Text, checkBox_fullmode.Checked);

            isExecute = true;
            ButtonExecuteEnable(true);

            token = new System.Threading.CancellationTokenSource();

            logic.Execute(new Progress<string>(prog =>
                {
                    label_prog.Text = prog;
                }), token).ContinueWith(_ =>
                {
                    isExecute = false;
                    ButtonExecuteEnable(false);
                    MessageBox.Show("ログのコンバートが完了しました");
                }, token.Token, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
        }

        //キャンセル
        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (token != null) token.Cancel();
        }

        //フォルダを開く
        private void button_openfolder_Click(object sender, EventArgs e)
        {
            string path = textBox_folder.Text;
            if (Directory.Exists(path)) System.Diagnostics.Process.Start(path);
        }

        //処理中はフォームを閉じない
        private void LogConvert_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isExecute)
            {
                e.Cancel = true;
            }
            isExecute = false;
            ButtonExecuteEnable(false);
        }
        #endregion

    }
}
