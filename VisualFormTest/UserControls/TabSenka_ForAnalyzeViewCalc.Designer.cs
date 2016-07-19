namespace VisualFormTest.UserControls
{
    partial class TabSenka_ForAnalyzeViewCalc
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label_border = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton_4_mod = new System.Windows.Forms.RadioButton();
            this.radioButton_3_divide = new System.Windows.Forms.RadioButton();
            this.radioButton_2_minus = new System.Windows.Forms.RadioButton();
            this.radioButton_1_plus = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.button_mult1 = new System.Windows.Forms.Button();
            this.button_mult2 = new System.Windows.Forms.Button();
            this.button_mult3 = new System.Windows.Forms.Button();
            this.button_mult4 = new System.Windows.Forms.Button();
            this.button_mult5 = new System.Windows.Forms.Button();
            this.button_mult0 = new System.Windows.Forms.Button();
            this.button_mult9 = new System.Windows.Forms.Button();
            this.button_mult8 = new System.Windows.Forms.Button();
            this.button_mult7 = new System.Windows.Forms.Button();
            this.button_mult6 = new System.Windows.Forms.Button();
            this.button_div10 = new System.Windows.Forms.Button();
            this.button_div9 = new System.Windows.Forms.Button();
            this.button_div8 = new System.Windows.Forms.Button();
            this.button_div7 = new System.Windows.Forms.Button();
            this.button_div6 = new System.Windows.Forms.Button();
            this.button_div5 = new System.Windows.Forms.Button();
            this.button_div4 = new System.Windows.Forms.Button();
            this.button_div3 = new System.Windows.Forms.Button();
            this.button_div2 = new System.Windows.Forms.Button();
            this.button_div1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown_div = new System.Windows.Forms.NumericUpDown();
            this.button_divmore = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown_start = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown_end = new System.Windows.Forms.NumericUpDown();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_apply = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_calc_exp = new System.Windows.Forms.TextBox();
            this.textBox_calc_senka = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_div)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_end)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "ほっぽアルファが情報解析用に使用する提督別戦果の表示値を設定します。\r\nランキングのSSから入力しても良いですし、全く自分好みの値も設定可能です。";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 65);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(400, 335);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValidated);
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            // 
            // label_border
            // 
            this.label_border.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_border.Location = new System.Drawing.Point(435, 0);
            this.label_border.Name = "label_border";
            this.label_border.Size = new System.Drawing.Size(1, 480);
            this.label_border.TabIndex = 4;
            this.label_border.Text = "aaa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(450, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "電卓（表示値の一括計算）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(470, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "表示値 = ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(550, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 21);
            this.label4.TabIndex = 7;
            this.label4.Text = " (１）　";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(620, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = " (２）　";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(448, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "(1）";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton_4_mod);
            this.panel1.Controls.Add(this.radioButton_3_divide);
            this.panel1.Controls.Add(this.radioButton_2_minus);
            this.panel1.Controls.Add(this.radioButton_1_plus);
            this.panel1.Location = new System.Drawing.Point(495, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 75);
            this.panel1.TabIndex = 10;
            // 
            // radioButton_4_mod
            // 
            this.radioButton_4_mod.AutoSize = true;
            this.radioButton_4_mod.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton_4_mod.Location = new System.Drawing.Point(120, 30);
            this.radioButton_4_mod.Name = "radioButton_4_mod";
            this.radioButton_4_mod.Size = new System.Drawing.Size(162, 34);
            this.radioButton_4_mod.TabIndex = 3;
            this.radioButton_4_mod.Text = "(測定 % 順位)\r\n（% = 順位で割った余り）";
            this.radioButton_4_mod.UseVisualStyleBackColor = true;
            // 
            // radioButton_3_divide
            // 
            this.radioButton_3_divide.AutoSize = true;
            this.radioButton_3_divide.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton_3_divide.Location = new System.Drawing.Point(5, 38);
            this.radioButton_3_divide.Name = "radioButton_3_divide";
            this.radioButton_3_divide.Size = new System.Drawing.Size(103, 19);
            this.radioButton_3_divide.TabIndex = 2;
            this.radioButton_3_divide.Text = "測定値 ÷ 順位";
            this.radioButton_3_divide.UseVisualStyleBackColor = true;
            // 
            // radioButton_2_minus
            // 
            this.radioButton_2_minus.AutoSize = true;
            this.radioButton_2_minus.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton_2_minus.Location = new System.Drawing.Point(120, 5);
            this.radioButton_2_minus.Name = "radioButton_2_minus";
            this.radioButton_2_minus.Size = new System.Drawing.Size(115, 19);
            this.radioButton_2_minus.TabIndex = 1;
            this.radioButton_2_minus.Text = "(測定値 － 順位)";
            this.radioButton_2_minus.UseVisualStyleBackColor = true;
            // 
            // radioButton_1_plus
            // 
            this.radioButton_1_plus.AutoSize = true;
            this.radioButton_1_plus.Checked = true;
            this.radioButton_1_plus.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton_1_plus.Location = new System.Drawing.Point(5, 5);
            this.radioButton_1_plus.Name = "radioButton_1_plus";
            this.radioButton_1_plus.Size = new System.Drawing.Size(115, 19);
            this.radioButton_1_plus.TabIndex = 0;
            this.radioButton_1_plus.TabStop = true;
            this.radioButton_1_plus.Text = "(測定値 ＋ 順位)";
            this.radioButton_1_plus.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(448, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "(2）";
            // 
            // button_mult1
            // 
            this.button_mult1.Location = new System.Drawing.Point(495, 200);
            this.button_mult1.Name = "button_mult1";
            this.button_mult1.Size = new System.Drawing.Size(40, 20);
            this.button_mult1.TabIndex = 12;
            this.button_mult1.Tag = "1";
            this.button_mult1.Text = "× 1";
            this.button_mult1.UseVisualStyleBackColor = true;
            this.button_mult1.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult2
            // 
            this.button_mult2.Location = new System.Drawing.Point(545, 200);
            this.button_mult2.Name = "button_mult2";
            this.button_mult2.Size = new System.Drawing.Size(40, 20);
            this.button_mult2.TabIndex = 13;
            this.button_mult2.Tag = "2";
            this.button_mult2.Text = "× 2";
            this.button_mult2.UseVisualStyleBackColor = true;
            this.button_mult2.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult3
            // 
            this.button_mult3.Location = new System.Drawing.Point(595, 200);
            this.button_mult3.Name = "button_mult3";
            this.button_mult3.Size = new System.Drawing.Size(40, 20);
            this.button_mult3.TabIndex = 14;
            this.button_mult3.Tag = "3";
            this.button_mult3.Text = "× 3";
            this.button_mult3.UseVisualStyleBackColor = true;
            this.button_mult3.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult4
            // 
            this.button_mult4.Location = new System.Drawing.Point(645, 200);
            this.button_mult4.Name = "button_mult4";
            this.button_mult4.Size = new System.Drawing.Size(40, 20);
            this.button_mult4.TabIndex = 15;
            this.button_mult4.Tag = "4";
            this.button_mult4.Text = "× 4";
            this.button_mult4.UseVisualStyleBackColor = true;
            this.button_mult4.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult5
            // 
            this.button_mult5.Location = new System.Drawing.Point(695, 200);
            this.button_mult5.Name = "button_mult5";
            this.button_mult5.Size = new System.Drawing.Size(40, 20);
            this.button_mult5.TabIndex = 16;
            this.button_mult5.Tag = "5";
            this.button_mult5.Text = "× 5";
            this.button_mult5.UseVisualStyleBackColor = true;
            this.button_mult5.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult0
            // 
            this.button_mult0.Location = new System.Drawing.Point(695, 230);
            this.button_mult0.Name = "button_mult0";
            this.button_mult0.Size = new System.Drawing.Size(40, 20);
            this.button_mult0.TabIndex = 21;
            this.button_mult0.Tag = "0";
            this.button_mult0.Text = "× 0";
            this.button_mult0.UseVisualStyleBackColor = true;
            this.button_mult0.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult9
            // 
            this.button_mult9.Location = new System.Drawing.Point(645, 230);
            this.button_mult9.Name = "button_mult9";
            this.button_mult9.Size = new System.Drawing.Size(40, 20);
            this.button_mult9.TabIndex = 20;
            this.button_mult9.Tag = "9";
            this.button_mult9.Text = "× 9";
            this.button_mult9.UseVisualStyleBackColor = true;
            this.button_mult9.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult8
            // 
            this.button_mult8.Location = new System.Drawing.Point(595, 230);
            this.button_mult8.Name = "button_mult8";
            this.button_mult8.Size = new System.Drawing.Size(40, 20);
            this.button_mult8.TabIndex = 19;
            this.button_mult8.Tag = "8";
            this.button_mult8.Text = "× 8";
            this.button_mult8.UseVisualStyleBackColor = true;
            this.button_mult8.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult7
            // 
            this.button_mult7.Location = new System.Drawing.Point(545, 230);
            this.button_mult7.Name = "button_mult7";
            this.button_mult7.Size = new System.Drawing.Size(40, 20);
            this.button_mult7.TabIndex = 18;
            this.button_mult7.Tag = "7";
            this.button_mult7.Text = "× 7";
            this.button_mult7.UseVisualStyleBackColor = true;
            this.button_mult7.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_mult6
            // 
            this.button_mult6.Location = new System.Drawing.Point(495, 230);
            this.button_mult6.Name = "button_mult6";
            this.button_mult6.Size = new System.Drawing.Size(40, 20);
            this.button_mult6.TabIndex = 17;
            this.button_mult6.Tag = "6";
            this.button_mult6.Text = "× 6";
            this.button_mult6.UseVisualStyleBackColor = true;
            this.button_mult6.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div10
            // 
            this.button_div10.Location = new System.Drawing.Point(695, 300);
            this.button_div10.Name = "button_div10";
            this.button_div10.Size = new System.Drawing.Size(40, 20);
            this.button_div10.TabIndex = 31;
            this.button_div10.Tag = "10";
            this.button_div10.Text = "÷10";
            this.button_div10.UseVisualStyleBackColor = true;
            this.button_div10.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div9
            // 
            this.button_div9.Location = new System.Drawing.Point(645, 300);
            this.button_div9.Name = "button_div9";
            this.button_div9.Size = new System.Drawing.Size(40, 20);
            this.button_div9.TabIndex = 30;
            this.button_div9.Tag = "9";
            this.button_div9.Text = "÷ 9";
            this.button_div9.UseVisualStyleBackColor = true;
            this.button_div9.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div8
            // 
            this.button_div8.Location = new System.Drawing.Point(595, 300);
            this.button_div8.Name = "button_div8";
            this.button_div8.Size = new System.Drawing.Size(40, 20);
            this.button_div8.TabIndex = 29;
            this.button_div8.Tag = "8";
            this.button_div8.Text = "÷ 8";
            this.button_div8.UseVisualStyleBackColor = true;
            this.button_div8.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div7
            // 
            this.button_div7.Location = new System.Drawing.Point(545, 300);
            this.button_div7.Name = "button_div7";
            this.button_div7.Size = new System.Drawing.Size(40, 20);
            this.button_div7.TabIndex = 28;
            this.button_div7.Tag = "7";
            this.button_div7.Text = "÷ 7";
            this.button_div7.UseVisualStyleBackColor = true;
            this.button_div7.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div6
            // 
            this.button_div6.Location = new System.Drawing.Point(495, 300);
            this.button_div6.Name = "button_div6";
            this.button_div6.Size = new System.Drawing.Size(40, 20);
            this.button_div6.TabIndex = 27;
            this.button_div6.Tag = "6";
            this.button_div6.Text = "÷ 6";
            this.button_div6.UseVisualStyleBackColor = true;
            this.button_div6.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div5
            // 
            this.button_div5.Location = new System.Drawing.Point(695, 270);
            this.button_div5.Name = "button_div5";
            this.button_div5.Size = new System.Drawing.Size(40, 20);
            this.button_div5.TabIndex = 26;
            this.button_div5.Tag = "5";
            this.button_div5.Text = "÷ 5";
            this.button_div5.UseVisualStyleBackColor = true;
            this.button_div5.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div4
            // 
            this.button_div4.Location = new System.Drawing.Point(645, 270);
            this.button_div4.Name = "button_div4";
            this.button_div4.Size = new System.Drawing.Size(40, 20);
            this.button_div4.TabIndex = 25;
            this.button_div4.Tag = "4";
            this.button_div4.Text = "÷ 4";
            this.button_div4.UseVisualStyleBackColor = true;
            this.button_div4.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div3
            // 
            this.button_div3.Location = new System.Drawing.Point(595, 270);
            this.button_div3.Name = "button_div3";
            this.button_div3.Size = new System.Drawing.Size(40, 20);
            this.button_div3.TabIndex = 24;
            this.button_div3.Tag = "3";
            this.button_div3.Text = "÷ 3";
            this.button_div3.UseVisualStyleBackColor = true;
            this.button_div3.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div2
            // 
            this.button_div2.Location = new System.Drawing.Point(545, 270);
            this.button_div2.Name = "button_div2";
            this.button_div2.Size = new System.Drawing.Size(40, 20);
            this.button_div2.TabIndex = 23;
            this.button_div2.Tag = "2";
            this.button_div2.Text = "÷ 2";
            this.button_div2.UseVisualStyleBackColor = true;
            this.button_div2.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // button_div1
            // 
            this.button_div1.Location = new System.Drawing.Point(495, 270);
            this.button_div1.Name = "button_div1";
            this.button_div1.Size = new System.Drawing.Size(40, 20);
            this.button_div1.TabIndex = 22;
            this.button_div1.Tag = "1";
            this.button_div1.Text = "÷ 1";
            this.button_div1.UseVisualStyleBackColor = true;
            this.button_div1.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(450, 365);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 15);
            this.label8.TabIndex = 32;
            this.label8.Text = "電卓（戦果計算）";
            // 
            // numericUpDown_div
            // 
            this.numericUpDown_div.Location = new System.Drawing.Point(525, 330);
            this.numericUpDown_div.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_div.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.numericUpDown_div.Name = "numericUpDown_div";
            this.numericUpDown_div.Size = new System.Drawing.Size(80, 19);
            this.numericUpDown_div.TabIndex = 33;
            this.numericUpDown_div.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            // 
            // button_divmore
            // 
            this.button_divmore.Location = new System.Drawing.Point(620, 329);
            this.button_divmore.Name = "button_divmore";
            this.button_divmore.Size = new System.Drawing.Size(120, 20);
            this.button_divmore.TabIndex = 34;
            this.button_divmore.Tag = "99";
            this.button_divmore.Text = "それ以上の値で割る";
            this.button_divmore.UseVisualStyleBackColor = true;
            this.button_divmore.Click += new System.EventHandler(this.ValueSetCommonButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(450, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 15);
            this.label9.TabIndex = 35;
            this.label9.Text = "計算する順位";
            // 
            // numericUpDown_start
            // 
            this.numericUpDown_start.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown_start.Location = new System.Drawing.Point(535, 72);
            this.numericUpDown_start.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_start.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_start.Name = "numericUpDown_start";
            this.numericUpDown_start.Size = new System.Drawing.Size(100, 23);
            this.numericUpDown_start.TabIndex = 36;
            this.numericUpDown_start.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(640, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 15);
            this.label10.TabIndex = 37;
            this.label10.Text = "～";
            // 
            // numericUpDown_end
            // 
            this.numericUpDown_end.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown_end.Location = new System.Drawing.Point(665, 72);
            this.numericUpDown_end.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_end.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_end.Name = "numericUpDown_end";
            this.numericUpDown_end.Size = new System.Drawing.Size(100, 23);
            this.numericUpDown_end.TabIndex = 38;
            this.numericUpDown_end.Value = new decimal(new int[] {
            990,
            0,
            0,
            0});
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(50, 410);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(100, 23);
            this.button_ok.TabIndex = 39;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(170, 410);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 23);
            this.button_cancel.TabIndex = 40;
            this.button_cancel.Text = "キャンセル";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(290, 410);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(100, 23);
            this.button_apply.TabIndex = 41;
            this.button_apply.Text = "適用";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(460, 400);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 15);
            this.label11.TabIndex = 42;
            this.label11.Text = "提督経験値";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(640, 400);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 15);
            this.label12.TabIndex = 43;
            this.label12.Text = "戦果";
            // 
            // textBox_calc_exp
            // 
            this.textBox_calc_exp.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_calc_exp.Location = new System.Drawing.Point(525, 392);
            this.textBox_calc_exp.Name = "textBox_calc_exp";
            this.textBox_calc_exp.Size = new System.Drawing.Size(100, 23);
            this.textBox_calc_exp.TabIndex = 44;
            this.textBox_calc_exp.TextChanged += new System.EventHandler(this.textBox_calc_exp_TextChanged);
            // 
            // textBox_calc_senka
            // 
            this.textBox_calc_senka.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_calc_senka.Location = new System.Drawing.Point(675, 392);
            this.textBox_calc_senka.Name = "textBox_calc_senka";
            this.textBox_calc_senka.Size = new System.Drawing.Size(100, 23);
            this.textBox_calc_senka.TabIndex = 45;
            this.textBox_calc_senka.TextChanged += new System.EventHandler(this.textBox_calc_senka_TextChanged);
            // 
            // TabSenka_ForAnalyzeViewCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(784, 443);
            this.Controls.Add(this.textBox_calc_senka);
            this.Controls.Add(this.textBox_calc_exp);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.numericUpDown_end);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numericUpDown_start);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button_divmore);
            this.Controls.Add(this.numericUpDown_div);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button_div10);
            this.Controls.Add(this.button_div9);
            this.Controls.Add(this.button_div8);
            this.Controls.Add(this.button_div7);
            this.Controls.Add(this.button_div6);
            this.Controls.Add(this.button_div5);
            this.Controls.Add(this.button_div4);
            this.Controls.Add(this.button_div3);
            this.Controls.Add(this.button_div2);
            this.Controls.Add(this.button_div1);
            this.Controls.Add(this.button_mult0);
            this.Controls.Add(this.button_mult9);
            this.Controls.Add(this.button_mult8);
            this.Controls.Add(this.button_mult7);
            this.Controls.Add(this.button_mult6);
            this.Controls.Add(this.button_mult5);
            this.Controls.Add(this.button_mult4);
            this.Controls.Add(this.button_mult3);
            this.Controls.Add(this.button_mult2);
            this.Controls.Add(this.button_mult1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_border);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TabSenka_ForAnalyzeViewCalc";
            this.Text = "戦果解析用電卓";
            this.Load += new System.EventHandler(this.TabSenka_ForAnalyzeViewCalc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_div)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_end)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label_border;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_4_mod;
        private System.Windows.Forms.RadioButton radioButton_3_divide;
        private System.Windows.Forms.RadioButton radioButton_2_minus;
        private System.Windows.Forms.RadioButton radioButton_1_plus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_mult1;
        private System.Windows.Forms.Button button_mult2;
        private System.Windows.Forms.Button button_mult3;
        private System.Windows.Forms.Button button_mult4;
        private System.Windows.Forms.Button button_mult5;
        private System.Windows.Forms.Button button_mult0;
        private System.Windows.Forms.Button button_mult9;
        private System.Windows.Forms.Button button_mult8;
        private System.Windows.Forms.Button button_mult7;
        private System.Windows.Forms.Button button_mult6;
        private System.Windows.Forms.Button button_div10;
        private System.Windows.Forms.Button button_div9;
        private System.Windows.Forms.Button button_div8;
        private System.Windows.Forms.Button button_div7;
        private System.Windows.Forms.Button button_div6;
        private System.Windows.Forms.Button button_div5;
        private System.Windows.Forms.Button button_div4;
        private System.Windows.Forms.Button button_div3;
        private System.Windows.Forms.Button button_div2;
        private System.Windows.Forms.Button button_div1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown_div;
        private System.Windows.Forms.Button button_divmore;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown_start;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDown_end;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_calc_exp;
        private System.Windows.Forms.TextBox textBox_calc_senka;
    }
}