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
    public partial class BattleDetailSquare : UserControl
    {
        public BattleDetailLabelHandler LabelHandler { get; set; }
        public bool IsShown { get; set; }

        private bool clicked = false;

        public BattleDetailSquare()
        {
            InitializeComponent();

            toolStripMenuItem_keep.Checked = Config.BattleDetailViewKeeping;
        }

        //ハンドラーの初期化
        public void Init()
        {
            this.LabelHandler = new BattleDetailLabelHandler();

            //ハンドラー
            //概況
            this.LabelHandler.Overview = new BattleDetailLabelHandler.OverviewLabels()
            {
                Header = label_overview_header,
                MapCell = label_overview_mapcell,
                ID = label_overview_eid,
                BattleCount = label_overview_battlecount,
                GaugeRatio = label_overview_ratio,
                GaugeDetail = label_overview_percent,
            };

            //会戦
            this.LabelHandler.Situation = new BattleDetailLabelHandler.SituationLabels()
            {
                FormationFriend = label_situation_formation1,
                FormationEnemy = label_situation_formation2,
                SearchFriend = label_situation_search1,
                SearchEnemy = label_situation_search2,
                Engagement = label_situation_engagement,
                AirSup = label_situation_airsup,
                AttachFriend = label_situation_attach1,
                AttachEnemy = label_situation_attach2,
            };

            //航空戦
            this.LabelHandler.AirBattle = new BattleDetailLabelHandler.AirBattleLabels()
            {
                Stage1Friend = label_air_st11,
                Stage1Enemy = label_air_st12,
                Stage2Friend = label_air_st21,
                Stage2Enemy = label_air_st22,
                AirSupValueFriend = label_air_supval1,
                AirSupValueEnemy = label_air_supval2,
                AirBaseFriend = label_airbase_val1,
                AirBaseEnemy = label_airbase_val2,
            };

            //総ダメージ（自軍＋敵軍　縦長のとは異なる）
            this.LabelHandler.Damage = new Label[][]
            {
                new Label[]
                {
                    label_damage_11, label_damage_12, label_damage_13, label_damage_14, label_damage_15,
                    label_damage_enemy_12, label_damage_enemy_13,
                },
                new Label[]
                {
                    label_damage_21, label_damage_22, label_damage_23, label_damage_24, label_damage_25,
                    label_damage_enemy_22, label_damage_enemy_23,
                },
                new Label[]
                {
                    label_damage_31, label_damage_32, label_damage_33, label_damage_34, label_damage_35,
                    label_damage_enemy_32, label_damage_enemy_33,
                },
                new Label[]
                {
                    label_damage_41, label_damage_42, label_damage_43, label_damage_44, label_damage_45,
                    label_damage_enemy_42, label_damage_enemy_43,
                },
                new Label[]
                {
                    label_damage_51, label_damage_52, label_damage_53, label_damage_54, label_damage_55,
                    label_damage_enemy_52, label_damage_enemy_53,
                },
                new Label[]
                {
                    label_damage_61, label_damage_62, label_damage_63, label_damage_64, label_damage_65,
                    label_damage_enemy_62, label_damage_enemy_63,
                },
            };

            //連合艦隊
            this.LabelHandler.DamageCombined = new Label[][]
            {
                new Label[]{label_damage_combined_11, label_damage_combined_12, label_damage_combined_13, label_damage_combined_14, label_damage_combined_15,},
                new Label[]{label_damage_combined_21, label_damage_combined_22, label_damage_combined_23, label_damage_combined_24, label_damage_combined_25,},
                new Label[]{label_damage_combined_31, label_damage_combined_32, label_damage_combined_33, label_damage_combined_34, label_damage_combined_35,},
                new Label[]{label_damage_combined_41, label_damage_combined_42, label_damage_combined_43, label_damage_combined_44, label_damage_combined_45,},
                new Label[]{label_damage_combined_51, label_damage_combined_52, label_damage_combined_53, label_damage_combined_54, label_damage_combined_55,},
                new Label[]{label_damage_combined_61, label_damage_combined_62, label_damage_combined_63, label_damage_combined_64, label_damage_combined_65,},
            };

            //敵編成
            this.LabelHandler.EnemyFleet = new BattleDetailLabelHandler.EnemyFleetLabels()
            {
                Header = label_enemy_header,
                Equips = new Label[][]
                {
                    new Label[]{label_enemy_equip11, label_enemy_equip12, label_enemy_equip13, label_enemy_equip14, label_enemy_equip15},
                    new Label[]{label_enemy_equip21, label_enemy_equip22, label_enemy_equip23, label_enemy_equip24, label_enemy_equip25},
                    new Label[]{label_enemy_equip31, label_enemy_equip32, label_enemy_equip33, label_enemy_equip34, label_enemy_equip35},
                    new Label[]{label_enemy_equip41, label_enemy_equip42, label_enemy_equip43, label_enemy_equip44, label_enemy_equip45},
                    new Label[]{label_enemy_equip51, label_enemy_equip52, label_enemy_equip53, label_enemy_equip54, label_enemy_equip55},
                    new Label[]{label_enemy_equip61, label_enemy_equip62, label_enemy_equip63, label_enemy_equip64, label_enemy_equip65},
                },
                Name = new Label[]
                {
                    label_damage_enemy_11, label_damage_enemy_21, label_damage_enemy_31, label_damage_enemy_41, label_damage_enemy_51, label_damage_enemy_61,
                }
            };
        }

        //コントロールのアップデート
        public void ControlUpdate_Q()
        {
            if (!IsShown) return;
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
            //戦闘詳細3の場合
            var form = this.FindForm();
            if (form is DockingWindows.DockWindowBattleDetailSquare2)
            {
                HelperScreen.ScreenShot(this, "battledetail", 7, 115);
            }
            //その他の場合
            else
            {
                HelperScreen.ScreenShot(this, "battledetail");
            }
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
            if(isMainItem)
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
                else if(form is DockingWindows.DockWindowBattleDetailSquare2)
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
}
