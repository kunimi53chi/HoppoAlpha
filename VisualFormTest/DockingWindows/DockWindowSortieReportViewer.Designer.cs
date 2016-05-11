namespace VisualFormTest.DockingWindows
{
    partial class DockWindowSortieReportViewer
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
            this.sortieReportViewer1 = new VisualFormTest.UserControls.SortieReportViewer();
            this.SuspendLayout();
            // 
            // sortieReportViewer1
            // 
            this.sortieReportViewer1.IsShown = false;
            this.sortieReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.sortieReportViewer1.Name = "sortieReportViewer1";
            this.sortieReportViewer1.Size = new System.Drawing.Size(800, 530);
            this.sortieReportViewer1.TabIndex = 0;
            // 
            // DockWindowSortieReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 533);
            this.Controls.Add(this.sortieReportViewer1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowSortieReportViewer";
            this.Text = "出撃報告書";
            this.DockStateChanged += new System.EventHandler(this.DockWindowSortieReportViewer_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.SortieReportViewer sortieReportViewer1;

    }
}