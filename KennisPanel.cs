using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace KenisBank
{
    public class KennisPanel : Panel
    {
        // een kennis panel bevat een
        // 1) HoofdStuk, dus label
        // 2) Text
        // 3) Link , LinkLabel
        // 4) Pagina, Button

        public type kType { get; set; }
        public string kText { get; set; }
        public string kUrl { get; set; }
        private Panel kPanel { get; set; }
        public int kId { get; set; }

        private KennisPanel() { }
        public KennisPanel(Panel MainPanel, type Type, string Text, string Url)
        {
            kPanel = MainPanel;
            kType = Type;
            kText = Text;
            kUrl = Url;

            switch (kType)
            {
                case type.Hoofdstuk:
                    MaakHoofdStuk();
                    break;
                case type.TekstBlok:
                    MaakTekstBlok();
                    break;
                case type.LinkDir:
                    MaakLinkDir();
                    break;
                case type.LinkFile:
                    MaakLinkDir();
                    break;
                case type.PaginaNaam:
                    MaakPaginaKnop();
                    break;
                case type.Leeg:
                    kText = string.Empty;
                    MaakTekstBlok();
                    break;

            }

            kId = KennisMainForm.MaakID();
            Tag = kId;
        }

        private void MaakNewPanel()
        {
            Dock = DockStyle.Top;
            BorderStyle = BorderStyle.None;
            AutoSize = true;

            Click += new EventHandler(kKlik);
            kPanel.Controls.Add(this);
            kPanel.Controls.SetChildIndex(this, 0);
        }

        private void kKlik(object sender, EventArgs e)
        {
            KennisMainForm.KleurGeselecteerdePanel(kId);
        }

        private void kLinkKlik(object sender, EventArgs e)
        {
            KennisMainForm.Label_LinkKlik(kId);
        }

        private void kPaginaKlik(object sender, EventArgs e)
        {
            KennisMainForm.PaginaKlik(kId);
        }


        private void MaakHoofdStuk()
        {
            MaakNewPanel();

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            Point org = new Point(label.Location.X, label.Location.Y);
            org.X += 30;
            label.Location = org;
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold);
            label.Text = kText;

            Controls.Add(label);
            //Refresh();
        }

        private void MaakTekstBlok()
        {
            MaakNewPanel();

            //SuspendLayout();

            // split string at new line
            string[] result = kText.Split('\n');
            int regeloffset = 0;
            foreach (string str in result)
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                Point org = new Point(label.Location.X, label.Location.Y);
                org.X += 30;
                org.Y += regeloffset * 20;
                regeloffset++;
                label.Location = org;
                label.AutoSize = true;
                label.MaximumSize = new Size(kPanel.Width - 60, 0);
                label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                label.Text = str;
                Controls.Add(label);
            }
            //ResumeLayout();
        }

        private void MaakLinkDir()
        {
            MaakNewPanel();

            System.Windows.Forms.LinkLabel label = new System.Windows.Forms.LinkLabel();

            Point org = new Point(label.Location.X, label.Location.Y);
            org.X += 30;
            label.Location = org;

            label.Width = kPanel.Width - 100;
            label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            label.Text = kText;
            label.BorderStyle = BorderStyle.None;
            label.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(kLinkKlik);
            //label.MouseHover += new EventHandler(LinkHover);

            Controls.Add(label);
            //Refresh();
        }

        private void MaakPaginaKnop()
        {
            MaakNewPanel();

            System.Windows.Forms.Button but = new System.Windows.Forms.Button();

            Point org = new Point(but.Location.X, but.Location.Y);

            if (kPanel.Width > 235)
            {
                org.X += 30;
                but.Width = 500;
                but.Height = 30;
            }
            else
            {
                org.X += 10;
                but.Width = 200;
                but.Height = 30;
            }
            but.Location = org;
            but.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            but.Text = kText;
            but.Click += new EventHandler(kPaginaKlik);
            Controls.Add(but);
            //Refresh();
        }
    }
}
