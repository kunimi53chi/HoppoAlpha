namespace VisualFormTest.UserControls
{
    partial class RankingViewer
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_copytoclipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_screenshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_submariner = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_view = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.treeView_files = new System.Windows.Forms.TreeView();
            this.listView_ranking = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_addsubmariner = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_refresh,
            this.toolStripMenuItem_copytoclipboard,
            this.toolStripMenuItem_screenshot,
            this.toolStripMenuItem_submariner,
            this.toolStripMenuItem_view});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1024, 26);
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
            // toolStripMenuItem_copytoclipboard
            // 
            this.toolStripMenuItem_copytoclipboard.Name = "toolStripMenuItem_copytoclipboard";
            this.toolStripMenuItem_copytoclipboard.Size = new System.Drawing.Size(206, 22);
            this.toolStripMenuItem_copytoclipboard.Text = "結果をクリップボードにコピー(&C)";
            this.toolStripMenuItem_copytoclipboard.Click += new System.EventHandler(this.toolStripMenuItem_copytoclipboard_Click);
            // 
            // toolStripMenuItem_screenshot
            // 
            this.toolStripMenuItem_screenshot.Name = "toolStripMenuItem_screenshot";
            this.toolStripMenuItem_screenshot.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem_screenshot.Text = "スクリーンショット(&S)";
            this.toolStripMenuItem_screenshot.Click += new System.EventHandler(this.toolStripMenuItem_screenshot_Click);
            // 
            // toolStripMenuItem_submariner
            // 
            this.toolStripMenuItem_submariner.Name = "toolStripMenuItem_submariner";
            this.toolStripMenuItem_submariner.Size = new System.Drawing.Size(109, 22);
            this.toolStripMenuItem_submariner.Text = "潜水マン設定(&F)";
            this.toolStripMenuItem_submariner.Click += new System.EventHandler(this.toolStripMenuItem_submariner_Click);
            // 
            // toolStripMenuItem_view
            // 
            this.toolStripMenuItem_view.Name = "toolStripMenuItem_view";
            this.toolStripMenuItem_view.Size = new System.Drawing.Size(12, 22);
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 26);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.treeView_files);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.listView_ranking);
            this.splitContainer_main.Size = new System.Drawing.Size(1024, 454);
            this.splitContainer_main.SplitterDistance = 120;
            this.splitContainer_main.TabIndex = 1;
            // 
            // treeView_files
            // 
            this.treeView_files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_files.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_files.Indent = 10;
            this.treeView_files.Location = new System.Drawing.Point(0, 0);
            this.treeView_files.Name = "treeView_files";
            this.treeView_files.Size = new System.Drawing.Size(120, 454);
            this.treeView_files.TabIndex = 0;
            this.treeView_files.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_files_AfterSelect);
            // 
            // listView_ranking
            // 
            this.listView_ranking.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader16,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15});
            this.listView_ranking.ContextMenuStrip = this.contextMenuStrip1;
            this.listView_ranking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_ranking.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listView_ranking.FullRowSelect = true;
            this.listView_ranking.Location = new System.Drawing.Point(0, 0);
            this.listView_ranking.Name = "listView_ranking";
            this.listView_ranking.Size = new System.Drawing.Size(900, 454);
            this.listView_ranking.TabIndex = 0;
            this.listView_ranking.UseCompatibleStateImageBehavior = false;
            this.listView_ranking.View = System.Windows.Forms.View.Details;
            this.listView_ranking.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ranking_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "順";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Lv";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 40;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "提督名";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "提督経験値";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "戦果";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "増加";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "EO増";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Rankコメント";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "甲";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader9.Width = 40;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "EO潜水補正戦果";
            this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader10.Width = 120;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "破壊済EO";
            this.columnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader11.Width = 80;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "月初戦果";
            this.columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader12.Width = 80;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "EO補正戦果";
            this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader13.Width = 100;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "月初データ";
            this.columnHeader14.Width = 80;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "潜";
            this.columnHeader15.Width = 40;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_addsubmariner});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 26);
            // 
            // toolStripMenuItem_addsubmariner
            // 
            this.toolStripMenuItem_addsubmariner.Name = "toolStripMenuItem_addsubmariner";
            this.toolStripMenuItem_addsubmariner.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem_addsubmariner.Text = "潜水マンに追加";
            this.toolStripMenuItem_addsubmariner.Click += new System.EventHandler(this.toolStripMenuItem_addsubmariner_Click);
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "#";
            this.columnHeader16.Width = 40;
            // 
            // RankingViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.menuStrip1);
            this.Name = "RankingViewer";
            this.Size = new System.Drawing.Size(1024, 480);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.TreeView treeView_files;
        private System.Windows.Forms.ListView listView_ranking;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_refresh;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_copytoclipboard;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_view;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_screenshot;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_submariner;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_addsubmariner;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
    }
}
