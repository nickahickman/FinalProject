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
            List<string> elements = new List<string>();
            string[] blockedSections = { "Notes", "References", "External links" };
            string source = client.DownloadString(url);
            var HTML = await context.OpenAsync(req => req.Content(source));
            var queriedElements = HTML.QuerySelectorAll(".mw-headline, #mw-content-text p").ToList();

            elements.Add("Chapter: Introduction");
            queriedElements.ForEach(item => {
                if (item.ClassName == "mw-headline")
                {
                    if (!blockedSections.Contains(item.TextContent))
                    {
                        elements.Add($"Chapter: {item.TextContent}");
                    }
                }
                else
                {
                    elements.Add(item.TextContent);
                }
            
            });

            return RemoveCitations(elements);
        }

        public List<string> RemoveCitations(List<string> paragraphs)
        {
            for (int i = 0; i < paragraphs.Count; i++)
            {
                paragraphs[i] = RemoveCitation(paragraphs[i]);
            }

            return paragraphs;
        }

        public string RemoveCitation(string s)
        {
            return Regex.Replace(s, @"\[.*?\]", "");
        }
    } 
}
