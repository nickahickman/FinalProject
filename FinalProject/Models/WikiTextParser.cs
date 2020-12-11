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

            RemovePhoeneticsAndCitations(elements);

            return elements;
        }

        public async Task<List<string>> GetArticleLinks(string url, string title)
        {
            List<string> links = new List<string>();
            string[] extraneousLinks = { "left", "right", "sic", "a", "b" };
            string source = client.DownloadString(url);
            var HTML = await context.OpenAsync(req => req.Content(source));
            var queriedElements = HTML.QuerySelectorAll("#mw-content-text i a").ToList();

            foreach (AngleSharp.Dom.IElement item in queriedElements)
            {
                if (!extraneousLinks.Contains(item.TextContent) && item.TextContent.Length > 1 && item.TextContent != title)
                {
                    if (!links.Contains(item.TextContent))
                    {
                        links.Add(item.TextContent);
                    }
                }
            }

            return links;
        }

        public List<string> RemovePhoeneticsAndCitations(List<string> paragraphs)
        {
            for (int i = 0; i < paragraphs.Count; i++)
            {
                paragraphs[i] = RemoveCitation(paragraphs[i]);
                paragraphs[i] = RemovePhoenetics(paragraphs[i]);
            }

            return paragraphs;
        }

        public string RemovePhoenetics(string s)
        {
            return Regex.Replace(s, @"\(/.*?\)", "");
        }

        public string RemoveCitation(string s)
        {
            return Regex.Replace(s, @"\[.*?\]", "");
        }
    }
}
