using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace KenisBank
{
    internal enum type
    {
        PaginaNaam,
        Link,
        Hoofdstuk
    }

    [Serializable]
    public class Regel
    {
        public List<Regel> ListRegels = new List<Regel>();
        public Regel() { }
        public Regel(string tekst, int type)
        {
            tekst_ = tekst;
            type_ = type;
        }
        public string tekst_ { set; get; }
        public int type_ { set; get; }

        public void Laad(string file)
        {
            try
            {
                string xmlTekst = File.ReadAllText(file);
                ListRegels.Clear();
                ListRegels = FromXML<List<Regel>>(xmlTekst);
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
