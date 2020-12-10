using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class TrendingRoot
    {
        [JsonPropertyName("hot topics")]
        public string[] hottopics { get; set; }
    }
}
