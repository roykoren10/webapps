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
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using internetiot.Handlers;

namespace internetiot.Controllers
{
    public class EscapeRoomsController : Controller
    {
        private readonly EscapeRoomsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public EscapeRoomsController(EscapeRoomsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EscapeRooms
        public async Task<IActionResult> Index(string roomName, int participants, int maxPrice, string genre)
        {
            var rooms = from r in _context.EscapeRooms
                        select r;

            if (!String.IsNullOrEmpty(roomName))
            {
                rooms = rooms.Where(s => s.Name.Contains(roomName));
            }

            if (participants > 0)
            {
                rooms = rooms.Where(s =>
                    (s.MinParticipants <= participants && s.MaxParticipants >= participants));
            }

            if (maxPrice > 0)
            {
                rooms = rooms.Where(s => s.PricePerParticipant <= maxPrice);
            }

            if (!String.IsNullOrEmpty(genre))
            {
                rooms = rooms.Where(s => s.Genre.Name.Equals(genre));
            }

            return View(await rooms.Include(r => r.Genre).ToListAsync());
        }

        // GET: EscapeRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var escapeRoom = await _context.EscapeRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (escapeRoom == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            return View(escapeRoom);
        }

        public async Task<IActionResult> Order(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var escapeRoom = await _context.EscapeRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (escapeRoom == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            return View(escapeRoom);
        }


        // GET: EscapeRooms/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: EscapeRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Description,MinParticipants,MaxParticipants,PricePerParticipant,Duration,ImgUrl,Genre")] EscapeRoom escapeRoom)
        {
            string gname = ModelState.ToList().First(x => x.Key == "Genre.Name").Value.RawValue.ToString();
            var genre = _context.Genre.Where(g => g.Name == gname);
            if (genre.Count() != 0)
            {
                if (ModelState.IsValid)
                {
                    escapeRoom.Genre = genre.ToList()[0];
                    _context.Add(escapeRoom);
                    await _context.SaveChangesAsync();
                    FaceBookHandler.PostMessage(escapeRoom);
                    return RedirectToAction(nameof(Index));
                }                

                return View(escapeRoom);
            }
            else
            {
                return new NotFoundViewResult("NotFoundError", "Genre was not found");
            }

        }

        // GET: EscapeRooms/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var escapeRoom = _context.EscapeRooms.Include(r => r.Genre).Where(r => r.Id == id);
            if (escapeRoom.Count() == 0)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }
            return View(escapeRoom.ToList()[0]);
        }

        // POST: EscapeRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Description,MinParticipants,MaxParticipants,PricePerParticipant,Duration,ImgUrl,Genre")] EscapeRoom escapeRoom)
        {
            if (id != escapeRoom.Id)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string gname = ModelState.ToList().First(x => x.Key == "Genre.Name").Value.RawValue.ToString();
                    var genre = _context.Genre.Where(g => g.Name == gname);
                    if (genre.Count() != 0)
                    {
                        escapeRoom.Genre = genre.ToList()[0];
                        _context.Update(escapeRoom);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return new NotFoundViewResult("NotFoundError", "Genre was not found");
                    }

                }
                catch (DbUpdateConcurrencyException)
                {

                    return new NotFoundViewResult("NotFoundError", "Room was not found");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(escapeRoom);
        }

        // GET: EscapeRooms/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var escapeRoom = await _context.EscapeRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (escapeRoom == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            return View(escapeRoom);
        }

        // POST: EscapeRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var escapeRoom = await _context.EscapeRooms.FindAsync(id);
                _context.EscapeRooms.Remove(escapeRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {

                return new UnableToDeleteViewResult("UnableToDeleteError", "Unable to delete specified room. There are reservations associated with that room!");
            }
        }

        
        [Authorize(Roles = "Admin")]
        public JsonResult MostProfitableRooms()
        {
            var result = this._context.Reservations.GroupBy(x => x.EscapeRoom.Name).Select(s =>
            new
            {
                RoomName = s.FirstOrDefault().EscapeRoom.Name,
                Profit = s.Sum(p => p.TotalPrice)
            }).OrderByDescending(x => x.Profit).Take(2).ToList();

            return Json(result);
        }

        // GET: EscapeRooms for statistics
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Statistics(string roomName, int participants, int maxPrice, string genre)
        {
            var rooms = from r in _context.EscapeRooms
                        select r;

            if (!String.IsNullOrEmpty(roomName))
            {
                rooms = rooms.Where(s => s.Name.Contains(roomName));
            }

            if (participants > 0)
            {
                rooms = rooms.Where(s =>
                    (s.MinParticipants <= participants && s.MaxParticipants >= participants));
            }

            if (maxPrice > 0)
            {
                rooms = rooms.Where(s => s.PricePerParticipant <= maxPrice);
            }

            if (!String.IsNullOrEmpty(genre))
            {
                rooms = rooms.Where(s => s.Genre.Name.Equals(genre));
            }

            return View(await rooms.Include(r => r.Genre).ToListAsync());
        }

        private bool EscapeRoomExists(int id)
        {
            return _context.EscapeRooms.Any(e => e.Id == id);
        }
    }
}
