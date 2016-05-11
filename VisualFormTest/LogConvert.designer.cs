namespace VisualFormTest
{
    partial class LogConvert
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
            this.textBox_folder = new System.Windows.Forms.TextBox();
            this.button_folderef = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listView_files = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_selectedclear = new System.Windows.Forms.Button();
            this.button_appendall = new System.Windows.Forms.Button();
            this.button_allclear = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label_prog = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBox_fullmode = new System.Windows.Forms.CheckBox();
            this.button_openfolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ログを出力するフォルダ";
            // 
            // textBox_folder
            // 
            this.textBox_folder.Location = new System.Drawing.Point(25, 35);
            this.textBox_folder.Name = "textBox_folder";
            this.textBox_folder.ReadOnly = true;
            this.textBox_folder.Size = new System.Drawing.Size(300, 19);
            this.textBox_folder.TabIndex = 1;
            // 
            // button_folderef
            // 
            this.button_folderef.Location = new System.Drawing.Point(330, 33);
            this.button_folderef.Name = "button_folderef";
            this.button_folderef.Size = new System.Drawing.Size(60, 23);
            this.button_folderef.TabIndex = 2;
            this.button_folderef.Text = "参照";
            this.button_folderef.UseVisualStyleBackColor = true;
            this.button_folderef.Click += new System.EventHandler(this.button_folderef_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "コンバートするログファイル一覧";
            // 
            // listView_files
            // 
            this.listView_files.AllowDrop = true;
            this.listView_files.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_files.FullRowSelect = true;
            this.listView_files.Location = new System.Drawing.Point(10, 105);
            this.listView_files.Name = "listView_files";
            this.listView_files.Size = new System.Drawing.Size(400, 200);
            this.listView_files.TabIndex = 4;
            this.listView_files.UseCompatibleStateImageBehavior = false;
            this.listView_files.View = System.Windows.Forms.View.Details;
            this.listView_files.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_files_DragDrop);
            this.listView_files.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_files_DragEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ファイル名";
            this.columnHeader1.Width = 130;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "タイプ";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "フルパス";
            this.columnHeader3.Width = 170;
            // 
            // button_selectedclear
            // 
            this.button_selectedclear.Location = new System.Drawing.Point(10, 320);
            this.button_selectedclear.Name = "button_selectedclear";
            this.button_selectedclear.Size = new System.Drawing.Size(120, 23);
            this.button_selectedclear.TabIndex = 5;
            this.button_selectedclear.Text = "選択をクリア";
            this.button_selectedclear.UseVisualStyleBackColor = true;
            this.button_selectedclear.Click += new System.EventHandler(this.button_selectedclear_Click);
            // 
            // button_appendall
            // 
            this.button_appendall.Location = new System.Drawing.Point(150, 320);
            this.button_appendall.Name = "button_appendall";
            this.button_appendall.Size = new System.Drawing.Size(120, 23);
            this.button_appendall.TabIndex = 6;
            this.button_appendall.Text = "フォルダから一斉追加";
            this.button_appendall.UseVisualStyleBackColor = true;
            this.button_appendall.Click += new System.EventHandler(this.button_appendall_Click);
            // 
            // button_allclear
            // 
            this.button_allclear.Location = new System.Drawing.Point(290, 320);
            this.button_allclear.Name = "button_allclear";
            this.button_allclear.Size = new System.Drawing.Size(120, 23);
            this.button_allclear.TabIndex = 7;
            this.button_allclear.Text = "全てクリア";
            this.button_allclear.UseVisualStyleBackColor = true;
            this.button_allclear.Click += new System.EventHandler(this.button_allclear_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 400);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "進捗：";
            // 
            // label_prog
            // 
            this.label_prog.AutoSize = true;
            this.label_prog.Location = new System.Drawing.Point(65, 400);
            this.label_prog.Name = "label_prog";
            this.label_prog.Size = new System.Drawing.Size(0, 12);
            this.label_prog.TabIndex = 9;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(150, 430);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(120, 23);
            this.button_start.TabIndex = 10;
            this.button_start.Text = "出力開始";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(290, 430);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(120, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Text = "キャンセル";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "フォルダを指定してください";
            // 
            // checkBox_fullmode
            // 
            this.checkBox_fullmode.AutoSize = true;
            this.checkBox_fullmode.Location = new System.Drawing.Point(25, 360);
            this.checkBox_fullmode.Name = "checkBox_fullmode";
            this.checkBox_fullmode.Size = new System.Drawing.Size(320, 16);
            this.checkBox_fullmode.TabIndex = 12;
            this.checkBox_fullmode.Text = "戦果ファイルを詳細モードで展開する（ファイルが4分割されます）";
            this.checkBox_fullmode.UseVisualStyleBackColor = true;
            // 
            // button_openfolder
            // 
            this.button_openfolder.Location = new System.Drawing.Point(255, 69);
            this.button_openfolder.Name = "button_openfolder";
            this.button_openfolder.Size = new System.Drawing.Size(135, 23);
            this.button_openfolder.TabIndex = 13;
            this.button_openfolder.Text = "出力フォルダを開く";
            this.button_openfolder.UseVisualStyleBackColor = true;
            this.button_openfolder.Click += new System.EventHandler(this.button_openfolder_Click);
            // 
            // LogConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 481);
            this.Controls.Add(this.button_openfolder);
            this.Controls.Add(this.checkBox_fullmode);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.label_prog);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_allclear);
            this.Controls.Add(this.button_appendall);
            this.Controls.Add(this.button_selectedclear);
            this.Controls.Add(this.listView_files);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_folderef);
            this.Controls.Add(this.textBox_folder);
            this.Controls.Add(this.label1);
            this.Name = "LogConvert";
            this.Text = "ログ出力";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogConvert_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_folder;
        private System.Windows.Forms.Button button_folderef;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView_files;
        private System.Windows.Forms.Button button_selectedclear;
        private System.Windows.Forms.Button button_appendall;
        private System.Windows.Forms.Button button_allclear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_prog;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox checkBox_fullmode;
        private System.Windows.Forms.Button button_openfolder;
    }
}