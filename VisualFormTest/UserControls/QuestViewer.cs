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
    public partial class QuestViewer : UserControl
    {
        public Label[] Labels { get; set; }
        public bool IsShown { get; set; }

        public QuestViewer()
        {
            InitializeComponent();
        }

        public void Init()
        {
            Labels = new Label[]
            {
                label_quest_name_1, label_quest_name_2, label_quest_name_3, label_quest_name_4,
                label_quest_name_5, label_quest_name_6, label_quest_name_7, label_quest_name_8
            };
        }

        public void ControlUpdate_Q()
        {
            if (!IsShown) return;
            //遠征のアップデート（一般のを転用）
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfoGeneral.ShowQuests(Labels, toolTip1);
                }));
        }
    }
}
