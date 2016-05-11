namespace VisualFormTest.DockingWindows
{
    partial class DockWindowShortTabFleet
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
            this.tabFleetShort1 = new VisualFormTest.UserControls.TabFleetShort();
            this.SuspendLayout();
            // 
            // tabFleetShort1
            // 
            this.tabFleetShort1.BackColor = System.Drawing.Color.White;
            this.tabFleetShort1.FleetIndex = 0;
            this.tabFleetShort1.Init2Finished = false;
            this.tabFleetShort1.InitFinished = false;
            this.tabFleetShort1.IsShown = false;
            this.tabFleetShort1.LabelFleet = null;
            this.tabFleetShort1.Location = new System.Drawing.Point(0, 0);
            this.tabFleetShort1.Margin = new System.Windows.Forms.Padding(0);
            this.tabFleetShort1.Name = "tabFleetShort1";
            this.tabFleetShort1.Size = new System.Drawing.Size(380, 155);
            this.tabFleetShort1.TabIndex = 0;
            // 
            // DockWindowShortTabFleet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(389, 157);
            this.Controls.Add(this.tabFleetShort1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowShortTabFleet";
            this.Text = "S艦隊";
            this.DockStateChanged += new System.EventHandler(this.DockWindowShortTabFleet_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.TabFleetShort tabFleetShort1;

    }
}