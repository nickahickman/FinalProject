using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class Audiofiles
    {
        public int Id { get; set; }
        public byte? SectionNumber { get; set; }
        public int? PageId { get; set; }
        public string StorageAddress { get; set; }
    }
}
