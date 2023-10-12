using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

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
        public Regel PaginaInhoud = new Regel();
        public Panel panelGeselecteerd = new Panel();
        public bool change_pagina = false;
        private static readonly Random random = new Random();
        private readonly List<Regel> PaginaMetRegelsGevonden = new List<Regel>();
        private readonly List<string> history = new List<string>();
        public string PrevPagina = string.Empty;
        public bool BlokSchrijf = false;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool LockWindowUpdate(IntPtr hWndLock);


        public KennisMainForm()
        {
            InitializeComponent();
        }
        private void KennisMainForm_Shown(object sender, EventArgs e)
        {
            // zet panelen netjes
            KennisMainForm_Resize(this, null);
            // laad Start.xml
            if (PaginaInhoud.Laad("start"))
            {
                labelPaginaInBeeld.Text = "start";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
            }
            else
            {
                // start bestaan niet, maak lege
                labelPaginaInBeeld.Text = "start";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
                saveHuidigePaginaToolStripMenuItem_Click(this, null);
            }

            BackupNu();

            // bouw Pagina
            SchermUpdate();
        }
        private void KennisMainForm_Resize(object sender, EventArgs e)
        {
            panel1.Width = Width - 45;
            panelMain.Width = Width - 45;
            panelMain.Height = Height - 180;
        }


        // interact met gebruiker
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            // ook in alle paginaas en zoek, wees , alleen een beheerder kan hier bij komen.
            if ((labelPaginaInBeeld.Text.Length > 3) && (labelPaginaInBeeld.Text.Substring(0, 4) == "Zoek"/* || labelPaginaInBeeld.Text == "Alle paginas" || labelPaginaInBeeld.Text.Substring(0, 4) == "Wees"*/))
            {
                editModeAanToolStripMenuItem.Checked = false;
                return;
            }

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
                buttonEditSelectie.Enabled = false;
                editPaginaToolStripMenuItem.BackColor = SystemColors.MenuBar;
                panelUpDown.Visible = false;

                foreach (Panel a in panelMain.Controls)
                {
                    a.BackColor = panelMain.BackColor;
                    a.BorderStyle = BorderStyle.None;
                }
            }
        }
        private void label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (editModeAanToolStripMenuItem.Checked)
            {
                return;
            }

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
            if (editModeAanToolStripMenuItem.Checked)
            {
                return;
            }

            PrevPagina = labelPaginaInBeeld.Text;
            Button but = (Button)sender;
            string pagina = but.Text.Trim();

            //pagina worden opgeslagen in lower case Enabled elke spatie vervangen door _
            pagina = PaginaInhoud.RemoveOudeWikiTekens(pagina);

            if (!PaginaInhoud.Laad(pagina))
            {
                // dus nieuwe pagina
                PaginaInhoud.InhoudPaginaMetRegels.Clear();
                labelPaginaInBeeld.Text = pagina;
                HistoryBalkAdd(labelPaginaInBeeld.Text);
                SchermUpdate();
            }
            else
            {
                labelPaginaInBeeld.Text = pagina;
                HistoryBalkAdd(labelPaginaInBeeld.Text);
                // bouw Pagina
                SchermUpdate();
            }
        }
        private void panel_Click(object sender, EventArgs e)
        {
            panelGeselecteerd = null;
            buttonEditSelectie.Enabled = false;
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
                        buttonEditSelectie.Enabled = true;
                    }
                }
            }
        }
        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);

            // laad Start.xml
            if (PaginaInhoud.Laad("start"))
            {
                labelPaginaInBeeld.Text = "start";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
            }

            // bouw Pagina
            SchermUpdate();
        }
        private void saveHuidigePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (labelPaginaInBeeld.Text.Length > 3 && labelPaginaInBeeld.Text.Substring(0, 4) == "Wees")
            {
                change_pagina = false;
                return;
            }

            PaginaInhoud.Save(labelPaginaInBeeld.Text);
            change_pagina = false;
        }
        private void toevoegenLegeRegelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Toevoegen(" ", type.Leeg, "");
            // bouw Pagina
            SchermUpdate();
            // meteen selecteren
            SelecteerLaatstePaneel();
        }
        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
                return;
            MovePanel(-1);
        }
        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
                return;
            MovePanel(1);
        }
        private void buttonMoveUp5_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
                return;
            BlokSchrijf = true;
            MovePanel(-1);
            MovePanel(-1);
            MovePanel(-1);
            MovePanel(-1);
            BlokSchrijf = false;
            MovePanel(-1);
        }
        private void buttonMoveDown5_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
                return;
            BlokSchrijf = true;
            MovePanel(1);
            MovePanel(1);
            MovePanel(1);
            MovePanel(1);
            BlokSchrijf = false;
            MovePanel(1);
        }
        private void buttonSaveCloseEdit_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);
            if (change_pagina)
            {
                DialogResult dialogResult = MessageBox.Show($"Pagina is aangepast, eerst saven ?", "Vraagje", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveHuidigePaginaToolStripMenuItem_Click(this, null);
                }
            }
            SchermUpdate();
        }
        private void buttonEditSelectie_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
                return;
            
            int eigenaar = (int)panelGeselecteerd.Tag;

            for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
            {
                if (PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ == eigenaar)
                {
                    // oke panel en regel nu bekend.
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.Hoofdstuk)
                    {
                        Hoofdstuk hoofdstuk = new Hoofdstuk();
                        hoofdstuk.textBox1.Text = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                        DialogResult save = hoofdstuk.ShowDialog();
                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(hoofdstuk.textBox1.Text, type.Hoofdstuk, "")
                            {
                                ID_ = MaakID()
                            };
                            change_pagina = true;
                            PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                            PaginaInhoud.InhoudPaginaMetRegels.Insert(i, regel);

                        }
                        // bouw Pagina
                        SchermUpdate();
                    }
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkDir)
                    {
                        LinkDir linkdir = new LinkDir();
                        linkdir.textBoxLinkText.Text = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                        linkdir.textBoxDir.Text = PaginaInhoud.InhoudPaginaMetRegels[i].url_;
                        DialogResult save = linkdir.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(linkdir.textBoxLinkText.Text, type.LinkDir, linkdir.textBoxDir.Text)
                            {
                                ID_ = MaakID()
                            };
                            change_pagina = true;
                            PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                            PaginaInhoud.InhoudPaginaMetRegels.Insert(i, regel);
                        }
                        // bouw Pagina
                        SchermUpdate();
                    }
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkFile)
                    {
                        LinkFile linkfile = new LinkFile();
                        linkfile.textBox2.Text = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                        linkfile.textBox1.Text = PaginaInhoud.InhoudPaginaMetRegels[i].url_;
                        DialogResult save = linkfile.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(linkfile.textBox2.Text, type.LinkFile, linkfile.textBox1.Text)
                            {
                                ID_ = MaakID()
                            };
                            change_pagina = true;
                            PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                            PaginaInhoud.InhoudPaginaMetRegels.Insert(i, regel);
                        }
                        // bouw Pagina
                        SchermUpdate();
                    }
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.TekstBlok)
                    {
                        TekstBlok tb = new TekstBlok();
                        tb.textBoxTextBlok.Text = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                        DialogResult save = tb.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            Regel regel = new Regel(tb.textBoxTextBlok.Text, type.TekstBlok, "")
                            {
                                ID_ = MaakID()
                            };
                            change_pagina = true;
                            PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                            PaginaInhoud.InhoudPaginaMetRegels.Insert(i, regel);
                        }
                        // bouw Pagina
                        SchermUpdate();
                    }
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.PaginaNaam)
                    {
                        Pagina pa = new Pagina();
                        pa.textBoxPaginaNaam.Text = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                        DialogResult save = pa.ShowDialog();

                        if (save == DialogResult.OK)
                        {
                            string oudenaam = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                            //string linkregel = "#" + pa.textBoxPaginaNaam.Text;
                            Regel regel = new Regel(pa.textBoxPaginaNaam.Text, type.PaginaNaam, "")
                            {
                                ID_ = MaakID()
                            };
                            change_pagina = true;
                            PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                            PaginaInhoud.InhoudPaginaMetRegels.Insert(i, regel);
                            // zoek nu naam.xml op en zet om in nieuwe naam.

                            oudenaam = PaginaInhoud.RemoveOudeWikiTekens(oudenaam);
                            System.IO.File.Move($"Data\\{oudenaam}.xml", $"Data\\{pa.textBoxPaginaNaam.Text}.xml");
                        }
                        // bouw Pagina
                        SchermUpdate();
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
            List<FileInfo> files = new DirectoryInfo("pages").EnumerateFiles("*.txt")
                            .OrderBy(f => f.Name)
                            .ToList();

            foreach (FileInfo file in files)
            {
                PaginaInhoud.InhoudPaginaMetRegels.Clear();
                List<string> OudeWikiTekst = File.ReadAllLines($"pages\\{Path.GetFileNameWithoutExtension(file.Name)}.txt").ToList();

                int aantal_regels = OudeWikiTekst.Count();
                for (int i = 0; i < aantal_regels; i++)
                {
                    string regel = OudeWikiTekst[i].Trim();

                    // als tabel '|' maak er losse regels van
                    int pos = regel.IndexOf("|");
                    if (pos == 0)
                    {
                        string[] opgedeeld = regel.Split('|');
                        for (int j = 0; j < opgedeeld.Length; j++)
                        {
                            ImporteerRegel(opgedeeld[j], file.Name);
                        }
                    }
                    else
                    {
                        ImporteerRegel(regel, file.Name);
                    }

                }
                // save als 
                PaginaInhoud.ChangePagina.Clear();
                //string file_Naam = file.Name.Trim();
                PaginaInhoud.Save(Path.GetFileNameWithoutExtension(file.Name));
            }
            change_pagina = false;
            SchermUpdate();
            _ = MessageBox.Show("Klaar import");
        }
        private void allePaginasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();

            PaginaInhoud.InhoudPaginaMetRegels.Clear();
            ProgressBarAan(files.Count);

            foreach (FileInfo file in files)
            {
                ProgressBarUpdate();

                Regel regel = new Regel(Path.GetFileNameWithoutExtension(file.Name), type.PaginaNaam, "")
                {
                    ID_ = MaakID()
                };
                PaginaInhoud.InhoudPaginaMetRegels.Add(regel);

            }
            SchermUpdate();
            ProgressBarUit();
            labelPaginaInBeeld.Text = "Alle paginas";

        }
        private void vorigeVersiePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test of vorige versie bestaat
            string fi = labelPaginaInBeeld.Text;
            string opslagnaam = $"Data\\{fi}.xml";
            string backup1 = $"Data\\{fi}.bak";

            BackupTerug BT = new BackupTerug();

            BT.PaginaNaam.Text = fi;
            BT.labelHuidig.Text = File.GetLastWriteTime(opslagnaam).ToLongDateString();
            BT.labelBackup1.Text = File.GetLastWriteTime(backup1).ToLongDateString();

            BT.ShowDialog();

            PaginaInhoud.Laad(fi);
            SchermUpdate();
        }
        private void zoekNaarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoekForm ZF = new ZoekForm();
            DialogResult save = ZF.ShowDialog();
            PaginaMetRegelsGevonden.Clear();

            if (save == DialogResult.OK)
            {
                List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();

                ProgressBarAan(files.Count);

                foreach (FileInfo file in files)
                {
                    ProgressBarUpdate();

                    _ = PaginaInhoud.Laad(Path.GetFileNameWithoutExtension(file.Name));

                    for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
                    {
                        if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkFile || PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkDir)
                        {
                            if (ContainsCaseInsensitive(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, ZF.textBoxZoek.Text))
                            {
                                //Regel regel = new Regel($"Gevonden op pagina {file.Name}", type.TekstBlok, "");
                                //regel.ID_ = MaakID();
                                //PaginaMetRegelsGevonden.Add(regel);
                                Regel regel = new Regel(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, PaginaInhoud.InhoudPaginaMetRegels[i].type_, PaginaInhoud.InhoudPaginaMetRegels[i].url_)
                                {
                                    ID_ = MaakID()
                                };
                                PaginaMetRegelsGevonden.Add(regel);
                                regel = new Regel("", type.Leeg, "")
                                {
                                    ID_ = MaakID()
                                };
                                PaginaMetRegelsGevonden.Add(regel);
                            }
                        }
                    }

                }
                // bouw Pagina
                labelPaginaInBeeld.Text = $"Zoek : {ZF.textBoxZoek.Text}";
                PaginaInhoud.InhoudPaginaMetRegels = PaginaMetRegelsGevonden;
                SchermUpdate();
                ProgressBarUit();
            }
        }
        private void zoekNaarWeesPaginasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // lijst met all pagina's opgeslagen.
            List<FileInfo> XMLFilesInDataDir = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();

            ProgressBarAan(XMLFilesInDataDir.Count);
            // maak lijst met de linken naar pagina's
            List<string> LinkNaarPaginaLijst = new List<string>();
            foreach (FileInfo file in XMLFilesInDataDir)
            {
                ProgressBarUpdate();

                _ = PaginaInhoud.Laad(Path.GetFileNameWithoutExtension(file.Name));
                for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
                {
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.PaginaNaam)
                    {
                        string linkNaarFile = PaginaInhoud.RemoveOudeWikiTekens(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_);
                        if (!LinkNaarPaginaLijst.Contains(linkNaarFile))
                        {
                            LinkNaarPaginaLijst.Add(linkNaarFile);
                        }
                    }
                }

            }
            ProgressBarUit();
            PaginaInhoud.InhoudPaginaMetRegels.Clear();
            ProgressBarAan(XMLFilesInDataDir.Count);
            foreach (FileInfo file in XMLFilesInDataDir)
            {
                ProgressBarUpdate();
                // check of filenaam een item bevat wat geen file is
                string FileNaam = Path.GetFileNameWithoutExtension(file.Name);
                if (!LinkNaarPaginaLijst.Contains(FileNaam) && FileNaam != "start")
                {
                    Regel regel = new Regel(FileNaam, type.PaginaNaam, "")
                    {
                        ID_ = MaakID()
                    };
                    PaginaInhoud.InhoudPaginaMetRegels.Add(regel);
                }
            }
            labelPaginaInBeeld.Text = $"Wees Pagina's";
            SchermUpdate();
            ProgressBarUit();
        }
        private void terugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (history.Count > 1)
            {
                string vorige_pagina = history[history.Count - 2];
                _ = PaginaInhoud.Laad(vorige_pagina);
                labelPaginaInBeeld.Text = vorige_pagina;
                history.RemoveAt(history.Count - 1);
                PlaatsHistoryOpScherm();
                SchermUpdate();
            }
        }
        private void linkLabelHis1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel a = (LinkLabel)sender;
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);

            if (PaginaInhoud.Laad(a.Text))
            {
                labelPaginaInBeeld.Text = a.Text;
                HistoryBalkChange(labelPaginaInBeeld.Text);
                PlaatsHistoryOpScherm();
            }

            // bouw Pagina
            SchermUpdate();

        }
        private void zoekLinksDieNietMeerBestaanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Doorzoek alle paginas en verwijder links die niet meer bestaan?", "Vraagje", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();

                foreach (FileInfo file in files)
                {
                    _ = PaginaInhoud.Laad(Path.GetFileNameWithoutExtension(file.Name));
                    // verander
                    int change = PaginaInhoud.InhoudPaginaMetRegels.Count;
                    // door file heenstappen
                    for (int i = PaginaInhoud.InhoudPaginaMetRegels.Count - 1; i > 0; i--)
                    {
                        //bij import vanuit oude wiki is link naar dir gelijk aan link naar file.
                        // dus check of het geen file en geen dir is voordat ik verwijder

                        if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkFile)
                        {
                            if (!File.Exists(PaginaInhoud.InhoudPaginaMetRegels[i].url_))
                            {
                                if (!Directory.Exists(PaginaInhoud.InhoudPaginaMetRegels[i].url_))
                                {
                                    _ = MessageBox.Show($"Remove link naar file \n{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}\nOp Pagina {Path.GetFileNameWithoutExtension(file.Name)}\n{PaginaInhoud.InhoudPaginaMetRegels[i].url_}");
                                    PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                                }
                            }
                        }
                        else if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkDir)
                        {
                            if (!Directory.Exists(PaginaInhoud.InhoudPaginaMetRegels[i].url_))
                            {
                                _ = MessageBox.Show($"Remove link naar dir \n{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}\nOp Pagina {Path.GetFileNameWithoutExtension(file.Name)}");
                                PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                            }
                        }
                        else if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.PaginaNaam)
                        {
                            if (!File.Exists($"Data\\{Path.GetFileNameWithoutExtension(file.Name)}.xml"))
                            {
                                _ = MessageBox.Show($"Remove link naar Pagina \n{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}\nOp Pagina {Path.GetFileNameWithoutExtension(file.Name)}");
                                PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                            }
                        }
                    }
                    if (change != PaginaInhoud.InhoudPaginaMetRegels.Count)
                    {
                        PaginaInhoud.Save(Path.GetFileNameWithoutExtension(file.Name));
                    }
                }
                _ = MessageBox.Show("Klaar met clean kennisbank");
            }
        }

        // main scherm update roetine
        private void SchermUpdate()
        {
            // delete oude
            panelMain.Controls.Clear();
            //labelInfo.Text = "";
            int MakerInfoIndex = -1;
            ProgressBarAan(PaginaInhoud.InhoudPaginaMetRegels.Count);

            _ = LockWindowUpdate(panelMain.Handle);


            for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
            {
                //labelInfo.Text = zoekinfo.ToString();
                //labelInfo.Refresh();
                //zoekinfo--;
                ProgressBarUpdate();
                string dum = RandomString(10);
                int eigenaar = dum.GetHashCode();

                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.Hoofdstuk)
                {
                    PlaatsHoofdstukOpBeeld(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, eigenaar);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkDir)
                {
                    PlaatsLinkOpBeeld(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, PaginaInhoud.InhoudPaginaMetRegels[i].url_, eigenaar);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkFile)
                {
                    PlaatsLinkOpBeeld(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, PaginaInhoud.InhoudPaginaMetRegels[i].url_, eigenaar);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.TekstBlok)
                {
                    PlaatsTextOpBeeld(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, eigenaar);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.PaginaNaam)
                {
                    PlaatsPaginaOpBeeld(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, PaginaInhoud.InhoudPaginaMetRegels[i].url_, eigenaar);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.Leeg)
                {
                    PlaatsTextOpBeeld(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, eigenaar);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = eigenaar;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.EditInfo)
                {
                    // bewaar locatie van info, wordt als laatse geplaatst op scherm.
                    MakerInfoIndex = i;
                }
            }
            labelInfo.Text = "";
            if (MakerInfoIndex > -1)
            {
                string dum = RandomString(10);
                int eigenaar = dum.GetHashCode();
                PlaatsTextOpBeeld(PaginaInhoud.InhoudPaginaMetRegels[MakerInfoIndex].tekst_, eigenaar);
                PaginaInhoud.InhoudPaginaMetRegels[MakerInfoIndex].eigenaar_ = eigenaar;
            }

            _ = LockWindowUpdate(IntPtr.Zero);

            ProgressBarUit();
        }

        // hulp roetines
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void ImporteerRegel(string regel, string file_naam)
        {
            string error_string = regel;
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
                Toevoegen("", type.Leeg, "");
                gevonden = true;
            }

            // hoofdstuk
            if (!gevonden && regel.Length > 5 && regel.Substring(0, 4) == "====")
            {
                try
                {
                    regel = regel.Substring(5);
                    int pos = regel.IndexOf('=');
                    regel = regel.Substring(0, pos);
                    Toevoegen(regel, type.Hoofdstuk, "");
                    gevonden = true;
                }
                catch
                {
                    _ = MessageBox.Show($"Error in file {file_naam}\nRegel : {error_string}");
                }
            }
            // tekst
            if (!gevonden && regel.Length > 5 && regel.Substring(0, 4) == "''''")
            {
                try
                {
                    regel = regel.Substring(5);
                    int pos = regel.IndexOf('\'');
                    regel = regel.Substring(0, pos);
                    Toevoegen(regel, type.TekstBlok, "");
                    gevonden = true;
                }
                catch
                {
                    _ = MessageBox.Show($"Error in file {file_naam}\nRegel : {error_string}");
                }
            }
            // link bv
            // [[file://Q:\Productie\OSF2\09_Ondersteuning\05_TechnischTeam\06_Documentatie\99_Overigen\Passwoorden.txt|Passwoorden]]
            if (!gevonden && regel.Length > 9 && regel.Substring(0, 9) == "[[file://")
            {
                try
                {
                    string dummy = regel;
                    regel = regel.Substring(9);
                    int pos = regel.IndexOf('|');
                    string url = regel.Substring(0, pos);
                    regel = regel.Substring(pos + 1);
                    pos = regel.IndexOf(']');
                    regel = regel.Substring(0, pos);
                    Toevoegen(regel, type.LinkFile, url);
                    gevonden = true;
                }
                catch
                {
                    _ = MessageBox.Show($"Error in file {file_naam}\nRegel : {error_string}");
                }
            }
            if (!gevonden && regel.Length > 9 && regel.Substring(0, 9) == "[[http://")
            {
                try
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
                catch
                {
                    _ = MessageBox.Show($"Error in file {file_naam}\nRegel : {error_string}");
                }
            }
            // pagina
            // [[Inlog gegevens PC's Roza's]]
            if (!gevonden && regel.Length > 5 && regel.Substring(0, 2) == "[[")
            {
                try
                {
                    regel = regel.Substring(2);
                    int pos = regel.IndexOf(']');
                    regel = regel.Substring(0, pos);
                    Toevoegen(regel, type.PaginaNaam, "");
                    gevonden = true;
                }
                catch
                {
                    _ = MessageBox.Show($"Error in file {file_naam}\nRegel : {error_string}");
                }
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
        public bool ContainsCaseInsensitive(string source, string substring)
        {
            return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
        }
        private void HistoryBalkAdd(string pagina)
        {
            if (pagina == "start")
            {
                history.Clear();
            }

            history.Add(pagina);

            if (history.Count > 5)
            {
                history.RemoveAt(0);
            }

            PlaatsHistoryOpScherm();

        }
        private void HistoryBalkChange(string pagina)
        {
            int gevonden_pos = history.Count;
            for (int i = 0; i < 4; i++)
            {
                if (history[i] == pagina)
                {
                    gevonden_pos = i;
                    break;
                }
            }
            for (int i = history.Count - 1; i > gevonden_pos; i--)
            {
                history.RemoveAt(i);
            }
        }
        public int MaakID()
        {
            string dum = RandomString(10);
            return dum.GetHashCode();
        }
        public bool TestKlik()
        {
            if (panelGeselecteerd == null)
            {
                SelectHelpForm sh = new SelectHelpForm();
                sh.ShowDialog();
                return false;
            }
            return true;
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
            // nieuwe regel
            Regel rg = new Regel
            {
                ID_ = MaakID(),
                tekst_ = undo.tekst_,
                url_ = undo.url_,
                type_ = undo.type_
            };
            // voor contra actie uit
            if (undo.undo_ == type.Delete)
            {
                // dus toevoegen
                PaginaInhoud.InhoudPaginaMetRegels.Insert(undo.index_, rg);
            }

            if (undo.undo_ == type.Move)
            {
                PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(undo.index_);
                PaginaInhoud.InhoudPaginaMetRegels.Insert(undo.index_ - undo.eigenaar_, rg);
            }

            if (undo.undo_ == type.Toevoegen) // even error om na vakantie verder te gaan.
            {
                // dus verwijderen

                for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
                {
                    if (PaginaInhoud.InhoudPaginaMetRegels[i].ID_ == rg.ID_)
                    {
                        PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
                    }
                }
            }
            // bouw Pagina
            SchermUpdate();
        }
        public void BackupNu()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                if (!File.Exists("Data\\backup.time"))
                {
                    using (File.Create("Data\\backup.time"))
                    { };
                    Backup();
                }
                else // hier kijken of er een nieuwe dag is
                {
                    DateTime laatste_keer = File.GetLastWriteTime("Data\\backup.time");
                    if (laatste_keer.Day != DateTime.Now.Day && DateTime.Now.Hour > 1)
                    {
                        Backup();
                    }
                }
            }
        }
        private void Backup()
        {
            MessageBox.Show("Even Backup van pagina's");
            // lijst met all pagina's opgeslagen.
            List<FileInfo> XMLFilesInDataDir = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                        .OrderByDescending(f => f.Name)
                        .ToList();
            ProgressBarAan(XMLFilesInDataDir.Count);
            
            foreach (FileInfo file in XMLFilesInDataDir)
            {
                string baknaam = Path.ChangeExtension(file.FullName, ".bak");
                //FileInfo fi = new FileInfo(baknaam);
                //if (fi.Length != file.Length)
                File.Copy(file.FullName, baknaam, true);
                ProgressBarUpdate();
            }
            ProgressBarUit();
        }

        private void beheerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = "...";
            //Display the custom input dialog box with the following prompt, window title, and dimensions
            ShowInputDialogBox(ref input, "Wachtwoord", "Wachtwoord", 300, 200);

            if (input == "majoor404")
            {
                importAllePaginasOudeWikiToolStripMenuItem.Visible = true;
                paginaBackupTerugZettenToolStripMenuItem.Visible = true;
                zoekNaarWeesPaginasToolStripMenuItem1.Visible = true;
                zoekNaarLinksDieNietMeerBestaanToolStripMenuItem.Visible = true;
                allePaginasToolStripMenuItem1.Visible=true;
            }
        }

        private static DialogResult ShowInputDialogBox(ref string input, string prompt, string title = "Title", int width = 300, int height = 200)
        {
            //This function creates the custom input dialog box by individually creating the different window elements and adding them to the dialog box

            //Specify the size of the window using the parameters passed
            Size size = new Size(width, height);
            //Create a new form using a System.Windows Form
            Form inputBox = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ClientSize = size,
                //Set the window title using the parameter passed
                Text = title
            };

            //Create a new label to hold the prompt
            Label label = new Label
            {
                Text = prompt,
                Location = new Point(5, 5),
                Width = size.Width - 10
            };
            inputBox.Controls.Add(label);

            //Create a textbox to accept the user's input
            TextBox textBox = new TextBox
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, label.Location.Y + 20),
                Text = input
            };
            inputBox.Controls.Add(textBox);

            //Create an OK Button 
            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new Size(75, 23),
                Text = "&OK",
                Location = new Point(size.Width - 80 - 80, size.Height - 30)
            };
            inputBox.Controls.Add(okButton);

            //Create a Cancel Button
            Button cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Name = "cancelButton",
                Size = new Size(75, 23),
                Text = "&Cancel",
                Location = new Point(size.Width - 80, size.Height - 30)
            };
            inputBox.Controls.Add(cancelButton);

            //Set the input box's buttons to the created OK and Cancel Buttons respectively so the window appropriately behaves with the button clicks
            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            //Show the window dialog box 
            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;

            //After input has been submitted, return the input value
            return result;
        }

 
    }
}