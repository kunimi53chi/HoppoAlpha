namespace VisualFormTest.DockingWindows
{
    partial class DockWindowShipSlotitemNum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockWindowShipSlotitemNum));
            this.shipSlotitemNum1 = new VisualFormTest.UserControls.ShipSlotitemNum();
            this.SuspendLayout();
            // 
            // shipSlotitemNum1
            // 
            this.shipSlotitemNum1.Location = new System.Drawing.Point(0, 0);
            this.shipSlotitemNum1.Margin = new System.Windows.Forms.Padding(0);
            this.shipSlotitemNum1.Name = "shipSlotitemNum1";
            this.shipSlotitemNum1.Size = new System.Drawing.Size(170, 50);
            this.shipSlotitemNum1.TabIndex = 0;
            // 
            // DockWindowShipSlotitemNum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 60);
            this.Controls.Add(this.shipSlotitemNum1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockWindowShipSlotitemNum";
            this.Text = "保有アイテム数";
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.ShipSlotitemNum shipSlotitemNum1;

    }
}