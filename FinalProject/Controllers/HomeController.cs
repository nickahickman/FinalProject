using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult WikiSearch(string subwiki, string query)
        {
            WikipediaSearchRoot wsr = WikipediaDAL.SearchOnWiki(subwiki,query);
            ViewBag.SubWiki = subwiki;
            return View(wsr);
        }

        public async Task<IActionResult> WikiParse(string subwiki, string title)
        {
            WikiTextParser wtp = new WikiTextParser();
            WikipediaParseRoot war = WikipediaDAL.ParseWikitext(subwiki, title);
            List<string> paragraphs = await wtp.GetParagraphs($"https://en.{subwiki}.org/?curid={war.parse.pageid}");
            ViewBag.Title = war.parse.title;
            ViewBag.PageId = war.parse.pageid;
            if (subwiki.Equals("wikibooks"))
            {
                return View("WikiParseBook", paragraphs);
            } 
            else if (subwiki.Equals("wikinews"))
            {
                return View("WikiParseNews", paragraphs);
            }
            else
            {
                return View(paragraphs);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
