using Melding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
        public Regel PaginaZijBalk = new Regel();

        public bool change_pagina = false;
        private static readonly Random random = new Random();
        private readonly List<Regel> PaginaMetRegelsGevonden = new List<Regel>();

        public string PrevPagina = string.Empty;
        public bool BlokSchrijf = false;  // bij 5 plekken omhoog of omlaag niet schrijven tussendoor.
        public Regel CopyRegel = null;
        private static int diep = 0;

        //private static readonly int StartMinBackup = random.Next(30);

        public static KennisMainForm mainForm;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool LockWindowUpdate(IntPtr hWndLock);

        public KennisMainForm()
        {
            InitializeComponent();
            mainForm = this;
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
                SaveHuidigePaginaToolStripMenuItem_Click(this, null);
            }

            BackupNu();

            // bouw Pagina
            SchermUpdate();

            if (PaginaZijBalk.Laad("zijbalk"))
            {
                SchermUpdateZijBalk();
            }

            if (!File.Exists("Data\\Paginas.txt") || !File.Exists("Data\\Url.txt")) // opnieuw index maken
            {
                MaakLinkLijst(this, null);
            }

            FormMelding md = new FormMelding(FormMelding.Type.Info, "KennisBank", "R.Majoor");
            md.Show();
        }
        private void KennisMainForm_Resize(object sender, EventArgs e)
        {
            panel1.Width = Width - 50; // top
            Point hoek = panel1.Location;
            flowHistorie.Location = new Point(hoek.X + panel1.Width - flowHistorie.Width, flowHistorie.Location.Y);
            panelMain.Width = Width - panelZij.Width - flowHistorie.Width - 95;
            flowHistorie.Height = panelMain.Height = Height - 180;
            panelZij.Height = panelMain.Height - panelLogo.Height - 16;

            progressBar.Location = new Point(hoek.X, progressBar.Location.Y);
            progressBar.Width = panel1.Width;
        }
        private void FlowHistorie_SizeChanged(object sender, EventArgs e)
        {
            // een max with
            if (flowHistorie.Width > 250)
            {
                flowHistorie.Width = 250;
                flowHistorie.Height += 30;
            }
        }

        // interact met gebruiker
        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            // ook in alle paginaas en zoek, wees , alleen een beheerder kan hier bij komen.
            if (editModeAanToolStripMenuItem.Checked && (labelPaginaInBeeld.Text.Length > 3) && (labelPaginaInBeeld.Text.Substring(0, 4) == "Zoek"))
            {
                _ = MessageBox.Show("Zoek pagina kunt u niet aanpassen!");
                editModeAanToolStripMenuItem.Checked = false;
                editModeToolStripMenuItem.Checked = editModeAanToolStripMenuItem.Checked;
                return;
            }

            addItemToolStripMenuItem.Enabled = editModeAanToolStripMenuItem.Checked;
            saveHuidigePaginaToolStripMenuItem.Enabled = buttonSaveCloseEdit.Enabled = editModeAanToolStripMenuItem.Checked;
            editModeToolStripMenuItem.Checked = editModeAanToolStripMenuItem.Checked;

            if (editModeAanToolStripMenuItem.Checked)
            {
                editPaginaToolStripMenuItem.BackColor = Color.FromArgb(204, 231, 150);
                change_pagina = false;
                SelecteerEerstePaneel();

                panel1.Width = Width - 50; // top
                Point hoek = panel1.Location;
                panelUpDown.Location = new Point(hoek.X + panel1.Width - panelUpDown.Width, flowHistorie.Location.Y);

                panelUpDown.Visible = true;
                panelUpDown.BringToFront();
                FormMelding md = new FormMelding(FormMelding.Type.Edit, "KennisBank", "Edit Mode");
                md.Show();
            }
            else
            {
                if (change_pagina)
                {
                    DialogResult dialogResult = MessageBox.Show($"Pagina is aangepast, eerst saven ?", "Vraagje", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        SaveHuidigePaginaToolStripMenuItem_Click(this, null);
                    }
                    else
                    {
                        _ = PaginaInhoud.Laad(labelPaginaInBeeld.Text);
                        SchermUpdate();
                    }
                    change_pagina = false;
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

        public static void Label_LinkKlik(int kId)
        {
            if (mainForm.editModeAanToolStripMenuItem.Checked)
            {
                KleurGeselecteerdePanel(kId);
                //_ = MessageBox.Show("Eerst uit edit mode om link te volgen.");
                return;
            }

            KennisPanel item = GetKennisPanel(kId);
            if (item != null)
            {
                LinkLabel a = (LinkLabel)item.Controls[0];
                a.LinkVisited = true;
                Start(item.kUrl);
            }
        }
        public static void PaginaKlik(int kId)
        {
            if (mainForm.editModeAanToolStripMenuItem.Checked)
            {
                KleurGeselecteerdePanel(kId);
                //_ = MessageBox.Show("Eerst uit edit mode om naar andere pagina te gaan.");
                return;
            }

            KennisPanel item = GetKennisPanel(kId);
            if (item != null)
            {
                mainForm.PrevPagina = mainForm.labelPaginaInBeeld.Text;
                //Button but = (Button)sender;
                string pagina = item.kText.Trim();

                //pagina worden opgeslagen in lower case Enabled elke spatie vervangen door _
                pagina = mainForm.PaginaInhoud.VertaalNaarFileNaam(pagina);

                if (!mainForm.PaginaInhoud.Laad(pagina))
                {
                    // dus nieuwe pagina
                    mainForm.PaginaInhoud.InhoudPaginaMetRegels.Clear();
                    mainForm.labelPaginaInBeeld.Text = pagina;
                    mainForm.HistoryBalkAdd(mainForm.labelPaginaInBeeld.Text);
                    mainForm.SchermUpdate();
                }
                else
                {
                    mainForm.labelPaginaInBeeld.Text = pagina;
                    mainForm.HistoryBalkAdd(mainForm.labelPaginaInBeeld.Text);
                    // bouw Pagina
                    mainForm.SchermUpdate();
                }
            }
        }
        public static void KleurGeselecteerdePanel(int kId)
        {
            mainForm.buttonEditSelectie.Enabled = false;
            if (mainForm.editModeAanToolStripMenuItem.Checked)
            {
                foreach (KennisPanel item in mainForm.panelMain.Controls.OfType<KennisPanel>())
                {
                    item.BackColor = mainForm.panelMain.BackColor;
                    item.BorderStyle = BorderStyle.None;
                    if (item.kId == kId)
                    {
                        item.BackColor = Color.FromArgb(202, 229, 149);// Aqua;
                        item.BorderStyle = BorderStyle.FixedSingle;
                        mainForm.buttonEditSelectie.Enabled = true;
                    }

                }
            }
            mainForm.GekozenItem.Text = kId.ToString();
        }
        private void HomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            ButtonEdit_Click(this, null);

            // laad Start.xml
            if (PaginaInhoud.Laad("start"))
            {
                labelPaginaInBeeld.Text = "start";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
            }

            // bouw Pagina
            SchermUpdate();
            _ = PaginaZijBalk.Laad("zijbalk");
            SchermUpdateZijBalk();
        }
        private void SaveHuidigePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (labelPaginaInBeeld.Text.Length > 3 && labelPaginaInBeeld.Text.Substring(0, 4) == "Wees")
            {
                change_pagina = false;
                return;
            }

            PaginaInhoud.Save(labelPaginaInBeeld.Text);
            change_pagina = false;
        }
        private void ToevoegenLegeRegelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Toevoegen(" ", type.Leeg, "");
            // bouw Pagina
            SchermUpdate();
            // meteen selecteren
            SelecteerLaatstePaneel();
        }
        private void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            int oud = int.Parse(GekozenItem.Text);
            oud = GetIndexVanId(oud);
            int nieuw = oud - 1;
            if (nieuw > -1)
            {
                MovePanel(oud, nieuw);
            }
        }
        private void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            int oud = int.Parse(GekozenItem.Text);
            oud = GetIndexVanId(oud);
            int nieuw = oud + 1;
            if (nieuw < PaginaInhoud.InhoudPaginaMetRegels.Count)
            {
                MovePanel(oud, nieuw);
            }
        }
        private void ButtonMoveUp5_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            int oud = int.Parse(GekozenItem.Text);
            oud = GetIndexVanId(oud);
            int nieuw = oud - 10;
            if (nieuw < 0)
            {
                nieuw = 0;
            }

            MovePanel(oud, nieuw);
        }
        private void ButtonMoveDown5_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            int oud = int.Parse(GekozenItem.Text);
            oud = GetIndexVanId(oud);
            int nieuw = oud + 10;
            if (nieuw > PaginaInhoud.InhoudPaginaMetRegels.Count)
            {
                nieuw = PaginaInhoud.InhoudPaginaMetRegels.Count;
            }

            MovePanel(oud, nieuw);
        }
        private void ButtonSaveCloseEdit_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            ButtonEdit_Click(this, null);
        }
        private void ButtonEditSelectie_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            // bij edit maak ik index opnieuw, dus oude gaat dan weg en nieuwe in lijst.
            // bij toevoegen item voeg ik alleen item toe aan index.

            int eigenaar = int.Parse(GekozenItem.Text);
            int i = GetIndexVanId(eigenaar);

            // oke panel en regel nu bekend.
            if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.Hoofdstuk)
            {
                Hoofdstuk hoofdstuk = new Hoofdstuk();
                hoofdstuk.textBox1.Text = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                DialogResult save = hoofdstuk.ShowDialog();
                if (save == DialogResult.OK)
                {
                    Regel regel = new Regel(hoofdstuk.textBox1.Text, type.Hoofdstuk, "");
                    UpdateRegel(i, regel);
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
                    Regel regel = new Regel(linkdir.textBoxLinkText.Text, type.LinkDir, linkdir.textBoxDir.Text);
                    UpdateRegel(i, regel);
                    MaakLinkLijst(this, null);
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
                    Regel regel = new Regel(linkfile.textBox2.Text, type.LinkFile, linkfile.textBox1.Text);
                    UpdateRegel(i, regel);
                    MaakLinkLijst(this, null);
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
                    Regel regel = new Regel(tb.textBoxTextBlok.Text, type.TekstBlok, "");
                    change_pagina = true;
                    UpdateRegel(i, regel);
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
                    Regel regel = new Regel(pa.textBoxPaginaNaam.Text, type.PaginaNaam, "");
                    UpdateRegel(i, regel);

                    // verander op elke pagina waar oudenaam voorkomt, deze in nieuwe naam
                    VeranderPagineLinkOpElkePagina(oudenaam, pa.textBoxPaginaNaam.Text);

                    // zoek nu naam.xml op en zet om in nieuwe naam.
                    string nieuwnaam = PaginaInhoud.VertaalNaarFileNaam(pa.textBoxPaginaNaam.Text);
                    oudenaam = PaginaInhoud.VertaalNaarFileNaam(oudenaam);
                    System.IO.File.Move($"Data\\{oudenaam}.xml", $"Data\\{nieuwnaam}.xml");
                    MaakLinkLijst(this, null);
                }
                // bouw Pagina
                SchermUpdate();
            }
        }
        private void UpdateRegel(int i, Regel regel)
        {
            PaginaInhoud.InhoudPaginaMetRegels.RemoveAt(i);
            PaginaInhoud.InhoudPaginaMetRegels.Insert(i, regel);
            change_pagina = false;
            PaginaInhoud.Save(labelPaginaInBeeld.Text);
        }
        private void VersieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab = new About();
            _ = ab.ShowDialog();
        }
        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //// importeer oude wiki data
            //List<FileInfo> files = new DirectoryInfo("pages").EnumerateFiles("*.txt")
            //                .OrderBy(f => f.Name)
            //                .ToList();

            //foreach (FileInfo file in files)
            //{
            //    PaginaInhoud.InhoudPaginaMetRegels.Clear();
            //    List<string> OudeWikiTekst = File.ReadAllLines($"pages\\{Path.GetFileNameWithoutExtension(file.Name)}.txt").ToList();

            //    int aantal_regels = OudeWikiTekst.Count();
            //    for (int i = 0; i < aantal_regels; i++)
            //    {
            //        string regel = OudeWikiTekst[i].Trim();

            //        // als tabel '|' maak er losse regels van
            //        int pos = regel.IndexOf("|");
            //        if (pos == 0)
            //        {
            //            string[] opgedeeld = regel.Split('|');
            //            for (int j = 0; j < opgedeeld.Length; j++)
            //            {
            //                ImporteerRegel(opgedeeld[j], file.Name);
            //            }
            //        }
            //        else
            //        {
            //            ImporteerRegel(regel, file.Name);
            //        }

            //    }
            //    // save als 
            //    PaginaInhoud.ChangePagina.Clear();
            //    string file_Naam = Path.GetFileNameWithoutExtension(file.Name);
            //    if (file_Naam == "sidebar")
            //    {
            //        file_Naam = "zijbalk";
            //    }

            //    PaginaInhoud.Save(file_Naam);
            //}
            //change_pagina = false;
            //SchermUpdate();
            //_ = MessageBox.Show("Klaar import");
        }
        private void VorigeVersiePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test of vorige versie bestaat
            string fi = labelPaginaInBeeld.Text;
            string opslagnaam = $"Data\\{fi}.xml";
            string backup1 = $"Data\\{fi}.bak";

            BackupTerug BT = new BackupTerug();

            BT.PaginaNaam.Text = fi;
            BT.labelHuidig.Text = File.GetLastWriteTime(opslagnaam).ToLongDateString();
            BT.labelBackup1.Text = File.GetLastWriteTime(backup1).ToLongDateString();

            _ = BT.ShowDialog();

            _ = PaginaInhoud.Laad(fi);
            SchermUpdate();
        }
        private void ZoekNaarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoekForm ZF = new ZoekForm();
            DialogResult save = ZF.ShowDialog();
            PaginaMetRegelsGevonden.Clear();
            _ = new List<string>();
            _ = new Regel();

            if (save == DialogResult.OK)
            {
                if (!File.Exists("Data\\Paginas.txt") || !File.Exists("Data\\Url.txt")) // opnieuw index maken
                {
                    MaakLinkLijst(this, null);
                }

                List<string> LijstPaginas = File.ReadAllLines("Data\\Paginas.txt").ToList();

                for (int i = 0; i < LijstPaginas.Count; i++)
                {
                    if (ContainsCaseInsensitive(LijstPaginas[i], ZF.textBoxZoek.Text))
                    {
                        Regel regel = new Regel(LijstPaginas[i], type.PaginaNaam, LijstPaginas[i]);
                        PaginaMetRegelsGevonden.Add(regel);
                        regel = new Regel("", type.Leeg, "");
                        PaginaMetRegelsGevonden.Add(regel);
                    }
                }

                List<string> LijstUrl = File.ReadAllLines("Data\\Url.txt").ToList();

                for (int i = 0; i < LijstUrl.Count; i++)
                {
                    if (ContainsCaseInsensitive(LijstUrl[i], ZF.textBoxZoek.Text))
                    {
                        string link;
                        string url;
                        if (i % 2 == 0)
                        {
                            link = LijstUrl[i];
                            url = LijstUrl[i + 1];
                        }
                        else
                        {
                            link = LijstUrl[i - 1];
                            url = LijstUrl[i];
                            i++;
                        }

                        Regel regel = new Regel($"Gevonden in {url}", type.TekstBlok, "");
                        PaginaMetRegelsGevonden.Add(regel);
                        regel = new Regel(link, type.LinkFile, url);
                        PaginaMetRegelsGevonden.Add(regel);
                        regel = new Regel("", type.Leeg, "");
                        PaginaMetRegelsGevonden.Add(regel);
                    }
                }

                // bouw Pagina
                labelPaginaInBeeld.Text = $"Zoek : {ZF.textBoxZoek.Text}";
                PaginaInhoud.InhoudPaginaMetRegels = PaginaMetRegelsGevonden;
                SchermUpdate();
                change_pagina = false;
            }
        }
        private void ZoekNaarWeesPaginasToolStripMenuItem_Click(object sender, EventArgs e)
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
                        string linkNaarFile = PaginaInhoud.VertaalNaarFileNaam(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_);
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
                if (!LinkNaarPaginaLijst.Contains(FileNaam) && FileNaam != "start" && FileNaam != "zijbalk")
                {
                    Regel regel = new Regel(FileNaam, type.PaginaNaam, "");
                    PaginaInhoud.InhoudPaginaMetRegels.Add(regel);
                }
            }
            labelPaginaInBeeld.Text = $"Wees Pagina's";
            SchermUpdate();
            ProgressBarUit();
        }
        private void TerugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (flowHistorie.Controls.Count > 1)
            {
                LinkLabel a = (LinkLabel)flowHistorie.Controls[flowHistorie.Controls.Count - 2];
                string vorige_pagina = a.Text;

                _ = PaginaInhoud.Laad(vorige_pagina);
                labelPaginaInBeeld.Text = vorige_pagina;

                flowHistorie.Controls.RemoveAt(flowHistorie.Controls.Count - 1);
                SchermUpdate();
            }
        }
        private void zoekLinksDieNietMeerBestaanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                        .OrderByDescending(f => f.Name)
                        .ToList();

            List<string> LijstMetLinksDieNietBestaan = new List<string>();

            ProgressBarAan(files.Count);
            foreach (FileInfo file in files)
            {
                ProgressBarUpdate();
                if (file.Name != "zijbalk.xml")
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
                                    LijstMetLinksDieNietBestaan.Add($"Op Pagina {Path.GetFileNameWithoutExtension(file.Name)}");
                                    LijstMetLinksDieNietBestaan.Add($"{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}");
                                    LijstMetLinksDieNietBestaan.Add("");
                                }
                            }
                        }
                        else if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkDir)
                        {
                            if (!Directory.Exists(PaginaInhoud.InhoudPaginaMetRegels[i].url_))
                            {
                                LijstMetLinksDieNietBestaan.Add($"Op Pagina {Path.GetFileNameWithoutExtension(file.Name)}");
                                LijstMetLinksDieNietBestaan.Add($"{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}");
                                LijstMetLinksDieNietBestaan.Add("");
                            }
                        }
                        else if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.PaginaNaam)
                        {
                            if (!File.Exists($"Data\\{Path.GetFileNameWithoutExtension(file.Name)}.xml"))
                            {
                                LijstMetLinksDieNietBestaan.Add($"Op Pagina {Path.GetFileNameWithoutExtension(file.Name)}");
                                LijstMetLinksDieNietBestaan.Add($"{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}");
                                LijstMetLinksDieNietBestaan.Add("");
                            }
                        }
                    }
                }
            }

            try
            {
                File.WriteAllLines("Data\\NietWerkendeLinks.txt", LijstMetLinksDieNietBestaan);

                Process process = new Process();
                process.StartInfo.FileName = "Data\\NietWerkendeLinks.txt";
                try
                {
                    _ = process.Start();
                }
                catch { }
            }
            catch (IOException)
            {
                _ = MessageBox.Show("info file save Error()");
            }
            ProgressBarUit();
            HomeToolStripMenuItem_Click(this, null);
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
                ProgressBarUpdate();
                string dum = RandomString(10);
                _ = dum.GetHashCode();

                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.Hoofdstuk)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.Hoofdstuk, PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, "");
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkDir)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.LinkDir, PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, PaginaInhoud.InhoudPaginaMetRegels[i].url_);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.LinkFile)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.LinkFile, PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, PaginaInhoud.InhoudPaginaMetRegels[i].url_);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.TekstBlok)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.TekstBlok, PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, "");
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.PaginaNaam)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.PaginaNaam, PaginaInhoud.InhoudPaginaMetRegels[i].tekst_, PaginaInhoud.InhoudPaginaMetRegels[i].url_);
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.Leeg)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.TekstBlok, "", "");
                    PaginaInhoud.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaInhoud.InhoudPaginaMetRegels[i].type_ == type.EditInfo)
                {
                    // bewaar locatie van info, wordt als laatse geplaatst op scherm.
                    MakerInfoIndex = i;
                }
            }

            if (MakerInfoIndex > -1)
            {
                KennisPanel a = new KennisPanel(panelMain, type.TekstBlok, PaginaInhoud.InhoudPaginaMetRegels[MakerInfoIndex].tekst_, "");
                PaginaInhoud.InhoudPaginaMetRegels[MakerInfoIndex].eigenaar_ = a.kId;
            }

            _ = LockWindowUpdate(IntPtr.Zero);

            ProgressBarUit();

            KennisMainForm_Resize(this, null);
        }
        private void SchermUpdateZijBalk()
        {
            // delete oude
            panelZij.Controls.Clear();

            ProgressBarAan(PaginaZijBalk.InhoudPaginaMetRegels.Count);

            _ = LockWindowUpdate(panelZij.Handle);

            for (int i = 0; i < PaginaZijBalk.InhoudPaginaMetRegels.Count; i++)
            {
                ProgressBarUpdate();

                if (PaginaZijBalk.InhoudPaginaMetRegels[i].type_ == type.Hoofdstuk)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.TekstBlok, PaginaZijBalk.InhoudPaginaMetRegels[i].tekst_, "");
                    PaginaZijBalk.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.InhoudPaginaMetRegels[i].type_ == type.LinkDir)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.LinkDir, PaginaZijBalk.InhoudPaginaMetRegels[i].tekst_, PaginaZijBalk.InhoudPaginaMetRegels[i].url_);
                    PaginaZijBalk.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.InhoudPaginaMetRegels[i].type_ == type.LinkFile)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.LinkFile, PaginaZijBalk.InhoudPaginaMetRegels[i].tekst_, PaginaZijBalk.InhoudPaginaMetRegels[i].url_);
                    PaginaZijBalk.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.InhoudPaginaMetRegels[i].type_ == type.TekstBlok)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.TekstBlok, PaginaZijBalk.InhoudPaginaMetRegels[i].tekst_, "");
                    PaginaZijBalk.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.InhoudPaginaMetRegels[i].type_ == type.PaginaNaam)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.PaginaNaam, PaginaZijBalk.InhoudPaginaMetRegels[i].tekst_, PaginaZijBalk.InhoudPaginaMetRegels[i].url_);
                    PaginaZijBalk.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.InhoudPaginaMetRegels[i].type_ == type.Leeg)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.Leeg, "", "");
                    PaginaZijBalk.InhoudPaginaMetRegels[i].eigenaar_ = a.kId;
                }
            }
            _ = LockWindowUpdate(IntPtr.Zero);

            ProgressBarUit();
        }

        private void BeheerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputStringForm IPS = new InputStringForm();

            DialogResult dialogResult = IPS.ShowDialog();
            if (dialogResult == DialogResult.OK && IPS.textBox1.Text == DateTime.Now.ToString("ddMM"))
            {
                importAllePaginasOudeWikiToolStripMenuItem.Visible = true;
                paginaBackupTerugZettenToolStripMenuItem.Visible = true;
                zoekNaarWeesPaginasToolStripMenuItem1.Visible = true;
                zoekNaarLinksDieNietMeerBestaanToolStripMenuItem.Visible = true;
                editZijBlakToolStripMenuItem.Visible = true;
                boomKennisDataToolStripMenuItem.Visible = true;
                repareerToolStripMenuItem.Visible = true;
            }
        }
        private void CopyBut_Click(object sender, EventArgs e)
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
                    // oke panel en regel nu bekend.
                    CopyRegel = PaginaInhoud.InhoudPaginaMetRegels[i];
                    break;
                }
            }
        }
        private void PasteBut_Click(object sender, EventArgs e)
        {
            Toevoegen(CopyRegel.tekst_, CopyRegel.type_, CopyRegel.url_);
            change_pagina = true;

            // bouw Pagina
            SchermUpdate();
            SelecteerLaatstePaneel();
        }
        private void EditZijBlakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // laad ZijBalk.xml
            if (PaginaInhoud.Laad("zijbalk"))
            {
                labelPaginaInBeeld.Text = "zijbalk";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
            }
            else
            {
                PaginaInhoud.InhoudPaginaMetRegels.Clear();
                labelPaginaInBeeld.Text = "zijbalk";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
                SaveHuidigePaginaToolStripMenuItem_Click(this, null);
            }
            SchermUpdate();
        }
        private void Historylabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel a = (LinkLabel)sender;
            string pagina = a.Tag as string;

            if (editModeAanToolStripMenuItem.Checked)
            {
                _ = MessageBox.Show("Eerst uit edit mode!");
            }
            else
            {
                if (PaginaInhoud.Laad(pagina))
                {
                    labelPaginaInBeeld.Text = pagina;

                    if (flowHistorie.Controls.Count > 1)
                    {
                        for (int i = flowHistorie.Controls.Count - 1; i > 0; i--)
                        {
                            LinkLabel qa = (LinkLabel)flowHistorie.Controls[i];
                            if ((string)qa.Tag != pagina)
                            {
                                flowHistorie.Controls.RemoveAt(i);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                // bouw Pagina
                SchermUpdate();
            }
        }
        private void BoomKennisDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> BoomData = new List<string>
                {
                    "Start"
                };

            string VorigePagina = "Start";
            int VorigeIndex = 0;
            diep = 0;

            _ = BoomDataVerzamel("Start", BoomData, VorigeIndex, VorigePagina);

            try
            {
                File.WriteAllLines("Data\\BoomData.txt", BoomData);
            }
            catch (IOException)
            {
                _ = MessageBox.Show("info file save Error()");
            }
        }
        private bool BoomDataVerzamel(string ZoekPagina, List<string> BoomData, int VorigeIndex, string VorigePagina)
        {
            string pagina = PaginaInhoud.VertaalNaarFileNaam(ZoekPagina);
            if (!PaginaInhoud.Laad(pagina))
            {
                _ = MessageBox.Show($"Kon pagina {ZoekPagina} niet laden, staat op vorige pagina {VorigePagina}");
                Process.GetCurrentProcess().Kill();
            }

            for (int index = VorigeIndex; index < PaginaInhoud.InhoudPaginaMetRegels.Count; index++)
            {
                if (PaginaInhoud.InhoudPaginaMetRegels[index].type_ == type.PaginaNaam)
                {
                    string GevondenPagina = PaginaInhoud.InhoudPaginaMetRegels[index].tekst_;
                    string inspring = "-";
                    for (int i = 0; i < diep; i++)
                    {
                        inspring += "--";
                    }
                    BoomData.Add($"{inspring} {GevondenPagina}");
                    diep++;
                    VorigeIndex = index;
                    string bewaarVorigePagina = pagina;
                    // nieuwe pagina, dus begin op index 0
                    if (!BoomDataVerzamel(GevondenPagina, BoomData, 0, ZoekPagina))
                    {
                        index = VorigeIndex;
                        pagina = PaginaInhoud.VertaalNaarFileNaam(bewaarVorigePagina);
                        if (!PaginaInhoud.Laad(pagina))
                        {
                            _ = MessageBox.Show($"Kon pagina {ZoekPagina} niet laden");
                        }

                        diep--;
                    }
                }
            }
            return false;
        }
        private void OpbouwKennisBankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "Data\\BoomData.txt";
                try
                {
                    _ = process.Start();
                }
                catch { }
            }
            catch (IOException)
            {
                _ = MessageBox.Show("Opbouw Kennisbank niet aanwezig.");
            }
        }
        private void MaakLinkLijst(object sender, EventArgs e)
        {
            FormMelding md = new FormMelding(FormMelding.Type.Info, "KennisBank", "Maak gehele index..");
            md.Show();

            string huidigePagina = labelPaginaInBeeld.Text;

            List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
            .OrderBy(f => f.Name)
            .ToList();

            List<string> LijstUrl = new List<string>();
            List<string> ListPaginas = new List<string>();
            ProgressBarAan(files.Count);
            foreach (FileInfo file in files)
            {
                ProgressBarUpdate();
                bool verander = false;
                if (file.Name != "zijbalk.xml")
                {
                    string fileNaam = Path.GetFileNameWithoutExtension(file.Name);
                    _ = PaginaInhoud.Laad(fileNaam);

                    // door file heenstappen
                    for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
                    {
                        type Type = PaginaInhoud.InhoudPaginaMetRegels[i].type_;

                        switch (Type)
                        {
                            case type.Leeg: break;
                            case type.LinkFile:
                                try
                                {
                                    string ext = Path.GetExtension(PaginaInhoud.InhoudPaginaMetRegels[i].url_);
                                    if (ext == "")
                                    {
                                        // Dan is het een link naar een dir
                                        PaginaInhoud.InhoudPaginaMetRegels[i].type_ = type.LinkDir;
                                        verander = true;
                                        break;
                                    }
                                    LijstUrl.Add(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_);
                                    LijstUrl.Add(PaginaInhoud.InhoudPaginaMetRegels[i].url_);
                                }
                                catch
                                {
                                    MessageBox.Show($"Error in Index Maken\n{PaginaInhoud.InhoudPaginaMetRegels[i].tekst_}\n{PaginaInhoud.InhoudPaginaMetRegels[i].url_}");
                                }
                                break;
                            case type.PaginaNaam:
                                ListPaginas.Add(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_);
                                break;
                            case type.LinkDir:
                                LijstUrl.Add(PaginaInhoud.InhoudPaginaMetRegels[i].tekst_);
                                LijstUrl.Add(PaginaInhoud.InhoudPaginaMetRegels[i].url_);
                                break;
                        }
                    }
                    if (verander)
                    {
                        PaginaInhoud.Save(Path.GetFileNameWithoutExtension(file.Name));
                    }
                }
            }
            try
            {
                File.WriteAllLines("Data\\Url.txt", LijstUrl);
                File.WriteAllLines("Data\\Paginas.txt", ListPaginas);
            }
            catch (IOException)
            {
                _ = MessageBox.Show("info file save Error()");
            }
            ProgressBarUit();
            PaginaInhoud.Laad(huidigePagina);
        }
        private void LinkLijstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "Data\\Url.txt";
                try
                {
                    _ = process.Start();
                }
                catch { }
            }
            catch (IOException)
            {
                _ = MessageBox.Show("Kan Link Lijst niet Laden.");
            }
        }
        private void ButtonBoven_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            int oud = int.Parse(GekozenItem.Text);
            oud = GetIndexVanId(oud);
            int nieuw = 0;

            MovePanel(oud, nieuw);

            change_pagina = true;
            SchermUpdate();
            SelecteerEerstePaneel();
        }
        private void ButtonBeneden_Click(object sender, EventArgs e)
        {
            if (!TestKlik())
            {
                return;
            }

            int oud = int.Parse(GekozenItem.Text);
            oud = GetIndexVanId(oud);
            int nieuw = PaginaInhoud.InhoudPaginaMetRegels.Count - 1;

            MovePanel(oud, nieuw);

            change_pagina = true;
            SchermUpdate();
            SelecteerLaatstePaneel();
        }
        private void allePaginasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "Data\\Paginas.txt";
                try
                {
                    _ = process.Start();
                }
                catch { }
            }
            catch (IOException)
            {
                _ = MessageBox.Show("Kan Paginas Lijst niet Laden.");
            }
        }

        // verander op elke pagina waar oudenaam voorkomt, deze in nieuwe naam
        private void VeranderPagineLinkOpElkePagina(string oudenaam, string nieuwnaam)
        {
            FormMelding md = new FormMelding(FormMelding.Type.Info, "KennisBank", "verander pagina op elke pagina.");
            md.Show();

            List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
            .OrderBy(f => f.Name)
            .ToList();

            foreach (FileInfo file in files)
            {
                bool change = false;
                string fileNaam = Path.GetFileNameWithoutExtension(file.Name);
                _ = PaginaInhoud.Laad(fileNaam);
                // door file heenstappen
                for (int i = 0; i < PaginaInhoud.InhoudPaginaMetRegels.Count; i++)
                {
                    type Type = PaginaInhoud.InhoudPaginaMetRegels[i].type_;
                    if (Type == type.PaginaNaam)
                    {
                        if (PaginaInhoud.InhoudPaginaMetRegels[i].tekst_ == oudenaam)
                        {
                            PaginaInhoud.InhoudPaginaMetRegels[i].tekst_ = nieuwnaam;
                            change = true;
                        }
                    }
                }
                if (change)
                {
                    PaginaInhoud.Save(fileNaam);
                }
            }
        }

        // bij toevoegen van item gewoon toevoegen aan index
        // bij backup wordt lijst helemaal opnieuw gemaakt.
        private void AddLinkLijst(type type_, string text, string url)
        {
            List<string> Lijst = new List<string>();
            switch (type_)
            {
                case type.PaginaNaam:
                    Lijst = File.ReadAllLines("Data\\Paginas.txt").ToList();
                    Lijst.Add(@text);
                    Lijst.Add(@url);
                    File.WriteAllLines("Data\\Paginas.txt", Lijst);
                    break;
                case type.LinkDir:
                case type.LinkFile:
                    Lijst = File.ReadAllLines("Data\\Url.txt").ToList();
                    Lijst.Add(@text);
                    Lijst.Add(@url);
                    File.WriteAllLines("Data\\Url.txt", Lijst);
                    break;

            }

        }

        private static void Start(string fileEnPath)
        {
            string path = string.Empty;
            string file = string.Empty;
            
            try
            {
                // bij bv http:// of https://
                if (fileEnPath.Length > 5 && fileEnPath.Substring(0, 4) == "http")
                {
                    //string browser = "msedge.exe";
                    Process.Start("explorer"/*browser*/, fileEnPath); // altijd default brouwser
                    return;
                }
                
                if (Directory.Exists(fileEnPath))
                {
                    path = fileEnPath;
                    file = fileEnPath;
                }
                
                if (File.Exists(fileEnPath))
                {
                    path = Path.GetDirectoryName(fileEnPath);
                    file = Path.GetFileName(fileEnPath);
                }

                if(path == string.Empty)
                {
                    MessageBox.Show($"Link {fileEnPath} bestaat niet");
                    return;
                }

                ProcessStartInfo _processStartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = path,
                    FileName = file
                };

                //_processStartInfo.Arguments = "test.txt";

                //_processStartInfo.CreateNoWindow = true;

                try
                {
                    Process myProcess = Process.Start(_processStartInfo);
                }
                catch { }
            }
            catch (IOException)
            {
                _ = MessageBox.Show("Kan Paginas Lijst niet Laden.");
            }
        }

        private void editModeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = !editModeAanToolStripMenuItem.Checked;
            editModeToolStripMenuItem.Checked = editModeAanToolStripMenuItem.Checked;
            ButtonEdit_Click(this, null);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void zoekToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ZoekNaarToolStripMenuItem_Click(this, null);
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZetBackupDatumInFile();
            Backup();
            BoomKennisDataToolStripMenuItem_Click(this, null);
            MaakLinkLijst(this, null);
        }
    }
}