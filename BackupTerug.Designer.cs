namespace KenisBank
{
    partial class BackupTerug
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PaginaNaam = new System.Windows.Forms.Label();
            this.labelHuidig = new System.Windows.Forms.Label();
            this.labelBackup1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonBackup1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zet backup terug.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Huidige edit datum";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Backup";
            // 
            // PaginaNaam
            // 
            this.PaginaNaam.AutoSize = true;
            this.PaginaNaam.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PaginaNaam.Location = new System.Drawing.Point(290, 13);
            this.PaginaNaam.Name = "PaginaNaam";
            this.PaginaNaam.Size = new System.Drawing.Size(97, 18);
            this.PaginaNaam.TabIndex = 0;
            this.PaginaNaam.Text = "Pagina Naam";
            // 
            // labelHuidig
            // 
            this.labelHuidig.AutoSize = true;
            this.labelHuidig.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHuidig.Location = new System.Drawing.Point(290, 63);
            this.labelHuidig.Name = "labelHuidig";
            this.labelHuidig.Size = new System.Drawing.Size(97, 18);
            this.labelHuidig.TabIndex = 0;
            this.labelHuidig.Text = "Pagina Naam";
            // 
            // labelBackup1
            // 
            this.labelBackup1.AutoSize = true;
            this.labelBackup1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBackup1.Location = new System.Drawing.Point(290, 110);
            this.labelBackup1.Name = "labelBackup1";
            this.labelBackup1.Size = new System.Drawing.Size(97, 18);
            this.labelBackup1.TabIndex = 0;
            this.labelBackup1.Text = "Pagina Naam";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(1059, 52);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(213, 41);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonBackup1
            // 
            this.buttonBackup1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBackup1.Location = new System.Drawing.Point(1059, 99);
            this.buttonBackup1.Name = "buttonBackup1";
            this.buttonBackup1.Size = new System.Drawing.Size(213, 41);
            this.buttonBackup1.TabIndex = 1;
            this.buttonBackup1.Text = "Zet Backup terug";
            this.buttonBackup1.UseVisualStyleBackColor = true;
            this.buttonBackup1.Click += new System.EventHandler(this.buttonBackup1_Click);
            // 
            // BackupTerug
            // 
            this.AcceptButton = this.buttonCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(1293, 229);
            this.Controls.Add(this.buttonBackup1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelBackup1);
            this.Controls.Add(this.labelHuidig);
            this.Controls.Add(this.PaginaNaam);
            this.Controls.Add(this.label1);
            this.Name = "BackupTerug";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vorige Pagina Terug Zetten";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label PaginaNaam;
        public System.Windows.Forms.Label labelHuidig;
        public System.Windows.Forms.Label labelBackup1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonBackup1;
    }
}