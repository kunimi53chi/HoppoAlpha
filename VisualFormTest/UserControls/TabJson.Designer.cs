namespace VisualFormTest.UserControls
{
    partial class TabJson
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
            this.textBox_json = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_json
            // 
            this.textBox_json.BackColor = System.Drawing.Color.White;
            this.textBox_json.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_json.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_json.HideSelection = false;
            this.textBox_json.Location = new System.Drawing.Point(0, 0);
            this.textBox_json.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_json.Multiline = true;
            this.textBox_json.Name = "textBox_json";
            this.textBox_json.ReadOnly = true;
            this.textBox_json.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_json.Size = new System.Drawing.Size(994, 155);
            this.textBox_json.TabIndex = 6;
            this.textBox_json.WordWrap = false;
            // 
            // TabJson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_json);
            this.Name = "TabJson";
            this.Size = new System.Drawing.Size(994, 155);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_json;
    }
}
