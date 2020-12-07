using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class WikipediaParseRoot
    {
        public Parse parse { get; set; }
    }

    public class Parse
    {
        public string title { get; set; }
        public int pageid { get; set; }
        public string wikitext { get; set; } // This is the target text that Nick's parsing method will take and "translate" into readable format.
    }

}
