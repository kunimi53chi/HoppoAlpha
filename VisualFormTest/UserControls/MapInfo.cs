using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;

namespace VisualFormTest.UserControls
{
    public partial class MapInfo : UserControl
    {
        public bool IsShown { get; set; }

        public MapInfo()
        {
            InitializeComponent();
            //チェックボックス
            checkBox_showcleard.Checked = Config.MapInfoShowtCleard;
        }

        //テキストのアップデート
        public void TextUpdate_Q()
        {
            if (!IsShown) return;
            //テキストの作成
            string text = MakeText();

            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    MethodInvoker invoker = delegate()
                    {
                        textBox_mapinfo.Text = text;
                        textBox_mapinfo.SelectionStart = 0;
                        textBox_mapinfo.SelectionLength = 0;
                    };

                    if (!textBox_mapinfo.IsHandleCreated)
                    {
                        if (!this.IsHandleCreated) return;
                    }
                    if (textBox_mapinfo.InvokeRequired)
                        textBox_mapinfo.Invoke(invoker);
                    else
                        invoker.Invoke();
                }));
        }

        //テキストの作成
        private string MakeText()
        {
            if (APIGetMember.MapInfo != null)
            {
                //マップのクエリ
                var query = APIGetMember.MapInfo.OrderByDescending(x => x.api_id).Select(delegate(ApiMapInfo info)
                {
                    //イベントマップの場合
                    if (info.api_eventmap != null)
                        return string.Format("{0}-{1} {2} ゲージ:{3}/{4} 難易度:{5}",
                            info.api_id / 10, info.api_id % 10, info.api_cleared == 1 ? "クリア済" : "未クリア",
                            info.api_eventmap.api_now_maphp, info.api_eventmap.api_max_maphp, Helper.MapRankToString(info.api_eventmap.api_selected_rank));
                    //通常マップの場合
                    else
                        return string.Format("{0}-{1} {2} {3}",
                            info.api_id / 10, info.api_id % 10, info.api_cleared == 1 ? "クリア済" : "未クリア",
                            info.api_exboss_flag == 1 ? "ボス撃破回数:" + info.api_defeat_count.ToString() : "");
                });
                //クリア済みをフィルタリング
                if(!Config.MapInfoShowtCleard)
                {
                    query = query.Where(x => !x.Contains("クリア済"));
                }

                return string.Join(Environment.NewLine, query);
            }
            else return string.Empty;
        }

        private void checkBox_showcleard_CheckedChanged(object sender, EventArgs e)
        {
            Config.MapInfoShowtCleard = checkBox_showcleard.Checked;
            TextUpdate_Q();
        }
    }
}
