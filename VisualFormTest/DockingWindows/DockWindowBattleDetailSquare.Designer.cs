namespace VisualFormTest.DockingWindows
{
    partial class DockWindowBattleDetailSquare
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            VisualFormTest.UserControls.BattleDetailLabelHandler battleDetailLabelHandler1 = new VisualFormTest.UserControls.BattleDetailLabelHandler();
            VisualFormTest.UserControls.BattleDetailLabelHandler.AirBattleLabels airBattleLabels1 = new VisualFormTest.UserControls.BattleDetailLabelHandler.AirBattleLabels();
            VisualFormTest.UserControls.BattleDetailLabelHandler.EnemyFleetLabels enemyFleetLabels1 = new VisualFormTest.UserControls.BattleDetailLabelHandler.EnemyFleetLabels();
            VisualFormTest.UserControls.BattleDetailLabelHandler.OverviewLabels overviewLabels1 = new VisualFormTest.UserControls.BattleDetailLabelHandler.OverviewLabels();
            VisualFormTest.UserControls.BattleDetailLabelHandler.SituationLabels situationLabels1 = new VisualFormTest.UserControls.BattleDetailLabelHandler.SituationLabels();
            this.battleDetailSquare1 = new VisualFormTest.UserControls.BattleDetailSquare();
            this.SuspendLayout();
            // 
            // battleDetailSquare1
            // 
            this.battleDetailSquare1.BackColor = System.Drawing.Color.White;
            this.battleDetailSquare1.IsShown = false;
            airBattleLabels1.AirSupValueEnemy = null;
            airBattleLabels1.AirSupValueFriend = null;
            airBattleLabels1.Stage1Enemy = null;
            airBattleLabels1.Stage1Friend = null;
            airBattleLabels1.Stage2Enemy = null;
            airBattleLabels1.Stage2Friend = null;
            battleDetailLabelHandler1.AirBattle = airBattleLabels1;
            battleDetailLabelHandler1.Damage = null;
            battleDetailLabelHandler1.DamageCombined = null;
            enemyFleetLabels1.Equips = null;
            enemyFleetLabels1.Header = null;
            enemyFleetLabels1.Name = null;
            battleDetailLabelHandler1.EnemyFleet = enemyFleetLabels1;
            overviewLabels1.BattleCount = null;
            overviewLabels1.GaugeDetail = null;
            overviewLabels1.GaugeRatio = null;
            overviewLabels1.Header = null;
            overviewLabels1.ID = null;
            overviewLabels1.MapCell = null;
            battleDetailLabelHandler1.Overview = overviewLabels1;
            situationLabels1.AirSup = null;
            situationLabels1.AttachEnemy = null;
            situationLabels1.AttachFriend = null;
            situationLabels1.Engagement = null;
            situationLabels1.FormationEnemy = null;
            situationLabels1.FormationFriend = null;
            situationLabels1.SearchEnemy = null;
            situationLabels1.SearchFriend = null;
            battleDetailLabelHandler1.Situation = situationLabels1;
            this.battleDetailSquare1.LabelHandler = battleDetailLabelHandler1;
            this.battleDetailSquare1.Location = new System.Drawing.Point(0, 0);
            this.battleDetailSquare1.Name = "battleDetailSquare1";
            this.battleDetailSquare1.Size = new System.Drawing.Size(560, 405);
            this.battleDetailSquare1.TabIndex = 0;
            // 
            // DockWindowBattleDetailSquare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 411);
            this.Controls.Add(this.battleDetailSquare1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowBattleDetailSquare";
            this.Text = "戦闘詳細２";
            this.DockStateChanged += new System.EventHandler(this.DockWindowBattleDetailSquare_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.BattleDetailSquare battleDetailSquare1;

    }
}