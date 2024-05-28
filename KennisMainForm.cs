using Melding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using File = System.IO.File;

/*
 * in roetine SchermUpdate worden de kennispanelen gemaakt, hier krijgt elk panel een kID
 * Deze kId plaats ik in panel.tag die dan gemaakt word voor die InfoPagina.Regel
 * hierna plaats ik de zelfde hash dan in de betrevende InfoPagina.Regel
 * 
 * Hierdoor is tijdens runtime NA opbouwen pagina panel en regel gekoppeld.
 * 
 * In PaginaInhoud is een <list>InhoudPaginaMetRegels , wat een list is van Type Regel
 * Regel is een regel in XML file
 * 
 */

namespace KenisBank
{
    public partial class KennisMainForm : Form
    {
        public RegelInXML MainPagina = new RegelInXML();
        public RegelInXML PaginaZijBalk = new RegelInXML();

        public bool change_pagina = false;
        private static readonly Random random = new Random();
        private readonly List<RegelInXML> PaginaMetRegelsGevonden = new List<RegelInXML>();

        public List<Index> IndexLijst = new List<Index>();

        public string PrevPagina = string.Empty;
        public bool BlokSchrijf = false;  // bij 5 plekken omhoog of omlaag niet schrijven tussendoor.
        public RegelInXML CopyRegel = null;
        private static int diep = 0;

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
            if (MainPagina.Laad("start"))
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

            if (!File.Exists("Data\\Index.xml")) // opnieuw index maken
            {
                MaakIndex(this, null);
            }

            //if (!File.Exists("Data\\Paginas.txt") || !File.Exists("Data\\Url.txt")) // opnieuw index maken
            //{
            //    MaakLinkLijst(this, null);
            //}

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
            copyToolStripMenuItem.Enabled = editModeAanToolStripMenuItem.Checked;

            if (editModeAanToolStripMenuItem.Checked)
            {
                editPaginaToolStripMenuItem.BackColor = Color.FromArgb(189, 189, 189);
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
                        _ = MainPagina.Laad(labelPaginaInBeeld.Text);
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

            _ = mainForm.DummyBut.Focus(); // zodat focus weg gaat van panelmain, ivm raar scrollen.
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
                pagina = mainForm.MainPagina.VertaalNaarFileNaam(pagina);

                if (!mainForm.MainPagina.Laad(pagina))
                {
                    // dus nieuwe pagina
                    mainForm.MainPagina.LijstMetRegels.Clear();
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
                        item.BackColor = Color.FromArgb(189, 189, 189);// Aqua;
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
            if (MainPagina.Laad("start"))
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

            MainPagina.Save(labelPaginaInBeeld.Text);
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
            if (nieuw < MainPagina.LijstMetRegels.Count)
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
            if (nieuw > MainPagina.LijstMetRegels.Count)
            {
                nieuw = MainPagina.LijstMetRegels.Count;
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

            if (i < 0)
            {
                _ = MessageBox.Show("i<0");
                return;
            }

            // oke panel en regel nu bekend.
            if (MainPagina.LijstMetRegels[i].type_ == type.Hoofdstuk)
            {
                Hoofdstuk hoofdstuk = new Hoofdstuk();
                hoofdstuk.textBox1.Text = MainPagina.LijstMetRegels[i].tekst_;
                DialogResult save = hoofdstuk.ShowDialog();
                if (save == DialogResult.OK)
                {
                    RegelInXML regel = new RegelInXML(hoofdstuk.textBox1.Text, type.Hoofdstuk, "");
                    UpdateRegel(i, regel);
                }
                // bouw Pagina
                SchermUpdate();
            }
            if (MainPagina.LijstMetRegels[i].type_ == type.LinkDir)
            {
                LinkDir linkdir = new LinkDir();
                linkdir.textBoxLinkText.Text = MainPagina.LijstMetRegels[i].tekst_;
                linkdir.textBoxDir.Text = MainPagina.LijstMetRegels[i].url_;
                DialogResult save = linkdir.ShowDialog();

                if (save == DialogResult.OK)
                {
                    RegelInXML regel = new RegelInXML(linkdir.textBoxLinkText.Text, type.LinkDir, linkdir.textBoxDir.Text);
                    UpdateRegel(i, regel);
                    MaakIndex(this, null);
                }
                // bouw Pagina
                SchermUpdate();
            }
            if (MainPagina.LijstMetRegels[i].type_ == type.LinkFile)
            {
                LinkFile linkfile = new LinkFile();
                linkfile.textBox2.Text = MainPagina.LijstMetRegels[i].tekst_;
                linkfile.textBox1.Text = MainPagina.LijstMetRegels[i].url_;
                DialogResult save = linkfile.ShowDialog();

                if (save == DialogResult.OK)
                {
                    RegelInXML regel = new RegelInXML(linkfile.textBox2.Text, type.LinkFile, linkfile.textBox1.Text);
                    UpdateRegel(i, regel);
                    MaakIndex(this, null);
                }
                // bouw Pagina
                SchermUpdate();
            }
            if (MainPagina.LijstMetRegels[i].type_ == type.TekstBlok)
            {
                TekstBlok tb = new TekstBlok();
                string regels = MainPagina.LijstMetRegels[i].tekst_;
                string[] lines = regels.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                tb.textBoxTextBlok.Lines = lines;

                DialogResult save = tb.ShowDialog();

                if (save == DialogResult.OK)
                {
                    RegelInXML regel = new RegelInXML(tb.textBoxTextBlok.Text, type.TekstBlok, "");
                    change_pagina = true;
                    UpdateRegel(i, regel);
                }
                // bouw Pagina
                SchermUpdate();
            }
            if (MainPagina.LijstMetRegels[i].type_ == type.PaginaNaam)
            {
                Pagina pa = new Pagina();
                pa.textBoxPaginaNaam.Text = MainPagina.LijstMetRegels[i].tekst_;
                DialogResult save = pa.ShowDialog();

                if (save == DialogResult.OK)
                {
                    string oudenaam = MainPagina.LijstMetRegels[i].tekst_;
                    RegelInXML regel = new RegelInXML(pa.textBoxPaginaNaam.Text, type.PaginaNaam, "");
                    UpdateRegel(i, regel);

                    // verander op elke pagina waar oudenaam voorkomt, deze in nieuwe naam
                    VeranderPagineLinkOpElkePagina(oudenaam, pa.textBoxPaginaNaam.Text);

                    // zoek nu naam.xml op en zet om in nieuwe naam.
                    string nieuwnaam = MainPagina.VertaalNaarFileNaam(pa.textBoxPaginaNaam.Text);
                    oudenaam = MainPagina.VertaalNaarFileNaam(oudenaam);
                    System.IO.File.Move($"Data\\{oudenaam}.xml", $"Data\\{nieuwnaam}.xml");
                    MaakIndex(this, null);
                }
                // bouw Pagina
                SchermUpdate();
            }
        }
        private void UpdateRegel(int i, RegelInXML regel)
        {
            MainPagina.LijstMetRegels.RemoveAt(i);
            MainPagina.LijstMetRegels.Insert(i, regel);
            change_pagina = false;
            MainPagina.Save(labelPaginaInBeeld.Text);
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
        }
        private void ZoekNaarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoekForm ZF = new ZoekForm();
            DialogResult save = ZF.ShowDialog();
            PaginaMetRegelsGevonden.Clear();
            _ = new List<string>();
            _ = new RegelInXML();

            if (save == DialogResult.OK)
            {
                //if (!File.Exists("Data\\Paginas.txt") || !File.Exists("Data\\Url.txt")) // opnieuw index maken
                //{
                //    MaakLinkLijst(this, null);
                //}
                
                if (!File.Exists("Data\\Index.xml")) // opnieuw index maken
                {
                    MaakIndex(this, null);
                }

                IndexLaad();

                foreach(Index a in IndexLijst)
                {
                    if (ContainsCaseInsensitive(a.text, ZF.textBoxZoek.Text) || ContainsCaseInsensitive(a.url, ZF.textBoxZoek.Text))
                    {
                        if (a.url != "")
                        {
                            // zoek tekst in tekst
                            RegelInXML regel = new RegelInXML($"Op pagina {a.pagina}", type.TekstBlok, "");
                            PaginaMetRegelsGevonden.Add(regel);
                            regel = new RegelInXML(a.text, type.TekstBlok, "");
                            PaginaMetRegelsGevonden.Add(regel);
                            regel = new RegelInXML(a.url, type.LinkFile, a.url);
                            PaginaMetRegelsGevonden.Add(regel);
                            regel = new RegelInXML("", type.Leeg, "");
                            PaginaMetRegelsGevonden.Add(regel);
                        }
                    }
                }

                // bouw Pagina
                labelPaginaInBeeld.Text = $"Zoek : {ZF.textBoxZoek.Text}";
                MainPagina.LijstMetRegels = PaginaMetRegelsGevonden;
                SchermUpdate();
                change_pagina = false;
            }
        }

        private void IndexLaad()
        {
            IndexLijst.Clear();
            try
            {
                string fileNaam = $"Data\\Index.xml";
                string xmlTekst = File.ReadAllText(fileNaam);
                IndexLijst = FromXML<List<Index>>(xmlTekst);
            }
            catch
            {
            }
        }

        private static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
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

                _ = MainPagina.Laad(Path.GetFileNameWithoutExtension(file.Name));
                for (int i = 0; i < MainPagina.LijstMetRegels.Count; i++)
                {
                    if (MainPagina.LijstMetRegels[i].type_ == type.PaginaNaam)
                    {
                        string linkNaarFile = MainPagina.VertaalNaarFileNaam(MainPagina.LijstMetRegels[i].tekst_);
                        if (!LinkNaarPaginaLijst.Contains(linkNaarFile))
                        {
                            LinkNaarPaginaLijst.Add(linkNaarFile);
                        }
                    }
                }

            }
            ProgressBarUit();
            MainPagina.LijstMetRegels.Clear();
            ProgressBarAan(XMLFilesInDataDir.Count);

            foreach (FileInfo file in XMLFilesInDataDir)
            {
                ProgressBarUpdate();
                // check of filenaam een item bevat wat geen file is
                string FileNaam = Path.GetFileNameWithoutExtension(file.Name);
                if (!LinkNaarPaginaLijst.Contains(FileNaam) && FileNaam != "start" && FileNaam != "zijbalk")
                {
                    RegelInXML regel = new RegelInXML(FileNaam, type.PaginaNaam, "");
                    MainPagina.LijstMetRegels.Add(regel);
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

                _ = MainPagina.Laad(vorige_pagina);
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

            bool afbreken = false;

            ProgressBarAan(files.Count);
            foreach (FileInfo file in files)
            {
                ProgressBarUpdate();
                if (file.Name != "zijbalk.xml")
                {
                    _ = MainPagina.Laad(Path.GetFileNameWithoutExtension(file.Name));
                    // verander
                    int change = MainPagina.LijstMetRegels.Count;
                    // door file heenstappen
                    for (int i = MainPagina.LijstMetRegels.Count - 1; i > 0; i--)
                    {
                        //bij import vanuit oude wiki is link naar dir gelijk aan link naar file.
                        // dus check of het geen file en geen dir is voordat ik verwijder
                        if (MainPagina.LijstMetRegels[i].type_ == type.LinkFile)
                        {
                            if (!File.Exists(MainPagina.LijstMetRegels[i].url_))
                            {
                                if (!Directory.Exists(MainPagina.LijstMetRegels[i].url_))
                                {
                                    // link kan ook een url naar intranet zijn.
                                    if (!IsValidHttpLink(MainPagina.LijstMetRegels[i].url_))
                                    {
                                        afbreken = VraagAanpassing(file.Name, MainPagina.LijstMetRegels[i]);
                                    }
                                }
                            }
                        }
                        else if (MainPagina.LijstMetRegels[i].type_ == type.LinkDir)
                        {
                            if (!Directory.Exists(MainPagina.LijstMetRegels[i].url_))
                            {
                                // link kan ook een url naar intranet zijn.
                                if (!IsValidHttpLink(MainPagina.LijstMetRegels[i].url_))
                                {
                                    afbreken = VraagAanpassing(file.Name, MainPagina.LijstMetRegels[i]);
                                }
                            }
                        }
                        else if (MainPagina.LijstMetRegels[i].type_ == type.PaginaNaam)
                        {
                            if (!File.Exists($"Data\\{Path.GetFileNameWithoutExtension(file.Name)}.xml"))
                            {
                                afbreken = VraagAanpassing(file.Name, MainPagina.LijstMetRegels[i]);
                            }
                        }
                        if (afbreken)
                        {
                            break;
                        }
                    }
                    if (afbreken)
                    {
                        break;
                    }
                }
                if (afbreken)
                {
                    break;
                }
            }

            refreshToolStripMenuItem_Click(this, null);

            ProgressBarUit();

            if (afbreken)    // link lijst is aangepast
            {
                MaakIndex(this, null);
            }

            if (!afbreken)
            {
                HomeToolStripMenuItem_Click(this, null);
            }
        }
        // main scherm update roetine
        private void SchermUpdate()
        {
            // delete oude
            panelMain.Controls.Clear();
            //labelInfo.Text = "";
            int MakerInfoIndex = -1;
            ProgressBarAan(MainPagina.LijstMetRegels.Count);

            _ = LockWindowUpdate(panelMain.Handle);

            for (int i = 0; i < MainPagina.LijstMetRegels.Count; i++)
            {
                ProgressBarUpdate();
                //string dum = RandomString(10);
                //_ = dum.GetHashCode();

                if (MainPagina.LijstMetRegels[i].type_ == type.Hoofdstuk)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.Hoofdstuk, MainPagina.LijstMetRegels[i].tekst_, "");
                    MainPagina.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (MainPagina.LijstMetRegels[i].type_ == type.LinkDir)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.LinkDir, MainPagina.LijstMetRegels[i].tekst_, MainPagina.LijstMetRegels[i].url_);
                    MainPagina.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (MainPagina.LijstMetRegels[i].type_ == type.LinkFile)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.LinkFile, MainPagina.LijstMetRegels[i].tekst_, MainPagina.LijstMetRegels[i].url_);
                    MainPagina.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (MainPagina.LijstMetRegels[i].type_ == type.TekstBlok)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.TekstBlok, MainPagina.LijstMetRegels[i].tekst_, "");
                    MainPagina.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (MainPagina.LijstMetRegels[i].type_ == type.PaginaNaam)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.PaginaNaam, MainPagina.LijstMetRegels[i].tekst_, MainPagina.LijstMetRegels[i].url_);
                    MainPagina.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (MainPagina.LijstMetRegels[i].type_ == type.Leeg)
                {
                    KennisPanel a = new KennisPanel(panelMain, type.TekstBlok, "", "");
                    MainPagina.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (MainPagina.LijstMetRegels[i].type_ == type.EditInfo)
                {
                    // bewaar locatie van info, wordt als laatse geplaatst op scherm.
                    MakerInfoIndex = i;
                }
            }

            if (MakerInfoIndex > -1)
            {
                KennisPanel a = new KennisPanel(panelMain, type.TekstBlok, MainPagina.LijstMetRegels[MakerInfoIndex].tekst_, "");
                MainPagina.LijstMetRegels[MakerInfoIndex].eigenaar_ = a.kId;
            }

            _ = LockWindowUpdate(IntPtr.Zero);

            ProgressBarUit();

            KennisMainForm_Resize(this, null);
        }
        private void SchermUpdateZijBalk()
        {
            // delete oude
            panelZij.Controls.Clear();

            ProgressBarAan(PaginaZijBalk.LijstMetRegels.Count);

            _ = LockWindowUpdate(panelZij.Handle);

            for (int i = 0; i < PaginaZijBalk.LijstMetRegels.Count; i++)
            {
                ProgressBarUpdate();

                if (PaginaZijBalk.LijstMetRegels[i].type_ == type.Hoofdstuk)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.TekstBlok, PaginaZijBalk.LijstMetRegels[i].tekst_, "");
                    PaginaZijBalk.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.LijstMetRegels[i].type_ == type.LinkDir)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.LinkDir, PaginaZijBalk.LijstMetRegels[i].tekst_, PaginaZijBalk.LijstMetRegels[i].url_);
                    PaginaZijBalk.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.LijstMetRegels[i].type_ == type.LinkFile)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.LinkFile, PaginaZijBalk.LijstMetRegels[i].tekst_, PaginaZijBalk.LijstMetRegels[i].url_);
                    PaginaZijBalk.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.LijstMetRegels[i].type_ == type.TekstBlok)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.TekstBlok, PaginaZijBalk.LijstMetRegels[i].tekst_, "");
                    PaginaZijBalk.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.LijstMetRegels[i].type_ == type.PaginaNaam)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.PaginaNaam, PaginaZijBalk.LijstMetRegels[i].tekst_, PaginaZijBalk.LijstMetRegels[i].url_);
                    PaginaZijBalk.LijstMetRegels[i].eigenaar_ = a.kId;
                }
                if (PaginaZijBalk.LijstMetRegels[i].type_ == type.Leeg)
                {
                    KennisPanel a = new KennisPanel(panelZij, type.Leeg, "", "");
                    PaginaZijBalk.LijstMetRegels[i].eigenaar_ = a.kId;
                }
            }
            _ = LockWindowUpdate(IntPtr.Zero);

            ProgressBarUit();
        }
        private void CopyBut_Click(object sender, EventArgs e)
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
                    // oke panel en regel nu bekend.
                    CopyRegel = MainPagina.LijstMetRegels[i];
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
            if (MainPagina.Laad("zijbalk"))
            {
                labelPaginaInBeeld.Text = "zijbalk";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
            }
            else
            {
                MainPagina.LijstMetRegels.Clear();
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
                if (MainPagina.Laad(pagina))
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
        //private void BoomKennisDataToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    List<string> BoomData = new List<string>
        //        {
        //            "Start"
        //        };

        //    string VorigePagina = "Start";
        //    int VorigeIndex = 0;
        //    diep = 0;

        //    _ = BoomDataVerzamel("Start", BoomData, VorigeIndex, VorigePagina);

        //    try
        //    {
        //        File.WriteAllLines("Data\\BoomData.txt", BoomData);
        //        _ = MessageBox.Show("Klaar");
        //    }
        //    catch (IOException)
        //    {
        //        _ = MessageBox.Show("info file save Error()");
        //    }
        //}
        //private bool BoomDataVerzamel(string ZoekPagina, List<string> BoomData, int VorigeIndex, string VorigePagina)
        //{
        //    string pagina = MainPagina.VertaalNaarFileNaam(ZoekPagina);
        //    if (!MainPagina.Laad(pagina))
        //    {
        //        _ = MessageBox.Show($"Kon pagina {ZoekPagina} niet laden, staat op vorige pagina {VorigePagina}");
        //        //Process.GetCurrentProcess().Kill();
        //    }

        //    for (int index = VorigeIndex; index < MainPagina.LijstMetRegels.Count; index++)
        //    {
        //        if (MainPagina.LijstMetRegels[index].type_ == type.PaginaNaam)
        //        {
        //            string GevondenPagina = MainPagina.LijstMetRegels[index].tekst_;
        //            string inspring = "-";
        //            for (int i = 0; i < diep; i++)
        //            {
        //                inspring += "--";
        //            }
        //            BoomData.Add($"{inspring} {GevondenPagina}");
        //            diep++;
        //            VorigeIndex = index;
        //            string bewaarVorigePagina = pagina;
        //            // nieuwe pagina, dus begin op index 0
        //            if (!BoomDataVerzamel(GevondenPagina, BoomData, 0, ZoekPagina))
        //            {
        //                index = VorigeIndex;
        //                pagina = MainPagina.VertaalNaarFileNaam(bewaarVorigePagina);
        //                if (!MainPagina.Laad(pagina))
        //                {
        //                    _ = MessageBox.Show($"Kon pagina {ZoekPagina} niet laden");
        //                }

        //                diep--;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //private void OpbouwKennisBankToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Process process = new Process();
        //        process.StartInfo.FileName = "Data\\BoomData.txt";
        //        try
        //        {
        //            _ = process.Start();
        //        }
        //        catch { }
        //    }
        //    catch (IOException)
        //    {
        //        _ = MessageBox.Show("Opbouw Kennisbank niet aanwezig.");
        //    }
        //}
        private void MaakIndex(object sender, EventArgs e)
        {
            FormMelding md = new FormMelding(FormMelding.Type.Info, "KennisBank", "Maak gehele index..");
            md.Show();

            string huidigePagina = labelPaginaInBeeld.Text;

            List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
            .OrderBy(f => f.Name)
            .ToList();

            ProgressBarAan(files.Count);
            foreach (FileInfo file in files)
            {
                ProgressBarUpdate();
                bool verander = false;
                if (file.Name != "zijbalk.xml")
                {
                    string fileNaam = Path.GetFileNameWithoutExtension(file.Name);
                    _ = MainPagina.Laad(fileNaam);

                    // door file heenstappen
                    for (int i = 0; i < MainPagina.LijstMetRegels.Count; i++)
                    {
                        type Type = MainPagina.LijstMetRegels[i].type_;

                        switch (Type)
                        {
                            case type.Leeg: break;
                            case type.LinkFile:
                            case type.PaginaNaam:
                            case type.LinkDir:
                                IndexLijst.Add(new Index(fileNaam, MainPagina.LijstMetRegels[i].tekst_, MainPagina.LijstMetRegels[i].url_, MainPagina.LijstMetRegels[i].type_));
                                break;
                        }
                    }
                    if (verander)
                    {
                        MainPagina.Save(Path.GetFileNameWithoutExtension(file.Name));
                    }
                }
            }
            try
            {
                IndexSave();
            }
            catch (IOException)
            {
                _ = MessageBox.Show("Data\\Index.xml save Error()");
            }
            ProgressBarUit();
            _ = MainPagina.Laad(huidigePagina);
        }
        private void IndexSave()
        {
            try
            {
                string opslagnaam = $"Data\\Index.xml";
                string indexTekst = ToXML(IndexLijst);
                
                if(File.Exists(opslagnaam))
                {
                    File.Delete(opslagnaam);
                }
                
                File.WriteAllText(opslagnaam, indexTekst);

                FormMelding md = new FormMelding(FormMelding.Type.Save, "KennisBank", "Opslaan..");
                md.Show();
            }
            catch { }
        }
        private string ToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
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
            int nieuw = MainPagina.LijstMetRegels.Count - 1;

            MovePanel(oud, nieuw);

            change_pagina = true;
            SchermUpdate();
            SelecteerLaatstePaneel();
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
                _ = MainPagina.Laad(fileNaam);
                // door file heenstappen
                for (int i = 0; i < MainPagina.LijstMetRegels.Count; i++)
                {
                    type Type = MainPagina.LijstMetRegels[i].type_;
                    if (Type == type.PaginaNaam)
                    {
                        if (MainPagina.LijstMetRegels[i].tekst_ == oudenaam)
                        {
                            MainPagina.LijstMetRegels[i].tekst_ = nieuwnaam;
                            change = true;
                        }
                    }
                }
                if (change)
                {
                    MainPagina.Save(fileNaam);
                }
            }
        }

        // bij toevoegen van item gewoon toevoegen aan index
        // bij backup wordt lijst helemaal opnieuw gemaakt.
        private void AddLinkLijst(string pagina, type type_, string text, string url)
        {
            IndexLaad();
            IndexLijst.Add(new Index(pagina, text, url, type_));
            IndexSave();

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
                    //string test = GetBrowser();
                    //string browser = "msedge.exe";
                    _ = Process.Start("explorer", fileEnPath); // altijd default brouwser
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

                if (path == string.Empty)
                {
                    _ = MessageBox.Show($"Link {fileEnPath} bestaat niet");
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
            //BoomKennisDataToolStripMenuItem_Click(this, null);
            MaakIndex(this, null);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zoek regel welke geselecteerd
            if (!TestKlik())
            {
                return;
            }

            int eigenaar = int.Parse(GekozenItem.Text);
            int i = GetIndexVanId(eigenaar);

            if (MainPagina.LijstMetRegels[i].type_ == type.TekstBlok)
            {
                //string regels = PaginaInhoud.InhoudPaginaMetRegels[i].tekst_;
                Clipboard.SetText(MainPagina.LijstMetRegels[i].tekst_);
            }
        }

        //private void LBItem_TextChanged(object sender, EventArgs e)
        //{
        //    if (mainForm.editModeAanToolStripMenuItem.Checked)
        //    {
        //        foreach (KennisPanel item in mainForm.panelMain.Controls.OfType<KennisPanel>())
        //        {
        //            item.BackColor = mainForm.panelMain.BackColor;
        //            item.BorderStyle = BorderStyle.None;
        //            if (item.kId.ToString() == KennisMainForm.mainForm.LBItem.Text)
        //            {
        //                item.BorderStyle = BorderStyle.FixedSingle;
        //                mainForm.buttonEditSelectie.Enabled = true;
        //            }

        //        }
        //    }
        //    mainForm.GekozenItem.Text = KennisMainForm.mainForm.LBItem.Text;
        //}

        private void DummyBut_TextChanged(object sender, EventArgs e)
        {
            if (mainForm.editModeAanToolStripMenuItem.Checked)
            {
                foreach (KennisPanel item in mainForm.panelMain.Controls.OfType<KennisPanel>())
                {
                    item.BackColor = mainForm.panelMain.BackColor;
                    item.BorderStyle = BorderStyle.None;
                    if (item.kId.ToString() == KennisMainForm.mainForm.DummyBut.Text)
                    {
                        item.BorderStyle = BorderStyle.FixedSingle;
                        mainForm.buttonEditSelectie.Enabled = true;
                    }

                }
            }
            mainForm.GekozenItem.Text = KennisMainForm.mainForm.DummyBut.Text;
        }

        private void DummyBut_Click(object sender, EventArgs e)
        {
            _ = MessageBox.Show("De tekst van deze button wordt gebruikt voor tracking van panel ID");
        }

        //private static string GetBrowser()
        //{
        //    string name = string.Empty;
        //    RegistryKey regKey = null;

        //    try
        //    {
        //        var regDefault = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.htm\\UserChoice", false);
        //        var stringDefault = regDefault.GetValue("ProgId");

        //        regKey = Registry.ClassesRoot.OpenSubKey(stringDefault + "\\shell\\open\\command", false);
        //        name = regKey.GetValue(null).ToString().ToLower().Replace("" + (char)34, "");

        //        if (!name.EndsWith("exe"))
        //            name = name.Substring(0, name.LastIndexOf(".exe") + 4);

        //    }
        //    catch (Exception ex)
        //    {
        //        name = string.Format("ERROR: An exception of type: {0} occurred in method: {1} in the following module: {2}", ex.GetType(), ex.TargetSite, this.GetType());
        //    }
        //    finally
        //    {
        //        if (regKey != null)
        //            regKey.Close();
        //    }

        //    return name;
        //}

        private bool VraagAanpassing(string file_Name, RegelInXML a/* string Regel, string url*/)
        {
            Aanpassen aanpassen = new Aanpassen();
            aanpassen.PaginaLabel.Text = file_Name;
            aanpassen.LinkLabel.Text = a.tekst_;
            aanpassen.UrlLabel.Text = a.url_;
            DialogResult ant = aanpassen.ShowDialog();

            string File = Path.GetFileNameWithoutExtension(file_Name);

            if (ant == DialogResult.OK)
            {
                // open pagina
                OpenPaginaInBeeld(File);
                return true; // afbreken
            }
            if (ant == DialogResult.Abort)
            {
                // verwijder link
                _ = MainPagina.Laad(File);
                for (int i = MainPagina.LijstMetRegels.Count - 1; i > 0; i--)
                {
                    if (MainPagina.LijstMetRegels[i].tekst_ == a.tekst_)
                    {
                        MainPagina.LijstMetRegels.RemoveAt(i);
                    }
                }
                MainPagina.Save(File);
                OpenPaginaInBeeld("start");
                return true;
            }
            if (ant == DialogResult.Retry)
            {
                // aanpassen link
                _ = MainPagina.Laad(File);
                for (int i = MainPagina.LijstMetRegels.Count - 1; i > 0; i--)
                {
                    if (MainPagina.LijstMetRegels[i].tekst_ == a.tekst_)
                    {
                        LinkFile linkFile = new LinkFile();
                        linkFile.textBox2.Text = MainPagina.LijstMetRegels[i].tekst_;
                        linkFile.textBox1.Text = MainPagina.LijstMetRegels[i].url_;
                        DialogResult res = linkFile.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            MainPagina.LijstMetRegels[i].tekst_ = linkFile.textBox2.Text;
                            MainPagina.LijstMetRegels[i].url_ = linkFile.textBox1.Text;
                            MainPagina.Save(File);
                        }
                    }
                }
                OpenPaginaInBeeld("start");
                return true;
            }
            if (ant == DialogResult.Ignore)
            {
                return true; // afbreken
            }
            return false;
        }

        private void OpenPaginaInBeeld(string File)
        {
            _ = MainPagina.Laad(File);
            labelPaginaInBeeld.Text = File;
            mainForm.HistoryBalkAdd(mainForm.labelPaginaInBeeld.Text);
            mainForm.SchermUpdate();
        }

        private bool IsValidHttpLink(string url)
        {
            try
            {
                Uri uri = new Uri(url);

                // als start met file:// dan geen intranet pagina
                if (uri.Scheme == "file")
                {
                    return false;
                }

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return true;
                        }
                    }
                }
                catch (WebException)
                {
                    return false;
                }
            }
            catch (UriFormatException)
            {
                return false;
            }
            return false;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            ButtonEdit_Click(this, null);

            _ = MainPagina.Laad(labelPaginaInBeeld.Text);

            // bouw Pagina
            SchermUpdate();
            _ = PaginaZijBalk.Laad("zijbalk");
            SchermUpdateZijBalk();
        }
    }
}
