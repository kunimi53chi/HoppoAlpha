namespace VisualFormTest.DockingWindows
{
    partial class DockWindowBattleState
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockWindowBattleState));
            this.battleState1 = new VisualFormTest.UserControls.BattleState();
            this.SuspendLayout();
            // 
            // battleState1
            // 
            this.battleState1.Location = new System.Drawing.Point(0, 0);
            this.battleState1.Margin = new System.Windows.Forms.Padding(0);
            this.battleState1.Name = "battleState1";
            this.battleState1.Size = new System.Drawing.Size(170, 78);
            this.battleState1.TabIndex = 0;
            // 
            // DockWindowBattleState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(177, 75);
            this.Controls.Add(this.battleState1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockWindowBattleState";
            this.Text = "簡易戦況";
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.BattleState battleState1;

    }
}