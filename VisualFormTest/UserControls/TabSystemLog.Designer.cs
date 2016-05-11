namespace VisualFormTest.UserControls
{
    partial class TabSystemLog
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
            this.textBox_systemlog = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // textBox_systemlog
            // 
            this.textBox_systemlog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox_systemlog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_systemlog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_systemlog.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_systemlog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.textBox_systemlog.HideSelection = false;
            this.textBox_systemlog.Location = new System.Drawing.Point(0, 0);
            this.textBox_systemlog.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_systemlog.Multiline = true;
            this.textBox_systemlog.Name = "textBox_systemlog";
            this.textBox_systemlog.ReadOnly = true;
            this.textBox_systemlog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_systemlog.Size = new System.Drawing.Size(994, 155);
            this.textBox_systemlog.TabIndex = 7;
            this.textBox_systemlog.WordWrap = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TabSystemLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_systemlog);
            this.Name = "TabSystemLog";
            this.Size = new System.Drawing.Size(994, 155);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_systemlog;
        private System.Windows.Forms.Timer timer1;
    }
}
