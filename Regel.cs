using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        EditInfo
    }

    [Serializable]
    public class Regel
    {
        public Regel() { }
        public Regel(string tekst, type T, string url)
        {
            tekst_ = tekst;
            type_ = T;
            url_ = url;
            eigenaar_ = -1; // koppeling panel naar data veld
        }
        public string tekst_ { set; get; }
        public type type_ { set; get; }
        public string url_ { set; get; }
        public int eigenaar_ { set; get; }


        public List<Regel> PaginaMetRegels = new List<Regel>();

        public bool Laad(string file)
        {
            try
            {
                string xmlTekst = File.ReadAllText($"Data\\{file}.xml");
                PaginaMetRegels.Clear();
                PaginaMetRegels = FromXML<List<Regel>>(xmlTekst);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save(string file)
        {
            string edit = $"Laatste edit door : {Environment.UserName} op {DateTime.Now.ToString()}";
            bool al_edit_veld_aanwezig = false;
            // toevoegen EditInfo
            foreach(Regel regel in PaginaMetRegels)
            {
                if(regel.type_ == type.EditInfo)
                {
                    regel.tekst_ = edit;
                    regel.type_ = type.EditInfo;
                    al_edit_veld_aanwezig = true;
                }
            }
            
            // eerste keer lege pagina dan aanmaken
            if(!al_edit_veld_aanwezig)
            {
                Regel r = new Regel();
                r.type_ = type.EditInfo;
                r.tekst_ = edit;
                r.url_ = "";
                PaginaMetRegels.Add(r);
            }
            
            try
            {
                string fi = RemoveOudeWikiTekens(file);
                string opslagnaam = $"Data\\{fi}.xml";
                // backup
                MaakBackUpFile(fi);

                // save
                string xmlTekst = ToXML(PaginaMetRegels);
                File.WriteAllText(opslagnaam, xmlTekst);
            }
            catch { }
        }

        public string RemoveOudeWikiTekens(string pagina)
        {
            string ret = pagina;
            ret = ret.Trim();
            ret = ret.Replace(@"&", "");
            ret = ret.Replace(@"\", "");
            ret = ret.Replace(@"/", "");
            ret = ret.Replace(@"(", "");
            ret = ret.Replace(@")", "");
            ret = ret.Replace(@" ", "_");
            ret = ret.Replace(@"'", "_");
            ret = ret.Replace(@"__", "_");

            int pos = ret.IndexOf('"');
            if (pos > -1)
                ret = ret.Substring(0, pos) + ret.Substring(pos+1);
            pos = ret.IndexOf('"');
            if (pos > -1)
                ret = ret.Substring(0, pos) + ret.Substring(pos + 1);


            return ret.ToLower();
        }

        private static void MaakBackUpFile(string fi)
        {
            string opslagnaam = $"Data\\{fi}.xml";
            string backup1 = $"Data\\{fi}.xm1";
            string backup2 = $"Data\\{fi}.xm2";
                        
            // van 1 naar 2
            if(File.Exists(backup1))
                File.Copy(backup1, backup2,true);    
            //van org naar 1
            if(File.Exists(opslagnaam))
                File.Copy(opslagnaam, backup1,true);
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
