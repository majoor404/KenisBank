using Melding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace KenisBank
{
    public enum type
    {
        Hoofdstuk,
        LinkFile,
        LinkDir,
        TekstBlok,
        PaginaNaam,
        Leeg,
        EditInfo,
        Delete,
        Move,
        Toevoegen
    }

    [Serializable]
    public class RegelInXML // is een regel (record) in XML file
    {
        public RegelInXML() { }
        public RegelInXML(string tekst, type T, string url)
        {
            tekst_ = tekst;
            type_ = T;
            url_ = url;
            eigenaar_ = KennisMainForm.MaakID(); //  - 1; // koppeling panel naar data veld
        }
        public string tekst_ { set; get; }
        public type type_ { set; get; }
        public string url_ { set; get; }
        public int eigenaar_ { set; get; } // eigenaar is uniek nummer welk gelijk is tussen paneel en regel welk daarop leeft.
        public type undo_ { set; get; }
        public int index_ { set; get; }
        public int ID_ { set; get; }    // uniek nummer voor undo actie's


        public List<RegelInXML> LijstMetRegels = new List<RegelInXML>();
        public List<RegelInXML> LijstChangePaginaRegels = new List<RegelInXML>();

        public bool Laad(string file)
        {
            try
            {
                string fileNaam = $"Data\\{file}.xml";
                string xmlTekst = File.ReadAllText(fileNaam);

                if(xmlTekst.Length > 0 && xmlTekst.Contains("<Regel>")) 
                {
                    // omzetten na nieuwe class naam
                    string patroon = @"<Regel>";
                    string gewijzigdexmlText = Regex.Replace(xmlTekst, patroon, "<RegelInXML>", RegexOptions.IgnoreCase);
                    patroon = @"</Regel>";
                    gewijzigdexmlText = Regex.Replace(gewijzigdexmlText, patroon, "</RegelInXML>", RegexOptions.IgnoreCase);
                    patroon = @"<ArrayOfRegel";
                    gewijzigdexmlText = Regex.Replace(gewijzigdexmlText, patroon, "<ArrayOfRegelInXML", RegexOptions.IgnoreCase);
                    patroon = @"</ArrayOfRegel>";
                    gewijzigdexmlText = Regex.Replace(gewijzigdexmlText, patroon, "</ArrayOfRegelInXML>", RegexOptions.IgnoreCase);
                    
                    xmlTekst = gewijzigdexmlText;

                    File.WriteAllText(fileNaam, xmlTekst);
                }

                LijstMetRegels.Clear();
                LijstMetRegels = FromXML<List<RegelInXML>>(xmlTekst);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Save(string file)
        {
            if (file != "zijbalk")
            {
                string edit = $"Laatste edit door : {Environment.UserName} op {DateTime.Now}";
                bool al_edit_veld_aanwezig = false;
                // toevoegen EditInfo
                foreach (RegelInXML regel in LijstMetRegels)
                {
                    if (regel.type_ == type.EditInfo)
                    {
                        regel.tekst_ = edit;
                        regel.type_ = type.EditInfo;
                        al_edit_veld_aanwezig = true;
                    }
                }

                // eerste keer lege pagina dan aanmaken
                if (!al_edit_veld_aanwezig)
                {
                    RegelInXML r = new RegelInXML
                    {
                        type_ = type.EditInfo,
                        tekst_ = edit,
                        url_ = ""
                    };
                    LijstMetRegels.Add(r);
                }
            }
            try
            {
                string fi = VertaalNaarFileNaam(file);
                string opslagnaam = $"Data\\{fi}.xml";

                // save
                string xmlTekst = ToXML(LijstMetRegels);
                File.WriteAllText(opslagnaam, xmlTekst);

                FormMelding md = new FormMelding(FormMelding.Type.Save, "KennisBank", "Opslaan..");
                md.Show();
            }
            catch { }
        }
        public string VertaalNaarFileNaam(string pagina)
        {
            string resultaat = "";
            for (int i = 0; i < pagina.Length; i++)
            {
                char test = pagina[i];

                if (test > 127)
                {
                    resultaat += " ";
                }
                else
                {
                    resultaat += test;
                }
            }

            _ = resultaat.Trim();

            resultaat = resultaat.Replace(@" ", "_");
            resultaat = resultaat.Replace("\"", "");
            resultaat = resultaat.Replace(@"&", "");
            resultaat = resultaat.Replace(@"\", "");
            resultaat = resultaat.Replace(@"/", "");
            resultaat = resultaat.Replace(@"(", "");
            resultaat = resultaat.Replace(@")", "");
            resultaat = resultaat.Replace(@" ", "_");
            resultaat = resultaat.Replace(@"'", "_");
            resultaat = resultaat.Replace(@"__", "_");
            resultaat = resultaat.ToLower();

            return resultaat;

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
        private static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
