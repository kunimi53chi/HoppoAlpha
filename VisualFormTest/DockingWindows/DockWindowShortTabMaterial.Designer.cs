namespace VisualFormTest.DockingWindows
{
    partial class DockWindowShortTabMaterial
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
            this.tabMaterialShort1 = new VisualFormTest.UserControls.TabMaterialShort();
            this.SuspendLayout();
            // 
            // tabMaterialShort1
            // 
            this.tabMaterialShort1.BackColor = System.Drawing.Color.White;
            this.tabMaterialShort1.Init2Finished = false;
            this.tabMaterialShort1.InitFinished = false;
            this.tabMaterialShort1.IsShown = false;
            this.tabMaterialShort1.Location = new System.Drawing.Point(0, 0);
            this.tabMaterialShort1.Name = "tabMaterialShort1";
            this.tabMaterialShort1.Size = new System.Drawing.Size(380, 100);
            this.tabMaterialShort1.TabIndex = 0;
            // 
            // DockWindowShortTabMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(386, 103);
            this.Controls.Add(this.tabMaterialShort1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowShortTabMaterial";
            this.Text = "S資材";
            this.DockStateChanged += new System.EventHandler(this.DockWindowShortTabFleet_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.TabMaterialShort tabMaterialShort1;
    }
}