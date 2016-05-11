namespace VisualFormTest.DockingWindows
{
    partial class DockWindowBattleDetail
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
            VisualFormTest.UserControls.BattleDetailLabelHandler battleDetailLabelHandler2 = new VisualFormTest.UserControls.BattleDetailLabelHandler();
            VisualFormTest.UserControls.BattleDetailLabelHandler.AirBattleLabels airBattleLabels2 = new VisualFormTest.UserControls.BattleDetailLabelHandler.AirBattleLabels();
            VisualFormTest.UserControls.BattleDetailLabelHandler.EnemyFleetLabels enemyFleetLabels2 = new VisualFormTest.UserControls.BattleDetailLabelHandler.EnemyFleetLabels();
            VisualFormTest.UserControls.BattleDetailLabelHandler.OverviewLabels overviewLabels2 = new VisualFormTest.UserControls.BattleDetailLabelHandler.OverviewLabels();
            VisualFormTest.UserControls.BattleDetailLabelHandler.SituationLabels situationLabels2 = new VisualFormTest.UserControls.BattleDetailLabelHandler.SituationLabels();
            this.battleDetail1 = new VisualFormTest.UserControls.BattleDetail();
            this.SuspendLayout();
            // 
            // battleDetail1
            // 
            this.battleDetail1.BackColor = System.Drawing.Color.White;
            airBattleLabels2.AirSupValueEnemy = null;
            airBattleLabels2.AirSupValueFriend = null;
            airBattleLabels2.Stage1Enemy = null;
            airBattleLabels2.Stage1Friend = null;
            airBattleLabels2.Stage2Enemy = null;
            airBattleLabels2.Stage2Friend = null;
            battleDetailLabelHandler2.AirBattle = airBattleLabels2;
            battleDetailLabelHandler2.Damage = null;
            battleDetailLabelHandler2.DamageCombined = null;
            enemyFleetLabels2.Equips = null;
            enemyFleetLabels2.Header = null;
            enemyFleetLabels2.Name = null;
            battleDetailLabelHandler2.EnemyFleet = enemyFleetLabels2;
            overviewLabels2.BattleCount = null;
            overviewLabels2.GaugeDetail = null;
            overviewLabels2.GaugeRatio = null;
            overviewLabels2.Header = null;
            overviewLabels2.ID = null;
            overviewLabels2.MapCell = null;
            battleDetailLabelHandler2.Overview = overviewLabels2;
            situationLabels2.AirSup = null;
            situationLabels2.AttachEnemy = null;
            situationLabels2.AttachFriend = null;
            situationLabels2.Engagement = null;
            situationLabels2.FormationEnemy = null;
            situationLabels2.FormationFriend = null;
            situationLabels2.SearchEnemy = null;
            situationLabels2.SearchFriend = null;
            battleDetailLabelHandler2.Situation = situationLabels2;
            this.battleDetail1.LabelHandler = battleDetailLabelHandler2;
            this.battleDetail1.Location = new System.Drawing.Point(0, 0);
            this.battleDetail1.Name = "battleDetail1";
            this.battleDetail1.Size = new System.Drawing.Size(170, 815);
            this.battleDetail1.TabIndex = 0;
            // 
            // DockWindowBattleDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(178, 809);
            this.Controls.Add(this.battleDetail1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowBattleDetail";
            this.Text = "戦闘詳細";
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.BattleDetail battleDetail1;





    }
}