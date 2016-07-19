namespace VisualFormTest.DockingWindows
{
    partial class DockWindowKisuTetyo
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
            this.kisuTetyo1 = new VisualFormTest.UserControls.KisuTetyo();
            this.SuspendLayout();
            // 
            // kisuTetyo1
            // 
            this.kisuTetyo1.IsShown = false;
            this.kisuTetyo1.Location = new System.Drawing.Point(0, 0);
            this.kisuTetyo1.Name = "kisuTetyo1";
            this.kisuTetyo1.Size = new System.Drawing.Size(800, 480);
            this.kisuTetyo1.TabIndex = 0;
            // 
            // DockWindowKisuTetyo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 483);
            this.Controls.Add(this.kisuTetyo1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowKisuTetyo";
            this.Text = "KISU手帳";
            this.DockStateChanged += new System.EventHandler(this.DockWindowPracticeInfo_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.KisuTetyo kisuTetyo1;
    }
}