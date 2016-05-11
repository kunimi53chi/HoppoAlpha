namespace VisualFormTest.DockingWindows
{
    partial class DockWindowQuestViewer
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
            this.questViewer1 = new VisualFormTest.UserControls.QuestViewer();
            this.SuspendLayout();
            // 
            // questViewer1
            // 
            this.questViewer1.BackColor = System.Drawing.Color.White;
            this.questViewer1.IsShown = false;
            this.questViewer1.Labels = null;
            this.questViewer1.Location = new System.Drawing.Point(0, 0);
            this.questViewer1.Name = "questViewer1";
            this.questViewer1.Size = new System.Drawing.Size(170, 243);
            this.questViewer1.TabIndex = 0;
            // 
            // DockWindowQuestViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 251);
            this.Controls.Add(this.questViewer1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowQuestViewer";
            this.Text = "任務";
            this.DockStateChanged += new System.EventHandler(this.DockWindowQuestViewer_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.QuestViewer questViewer1;

    }
}