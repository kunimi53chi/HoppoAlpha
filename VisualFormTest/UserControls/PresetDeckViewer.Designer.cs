namespace VisualFormTest.UserControls
{
    partial class PresetDeckViewer
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox_duplicate = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_exceptFlagship = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_duplicate
            // 
            this.textBox_duplicate.BackColor = System.Drawing.Color.White;
            this.textBox_duplicate.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox_duplicate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_duplicate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_duplicate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.textBox_duplicate.Location = new System.Drawing.Point(0, 0);
            this.textBox_duplicate.Multiline = true;
            this.textBox_duplicate.Name = "textBox_duplicate";
            this.textBox_duplicate.ReadOnly = true;
            this.textBox_duplicate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_duplicate.Size = new System.Drawing.Size(170, 150);
            this.textBox_duplicate.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_exceptFlagship});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem_exceptFlagship
            // 
            this.toolStripMenuItem_exceptFlagship.Name = "toolStripMenuItem_exceptFlagship";
            this.toolStripMenuItem_exceptFlagship.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem_exceptFlagship.Text = "旗艦の重複を無視する";
            this.toolStripMenuItem_exceptFlagship.Click += new System.EventHandler(this.toolStripMenuItem_exceptFlagship_Click);
            // 
            // PresetDeckViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_duplicate);
            this.Name = "PresetDeckViewer";
            this.Size = new System.Drawing.Size(170, 150);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_duplicate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_exceptFlagship;
    }
}
