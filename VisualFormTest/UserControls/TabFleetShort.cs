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
    public partial class TabFleetShort : UserControl, ITabControl
    {
        Label[,] label_fleet;
        ToolStripMenuItem[] toolstipfleets;

        public bool InitFinished { get; set; }
        public bool Init2Finished { get; set; }
        public bool IsShown { get; set; }
        public int FleetIndex { get; set; }

        public Label[,] LabelFleet
        {
            get { return this.label_fleet; }
            set { this.label_fleet = value; }
        }
        public Label LabelFleetName
        {
            get { return this.label_fleet_1_fleetname; }
            set { this.label_fleet_1_fleetname = value; }
        }
        public Panel PanelFleet
        {
            get { return this.panel_fleet_1; }
            set { this.panel_fleet_1 = value; }
        }


        public TabFleetShort()
        {
            InitializeComponent();
        }

        public void Init()
        {
            //変数の初期化
            label_fleet = new Label[6, 8]
            {
                {label_fleet_1_11, label_fleet_1_12, label_fleet_1_13, label_fleet_1_14, label_fleet_1_15, label_fleet_1_16, label_fleet_1_17, label_fleet_1_18},
                {label_fleet_1_21, label_fleet_1_22, label_fleet_1_23, label_fleet_1_24, label_fleet_1_25, label_fleet_1_26, label_fleet_1_27, label_fleet_1_28},
                {label_fleet_1_31, label_fleet_1_32, label_fleet_1_33, label_fleet_1_34, label_fleet_1_35, label_fleet_1_36, label_fleet_1_37, label_fleet_1_38},
                {label_fleet_1_41, label_fleet_1_42, label_fleet_1_43, label_fleet_1_44, label_fleet_1_45, label_fleet_1_46, label_fleet_1_47, label_fleet_1_48},
                {label_fleet_1_51, label_fleet_1_52, label_fleet_1_53, label_fleet_1_54, label_fleet_1_55, label_fleet_1_56, label_fleet_1_57, label_fleet_1_58},
                {label_fleet_1_61, label_fleet_1_62, label_fleet_1_63, label_fleet_1_64, label_fleet_1_65, label_fleet_1_66, label_fleet_1_67, label_fleet_1_68}
            };
            toolstipfleets = new ToolStripMenuItem[]
            {
                toolStripMenuItem_fleet1, toolStripMenuItem_fleet2, toolStripMenuItem_fleet3, toolStripMenuItem_fleet4,
            };

            //イベントハンドラ
            for(int i=0; i<label_fleet.GetLength(0); i++)
            {
                label_fleet[i, 0].MouseHover += new EventHandler(KancolleInfoFleet.FleetLabel_MouseHover);
                label_fleet[i, 0].MouseLeave += new EventHandler(KancolleInfoFleet.FleetLabel_MouseLeave);
            }
            //コンテキスト
            contextMenuStrip_fleetshort.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip_fleetshort_ItemClicked);
            //艦隊IDの復元
            if (Config.TabFleetShortFleetIndex >= 0 && Config.TabFleetShortFleetIndex < toolstipfleets.Length) toolstipfleets[Config.TabFleetShortFleetIndex].PerformClick();

            InitFinished = true;
        }

        public void Init2()
        {
            Init2Finished = true;
        }

        void contextMenuStrip_fleetshort_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch(e.ClickedItem.Name)
            {
                //艦隊IDの変更
                case "toolStripMenuItem_fleet1":
                    ChangeFleetIndex(0); break;
                case "toolStripMenuItem_fleet2":
                    ChangeFleetIndex(1); break;
                case "toolStripMenuItem_fleet3":
                    ChangeFleetIndex(2); break;
                case "toolStripMenuItem_fleet4":
                    ChangeFleetIndex(3); break;
                //TabFleetのContextと共通
                case "toolStripMenuItem_fleet_copy":
                    KancolleInfoFleet.context_Fleet_CopyToClipboard(FleetIndex);
                    break;
                case "toolStripMenuItem_fleet_query":
                    var tab = DockingWindows.DockWindowTabCollection.GetCollection(panel_fleet_1);
                    KancolleInfoFleet.context_Fleet_MakeQuery(FleetIndex, tab);
                    break;
                case "toolStripMenuItem_fleet_convertdeck":
                    KancolleInfoFleet.context_ConvertDeckBuilder(FleetIndex);
                    break;
                case "toolStripMenuItem_fleet_screenshot":
                    HelperScreen.ScreenShot(this, "sfleet");
                    break;
            }
        }

        //タブの更新
        /*
        public Task TabUpdate()
        {
            if (!IsShown || !InitFinished) return Task.Factory.StartNew(() => { });
            return Task.Factory.StartNew(() =>
                {
                    KancolleInfoFleet.FleetInfoShortUpdate(this);
                });
        }*/

        public void TabUpdate_Q()
        {
            if (!IsShown || !InitFinished) return;
            UIMethods.UIQueue.Enqueue(new UIMethods(
                () => { KancolleInfoFleet.FleetInfoShortUpdate(this); }
            ));
        }

        //艦隊の変更
        public void ChangeFleetIndex(int index)
        {
            //押すToolStripMenuItemの選択
            foreach(int i in Enumerable.Range(0, toolstipfleets.Length))
            {
                if (i == index) CallBacks.SetToolStripMenuItemChecked(toolstipfleets[i], contextMenuStrip_fleetshort, true);
                else
                {
                    if (toolstipfleets[i].Checked) CallBacks.SetToolStripMenuItemChecked(toolstipfleets[i], contextMenuStrip_fleetshort, false);
                }
            }
            //艦隊IDの変更
            this.FleetIndex = index;
            Config.TabFleetShortFleetIndex = this.FleetIndex;
            //TabUpdate();
            TabUpdate_Q();
        }
    }
}
