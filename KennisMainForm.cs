using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
        public Regel InfoPagina = new Regel();
        public Panel panelGeselecteerd = new Panel();
        public bool change_pagina = false;
        private static readonly Random random = new Random();
        private readonly List<Regel> PaginaMetRegelsGevonden = new List<Regel>();
        private readonly List<string> history = new List<string>();
        public string PrevPagina = string.Empty;

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
                HistoryBalkAdd(labelPaginaInBeeld.Text);
            }
            else
            {
                // start bestaan niet, maak lege
                labelPaginaInBeeld.Text = "Start";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
                saveHuidigePaginaToolStripMenuItem_Click(this, null);
            }

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
            if (labelPaginaInBeeld.Text.Substring(0,4) == "Zoek" || labelPaginaInBeeld.Text == "Alle paginas" || labelPaginaInBeeld.Text.Substring(0,4) == "Wees")
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
                deleteItemToolStripMenuItem.Enabled = buttonDelete.Enabled = buttonEditSelectie.Enabled = false;
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
            PrevPagina = labelPaginaInBeeld.Text;
            Button but = (Button)sender;
            string pagina = but.Text;

            //pagina worden opgeslagen in lower case Enabled elke spatie vervangen door _
            pagina = InfoPagina.RemoveOudeWikiTekens(pagina);

            if (!InfoPagina.Laad(pagina))
            {
                // dus nieuwe pagina
                InfoPagina.PaginaMetRegels.Clear();
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
        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editModeAanToolStripMenuItem.Checked = false;
            buttonEdit_Click(this, null);

            // laad Start.xml
            if (InfoPagina.Laad("Start"))
            {
                labelPaginaInBeeld.Text = "Start";
                HistoryBalkAdd(labelPaginaInBeeld.Text);
            }

            // bouw Pagina
            SchermUpdate();
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
            SchermUpdate();
            // meteen selecteren
            SelecteerLaatstePaneel();
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
            SchermUpdate();
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
                        SchermUpdate();
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
                        SchermUpdate();
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
                        SchermUpdate();
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
                        SchermUpdate();
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
                            .OrderByDescending(f => f.Name)
                            .ToList();

            foreach (FileInfo file in files)
            {
                InfoPagina.PaginaMetRegels.Clear();
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
                InfoPagina.Save(Path.GetFileNameWithoutExtension(file.Name));
            }
            MessageBox.Show("Klaar import");
        }
        private void allePaginasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FileInfo> files = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();

            InfoPagina.PaginaMetRegels.Clear();
            int zoekinfo = files.Count;

            foreach (FileInfo file in files)
            {
                labelInfo.Text = zoekinfo.ToString();
                labelInfo.Refresh();
                zoekinfo--;

                Regel regel = new Regel(Path.GetFileNameWithoutExtension(file.Name), type.PaginaNaam, "");
                InfoPagina.PaginaMetRegels.Add(regel);
            }
            SchermUpdate();

            labelPaginaInBeeld.Text = "Alle paginas";

        }
        private void vorigeVersiePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test of vorige versie bestaat
            string fi = labelPaginaInBeeld.Text;
            string opslagnaam = fi;
            string backup1 = fi + "_backup1";
            string backup2 = fi + "_backup2";

            BackupTerug BT = new BackupTerug();

            BT.PaginaNaam.Text = labelPaginaInBeeld.Text;
            BT.labelHuidig.Text = GetLaatsteEdit(opslagnaam);
            BT.labelBackup1.Text = GetLaatsteEdit(backup1);
            BT.labelBackup2.Text = GetLaatsteEdit(backup2);

            BT.ShowDialog();
            
            InfoPagina.Laad(fi);
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


                foreach (FileInfo file in files)
                {
                    _ = InfoPagina.Laad(Path.GetFileNameWithoutExtension(file.Name));
                    //if (ContainsCaseInsensitive(file.Name, ZF.textBoxZoek.Text))
                    //{
                    //    Regel regel = new Regel(Path.GetFileNameWithoutExtension(file.Name), type.PaginaNaam, "");
                    //    PaginaMetRegelsGevonden.Add(regel);
                    //}
                    for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
                    {
                        if (ContainsCaseInsensitive(InfoPagina.PaginaMetRegels[i].tekst_, ZF.textBoxZoek.Text)/* || ContainsCaseInsensitive(InfoPagina.PaginaMetRegels[i].url_, ZF.textBoxZoek.Text)*/)
                        {
                            Regel regel = new Regel($"Gevonden op pagina {file.Name}", type.TekstBlok, "");
                            PaginaMetRegelsGevonden.Add(regel);
                            regel = new Regel(InfoPagina.PaginaMetRegels[i].tekst_, InfoPagina.PaginaMetRegels[i].type_, InfoPagina.PaginaMetRegels[i].url_);
                            PaginaMetRegelsGevonden.Add(regel);
                            regel = new Regel("", type.Leeg, "");
                            PaginaMetRegelsGevonden.Add(regel);
                        }
                    }
                }
                // bouw Pagina
                labelPaginaInBeeld.Text = $"Zoek : {ZF.textBoxZoek.Text}";

                InfoPagina.PaginaMetRegels = PaginaMetRegelsGevonden;
                SchermUpdate();
                MessageBox.Show("Klaar met zoeken");
            }
        }
        private void zoekNaarWeesPaginasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // lijst met all pagina's opgeslagen.
            List<FileInfo> XMLFilesInDataDir = new DirectoryInfo("Data").EnumerateFiles("*.xml")
                            .OrderByDescending(f => f.Name)
                            .ToList();

            // maak lijst met de linken naar pagina's
            List<string> LinkNaarPaginaLijst = new List<string>();
            foreach (FileInfo file in XMLFilesInDataDir)
            {
                _ = InfoPagina.Laad(Path.GetFileNameWithoutExtension(file.Name));
                for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
                {
                    if (InfoPagina.PaginaMetRegels[i].type_ == type.PaginaNaam)
                    {
                        string linkNaarFile = InfoPagina.RemoveOudeWikiTekens(InfoPagina.PaginaMetRegels[i].tekst_);
                        if (!LinkNaarPaginaLijst.Contains(linkNaarFile))
                        {
                            LinkNaarPaginaLijst.Add(linkNaarFile);
                        }
                    }
                }
            }

            InfoPagina.PaginaMetRegels.Clear();

            foreach (FileInfo file in XMLFilesInDataDir)
            {
                // check of filenaam een item bevat wat geen file is
                string FileNaam = Path.GetFileNameWithoutExtension(file.Name);
                if (!LinkNaarPaginaLijst.Contains(FileNaam) && FileNaam != "Start")
                {
                    Regel regel = new Regel(FileNaam, type.PaginaNaam, "");
                    InfoPagina.PaginaMetRegels.Add(regel);
                }
            }
            labelPaginaInBeeld.Text = $"Wees Pagina's";
            SchermUpdate();
        }
        private void terugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (history.Count > 1)
            {
                string vorige_pagina = history[history.Count - 2];
                InfoPagina.Laad(vorige_pagina);
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
            
            if (InfoPagina.Laad(a.Text))
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
                    labelInfo.Text = $"Lees {file.Name}";
                    labelInfo.Refresh();
                    // laad file
                    _ = InfoPagina.Laad(Path.GetFileNameWithoutExtension(file.Name));
                    // verander
                    int change = InfoPagina.PaginaMetRegels.Count;
                    // door file heenstappen
                    for (int i = InfoPagina.PaginaMetRegels.Count - 1; i > 0; i--)
                    {
                        //bij import vanuit oude wiki is link naar dir gelijk aan link naar file.
                        // dus check of het geen file en geen dir is voordat ik verwijder

                        if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkFile)
                        {
                            if (!File.Exists(InfoPagina.PaginaMetRegels[i].url_))
                            {
                                if (!Directory.Exists((InfoPagina.PaginaMetRegels[i].url_)))
                                {
                                    _ = MessageBox.Show($"Remove link naar file \n{InfoPagina.PaginaMetRegels[i].tekst_}\nOp Pagina {Path.GetFileNameWithoutExtension(file.Name)}\n{InfoPagina.PaginaMetRegels[i].url_}");
                                    InfoPagina.PaginaMetRegels.RemoveAt(i);
                                }
                            }
                        }
                        else if (InfoPagina.PaginaMetRegels[i].type_ == type.LinkDir)
                        {
                            if (!Directory.Exists(InfoPagina.PaginaMetRegels[i].url_))
                            {
                                _ = MessageBox.Show($"Remove link naar dir \n{InfoPagina.PaginaMetRegels[i].tekst_}\nOp Pagina {Path.GetFileNameWithoutExtension(file.Name)}");
                                InfoPagina.PaginaMetRegels.RemoveAt(i);
                            }
                        }
                        else if (InfoPagina.PaginaMetRegels[i].type_ == type.PaginaNaam)
                        {
                            if (!File.Exists($"Data\\{Path.GetFileNameWithoutExtension(file.Name)}.xml"))
                            {
                                _ = MessageBox.Show($"Remove link naar Pagina \n{InfoPagina.PaginaMetRegels[i].tekst_}\nOp Pagina {Path.GetFileNameWithoutExtension(file.Name)}");
                                InfoPagina.PaginaMetRegels.RemoveAt(i);
                            }
                        }
                    }
                    if (change != InfoPagina.PaginaMetRegels.Count)
                    {
                        InfoPagina.Save(Path.GetFileNameWithoutExtension(file.Name));
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
            labelInfo.Text = "";
            int MakerInfoIndex = -1;
            int zoekinfo = InfoPagina.PaginaMetRegels.Count;

            for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
            {
                labelInfo.Text = zoekinfo.ToString();
                labelInfo.Refresh();
                zoekinfo--;

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
                if(InfoPagina.PaginaMetRegels[i].type_ == type.EditInfo)
                {
                    // bewaar locatie van info, wordt als laatse geplaatst op scherm.
                    MakerInfoIndex = i;
                }
            }
            labelInfo.Text = "";
            if(MakerInfoIndex > -1)
            {
                string dum = RandomString(10);
                int eigenaar = dum.GetHashCode();
                PlaatsTextOpBeeld(InfoPagina.PaginaMetRegels[MakerInfoIndex].tekst_, eigenaar);
                InfoPagina.PaginaMetRegels[MakerInfoIndex].eigenaar_ = eigenaar;
            }
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
            if(pagina == "Start")
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
            for (int i = history.Count - 1;  i > gevonden_pos; i--)
            {
                history.RemoveAt(i);
            }
        }

        private string GetLaatsteEdit(string file)
        {
            string ret = "";
            string opslagnaam = $"Data\\{file}.xml";
            if (File.Exists(opslagnaam))
            {
                InfoPagina.Laad(file);
                for (int i = 0; i < InfoPagina.PaginaMetRegels.Count; i++)
                {
                    if (InfoPagina.PaginaMetRegels[i].type_ == type.EditInfo)
                    {
                        ret = InfoPagina.PaginaMetRegels[i].tekst_;
                    }
                }
            }
            return ret;
        }
    }
}