namespace VisualFormTest.UserControls
{
    partial class SankWarning
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
            this.textBox_sankwarning = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_sankwarning
            // 
            this.textBox_sankwarning.BackColor = System.Drawing.Color.White;
            this.textBox_sankwarning.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_sankwarning.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_sankwarning.ForeColor = System.Drawing.Color.White;
            this.textBox_sankwarning.Location = new System.Drawing.Point(0, 0);
            this.textBox_sankwarning.Multiline = true;
            this.textBox_sankwarning.Name = "textBox_sankwarning";
            this.textBox_sankwarning.ReadOnly = true;
            this.textBox_sankwarning.Size = new System.Drawing.Size(170, 36);
            this.textBox_sankwarning.TabIndex = 13;
            this.textBox_sankwarning.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SankWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_sankwarning);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SankWarning";
            this.Size = new System.Drawing.Size(170, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox_sankwarning;

    }
}
