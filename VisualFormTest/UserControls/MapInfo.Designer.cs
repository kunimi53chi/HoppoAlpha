namespace VisualFormTest.UserControls
{
    partial class MapInfo
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
            this.textBox_mapinfo = new System.Windows.Forms.TextBox();
            this.checkBox_showcleard = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox_mapinfo
            // 
            this.textBox_mapinfo.BackColor = System.Drawing.Color.White;
            this.textBox_mapinfo.Location = new System.Drawing.Point(0, 0);
            this.textBox_mapinfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_mapinfo.Multiline = true;
            this.textBox_mapinfo.Name = "textBox_mapinfo";
            this.textBox_mapinfo.ReadOnly = true;
            this.textBox_mapinfo.Size = new System.Drawing.Size(245, 155);
            this.textBox_mapinfo.TabIndex = 0;
            // 
            // checkBox_showcleard
            // 
            this.checkBox_showcleard.AutoSize = true;
            this.checkBox_showcleard.Location = new System.Drawing.Point(20, 155);
            this.checkBox_showcleard.Name = "checkBox_showcleard";
            this.checkBox_showcleard.Size = new System.Drawing.Size(160, 19);
            this.checkBox_showcleard.TabIndex = 1;
            this.checkBox_showcleard.Text = "クリア済みのマップを表示する";
            this.checkBox_showcleard.UseVisualStyleBackColor = true;
            this.checkBox_showcleard.CheckedChanged += new System.EventHandler(this.checkBox_showcleard_CheckedChanged);
            // 
            // MapInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.checkBox_showcleard);
            this.Controls.Add(this.textBox_mapinfo);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MapInfo";
            this.Size = new System.Drawing.Size(245, 170);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_mapinfo;
        private System.Windows.Forms.CheckBox checkBox_showcleard;
    }
}
