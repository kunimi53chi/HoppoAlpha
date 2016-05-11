namespace VisualFormTest.DockingWindows
{
    partial class DockWindowTimerViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockWindowTimerViewer));
            this.timerViewer1 = new VisualFormTest.UserControls.TimerViewer();
            this.SuspendLayout();
            // 
            // timerViewer1
            // 
            this.timerViewer1.Location = new System.Drawing.Point(0, 0);
            this.timerViewer1.Name = "timerViewer1";
            this.timerViewer1.Size = new System.Drawing.Size(170, 250);
            this.timerViewer1.TabIndex = 0;
            // 
            // DockWindowTimerViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(175, 277);
            this.Controls.Add(this.timerViewer1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockWindowTimerViewer";
            this.Text = "タイマー情報";
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.TimerViewer timerViewer1;


    }
}