namespace VisualFormTest.DockingWindows
{
    partial class DockWindowShortTabSenka
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
            this.tabSenkaShort1 = new VisualFormTest.UserControls.TabSenkaShort();
            this.SuspendLayout();
            // 
            // tabSenkaShort1
            // 
            this.tabSenkaShort1.BackColor = System.Drawing.Color.White;
            this.tabSenkaShort1.Init2Finished = false;
            this.tabSenkaShort1.InitFinished = false;
            this.tabSenkaShort1.IsShown = false;
            this.tabSenkaShort1.Location = new System.Drawing.Point(0, 0);
            this.tabSenkaShort1.Name = "tabSenkaShort1";
            this.tabSenkaShort1.Size = new System.Drawing.Size(200, 65);
            this.tabSenkaShort1.TabIndex = 0;
            // 
            // DockWindowShortTabSenka
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(203, 68);
            this.Controls.Add(this.tabSenkaShort1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Name = "DockWindowShortTabSenka";
            this.Text = "S戦果";
            this.DockStateChanged += new System.EventHandler(this.DockWindowShortTabSenka_DockStateChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControls.TabSenkaShort tabSenkaShort1;

    }
}