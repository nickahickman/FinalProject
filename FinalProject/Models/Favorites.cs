using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class Favorites
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string UserId { get; set; }
        public int? PageId { get; set; }
        public string Tags { get; set; }
        public bool? HasStarted { get; set; }
        public bool? HasFinished { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
