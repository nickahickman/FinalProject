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

        public IActionResult Index()
        {
            int highestID = _context.TrendingKeywords.Select(x => x.Id).Max();
            TrendingKeywords tK = _context.TrendingKeywords.Find(highestID);
            List<string> keyword = tK.Keyword.Split("|").ToList();

            DateTime Now = DateTime.Now;
            DateTime tkDate = (DateTime)tK.DatePulled;

            TimeSpan diff = Now - tkDate;
            double hours = diff.TotalHours;
            

            if(hours > 1)
            {                
                return RedirectToAction("GetTrendingKeywords");
            }
            else
            {
                return View(keyword);
            }

        }
        public async Task<IActionResult> GetTrendingKeywords()
        {
            List<string> topics = await _dal.GetTrendingTopicsAsync();
            string keywords = String.Join("|", topics);
            TrendingKeywords tk = new TrendingKeywords();

            tk.Keyword = keywords;

            if (ModelState.IsValid)
            {
                _context.TrendingKeywords.Add(tk);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
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
            List<string> links = await wtp.GetArticleLinks($"https://en.wikipedia.org/?curid={war.parse.pageid}", title);
            List<Audiofiles> matches = _context.Audiofiles.Where(x => x.PageId == war.parse.pageid).ToList();

            if (matches.Count == 0)
            {
                ViewBag.fileURL = null;
            }
            else 
            {
                ViewBag.fileURL = matches[0].StorageAddress;
            }

            ViewBag.Title = war.parse.title;
            ViewBag.PageId = war.parse.pageid;
            ViewBag.Links = links;

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

            Audiofiles af = new Audiofiles();
            af.SectionNumber = 1;
            af.PageId = war.parse.pageid;
            af.StorageAddress = fileURL;

            if (ModelState.IsValid)
            {
                _context.Audiofiles.Add(af);
                _context.SaveChanges();
            }

            return RedirectToAction("Wikiparse", "Home", new { subwiki, title });
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
