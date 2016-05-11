namespace VisualFormTest.UserControls
{
    partial class ToolBox
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
            this.numericUpDown_tool_combine = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.button_tool_combine = new System.Windows.Forms.Button();
            this.button_tool_black2 = new System.Windows.Forms.Button();
            this.button_tool_black = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.button_tool_trimcombine = new System.Windows.Forms.Button();
            this.button_tool_trimfleet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_tool_combine)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown_tool_combine
            // 
            this.numericUpDown_tool_combine.Location = new System.Drawing.Point(237, 165);
            this.numericUpDown_tool_combine.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_tool_combine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_tool_combine.Name = "numericUpDown_tool_combine";
            this.numericUpDown_tool_combine.Size = new System.Drawing.Size(60, 19);
            this.numericUpDown_tool_combine.TabIndex = 18;
            this.numericUpDown_tool_combine.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_tool_combine.ValueChanged += new System.EventHandler(this.numericUpDown_tool_combine_ValueChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(235, 150);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(73, 12);
            this.label34.TabIndex = 17;
            this.label34.Text = "結合の列の数";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(160, 190);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(63, 12);
            this.label30.TabIndex = 16;
            this.label30.Text = "画像の結合";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(131, 90);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(154, 12);
            this.label29.TabIndex = 14;
            this.label29.Text = "提督名を黒塗り（戦闘結果用）";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(0, 90);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(130, 12);
            this.label28.TabIndex = 12;
            this.label28.Text = "提督名を黒塗り（母港用）";
            // 
            // button_tool_combine
            // 
            this.button_tool_combine.AllowDrop = true;
            this.button_tool_combine.BackgroundImage = global::VisualFormTest.Properties.Resources.circle_plus_2x;
            this.button_tool_combine.Location = new System.Drawing.Point(160, 120);
            this.button_tool_combine.Name = "button_tool_combine";
            this.button_tool_combine.Size = new System.Drawing.Size(64, 64);
            this.button_tool_combine.TabIndex = 15;
            this.button_tool_combine.UseVisualStyleBackColor = true;
            // 
            // button_tool_black2
            // 
            this.button_tool_black2.AllowDrop = true;
            this.button_tool_black2.BackgroundImage = global::VisualFormTest.Properties.Resources.confused_2x;
            this.button_tool_black2.Location = new System.Drawing.Point(160, 15);
            this.button_tool_black2.Margin = new System.Windows.Forms.Padding(0);
            this.button_tool_black2.Name = "button_tool_black2";
            this.button_tool_black2.Size = new System.Drawing.Size(64, 64);
            this.button_tool_black2.TabIndex = 13;
            this.button_tool_black2.UseVisualStyleBackColor = true;
            // 
            // button_tool_black
            // 
            this.button_tool_black.AllowDrop = true;
            this.button_tool_black.BackgroundImage = global::VisualFormTest.Properties.Resources.geek_2x;
            this.button_tool_black.Location = new System.Drawing.Point(35, 15);
            this.button_tool_black.Name = "button_tool_black";
            this.button_tool_black.Size = new System.Drawing.Size(64, 64);
            this.button_tool_black.TabIndex = 11;
            this.button_tool_black.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(28, 190);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(87, 24);
            this.label31.TabIndex = 20;
            this.label31.Text = "画像を編成用に\r\nトリミングして結合";
            // 
            // button_tool_trimcombine
            // 
            this.button_tool_trimcombine.AllowDrop = true;
            this.button_tool_trimcombine.BackgroundImage = global::VisualFormTest.Properties.Resources.cloud_plus_2x;
            this.button_tool_trimcombine.Location = new System.Drawing.Point(35, 120);
            this.button_tool_trimcombine.Name = "button_tool_trimcombine";
            this.button_tool_trimcombine.Size = new System.Drawing.Size(64, 64);
            this.button_tool_trimcombine.TabIndex = 19;
            this.button_tool_trimcombine.UseVisualStyleBackColor = true;
            // 
            // button_tool_trimfleet
            // 
            this.button_tool_trimfleet.AllowDrop = true;
            this.button_tool_trimfleet.BackgroundImage = global::VisualFormTest.Properties.Resources.crop_2x;
            this.button_tool_trimfleet.Location = new System.Drawing.Point(285, 15);
            this.button_tool_trimfleet.Margin = new System.Windows.Forms.Padding(0);
            this.button_tool_trimfleet.Name = "button_tool_trimfleet";
            this.button_tool_trimfleet.Size = new System.Drawing.Size(64, 64);
            this.button_tool_trimfleet.TabIndex = 21;
            this.button_tool_trimfleet.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 24);
            this.label1.TabIndex = 22;
            this.label1.Text = "編成画面の\r\nトリミング";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_tool_trimfleet);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.button_tool_trimcombine);
            this.Controls.Add(this.numericUpDown_tool_combine);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.button_tool_combine);
            this.Controls.Add(this.button_tool_black2);
            this.Controls.Add(this.button_tool_black);
            this.Name = "ToolBox";
            this.Size = new System.Drawing.Size(365, 225);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_tool_combine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown_tool_combine;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button button_tool_combine;
        private System.Windows.Forms.Button button_tool_black2;
        private System.Windows.Forms.Button button_tool_black;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button button_tool_trimcombine;
        private System.Windows.Forms.Button button_tool_trimfleet;
        private System.Windows.Forms.Label label1;
    }
}
