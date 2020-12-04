using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{

    public class WikipediaExtractRoot
    {
        public string batchcomplete { get; set; }
        public ExtractQuery query { get; set; }
    }

    public class ExtractQuery
    {
        public Pages pages { get; set; }
    }

    public class Pages
    {
        public _Page _page { get; set; }
    }

    public class _Page
    {
        public int pageid { get; set; }
        public int ns { get; set; }
        public string title { get; set; }
        public string extract { get; set; } // This is where the intro section's text is found. We're looking at @Model.query.pages._page.extract
    }

}
