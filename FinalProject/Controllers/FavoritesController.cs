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
        [HttpPost]
        public IActionResult AddFavorite(string Title, string Source)
        {
            Favorites F = new Favorites();

            F.Title = Title;
            F.Source = Source;
            F.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                _context.Favorites.Add(F);
                _context.SaveChanges();
            }

            return RedirectToAction("ViewFavorites");
        }
        [HttpGet]
        public IActionResult ViewFavorites()
        {
            string userKey = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Favorites> favorites = _context.Favorites.Where(x => x.UserId == userKey).ToList();
            return View(favorites);
        }

        public IActionResult DeleteFavorite(int Id)
        {
            Favorites f = _context.Favorites.Find(Id);
            _context.Favorites.Remove(f);
            _context.SaveChanges();

            return RedirectToAction("ViewFavorites");
        }
    }
}
