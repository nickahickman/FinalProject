using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{

    public class CategoriesRoot
    {
        public CatContinue _continue { get; set; }
        public CatQuery query { get; set; }
    }

    public class CatContinue
    {
        public string clcontinue { get; set; }
        public string _continue { get; set; }
    }

    public class CatQuery
    {
        public CatPages pages { get; set; }
    }

    public class CatPages
    {
        public CatPage page { get; set; }
    }

    public class CatPage
    {
        public int pageid { get; set; }
        public int ns { get; set; }
        public string title { get; set; }
        public Category[] categories { get; set; }
    }

    public class Category
    {
        public int ns { get; set; }
        public string title { get; set; }
    }

}
