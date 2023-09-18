using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/*
 * in roetine  BouwPaginaOp maak ik een hash van de InfoPagina.Regel
 * Deze hash plaats ik in panel.tag die dan gemaakt word voor die InfoPagina.Regel
 * hierna plaats ik de zelfde hash dan in de betrevende InfoPagina.Regel
 * 
 * Hierdoor is tijdens runtime NA opbouwen pagina panel en regel gekoppeld.
 */



namespace KenisBank
{
    public partial class KennisMainForm : Form
    {
        public Regel InfoPagina = new Regel();
        public Panel panelGeselecteerd = new Panel();
        public bool change_pagina = false;
        private static readonly Random random = new Random();
        private readonly List<Regel> PaginaMetRegelsGevonden = new List<Regel>();
        private readonly List<string> history = new List<string>();

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
                HistoryBalkUpdate(labelPaginaInBeeld.Text);
            }

            // bouw Pagina
            BouwPaginaOp();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            addItemToolStripMenuItem.Enabled = editModeAanToolStripMenuItem.Checked;
            saveHuidigePaginaToolStripMenuItem.Enabled = buttonSaveCloseEdit.Enabled = editModeAanToolStripMenuItem.Checked;

            if (editModeAanToolStripMenuItem.Checked)
            {
                editPaginaToolStripMenuItem.BackColor = Color.LightCyan;
                change_pagina = false;
                SelecteerEerstePaneel();

                Point newlocatie = new Point
                {
                    X = panelMain.Width - panelUpDown.Width - 10,
                    Y = panelMain.Location.Y + 10
                };
                panelUpDown.Location = newlocatie;
                panelUpDown.Visible = true;
                panelUpDown.BringToFront();
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
                panelUpDown.Visible = false;

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
            label.MouseHover += new EventHandler(LinkHover);

            panel.Controls.Add(label);
            panelMain.Refresh();
        }

        private void PlaatsPaginaOpBeeld(string link, string locatie, int eigenaar)
        {
            Panel panel = MaakNewPanel(eigenaar);
            System.Windows.Forms.Button but = new Button();

            Point org = new Point(but.Location.X, but.Location.Y);
            org.X += 30;
            but.Location = org;

            but.Width = 300;
            but.Height = 30;
            but.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            but.Text = link;
            //but.Tag = locatie;
            //but.BorderStyle = BorderStyle.None;
            but.Click += new EventHandler(PaginaButtonClick);
            panel.Controls.Add(but);
            panelMain.Refresh();
        }

        private void label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.LinkLabel label = (System.Windows.Forms.LinkLabel)sender;
            string link = (string)label.Tag;
            label.LinkVisited = true;
            Process process = new Process();
            process.StartInfo.FileName = link;
            try
            {
                _ = process.Start();
            }
            catch { }
        }

        private void PaginaButtonClick(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            string pagina = but.Text;

            if (!InfoPagina.Laad(pagina))
            {
                // dus nieuwe pagina
                InfoPagina.PaginaMetRegels.Clear();
                labelPaginaInBeeld.Text = pagina;
                HistoryBalkUpdate(labelPaginaInBeeld.Text);
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
                labelPaginaInBeeld.Text = pagina;
                HistoryBalkUpdate(labelPaginaInBeeld.Text);
                // bouw Pagina
                BouwPaginaOp();
            }
        }

        private void KennisMainForm_Resize(object sender, EventArgs e)
        {
            panel1.Width = Width - 45;
            panelMain.Width = Width - 45;
            panelMain.Height = Height - 180;
        }

        private void BouwPaginaOp()
        {
            // delete oude
            panelMain.Controls.Clear();

            for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
            {
                string dum = RandomString(10);
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
                    //PlaatsLinkOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].url_, eigenaar);
                    PlaatsPaginaOpBeeld(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].url_, eigenaar);
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
            deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = buttonEditSelectie.Enabled = false;
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
                        deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = buttonEditSelectie.Enabled = true;
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
            SelecteerLaatstePaneel();
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
            SelecteerLaatstePaneel();
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
            SelecteerLaatstePaneel();
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
            SelecteerLaatstePaneel();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // bouw Pagina
            BouwPaginaOp();
        }

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Zeker weten, verwijderen?", "Vraagje", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
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
                deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = false;


                foreach (Panel a in panelMain.Controls)
                {
                    a.BackColor = panelMain.BackColor;
                    a.BorderStyle = BorderStyle.None;
                }

                // bouw Pagina
                BouwPaginaOp();
            }
        }

        private void toevoegenLinkNaarNieuwePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pagina pagina = new Pagina();
            DialogResult save = pagina.ShowDialog();
            if (save == DialogResult.OK)
            {
                Toevoegen(pagina.textBoxPaginaNaam.Text, type.PaginaNaam, "");
            }
            // bouw Pagina
            BouwPaginaOp();
            SelecteerLaatstePaneel();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);

            // laad Start.xml
            if (InfoPagina.Laad("Start"))
            {
                labelPaginaInBeeld.Text = "Start";
                HistoryBalkUpdate(labelPaginaInBeeld.Text);
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
            // bouw Pagina
            BouwPaginaOp();
            // meteen selecteren
            SelecteerLaatstePaneel();
        }

        private void SelecteerLaatstePaneel()
        {
            panelGeselecteerd = null;
            deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = buttonEditSelectie.Enabled = false;
            foreach (Panel a in panelMain.Controls)
            {
                a.BackColor = panelMain.BackColor;
                a.BorderStyle = BorderStyle.None;
                if ((int)a.Tag == InfoPagina.PaginaMetRegels[InfoPagina.PaginaMetRegels.Count - 1].eigenaar_)
                {
                    a.BackColor = Color.Aqua;
                    a.BorderStyle = BorderStyle.FixedSingle;
                    panelGeselecteerd = a;
                    deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = buttonEditSelectie.Enabled = true;
                }
            }
        }

        private void SelecteerEerstePaneel()
        {
            panelGeselecteerd = null;
            deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = buttonEditSelectie.Enabled = false;
            foreach (Panel a in panelMain.Controls)
            {
                a.BackColor = panelMain.BackColor;
                a.BorderStyle = BorderStyle.None;
                if ((int)a.Tag == InfoPagina.PaginaMetRegels[0].eigenaar_)
                {
                    a.BackColor = Color.Aqua;
                    a.BorderStyle = BorderStyle.FixedSingle;
                    panelGeselecteerd = a;
                    deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = buttonEditSelectie.Enabled = true;
                }
            }
        }

        private void MovePanel(int richting)
        {
            if (!editModeAanToolStripMenuItem.Checked)
            {
                return;
            }

            int eigenaar = (int)panelGeselecteerd.Tag;
            int nieuw_index = -1;

            for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
            {
                if (InfoPagina.PaginaMetRegels[i].eigenaar_ == eigenaar)
                {
                    nieuw_index = i + richting;

                    if (nieuw_index < 0 || nieuw_index + 1 > InfoPagina.PaginaMetRegels.Count)
                    {
                        return;
                    }

                    Regel gekozen = InfoPagina.PaginaMetRegels[i];
                    InfoPagina.PaginaMetRegels.RemoveAt(i);
                    InfoPagina.PaginaMetRegels.Insert(nieuw_index, gekozen);
                    i = 1000;
                    change_pagina = true;
                }
            }

            // bouw Pagina
            BouwPaginaOp();

            // nu weer regel op nieuw_index kleuren
            // get eigenaar nummer
            int eig = InfoPagina.PaginaMetRegels[nieuw_index].eigenaar_;
            foreach (Panel panel in panelMain.Controls)
            {
                if ((int)panel.Tag == eig)
                {
                    panel.BackColor = Color.Aqua;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panelGeselecteerd = panel;
                }
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            MovePanel(-1);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            MovePanel(1);
        }

        private void buttonSaveCloseEdit_Click(object sender, EventArgs e)
        {
            saveHuidigePaginaToolStripMenuItem_Click(this, null);
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);
        }

        private void buttonEditSelectie_Click(object sender, EventArgs e)
        {
            int eigenaar = (int)panelGeselecteerd.Tag;

            for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
            {
                if (InfoPagina.PaginaMetRegels[i].eigenaar_ == eigenaar)
                {
                    // oke panel en regel nu bekend.
                    if (InfoPagina.PaginaMetRegels[i].type_ == type.Hoofdstuk)
                    {
                        Hoofdstuk hoofdstuk = new Hoofdstuk();
                        hoofdstuk.textBox1.Text = InfoPagina.PaginaMetRegels[i].tekst_;
                        DialogResult save = hoofdstuk.ShowDialog();
                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(hoofdstuk.textBox1.Text, type.Hoofdstuk, "");
                            change_pagina = true;
                            InfoPagina.PaginaMetRegels.RemoveAt(i);
                            InfoPagina.PaginaMetRegels.Insert(i, regel);

                        }
                        // bouw Pagina
                        BouwPaginaOp();
                    }
                    if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkDir)
                    {
                        LinkDir linkdir = new LinkDir();
                        linkdir.textBoxLinkText.Text = InfoPagina.PaginaMetRegels[i].tekst_;
                        linkdir.textBoxDir.Text = InfoPagina.PaginaMetRegels[i].url_;
                        DialogResult save = linkdir.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(linkdir.textBoxLinkText.Text, type.LinkDir, linkdir.textBoxDir.Text);
                            change_pagina = true;
                            InfoPagina.PaginaMetRegels.RemoveAt(i);
                            InfoPagina.PaginaMetRegels.Insert(i, regel);
                        }
                        // bouw Pagina
                        BouwPaginaOp();
                    }
                    if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkFile)
                    {
                        LinkFile linkfile = new LinkFile();
                        linkfile.textBox2.Text = InfoPagina.PaginaMetRegels[i].tekst_;
                        linkfile.textBox1.Text = InfoPagina.PaginaMetRegels[i].url_;
                        DialogResult save = linkfile.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(linkfile.textBox2.Text, type.LinkFile, linkfile.textBox1.Text);
                            change_pagina = true;
                            InfoPagina.PaginaMetRegels.RemoveAt(i);
                            InfoPagina.PaginaMetRegels.Insert(i, regel);
                        }
                        // bouw Pagina
                        BouwPaginaOp();
                    }
                    if (InfoPagina.PaginaMetRegels[i].type_ == type.TekstBlok)
                    {
                        TekstBlok tb = new TekstBlok();
                        tb.textBoxTextBlok.Text = InfoPagina.PaginaMetRegels[i].tekst_;
                        DialogResult save = tb.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(tb.textBoxTextBlok.Text, type.TekstBlok, "");
                            change_pagina = true;
                            InfoPagina.PaginaMetRegels.RemoveAt(i);
                            InfoPagina.PaginaMetRegels.Insert(i, regel);
                        }
                        // bouw Pagina
                        BouwPaginaOp();
                    }
                    if (InfoPagina.PaginaMetRegels[i].type_ == type.PaginaNaam)
                    {
                        Pagina pa = new Pagina();
                        pa.textBoxPaginaNaam.Text = InfoPagina.PaginaMetRegels[i].tekst_;
                        DialogResult save = pa.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            string oudenaam = InfoPagina.PaginaMetRegels[i].tekst_;
                            //string linkregel = "#" + pa.textBoxPaginaNaam.Text;
                            Regel regel = new Regel(pa.textBoxPaginaNaam.Text, type.PaginaNaam, "");
                            change_pagina = true;
                            InfoPagina.PaginaMetRegels.RemoveAt(i);
                            InfoPagina.PaginaMetRegels.Insert(i, regel);
                            // zoek nu naam.xml op en zet om in nieuwe naam.

                            System.IO.File.Move($"Data\\{oudenaam}.xml", $"Data\\{pa.textBoxPaginaNaam.Text}.xml");
                        }
                        // bouw Pagina
                        BouwPaginaOp();
                    }
                }
            }
        }

        private void versieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab = new About();
            _ = ab.ShowDialog();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // importeer oude wiki data
            TekstBlok tekstblok = new TekstBlok();
            DialogResult save = tekstblok.ShowDialog();
            if (save == DialogResult.OK)
            {
                int aantal_regels = tekstblok.textBoxTextBlok.Lines.Length;
                for (int i = 0; i < aantal_regels; i++)
                {
                    string regel = tekstblok.textBoxTextBlok.Lines[i];

                    // als tabel '|' maak er losse regels van
                    //int pos = regel.IndexOf("|");
                    //if (pos > -1)
                    //{
                    //    string[] opgedeeld = regel.Split('|');
                    //    for (int j = 0; j < opgedeeld.Length;j++)
                    //        ImporteerRegel(opgedeeld[j]);
                    //}
                    //else
                    //{
                    ImporteerRegel(regel);
                    //}

                }
            }
            BouwPaginaOp();
        }

        private void ImporteerRegel(string regel)
        {
            bool gevonden = false;
            regel = regel.Trim();

            // inspringen verwijderen
            if (regel.Length > 0 && regel[0] == '*')
            {
                regel = regel.Substring(1);
            }

            regel = regel.Trim();

            // lege regel
            if (regel.Length == 0)
            {
                Toevoegen(" ", type.Leeg, "");
                gevonden = true;
            }

            // hoofdstuk
            if (!gevonden && regel.Length > 5 && regel.Substring(0, 4) == "====")
            {
                regel = regel.Substring(5);
                int pos = regel.IndexOf('=');
                regel = regel.Substring(0, pos);
                Toevoegen(regel, type.Hoofdstuk, "");
                gevonden = true;
            }
            // tekst
            if (!gevonden && regel.Length > 5 && regel.Substring(0, 4) == "''''")
            {
                regel = regel.Substring(5);
                int pos = regel.IndexOf('\'');
                regel = regel.Substring(0, pos);
                Toevoegen(regel, type.TekstBlok, "");
                gevonden = true;
            }
            // link bv
            // [[file://Q:\Productie\OSF2\09_Ondersteuning\05_TechnischTeam\06_Documentatie\99_Overigen\Passwoorden.txt|Passwoorden]]
            if (!gevonden && regel.Length > 9 && regel.Substring(0, 9) == "[[file://")
            {
                regel = regel.Substring(9);
                int pos = regel.IndexOf('|');
                string url = regel.Substring(0, pos);
                regel = regel.Substring(pos + 1);
                pos = regel.IndexOf(']');
                regel = regel.Substring(0, pos);
                Toevoegen(regel, type.LinkFile, url);
                gevonden = true;
            }
            if (!gevonden && regel.Length > 9 && regel.Substring(0, 9) == "[[http://")
            {
                regel = regel.Substring(9);
                int pos = regel.IndexOf('|');
                string url = regel.Substring(0, pos);
                regel = regel.Substring(pos + 1);
                pos = regel.IndexOf(']');
                regel = regel.Substring(0, pos);
                Toevoegen(regel, type.LinkFile, url);
                gevonden = true;
            }
            // pagina
            // [[Inlog gegevens PC's Roza's]]
            if (!gevonden && regel.Length > 5 && regel.Substring(0, 2) == "[[")
            {
                regel = regel.Substring(2);
                int pos = regel.IndexOf(']');
                regel = regel.Substring(0, pos);
                Toevoegen(regel, type.PaginaNaam, "");
                gevonden = true;
            }

            if (!gevonden)
            {
                Toevoegen(regel, type.TekstBlok, "");
            }

        }

        private void LinkHover(object sender, EventArgs e)
        {
            _ = new LinkLabel();
            LinkLabel a = sender as LinkLabel;
            labelInfo.Text = a.Tag.ToString();
        }

        private void allePaginasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();

            InfoPagina.PaginaMetRegels.Clear();

            foreach (FileInfo file in files)
            {
                Regel regel = new Regel(Path.GetFileNameWithoutExtension(file.Name), type.PaginaNaam, "");
                InfoPagina.PaginaMetRegels.Add(regel);
            }
            BouwPaginaOp();

        }

        private void zoekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoekForm ZF = new ZoekForm();
            DialogResult save = ZF.ShowDialog();
            PaginaMetRegelsGevonden.Clear();

            if (save == DialogResult.OK)
            {
                List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();




                foreach (FileInfo file in files)
                {
                    _ = InfoPagina.Laad(Path.GetFileNameWithoutExtension(file.Name));
                    if (ContainsCaseInsensitive(file.Name, ZF.textBoxZoek.Text))
                    {
                        Regel regel = new Regel(Path.GetFileNameWithoutExtension(file.Name), type.PaginaNaam, "");
                        PaginaMetRegelsGevonden.Add(regel);
                    }
                    for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
                    {
                        if (ContainsCaseInsensitive(InfoPagina.PaginaMetRegels[i].tekst_, ZF.textBoxZoek.Text)/* || ContainsCaseInsensitive(InfoPagina.PaginaMetRegels[i].url_, ZF.textBoxZoek.Text)*/)
                        {
                            Regel regel = new Regel(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].type_, InfoPagina.PaginaMetRegels[i].url_);
                            PaginaMetRegelsGevonden.Add(regel);
                        }
                    }
                }
                // bouw Pagina
                InfoPagina.PaginaMetRegels = PaginaMetRegelsGevonden;
                BouwPaginaOp();
            }
        }
        public bool ContainsCaseInsensitive(string source, string substring)
        {
            return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
        }

        private void HistoryBalkUpdate(string pagina)
        {
            for (int i = 0; i < history.Count; i++)
            {
                if (pagina == history[i])
                {
                    return;
                }
            }

            history.Add(pagina);
            Point pos = new Point();
            if (history.Count > 5)
            {
                history.RemoveAt(0);
            }
            if (history.Count > 0 && history[0] != null)
            {
                linkLabelHis0.Text = history[0].ToString();
            }
            if (history.Count > 1 && history[1] != null)
            {
                pos.X = linkLabelHis0.Location.X + linkLabelHis0.Width + 10;
                pos.Y = linkLabelHis0.Location.Y;
                linkLabelHis1.Location = pos;
                linkLabelHis1.Text = history[1].ToString();
            }
            if (history.Count > 2 && history[2] != null)
            {
                pos.X = linkLabelHis1.Location.X + linkLabelHis1.Width + 10;
                pos.Y = linkLabelHis1.Location.Y;
                linkLabelHis2.Location = pos;
                linkLabelHis2.Text = history[2].ToString();
            }
            if (history.Count > 3 && history[3] != null)
            {
                pos.X = linkLabelHis2.Location.X + linkLabelHis2.Width + 10;
                pos.Y = linkLabelHis2.Location.Y;
                linkLabelHis3.Location = pos;
                linkLabelHis3.Text = history[3].ToString();
            }
            if (history.Count > 4 && history[4] != null)
            {
                pos.X = linkLabelHis3.Location.X + linkLabelHis3.Width + 10;
                pos.Y = linkLabelHis3.Location.Y;
                linkLabelHis4.Location = pos;
                linkLabelHis4.Text = history[4].ToString();
            }

        }

        private void linkLabelHis1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel a = (LinkLabel)sender;
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);

            // laad Start.xml
            if (InfoPagina.Laad(a.Text))
            {
                labelPaginaInBeeld.Text = a.Text;
                HistoryBalkUpdate(labelPaginaInBeeld.Text);
            }

            // bouw Pagina
            BouwPaginaOp();

        }
    }
}