namespace VisualFormTest.DockingWindows
{
    partial class DockWindowSankWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockWindowSankWarning));
            this.sankWarning1 = new VisualFormTest.UserControls.SankWarning();
            this.SuspendLayout();
            // 
            // sankWarning1
            // 
            this.sankWarning1.Location = new System.Drawing.Point(0, 0);
            this.sankWarning1.Margin = new System.Windows.Forms.Padding(0);
            this.sankWarning1.Name = "sankWarning1";
            this.sankWarning1.Size = new System.Drawing.Size(170, 36);
            this.sankWarning1.TabIndex = 0;
            // 
            // DockWindowSankWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 59);
            this.Controls.Add(this.sankWarning1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockWindowSankWarning";
            this.Text = "大破チェッカー";
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.SankWarning sankWarning1;

    }
}