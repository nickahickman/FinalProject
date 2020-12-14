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
        private readonly lrnrDBContext _context;
        private readonly TrendingTopicsDAL _dal;

        public HomeController(lrnrDBContext context, TrendingTopicsDAL dal)
        {
            _context = context;
            _dal = dal;
        }

        public async Task<IActionResult> Index()
        {
            List<string> topics = await _dal.GetTrendingTopicsAsync();

            return View(topics);
        }

        public async Task<IActionResult> FetchKeywords()
        {
            List<string> topics = await _dal.GetTrendingTopicsAsync();

            return View(topics);
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

        public async Task<IActionResult> BuildAudioFile(string subwiki, string title)
        {
            WikiTextParser wtp = new WikiTextParser();
            WikipediaParseRoot war = WikipediaDAL.ParseWikitext(subwiki, title);
            List<string> paragraphs = await wtp.GetParagraphs($"https://en.{subwiki}.org/?curid={war.parse.pageid}");
            string fileURL = await TextToSpeech.SynthesizeAudioAsync(String.Join(",", paragraphs), title);
            ViewBag.Title = war.parse.title;
            ViewBag.PageId = war.parse.pageid;
            ViewBag.FileURL = fileURL;

            //Audiofiles af = new Audiofiles();
            //af.SectionNumber = 1;
            //af.PageId = war.parse.pageid;
            //af.StorageAddress = fileURL;

            //if (ModelState.IsValid)
            //{
            //    _context.Audiofiles.Add(af);
            //    _context.SaveChanges();
            //}

            return RedirectToAction("ListenToArticle", "Home", new { fileURL });
        }

        public IActionResult ListenToArticle(string fileURL)
        {
            ViewBag.FileURL = fileURL;
            return View();
        }


        public async Task<IActionResult> ViewLinks(string subwiki, string title)
        {
            WikiTextParser wtp = new WikiTextParser();
            WikipediaParseRoot war = WikipediaDAL.ParseWikitext(subwiki, title);
            List<string> links = await wtp.GetArticleLinks($"https://en.wikipedia.org/?curid={war.parse.pageid}", title);
            ViewBag.Title = war.parse.title;
            ViewBag.PageId = war.parse.pageid;

            return View(links);
        }

        public IActionResult RelatedArticles(string subwiki, string title)
        {
            CategoriesRoot cr = WikipediaDAL.GetCategories(subwiki, title);
            Category[] catArray = cr.query.pages.page.categories; // PageID goofyness here

            List<Categorymember[]> catMemArrList = new List<Categorymember[]>();
            foreach (Category c in catArray)
            {
                string catTitle = c.title;
                if (catTitle.Contains("Articles"))
                {
                    continue;
                }
                else
                {
                    CategoryMembersRoot cmr = WikipediaDAL.GetCategoryMembers(subwiki, catTitle);
                    Categorymember[] cm = cmr.query.categorymembers;
                    catMemArrList.Add(cm);
                }
            }

            return View(catMemArrList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
