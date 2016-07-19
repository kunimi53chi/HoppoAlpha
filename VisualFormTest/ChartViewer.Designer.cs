namespace VisualFormTest
{
    partial class ChartViewer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_s10 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea2.AxisX.LabelStyle.Format = "M/d H:mm";
            chartArea2.AxisY.IsStartedFromZero = false;
            chartArea2.AxisY.LabelStyle.Format = "N0";
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 26);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            this.chart1.Size = new System.Drawing.Size(624, 416);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem14,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem13});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 26);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(44, 22);
            this.toolStripMenuItem1.Text = "項目";
            this.toolStripMenuItem1.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItem1_DropDownItemClicked);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem4.Text = "資源（1～4）";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem5.Text = "資源（5～8）";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem6.Text = "提督経験値";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem7.Text = "戦果ボーダー";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(68, 22);
            this.toolStripMenuItem2.Text = "計算方法";
            this.toolStripMenuItem2.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItem2_DropDownItemClicked);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem8.Text = "絶対表示";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem9.Text = "相対表示";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(44, 22);
            this.toolStripMenuItem3.Text = "期間";
            this.toolStripMenuItem3.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItem3_DropDownItemClicked);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem10.Text = "全期間";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem11.Text = "直近1週間";
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem12.Text = "直近1日間";
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(68, 22);
            this.toolStripMenuItem13.Text = "パレット";
            this.toolStripMenuItem13.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItem13_DropDownItemClicked);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_s1,
            this.toolStripMenuItem_s2,
            this.toolStripMenuItem_s3,
            this.toolStripMenuItem_s4,
            this.toolStripMenuItem_s5,
            this.toolStripMenuItem_s6,
            this.toolStripMenuItem_s7,
            this.toolStripMenuItem_s8,
            this.toolStripMenuItem_s9,
            this.toolStripMenuItem_s10});
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(44, 22);
            this.toolStripMenuItem14.Text = "系列";
            this.toolStripMenuItem14.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItem14_DropDownItemClicked);
            // 
            // toolStripMenuItem_s1
            // 
            this.toolStripMenuItem_s1.Name = "toolStripMenuItem_s1";
            this.toolStripMenuItem_s1.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s1.Text = "系列1";
            // 
            // toolStripMenuItem_s2
            // 
            this.toolStripMenuItem_s2.Name = "toolStripMenuItem_s2";
            this.toolStripMenuItem_s2.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s2.Text = "系列2";
            // 
            // toolStripMenuItem_s3
            // 
            this.toolStripMenuItem_s3.Name = "toolStripMenuItem_s3";
            this.toolStripMenuItem_s3.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s3.Text = "系列3";
            // 
            // toolStripMenuItem_s4
            // 
            this.toolStripMenuItem_s4.Name = "toolStripMenuItem_s4";
            this.toolStripMenuItem_s4.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s4.Text = "系列4";
            // 
            // toolStripMenuItem_s5
            // 
            this.toolStripMenuItem_s5.Name = "toolStripMenuItem_s5";
            this.toolStripMenuItem_s5.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s5.Text = "系列5";
            // 
            // toolStripMenuItem_s6
            // 
            this.toolStripMenuItem_s6.Name = "toolStripMenuItem_s6";
            this.toolStripMenuItem_s6.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s6.Text = "系列6";
            // 
            // toolStripMenuItem_s7
            // 
            this.toolStripMenuItem_s7.Name = "toolStripMenuItem_s7";
            this.toolStripMenuItem_s7.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s7.Text = "系列7";
            // 
            // toolStripMenuItem_s8
            // 
            this.toolStripMenuItem_s8.Name = "toolStripMenuItem_s8";
            this.toolStripMenuItem_s8.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s8.Text = "系列8";
            // 
            // toolStripMenuItem_s9
            // 
            this.toolStripMenuItem_s9.Name = "toolStripMenuItem_s9";
            this.toolStripMenuItem_s9.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s9.Text = "系列9";
            // 
            // toolStripMenuItem_s10
            // 
            this.toolStripMenuItem_s10.Name = "toolStripMenuItem_s10";
            this.toolStripMenuItem_s10.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem_s10.Text = "系列10";
            // 
            // ChartViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ChartViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "グラフビュワー";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_s10;
    }
}