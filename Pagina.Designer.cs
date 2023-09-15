namespace KenisBank
{
    partial class Pagina
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
            this.textBoxPaginaNaam = new System.Windows.Forms.TextBox();
            this.buttonHoofdstuk = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 18);
            this.label2.TabIndex = 13;
            this.label2.Text = "Pagina Naam";
            // 
            // textBoxPaginaNaam
            // 
            this.textBoxPaginaNaam.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPaginaNaam.Location = new System.Drawing.Point(231, 26);
            this.textBoxPaginaNaam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPaginaNaam.Name = "textBoxPaginaNaam";
            this.textBoxPaginaNaam.Size = new System.Drawing.Size(506, 24);
            this.textBoxPaginaNaam.TabIndex = 14;
            // 
            // buttonHoofdstuk
            // 
            this.buttonHoofdstuk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonHoofdstuk.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHoofdstuk.Location = new System.Drawing.Point(570, 70);
            this.buttonHoofdstuk.Name = "buttonHoofdstuk";
            this.buttonHoofdstuk.Size = new System.Drawing.Size(167, 54);
            this.buttonHoofdstuk.TabIndex = 15;
            this.buttonHoofdstuk.Text = "Save";
            this.buttonHoofdstuk.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(42, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 54);
            this.button1.TabIndex = 16;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Pagina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 156);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonHoofdstuk);
            this.Controls.Add(this.textBoxPaginaNaam);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Pagina";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pagina";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBoxPaginaNaam;
        private System.Windows.Forms.Button buttonHoofdstuk;
        private System.Windows.Forms.Button button1;
    }
}