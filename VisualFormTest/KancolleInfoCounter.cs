using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    static class KancolleInfoCounter
    {
        //カウンターのデータ
        public static List<CounterItem> Counters { get; set; }
        public static string Directory { get; set; }
        public static string Filename { get; set; }
        //初期化したかどうか
        public static bool isInited { get; set; }

        //コンボボックス用のデータテーブル
        public static Dictionary<int, string> ParentMapComboBoxItem { get; set; }
        public static Dictionary<int, string> SubMapComboBoxItem { get; set; }
        public static Dictionary<int, string> ModeComboBoxItem { get; set; }

        //マップデータ
        public static Dictionary<int, MapDataForCounter> MapData { get; set; }

        //ツールチップ
        private static System.Windows.Forms.ToolTip CounterToolTip { get; set; }

        public static int CounterCount = 3;

        //コールバック
        delegate int SetComboBoxParametersCallBack(System.Windows.Forms.ComboBox combobox, string[] items, object tag, int selectedindex);
        delegate bool SetCheckBoxCheckCallBack(System.Windows.Forms.CheckBox checkbox , bool value, bool readmode);
        delegate string SetTextBoxTextCallBack(System.Windows.Forms.TextBox textbox, string text);
        delegate void SetTextBoxToolTipCallBack(System.Windows.Forms.TextBox textbox, string text);

        //コールバック
        #region コールバック
        private static int SetComboBoxParametersValue(System.Windows.Forms.ComboBox combobox, string[] items, object tag, int selectedindex)
        {
            if(combobox.InvokeRequired)
            {
                SetComboBoxParametersCallBack d = new SetComboBoxParametersCallBack(SetComboBoxParametersValue);
                combobox.Invoke(d, new object[] { combobox, items, tag, selectedindex});
            }
            else
            {
                combobox.Items.Clear();
                combobox.Items.AddRange(items);
                combobox.Tag = tag;
                combobox.SelectedIndex = selectedindex;
                return selectedindex;
            }
            return -1;
        }

        private static bool SetCheckBoxCheck(System.Windows.Forms.CheckBox checkbox, bool flag, bool readmode)
        {
            if(checkbox.InvokeRequired)
            {
                SetCheckBoxCheckCallBack d = new SetCheckBoxCheckCallBack(SetCheckBoxCheck);
                checkbox.Invoke(d, new object[] { checkbox, flag, readmode});
            }
            else
            {
                if(!readmode) checkbox.Checked = flag;
            }
            return checkbox.Checked;
        }

        private static string SetTextBoxText(System.Windows.Forms.TextBox textbox, string text)
        {
            if(textbox.InvokeRequired)
            {
                SetTextBoxTextCallBack d = new SetTextBoxTextCallBack(SetTextBoxText);
                textbox.Invoke(d, new object[] { textbox, text });
            }
            else
            {
                if(text != "") textbox.Text = text;
            }
            return textbox.Text;
        }

        private static void SetTextBoxToolTip(System.Windows.Forms.TextBox textbox, string text)
        {
            if(textbox.InvokeRequired)
            {
                SetTextBoxToolTipCallBack d = new SetTextBoxToolTipCallBack(SetTextBoxToolTip);
                textbox.Invoke(d, new object[] { textbox, text});
            }
            else
            {
                CounterToolTip.SetToolTip(textbox, text);
            }
        }
        #endregion


        //初期化
        public static void Init(System.Windows.Forms.Control[,] controls)
        {
            Counters = new List<CounterItem>();
            //コンボボックス            
            ParentMapComboBoxItem = new Dictionary<int, string>();
            SubMapComboBoxItem = new Dictionary<int, string>();
            ModeComboBoxItem = new Dictionary<int, string>();
            //ToolTip
            CounterToolTip = new System.Windows.Forms.ToolTip() { InitialDelay = 100};
            //親モード
            string[] parent = new string[] { "特定マップ", "全マップ" };
            for (int i = 0; i < parent.Length; i++)
            {
                ParentMapComboBoxItem.Add(i, parent[i]);
            }
            //子モード（マップ名）
            MapData = new Dictionary<int, MapDataForCounter>();
            foreach (ApiMstMapinfo x in APIMaster.MstMapinfos)
            {
                MapData.Add(x.api_id,
                    new MapDataForCounter() { ID = x.api_id, AreaNo = x.api_maparea_id, MapNo = x.api_no, MapName = x.api_name });
                SubMapComboBoxItem.Add(x.api_id,
                    string.Format("{0}-{1} {2}", x.api_maparea_id, x.api_no, x.api_name));
            }
            //読み込み
            Directory = @"user/" + APIPort.Basic.api_member_id + @"/general/";
            Filename = Directory + "counter.dat";

            var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(Filename, HoppoAlpha.DataLibrary.DataType.Counter);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Counter, loadResult, false);
            if (loadResult.IsSuccess)
            {
                Counters = (List<CounterItem>)loadResult.Instance;
                //マップが存在するかどうかチェック
                for (int i = 0; i < Counters.Count; i++)
                {
                    if (!MapData.ContainsKey(Counters[i].ID))
                    {
                        Counters[i] = new CounterItem();
                    }
                }
            }
            else
            {
                for (int i = 0; i < CounterCount; i++)
                {
                    Counters.Add(new CounterItem());
                }
            }
            //モード名
            for(int i=1; i<10; i++)
            {
                ModeComboBoxItem.Add(i, ((CounterMode)i).Display());
            }
            //フォームコントロールへの更新
            for (int i = 0; i < Counters.Count; i++)
            {
                SetDataToControl(controls, i);
            }
            //初期化フラグの更新
            isInited = true;
        }

        //リセット
        public static void Reset(System.Windows.Forms.Control[,] controls, int index)
        {
            isInited = false;
            Counters[index] = new CounterItem();
            SetDataToControl(controls, index);
            isInited = true;
        }

        //全てリセット
        public static void ResetAll(System.Windows.Forms.Control[,] controls)
        {
            isInited = false;
            for(int i=0; i<Counters.Count; i++) Counters[i] = new CounterItem();
            SetDataToControl(controls, -1);
            isInited = true;
        }

        //データからコントロールに更新
        public static void SetDataToControl(System.Windows.Forms.Control[,] controls, int index)
        {
            int start_i = (index < 0) ? 0 : index;
            int end_i = (index < 0) ? Counters.Count - 1 : index;
            isInited = false;
            for (int i = start_i; i <= end_i; i++)
            {
                CounterItem item = Counters[i];
                //チェックボックス
                SetCheckBoxCheck((System.Windows.Forms.CheckBox)controls[i, 0], item.Enabled, false);
                for(int j=1; j<controls.GetLength(1); j++)
                {
                    if (j == 2)
                    {
                        bool flag =  (!item.AllMap)&item.Enabled;
                        CallBacks.SetControlEnabled(controls[i, j], flag);
                    }
                    else
                    {
                        CallBacks.SetControlEnabled(controls[i, j], item.Enabled);//カウンターの有効・無効にあわせてEnabledを切り替え
                    }
                }
                //--コンボボックス
                //親モード
                string[] parent_items = ParentMapComboBoxItem.Values.ToArray();
                int[] parent_keys = ParentMapComboBoxItem.Keys.ToArray();
                SetComboBoxParametersValue((System.Windows.Forms.ComboBox)controls[i, 1],
                    parent_items, parent_keys, Convert.ToInt32(Counters[i].AllMap));
                //子モード
                string[] sub_items = SubMapComboBoxItem.Values.ToArray();
                int[] sub_keys = SubMapComboBoxItem.Keys.ToArray();
                int sub_index = Array.IndexOf(sub_keys, Counters[i].ID);
                SetComboBoxParametersValue((System.Windows.Forms.ComboBox)controls[i, 2],
                    sub_items, sub_keys, Math.Max(sub_index, 0));
                //モード
                string[] mode_items = ModeComboBoxItem.Values.ToArray();
                int[] mode_keys = ModeComboBoxItem.Keys.ToArray();
                int mode_index = Array.IndexOf(mode_keys, (int)Counters[i].Mode);
                SetComboBoxParametersValue((System.Windows.Forms.ComboBox)controls[i, 3],
                    mode_items, mode_keys, Math.Max(mode_index, 0));
                //テキストボックス
                System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)controls[i, 5];
                SetTextBoxText(tb, item.Value.ToString());
                //テキストボックスのToolTip
                SetTextBoxToolTip(tb, item.GetToolTipText());
                //リセット条件
                CallBacks.SetLabelText((System.Windows.Forms.Label)controls[i, 10], item.AutoReset.ToStrShort());
            }
            isInited = true;
        }

        //値の部分だけ更新
        public static void SetValueToTextBox(System.Windows.Forms.Control[,] controls)
        {
            for (int i = 0; i < Counters.Count; i++)
            {
                System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)controls[i, 5];
                //テキストボックス
                SetTextBoxText(tb, Counters[i].Value.ToString());
                //ToolTip
                SetTextBoxToolTip(tb, Counters[i].GetToolTipText());
            }
        }

        //コントロールからデータを取得
        public static void UpdateCountersFromControl(System.Windows.Forms.Control[,] controls, int counter_index)
        {
            CounterItem count = Counters[counter_index];
            //有効化(1)
            count.Enabled = SetCheckBoxCheck((System.Windows.Forms.CheckBox)controls[counter_index, 0], true, true);
            //全マップかどうか(1)
            int parent_selected = ((System.Windows.Forms.ComboBox)controls[counter_index, 1]).SelectedIndex;
            count.AllMap = parent_selected == 1;
            //マップIDの置き換え(3)
            System.Windows.Forms.ComboBox cb_map = (System.Windows.Forms.ComboBox)controls[counter_index, 2];
            int map_id = ((int[])cb_map.Tag)[cb_map.SelectedIndex];
            MapDataForCounter mapdata = MapData[map_id];
            count.ID = map_id;
            count.AreaNo = mapdata.AreaNo;
            count.MapNo = mapdata.MapNo;
            //モードの置き換え(1)
            System.Windows.Forms.ComboBox cb_mode = (System.Windows.Forms.ComboBox)controls[counter_index, 3];
            int mode_id = ((int[])cb_mode.Tag)[cb_mode.SelectedIndex];
            count.Mode = (CounterMode)mode_id;
            //値の置き換え(1)
            string textvalue = ((System.Windows.Forms.TextBox)controls[counter_index, 5]).Text;
            int value = Convert.ToInt32(textvalue);
            count.Value = value;
            //Trialのセット
            if (value == 0) count.Trial = 0;
            //ToolTipの再描画
            SetTextBoxToolTip((System.Windows.Forms.TextBox)controls[counter_index, 5], count.GetToolTipText());
        }

        //出撃時のチェック
        public static void TrialCheckAllAndCount(UserControls.TabCounter counter, BattleView view)
        {
            if (!isInited) return;
            foreach (var x in Counters)
            {
                x.TrialCheckAndCount(view);
            }
            SetValueToTextBox(counter.control_counter);
        }

        //戦闘終了後のチェック
        public static void CheckAllAndCount(UserControls.TabCounter counter, BattleView view)
        {
            if (!isInited) return;
            foreach (var x in Counters)
            {
                x.CheckAndCount(view);
            }
            SetValueToTextBox(counter.control_counter);
        }

        //保存
        public static void Save()
        {
            //初期化されていなかったら
            if (!isInited || Counters == null) return;
            //ディレクトリの作成
            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
            //保存
            var saveResult = HoppoAlpha.DataLibrary.Files.Save(Filename, HoppoAlpha.DataLibrary.DataType.Counter, Counters);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Counter, saveResult, true);
        }

        //イベントハンドラー
        #region イベントハンドラー
        //control_counterの取得
        private static Control[,] GetControlCounter(Control c)
        {
            DockingWindows.DockWindowTabCollection tab = DockingWindows.DockWindowTabCollection.GetCollection(c);
            return tab.Counter.control_counter;
        }

        //チェックボックス
        public static void counterCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            CheckBox cb = (CheckBox)sender;
            Control[,] control_counter = GetControlCounter(cb);
            int counter_idx = Convert.ToInt32((string)cb.Tag);
            for (int i = 1; i < control_counter.GetLength(1); i++)
            {
                if (i == 2) CallBacks.SetControlEnabled(control_counter[counter_idx, i], cb.Checked && !KancolleInfoCounter.Counters[counter_idx].AllMap);
                else CallBacks.SetControlEnabled(control_counter[counter_idx, i], cb.Checked);
            }
            //値のアップデート
            KancolleInfoCounter.UpdateCountersFromControl(control_counter, counter_idx);
        }

        //全マップのコンボボックス
        public static void counterComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            ComboBox cb = (ComboBox)sender;
            Control[,] control_counter = GetControlCounter(cb);
            int counter_idx = -1;
            for (int i = 0; i < control_counter.GetLength(0); i++)
            {
                if (cb == control_counter[i, 1])
                {
                    counter_idx = i;
                    break;
                }
            }
            //2番のコンボボックスを変更
            CheckBox check = (CheckBox)control_counter[counter_idx, 0];
            CallBacks.SetControlEnabled(control_counter[counter_idx, 2], (cb.SelectedIndex == 0 & check.Checked));
            //値のアップデート
            KancolleInfoCounter.UpdateCountersFromControl(control_counter, counter_idx);
        }

        //プラスボタン
        public static void counterPlusButtonClick(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            int counter_idx = Convert.ToInt32((string)(sender as Button).Tag);
            //値を+1
            Control[,] control_counter = GetControlCounter(sender as Control);
            TextBox tb = (TextBox)control_counter[counter_idx, 5];
            int value = Convert.ToInt32(tb.Text) + 1;
            CallBacks.SetTextBoxText(tb, value.ToString());
            //値のアップデート
            KancolleInfoCounter.UpdateCountersFromControl(control_counter, counter_idx);
        }

        //マイナスボタン
        public static void counterMinusButtonClick(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            int counter_idx = Convert.ToInt32((string)(sender as Button).Tag);
            //値を-1
            Control[,] control_counter = GetControlCounter(sender as Control);
            TextBox tb = (TextBox)control_counter[counter_idx, 5];
            int value = Math.Max(Convert.ToInt32(tb.Text) - 1, 0);
            CallBacks.SetTextBoxText(tb, value.ToString());
            //値のアップデート
            KancolleInfoCounter.UpdateCountersFromControl(control_counter, counter_idx);
        }

        //初期化ボタン
        public static void counterResetButtonClick(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            int counter_idx = Convert.ToInt32((string)(sender as Button).Tag);
            //ここを変える
            if (MessageBox.Show(string.Format("カウンター {0} を初期化します？", counter_idx + 1), "確認",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Control[,] control_counter = GetControlCounter(sender as Control);
                //全てリセット
                KancolleInfoCounter.Reset(control_counter, counter_idx);
            }

        }

        //ゼロにするボタン
        public static void counterZeroButtonClick(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            int counter_idx = Convert.ToInt32((string)(sender as Button).Tag);
            //カウントを0に
            Control[,] control_counter = GetControlCounter(sender as Control);
            CallBacks.SetTextBoxText((System.Windows.Forms.TextBox)control_counter[counter_idx, 5], "0");
            //条件のアップデート
            KancolleInfoCounter.UpdateCountersFromControl(control_counter, counter_idx);
        }

        //更新ボタン
        public static void counterConditionModifyButtonClick(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            int counter_idx = Convert.ToInt32((string)(sender as Button).Tag);
            //確認
            if (MessageBox.Show(string.Format("カウンター {0} の条件を更新しますか？\n（カウントはリセットされます）", counter_idx + 1), "確認",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Control[,] control_counter = GetControlCounter(sender as Control);
                //カウントを0に
                CallBacks.SetTextBoxText((System.Windows.Forms.TextBox)control_counter[counter_idx, 5], "0");
                //条件のアップデート
                KancolleInfoCounter.UpdateCountersFromControl(control_counter, counter_idx);
            }
        }

        //カウンターの自動リセットタイミングを変更
        public static void contextMenuStrip_counterTime_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            //クリックしたカウンターの取得
            System.Windows.Forms.ContextMenuStrip context = (System.Windows.Forms.ContextMenuStrip)sender;
            var source = (System.Windows.Forms.GroupBox)context.SourceControl;
            int counter_idx = -1;
            switch(source.Name)
            {
                case "groupBox1":
                    counter_idx = 0;
                    break;
                case "groupBox2":
                    counter_idx = 1;
                    break;
                case "groupBox3":
                    counter_idx = 2;
                    break;
            }
            if (counter_idx == -1) return;
            //クリックしたメニューの取得
            CounterResetCondition after_condition = CounterResetCondition.None;
            switch(e.ClickedItem.Name)
            {
                case "toolStripMenuItem_daily":
                    after_condition = CounterResetCondition.Daily;
                    break;
                case "toolStripMenuItem_weekly":
                    after_condition = CounterResetCondition.Weekly;
                    break;
                case "toolStripMenuItem_monthly":
                    after_condition = CounterResetCondition.Monthly;
                    break;
            }
            CounterResetCondition before_condtion = Counters[counter_idx].AutoReset;
            //条件の変更
            if(MessageBox.Show(string.Format("カウンター {0} のリセット条件を {1} から {2} に変更しますか？", counter_idx+1, before_condtion.ToStrLong(), after_condition.ToStrLong()),
                "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //オブジェクトの変更
                Counters[counter_idx].AutoReset = after_condition;
                //アップデート
                var controls = (Control[,])context.Tag;
                SetDataToControl(controls, counter_idx);
            }
        }

        //全てリセット
        public static void counterAllResetButtonClick(object sender, EventArgs e)
        {
            //カウンターデータ側の初期化が終わっていない場合
            if (!KancolleInfoCounter.isInited) return;
            //確認
            if (MessageBox.Show("全てのカウンターを初期化しますか？\n（条件もリセットされます）", "確認",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Control[,] control_counter = GetControlCounter(sender as Control);
                //全てリセット
                KancolleInfoCounter.ResetAll(control_counter);
            }
        }
        #endregion
    }

    //マップのカウンター用
    public class MapDataForCounter
    {
        public string MapName { get; set; }
        public int ID { get; set; }
        public int AreaNo { get; set; }
        public int MapNo { get; set; }
    }
}
