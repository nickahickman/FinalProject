using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{

    public class CategoryMembersRoot
    {
        public string batchcomplete { get; set; }
        public Continue _continue { get; set; }
        public Query query { get; set; }
    }

    public class CatMemContinue
    {
        public string cmcontinue { get; set; }
        public string _continue { get; set; }
    }

    public class CatMemQuery
    {
        public Categorymember[] categorymembers { get; set; }
    }

    public class Categorymember
    {
        public int pageid { get; set; }
        public int ns { get; set; }
        public string title { get; set; }
    }

}
