namespace VisualFormTest.UserControls
{
    partial class DropAnalyzer
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_11 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_12 = new System.Windows.Forms.ComboBox();
            this.comboBox_13 = new System.Windows.Forms.ComboBox();
            this.comboBox_14 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_21 = new System.Windows.Forms.ComboBox();
            this.button_search1 = new System.Windows.Forms.Button();
            this.button_search2 = new System.Windows.Forms.Button();
            this.borderline = new System.Windows.Forms.Label();
            this.listView_output = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_refreshheader = new System.Windows.Forms.Button();
            this.textBox_support = new System.Windows.Forms.TextBox();
            this.button_readrecent = new System.Windows.Forms.Button();
            this.checkBox_exceptdropcut = new System.Windows.Forms.CheckBox();
            this.checkBox_mergebycell = new System.Windows.Forms.CheckBox();
            this.comboBox_difficulty = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBox_wins = new System.Windows.Forms.CheckBox();
            this.checkBox_wina = new System.Windows.Forms.CheckBox();
            this.checkBox_winb = new System.Windows.Forms.CheckBox();
            this.checkBox_losec = new System.Windows.Forms.CheckBox();
            this.button_copy = new System.Windows.Forms.Button();
            this.button_extract = new System.Windows.Forms.Button();
            this.label_result = new System.Windows.Forms.Label();
            this.button_timeselect = new System.Windows.Forms.Button();
            this.button_fleetdetail = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_22 = new System.Windows.Forms.ComboBox();
            this.button_search3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "[1] マップで検索";
            // 
            // comboBox_11
            // 
            this.comboBox_11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_11.FormattingEnabled = true;
            this.comboBox_11.Location = new System.Drawing.Point(35, 29);
            this.comboBox_11.Name = "comboBox_11";
            this.comboBox_11.Size = new System.Drawing.Size(50, 20);
            this.comboBox_11.TabIndex = 1;
            this.comboBox_11.SelectedIndexChanged += new System.EventHandler(this.comboBox_11_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "海域";
            // 
            // comboBox_12
            // 
            this.comboBox_12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_12.FormattingEnabled = true;
            this.comboBox_12.Location = new System.Drawing.Point(130, 29);
            this.comboBox_12.Name = "comboBox_12";
            this.comboBox_12.Size = new System.Drawing.Size(125, 20);
            this.comboBox_12.TabIndex = 3;
            this.comboBox_12.SelectedIndexChanged += new System.EventHandler(this.comboBox_12_SelectedIndexChanged);
            // 
            // comboBox_13
            // 
            this.comboBox_13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_13.FormattingEnabled = true;
            this.comboBox_13.Location = new System.Drawing.Point(35, 57);
            this.comboBox_13.Name = "comboBox_13";
            this.comboBox_13.Size = new System.Drawing.Size(125, 20);
            this.comboBox_13.TabIndex = 4;
            this.comboBox_13.SelectedIndexChanged += new System.EventHandler(this.comboBox_13_SelectedIndexChanged);
            // 
            // comboBox_14
            // 
            this.comboBox_14.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_14.FormattingEnabled = true;
            this.comboBox_14.Location = new System.Drawing.Point(195, 57);
            this.comboBox_14.Name = "comboBox_14";
            this.comboBox_14.Size = new System.Drawing.Size(110, 20);
            this.comboBox_14.TabIndex = 5;
            this.comboBox_14.SelectedIndexChanged += new System.EventHandler(this.comboBox_14_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(95, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "マップ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "セル";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(165, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "LID";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "[2] 艦名で検索";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "艦名";
            // 
            // comboBox_21
            // 
            this.comboBox_21.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_21.FormattingEnabled = true;
            this.comboBox_21.Location = new System.Drawing.Point(50, 130);
            this.comboBox_21.Name = "comboBox_21";
            this.comboBox_21.Size = new System.Drawing.Size(200, 20);
            this.comboBox_21.TabIndex = 11;
            this.comboBox_21.SelectedIndexChanged += new System.EventHandler(this.comboBox_21_SelectedIndexChanged);
            // 
            // button_search1
            // 
            this.button_search1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_search1.Location = new System.Drawing.Point(295, 90);
            this.button_search1.Name = "button_search1";
            this.button_search1.Size = new System.Drawing.Size(85, 23);
            this.button_search1.TabIndex = 12;
            this.button_search1.Text = "マップで検索";
            this.button_search1.UseVisualStyleBackColor = true;
            this.button_search1.Click += new System.EventHandler(this.button_search1_Click);
            // 
            // button_search2
            // 
            this.button_search2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_search2.Location = new System.Drawing.Point(295, 125);
            this.button_search2.Name = "button_search2";
            this.button_search2.Size = new System.Drawing.Size(85, 23);
            this.button_search2.TabIndex = 13;
            this.button_search2.Text = "艦名で検索";
            this.button_search2.UseVisualStyleBackColor = true;
            this.button_search2.Click += new System.EventHandler(this.button_search2_Click);
            // 
            // borderline
            // 
            this.borderline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.borderline.Location = new System.Drawing.Point(5, 185);
            this.borderline.Name = "borderline";
            this.borderline.Size = new System.Drawing.Size(555, 1);
            this.borderline.TabIndex = 14;
            // 
            // listView_output
            // 
            this.listView_output.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView_output.FullRowSelect = true;
            this.listView_output.Location = new System.Drawing.Point(10, 195);
            this.listView_output.Name = "listView_output";
            this.listView_output.Size = new System.Drawing.Size(545, 224);
            this.listView_output.TabIndex = 15;
            this.listView_output.UseCompatibleStateImageBehavior = false;
            this.listView_output.View = System.Windows.Forms.View.Details;
            this.listView_output.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_output_ColumnClick);
            this.listView_output.SelectedIndexChanged += new System.EventHandler(this.listView_output_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "艦名";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "件数";
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "S";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "A";
            this.columnHeader4.Width = 50;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "B";
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "なし";
            this.columnHeader6.Width = 50;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "％";
            // 
            // button_refreshheader
            // 
            this.button_refreshheader.Location = new System.Drawing.Point(9, 425);
            this.button_refreshheader.Name = "button_refreshheader";
            this.button_refreshheader.Size = new System.Drawing.Size(120, 23);
            this.button_refreshheader.TabIndex = 16;
            this.button_refreshheader.Text = "検索条件のリロード";
            this.button_refreshheader.UseVisualStyleBackColor = true;
            this.button_refreshheader.Click += new System.EventHandler(this.button_refreshheader_Click);
            // 
            // textBox_support
            // 
            this.textBox_support.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_support.Location = new System.Drawing.Point(406, 2);
            this.textBox_support.Multiline = true;
            this.textBox_support.Name = "textBox_support";
            this.textBox_support.ReadOnly = true;
            this.textBox_support.Size = new System.Drawing.Size(155, 130);
            this.textBox_support.TabIndex = 17;
            // 
            // button_readrecent
            // 
            this.button_readrecent.Location = new System.Drawing.Point(425, 425);
            this.button_readrecent.Name = "button_readrecent";
            this.button_readrecent.Size = new System.Drawing.Size(112, 23);
            this.button_readrecent.TabIndex = 18;
            this.button_readrecent.Text = "直近100件を表示";
            this.button_readrecent.UseVisualStyleBackColor = true;
            this.button_readrecent.Click += new System.EventHandler(this.button_readrecent_Click);
            // 
            // checkBox_exceptdropcut
            // 
            this.checkBox_exceptdropcut.AutoSize = true;
            this.checkBox_exceptdropcut.Location = new System.Drawing.Point(120, 10);
            this.checkBox_exceptdropcut.Name = "checkBox_exceptdropcut";
            this.checkBox_exceptdropcut.Size = new System.Drawing.Size(159, 16);
            this.checkBox_exceptdropcut.TabIndex = 19;
            this.checkBox_exceptdropcut.Text = "ドロップ封じしたケースの除外";
            this.checkBox_exceptdropcut.UseVisualStyleBackColor = true;
            // 
            // checkBox_mergebycell
            // 
            this.checkBox_mergebycell.AutoSize = true;
            this.checkBox_mergebycell.Location = new System.Drawing.Point(105, 106);
            this.checkBox_mergebycell.Name = "checkBox_mergebycell";
            this.checkBox_mergebycell.Size = new System.Drawing.Size(127, 16);
            this.checkBox_mergebycell.TabIndex = 20;
            this.checkBox_mergebycell.Text = "編成IDをセルでマージ";
            this.checkBox_mergebycell.UseVisualStyleBackColor = true;
            // 
            // comboBox_difficulty
            // 
            this.comboBox_difficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_difficulty.FormattingEnabled = true;
            this.comboBox_difficulty.Location = new System.Drawing.Point(316, 29);
            this.comboBox_difficulty.Name = "comboBox_difficulty";
            this.comboBox_difficulty.Size = new System.Drawing.Size(79, 20);
            this.comboBox_difficulty.TabIndex = 21;
            this.comboBox_difficulty.SelectedIndexChanged += new System.EventHandler(this.comboBox_difficulty_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(313, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "難易度";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "勝利";
            // 
            // checkBox_wins
            // 
            this.checkBox_wins.AutoSize = true;
            this.checkBox_wins.Checked = true;
            this.checkBox_wins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_wins.Location = new System.Drawing.Point(35, 85);
            this.checkBox_wins.Name = "checkBox_wins";
            this.checkBox_wins.Size = new System.Drawing.Size(31, 16);
            this.checkBox_wins.TabIndex = 24;
            this.checkBox_wins.Text = "S";
            this.checkBox_wins.UseVisualStyleBackColor = true;
            // 
            // checkBox_wina
            // 
            this.checkBox_wina.AutoSize = true;
            this.checkBox_wina.Checked = true;
            this.checkBox_wina.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_wina.Location = new System.Drawing.Point(95, 85);
            this.checkBox_wina.Name = "checkBox_wina";
            this.checkBox_wina.Size = new System.Drawing.Size(32, 16);
            this.checkBox_wina.TabIndex = 25;
            this.checkBox_wina.Text = "A";
            this.checkBox_wina.UseVisualStyleBackColor = true;
            // 
            // checkBox_winb
            // 
            this.checkBox_winb.AutoSize = true;
            this.checkBox_winb.Checked = true;
            this.checkBox_winb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_winb.Location = new System.Drawing.Point(155, 85);
            this.checkBox_winb.Name = "checkBox_winb";
            this.checkBox_winb.Size = new System.Drawing.Size(32, 16);
            this.checkBox_winb.TabIndex = 26;
            this.checkBox_winb.Text = "B";
            this.checkBox_winb.UseVisualStyleBackColor = true;
            // 
            // checkBox_losec
            // 
            this.checkBox_losec.AutoSize = true;
            this.checkBox_losec.Checked = true;
            this.checkBox_losec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_losec.Location = new System.Drawing.Point(215, 85);
            this.checkBox_losec.Name = "checkBox_losec";
            this.checkBox_losec.Size = new System.Drawing.Size(56, 16);
            this.checkBox_losec.TabIndex = 27;
            this.checkBox_losec.Text = "C以下";
            this.checkBox_losec.UseVisualStyleBackColor = true;
            // 
            // button_copy
            // 
            this.button_copy.Location = new System.Drawing.Point(145, 425);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(120, 23);
            this.button_copy.TabIndex = 28;
            this.button_copy.Text = "結果をコピー\r\n";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // button_extract
            // 
            this.button_extract.Location = new System.Drawing.Point(285, 425);
            this.button_extract.Name = "button_extract";
            this.button_extract.Size = new System.Drawing.Size(120, 23);
            this.button_extract.TabIndex = 29;
            this.button_extract.Text = "結果を展開してコピー\r\n";
            this.button_extract.UseVisualStyleBackColor = true;
            this.button_extract.Click += new System.EventHandler(this.button_extract_Click);
            // 
            // label_result
            // 
            this.label_result.AutoSize = true;
            this.label_result.Location = new System.Drawing.Point(405, 165);
            this.label_result.Name = "label_result";
            this.label_result.Size = new System.Drawing.Size(96, 12);
            this.label_result.TabIndex = 30;
            this.label_result.Text = "検索されていません";
            // 
            // button_timeselect
            // 
            this.button_timeselect.Location = new System.Drawing.Point(405, 133);
            this.button_timeselect.Name = "button_timeselect";
            this.button_timeselect.Size = new System.Drawing.Size(155, 23);
            this.button_timeselect.TabIndex = 31;
            this.button_timeselect.Text = "期間指定 [なし]";
            this.button_timeselect.UseVisualStyleBackColor = true;
            this.button_timeselect.Click += new System.EventHandler(this.button_timeselect_Click);
            // 
            // button_fleetdetail
            // 
            this.button_fleetdetail.Location = new System.Drawing.Point(320, 55);
            this.button_fleetdetail.Name = "button_fleetdetail";
            this.button_fleetdetail.Size = new System.Drawing.Size(70, 23);
            this.button_fleetdetail.TabIndex = 32;
            this.button_fleetdetail.Text = "編成詳細";
            this.button_fleetdetail.UseVisualStyleBackColor = true;
            this.button_fleetdetail.Click += new System.EventHandler(this.button_fleetdetail_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 12);
            this.label10.TabIndex = 33;
            this.label10.Text = "アイテム";
            // 
            // comboBox_22
            // 
            this.comboBox_22.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_22.FormattingEnabled = true;
            this.comboBox_22.Location = new System.Drawing.Point(50, 155);
            this.comboBox_22.Name = "comboBox_22";
            this.comboBox_22.Size = new System.Drawing.Size(200, 20);
            this.comboBox_22.TabIndex = 34;
            this.comboBox_22.SelectedIndexChanged += new System.EventHandler(this.comboBox_22_SelectedIndexChanged);
            // 
            // button_search3
            // 
            this.button_search3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_search3.Location = new System.Drawing.Point(295, 155);
            this.button_search3.Name = "button_search3";
            this.button_search3.Size = new System.Drawing.Size(95, 23);
            this.button_search3.TabIndex = 35;
            this.button_search3.Text = "アイテムで検索";
            this.button_search3.UseVisualStyleBackColor = true;
            this.button_search3.Click += new System.EventHandler(this.button_search3_Click);
            // 
            // DropAnalyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_search3);
            this.Controls.Add(this.comboBox_22);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button_fleetdetail);
            this.Controls.Add(this.button_timeselect);
            this.Controls.Add(this.label_result);
            this.Controls.Add(this.button_extract);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.checkBox_losec);
            this.Controls.Add(this.checkBox_winb);
            this.Controls.Add(this.checkBox_wina);
            this.Controls.Add(this.checkBox_wins);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox_difficulty);
            this.Controls.Add(this.checkBox_mergebycell);
            this.Controls.Add(this.checkBox_exceptdropcut);
            this.Controls.Add(this.button_readrecent);
            this.Controls.Add(this.textBox_support);
            this.Controls.Add(this.button_refreshheader);
            this.Controls.Add(this.listView_output);
            this.Controls.Add(this.borderline);
            this.Controls.Add(this.button_search2);
            this.Controls.Add(this.button_search1);
            this.Controls.Add(this.comboBox_21);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_14);
            this.Controls.Add(this.comboBox_13);
            this.Controls.Add(this.comboBox_12);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_11);
            this.Controls.Add(this.label1);
            this.Name = "DropAnalyzer";
            this.Size = new System.Drawing.Size(570, 455);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_11;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_12;
        private System.Windows.Forms.ComboBox comboBox_13;
        private System.Windows.Forms.ComboBox comboBox_14;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_21;
        private System.Windows.Forms.Button button_search1;
        private System.Windows.Forms.Button button_search2;
        private System.Windows.Forms.Label borderline;
        private System.Windows.Forms.ListView listView_output;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button button_refreshheader;
        private System.Windows.Forms.TextBox textBox_support;
        private System.Windows.Forms.Button button_readrecent;
        private System.Windows.Forms.CheckBox checkBox_exceptdropcut;
        private System.Windows.Forms.CheckBox checkBox_mergebycell;
        private System.Windows.Forms.ComboBox comboBox_difficulty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBox_wins;
        private System.Windows.Forms.CheckBox checkBox_wina;
        private System.Windows.Forms.CheckBox checkBox_winb;
        private System.Windows.Forms.CheckBox checkBox_losec;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.Button button_extract;
        private System.Windows.Forms.Label label_result;
        private System.Windows.Forms.Button button_timeselect;
        private System.Windows.Forms.Button button_fleetdetail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox_22;
        private System.Windows.Forms.Button button_search3;
    }
}
