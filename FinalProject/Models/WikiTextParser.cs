using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp;
using System.Text.RegularExpressions;

namespace FinalProject.Models
{
    public class WikiTextParser
    {
        public List<string> Sections;
        public List<string> Paragraphs;

        IConfiguration config;
        IBrowsingContext context;

        WebClient client = new WebClient();

        public WikiTextParser()
        {
            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);

            Sections = new List<string>();
            Paragraphs = new List<string>();
        }

        public async Task<List<string>> GetParagraphs(string url)
        {
            string source = client.DownloadString(url);
            var HTML = await context.OpenAsync(req => req.Content(source));
            var paragraphs = HTML.GetElementsByTagName("p").ToList();
            List<string> paragraphList = paragraphs.Select(item => item.TextContent).ToList();

            return RemoveCitations(paragraphList);
        }

        public List<string> RemoveCitations(List<string> paragraphs)
        {
            for (int i = 0; i < paragraphs.Count; i++)
            {
                paragraphs[i] = RemoveCitations(paragraphs[i]);
            }

            return paragraphs;
        }

        public string RemoveCitations(string s)
        {
            return Regex.Replace(s, @"\[.*?\]", "");
        }
    } 
}
