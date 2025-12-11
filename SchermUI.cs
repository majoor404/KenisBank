using Melding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class KennisMainForm
    {
        private static int idCounter = Environment.TickCount & 0x7FFFFFFF; // altijd positief getal

        // toevoegen regel 
        public void Toevoegen(string text, type type, string url)
        {
            text = text.Trim();
            RegelInXML regel = new RegelInXML(text, type, url)
            {
                ID_ = MaakID()
            };
            MainPagina.LijstMetRegels.Add(regel);

            RegelInXML rg = new RegelInXML(text, type, url)
            {
                ID_ = regel.ID_,
                undo_ = type.Toevoegen
            };
            MainPagina.LijstChangePaginaRegels.Add(rg);

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
                AddLinkLijst(labelPaginaInBeeld.Text, type.LinkDir, linkdir.textBoxLinkText.Text, linkdir.textBoxDir.Text);
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
                AddLinkLijst(labelPaginaInBeeld.Text, type.LinkFile, linkFile.textBox2.Text, linkFile.textBox1.Text);
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
                MainPagina.Save(labelPaginaInBeeld.Text);
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
                AddLinkLijst(labelPaginaInBeeld.Text, type.PaginaNaam, pagina.textBoxPaginaNaam.Text, "");
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

            for (int i = 0; i < MainPagina.LijstMetRegels.Count; i++)
            {
                if (MainPagina.LijstMetRegels[i].eigenaar_ == eigenaar)
                {
                    //als wees pagina gewoon xml verwijderen
                    if (labelPaginaInBeeld.Text.Length > 3 && labelPaginaInBeeld.Text.Substring(0, 4) == "Wees")
                    {
                        string file_naam = $"Data\\{MainPagina.LijstMetRegels[i].tekst_}.xml";
                        System.IO.File.Delete(file_naam);
                        MainPagina.LijstMetRegels.RemoveAt(i);
                        change_pagina = false;
                    }
                    else
                    {
                        RegelInXML rg = new RegelInXML
                        {
                            //ID_ = MaakID(),
                            tekst_ = MainPagina.LijstMetRegels[i].tekst_,
                            url_ = MainPagina.LijstMetRegels[i].url_,
                            type_ = MainPagina.LijstMetRegels[i].type_,
                            undo_ = type.Delete,
                            index_ = i
                        };
                        MainPagina.LijstChangePaginaRegels.Add(rg);
                        MainPagina.LijstMetRegels.RemoveAt(i);
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
            KleurGeselecteerdePanel(MainPagina.LijstMetRegels[MainPagina.LijstMetRegels.Count - 1].eigenaar_);
            _ = panelMain.Height / MainPagina.LijstMetRegels.Count;
            panelMain.AutoScrollPosition = new Point(0, 50000);
        }
        private void SelecteerEerstePaneel()
        {
            buttonEditSelectie.Enabled = false;
            if (MainPagina.LijstMetRegels.Count > 0)
            {
                KleurGeselecteerdePanel(MainPagina.LijstMetRegels[0].eigenaar_);
            }

            panelMain.AutoScrollPosition = new Point(0, 0);
        }
        public void MovePanel(int oud, int nieuw)
        {
            if (!editModeAanToolStripMenuItem.Checked)
            {
                return;
            }

            RegelInXML rg = new RegelInXML
            {
                tekst_ = MainPagina.LijstMetRegels[oud].tekst_,
                url_ = MainPagina.LijstMetRegels[oud].url_,
                type_ = MainPagina.LijstMetRegels[oud].type_,
                undo_ = type.Move,
                index_ = oud,
                eigenaar_ = nieuw
            };

            RegelInXML gekozen = MainPagina.LijstMetRegels[oud];
            MainPagina.LijstMetRegels.RemoveAt(oud);
            MainPagina.LijstMetRegels.Insert(nieuw, gekozen);

            MainPagina.LijstChangePaginaRegels.Add(rg);

            change_pagina = true;

            if (BlokSchrijf)
            {
                return;
            }

            // bouw Pagina
            SchermUpdate();

            int eig = MainPagina.LijstMetRegels[nieuw].eigenaar_;
            KleurGeselecteerdePanel(eig);

            panelMain.AutoScrollPosition = new Point(0, nieuw * 20);
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
            return Interlocked.Increment(ref idCounter);
            //return Guid.NewGuid().GetHashCode();
            //string dum = RandomString(10);
            //return dum.GetHashCode();
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
        public int GetIndexVanId(int Id)
        {
            for (int i = 0; i < MainPagina.LijstMetRegels.Count; i++)
            {
                if (MainPagina.LijstMetRegels[i].eigenaar_ == Id)
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
                if (!System.IO.File.Exists("Data\\backup.time"))
                {
                    Backup();
                    ZetBackupDatumInFile();
                }
                else // hier kijken of er een nieuwe dag is
                {
                    List<string> laatste_dag = System.IO.File.ReadAllLines("Data\\backup.time").ToList();

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
                            //BoomKennisDataToolStripMenuItem_Click(this, null);
                            MaakIndex(this, null);
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
            System.IO.File.WriteAllLines("Data\\backup.time", laatste_dag);
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

            using (ProgressManager progress = new ProgressManager(this, "Backup Pagina's die veranderd zijn.", XMLFilesInDataDir.Count))
            {
                foreach (FileInfo file in XMLFilesInDataDir)
                {
                    string s = DateTime.Now.ToString("MM-dd-yyyy HH-mm");
                    string BackupNaam = Directory.GetCurrentDirectory() + $"\\Backup\\{file.Name} {s}";

                    BackUpFile(file.FullName, BackupNaam);

                    progress.Update();
                }
            }
        }

        private void BackUpFile(string Filename, string BackupNaam)
        {
            string AllenFileNaam = Path.GetFileName(Filename);
            string PathVanBackup = Path.GetDirectoryName(BackupNaam);
            string laatsteBackup = BackupNaam;

            try
            {
                // Ensure the backup directory exists
                if (string.IsNullOrEmpty(PathVanBackup))
                {
                    PathVanBackup = Directory.GetCurrentDirectory();
                }

                if (!Directory.Exists(PathVanBackup))
                {
                    try
                    {
                        Directory.CreateDirectory(PathVanBackup);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex, $"Failed to create backup directory: {PathVanBackup}");
                        // fallback to current directory
                        PathVanBackup = Directory.GetCurrentDirectory();
                    }
                }

                string zoek = $"{AllenFileNaam}*";
                // verwijder oude backups
                List<FileInfo> files = new DirectoryInfo(PathVanBackup)
                        .EnumerateFiles(zoek)
                        //.Skip(3)  // als aantal < 3 wordt lijst dus leeg
                        .OrderByDescending(f => f.CreationTime)
                        .ToList();

                if (files.Count < 5)
                {
                    System.IO.File.Copy(Filename, BackupNaam, true);
                }
                else
                {
                    laatsteBackup = files[0].FullName; // bovenste is laatste
                    files = files.Skip(5).ToList(); // bewaar er 5
                    files.ForEach(f => f.Delete()); // delete andere
                    DateTime fl = System.IO.File.GetLastWriteTime(laatsteBackup);
                    DateTime fn = System.IO.File.GetLastWriteTime(Filename);
                    if (fl != fn)
                    {
                        System.IO.File.Copy(Filename, BackupNaam, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex, $"BackUpFile failed for {Filename}");
            }
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            if (MainPagina.LijstChangePaginaRegels.Count < 1)
            {
                return;
            }
            // get laatste actie
            RegelInXML undo = MainPagina.LijstChangePaginaRegels[MainPagina.LijstChangePaginaRegels.Count - 1];
            // verwijder deze uit lijst
            MainPagina.LijstChangePaginaRegels.RemoveAt(MainPagina.LijstChangePaginaRegels.Count - 1);
            change_pagina = true;

            // voor contra actie uit
            if (undo.undo_ == type.Delete)
            {
                // dus toevoegen
                MainPagina.LijstMetRegels.Insert(undo.index_, undo);
            }

            if (undo.undo_ == type.Move)
            {
                MainPagina.LijstMetRegels.RemoveAt(undo.eigenaar_);
                MainPagina.LijstMetRegels.Insert(undo.index_, undo);
            }

            if (undo.undo_ == type.Toevoegen)
            {
                // dus verwijderen
                for (int i = 0; i < MainPagina.LijstMetRegels.Count; i++)
                {
                    if (MainPagina.LijstMetRegels[i].ID_ == undo.ID_)
                    {
                        MainPagina.LijstMetRegels.RemoveAt(i);
                    }
                }
            }
            // bouw Pagina
            SchermUpdate();
            change_pagina = true;
        }
    }
}
