using System.Drawing;
using System.Windows.Forms;
using System;

namespace KenisBank
{
    partial class KennisMainForm
    {
        // plaats regel op formulier
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

            but.Width = 500;
            but.Height = 30;
            but.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            but.Text = link;
            //but.Tag = locatie;
            //but.BorderStyle = BorderStyle.None;
            but.Click += new EventHandler(PaginaButtonClick);
            panel.Controls.Add(but);
            panelMain.Refresh();
        }
        
        // regel history regel
        private void PlaatsHistoryOpScherm()
        {
            linkLabelHis0.Text = linkLabelHis1.Text = linkLabelHis2.Text = linkLabelHis3.Text = linkLabelHis4.Text = "";
            Point pos = new Point();
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

        // toevoegen regel 
        public void Toevoegen(string text, type type, string url)
        {
            Regel regel = new Regel(text, type, url);
            InfoPagina.PaginaMetRegels.Add(regel);
            change_pagina = true;
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
            SchermUpdate();
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
            SchermUpdate();
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
            SchermUpdate();
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
            SchermUpdate();
            SelecteerLaatstePaneel();
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
            SchermUpdate();
            SelecteerLaatstePaneel();
        }

        // hulp roetines voor scherm dingen
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // bouw Pagina
            SchermUpdate();
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
                SchermUpdate();
            }
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
            SchermUpdate();

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
    }
}
