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
    public partial class TabJson : UserControl, ITabControl
    {
        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        public TabJson()
        {
            InitializeComponent();
        }

        public void Init()
        {
            InitFinished = true;
        }

        public void Init2()
        {
            Init2Finished = true;
        }

        //JSONの更新
        public void UpdateJson_Q(string response, string body)
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    CallBacks.SetTextBoxTextAppend(textBox_json, Environment.NewLine + response + Environment.NewLine + body);
                }));
        }

        //JSONのクリア
        public void ClearJson()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    textBox_json.Clear();
                }));
            //CallBacks.SetTextBoxText(textBox_json, String.Empty);
        }
    }
}
