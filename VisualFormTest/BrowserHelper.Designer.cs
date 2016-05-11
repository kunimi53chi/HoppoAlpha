namespace VisualFormTest
{
    partial class BrowserHelper
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.button_scrollreset = new System.Windows.Forms.Button();
            this.button_scrollapply = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_urlnavigate = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button_urlfavorite = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button_style = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "左";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "上";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(70, 38);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(64, 19);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(70, 73);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(64, 19);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // button_scrollreset
            // 
            this.button_scrollreset.Location = new System.Drawing.Point(185, 40);
            this.button_scrollreset.Name = "button_scrollreset";
            this.button_scrollreset.Size = new System.Drawing.Size(75, 23);
            this.button_scrollreset.TabIndex = 4;
            this.button_scrollreset.Text = "リセット";
            this.button_scrollreset.UseVisualStyleBackColor = true;
            this.button_scrollreset.Click += new System.EventHandler(this.button_scrollreset_Click);
            // 
            // button_scrollapply
            // 
            this.button_scrollapply.Location = new System.Drawing.Point(185, 90);
            this.button_scrollapply.Name = "button_scrollapply";
            this.button_scrollapply.Size = new System.Drawing.Size(87, 23);
            this.button_scrollapply.TabIndex = 5;
            this.button_scrollapply.Text = "微調整の適用";
            this.button_scrollapply.UseVisualStyleBackColor = true;
            this.button_scrollapply.Click += new System.EventHandler(this.button_scrollapply_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(15, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "ブラウザの位置を微調整します";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(15, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "ブラウザのアドレス";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(25, 180);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(245, 40);
            this.textBox1.TabIndex = 8;
            // 
            // button_urlnavigate
            // 
            this.button_urlnavigate.Location = new System.Drawing.Point(185, 230);
            this.button_urlnavigate.Name = "button_urlnavigate";
            this.button_urlnavigate.Size = new System.Drawing.Size(85, 23);
            this.button_urlnavigate.TabIndex = 9;
            this.button_urlnavigate.Text = "Navigate";
            this.button_urlnavigate.UseVisualStyleBackColor = true;
            this.button_urlnavigate.Click += new System.EventHandler(this.button_urlnavigate_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(25, 290);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(245, 20);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button_urlfavorite
            // 
            this.button_urlfavorite.Location = new System.Drawing.Point(27, 230);
            this.button_urlfavorite.Name = "button_urlfavorite";
            this.button_urlfavorite.Size = new System.Drawing.Size(85, 23);
            this.button_urlfavorite.TabIndex = 11;
            this.button_urlfavorite.Text = "URLメモに追加";
            this.button_urlfavorite.UseVisualStyleBackColor = true;
            this.button_urlfavorite.Click += new System.EventHandler(this.button_urlfavorite_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(27, 335);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(100, 23);
            this.button_ok.TabIndex = 12;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(170, 335);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 23);
            this.button_cancel.TabIndex = 13;
            this.button_cancel.Text = "キャンセル";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(27, 262);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(177, 16);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "ブラウザのスクロールを有効にする";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button_style
            // 
            this.button_style.Location = new System.Drawing.Point(27, 105);
            this.button_style.Name = "button_style";
            this.button_style.Size = new System.Drawing.Size(120, 23);
            this.button_style.TabIndex = 15;
            this.button_style.Text = "ゲーム画面の再抽出";
            this.button_style.UseVisualStyleBackColor = true;
            this.button_style.Click += new System.EventHandler(this.button_style_Click);
            // 
            // BrowserHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 413);
            this.Controls.Add(this.button_style);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_urlfavorite);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button_urlnavigate);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_scrollapply);
            this.Controls.Add(this.button_scrollreset);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BrowserHelper";
            this.Text = "ブラウザツール";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button button_scrollreset;
        private System.Windows.Forms.Button button_scrollapply;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_urlnavigate;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button_urlfavorite;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button_style;
    }
}