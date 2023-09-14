using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class KennisMainForm : Form
    {
        public Regel InfoPagina = new Regel();

        public KennisMainForm()
        {
            InitializeComponent();
        }

        private void KennisMainForm_Shown(object sender, EventArgs e)
        {
            // zet panelen netjes
            KennisMainForm_Resize(this, null);
            // laad main.xml
            _ = InfoPagina.Laad(@"Data\main.xml");

            // voor debug
            Toevoegen("Hallo", type.LinkDir, "C:\\");
            Toevoegen("Lege pagina", type.TekstBlok, "");

            // bouw Pagina
            BouwPaginaOp();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            addItemToolStripMenuItem.Enabled = editModeAanToolStripMenuItem.Checked;

            if (editModeAanToolStripMenuItem.Checked)
            {
                editPaginaToolStripMenuItem.BackColor = Color.LightCyan;
                labelEditMode.Visible = true;
            }
            else
            {
                editPaginaToolStripMenuItem.BackColor = SystemColors.MenuBar;
                labelEditMode.Visible = false;

                foreach (Panel a in panelMain.Controls)
                {
                    a.BackColor = panelMain.BackColor;
                    a.BorderStyle = BorderStyle.None;
                }
            }
        }

        private Panel MaakNewPanel()
        {
            // maak new panel
            Panel panel = new Panel
            {
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.None,
                AutoSize = true
            };
            panelMain.Controls.Add(panel);
            panelMain.Controls.SetChildIndex(panel, 0);

            panel.Click += new EventHandler(panel_Click);
            return panel;
        }

        private void PlaatsHoofdstukOpBeeld(string text)
        {
            Panel panel = MaakNewPanel();

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            Point org = new Point(label.Location.X, label.Location.Y);
            org.X += 30;
            label.Location = org;
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold);
            label.Text = text;

            panel.Controls.Add(label);
            panelMain.Refresh();
        }

        private void PlaatsTextOpBeeld(string tekst)
        {
            
            Panel panel = MaakNewPanel();
            panel.SuspendLayout();
            
            // split string at new line
            string t = tekst.Replace("\n", "");
            string[] result = t.Split('\r');
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
                label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                label.Text = str;
                panel.Controls.Add(label);
            }
            panel.ResumeLayout();
        }

        private void PlaatsLinkOpBeeld(string link, string locatie)
        {
            Panel panel = MaakNewPanel();

            System.Windows.Forms.LinkLabel label = new System.Windows.Forms.LinkLabel();

            Point org = new Point(label.Location.X, label.Location.Y);
            org.X += 30;
            label.Location = org;

            label.Width = panelMain.Width - 100;
            label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            label.Text = link;
            label.Tag = locatie;
            label.BorderStyle = BorderStyle.None;
            label.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(label_LinkClicked);

            panel.Controls.Add(label);
            panelMain.Refresh();
        }

        private void label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.LinkLabel label = (System.Windows.Forms.LinkLabel)sender;
            label.LinkVisited = true;
            Process process = new Process();
            process.StartInfo.FileName = (string)label.Tag;
            _ = process.Start();
        }

        private void KennisMainForm_Resize(object sender, EventArgs e)
        {
            panel1.Width = Width - 45;
            panelMain.Width = Width - 45;
            panelMain.Height = Height - 150;
        }

        private void BouwPaginaOp()
        {
            // delete oude
            panelMain.Controls.Clear();

            for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
            {
                if (InfoPagina.PaginaMetRegels[i].type_ == type.Hoofdstuk)
                {
                    PlaatsHoofdstukOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_);
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkDir)
                {
                    PlaatsLinkOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].url_);
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkFile)
                {
                    PlaatsLinkOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].url_);
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.TekstBlok)
                {
                    PlaatsTextOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_);
                }
            }
        }

        public void Toevoegen(string text, type type, string url)
        {
            Regel regel = new Regel(text, type, url);
            InfoPagina.PaginaMetRegels.Add(regel);
        }

        private void panel_Click(object sender, EventArgs e)
        {
            if (editModeAanToolStripMenuItem.Checked)
            {
                foreach (Panel a in panelMain.Controls)
                {
                    a.BackColor = panelMain.BackColor;
                    a.BorderStyle = BorderStyle.None;
                    if (a == sender)
                    {
                        a.BackColor = Color.Aqua;
                        a.BorderStyle = BorderStyle.FixedSingle;
                    }
                }
            }
        }

        private void toevoegenLinkNaarDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LinkDir linkdir = new LinkDir();
            DialogResult save = linkdir.ShowDialog();
            if (save == DialogResult.OK)
            {
                Toevoegen(linkdir.textBoxLinkText.Text, type.LinkDir, linkdir.textBoxDir.Text);
            }
            // bouw Pagina
            BouwPaginaOp();
        }

        private void toevoegenHoofdstukTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hoofdstuk hoofdstuk = new Hoofdstuk();
            DialogResult save = hoofdstuk.ShowDialog();
            if (save == DialogResult.OK)
            {
                Toevoegen(hoofdstuk.textBox1.Text, type.Hoofdstuk, "");
            }
            // bouw Pagina
            BouwPaginaOp();
        }

        private void toevoegenLinkNaarFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LinkFile linkFile = new LinkFile();
            DialogResult save = linkFile.ShowDialog();
            if (save == DialogResult.OK)
            {
                Toevoegen(linkFile.textBox2.Text, type.LinkFile, linkFile.textBox1.Text);
            }
            // bouw Pagina
            BouwPaginaOp();
        }

        private void toevoegenTekstBlokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TekstBlok tekstblok = new TekstBlok();
            DialogResult save = tekstblok.ShowDialog();
            if (save == DialogResult.OK)
            {
                Toevoegen(tekstblok.textBoxTextBlok.Text, type.TekstBlok, "");
            }
            // bouw Pagina
            BouwPaginaOp();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // bouw Pagina
            BouwPaginaOp();
        }
    }
}
