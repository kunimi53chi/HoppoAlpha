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
    public partial class TabMaterialShort : UserControl, ITabControl
    {
        Label[] label_material, label_material_diff;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }
        public bool IsShown { get; set; }

        public TabMaterialShort()
        {
            InitializeComponent();
        }

        //変数の設定
        public void Init()
        {
            //資材タブ
            label_material = new Label[]
            {
                label_material_1, label_material_2, label_material_3, label_material_4,
                label_material_5, label_material_6, label_material_7, label_material_8,
            };
            label_material_diff = new Label[]
            {
                label_material_diff_1, label_material_diff_2, label_material_diff_3, label_material_diff_4,
                label_material_diff_5, label_material_diff_6, label_material_diff_7, label_material_diff_8,
            };

            InitFinished = true;
        }

        public void Init2()
        {
            Init2Finished = true;
        }

        //資材タブの更新
        public Task TabMaterialUpdate()
        {
            if (!InitFinished) return Task.Factory.StartNew(() => { });
            return Task.Factory.StartNew(() =>
            {
                KancolleInfoMaterial.SetMaterial(label_material, label_material_diff);
            });
        }
        public void TabMaterialUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
            {
                KancolleInfoMaterial.SetMaterial(label_material, label_material_diff);
            }));
        }

        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(this, "smaterial");
        }

    }
}
