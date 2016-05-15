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
    public partial class AirBaseCorps : UserControl
    {
        //ハンドラー
        public AirBaseCorpsLabelHandler Handler { get; set; }
        //表示されているかどうか
        public bool IsShown { get; set; }

        public ToolTip ToolTip { get; set; }

        public AirBaseCorps()
        {
            InitializeComponent();

            //Init(); これをあとで親画面から叩くこと
        }

        public void UpdateStatus()
        {
            if (IsShown)
            {
                UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfoAirBaseCorps.SetAirBaseStatus(this);
                }));
            }
        }

        public void UpdateBattle()
        {
            if (IsShown)
            {
                UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleInfoAirBaseCorps.SetAirBaseStatus(this);
                    KancolleInfoAirBaseCorps.SetAirBaseBattle(this);
                }));
            }

        }

        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(this, "airbase");
        }

        public void Init()
        {
            this.ToolTip = toolTip1;

            //ハンドラーのセット
            Handler = new AirBaseCorpsLabelHandler();

            //--飛行隊のステータス
            //第1飛行隊
            Handler.AirBases[0] = new AirBaseCorpsLabelHandler.AirBase();
            Handler.AirBases[0].Squadrons[0] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base1_name_1,
                PlaneNum = label_base1_num_1,
                Training = label_base1_training_1,
                Cost = label_base1_cost_1,
                Radius = label_base1_radius_1,
                Dispatch = label_base1_dispatch_1,
                AirSupValue = label_base1_airsup_1,
            };
            Handler.AirBases[0].Squadrons[1] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base1_name_2,
                PlaneNum = label_base1_num_2,
                Training = label_base1_training_2,
                Cost = label_base1_cost_2,
                Radius = label_base1_radius_2,
                Dispatch = label_base1_dispatch_2,
                AirSupValue = label_base1_airsup_2,
            };
            Handler.AirBases[0].Squadrons[2] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base1_name_3,
                PlaneNum = label_base1_num_3,
                Training = label_base1_training_3,
                Cost = label_base1_cost_3,
                Radius = label_base1_radius_3,
                Dispatch = label_base1_dispatch_3,
                AirSupValue = label_base1_airsup_3,
            };
            Handler.AirBases[0].Squadrons[3] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base1_name_4,
                PlaneNum = label_base1_num_4,
                Training = label_base1_training_4,
                Cost = label_base1_cost_4,
                Radius = label_base1_radius_4,
                Dispatch = label_base1_dispatch_4,
                AirSupValue = label_base1_airsup_4,
            };
            Handler.AirBases[0].TotalNum = label_base1_num_total;
            Handler.AirBases[0].TotalCost = label_base1_cost_total;
            Handler.AirBases[0].TotalRadius = label_base1_radius_total;
            Handler.AirBases[0].TotalDispatch = label_base1_dispatch_total;
            Handler.AirBases[0].TotalAirSup = label_base1_airsup_total;
            //第2飛行隊
            Handler.AirBases[1] = new AirBaseCorpsLabelHandler.AirBase();
            Handler.AirBases[1].Squadrons[0] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base2_name_1,
                PlaneNum = label_base2_num_1,
                Training = label_base2_training_1,
                Cost = label_base2_cost_1,
                Radius = label_base2_radius_1,
                Dispatch = label_base2_dispatch_1,
                AirSupValue = label_base2_airsup_1,
            };
            Handler.AirBases[1].Squadrons[1] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base2_name_2,
                PlaneNum = label_base2_num_2,
                Training = label_base2_training_2,
                Cost = label_base2_cost_2,
                Radius = label_base2_radius_2,
                Dispatch = label_base2_dispatch_2,
                AirSupValue = label_base2_airsup_2,
            };
            Handler.AirBases[1].Squadrons[2] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base2_name_3,
                PlaneNum = label_base2_num_3,
                Training = label_base2_training_3,
                Cost = label_base2_cost_3,
                Radius = label_base2_radius_3,
                Dispatch = label_base2_dispatch_3,
                AirSupValue = label_base2_airsup_3,
            };
            Handler.AirBases[1].Squadrons[3] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base2_name_4,
                PlaneNum = label_base2_num_4,
                Training = label_base2_training_4,
                Cost = label_base2_cost_4,
                Radius = label_base2_radius_4,
                Dispatch = label_base2_dispatch_4,
                AirSupValue = label_base2_airsup_4,
            };
            Handler.AirBases[1].TotalNum = label_base2_num_total;
            Handler.AirBases[1].TotalCost = label_base2_cost_total;
            Handler.AirBases[1].TotalRadius = label_base2_radius_total;
            Handler.AirBases[1].TotalDispatch = label_base2_dispatch_total;
            Handler.AirBases[1].TotalAirSup = label_base2_airsup_total;
            //第3飛行隊
            Handler.AirBases[2] = new AirBaseCorpsLabelHandler.AirBase();
            Handler.AirBases[2].Squadrons[0] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base3_name_1,
                PlaneNum = label_base3_num_1,
                Training = label_base3_training_1,
                Cost = label_base3_cost_1,
                Radius = label_base3_radius_1,
                Dispatch = label_base3_dispatch_1,
                AirSupValue = label_base3_airsup_1,
            };
            Handler.AirBases[2].Squadrons[1] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base3_name_2,
                PlaneNum = label_base3_num_2,
                Training = label_base3_training_2,
                Cost = label_base3_cost_2,
                Radius = label_base3_radius_2,
                Dispatch = label_base3_dispatch_2,
                AirSupValue = label_base3_airsup_2,
            };
            Handler.AirBases[2].Squadrons[2] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base3_name_3,
                PlaneNum = label_base3_num_3,
                Training = label_base3_training_3,
                Cost = label_base3_cost_3,
                Radius = label_base3_radius_3,
                Dispatch = label_base3_dispatch_3,
                AirSupValue = label_base3_airsup_3,
            };
            Handler.AirBases[2].Squadrons[3] = new AirBaseCorpsLabelHandler.Squadron()
            {
                PlaneName = label_base3_name_4,
                PlaneNum = label_base3_num_4,
                Training = label_base3_training_4,
                Cost = label_base3_cost_4,
                Radius = label_base3_radius_4,
                Dispatch = label_base3_dispatch_4,
                AirSupValue = label_base3_airsup_4,
            };
            Handler.AirBases[2].TotalNum = label_base3_num_total;
            Handler.AirBases[2].TotalCost = label_base3_cost_total;
            Handler.AirBases[2].TotalRadius = label_base3_radius_total;
            Handler.AirBases[2].TotalDispatch = label_base3_dispatch_total;
            Handler.AirBases[2].TotalAirSup = label_base3_airsup_total;

            //航空戦データ
            //第1の航空戦
            Handler.AirCombats[0] = new AirBaseCorpsLabelHandler.AirCombat();
            Handler.AirCombats[0].First = new AirBaseCorpsLabelHandler.AirCombatItem()
            {
                Stage1Player = label_base1_st1_1_player,
                Stage1Enemy = label_base1_st1_1_enemy,
                Stage2Player = label_base1_st2_1_player,
                Stage2Enemy = label_base1_st2_1_enemy,
                AttachPlayer = label_base1_touch_1_player,
                AttachEnemy = label_base1_touch_1_enemy,
                AirSupCondition = label_base1_1_airsupofplayer,
            };
            Handler.AirCombats[0].Second = new AirBaseCorpsLabelHandler.AirCombatItem()
            {
                Stage1Player = label_base1_st1_2_player,
                Stage1Enemy = label_base1_st1_2_enemy,
                Stage2Player = label_base1_st2_2_player,
                Stage2Enemy = label_base1_st2_2_enemy,
                AttachPlayer = label_base1_touch_2_player,
                AttachEnemy = label_base1_touch_2_enemy,
                AirSupCondition = label_base1_2_airsupofplayer,
            };
            //第2の航空戦
            Handler.AirCombats[1] = new AirBaseCorpsLabelHandler.AirCombat();
            Handler.AirCombats[1].First = new AirBaseCorpsLabelHandler.AirCombatItem()
            {
                Stage1Player = label_base2_st1_1_player,
                Stage1Enemy = label_base2_st1_1_enemy,
                Stage2Player = label_base2_st2_1_player,
                Stage2Enemy = label_base2_st2_1_enemy,
                AttachPlayer = label_base2_touch_1_player,
                AttachEnemy = label_base2_touch_1_enemy,
                AirSupCondition = label_base2_1_airsupofplayer,
            };
            Handler.AirCombats[1].Second = new AirBaseCorpsLabelHandler.AirCombatItem()
            {
                Stage1Player = label_base2_st1_2_player,
                Stage1Enemy = label_base2_st1_2_enemy,
                Stage2Player = label_base2_st2_2_player,
                Stage2Enemy = label_base2_st2_2_enemy,
                AttachPlayer = label_base2_touch_2_player,
                AttachEnemy = label_base2_touch_2_enemy,
                AirSupCondition = label_base2_2_airsupofplayer,
            };
            //第3の航空戦
            Handler.AirCombats[2] = new AirBaseCorpsLabelHandler.AirCombat();
            Handler.AirCombats[2].First = new AirBaseCorpsLabelHandler.AirCombatItem()
            {
                Stage1Player = label_base3_st1_1_player,
                Stage1Enemy = label_base3_st1_1_enemy,
                Stage2Player = label_base3_st2_1_player,
                Stage2Enemy = label_base3_st2_1_enemy,
                AttachPlayer = label_base3_touch_1_player,
                AttachEnemy = label_base3_touch_1_enemy,
                AirSupCondition = label_base3_1_airsupofplayer,
            };
            Handler.AirCombats[2].Second = new AirBaseCorpsLabelHandler.AirCombatItem()
            {
                Stage1Player = label_base3_st1_2_player,
                Stage1Enemy = label_base3_st1_2_enemy,
                Stage2Player = label_base3_st2_2_player,
                Stage2Enemy = label_base3_st2_2_enemy,
                AttachPlayer = label_base3_touch_2_player,
                AttachEnemy = label_base3_touch_2_enemy,
                AirSupCondition = label_base3_2_airsupofplayer,
            };
        }

    }

    public class AirBaseCorpsLabelHandler
    {
        //飛行隊の状態
        public AirBase[] AirBases { get; set; }
        public AirCombat[] AirCombats { get; set; }


        public AirBaseCorpsLabelHandler()
        {
            AirBases = new AirBase[BaseAirCorp.NumOfAirBase];
            AirCombats = new AirCombat[BaseAirCorp.NumOfAirBase];
        }

        #region 内部クラス
        //基地
        public class AirBase
        {
            public Squadron[] Squadrons { get; set; }
            public Label TotalNum { get; set; }
            public Label TotalCost { get; set; }
            public Label TotalRadius { get; set; }
            public Label TotalDispatch { get; set; }
            public Label TotalAirSup { get; set; }

            public AirBase()
            {
                Squadrons = new Squadron[BaseAirCorp.NumOfSquadron];
            }
        }

        //中隊
        public class Squadron
        {
            public Label PlaneName { get; set; }
            public Label PlaneNum { get; set; }
            public Label Training { get; set; }
            public Label Cost { get; set; }
            public Label Radius { get; set; }
            public Label Dispatch { get; set; }
            public Label AirSupValue { get; set; }
        }

        //航空戦ベース
        public class AirCombat
        {
            //1回目
            public AirCombatItem First { get; set; }
            //2回目
            public AirCombatItem Second { get; set; }
        }

        //1回あたりの航空戦
        public class AirCombatItem
        {
            public Label Stage1Player { get; set; }
            public Label Stage1Enemy { get; set; }
            public Label Stage2Player { get; set; }
            public Label Stage2Enemy { get; set; }
            public Label AttachPlayer { get; set; }
            public Label AttachEnemy { get; set; }
            public Label AirSupCondition { get; set; }
        }
        #endregion
    }

}
