namespace VisualFormTest.DockingWindows
{
    partial class DockWindowPracticeInfo
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
            this.practiceInfo1 = new VisualFormTest.UserControls.PracticeInfo();
            this.SuspendLayout();
            // 
            // practiceInfo1
            // 
            this.practiceInfo1.IsShown = false;
            this.practiceInfo1.Location = new System.Drawing.Point(0, 0);
            this.practiceInfo1.Name = "practiceInfo1";
            this.practiceInfo1.Size = new System.Drawing.Size(800, 360);
            this.practiceInfo1.TabIndex = 0;
            // 
            // DockWindowPracticeInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(794, 366);
            this.Controls.Add(this.practiceInfo1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowPracticeInfo";
            this.Text = "演習情報";
            this.DockStateChanged += new System.EventHandler(this.DockWindowPracticeInfo_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.PracticeInfo practiceInfo1;

    }
}