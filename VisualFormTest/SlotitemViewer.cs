using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public partial class SlotitemViewer : Form
    {
        public ExMasterSlotitem Slotitem { get; set; }

        public SlotitemViewer()
        {
            InitializeComponent();
        }

        public void Init(ExMasterSlotitem slotitem)
        {
            Slotitem = slotitem;
            //名前
            textBox1.Text = slotitem.api_name;
            //ID
            textBox2.Text = slotitem.api_id.ToString();
            //ステータス
            //テキストのセット
            StringBuilder sb = new StringBuilder();
            sb.Append(Helper.MstSlotitemDetailToString(slotitem));
            sb.Append("説明 : ").AppendLine(slotitem.api_info.Replace("<br>", Environment.NewLine));
            textBox3.Text = sb.ToString();
            //選択の解除
            textBox1.SelectionStart = 0;
            textBox2.SelectionStart = 0;
            textBox3.SelectionStart = 0;
        }
    }
}
