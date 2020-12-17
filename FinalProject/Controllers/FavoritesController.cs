using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly lrnrDBContext _context;
        
        public FavoritesController(lrnrDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value; // Grabbing the Currently Loggin in Users Id (Primary Key)
            var f = _context.Favorites.Where(x => x.UserId == id).ToList(); // Matching Currently Logged in User to Database Entries.
            return View(f);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> AddFavorite(string Title, string Source, int PageId, string Article)
        {
            // Check if the Favorite already exists
            string userKey = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Favorites> userFavorites = _context.Favorites.Where(x => x.UserId == userKey).ToList();
            List<Favorites> checkTarget = userFavorites.Where(x => x.PageId == PageId).ToList();
            List<Audiofiles> matches = _context.Audiofiles.Where(x => x.PageId == PageId).ToList();

            if (matches.Count == 0)
            {
                string fileURL = await TextToSpeech.SynthesizeAudioAsync(Article, Title);

                Audiofiles af = new Audiofiles();
                af.SectionNumber = 1;
                af.PageId = PageId;
                af.StorageAddress = fileURL;

                if (ModelState.IsValid)
                {
                    _context.Audiofiles.Add(af);
                    _context.SaveChanges();
                }
            }

            // If it doesn't already exist, create a new one.
            if (checkTarget.Count == 0)
            {
                Favorites F = new Favorites();

                F.Title = Title;
                F.Source = Source;
                F.PageId = PageId;
                F.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                F.Tags = "";

                if (ModelState.IsValid)
                {
                    _context.Favorites.Add(F);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("ViewFavorites");
        }

        // READ
        [HttpGet]
        public IActionResult ViewFavorites()
        {
            string userKey = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Favorites> favorites = _context.Favorites.Where(x => x.UserId == userKey).ToList();
            return View(favorites);
        }

        [HttpPost]
        public IActionResult ViewFavorites(string sortTag)
        {
            string userKey = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Favorites> favorites = _context.Favorites.Where(x => x.UserId == userKey).ToList();

            List<Favorites> sortedFavs = new List<Favorites>(favorites);
            foreach(Favorites f in favorites)
            {
                if (f.Tags.Contains(sortTag))
                {
                    sortedFavs.Remove(f);
                    sortedFavs.Insert(0, f);
                }
            }

            return View(sortedFavs);
        }

        // UPDATE
        public IActionResult UpdateFavorite(int Id)
        {
            Favorites F = _context.Favorites.Find(Id);
            return View(F);
        }

        public IActionResult ApplyTagChanges(int Id, string Tags)
        {
            Favorites F = _context.Favorites.Find(Id);
            if (Tags == null)
            {
                F.Tags = "";
            }
            else
            {
                F.Tags = Tags.Trim();
            }
            _context.Favorites.Update(F);
            _context.SaveChanges();
            return RedirectToAction("ViewFavorites");
        }

        // DELETE
        public IActionResult DeleteFavorite(int pageId)
        {
            string userKey = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Favorites> userFavorites = _context.Favorites.Where(x => x.UserId == userKey).ToList();
            Favorites f = userFavorites.Where(x => x.PageId == pageId).ToList().First();
            _context.Favorites.Remove(f);
            _context.SaveChanges();

            return RedirectToAction("ViewFavorites");
        }
    }
}
