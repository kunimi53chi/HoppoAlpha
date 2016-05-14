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
    public partial class BattleDetail : UserControl
    {
        //ハンドラー
        public BattleDetailLabelHandler LabelHandler { get; set; }

        private bool clicked = false;

        public BattleDetail()
        {
            InitializeComponent();

            toolStripMenuItem_keep.Checked = Config.BattleDetailViewKeeping;
        }

        public void Init()
        {

            //--ハンドラーのセット
            LabelHandler = new BattleDetailLabelHandler();
            //概況
            LabelHandler.Overview = new BattleDetailLabelHandler.OverviewLabels();
            LabelHandler.Overview.Header = label_overview_header;
            LabelHandler.Overview.MapCell = label_overview_mapcell;
            LabelHandler.Overview.ID = label_overview_eid; LabelHandler.Overview.BattleCount = label_overview_battlecount;
            LabelHandler.Overview.GaugeRatio = label_overview_ratio;
            LabelHandler.Overview.GaugeDetail = label_overview_percent;
            //会戦
            LabelHandler.Situation = new BattleDetailLabelHandler.SituationLabels();
            LabelHandler.Situation.FormationFriend = label_situation_formation1; LabelHandler.Situation.FormationEnemy = label_situation_formation2;
            LabelHandler.Situation.SearchFriend = label_situation_search1; LabelHandler.Situation.SearchEnemy = label_situation_search2;
            LabelHandler.Situation.Engagement = label_situation_engagement;
            LabelHandler.Situation.AirSup = label_situation_airsup;
            LabelHandler.Situation.AttachFriend = label_situation_attach1; LabelHandler.Situation.AttachEnemy = label_situation_attach2;
            //航空戦
            LabelHandler.AirBattle = new BattleDetailLabelHandler.AirBattleLabels();
            LabelHandler.AirBattle.Stage1Friend = label_air_st11; LabelHandler.AirBattle.Stage1Enemy = label_air_st12;
            LabelHandler.AirBattle.Stage2Friend = label_air_st21; LabelHandler.AirBattle.Stage2Enemy = label_air_st22;
            LabelHandler.AirBattle.AirSupValueFriend = label_air_supvalue1; LabelHandler.AirBattle.AirSupValueEnemy = label_air_supvalue2;
            //ダメージ
            LabelHandler.Damage = new Label[][]
            {
                new Label[]{label_damage_11, label_damage_12, label_damage_13, label_damage_14, label_damage_15},
                new Label[]{label_damage_21, label_damage_22, label_damage_23, label_damage_24, label_damage_25},
                new Label[]{label_damage_31, label_damage_32, label_damage_33, label_damage_34, label_damage_35},
                new Label[]{label_damage_41, label_damage_42, label_damage_43, label_damage_44, label_damage_45},
                new Label[]{label_damage_51, label_damage_52, label_damage_53, label_damage_54, label_damage_55},
                new Label[]{label_damage_61, label_damage_62, label_damage_63, label_damage_64, label_damage_65},
            };
            //敵編成
            LabelHandler.EnemyFleet = new BattleDetailLabelHandler.EnemyFleetLabels();
            LabelHandler.EnemyFleet.Header = label_enemy_header;
            LabelHandler.EnemyFleet.Name = new Label[6]
            {
                label_enemy_name1, label_enemy_name2, label_enemy_name3, label_enemy_name4, label_enemy_name5, label_enemy_name6,
            };
            LabelHandler.EnemyFleet.Equips = new Label[][]
            {
                new Label[]{label_enemy_equip11, label_enemy_equip12, label_enemy_equip13, label_enemy_equip14},
                new Label[]{label_enemy_equip21, label_enemy_equip22, label_enemy_equip23, label_enemy_equip24},
                new Label[]{label_enemy_equip31, label_enemy_equip32, label_enemy_equip33, label_enemy_equip34},
                new Label[]{label_enemy_equip41, label_enemy_equip42, label_enemy_equip43, label_enemy_equip44},
                new Label[]{label_enemy_equip51, label_enemy_equip52, label_enemy_equip53, label_enemy_equip54},
                new Label[]{label_enemy_equip61, label_enemy_equip62, label_enemy_equip63, label_enemy_equip64},
            };
            //連合艦隊
            LabelHandler.DamageCombined = new Label[][]
            {
                new Label[]{label_damage_combined_11, label_damage_combined_12, label_damage_combined_13},
                new Label[]{label_damage_combined_21, label_damage_combined_22, label_damage_combined_23},
                new Label[]{label_damage_combined_31, label_damage_combined_32, label_damage_combined_33},
                new Label[]{label_damage_combined_41, label_damage_combined_42, label_damage_combined_43},
                new Label[]{label_damage_combined_51, label_damage_combined_52, label_damage_combined_53},
                new Label[]{label_damage_combined_61, label_damage_combined_62, label_damage_combined_63},
            };
        }

        //アップデート
        public void ControlUpdate_Q()
        {
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    try
                    {
                        KancolleBattle.SetBattleDetail(this, toolTip1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "エラーが発生しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }));
        }

        private void toolStripMenuItem_screenshot_Click(object sender, EventArgs e)
        {
            HelperScreen.ScreenShot(this, "battledetail");
        }

        private void toolStripMenuItem_keep_Click(object sender, EventArgs e)
        {
            Config.BattleDetailViewKeeping = !toolStripMenuItem_keep.Checked;
            ViewKeepingChange(true);
        }

        public void ViewKeepingChange(bool isMainItem)
        {
            //押している最中ならば実行しない
            if (clicked) return;
            //コールバックから飛んでこないメインのアイテムの場合
            if (isMainItem)
            {
                clicked = true;
                //タブコレクションを探す
                var form = this.FindForm();
                DockingWindows.DockWindowTabCollection collection = null;
                if (form is DockingWindows.DockWindowBattleDetail)
                {
                    collection = (form as DockingWindows.DockWindowBattleDetail).MainForm.dwPageCollection;
                }
                else if (form is DockingWindows.DockWindowBattleDetailSquare)
                {
                    collection = (form as DockingWindows.DockWindowBattleDetailSquare).MainForm.dwPageCollection;
                }
                else if (form is DockingWindows.DockWindowBattleDetailSquare2)
                {
                    collection = (form as DockingWindows.DockWindowBattleDetailSquare2).MainForm.dwPageCollection;
                }
                //コールバック実行
                if (collection != null) collection.BattleDetail_ViewKeepingChangeCallBack();
            }
            //表示の切り替え
            toolStripMenuItem_keep.Checked = Config.BattleDetailViewKeeping;
            //終了処理
            if (isMainItem) clicked = false;
        }
    }

    //ラベルのハンドラー
    public class BattleDetailLabelHandler
    {
        //概況
        public OverviewLabels Overview { get; set; }
        public SituationLabels Situation { get; set; }
        public AirBattleLabels AirBattle { get; set; }
        public Label[][] Damage { get; set; }
        public EnemyFleetLabels EnemyFleet { get; set; }
        public Label[][] DamageCombined { get; set; }

        //内部クラス
        #region 内部クラス
        //概況
        public class OverviewLabels
        {
            public Label Header { get; set; }
            public Label MapCell { get; set; }
            public Label ID { get; set; }
            public Label BattleCount { get; set; }
            public Label GaugeRatio { get; set; }
            public Label GaugeDetail { get; set; }
        }

        //会戦
        public class SituationLabels
        {
            public Label FormationFriend { get; set; }
            public Label FormationEnemy { get; set; }
            public Label SearchFriend { get; set; }
            public Label SearchEnemy { get; set; }
            public Label Engagement { get; set; }
            public Label AirSup { get; set; }
            public Label AttachFriend { get; set; }
            public Label AttachEnemy { get; set; }
        }

        //航空戦
        public class AirBattleLabels
        {
            public Label AirBaseFriend { get; set; }
            public Label AirBaseEnemy { get; set; }
            public Label Stage1Friend { get; set; }
            public Label Stage1Enemy { get; set; }
            public Label Stage2Friend { get; set; }
            public Label Stage2Enemy { get; set; }
            public Label AirSupValueFriend { get; set; }
            public Label AirSupValueEnemy { get; set; }
        }

        //敵編成
        public class EnemyFleetLabels
        {
            public Label Header { get; set; }
            public Label[] Name { get; set; }
            public Label[][] Equips { get; set; }
        }
        #endregion
    }
}
