using Melding;
using System;
using System.Collections.Generic;

namespace KenisBank
{
    public class Index
    {
        public string pagina { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public type type1 {  get; set; }

        public Index() { }

        public Index(string pagina, string text, string url, type type)
        { 
            this.pagina = pagina;
            this.text = text;
            this.url = url;
            this.type1 = type;
        }
    }
}
