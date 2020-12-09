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
        IConfiguration config;
        IBrowsingContext context;

        WebClient client = new WebClient();

        public WikiTextParser()
        {
            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
        }

        public async Task<List<string>> GetParagraphs(string url)
        {
            List<string> elements = new List<string>();
            string source = client.DownloadString(url);
            string[] extraneousSections = { "External links", "External links[edit]", "See Also", "References", "Notes" };
            var HTML = await context.OpenAsync(req => req.Content(source));
            var queriedElements = HTML.QuerySelectorAll("#mw-content-text h2, #mw-content-text h3, #mw-content-text p").ToList();

            elements.Add("Chapter: Introduction");

            for (int i = 0; i < queriedElements.Count - 1; i++)
            {
                var item = queriedElements[i];

                if (item.LocalName == "h2" && i < queriedElements.Count - 1 && item.Id != "mw-toc-heading")
                {
                    if (!extraneousSections.Contains(item.TextContent))
                    {
                        if (queriedElements[i + 1].LocalName == "p" || queriedElements[i + 1].LocalName == "h3")
                        {
                            elements.Add($"Chapter: {item.TextContent}");
                        }
                    }
                }
                else if (item.LocalName == "h3" && i < queriedElements.Count - 1)
                {
                    if (queriedElements[i + 1].LocalName == "p")
                    {
                        elements.Add($"Section: {item.TextContent}");
                    }
                }
                else if (item.LocalName == "p" && item.ClassName != "mw-empty-elt")
                {
                    elements.Add(item.TextContent);
                }
            }

            RemoveCitations(elements);

            return elements;
        }

        public List<string> RemoveHeadlinesForMissingChapters(List<string> elements)
        {
            List<string> filtered = new List<string>();

            for (int i = 0; i < elements.Count - 1; i++)
            {
                if (elements[i].StartsWith("Chapter:") || elements[i].StartsWith("Section:") && !elements[i + 1].StartsWith("Chapter:") && !elements[i + 1].StartsWith("Section:"))
                {
                    filtered.Add(elements[i]);
                }
                else if (!elements[i + 1].StartsWith("Chapter:") && !elements[i + 1].StartsWith("Section:"))
                {
                    filtered.Add(elements[i]);
                }
            }

            return filtered;
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
