using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using internetiot.Data;
using internetiot.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;

namespace internetiot.Controllers
{
    public class GenresController : Controller
    {
        private readonly EscapeRoomsContext _context;

        public GenresController(EscapeRoomsContext context)
        {
            _context = context;
        }

        // GET: Genres
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string genre)
        {
            var genres = from g in _context.Genre
                         select g;

            if (!String.IsNullOrEmpty(genre))
            {
                genres = genres.Where(s => s.Name.Equals(genre));
            }

            return View(await genres.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Genre Not Found");
            }

            var genre = await _context.Genre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return new NotFoundViewResult("NotFoundError", "Genre Not Found");
            }

            return View(genre);
        }

        [Authorize(Roles = "Admin")]
        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,MinAge")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Genre Not Found");
            }

            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return new NotFoundViewResult("NotFoundError", "Genre Not Found");
            }

            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,MinAge")] Genre genre)
        {
            if (id != genre.Id)
            {
                return new NotFoundViewResult("NotFoundError", "Genre Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    return new NotFoundViewResult("NotFoundError", "Genre Not Found");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Genre Not Found");
            }

            var genre = await _context.Genre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return new NotFoundViewResult("NotFoundError", "Genre Not Found");
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var genre = await _context.Genre.FindAsync(id);
                _context.Genre.Remove(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return new UnableToDeleteViewResult("UnableToDeleteError", "Unable to delete specified genre. There are reservations associated with that genre!");
            }
        }

        private bool GenreExists(int id)
        {
            return _context.Genre.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Admin")]
        public JsonResult GetRoomsPerGenreGraph()
        {

            return Json((from g in _context.Genre
                         join r in _context.EscapeRooms
                         on g.Id equals r.Genre.Id
                         select new
                         {
                             Genre = g.Name,
                             RoomName = r.Name

                         })
                     .GroupBy(x => x.Genre)
                     .Select(g => new { Genre = g.Key, NumOfRooms = g.Count() }));
        }

        public IActionResult GetGenreList()
        {
            return Json(_context.Genre.Select(g => g.Name).ToList());
        }
    }
}
