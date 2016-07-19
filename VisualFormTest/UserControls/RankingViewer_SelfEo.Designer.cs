namespace VisualFormTest.UserControls
{
    partial class RankingViewer_SelfEo
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown_eo = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_date = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.label_senka = new System.Windows.Forms.Label();
            this.label_diff = new System.Windows.Forms.Label();
            this.label_addedeo = new System.Windows.Forms.Label();
            this.checkBox_15 = new System.Windows.Forms.CheckBox();
            this.checkBox_16 = new System.Windows.Forms.CheckBox();
            this.checkBox_25 = new System.Windows.Forms.CheckBox();
            this.checkBox_35 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_45 = new System.Windows.Forms.CheckBox();
            this.checkBox_55 = new System.Windows.Forms.CheckBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_eo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "日付";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(15, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "提督名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(15, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "戦果";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(135, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "戦果増分";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(15, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "加算済みEO補正値";
            // 
            // numericUpDown_eo
            // 
            this.numericUpDown_eo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown_eo.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_eo.Location = new System.Drawing.Point(95, 164);
            this.numericUpDown_eo.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown_eo.Name = "numericUpDown_eo";
            this.numericUpDown_eo.Size = new System.Drawing.Size(120, 23);
            this.numericUpDown_eo.TabIndex = 5;
            this.numericUpDown_eo.ValueChanged += new System.EventHandler(this.numericUpDown_eo_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(15, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "EO補正値";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label_addedeo);
            this.panel1.Controls.Add(this.label_diff);
            this.panel1.Controls.Add(this.label_senka);
            this.panel1.Controls.Add(this.label_name);
            this.panel1.Controls.Add(this.label_date);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(16, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(257, 137);
            this.panel1.TabIndex = 7;
            // 
            // label_date
            // 
            this.label_date.AutoSize = true;
            this.label_date.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_date.Location = new System.Drawing.Point(75, 13);
            this.label_date.Name = "label_date";
            this.label_date.Size = new System.Drawing.Size(31, 15);
            this.label_date.TabIndex = 5;
            this.label_date.Text = "　　　";
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_name.Location = new System.Drawing.Point(75, 33);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(31, 15);
            this.label_name.TabIndex = 6;
            this.label_name.Text = "　　　";
            // 
            // label_senka
            // 
            this.label_senka.AutoSize = true;
            this.label_senka.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_senka.Location = new System.Drawing.Point(50, 78);
            this.label_senka.Name = "label_senka";
            this.label_senka.Size = new System.Drawing.Size(31, 15);
            this.label_senka.TabIndex = 7;
            this.label_senka.Text = "　　　";
            // 
            // label_diff
            // 
            this.label_diff.AutoSize = true;
            this.label_diff.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_diff.Location = new System.Drawing.Point(195, 78);
            this.label_diff.Name = "label_diff";
            this.label_diff.Size = new System.Drawing.Size(31, 15);
            this.label_diff.TabIndex = 8;
            this.label_diff.Text = "　　　";
            // 
            // label_addedeo
            // 
            this.label_addedeo.AutoSize = true;
            this.label_addedeo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_addedeo.Location = new System.Drawing.Point(125, 98);
            this.label_addedeo.Name = "label_addedeo";
            this.label_addedeo.Size = new System.Drawing.Size(31, 15);
            this.label_addedeo.TabIndex = 9;
            this.label_addedeo.Text = "　　　";
            // 
            // checkBox_15
            // 
            this.checkBox_15.AutoSize = true;
            this.checkBox_15.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox_15.Location = new System.Drawing.Point(10, 195);
            this.checkBox_15.Name = "checkBox_15";
            this.checkBox_15.Size = new System.Drawing.Size(45, 19);
            this.checkBox_15.TabIndex = 8;
            this.checkBox_15.Tag = "75";
            this.checkBox_15.Text = "1-5";
            this.toolTip1.SetToolTip(this.checkBox_15, "75");
            this.checkBox_15.UseVisualStyleBackColor = true;
            // 
            // checkBox_16
            // 
            this.checkBox_16.AutoSize = true;
            this.checkBox_16.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox_16.Location = new System.Drawing.Point(70, 195);
            this.checkBox_16.Name = "checkBox_16";
            this.checkBox_16.Size = new System.Drawing.Size(45, 19);
            this.checkBox_16.TabIndex = 9;
            this.checkBox_16.Tag = "75";
            this.checkBox_16.Text = "1-6";
            this.toolTip1.SetToolTip(this.checkBox_16, "75");
            this.checkBox_16.UseVisualStyleBackColor = true;
            // 
            // checkBox_25
            // 
            this.checkBox_25.AutoSize = true;
            this.checkBox_25.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox_25.Location = new System.Drawing.Point(130, 195);
            this.checkBox_25.Name = "checkBox_25";
            this.checkBox_25.Size = new System.Drawing.Size(45, 19);
            this.checkBox_25.TabIndex = 10;
            this.checkBox_25.Tag = "100";
            this.checkBox_25.Text = "2-5";
            this.toolTip1.SetToolTip(this.checkBox_25, "100");
            this.checkBox_25.UseVisualStyleBackColor = true;
            // 
            // checkBox_35
            // 
            this.checkBox_35.AutoSize = true;
            this.checkBox_35.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox_35.Location = new System.Drawing.Point(190, 195);
            this.checkBox_35.Name = "checkBox_35";
            this.checkBox_35.Size = new System.Drawing.Size(45, 19);
            this.checkBox_35.TabIndex = 11;
            this.checkBox_35.Tag = "150";
            this.checkBox_35.Text = "3-5";
            this.toolTip1.SetToolTip(this.checkBox_35, "150");
            this.checkBox_35.UseVisualStyleBackColor = true;
            // 
            // checkBox_45
            // 
            this.checkBox_45.AutoSize = true;
            this.checkBox_45.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox_45.Location = new System.Drawing.Point(10, 220);
            this.checkBox_45.Name = "checkBox_45";
            this.checkBox_45.Size = new System.Drawing.Size(45, 19);
            this.checkBox_45.TabIndex = 12;
            this.checkBox_45.Tag = "180";
            this.checkBox_45.Text = "4-5";
            this.toolTip1.SetToolTip(this.checkBox_45, "180");
            this.checkBox_45.UseVisualStyleBackColor = true;
            // 
            // checkBox_55
            // 
            this.checkBox_55.AutoSize = true;
            this.checkBox_55.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox_55.Location = new System.Drawing.Point(130, 220);
            this.checkBox_55.Name = "checkBox_55";
            this.checkBox_55.Size = new System.Drawing.Size(45, 19);
            this.checkBox_55.TabIndex = 13;
            this.checkBox_55.Tag = "200";
            this.checkBox_55.Text = "5-5";
            this.toolTip1.SetToolTip(this.checkBox_55, "200");
            this.checkBox_55.UseVisualStyleBackColor = true;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(35, 260);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(100, 23);
            this.button_ok.TabIndex = 14;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(165, 260);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 23);
            this.button_cancel.TabIndex = 15;
            this.button_cancel.Text = "キャンセル";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // RankingViewer_SelfEo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 303);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.checkBox_55);
            this.Controls.Add(this.checkBox_45);
            this.Controls.Add(this.checkBox_35);
            this.Controls.Add(this.checkBox_25);
            this.Controls.Add(this.checkBox_16);
            this.Controls.Add(this.checkBox_15);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDown_eo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RankingViewer_SelfEo";
            this.Text = "EO補正値";
            this.Load += new System.EventHandler(this.RankingViewer_SelfEo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_eo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown_eo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_date;
        private System.Windows.Forms.Label label_addedeo;
        private System.Windows.Forms.Label label_diff;
        private System.Windows.Forms.Label label_senka;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.CheckBox checkBox_15;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_16;
        private System.Windows.Forms.CheckBox checkBox_25;
        private System.Windows.Forms.CheckBox checkBox_35;
        private System.Windows.Forms.CheckBox checkBox_45;
        private System.Windows.Forms.CheckBox checkBox_55;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
    }
}