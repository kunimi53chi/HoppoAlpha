using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiReqPractice;
using HoppoAlpha.DataLibrary.RawApi.ApiReqQuest;

namespace VisualFormTest
{
    public static class KancolleInfoGeneral
    {
        //任務表示
        public static void ShowQuests(System.Windows.Forms.Label[] labels, System.Windows.Forms.ToolTip tooltip)
        {
            if (APIReqQuest.Quests == null) return;
            //下のタブ表示部分
            string[] mes = (from q in APIReqQuest.Quests
                            select q.Value.Display()).ToArray();
            string[] tips = APIReqQuest.Quests.Select(delegate(KeyValuePair<int, ApiQuest> x)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(x.Value.api_detail);//任務名
                    switch (x.Value.api_type)
                    {
                        case 1: sb.AppendLine("[デイリー]"); break;
                        case 2: sb.AppendLine("[ウィークリー]"); break;
                        case 3: sb.AppendLine("[マンスリー]"); break;
                        case 4: sb.AppendLine("[単発]"); break;
                        case 5: sb.AppendLine("[クォタリー]"); break;
                    }

                    return sb.ToString();
                }).ToArray();

            for (int i = 0; i < labels.Length; i++)
            {
                //データがある場合
                if (i < mes.Length)
                {
                    CallBacks.SetLabelText(labels[i], mes[i]);
                    CallBacks.SetControlToolTip(tooltip, labels[i], tips[i]);
                }
                //ない場合
                else
                {
                    CallBacks.SetLabelText(labels[i], "");
                    CallBacks.SetControlToolTip(tooltip, labels[i], null);
                }
            }
        }

        public static void ShowQuestsForm(System.Windows.Forms.Form form)
        {
            if (APIReqQuest.Quests == null) return;
            var query = from q in APIReqQuest.Quests
                        select q.Value.DisplayShort();
            string str;
            if (query.Count() != 0 && !Config.QuestNotDisplayToForm) str = string.Join(" ", query);
            else if (AprilFool.IsAprilFool) str = "かもかもアルファ";
            else str = "ほっぽアルファ";
            CallBacks.SetFormText(form, str);
        }

        //演習表示
        public static void SetPractice(System.Windows.Forms.Label[] labels, System.Windows.Forms.ToolTip tooltip)
        {
            for (int i = 0; i < APIReqPractice.Practice.Count; i++)
            {
                ApiPractice data = APIReqPractice.Practice[i];
                CallBacks.SetLabelText(labels[i], data.Display());
                //ToolTip
                CallBacks.SetControlToolTip(tooltip, labels[i], data.DisplayToolTips());
            }
        }


    }
}
