using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace KenisBank
{
    public partial class KennisMainForm : Form
    {
        Regel regels = new Regel();

        public KennisMainForm()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            
        }

        private void KennisMainForm_Shown(object sender, EventArgs e)
        {
            // zet panelen netjes
            KennisMainForm_Resize(this, null);
            // laad main.xml
            regels.Laad(@"Data\main.xml");
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            voegItemToaAanPaginaToolStripMenuItem.Enabled = editModeAanToolStripMenuItem.Checked;

            if(editModeAanToolStripMenuItem.Checked)
                panel1.BackColor = Color.LightCyan;
            else panel1.BackColor = SystemColors.Control;
        }

        private void buttonVoegToe_Click(object sender, EventArgs e)
        {
            ItemToevoegen itemToevoegen = new ItemToevoegen();
            itemToevoegen.ShowDialog();

            //// add HoofdstukText
            //AddHoofdstuk("Hoofdstuk tekst");

            //// add text
            //string tekst = "13-10-20 R.Docter diverse aanpassingen.\r\n14-10-20 eerste versie test.\r\n15-10-20 M.Kieft bug load personeel vanuit verleden.\r\n17-10-20 Geel Minimize knopje history venster.\r\n17-10-20 Geel Grijze balk van werkplek over gehele breedte.\r\n17-10-20 Geel History filter verbeterd.\r\n18-10-20 Extra diensten en aanvraag niet visible als niet gebruik.\r\n18-10-20 Tel niet mee in telling toegevoegd.\r\n19-10-20 M.Kieft Afwijkingen van ingelogd persoon telling gemaakt.\r\n21-10-20 Vonhoff bug bij verhuizing niet kunnen invullen.\r\n21-10-20 inlog\r\n21-10-20 Boots DD toegevoegd\r\n22-10-20 M.Kieft Afwijkingen van ingelogd persoon beter in excel.\r\n22-10-20 Haaften E-mail en Adres per kleur\r\n24-10-20 Netwerk save en load 30 keer proberen ivm traagheid.\r\n25-10-20 M.Kieft Telling Ploeg en Persoon er in gezet.\r\n28-10-20 Oude data inlezen vanaf 1-1 vorig jaar.\r\n28-10-20 Majaar Bij Edit personeel rechten worden oude rechten meegenomen.\r\n28-10-20 Haaften Optie VRIJ.\r\n28-10-20 Haaften Als na laatste nacht een OPLO/K , Dan vraag VRIJ nacht er voor.\r\n29-10-20 About versie text in versie.ini file.\r\n29-10-20 Bug in edit personeel isw met filter kleur.\r\n30-10-20 Bezetting lijst save 30 pogingen.\r\n30-10-20 Boots Scroll Windows als klein scherm.\r\n31-10-20 Backup van files 1 keer per dag.\r\n02-11-20 Boots Invoer reeks X maal met interval Y.\r\n03-11-20 bug verander file bij nieuwe aanmaak maand.\r\n03-11-20 history verbeterd.\r\n04-11-20 Boots Melding verkeerd wachtwoord.\r\n04-11-20 Boots Bug ver in toekomst invullen.\r\n04-11-20 Tooltip.\r\n05-11-20 Den Blanken Verbetering nacht ervoor vrij.\r\n06-11-20 Diverse.\r\n07-11-20 Heesterbeek diverse labels/teksten anders.\r\n07-11-20 Dialoog als form dialoog ipv sizeable.\r\n07-11-20 History filter alleen op Datum.\r\n09-11-20 Heesterbeek ED/VD/RD in wachtoverzicht werkplek locatie.\r\n19-11-20 Heesterbeek kleur code in wachtoverzicht bij afwijking.\r\n22-11-20 Alleen uppercase invoeren afwijking.\r\n22-11-20 Source geoptimaliseerd door Visual studio.\r\n22-11-20 EV ook kleur in wachtoverzicht.\r\n23-11-20 Copy funtie in quickmenu invullen afwijking.\r\n23-11-20 Boots Copy afwijking dag door naar afwijking reeks.\r\n25-11-20 Heesterbeek Start wacht overzicht formulier 2 dagen.\r\n25-11-20 Admin WW aangepast aan oude wachtrapport rep tool.\r\n26-11-20 Brasser Screen shot bug wachtmail.\r\n26-11-20 Bug verhuis tool.\r\n26-11-20 Brasser lijntjes naar boven verplaatst.\r\n28-11-20 Heesterbeek Wachtoverzicht optie form 2 dagen.\r\n30-11-20 VK1 VK2 VK3 enz in kleur in overzicht.\r\n01-12-20 Test Netwerk voordat wijzeging of nieuwe bezetting maken.\r\n02-12-20 11 maanden ingevulde wijzegingen worden meegenomen bij verhuizing.\r\n03-12-20 den Blanken Bug geen vuilwerk bij ED/RD/VD opgelost.\r\n05-12-20 Cancel verhuis data terug zetten.\r\n06-12-20 den Blanken Auto inlog gebeuren.\r\n06-12-20 personeel kleur als dropdown ipv vrije tekst.\r\n10-12-20 in form balk ingelogde naam en personeel nummer.\r\n10-12-20 Na wijzigen rechten meteen update in personeel form.\r\n13-12-20 Bug invoeren access database.\r\n13-12-20 Kieft Bug screen bevroren na opstarten.\r\n20-12-20 Verbeterde invoer oude database oude bezetting.\r\n22-12-20 Zaaijer Bug invoer nieuwe namen met passwoord leeg.\r\n22-12-20 Bij rechten > 0 aanpassen en passwoord leeg, passwoord reset.\r\n22-12-20 Bij auto inlog check of persoon nog bestaat, anders verwijderen.\r\n22-12-20 Test auto rechten bij invoer vanuit oude programma.\r\n23-12-20 Zaaijer/Visser Opmerkingen in dag/wacht formulier.\r\n25-12-20 Kieft Bug wachtoverzicht volgende/vorige bij jaar overgang.\r\n26-12-20 5 ploegen data kleur en dag keuze in losse dll gezet.\r\n29-12-20 Bug opslag maand jpg.\r\n02-01-21 Bug opstart bij nieuw jaar als kleur niet bekend was.\r\n04-01-21 Bij meer dan 1 extra dienst, deze alle in popup in maand overzicht.\r\n07-01-21 Digitaal sign ivm opstart melding tatasteeleurope.\r\n10-01-21 Test of kill.ini meer dan 30 min oud is, verwijder deze dan.\r\n10-01-21 Test Lees en Schrijf toegang voordat we opstarten.\r\n12-01-21 Boots Mogelijkheid tot aanpassen pop-up menu.\r\n12-01-21 Boots Optie om oude data in te lezen vanaf huidige datum, of hele jaar.\r\n17-01-21 Auto inlog verbeterd.\r\n22-01-21 Blanken Bug bij naam veranderen van peroneel.\r\n22-01-21 Lagerwey Bug bij invullen DD man bij 5ploegendienst rooster.\r\n23-01-21 Lagerwey Bij extra text aanwezig in wachtoverzicht, knopje geel.\r\n25-01-21 Lagerwey Knopje 2de dag in wachtoverzicht met kleur.\r\n25-01-21 Lagerwey/Docter keuze datum op wachtoverzicht formulier.\r\n25-01-21 Bij import vanuit oude access database ook verwijdering afwijking meenemen.\r\n29-01-21 Reperatie bezetting met wijzeging file verbeterd.\r\n29-01-21 Bug opmerkingen plaatsen 2de dag wachtoverzicht.\r\n01-02-21 Toevoegen export maandoverzicht naar Excel.\r\n01-02-21 Check of directory Backup bestaat, anders aanmaken.\r\n02-02-21 Lagerwey alle hulpfiles in andere directory, BezData.\r\n02-02-21 Bij import vanuit oude access database ook verwijdering afwijking meenemen(2).";
            
            //AddText(tekst);

            //// add HoofdstukText
            //AddHoofdstuk("Hoofdstuk laast");

            //AddLink("Dit is een Link", "C:\\Source\\C#");

        }

        private Panel MaakNewPanel()
        {
            // maak new panel
            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.BorderStyle = BorderStyle.None;
            panel.AutoSize = true;
            panelMain.Controls.Add(panel);
            panelMain.Controls.SetChildIndex(panel, 0);

            //panel.Click += panel_Click();


            return panel;
        }

        private void AddHoofdstuk(string text)
        {
            Panel panel = MaakNewPanel();

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold);
            label.Text = "   " + text;
            panel.Controls.Add(label);
        }

        private void AddText(string tekst)
        {
            Panel panel = MaakNewPanel();

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            label.Text = tekst;
            // tel aantal regels.
            int regels = 0;
            for (int i = 0; i < tekst.Length; i++)
                if (tekst[i] == '\r') regels++;
            panel.Controls.Add(label);
        }

        private void AddLink(string link, string locatie)
        {
            Panel panel = MaakNewPanel();

            System.Windows.Forms.LinkLabel label = new System.Windows.Forms.LinkLabel();
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            label.Text = link;
            label.Tag = locatie;
            
            label.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(label_LinkClicked);

            panel.Controls.Add(label);
        }

        void label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.LinkLabel label = (System.Windows.Forms.LinkLabel)sender;
            label.LinkVisited = true;
            Process process = new Process();
            process.StartInfo.FileName = (string)label.Tag;
            process.Start();
        }

        private void KennisMainForm_Resize(object sender, EventArgs e)
        {
            panel1.Width = this.Width - 45;
            panelMain.Width = this.Width - 45;
            panelMain.Height = this.Height - 150;
        }

        private void editPaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
