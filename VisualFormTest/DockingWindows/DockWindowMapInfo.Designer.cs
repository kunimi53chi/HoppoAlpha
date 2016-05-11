namespace VisualFormTest.DockingWindows
{
    partial class DockWindowMapInfo
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
            this.mapInfo1 = new VisualFormTest.UserControls.MapInfo();
            this.SuspendLayout();
            // 
            // mapInfo1
            // 
            this.mapInfo1.BackColor = System.Drawing.SystemColors.Window;
            this.mapInfo1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.mapInfo1.IsShown = false;
            this.mapInfo1.Location = new System.Drawing.Point(0, 0);
            this.mapInfo1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mapInfo1.Name = "mapInfo1";
            this.mapInfo1.Size = new System.Drawing.Size(255, 170);
            this.mapInfo1.TabIndex = 0;
            // 
            // DockWindowMapInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 170);
            this.Controls.Add(this.mapInfo1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "DockWindowMapInfo";
            this.Text = "マップ情報";
            this.DockStateChanged += new System.EventHandler(this.DockWindowMapInfo_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.MapInfo mapInfo1;

    }
}