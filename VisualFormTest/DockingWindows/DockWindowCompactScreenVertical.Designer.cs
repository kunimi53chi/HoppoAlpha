namespace VisualFormTest.DockingWindows
{
    partial class DockWindowCompactScreenVertical
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
            this.compactScreenVertical1 = new VisualFormTest.UserControls.CompactScreenVertical();
            this.SuspendLayout();
            // 
            // compactScreenVertical1
            // 
            this.compactScreenVertical1.BattleView = null;
            this.compactScreenVertical1.Condition = null;
            this.compactScreenVertical1.Fleet = null;
            this.compactScreenVertical1.IsShown = false;
            this.compactScreenVertical1.Location = new System.Drawing.Point(0, 0);
            this.compactScreenVertical1.Material = null;
            this.compactScreenVertical1.Mission = null;
            this.compactScreenVertical1.Name = "compactScreenVertical1";
            this.compactScreenVertical1.Ndock = null;
            this.compactScreenVertical1.Num = null;
            this.compactScreenVertical1.Senka = null;
            this.compactScreenVertical1.Size = new System.Drawing.Size(175, 520);
            this.compactScreenVertical1.TabIndex = 0;
            // 
            // DockWindowCompactScreenVertical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(180, 521);
            this.Controls.Add(this.compactScreenVertical1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowCompactScreenVertical";
            this.Text = "省スペース表示(縦)";
            this.DockStateChanged += new System.EventHandler(this.DockWindowCompactScreenVertical_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.CompactScreenVertical compactScreenVertical1;

    }
}