using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace KenisBank
{
    public enum type
    {
        Hoofdstuk,
        LinkFile,
        LinkDir,
        TekstBlok,
        PaginaNaam,
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
                string xmlTekst = File.ReadAllText(file);
                PaginaMetRegels.Clear();
                PaginaMetRegels = FromXML<List<Regel>>(xmlTekst);
                return true;
            }
            catch {
                return false;
            }
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
