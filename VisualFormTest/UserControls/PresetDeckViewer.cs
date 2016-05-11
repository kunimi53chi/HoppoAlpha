using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using Microsoft.VisualBasic;

namespace VisualFormTest.UserControls
{
    public partial class PresetDeckViewer : UserControl
    {
        public bool IsShown { get; set; }

        public PresetDeckViewer()
        {
            InitializeComponent();
        }

        public void CheckDuplicate()
        {
            if (!IsShown) return;
            if (APIGetMember.Preset == null) return;

            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    //重複を調べる
                    var duplicate = new Dictionary<int, List<int>>();
                    foreach (var i in Enumerable.Range(0, APIGetMember.Preset.Count))
                    {
                        foreach (var j in Enumerable.Range(0, APIGetMember.Preset[i].api_ship.Count))
                        {
                            var shipid = APIGetMember.Preset[i].api_ship[j];

                            if (shipid < 0) continue;
                            if (!Config.PresetDuplicateCheckAppliesFlagship && j <= 1) continue;

                            List<int> list;
                            if (!duplicate.TryGetValue(shipid, out list)) list = new List<int>();
                            list.Add(i + 1);
                            duplicate[shipid] = list;
                        }
                    }

                    //重複していたらテキストボックスに登録
                    var sb = new StringBuilder();
                    foreach (var c in duplicate)
                    {
                        if (c.Value.Count >= 2)
                        {
                            ApiShip oship;
                            if (APIPort.ShipsDictionary.TryGetValue(c.Key, out oship))
                            {
                                sb.AppendFormat("【{0}(Lv{1})】が プリセット No.{2} に重複登録されています", oship.ShipName, oship.api_lv, string.Join(", ", c.Value))
                                    .AppendLine();
                            }
                        }
                    }
                    //フォーム用のテキスト作成
                    var formsb = new StringBuilder();
                    if (APIGetMember.SelectedPresetNo != 0) formsb.Append(CSharp.Japanese.Kanaxs.KanaEx.ToZenkaku(APIGetMember.SelectedPresetNo.ToString()));
                    if (sb.Length > 0) formsb.Append("！");//重複があった場合


                    Action act = () =>
                        {
                            textBox_duplicate.Text = sb.ToString();
                            textBox_duplicate.SelectionLength = 0;

                            var form = this.FindForm();
                            if (formsb.Length > 0) form.Text = "【" + formsb.ToString() + "】:プリセット";
                            else form.Text = "プリセット";
                        };
                    if (this.InvokeRequired)
                    {
                        this.Invoke(act);
                    }
                    else
                    {
                        act();
                    }
                }));
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItem_exceptFlagship.Checked = !Config.PresetDuplicateCheckAppliesFlagship;
        }

        private void toolStripMenuItem_exceptFlagship_Click(object sender, EventArgs e)
        {
            Config.PresetDuplicateCheckAppliesFlagship = !Config.PresetDuplicateCheckAppliesFlagship;
            toolStripMenuItem_exceptFlagship.Checked = !Config.PresetDuplicateCheckAppliesFlagship;
            CheckDuplicate();
        }
    }
}
