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
    public partial class TabFleet : UserControl, ITabControl
    {
        Label[][,] label_fleet; Label[] label_fleetname; Panel[] panel_fleet;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }

        public Label[][,] LabelFleet
        {
            get { return this.label_fleet; }
            set { this.label_fleet = value; }
        }
        public Label[] LabelFleetName
        {
            get { return this.label_fleetname; }
            set { this.label_fleetname = value; }
        }
        public Panel[] PanelFleet
        {
            get { return this.panel_fleet; }
            set { this.panel_fleet = value; }
        }

        public TabFleet()
        {
            InitializeComponent();
        }

        //艦隊タブの初期化
        public void Init()
        {
            //変数の初期化
            label_fleet = new Label[4][,];
            label_fleet[0] = new Label[6, 8]
            {
                {label_fleet_1_11, label_fleet_1_12, label_fleet_1_13, label_fleet_1_14, label_fleet_1_15, label_fleet_1_16, label_fleet_1_17, label_fleet_1_18},
                {label_fleet_1_21, label_fleet_1_22, label_fleet_1_23, label_fleet_1_24, label_fleet_1_25, label_fleet_1_26, label_fleet_1_27, label_fleet_1_28},
                {label_fleet_1_31, label_fleet_1_32, label_fleet_1_33, label_fleet_1_34, label_fleet_1_35, label_fleet_1_36, label_fleet_1_37, label_fleet_1_38},
                {label_fleet_1_41, label_fleet_1_42, label_fleet_1_43, label_fleet_1_44, label_fleet_1_45, label_fleet_1_46, label_fleet_1_47, label_fleet_1_48},
                {label_fleet_1_51, label_fleet_1_52, label_fleet_1_53, label_fleet_1_54, label_fleet_1_55, label_fleet_1_56, label_fleet_1_57, label_fleet_1_58},
                {label_fleet_1_61, label_fleet_1_62, label_fleet_1_63, label_fleet_1_64, label_fleet_1_65, label_fleet_1_66, label_fleet_1_67, label_fleet_1_68}
            };
            label_fleet[1] = new Label[6, 8]
            {
                {label_fleet_2_11, label_fleet_2_12, label_fleet_2_13, label_fleet_2_14, label_fleet_2_15, label_fleet_2_16, label_fleet_2_17, label_fleet_2_18},
                {label_fleet_2_21, label_fleet_2_22, label_fleet_2_23, label_fleet_2_24, label_fleet_2_25, label_fleet_2_26, label_fleet_2_27, label_fleet_2_28},
                {label_fleet_2_31, label_fleet_2_32, label_fleet_2_33, label_fleet_2_34, label_fleet_2_35, label_fleet_2_36, label_fleet_2_37, label_fleet_2_38},
                {label_fleet_2_41, label_fleet_2_42, label_fleet_2_43, label_fleet_2_44, label_fleet_2_45, label_fleet_2_46, label_fleet_2_47, label_fleet_2_48},
                {label_fleet_2_51, label_fleet_2_52, label_fleet_2_53, label_fleet_2_54, label_fleet_2_55, label_fleet_2_56, label_fleet_2_57, label_fleet_2_58},
                {label_fleet_2_61, label_fleet_2_62, label_fleet_2_63, label_fleet_2_64, label_fleet_2_65, label_fleet_2_66, label_fleet_2_67, label_fleet_2_68}
            };
            label_fleet[2] = new Label[6, 5]
            {
                {label_fleet_3_11, label_fleet_3_12, label_fleet_3_13, label_fleet_3_14, label_fleet_3_15},
                {label_fleet_3_21, label_fleet_3_22, label_fleet_3_23, label_fleet_3_24, label_fleet_3_25},
                {label_fleet_3_31, label_fleet_3_32, label_fleet_3_33, label_fleet_3_34, label_fleet_3_35},
                {label_fleet_3_41, label_fleet_3_42, label_fleet_3_43, label_fleet_3_44, label_fleet_3_45},
                {label_fleet_3_51, label_fleet_3_52, label_fleet_3_53, label_fleet_3_54, label_fleet_3_55},
                {label_fleet_3_61, label_fleet_3_62, label_fleet_3_63, label_fleet_3_64, label_fleet_3_65}
            };
            label_fleet[3] = new Label[6, 5]
            {
                {label_fleet_4_11, label_fleet_4_12, label_fleet_4_13, label_fleet_4_14, label_fleet_4_15},
                {label_fleet_4_21, label_fleet_4_22, label_fleet_4_23, label_fleet_4_24, label_fleet_4_25},
                {label_fleet_4_31, label_fleet_4_32, label_fleet_4_33, label_fleet_4_34, label_fleet_4_35},
                {label_fleet_4_41, label_fleet_4_42, label_fleet_4_43, label_fleet_4_44, label_fleet_4_45},
                {label_fleet_4_51, label_fleet_4_52, label_fleet_4_53, label_fleet_4_54, label_fleet_4_55},
                {label_fleet_4_61, label_fleet_4_62, label_fleet_4_63, label_fleet_4_64, label_fleet_4_65}
            };
            label_fleetname = new Label[4]
            {
                label_fleet_1_fleetname, label_fleet_2_fleetname, label_fleet_3_fleetname, label_fleet_4_fleetname
            };
            panel_fleet = new Panel[4]
            {
                panel_fleet_1, panel_fleet_2, panel_fleet_3, panel_fleet_4
            };
            //イベントハンドラ
            foreach (Label[,] x in label_fleet)
            {
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    x[i, 0].MouseHover += new EventHandler(KancolleInfoFleet.FleetLabel_MouseHover);
                    x[i, 0].MouseLeave += new EventHandler(KancolleInfoFleet.FleetLabel_MouseLeave);
                }
            }
            contextMenuStrip_fleet.ItemClicked += new ToolStripItemClickedEventHandler(KancolleInfoFleet.contextMenuStrip_fleet_ItemClicked);

            InitFinished = true;
        }

        public void Init2() 
        {
            Init2Finished = true;
        }

        //艦隊タブの更新
        /*
        public Task TabUpdate()
        {
            if (!InitFinished) return Task.Factory.StartNew(() => { });
            return Task.Factory.StartNew(() =>
                {
                    KancolleInfoFleet.FleetInfoUpdate(this);
                });
        }*/

        public void TabUpdate_Q()
        {
            if (!InitFinished) return;
            UIMethods.UIQueue.Enqueue(
                new UIMethods(() =>
                {
                    KancolleInfoFleet.FleetInfoUpdate(this);
                }));
        }

        private void toolStripMenuItem_fleet_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(this, "fleet");
        }
    }
}
