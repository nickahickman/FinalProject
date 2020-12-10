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

        public IActionResult RelatedArticles(string subwiki, string title)
        {
            CategoriesRoot cr = WikipediaDAL.GetCategories(subwiki, title);
            List<Category> catList = cr.query.pages.page.categories.ToList(); // PageID goofyness here

            List<Categorymember[]> catMemArrList = new List<Categorymember[]>();
            foreach (Category c in catList)
            {
                string catTitle = c.title;
                CategoryMembersRoot cmr = WikipediaDAL.GetCategoryMembers(subwiki, c.title);
                catMemArrList.Add(cmr.query.categorymembers);
            }

            return View("Index",catMemArrList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
