namespace KenisBank
{
    partial class ZoekForm
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
            this.textBoxZoek = new System.Windows.Forms.TextBox();
            this.buttonZoek = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxZoek
            // 
            this.textBoxZoek.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxZoek.Location = new System.Drawing.Point(29, 24);
            this.textBoxZoek.Name = "textBoxZoek";
            this.textBoxZoek.Size = new System.Drawing.Size(573, 24);
            this.textBoxZoek.TabIndex = 0;
            // 
            // buttonZoek
            // 
            this.buttonZoek.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonZoek.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonZoek.Location = new System.Drawing.Point(439, 98);
            this.buttonZoek.Name = "buttonZoek";
            this.buttonZoek.Size = new System.Drawing.Size(163, 56);
            this.buttonZoek.TabIndex = 1;
            this.buttonZoek.Text = "Zoek";
            this.buttonZoek.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(29, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 56);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ZoekForm
            // 
            this.AcceptButton = this.buttonZoek;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 186);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonZoek);
            this.Controls.Add(this.textBoxZoek);
            this.Name = "ZoekForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ZoekForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonZoek;
        public System.Windows.Forms.TextBox textBoxZoek;
        private System.Windows.Forms.Button button1;
    }
}