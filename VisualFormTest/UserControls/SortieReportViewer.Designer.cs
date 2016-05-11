namespace VisualFormTest.UserControls
{
    partial class SortieReportViewer
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_color = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_foreColor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_backColor = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.button_term_all = new System.Windows.Forms.Button();
            this.button_term_year = new System.Windows.Forms.Button();
            this.button_term_month = new System.Windows.Forms.Button();
            this.button_term_week = new System.Windows.Forms.Button();
            this.button_integrate_orderby = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button_integrate_slotitem = new System.Windows.Forms.Button();
            this.button_integrate_chara = new System.Windows.Forms.Button();
            this.button_integrate_shiptype = new System.Windows.Forms.Button();
            this.splitContainer_select = new System.Windows.Forms.SplitContainer();
            this.splitContainer_treeview = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.treeView_file = new System.Windows.Forms.TreeView();
            this.treeView_map = new System.Windows.Forms.TreeView();
            this.treeView_fleet = new System.Windows.Forms.TreeView();
            this.textBox_selected = new System.Windows.Forms.TextBox();
            this.textBox_report = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_select)).BeginInit();
            this.splitContainer_select.Panel1.SuspendLayout();
            this.splitContainer_select.Panel2.SuspendLayout();
            this.splitContainer_select.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_treeview)).BeginInit();
            this.splitContainer_treeview.Panel1.SuspendLayout();
            this.splitContainer_treeview.Panel2.SuspendLayout();
            this.splitContainer_treeview.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_refresh,
            this.toolStripMenuItem_color});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem_refresh
            // 
            this.toolStripMenuItem_refresh.Name = "toolStripMenuItem_refresh";
            this.toolStripMenuItem_refresh.Size = new System.Drawing.Size(62, 22);
            this.toolStripMenuItem_refresh.Text = "更新(&R)";
            this.toolStripMenuItem_refresh.Click += new System.EventHandler(this.toolStripMenuItem_refresh_Click);
            // 
            // toolStripMenuItem_color
            // 
            this.toolStripMenuItem_color.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_foreColor,
            this.toolStripMenuItem_backColor});
            this.toolStripMenuItem_color.Name = "toolStripMenuItem_color";
            this.toolStripMenuItem_color.Size = new System.Drawing.Size(182, 22);
            this.toolStripMenuItem_color.Text = "テキストボックスの表示色(&C)";
            // 
            // toolStripMenuItem_foreColor
            // 
            this.toolStripMenuItem_foreColor.Name = "toolStripMenuItem_foreColor";
            this.toolStripMenuItem_foreColor.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItem_foreColor.Text = "文字色";
            // 
            // toolStripMenuItem_backColor
            // 
            this.toolStripMenuItem_backColor.Name = "toolStripMenuItem_backColor";
            this.toolStripMenuItem_backColor.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItem_backColor.Text = "背景色";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.IsSplitterFixed = true;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 26);
            this.splitContainer_main.Name = "splitContainer_main";
            this.splitContainer_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.button_term_all);
            this.splitContainer_main.Panel1.Controls.Add(this.button_term_year);
            this.splitContainer_main.Panel1.Controls.Add(this.button_term_month);
            this.splitContainer_main.Panel1.Controls.Add(this.button_term_week);
            this.splitContainer_main.Panel1.Controls.Add(this.button_integrate_orderby);
            this.splitContainer_main.Panel1.Controls.Add(this.label4);
            this.splitContainer_main.Panel1.Controls.Add(this.button_integrate_slotitem);
            this.splitContainer_main.Panel1.Controls.Add(this.button_integrate_chara);
            this.splitContainer_main.Panel1.Controls.Add(this.button_integrate_shiptype);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.splitContainer_select);
            this.splitContainer_main.Size = new System.Drawing.Size(800, 504);
            this.splitContainer_main.SplitterDistance = 52;
            this.splitContainer_main.TabIndex = 1;
            // 
            // button_term_all
            // 
            this.button_term_all.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_term_all.Location = new System.Drawing.Point(610, 24);
            this.button_term_all.Name = "button_term_all";
            this.button_term_all.Size = new System.Drawing.Size(120, 23);
            this.button_term_all.TabIndex = 8;
            this.button_term_all.Text = "全ての期間を表示";
            this.button_term_all.UseVisualStyleBackColor = true;
            this.button_term_all.Click += new System.EventHandler(this.button_term_Click);
            // 
            // button_term_year
            // 
            this.button_term_year.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_term_year.Location = new System.Drawing.Point(470, 24);
            this.button_term_year.Name = "button_term_year";
            this.button_term_year.Size = new System.Drawing.Size(120, 23);
            this.button_term_year.TabIndex = 7;
            this.button_term_year.Text = "年単位でまとめる";
            this.button_term_year.UseVisualStyleBackColor = true;
            this.button_term_year.Click += new System.EventHandler(this.button_term_Click);
            // 
            // button_term_month
            // 
            this.button_term_month.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_term_month.Location = new System.Drawing.Point(330, 24);
            this.button_term_month.Name = "button_term_month";
            this.button_term_month.Size = new System.Drawing.Size(120, 23);
            this.button_term_month.TabIndex = 6;
            this.button_term_month.Text = "月単位でまとめる";
            this.button_term_month.UseVisualStyleBackColor = true;
            this.button_term_month.Click += new System.EventHandler(this.button_term_Click);
            // 
            // button_term_week
            // 
            this.button_term_week.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_term_week.Location = new System.Drawing.Point(190, 24);
            this.button_term_week.Name = "button_term_week";
            this.button_term_week.Size = new System.Drawing.Size(120, 23);
            this.button_term_week.TabIndex = 5;
            this.button_term_week.Text = "週単位でまとめる";
            this.button_term_week.UseVisualStyleBackColor = true;
            this.button_term_week.Click += new System.EventHandler(this.button_term_Click);
            // 
            // button_integrate_orderby
            // 
            this.button_integrate_orderby.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_integrate_orderby.Location = new System.Drawing.Point(560, 0);
            this.button_integrate_orderby.Name = "button_integrate_orderby";
            this.button_integrate_orderby.Size = new System.Drawing.Size(140, 23);
            this.button_integrate_orderby.TabIndex = 4;
            this.button_integrate_orderby.Text = "並び順を区別しない";
            this.button_integrate_orderby.UseVisualStyleBackColor = true;
            this.button_integrate_orderby.Click += new System.EventHandler(this.button_integrate_orderby_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "出撃ログのまとめ方";
            // 
            // button_integrate_slotitem
            // 
            this.button_integrate_slotitem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_integrate_slotitem.Location = new System.Drawing.Point(400, 0);
            this.button_integrate_slotitem.Name = "button_integrate_slotitem";
            this.button_integrate_slotitem.Size = new System.Drawing.Size(120, 23);
            this.button_integrate_slotitem.TabIndex = 2;
            this.button_integrate_slotitem.Text = "装備を区別する";
            this.button_integrate_slotitem.UseVisualStyleBackColor = true;
            this.button_integrate_slotitem.Click += new System.EventHandler(this.button_integrate_slotitem_Click);
            // 
            // button_integrate_chara
            // 
            this.button_integrate_chara.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_integrate_chara.Location = new System.Drawing.Point(260, 0);
            this.button_integrate_chara.Name = "button_integrate_chara";
            this.button_integrate_chara.Size = new System.Drawing.Size(120, 23);
            this.button_integrate_chara.TabIndex = 1;
            this.button_integrate_chara.Text = "キャラでまとめる";
            this.button_integrate_chara.UseVisualStyleBackColor = true;
            this.button_integrate_chara.Click += new System.EventHandler(this.button_integrate_chara_Click);
            // 
            // button_integrate_shiptype
            // 
            this.button_integrate_shiptype.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_integrate_shiptype.Location = new System.Drawing.Point(120, 0);
            this.button_integrate_shiptype.Name = "button_integrate_shiptype";
            this.button_integrate_shiptype.Size = new System.Drawing.Size(120, 23);
            this.button_integrate_shiptype.TabIndex = 0;
            this.button_integrate_shiptype.Text = "艦種でまとめる";
            this.button_integrate_shiptype.UseVisualStyleBackColor = true;
            this.button_integrate_shiptype.Click += new System.EventHandler(this.button_integrate_shiptype_Click);
            // 
            // splitContainer_select
            // 
            this.splitContainer_select.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer_select.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_select.IsSplitterFixed = true;
            this.splitContainer_select.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_select.Name = "splitContainer_select";
            // 
            // splitContainer_select.Panel1
            // 
            this.splitContainer_select.Panel1.Controls.Add(this.splitContainer_treeview);
            // 
            // splitContainer_select.Panel2
            // 
            this.splitContainer_select.Panel2.Controls.Add(this.textBox_report);
            this.splitContainer_select.Size = new System.Drawing.Size(800, 448);
            this.splitContainer_select.SplitterDistance = 400;
            this.splitContainer_select.TabIndex = 7;
            // 
            // splitContainer_treeview
            // 
            this.splitContainer_treeview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer_treeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_treeview.IsSplitterFixed = true;
            this.splitContainer_treeview.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_treeview.Name = "splitContainer_treeview";
            this.splitContainer_treeview.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_treeview.Panel1
            // 
            this.splitContainer_treeview.Panel1.Controls.Add(this.label1);
            this.splitContainer_treeview.Panel1.Controls.Add(this.label3);
            this.splitContainer_treeview.Panel1.Controls.Add(this.label2);
            this.splitContainer_treeview.Panel1.Controls.Add(this.treeView_file);
            this.splitContainer_treeview.Panel1.Controls.Add(this.treeView_map);
            this.splitContainer_treeview.Panel1.Controls.Add(this.treeView_fleet);
            // 
            // splitContainer_treeview.Panel2
            // 
            this.splitContainer_treeview.Panel2.Controls.Add(this.textBox_selected);
            this.splitContainer_treeview.Size = new System.Drawing.Size(400, 448);
            this.splitContainer_treeview.SplitterDistance = 361;
            this.splitContainer_treeview.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(40, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "期間を選択";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(265, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "編成を選択";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(135, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "マップを選択";
            // 
            // treeView_file
            // 
            this.treeView_file.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_file.Indent = 5;
            this.treeView_file.ItemHeight = 18;
            this.treeView_file.Location = new System.Drawing.Point(1, 20);
            this.treeView_file.Name = "treeView_file";
            this.treeView_file.Size = new System.Drawing.Size(135, 340);
            this.treeView_file.TabIndex = 1;
            this.treeView_file.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_file_AfterSelect);
            // 
            // treeView_map
            // 
            this.treeView_map.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_map.Indent = 5;
            this.treeView_map.ItemHeight = 18;
            this.treeView_map.Location = new System.Drawing.Point(140, 20);
            this.treeView_map.Name = "treeView_map";
            this.treeView_map.Size = new System.Drawing.Size(55, 340);
            this.treeView_map.TabIndex = 2;
            this.treeView_map.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_map_AfterSelect);
            // 
            // treeView_fleet
            // 
            this.treeView_fleet.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_fleet.Indent = 5;
            this.treeView_fleet.ItemHeight = 18;
            this.treeView_fleet.Location = new System.Drawing.Point(200, 20);
            this.treeView_fleet.Name = "treeView_fleet";
            this.treeView_fleet.Size = new System.Drawing.Size(195, 340);
            this.treeView_fleet.TabIndex = 5;
            this.treeView_fleet.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_fleet_AfterSelect);
            // 
            // textBox_selected
            // 
            this.textBox_selected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_selected.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_selected.Location = new System.Drawing.Point(0, 0);
            this.textBox_selected.Multiline = true;
            this.textBox_selected.Name = "textBox_selected";
            this.textBox_selected.ReadOnly = true;
            this.textBox_selected.Size = new System.Drawing.Size(398, 81);
            this.textBox_selected.TabIndex = 0;
            // 
            // textBox_report
            // 
            this.textBox_report.BackColor = System.Drawing.Color.Linen;
            this.textBox_report.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_report.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_report.Location = new System.Drawing.Point(0, 0);
            this.textBox_report.Multiline = true;
            this.textBox_report.Name = "textBox_report";
            this.textBox_report.ReadOnly = true;
            this.textBox_report.Size = new System.Drawing.Size(394, 446);
            this.textBox_report.TabIndex = 0;
            // 
            // SortieReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.menuStrip1);
            this.Name = "SortieReportViewer";
            this.Size = new System.Drawing.Size(800, 530);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel1.PerformLayout();
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.splitContainer_select.Panel1.ResumeLayout(false);
            this.splitContainer_select.Panel2.ResumeLayout(false);
            this.splitContainer_select.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_select)).EndInit();
            this.splitContainer_select.ResumeLayout(false);
            this.splitContainer_treeview.Panel1.ResumeLayout(false);
            this.splitContainer_treeview.Panel1.PerformLayout();
            this.splitContainer_treeview.Panel2.ResumeLayout(false);
            this.splitContainer_treeview.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_treeview)).EndInit();
            this.splitContainer_treeview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.SplitContainer splitContainer_select;
        private System.Windows.Forms.SplitContainer splitContainer_treeview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeView_file;
        private System.Windows.Forms.TreeView treeView_map;
        private System.Windows.Forms.TreeView treeView_fleet;
        private System.Windows.Forms.TextBox textBox_selected;
        private System.Windows.Forms.Button button_integrate_orderby;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_integrate_slotitem;
        private System.Windows.Forms.Button button_integrate_chara;
        private System.Windows.Forms.Button button_integrate_shiptype;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_refresh;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_color;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_foreColor;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_backColor;
        private System.Windows.Forms.TextBox textBox_report;
        private System.Windows.Forms.Button button_term_all;
        private System.Windows.Forms.Button button_term_year;
        private System.Windows.Forms.Button button_term_month;
        private System.Windows.Forms.Button button_term_week;
    }
}
