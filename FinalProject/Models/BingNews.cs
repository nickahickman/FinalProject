using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class BingNewsRoot
    {
        public string _type { get; set; }
        public string readLink { get; set; }
        public Querycontext queryContext { get; set; }
        public int totalEstimatedMatches { get; set; }
        public Sort[] sort { get; set; }
        public Value[] value { get; set; }
    }

    public class Querycontext
    {
        public string originalQuery { get; set; }
        public bool adultIntent { get; set; }
    }

    public class Sort
    {
        public string name { get; set; }
        public string id { get; set; }
        public bool isSelected { get; set; }
        public string url { get; set; }
    }

    public class Value
    {
        public string name { get; set; }
        public string url { get; set; }
        public Image image { get; set; }
        public string description { get; set; }
        public About[] about { get; set; }
        public Provider[] provider { get; set; }
        public DateTime datePublished { get; set; }
        public string category { get; set; }
        public Mention[] mentions { get; set; }
    }

    public class Image
    {
        public Thumbnail thumbnail { get; set; }
    }

    public class Thumbnail
    {
        public string contentUrl { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class About
    {
        public string readLink { get; set; }
        public string name { get; set; }
    }

    public class Provider
    {
        public string _type { get; set; }
        public string name { get; set; }
        public Image1 image { get; set; }
    }

    public class Image1
    {
        public Thumbnail1 thumbnail { get; set; }
    }

    public class Thumbnail1
    {
        public string contentUrl { get; set; }
    }

    public class Mention
    {
        public string name { get; set; }
    }
}
