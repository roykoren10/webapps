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
    public class RageRoomsController : Controller
    {
        private readonly RageRoomsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public RageRoomsController(RageRoomsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RageRooms
        public async Task<IActionResult> Index(string roomName, int participants, int maxPrice, string genre)
        {
            var rooms = from r in _context.RageRooms
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

        // GET: RageRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var RageRoom = await _context.RageRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (RageRoom == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            return View(RageRoom);
        }

        public async Task<IActionResult> Order(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var RageRoom = await _context.RageRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (RageRoom == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            return View(RageRoom);
        }


        // GET: RageRooms/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: RageRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Description,MinParticipants,MaxParticipants,PricePerParticipant,Duration,ImgUrl,Genre")] RageRoom RageRoom)
        {
            string gname = ModelState.ToList().First(x => x.Key == "Genre.Name").Value.RawValue.ToString();
            var genre = _context.Genre.Where(g => g.Name == gname);
            if (genre.Count() != 0)
            {
                if (ModelState.IsValid)
                {
                    RageRoom.Genre = genre.ToList()[0];
                    _context.Add(RageRoom);
                    await _context.SaveChangesAsync();
                    FaceBookHandler.PostMessage(RageRoom);
                    return RedirectToAction(nameof(Index));
                }                

                return View(RageRoom);
            }
            else
            {
                return new NotFoundViewResult("NotFoundError", "Genre was not found");
            }

        }

        // GET: RageRooms/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var RageRoom = _context.RageRooms.Include(r => r.Genre).Where(r => r.Id == id);
            if (RageRoom.Count() == 0)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }
            return View(RageRoom.ToList()[0]);
        }

        // POST: RageRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Description,MinParticipants,MaxParticipants,PricePerParticipant,Duration,ImgUrl,Genre")] RageRoom RageRoom)
        {
            if (id != RageRoom.Id)
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
                        RageRoom.Genre = genre.ToList()[0];
                        _context.Update(RageRoom);
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
            return View(RageRoom);
        }

        // GET: RageRooms/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            var RageRoom = await _context.RageRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (RageRoom == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room was not found");
            }

            return View(RageRoom);
        }

        // POST: RageRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var RageRoom = await _context.RageRooms.FindAsync(id);
                _context.RageRooms.Remove(RageRoom);
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
            var result = this._context.Reservations.GroupBy(x => x.RageRoom.Name).Select(s =>
            new
            {
                RoomName = s.FirstOrDefault().RageRoom.Name,
                Profit = s.Sum(p => p.TotalPrice)
            }).OrderByDescending(x => x.Profit).Take(2).ToList();

            return Json(result);
        }

        // GET: RageRooms for statistics
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Statistics(string roomName, int participants, int maxPrice, string genre)
        {
            var rooms = from r in _context.RageRooms
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

        private bool RageRoomExists(int id)
        {
            return _context.RageRooms.Any(e => e.Id == id);
        }
    }
}
