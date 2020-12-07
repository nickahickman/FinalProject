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
        private readonly BingNewsDAL _dal;

        public HomeController(BingNewsDAL dal)
        {
            _dal = dal;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> NewsSearch(string Query)
        {
            string formattedQuery = _dal.BuildSearchQuery(Query);
            ViewBag.Query = formattedQuery;
            BingNewsRoot bnr = await _dal.SearchNewsStoriesAsync(formattedQuery);

            return View(bnr);
        }

        public IActionResult WikiSearch(string subwiki, string query)
        {
            WikipediaSearchRoot wsr = WikipediaDAL.SearchOnWiki(subwiki, query);
            ViewBag.SubWiki = subwiki;
            return View(wsr);
        }

        public IActionResult WikiParse(string subwiki, string title)
        {
            WikipediaParseRoot war = WikipediaDAL.ParseWikitext(subwiki,title);
            return View(war);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
