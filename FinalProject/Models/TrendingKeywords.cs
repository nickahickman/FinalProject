using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class TrendingKeywords
    {
        public string Keyword { get; set; }
        public DateTime? DatePulled { get; set; }
        public int Id { get; set; }
    }
}
