namespace KenisBank
{
    partial class TekstBlok
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTextBlok = new System.Windows.Forms.TextBox();
            this.buttonTextBlok = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(364, 18);
            this.label2.TabIndex = 9;
            this.label2.Text = "Hier kunt u vrij text invullen, of plakken vanuit klembord";
            // 
            // textBoxTextBlok
            // 
            this.textBoxTextBlok.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTextBlok.Location = new System.Drawing.Point(23, 40);
            this.textBoxTextBlok.Multiline = true;
            this.textBoxTextBlok.Name = "textBoxTextBlok";
            this.textBoxTextBlok.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxTextBlok.Size = new System.Drawing.Size(1174, 568);
            this.textBoxTextBlok.TabIndex = 10;
            this.textBoxTextBlok.WordWrap = false;
            // 
            // buttonTextBlok
            // 
            this.buttonTextBlok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonTextBlok.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTextBlok.Location = new System.Drawing.Point(1030, 628);
            this.buttonTextBlok.Name = "buttonTextBlok";
            this.buttonTextBlok.Size = new System.Drawing.Size(167, 54);
            this.buttonTextBlok.TabIndex = 11;
            this.buttonTextBlok.Text = "Save";
            this.buttonTextBlok.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(23, 628);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 54);
            this.button1.TabIndex = 12;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TekstBlok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 704);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonTextBlok);
            this.Controls.Add(this.textBoxTextBlok);
            this.Controls.Add(this.label2);
            this.Name = "TekstBlok";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TekstBlok";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBoxTextBlok;
        private System.Windows.Forms.Button buttonTextBlok;
        private System.Windows.Forms.Button button1;
    }
}