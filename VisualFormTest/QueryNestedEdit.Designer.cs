namespace VisualFormTest
{
    partial class QueryNestedEdit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryNestedEdit));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_condition = new System.Windows.Forms.Label();
            this.label_and = new System.Windows.Forms.Label();
            this.label_not = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_type = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_queryjson = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.StringMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Value,
            this.IntMode,
            this.StringMode,
            this.Delete});
            this.dataGridView1.Location = new System.Drawing.Point(10, 80);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(445, 177);
            this.dataGridView1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_condition, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_and, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_not, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_type, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(40, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(390, 44);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "検索条件";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "AND";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(180, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "NOT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_condition
            // 
            this.label_condition.AutoSize = true;
            this.label_condition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_condition.Location = new System.Drawing.Point(83, 0);
            this.label_condition.Name = "label_condition";
            this.label_condition.Size = new System.Drawing.Size(94, 22);
            this.label_condition.TabIndex = 3;
            this.label_condition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_and
            // 
            this.label_and.AutoSize = true;
            this.label_and.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_and.Location = new System.Drawing.Point(83, 22);
            this.label_and.Name = "label_and";
            this.label_and.Size = new System.Drawing.Size(94, 22);
            this.label_and.TabIndex = 4;
            this.label_and.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.label_and, "×なら全OR、○なら全ANDで以下の条件を結合します");
            // 
            // label_not
            // 
            this.label_not.AutoSize = true;
            this.label_not.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_not.Location = new System.Drawing.Point(263, 22);
            this.label_not.Name = "label_not";
            this.label_not.Size = new System.Drawing.Size(124, 22);
            this.label_not.TabIndex = 5;
            this.label_not.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.label_not, resources.GetString("label_not.ToolTip"));
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(180, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 22);
            this.label4.TabIndex = 6;
            this.label4.Text = "値のタイプ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_type
            // 
            this.label_type.AutoSize = true;
            this.label_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_type.Location = new System.Drawing.Point(260, 0);
            this.label_type.Margin = new System.Windows.Forms.Padding(0);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(130, 22);
            this.label_type.TabIndex = 7;
            this.label_type.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 280);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "ネストクエリデータ";
            // 
            // textBox_queryjson
            // 
            this.textBox_queryjson.BackColor = System.Drawing.Color.White;
            this.textBox_queryjson.Location = new System.Drawing.Point(42, 295);
            this.textBox_queryjson.Multiline = true;
            this.textBox_queryjson.Name = "textBox_queryjson";
            this.textBox_queryjson.ReadOnly = true;
            this.textBox_queryjson.Size = new System.Drawing.Size(390, 54);
            this.textBox_queryjson.TabIndex = 10;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(220, 360);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 11;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(325, 360);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 12;
            this.button_cancel.Text = "キャンセル";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // Value
            // 
            this.Value.HeaderText = "値";
            this.Value.Name = "Value";
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IntMode
            // 
            dataGridViewCellStyle2.NullValue = "常にNO";
            this.IntMode.DefaultCellStyle = dataGridViewCellStyle2;
            this.IntMode.HeaderText = "数値の検索モード";
            this.IntMode.Items.AddRange(new object[] {
            "常にNO",
            "等しい(＝)",
            "より大きい(＞)",
            "以上(≧)",
            "より小さい(＜)",
            "以下(≦)",
            "等しくない(≠)",
            "常にYES"});
            this.IntMode.Name = "IntMode";
            this.IntMode.Width = 120;
            // 
            // StringMode
            // 
            dataGridViewCellStyle3.NullValue = "常にNO";
            this.StringMode.DefaultCellStyle = dataGridViewCellStyle3;
            this.StringMode.HeaderText = "文字列の検索モード";
            this.StringMode.Items.AddRange(new object[] {
            "常にNO",
            "完全一致",
            "前方一致",
            "後方一致",
            "部分一致",
            "常にYES"});
            this.StringMode.Name = "StringMode";
            this.StringMode.Width = 120;
            // 
            // Delete
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "Del";
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.Delete.DefaultCellStyle = dataGridViewCellStyle4;
            this.Delete.HeaderText = "削除";
            this.Delete.Name = "Delete";
            this.Delete.Width = 50;
            // 
            // QueryNestedEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 397);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_queryjson);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "QueryNestedEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ネストクエリ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueryNestedEdit_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_condition;
        private System.Windows.Forms.Label label_and;
        private System.Windows.Forms.Label label_not;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_type;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_queryjson;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewComboBoxColumn IntMode;
        private System.Windows.Forms.DataGridViewComboBoxColumn StringMode;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;

    }
}