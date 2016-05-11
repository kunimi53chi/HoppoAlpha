namespace VisualFormTest.DockingWindows
{
    partial class DockWindowToolBox
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
            this.toolBox1 = new VisualFormTest.UserControls.ToolBox();
            this.SuspendLayout();
            // 
            // toolBox1
            // 
            this.toolBox1.Location = new System.Drawing.Point(0, 0);
            this.toolBox1.Name = "toolBox1";
            this.toolBox1.Size = new System.Drawing.Size(365, 225);
            this.toolBox1.TabIndex = 0;
            // 
            // DockWindowToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 224);
            this.Controls.Add(this.toolBox1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowToolBox";
            this.Text = "ツールボックス";
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.ToolBox toolBox1;

    }
}