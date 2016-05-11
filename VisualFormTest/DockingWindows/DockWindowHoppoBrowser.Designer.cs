namespace VisualFormTest.DockingWindows
{
    partial class DockWindowHoppoBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockWindowHoppoBrowser));
            this.hoppoBrowser1 = new VisualFormTest.UserControls.HoppoBrowser();
            this.SuspendLayout();
            // 
            // hoppoBrowser1
            // 
            this.hoppoBrowser1.Location = new System.Drawing.Point(0, 0);
            this.hoppoBrowser1.Name = "hoppoBrowser1";
            this.hoppoBrowser1.Size = new System.Drawing.Size(800, 480);
            this.hoppoBrowser1.TabIndex = 0;
            // 
            // DockWindowHoppoBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 484);
            this.Controls.Add(this.hoppoBrowser1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockWindowHoppoBrowser";
            this.Text = "ブラウザ";
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.HoppoBrowser hoppoBrowser1;

    }
}