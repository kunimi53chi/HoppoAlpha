namespace VisualFormTest.UserControls
{
    partial class RankingViewer_SubmarinerSetting
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
            this.numericUpDown_eohandicap = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.listView_submariner = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_deleteselected = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_eohandicap)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "【潜水マンハンデ】の設定\r\n\r\n　潜水マンが破壊できないEOの合計値を設定";
            // 
            // numericUpDown_eohandicap
            // 
            this.numericUpDown_eohandicap.Location = new System.Drawing.Point(240, 27);
            this.numericUpDown_eohandicap.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_eohandicap.Name = "numericUpDown_eohandicap";
            this.numericUpDown_eohandicap.Size = new System.Drawing.Size(100, 19);
            this.numericUpDown_eohandicap.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "【潜水マンリスト】";
            // 
            // listView_submariner
            // 
            this.listView_submariner.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView_submariner.FullRowSelect = true;
            this.listView_submariner.Location = new System.Drawing.Point(25, 90);
            this.listView_submariner.Name = "listView_submariner";
            this.listView_submariner.Size = new System.Drawing.Size(230, 180);
            this.listView_submariner.TabIndex = 3;
            this.listView_submariner.UseCompatibleStateImageBehavior = false;
            this.listView_submariner.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "提督ID";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "提督名";
            this.columnHeader2.Width = 120;
            // 
            // button_deleteselected
            // 
            this.button_deleteselected.Location = new System.Drawing.Point(270, 140);
            this.button_deleteselected.Name = "button_deleteselected";
            this.button_deleteselected.Size = new System.Drawing.Size(75, 23);
            this.button_deleteselected.TabIndex = 4;
            this.button_deleteselected.Text = "選択を削除";
            this.button_deleteselected.UseVisualStyleBackColor = true;
            this.button_deleteselected.Click += new System.EventHandler(this.button_deleteselected_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(110, 280);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(100, 23);
            this.button_ok.TabIndex = 5;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(240, 280);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 23);
            this.button_cancel.TabIndex = 6;
            this.button_cancel.Text = "キャンセル";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // RankingViewer_SubmarinerSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 313);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_deleteselected);
            this.Controls.Add(this.listView_submariner);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown_eohandicap);
            this.Controls.Add(this.label1);
            this.Name = "RankingViewer_SubmarinerSetting";
            this.Text = "潜水マン設定";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_eohandicap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_eohandicap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView_submariner;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button_deleteselected;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
    }
}