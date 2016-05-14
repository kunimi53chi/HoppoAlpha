namespace VisualFormTest.DockingWindows
{
    partial class DockWindowAirBaseCorps
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
            this.airBaseCorps1 = new VisualFormTest.UserControls.AirBaseCorps();
            this.SuspendLayout();
            // 
            // airBaseCorps1
            // 
            this.airBaseCorps1.BackColor = System.Drawing.Color.White;
            this.airBaseCorps1.Handler = null;
            this.airBaseCorps1.IsShown = false;
            this.airBaseCorps1.Location = new System.Drawing.Point(0, 0);
            this.airBaseCorps1.Name = "airBaseCorps1";
            this.airBaseCorps1.Size = new System.Drawing.Size(800, 360);
            this.airBaseCorps1.TabIndex = 0;
            this.airBaseCorps1.ToolTip = null;
            // 
            // DockWindowAirBaseCorps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 368);
            this.Controls.Add(this.airBaseCorps1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowAirBaseCorps";
            this.Text = "基地航空隊";
            this.DockStateChanged += new System.EventHandler(this.DockWindowAirBaseCorps_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.AirBaseCorps airBaseCorps1;

    }
}