namespace VisualFormTest.DockingWindows
{
    partial class DockWindowRankingViewer
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
            this.rankingViewer1 = new VisualFormTest.UserControls.RankingViewer();
            this.SuspendLayout();
            // 
            // rankingViewer1
            // 
            this.rankingViewer1.IsShown = false;
            this.rankingViewer1.Location = new System.Drawing.Point(0, 0);
            this.rankingViewer1.Name = "rankingViewer1";
            this.rankingViewer1.Size = new System.Drawing.Size(1024, 480);
            this.rankingViewer1.TabIndex = 0;
            // 
            // DockWindowRankingViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 488);
            this.Controls.Add(this.rankingViewer1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowRankingViewer";
            this.Text = "ランキング分析";
            this.DockStateChanged += new System.EventHandler(this.DockWindowRankingViewer_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.RankingViewer rankingViewer1;

    }
}