namespace VisualFormTest.DockingWindows
{
    partial class DockWindowCompactScreen
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
            this.compactScreen1 = new VisualFormTest.UserControls.CompactScreen();
            this.SuspendLayout();
            // 
            // compactScreen1
            // 
            this.compactScreen1.BattleView = null;
            this.compactScreen1.Condition = null;
            this.compactScreen1.Fleet = null;
            this.compactScreen1.IsShown = false;
            this.compactScreen1.Location = new System.Drawing.Point(0, 0);
            this.compactScreen1.Material = null;
            this.compactScreen1.Mission = null;
            this.compactScreen1.Name = "compactScreen1";
            this.compactScreen1.Ndock = null;
            this.compactScreen1.Num = null;
            this.compactScreen1.Senka = null;
            this.compactScreen1.Size = new System.Drawing.Size(365, 218);
            this.compactScreen1.TabIndex = 0;
            // 
            // DockWindowCompactScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 222);
            this.Controls.Add(this.compactScreen1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowCompactScreen";
            this.Text = "省スペース表示";
            this.DockStateChanged += new System.EventHandler(this.DockWindowCompactScreen_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.CompactScreen compactScreen1;

    }
}