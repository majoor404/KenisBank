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
            this.checkBoxIndex = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxZoek
            // 
            this.textBoxZoek.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxZoek.Location = new System.Drawing.Point(28, 143);
            this.textBoxZoek.Name = "textBoxZoek";
            this.textBoxZoek.Size = new System.Drawing.Size(573, 24);
            this.textBoxZoek.TabIndex = 0;
            // 
            // buttonZoek
            // 
            this.buttonZoek.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonZoek.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonZoek.Location = new System.Drawing.Point(438, 193);
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
            this.button1.Location = new System.Drawing.Point(28, 193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 56);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // checkBoxIndex
            // 
            this.checkBoxIndex.AutoSize = true;
            this.checkBoxIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxIndex.Location = new System.Drawing.Point(437, 15);
            this.checkBoxIndex.Name = "checkBoxIndex";
            this.checkBoxIndex.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxIndex.Size = new System.Drawing.Size(164, 22);
            this.checkBoxIndex.TabIndex = 4;
            this.checkBoxIndex.Text = "Maak Index Opnieuw";
            this.checkBoxIndex.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(408, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(23, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "?";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ZoekForm
            // 
            this.AcceptButton = this.buttonZoek;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 273);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBoxIndex);
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
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.CheckBox checkBoxIndex;
    }
}