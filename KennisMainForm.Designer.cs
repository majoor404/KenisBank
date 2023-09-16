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
            this.labelPaginaInBeeld = new System.Windows.Forms.Label();
            this.labelEditMode = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.terugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editPaginaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editModeAanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenHoofdstukTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenLinkNaarFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenLinkNaarDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenTekstBlokToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toevoegenLegeRegelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveHuidigePaginaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelUpDown = new System.Windows.Forms.Panel();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelUpDown.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelPaginaInBeeld);
            this.panel1.Controls.Add(this.labelEditMode);
            this.panel1.Location = new System.Drawing.Point(12, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1570, 44);
            this.panel1.TabIndex = 0;
            // 
            // labelPaginaInBeeld
            // 
            this.labelPaginaInBeeld.AutoSize = true;
            this.labelPaginaInBeeld.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPaginaInBeeld.Location = new System.Drawing.Point(15, 13);
            this.labelPaginaInBeeld.Name = "labelPaginaInBeeld";
            this.labelPaginaInBeeld.Size = new System.Drawing.Size(46, 18);
            this.labelPaginaInBeeld.TabIndex = 1;
            this.labelPaginaInBeeld.Text = "label1";
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
            this.terugToolStripMenuItem,
            this.homeToolStripMenuItem,
            this.editPaginaToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1884, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // terugToolStripMenuItem
            // 
            this.terugToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.terugToolStripMenuItem.Name = "terugToolStripMenuItem";
            this.terugToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.terugToolStripMenuItem.Text = "Terug";
            this.terugToolStripMenuItem.Click += new System.EventHandler(this.terugToolStripMenuItem_Click);
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.homeToolStripMenuItem.Text = "Home";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // editPaginaToolStripMenuItem
            // 
            this.editPaginaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editModeAanToolStripMenuItem,
            this.addItemToolStripMenuItem,
            this.deleteItemToolStripMenuItem,
            this.saveHuidigePaginaToolStripMenuItem});
            this.editPaginaToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editPaginaToolStripMenuItem.Name = "editPaginaToolStripMenuItem";
            this.editPaginaToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editPaginaToolStripMenuItem.Text = "Edit";
            // 
            // editModeAanToolStripMenuItem
            // 
            this.editModeAanToolStripMenuItem.CheckOnClick = true;
            this.editModeAanToolStripMenuItem.Name = "editModeAanToolStripMenuItem";
            this.editModeAanToolStripMenuItem.Size = new System.Drawing.Size(214, 24);
            this.editModeAanToolStripMenuItem.Text = "Edit Mode Aan";
            this.editModeAanToolStripMenuItem.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // addItemToolStripMenuItem
            // 
            this.addItemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toevoegenHoofdstukTextToolStripMenuItem,
            this.toevoegenLinkNaarFileToolStripMenuItem,
            this.toevoegenLinkNaarDirToolStripMenuItem,
            this.toevoegenTekstBlokToolStripMenuItem,
            this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem,
            this.toevoegenLegeRegelToolStripMenuItem});
            this.addItemToolStripMenuItem.Enabled = false;
            this.addItemToolStripMenuItem.Name = "addItemToolStripMenuItem";
            this.addItemToolStripMenuItem.Size = new System.Drawing.Size(214, 24);
            this.addItemToolStripMenuItem.Text = "Add Item";
            // 
            // toevoegenHoofdstukTextToolStripMenuItem
            // 
            this.toevoegenHoofdstukTextToolStripMenuItem.Name = "toevoegenHoofdstukTextToolStripMenuItem";
            this.toevoegenHoofdstukTextToolStripMenuItem.Size = new System.Drawing.Size(313, 24);
            this.toevoegenHoofdstukTextToolStripMenuItem.Text = "Toevoegen Hoofdstuk text";
            this.toevoegenHoofdstukTextToolStripMenuItem.Click += new System.EventHandler(this.toevoegenHoofdstukTextToolStripMenuItem_Click);
            // 
            // toevoegenLinkNaarFileToolStripMenuItem
            // 
            this.toevoegenLinkNaarFileToolStripMenuItem.Name = "toevoegenLinkNaarFileToolStripMenuItem";
            this.toevoegenLinkNaarFileToolStripMenuItem.Size = new System.Drawing.Size(313, 24);
            this.toevoegenLinkNaarFileToolStripMenuItem.Text = "Toevoegen Link Naar File";
            this.toevoegenLinkNaarFileToolStripMenuItem.Click += new System.EventHandler(this.toevoegenLinkNaarFileToolStripMenuItem_Click);
            // 
            // toevoegenLinkNaarDirToolStripMenuItem
            // 
            this.toevoegenLinkNaarDirToolStripMenuItem.Name = "toevoegenLinkNaarDirToolStripMenuItem";
            this.toevoegenLinkNaarDirToolStripMenuItem.Size = new System.Drawing.Size(313, 24);
            this.toevoegenLinkNaarDirToolStripMenuItem.Text = "Toevoegen Link Naar Directory";
            this.toevoegenLinkNaarDirToolStripMenuItem.Click += new System.EventHandler(this.toevoegenLinkNaarDirToolStripMenuItem_Click);
            // 
            // toevoegenTekstBlokToolStripMenuItem
            // 
            this.toevoegenTekstBlokToolStripMenuItem.Name = "toevoegenTekstBlokToolStripMenuItem";
            this.toevoegenTekstBlokToolStripMenuItem.Size = new System.Drawing.Size(313, 24);
            this.toevoegenTekstBlokToolStripMenuItem.Text = "Toevoegen Tekst Blok";
            this.toevoegenTekstBlokToolStripMenuItem.Click += new System.EventHandler(this.toevoegenTekstBlokToolStripMenuItem_Click);
            // 
            // toevoegenLinkNaarNieuwePaginaToolStripMenuItem
            // 
            this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem.Name = "toevoegenLinkNaarNieuwePaginaToolStripMenuItem";
            this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem.Size = new System.Drawing.Size(313, 24);
            this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem.Text = "Toevoegen Link naar nieuwe Pagina";
            this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem.Click += new System.EventHandler(this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem_Click);
            // 
            // toevoegenLegeRegelToolStripMenuItem
            // 
            this.toevoegenLegeRegelToolStripMenuItem.Name = "toevoegenLegeRegelToolStripMenuItem";
            this.toevoegenLegeRegelToolStripMenuItem.Size = new System.Drawing.Size(313, 24);
            this.toevoegenLegeRegelToolStripMenuItem.Text = "Toevoegen Lege Regel";
            this.toevoegenLegeRegelToolStripMenuItem.Click += new System.EventHandler(this.toevoegenLegeRegelToolStripMenuItem_Click);
            // 
            // deleteItemToolStripMenuItem
            // 
            this.deleteItemToolStripMenuItem.Enabled = false;
            this.deleteItemToolStripMenuItem.Name = "deleteItemToolStripMenuItem";
            this.deleteItemToolStripMenuItem.Size = new System.Drawing.Size(214, 24);
            this.deleteItemToolStripMenuItem.Text = "Delete Item";
            this.deleteItemToolStripMenuItem.Click += new System.EventHandler(this.deleteItemToolStripMenuItem_Click);
            // 
            // saveHuidigePaginaToolStripMenuItem
            // 
            this.saveHuidigePaginaToolStripMenuItem.Enabled = false;
            this.saveHuidigePaginaToolStripMenuItem.Name = "saveHuidigePaginaToolStripMenuItem";
            this.saveHuidigePaginaToolStripMenuItem.Size = new System.Drawing.Size(214, 24);
            this.saveHuidigePaginaToolStripMenuItem.Text = "Save Huidige Pagina";
            this.saveHuidigePaginaToolStripMenuItem.Click += new System.EventHandler(this.saveHuidigePaginaToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // panelUpDown
            // 
            this.panelUpDown.BackColor = System.Drawing.Color.LightSalmon;
            this.panelUpDown.Controls.Add(this.buttonDelete);
            this.panelUpDown.Controls.Add(this.button6);
            this.panelUpDown.Controls.Add(this.button5);
            this.panelUpDown.Controls.Add(this.button4);
            this.panelUpDown.Controls.Add(this.button3);
            this.panelUpDown.Controls.Add(this.button2);
            this.panelUpDown.Controls.Add(this.button1);
            this.panelUpDown.Controls.Add(this.buttonMoveDown);
            this.panelUpDown.Controls.Add(this.buttonMoveUp);
            this.panelUpDown.Location = new System.Drawing.Point(822, 12);
            this.panelUpDown.Name = "panelUpDown";
            this.panelUpDown.Size = new System.Drawing.Size(413, 228);
            this.panelUpDown.TabIndex = 0;
            this.panelUpDown.Visible = false;
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Location = new System.Drawing.Point(3, 118);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(128, 100);
            this.buttonMoveDown.TabIndex = 1;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Location = new System.Drawing.Point(3, 6);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(128, 100);
            this.buttonMoveUp.TabIndex = 0;
            this.buttonMoveUp.Text = "Up";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(139, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(267, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Toevoegen Hoofdstuk Tekst";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.toevoegenHoofdstukTextToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(139, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(267, 29);
            this.button2.TabIndex = 3;
            this.button2.Text = "Toevoegen Link naar File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.toevoegenLinkNaarFileToolStripMenuItem_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(139, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(267, 29);
            this.button3.TabIndex = 4;
            this.button3.Text = "Toevoegen Link naar Dir";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.toevoegenLinkNaarDirToolStripMenuItem_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(139, 95);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(267, 29);
            this.button4.TabIndex = 5;
            this.button4.Text = "Toevoegen Tekst Blok";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(139, 126);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(267, 29);
            this.button5.TabIndex = 6;
            this.button5.Text = "Toevoegen Link naar nieuwe Pagina";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.toevoegenLinkNaarNieuwePaginaToolStripMenuItem_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(139, 156);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(267, 29);
            this.button6.TabIndex = 7;
            this.button6.Text = "Toevoegen Lege Regel";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.toevoegenLegeRegelToolStripMenuItem_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDelete.Location = new System.Drawing.Point(139, 189);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(267, 29);
            this.buttonDelete.TabIndex = 8;
            this.buttonDelete.Text = "Delete Selectie";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.deleteItemToolStripMenuItem_Click);
            // 
            // KennisMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1884, 961);
            this.Controls.Add(this.panelUpDown);
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
            this.panelUpDown.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem deleteItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toevoegenLinkNaarNieuwePaginaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.Label labelPaginaInBeeld;
        private System.Windows.Forms.ToolStripMenuItem saveHuidigePaginaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toevoegenLegeRegelToolStripMenuItem;
        private System.Windows.Forms.Panel panelUpDown;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
    }
}

