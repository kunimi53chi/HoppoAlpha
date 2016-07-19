namespace VisualFormTest.UserControls
{
    partial class KisuTetyo
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.extraWebBrowser1 = new VisualFormTest.UserControls.ExtraWebBrowser();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_refresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem_refresh
            // 
            this.toolStripMenuItem_refresh.Name = "toolStripMenuItem_refresh";
            this.toolStripMenuItem_refresh.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_refresh.Text = "更新";
            this.toolStripMenuItem_refresh.Click += new System.EventHandler(this.toolStripMenuItem_refresh_Click);
            // 
            // extraWebBrowser1
            // 
            this.extraWebBrowser1.AllowWebBrowserDrop = false;
            this.extraWebBrowser1.ContextMenuStrip = this.contextMenuStrip1;
            this.extraWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extraWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.extraWebBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.extraWebBrowser1.Name = "extraWebBrowser1";
            this.extraWebBrowser1.ScriptErrorsSuppressed = true;
            this.extraWebBrowser1.ScrollBarsEnabled = false;
            this.extraWebBrowser1.Size = new System.Drawing.Size(800, 480);
            this.extraWebBrowser1.TabIndex = 1;
            this.extraWebBrowser1.WebBrowserShortcutsEnabled = false;
            // 
            // KisuTetyo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extraWebBrowser1);
            this.Name = "KisuTetyo";
            this.Size = new System.Drawing.Size(800, 480);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_refresh;
        private ExtraWebBrowser extraWebBrowser1;
    }
}
