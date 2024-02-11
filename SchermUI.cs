using Melding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
                change_pagina = true;
                AddLinkLijst(type.LinkDir, linkdir.textBoxLinkText.Text, linkdir.textBoxDir.Text);
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
                change_pagina = true;
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
                change_pagina = true;
                AddLinkLijst(type.LinkFile, linkFile.textBox2.Text, linkFile.textBox1.Text);
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
                PaginaInhoud.Save(labelPaginaInBeeld.Text);
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
                change_pagina = true;
                AddLinkLijst(type.PaginaNaam, pagina.textBoxPaginaNaam.Text, "");
            }
            // bouw Pagina
            SchermUpdate();
            SelecteerLaatstePaneel();
        }

        // hulp roetines
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
                            //ID_ = MaakID(),
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
            _ = panelMain.Height / PaginaInhoud.InhoudPaginaMetRegels.Count;
            panelMain.AutoScrollPosition = new Point(0, 50000);
        }
        private void SelecteerEerstePaneel()
        {
            buttonEditSelectie.Enabled = false;
            if (PaginaInhoud.InhoudPaginaMetRegels.Count > 0)
            {
                KleurGeselecteerdePanel(PaginaInhoud.InhoudPaginaMetRegels[0].eigenaar_);
            }

            panelMain.AutoScrollPosition = new Point(0, 0);
        }
        private void MovePanel(int oud, int nieuw)
        {
            if (!editModeAanToolStripMenuItem.Checked)
            {
                return;
            }

            Regel rg = new Regel
            {
                tekst_ = PaginaInhoud.InhoudPaginaMetRegels[oud].tekst_,
                url_ = PaginaInhoud.InhoudPaginaMetRegels[oud].url_,
                type_ = PaginaInhoud.InhoudPaginaMetRegels[oud].type_,
                undo_ = type.Move,
                index_ = oud,
                eigenaar_ = nieuw
            };

            Regel gekozen = PaginaInhoud.InhoudPaginaMetRegels[oud];
            PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(oud);
            PaginaInhoud.InhoudPaginaMetRegels.Insert(nieuw, gekozen);

            PaginaInhoud.ChangePagina.Add(rg);

            change_pagina = true;

            if (BlokSchrijf)
            {
                return;
            }

            // bouw Pagina
            SchermUpdate();

            int eig = PaginaInhoud.InhoudPaginaMetRegels[nieuw].eigenaar_;
            KleurGeselecteerdePanel(eig);

            panelMain.AutoScrollPosition = new Point(0, nieuw * 20);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public bool ContainsCaseInsensitive(string source, string substring)
        {
            return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
        }
        private void HistoryBalkAdd(string pagina)
        {
            if (pagina == "start")
            {
                flowHistorie.Controls.Clear();
            }
            LinkLabel a = new LinkLabel
            {
                AutoSize = true,
                Tag = pagina,
                Text = pagina,
                Font = new Font("Microsoft Sans Serif", 11)
            };
            a.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(Historylabel_LinkClicked);
            flowHistorie.Controls.Add(a);

            KennisMainForm_Resize(this, null);
        }
        public static int MaakID()
        {
            string dum = RandomString(10);
            return dum.GetHashCode();
        }
        public bool TestKlik()
        {
            if (mainForm.GekozenItem.Text == "000000")
            {
                SelectHelpForm sh = new SelectHelpForm();
                _ = sh.ShowDialog();
                return false;
            }
            return true;
        }
        private int GetIndexVanId(int Id)
        {
            for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
            {
                if (PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ == Id)
                {
                    return i;
                }
            }
            return -1;
        }
        private static KennisPanel GetKennisPanel(int ID)
        {
            foreach (Panel a in mainForm.Controls.OfType<Panel>()) // zij en hoofd paneel
            {
                foreach (KennisPanel item in a.Controls.OfType<KennisPanel>())
                {
                    if (item.kId == ID)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        // backup
        public void BackupNu()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                if (!File.Exists("Data\\backup.time"))
                {
                    Backup();
                    ZetBackupDatumInFile();
                }
                else // hier kijken of er een nieuwe dag is
                {
                    List<string> laatste_dag = File.ReadAllLines("Data\\backup.time").ToList();

                    if (laatste_dag.Count < 1)
                    {
                        ZetBackupDatumInFile();
                    }

                    if (!int.TryParse(laatste_dag[0], out int ILaatsteDag))  // als niet bekend waarneer backup gedaan is, backup
                    {
                        if (ILaatsteDag != DateTime.Now.Day)    // als vandaag al backup, niet nog een keer
                        {
                            ZetBackupDatumInFile();
                            Backup();
                            BoomKennisDataToolStripMenuItem_Click(this, null);
                            MaakLinkLijst(this, null);

                            FormMelding md = new FormMelding(FormMelding.Type.Klaar, "KennisBank", "Klaar backup.");
                            md.Show();
                        }
                    }
                }
            }
        }

        private static void ZetBackupDatumInFile()
        {
            List<string> laatste_dag = new List<string>
            {
                DateTime.Now.Day.ToString()
            };
            File.WriteAllLines("Data\\backup.time", laatste_dag);
        }

        private void Backup()
        {
            FormMelding md = new FormMelding(FormMelding.Type.Err, "KennisBank", "Backup maken..");
            md.Refresh();
            md.Show();

            // lijst met all pagina's opgeslagen.
            List<FileInfo> XMLFilesInDataDir = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                        .OrderByDescending(f => f.Name)
                        .ToList();
            ProgressBarAan(XMLFilesInDataDir.Count);

            foreach (FileInfo file in XMLFilesInDataDir)
            {
                string baknaam = Path.ChangeExtension(file.FullName, ".bak");

                if (!File.Exists(baknaam))
                {
                    File.Copy(file.FullName, baknaam, true);
                }
                else
                {
                    FileInfo fi = new FileInfo(baknaam);
                    if (fi.Length != file.Length)
                    {
                        File.Copy(file.FullName, baknaam, true);
                    }
                }
                ProgressBarUpdate();
            }
            ProgressBarUit();
        }

        // undo
        private void Undo_Click(object sender, EventArgs e)
        {
            if (PaginaInhoud.ChangePagina.Count < 1)
            {
                return;
            }
            // get laatste actie
            Regel undo = PaginaInhoud.ChangePagina[PaginaInhoud.ChangePagina.Count - 1];
            // verwijder deze uit lijst
            PaginaInhoud.ChangePagina.RemoveAt(PaginaInhoud.ChangePagina.Count - 1);
            change_pagina = true;

            // voor contra actie uit
            if (undo.undo_ == type.Delete)
            {
                // dus toevoegen
                PaginaInhoud.InhoudPaginaMetRegels.Insert(undo.index_, undo);
            }

            if (undo.undo_ == type.Move)
            {
                PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(undo.eigenaar_);
                PaginaInhoud.InhoudPaginaMetRegels.Insert(undo.index_, undo);
            }

            if (undo.undo_ == type.Toevoegen)
            {
                // dus verwijderen
                for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
                {
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].ID_ == undo.ID_)
                    {
                        PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                    }
                }
            }
            // bouw Pagina
            SchermUpdate();
            change_pagina = true;
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
            Application.DoEvents();
            progressBar.PerformStep();
            progressBar.Refresh();
            _ = new System.Threading.ManualResetEvent(false).WaitOne(1);

        }
    }
}
