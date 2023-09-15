using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class KennisMainForm : Form
    {
        public Regel InfoPagina = new Regel();
        public Panel panelGeselecteerd = new Panel();
        public bool change_pagina = false;
        public Point screenLoc = new Point();

        public KennisMainForm()
        {
            InitializeComponent();
        }

        private void KennisMainForm_Shown(object sender, EventArgs e)
        {
            // zet panelen netjes
            KennisMainForm_Resize(this, null);
            // laad Start.xml
            if (InfoPagina.Laad("Start"))
            {
                labelPaginaInBeeld.Text = "Start";
            }

            // voor debug
            //Toevoegen("eerste regel", type.TekstBlok, "");
            //Toevoegen("tweede regel", type.TekstBlok, "");
            //Toevoegen("derde regel", type.TekstBlok, "");

            // bouw Pagina
            BouwPaginaOp();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            addItemToolStripMenuItem.Enabled = editModeAanToolStripMenuItem.Checked;
            saveHuidigePaginaToolStripMenuItem.Enabled = editModeAanToolStripMenuItem.Checked;

            if (editModeAanToolStripMenuItem.Checked)
            {
                editPaginaToolStripMenuItem.BackColor = Color.LightCyan;
                labelEditMode.Visible = true;
                change_pagina = false;
            }
            else
            {
                if (change_pagina)
                {
                    DialogResult dialogResult = MessageBox.Show($"Pagina is aangepast, eerst saven ?", "Vraagje", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        saveHuidigePaginaToolStripMenuItem_Click(this, null);
                    }
                }
                editPaginaToolStripMenuItem.BackColor = SystemColors.MenuBar;
                labelEditMode.Visible = false;

                foreach (Panel a in panelMain.Controls)
                {
                    a.BackColor = panelMain.BackColor;
                    a.BorderStyle = BorderStyle.None;
                }
            }
        }

        private Panel MaakNewPanel(int eigenaar)
        {
            // maak new panel
            Panel panel = new Panel
            {
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.None,
                AutoSize = true,
                Tag = eigenaar
            };

            panelMain.Controls.Add(panel);
            panelMain.Controls.SetChildIndex(panel, 0);
            panel.Click += new EventHandler(panel_Click);
            panel.MouseDown += new MouseEventHandler(muisknop_neer);
            panel.MouseUp += new MouseEventHandler(muisknop_op);
            return panel;
        }

        private void PlaatsHoofdstukOpBeeld(string text, int eigenaar)
        {
            Panel panel = MaakNewPanel(eigenaar);

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

        private void PlaatsTextOpBeeld(string tekst, int eigenaar)
        {

            Panel panel = MaakNewPanel(eigenaar);

            panel.SuspendLayout();

            // split string at new line
            //string t = tekst.Replace("\n", "");
            string[] result = tekst.Split('\n');
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

        private void PlaatsLinkOpBeeld(string link, string locatie, int eigenaar)
        {
            Panel panel = MaakNewPanel(eigenaar);


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
            string link = (string)label.Tag;
            if (link[0] == '#') // nieuwe pagina
            {
                link = link.Substring(1);
                if (!InfoPagina.Laad(link))
                {
                    // dus nieuwe pagina
                    InfoPagina.PaginaMetRegels.Clear();
                    labelPaginaInBeeld.Text = link;
                    Toevoegen(link, type.Hoofdstuk, "");
                    BouwPaginaOp();
                    // meteen in edit mode
                    if (!editModeAanToolStripMenuItem.Checked)
                    {
                        editModeAanToolStripMenuItem.Checked = true;
                        buttonEdit_Click(this, null);
                    }

                }
                else
                {
                    labelPaginaInBeeld.Text = link;
                    // bouw Pagina
                    BouwPaginaOp();
                }
            }
            else
            {
                label.LinkVisited = true;
                Process process = new Process();
                process.StartInfo.FileName = link;
                _ = process.Start();
            }
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
                string dum = InfoPagina.PaginaMetRegels[i].tekst_ + InfoPagina.PaginaMetRegels[i].type_ + InfoPagina.PaginaMetRegels[i].url_;
                int eigenaar = dum.GetHashCode();

                if (InfoPagina.PaginaMetRegels[i].type_ == type.Hoofdstuk)
                {
                    PlaatsHoofdstukOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, eigenaar);
                    InfoPagina.PaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkDir)
                {
                    PlaatsLinkOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].url_, eigenaar);
                    InfoPagina.PaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkFile)
                {
                    PlaatsLinkOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].url_, eigenaar);
                    InfoPagina.PaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.TekstBlok)
                {
                    PlaatsTextOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, eigenaar);
                    InfoPagina.PaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.PaginaNaam)
                {
                    PlaatsLinkOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].url_, eigenaar);
                    InfoPagina.PaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (InfoPagina.PaginaMetRegels[i].type_ == type.Leeg)
                {
                    PlaatsTextOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, eigenaar);
                    InfoPagina.PaginaMetRegels[i].eigenaar_ = eigenaar;
                }
            }
        }

        public void Toevoegen(string text, type type, string url)
        {
            Regel regel = new Regel(text, type, url);
            InfoPagina.PaginaMetRegels.Add(regel);
            change_pagina = true;
        }

        private void panel_Click(object sender, EventArgs e)
        {
            panelGeselecteerd = null;
            deleteItemToolStripMenuItem.Enabled = false;
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
                        panelGeselecteerd = a;
                        deleteItemToolStripMenuItem.Enabled = true;
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

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int eigenaar = (int)panelGeselecteerd.Tag;

            for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
            {
                if (InfoPagina.PaginaMetRegels[i].eigenaar_ == eigenaar)
                {
                    InfoPagina.PaginaMetRegels.RemoveAt(i);
                }
            }

            panelGeselecteerd = null;
            deleteItemToolStripMenuItem.Enabled = false;

            foreach (Panel a in panelMain.Controls)
            {
                a.BackColor = panelMain.BackColor;
                a.BorderStyle = BorderStyle.None;
            }

            // bouw Pagina
            BouwPaginaOp();
        }

        private void toevoegenLinkNaarNieuwePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pagina pagina = new Pagina();
            DialogResult save = pagina.ShowDialog();
            if (save == DialogResult.OK)
            {
                string urlnaarpagina = "#" + pagina.textBoxPaginaNaam.Text;
                Toevoegen(pagina.textBoxPaginaNaam.Text, type.PaginaNaam, urlnaarpagina);
            }
            // bouw Pagina
            BouwPaginaOp();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);

            // laad Start.xml
            if (InfoPagina.Laad("Start"))
            {
                labelPaginaInBeeld.Text = "Start";
            }

            // bouw Pagina
            BouwPaginaOp();
        }

        private void saveHuidigePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoPagina.Save(labelPaginaInBeeld.Text);
            change_pagina = false;
        }

        private void toevoegenLegeRegelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Toevoegen(" ", type.Leeg, "");
        }

        private void muisknop_neer(object sender, MouseEventArgs e)
        {
            screenLoc = PointToScreen(e.Location);
        }

        private void muisknop_op(object sender, MouseEventArgs e)
        {
            if (!editModeAanToolStripMenuItem.Checked)
            {
                return;
            }

            Point screenLoc_nu = PointToScreen(e.Location);
            int verschil = Math.Abs(screenLoc_nu.Y - screenLoc.Y);

            if (verschil < 30)
            {
                return;
            }

            int richting = screenLoc_nu.Y > screenLoc.Y ? 1 : -1;
            int eigenaar = (int)panelGeselecteerd.Tag;

            for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
            {
                if (InfoPagina.PaginaMetRegels[i].eigenaar_ == eigenaar)
                {
                    int nieuw_index = i + richting;

                    if (nieuw_index > -1 && nieuw_index + 1 > InfoPagina.PaginaMetRegels.Count)
                    {
                        return;
                    }

                    Regel gekozen = InfoPagina.PaginaMetRegels[i];
                    InfoPagina.PaginaMetRegels.RemoveAt(i);
                    InfoPagina.PaginaMetRegels.Insert(nieuw_index, gekozen);
                    i = 1000;
                }
            }

            // bouw Pagina
            BouwPaginaOp();

            
        }
    }
}
