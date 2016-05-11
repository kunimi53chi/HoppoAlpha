namespace VisualFormTest.DockingWindows
{
    partial class DockWindowDropAnalyzer
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
            this.dropAnalyzer1 = new VisualFormTest.UserControls.DropAnalyzer();
            this.SuspendLayout();
            // 
            // dropAnalyzer1
            // 
            this.dropAnalyzer1.IsShown = false;
            this.dropAnalyzer1.Location = new System.Drawing.Point(0, 0);
            this.dropAnalyzer1.Name = "dropAnalyzer1";
            this.dropAnalyzer1.Size = new System.Drawing.Size(540, 455);
            this.dropAnalyzer1.TabIndex = 0;
            // 
            // DockWindowDropAnalyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 449);
            this.Controls.Add(this.dropAnalyzer1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowDropAnalyzer";
            this.Text = "ドロップ解析";
            this.DockStateChanged += new System.EventHandler(this.DockWindowDropAnalyzer_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.DropAnalyzer dropAnalyzer1;

    }
}