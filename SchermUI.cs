using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class KennisMainForm
    {
        // toevoegen regel 
        public void Toevoegen(string text, type type, string url)
        {
            text = text.Trim();
            Regel regel = new Regel(text, type, url)
            {
                ID_ = MaakID()
            };
            PaginaInhoud.InhoudPaginaMetRegels.Add(regel);

            Regel rg = new Regel(text, type, url)
            {
                ID_ = regel.ID_,
                undo_ = type.Toevoegen
            };
            PaginaInhoud.ChangePagina.Add(rg);

            change_pagina = true;
        }
        private void ToevoegenLinkNaarDirToolStripMenuItem_Click(object sender, EventArgs e)
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
        public void ToevoegenHoofdstukTextToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void ToevoegenLinkNaarFileToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void ToevoegenTekstBlokToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void ToevoegenLinkNaarNieuwePaginaToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // bouw Pagina
            SchermUpdate();
            _ = PaginaZijBalk.Laad("zijbalk");
            SchermUpdateZijBalk();
        }
        private void DeleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            int eigenaar = int.Parse(GekozenItem.Text);

            for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
            {
                if (PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ == eigenaar)
                {
                    //als wees pagina gewoon xml verwijderen
                    if (labelPaginaInBeeld.Text.Length > 3 && labelPaginaInBeeld.Text.Substring(0, 4) == "Wees")
                    {
                        string file_naam = $"Data\\{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}.xml";
                        File.Delete(file_naam);
                        PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                        change_pagina = false;
                    }
                    else
                    {
                        Regel rg = new Regel
                        {
                            ID_ = MaakID(),
                            tekst_ = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_,
                            url_ = PaginaInhoud.InhoudPaginaMetRegels[i].url_,
                            type_ = PaginaInhoud.InhoudPaginaMetRegels[i].type_,
                            undo_ = type.Delete,
                            index_ = i
                        };
                        PaginaInhoud.ChangePagina.Add(rg);
                        PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                        change_pagina = true;
                    }
                }
            }

            GekozenItem.Text = "000000";// panelGeselecteerd = null;

            foreach (Panel a in panelMain.Controls)
            {
                a.BackColor = panelMain.BackColor;
                a.BorderStyle = BorderStyle.None;
            }

            // bouw Pagina
            SchermUpdate();
        }
        private void SelecteerLaatstePaneel()
        {
            KleurGeselecteerdePanel(PaginaInhoud.InhoudPaginaMetRegels[PaginaInhoud.InhoudPaginaMetRegels.Count - 1].eigenaar_);
            int beneden = panelMain.Height / PaginaInhoud.InhoudPaginaMetRegels.Count;
            panelMain.AutoScrollPosition = new Point(0, 50000);
        }
        private void SelecteerEerstePaneel()
        {
            buttonEditSelectie.Enabled = false;
            if(PaginaInhoud.InhoudPaginaMetRegels.Count > 0)
                KleurGeselecteerdePanel(PaginaInhoud.InhoudPaginaMetRegels[0].eigenaar_);
            panelMain.AutoScrollPosition = new Point(0, 0);
        }
        private void MovePanel(int richting)
        {
            if (!editModeAanToolStripMenuItem.Checked)
            {
                return;
            }

            int eigenaar = int.Parse(GekozenItem.Text);
            int nieuw_index = -1;

            for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
            {
                if (PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ == eigenaar)
                {
                    nieuw_index = i + richting;

                    if (nieuw_index < 0 || nieuw_index + 1 > PaginaInhoud.InhoudPaginaMetRegels.Count)
                    {
                        return;
                    }

                    Regel rg = new Regel
                    {
                        ID_ = MaakID(),
                        tekst_ = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_,
                        url_ = PaginaInhoud.InhoudPaginaMetRegels[i].url_,
                        type_ = PaginaInhoud.InhoudPaginaMetRegels[i].type_,
                        undo_ = type.Move,
                        index_ = nieuw_index,
                        eigenaar_ = richting
                    };

                    Regel gekozen = PaginaInhoud.InhoudPaginaMetRegels[i];
                    PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                    PaginaInhoud.InhoudPaginaMetRegels.Insert(nieuw_index, gekozen);
                    i = 1000;

                    PaginaInhoud.ChangePagina.Add(rg);

                    change_pagina = true;
                }
            }

            if (BlokSchrijf)
            {
                return;
            }

            // bouw Pagina
            SchermUpdate();

            int eig = PaginaInhoud.InhoudPaginaMetRegels[nieuw_index].eigenaar_;
            KleurGeselecteerdePanel(eig);

            panelMain.AutoScrollPosition = new Point(0, nieuw_index * 20);
        }

        // ProgressBar
        private void ProgressBarAan(int max)
        {
            if (max == 0)
            {
                max = 1;
            }

            progressBar.Maximum = max;
            progressBar.Visible = true;
            progressBar.Value = 1;
            progressBar.Step = 1;
        }
        private void ProgressBarUit()
        {
            progressBar.Value = 1;
            progressBar.Visible = false;
        }
        private void ProgressBarUpdate()
        {
            progressBar.PerformStep();
            progressBar.Refresh();
            _ = new System.Threading.ManualResetEvent(false).WaitOne(1);

        }


    }
}
