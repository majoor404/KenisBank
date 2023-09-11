namespace KenisBank
{
    partial class KennisMainForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editPaginaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editModeAanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voegItemToaAanPaginaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1570, 44);
            this.panel1.TabIndex = 0;
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Location = new System.Drawing.Point(12, 91);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1602, 701);
            this.panelMain.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editPaginaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1884, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editPaginaToolStripMenuItem
            // 
            this.editPaginaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editModeAanToolStripMenuItem,
            this.voegItemToaAanPaginaToolStripMenuItem});
            this.editPaginaToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editPaginaToolStripMenuItem.Name = "editPaginaToolStripMenuItem";
            this.editPaginaToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editPaginaToolStripMenuItem.Text = "Edit";
            this.editPaginaToolStripMenuItem.Click += new System.EventHandler(this.editPaginaToolStripMenuItem_Click);
            // 
            // editModeAanToolStripMenuItem
            // 
            this.editModeAanToolStripMenuItem.CheckOnClick = true;
            this.editModeAanToolStripMenuItem.Name = "editModeAanToolStripMenuItem";
            this.editModeAanToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.editModeAanToolStripMenuItem.Text = "Edit Mode Aan";
            this.editModeAanToolStripMenuItem.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // voegItemToaAanPaginaToolStripMenuItem
            // 
            this.voegItemToaAanPaginaToolStripMenuItem.Enabled = false;
            this.voegItemToaAanPaginaToolStripMenuItem.Name = "voegItemToaAanPaginaToolStripMenuItem";
            this.voegItemToaAanPaginaToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.voegItemToaAanPaginaToolStripMenuItem.Text = "Voeg Item toe aan pagina";
            this.voegItemToaAanPaginaToolStripMenuItem.Click += new System.EventHandler(this.buttonVoegToe_Click);
            // 
            // KennisMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1884, 961);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "KennisMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kennis Bank";
            this.Shown += new System.EventHandler(this.KennisMainForm_Shown);
            this.Resize += new System.EventHandler(this.KennisMainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editPaginaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editModeAanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem voegItemToaAanPaginaToolStripMenuItem;
    }
}

