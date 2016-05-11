using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualFormTest
{
    public partial class SettingsViewer : Form
    {
        //前に選択したコンボボックスのインデックス
        public int ComboBoxLastSelectedIndex { get; set; }
        //コンボボックス（SmallBasic）のアイテム
        public List<string> ComboItem { get; set; }

        //ドックパネル
        DockPanel _dockPanel = null;

        public SettingsViewer(DockPanel dockPanel)
        {
            InitializeComponent();

            //バージョンの表示
            var asm = Assembly.GetExecutingAssembly();
            var asm_name = asm.GetName();
            label4.Text = asm_name.Version.ToString();

            //スクリーンショットのフォルダ
            textBox1.Text = Config.ScreenshotDirectory;
            textBox1.SelectionStart = textBox1.Text.Length;

            //レンダリングバージョンとの対応
            Dictionary<int, string> rendering_dic = new Dictionary<int, string>();
            rendering_dic[7000] = "Internet Explorer 7";
            rendering_dic[8000] = "Internet Explorer 8";
            rendering_dic[8888] = "Internet Explorer 8 Standardsモード";
            rendering_dic[9000] = "Internet Explorer 9";
            rendering_dic[9999] = "Internet Explorer 9 Standardsモード";
            rendering_dic[10000] = "Internet Explorer 10";
            rendering_dic[10001] = "Internet Explorer 10 Standardsモード";
            rendering_dic[11000] = "Internet Explorer 11";
            rendering_dic[11001] = "Internet Explorer 11 Edgeモード";
            comboBox1.Items.AddRange(rendering_dic.Values.ToArray());
            int[] render_dic_keys = (int[])rendering_dic.Keys.ToArray();
            comboBox1.Tag = render_dic_keys;
            
            //インストールされているIEのバージョン
            Microsoft.Win32.RegistryKey regkey_ie = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Internet Explorer");
            string ieversion = (string)regkey_ie.GetValue("svcVersion");
            regkey_ie.Close();
            label6.Text = "インストール済IEのバージョン : " + ieversion;

            //現在のレンダリングバージョン
            Microsoft.Win32.RegistryKey regkey_render = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
            object nowrender_obj;
            if (regkey_render != null) nowrender_obj = regkey_render.GetValue(Path.GetFileName(asm.Location));
            else nowrender_obj = null;
            string nowrender_str = nowrender_obj != null ? nowrender_obj.ToString() : null;
            regkey_render.Close();
            int nowrender = 7000;//とりあえずIE7で
            if (nowrender_str != null) nowrender = Convert.ToInt32(nowrender_str);//キーが存在した場合
            string nowrender_disp = nowrender.ToString();
            if (rendering_dic.ContainsKey(nowrender)) nowrender_disp = rendering_dic[nowrender];
            int combo_index = Array.IndexOf(render_dic_keys, nowrender);
            comboBox1.SelectedIndex = combo_index;
            ComboBoxLastSelectedIndex = combo_index;
            label7.Text = "現在のレンダリングバージョン : " + nowrender_disp;

            //表示設定

            //バルーン通知
            checkBox3.Checked = !Config.NotShowNotifyBalloonMission;
            checkBox4.Checked = !Config.NotShowNotifyBalloonNdock;
            //通知音声設定
            //無効フラグ
            if (Config.SoundMissionDisableFlag) radioButton_mission_1.Checked = true;
            else radioButton_mission_2.Checked = true;
            if (Config.SoundNdockDisableFlag) radioButton_ndock_1.Checked = true;
            else radioButton_ndock_2.Checked = true;
            if (Config.SoundDamageDisableFlag) radioButton_damage_1.Checked = true;
            else radioButton_damage_2.Checked = true;
            if (Config.SoundDamageSortieDisableFlag) radioButton_sortie_1.Checked = true;
            else radioButton_sortie_2.Checked = true;
            //ファイル名
            textBox_mission.Text = Config.SoundMissionFileName;
            textBox_ndock.Text = Config.SoundNdockFileName;
            textBox_damage.Text = Config.SoundDamageFileName;
            textBox_sortie.Text = Config.SoundDamageSortieFileName;

            //SE音量
            trackBar_volume.Value = Config.SoundVolume;
            checkBox_semute.Checked = Config.SoundIsMuted;
            label_sevol.Text = Config.SoundVolume.ToString();
            //サーバー側にSE音量を送っておく
            Sounds.ChangeVolume(Config.SoundVolume);

            //--動作パネル
            //バケツライン
            numericUpDown_bucket.Value = Math.Max(Math.Min((int)(Config.BucketHPRatio * 100), 99), 1);
            //JSONロガー
            checkBox_json.Checked = !Config.LoggingDisableOnShowJson;
            //その他オプション
            //任務表示
            checkBox1.Checked = Config.QuestNotDisplayToForm;
            //ドロップログ
            checkBox2.Checked = Config.DropRecordAddDisable;
            //高画質モード
            checkBox5.Checked = Config.HighQualityMode;
            //終了時に確認する
            checkBox_closenotify.Checked = !Config.OnClosingNotifyDisable;
            //索敵モデル
            switch(Config.SearchUsingModel)
            {
                case HoppoAlpha.DataLibrary.SearchModel.Models.Model33: radioButton_search_3_model33.Checked = true; break;
                case HoppoAlpha.DataLibrary.SearchModel.Models.Hoppo201: radioButton_search_2_hoppo201.Checked = true; break;
                case HoppoAlpha.DataLibrary.SearchModel.Models.Old25: radioButton_search_1_old25.Checked = true; break;
            }
            //謎のおまじない
            textBox_omajinai.Text = Config.NazonoOmajonai;
            //UI更新頻度
            numericUpDown_ui_refresh.Value = Config.UIRefreshTimerInterval;

            //--ドックパネル
            _dockPanel = dockPanel;
            //上
            //numericUpDown_dtop_value.Maximum = _dockPanel.Height - 2;
            if (_dockPanel.DockTopPortion <= 1) radioButton_dtop_auto.Checked = true;
            else
            {
                radioButton_dtop_fix.Checked = true;
                numericUpDown_dtop_value.Value = (int)_dockPanel.DockTopPortion;
            }
            //下
            //numericUpDown_dbottom_value.Maximum = _dockPanel.Height - 2;
            if (_dockPanel.DockBottomPortion <= 1) radioButton_dbottom_auto.Checked = true;
            else
            {
                radioButton_dbottom_fix.Checked = true;
                numericUpDown_dbottom_value.Value = (int)_dockPanel.DockBottomPortion;
            }
            //左
            //numericUpDown_dleft_value.Maximum = _dockPanel.Width - 2;
            if (_dockPanel.DockLeftPortion <= 1) radioButton_dleft_auto.Checked = true;
            else
            {
                radioButton_dleft_fix.Checked = true;
                numericUpDown_dleft_value.Value = (int)_dockPanel.DockLeftPortion;
            }
            //右
            //numericUpDown_dleft_value.Maximum = _dockPanel.Width - 2;
            if (_dockPanel.DockRightPortion <= 1) radioButton_dright_auto.Checked = true;
            else
            {
                radioButton_dright_fix.Checked = true;
                numericUpDown_dright_value.Value = (int)_dockPanel.DockRightPortion;
            }

            //--連携ツール・統計データベース
            //アクセスキー
            textBox_db_accesskey.Text = Config.KancolleDbUserToken;
            //有効化
            checkBox_db_enable.Checked = !Config.KancolleDbPostDisable;
            //プロキシモードのとき送信するか
            checkBox_db_disableonproxy.Checked = !Config.KancolleDbPostOnProxyMode;

            //--連携ツール・検証データベース
            //有効化
            checkBox_verify_enable.Checked = Config.KancolleVerifyPostEnable;
            //更新頻度
            numericUpDown_kcvdb_refresh.Value = Config.KancolleVerifyScreenRefreshTimer;

        }

        //OKボタン
        private void button3_Click(object sender, EventArgs e)
        {
            //OKボタン
            //レジストリの記録
            if (comboBox1.SelectedIndex != ComboBoxLastSelectedIndex)
            {
                //書き換える値の取得
                int[] combotag = (int[])comboBox1.Tag;
                int val = combotag[comboBox1.SelectedIndex];
                //値の名前
                var asm = Assembly.GetExecutingAssembly();
                string keyname = Path.GetFileName(asm.Location);
                //レジストリを開く
                Microsoft.Win32.RegistryKey regkey_render = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
                regkey_render.SetValue(keyname, val, Microsoft.Win32.RegistryValueKind.DWord);
                regkey_render.Close();
            }
            //スクリーンショットのフォルダ
            Config.ScreenshotDirectory = textBox1.Text;
            //表示設定
            //通知設定
            Config.NotShowNotifyBalloonMission = !checkBox3.Checked;
            Config.NotShowNotifyBalloonNdock = !checkBox4.Checked;
            //遠征の音声
            if (radioButton_mission_1.Checked)
            {
                Config.SoundMissionDisableFlag = true;
            }
            else if (radioButton_mission_2.Checked)
            {
                Config.SoundMissionDisableFlag = false;
                if (!string.IsNullOrEmpty(textBox_mission.Text)) Config.SoundMissionFileName = textBox_mission.Text;
            }
            //入渠の音声
            if (radioButton_ndock_1.Checked)
            {
                Config.SoundNdockDisableFlag = true;
            }
            else if (radioButton_ndock_2.Checked)
            {
                Config.SoundNdockDisableFlag = false;
                if(!string.IsNullOrEmpty(textBox_ndock.Text)) Config.SoundNdockFileName = textBox_ndock.Text;
            }
            //大破の音声
            if(radioButton_damage_1.Checked)
            {
                Config.SoundDamageDisableFlag = true;
            }
            else if(radioButton_damage_2.Checked)
            {
                Config.SoundDamageDisableFlag = false;
                if (!string.IsNullOrEmpty(textBox_damage.Text)) Config.SoundDamageFileName = textBox_damage.Text;
            }
            //大破進撃の音声
            if(radioButton_sortie_1.Checked)
            {
                Config.SoundDamageSortieDisableFlag = true;
            }
            else if(radioButton_sortie_2.Checked)
            {
                Config.SoundDamageSortieDisableFlag = false;
                if (!string.IsNullOrEmpty(textBox_sortie.Text)) Config.SoundDamageSortieFileName = textBox_sortie.Text;
            }

            //音声の音量
            Config.SoundVolume = trackBar_volume.Value;
            Config.SoundIsMuted = checkBox_semute.Checked;

            //--動作
            //バケツライン
            Config.BucketHPRatio = (double)numericUpDown_bucket.Value / 100.0;
            //JSONロガー
            Config.LoggingDisableOnShowJson = !checkBox_json.Checked;
            //その他オプション
            //任務表示
            Config.QuestNotDisplayToForm = checkBox1.Checked;
            //ドロップログ
            Config.DropRecordAddDisable = checkBox2.Checked;
            //高画質モード
            Config.HighQualityMode = checkBox5.Checked;
            //終了時に確認する
            Config.OnClosingNotifyDisable = !checkBox_closenotify.Checked;
            //索敵モデル
            if (radioButton_search_3_model33.Checked) Config.SearchUsingModel = HoppoAlpha.DataLibrary.SearchModel.Models.Model33;
            else if (radioButton_search_2_hoppo201.Checked) Config.SearchUsingModel = HoppoAlpha.DataLibrary.SearchModel.Models.Hoppo201;
            else if (radioButton_search_1_old25.Checked) Config.SearchUsingModel = HoppoAlpha.DataLibrary.SearchModel.Models.Old25;
            //謎のおまじない
            Config.NazonoOmajonai = textBox_omajinai.Text;
            //UI更新頻度
            Config.UIRefreshTimerInterval = (int)numericUpDown_ui_refresh.Value;
            UIMethods.SetIntervalTimer();

            //--ドッキング
            SetDockingPage();

            //--連携ツール・艦これ統計DB
            //アクセスキー
            if (!string.IsNullOrEmpty(textBox_db_accesskey.Text)) Config.KancolleDbUserToken = textBox_db_accesskey.Text;
            //有効化
            Config.KancolleDbPostDisable = !checkBox_db_enable.Checked;
            //プロキシモードのとき送信するか
            Config.KancolleDbPostOnProxyMode = !checkBox_db_disableonproxy.Checked;

            //--連携ツール・艦これ検証DB
            Config.KancolleVerifyPostEnable = checkBox_verify_enable.Checked;
            KancolleVerifyDb.KCVDBObjects.ScreenRefreshRequired = true;//再描画
            Config.KancolleVerifyScreenRefreshTimer = (int)numericUpDown_kcvdb_refresh.Value;
            Form1 form = _dockPanel.FindForm() as Form1;
            form.dwPageCollection.KCVDBLog.SetTimerInterval();

            //フォームを閉じる
            this.Close();
        }


        //ドッキングのスタイル
        private void SetDockingPage()
        {
            double top = radioButton_dtop_auto.Checked ? 0.25 : (double)numericUpDown_dtop_value.Value;
            double bottom = radioButton_dbottom_auto.Checked ? 0.25 : (double)numericUpDown_dbottom_value.Value;
            double left = radioButton_dleft_auto.Checked ? 0.25 : (double)numericUpDown_dleft_value.Value;
            double right = radioButton_dright_auto.Checked ? 0.25 : (double)numericUpDown_dright_value.Value;

            SetDockPanelPortions(top, bottom, left, right);

            Form1 form = _dockPanel.FindForm() as Form1;
            form.DockParameterAutoSet(true);
        }

        #region デリゲート
        delegate void SetDockPanelPortionsCallBack(double top, double bottom, double left, double right);
        private void SetDockPanelPortions(double top, double bottom, double left, double right)
        {
            if (_dockPanel.InvokeRequired)
            {
                SetDockPanelPortionsCallBack d = new SetDockPanelPortionsCallBack(SetDockPanelPortions);
                _dockPanel.Invoke(d, new object[] { top, bottom, left, right });
            }
            else
            {
                _dockPanel.DockTopPortion = top;
                _dockPanel.DockBottomPortion = bottom;
                _dockPanel.DockLeftPortion = left;
                _dockPanel.DockRightPortion = right;
            }
        }

        #endregion

        #region その他のイベントハンドラー
        private void UrlStart(string url)
        {
            Process.Start(url);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Twitter
            UrlStart("https://twitter.com/koshian2");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //ブログ
            UrlStart("http://nekokan333.blog.fc2.com");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //DLページ
            UrlStart("http://nekokan333.web.fc2.com/hoppoalpha.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //スクリーンショットのフォルダの選択
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "スクリーンショットを保存するフォルダを指定";
            dialog.SelectedPath = Config.ScreenshotDirectory;
            if(dialog.ShowDialog(this) == DialogResult.OK)
            {
                string dir = dialog.SelectedPath;
                if (!dir.EndsWith(@"\")) dir = dir + @"\";
                textBox1.Text = dir;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //キーの削除
            //値の名前
            var asm = Assembly.GetExecutingAssembly();
            string keyname = Path.GetFileName(asm.Location);
            //レジストリを開く
            Microsoft.Win32.RegistryKey regkey_render = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            regkey_render.DeleteValue(keyname, false);
            //確認ダイアログ
            MessageBox.Show("レジストリの変更を初期化しました", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Play
        private void button_Play_Click(object sender, EventArgs e)
        {
            Button but = sender as Button;
            string filename = "";
            //遠征の場合
            if(but == button_mission_play)
            {
                if (radioButton_mission_1.Checked) return;
                if (radioButton_mission_2.Checked) filename = textBox_mission.Text;
            }
            //入渠の場合
            else if(but == button_ndock_play)
            {
                if (radioButton_ndock_1.Checked) return;
                if (radioButton_ndock_2.Checked) filename = textBox_ndock.Text;
            }
            //大破の場合
            else if(but == button_damage_play)
            {
                if (radioButton_damage_1.Checked) return;
                if (radioButton_damage_2.Checked) filename = textBox_damage.Text;
            }
            //大破進撃の場合
            else if (but == button_sortie_play)
            {
                if (radioButton_sortie_1.Checked) return;
                if (radioButton_sortie_2.Checked) filename = textBox_sortie.Text;
            }
            //再生
            Sounds.PlaySounds(filename, true);
        }

        //OpenFileDialog
        private void button_Open_Click(object sender, EventArgs e)
        {
            Button but = sender as Button;
            OpenFileDialog ofd = new OpenFileDialog();
            //ファイル名
            string filename = null;
            if (but == button_mission_refer) filename = textBox_mission.Text;
            else if (but == button_ndock_refer) filename = textBox_ndock.Text;
            else if (but == button_damage_refer) filename = textBox_damage.Text;
            else if (but == button_sortie_refer) filename = textBox_sortie.Text;
            ofd.FileName = Path.GetFileName(filename);
            //ディレクトリ
            if (filename == "") ofd.InitialDirectory = System.Environment.CurrentDirectory;
            else ofd.InitialDirectory = Path.GetDirectoryName(filename);
            //ファイルの種類
            ofd.Filter =
                "音声ファイル(*.wav;*.mp3;*.wma)|*.wav;*.mp3;*.wma|すべてのファイル(*.*)|*.*";
            //ofd.FilterIndex = 1;
            ofd.Title = "通知の音声ファイルを選択";
            ofd.RestoreDirectory = true;
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (but == button_mission_refer) textBox_mission.Text = ofd.FileName;
                else if (but == button_ndock_refer) textBox_ndock.Text = ofd.FileName;
                else if (but == button_damage_refer) textBox_damage.Text = ofd.FileName;
                else if (but == button_sortie_refer) textBox_sortie.Text = ofd.FileName;
            }
        }

        //ドッキングのラジオボタン
        private void radioButton_dock_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rad = sender as RadioButton;
            NumericUpDown target = null;

            switch(rad.Name)
            {
                case "radioButton_dtop_fix":
                    target = numericUpDown_dtop_value;
                    break;
                case "radioButton_dleft_fix":
                    target = numericUpDown_dleft_value;
                    break;
                case "radioButton_dright_fix":
                    target = numericUpDown_dright_value;
                    break;
                case "radioButton_dbottom_fix":
                    target = numericUpDown_dbottom_value;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            target.Enabled = rad.Checked;
        }

        //ドッキングのNumericUpDown
        
        private void numericUpDown_dock_ValueChanged(object sender, EventArgs e)
        {
            /*
            if (_dockPanel == null) return;

            NumericUpDown updown = sender as NumericUpDown;
            NumericUpDown target = null;
            int maximun = 0;

            switch(updown.Name)
            {
                case "numericUpDown_dtop_value":
                    target = numericUpDown_dbottom_value;
                    maximun = _dockPanel.Height;
                    break;
                case "numericUpDown_dbottom_value":
                    target = numericUpDown_dtop_value;
                    maximun = _dockPanel.Height;
                    break;
                case "numericUpDown_dleft_value":
                    target = numericUpDown_dright_value;
                    maximun = _dockPanel.Width;
                    break;
                case "numericUpDown_dright_value":
                    target = numericUpDown_dleft_value;
                    maximun = _dockPanel.Width;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            target.Maximum = maximun - updown.Value;*/
        }

        private void button_dock_apply_Click(object sender, EventArgs e)
        {
            SetDockingPage();
        }
        #endregion

        private void trackBar_volume_Scroll(object sender, EventArgs e)
        {
            label_sevol.Text = trackBar_volume.Value.ToString();
            Sounds.ChangeVolume(trackBar_volume.Value);
        }

        private void checkBox_semute_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            int volume;
            if (cb.Checked) volume = 0;
            else volume = trackBar_volume.Value;

            label_sevol.Text = volume.ToString();
            Sounds.ChangeVolume(volume);
        }



    }
}
