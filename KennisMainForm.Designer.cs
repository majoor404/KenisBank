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
            this.labelEditMode = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editPaginaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editModeAanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenHoofdstukTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenLinkNaarFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenLinkNaarDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenTekstBlokToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelEditMode);
            this.panel1.Location = new System.Drawing.Point(12, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1570, 44);
            this.panel1.TabIndex = 0;
            // 
            // labelEditMode
            // 
            this.labelEditMode.AutoSize = true;
            this.labelEditMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEditMode.Location = new System.Drawing.Point(669, 8);
            this.labelEditMode.Name = "labelEditMode";
            this.labelEditMode.Size = new System.Drawing.Size(134, 25);
            this.labelEditMode.TabIndex = 0;
            this.labelEditMode.Text = "Edit MODE!";
            this.labelEditMode.Visible = false;
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
            this.editPaginaToolStripMenuItem,
            this.refreshToolStripMenuItem});
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
            this.addItemToolStripMenuItem});
            this.editPaginaToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editPaginaToolStripMenuItem.Name = "editPaginaToolStripMenuItem";
            this.editPaginaToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editPaginaToolStripMenuItem.Text = "Edit";
            // 
            // editModeAanToolStripMenuItem
            // 
            this.editModeAanToolStripMenuItem.CheckOnClick = true;
            this.editModeAanToolStripMenuItem.Name = "editModeAanToolStripMenuItem";
            this.editModeAanToolStripMenuItem.Size = new System.Drawing.Size(177, 24);
            this.editModeAanToolStripMenuItem.Text = "Edit Mode Aan";
            this.editModeAanToolStripMenuItem.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // addItemToolStripMenuItem
            // 
            this.addItemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toevoegenHoofdstukTextToolStripMenuItem,
            this.toevoegenLinkNaarFileToolStripMenuItem,
            this.toevoegenLinkNaarDirToolStripMenuItem,
            this.toevoegenTekstBlokToolStripMenuItem});
            this.addItemToolStripMenuItem.Enabled = false;
            this.addItemToolStripMenuItem.Name = "addItemToolStripMenuItem";
            this.addItemToolStripMenuItem.Size = new System.Drawing.Size(177, 24);
            this.addItemToolStripMenuItem.Text = "Add Item";
            // 
            // toevoegenHoofdstukTextToolStripMenuItem
            // 
            this.toevoegenHoofdstukTextToolStripMenuItem.Name = "toevoegenHoofdstukTextToolStripMenuItem";
            this.toevoegenHoofdstukTextToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
            this.toevoegenHoofdstukTextToolStripMenuItem.Text = "Toevoegen Hoofdstuk text";
            this.toevoegenHoofdstukTextToolStripMenuItem.Click += new System.EventHandler(this.toevoegenHoofdstukTextToolStripMenuItem_Click);
            // 
            // toevoegenLinkNaarFileToolStripMenuItem
            // 
            this.toevoegenLinkNaarFileToolStripMenuItem.Name = "toevoegenLinkNaarFileToolStripMenuItem";
            this.toevoegenLinkNaarFileToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
            this.toevoegenLinkNaarFileToolStripMenuItem.Text = "Toevoegen Link Naar File";
            this.toevoegenLinkNaarFileToolStripMenuItem.Click += new System.EventHandler(this.toevoegenLinkNaarFileToolStripMenuItem_Click);
            // 
            // toevoegenLinkNaarDirToolStripMenuItem
            // 
            this.toevoegenLinkNaarDirToolStripMenuItem.Name = "toevoegenLinkNaarDirToolStripMenuItem";
            this.toevoegenLinkNaarDirToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
            this.toevoegenLinkNaarDirToolStripMenuItem.Text = "Toevoegen Link Naar Directory";
            this.toevoegenLinkNaarDirToolStripMenuItem.Click += new System.EventHandler(this.toevoegenLinkNaarDirToolStripMenuItem_Click);
            // 
            // toevoegenTekstBlokToolStripMenuItem
            // 
            this.toevoegenTekstBlokToolStripMenuItem.Name = "toevoegenTekstBlokToolStripMenuItem";
            this.toevoegenTekstBlokToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
            this.toevoegenTekstBlokToolStripMenuItem.Text = "Toevoegen Tekst Blok";
            this.toevoegenTekstBlokToolStripMenuItem.Click += new System.EventHandler(this.toevoegenTekstBlokToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem addItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toevoegenHoofdstukTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toevoegenLinkNaarFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toevoegenLinkNaarDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toevoegenTekstBlokToolStripMenuItem;
        private System.Windows.Forms.Label labelEditMode;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}

