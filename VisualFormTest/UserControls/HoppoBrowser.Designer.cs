namespace VisualFormTest.UserControls
{
    partial class HoppoBrowser
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
            this.extraWebBrowser1 = new VisualFormTest.UserControls.ExtraWebBrowser();
            this.SuspendLayout();
            // 
            // extraWebBrowser1
            // 
            this.extraWebBrowser1.AllowWebBrowserDrop = false;
            this.extraWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.extraWebBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.extraWebBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.extraWebBrowser1.Name = "extraWebBrowser1";
            this.extraWebBrowser1.ScriptErrorsSuppressed = true;
            this.extraWebBrowser1.ScrollBarsEnabled = false;
            this.extraWebBrowser1.Size = new System.Drawing.Size(800, 480);
            this.extraWebBrowser1.TabIndex = 0;
            this.extraWebBrowser1.ReplacedKeyDown += new System.Windows.Forms.KeyEventHandler(this.Browser_ReplacedKeyDown);
            this.extraWebBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.extraWebBrowser1_DocumentCompleted);
            // 
            // HoppoBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extraWebBrowser1);
            this.Name = "HoppoBrowser";
            this.Size = new System.Drawing.Size(800, 480);
            this.ResumeLayout(false);

        }

        #endregion

        internal ExtraWebBrowser extraWebBrowser1;





    }
}
