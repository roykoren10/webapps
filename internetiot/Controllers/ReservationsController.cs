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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace internetiot.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RageRoomsContext _context;

        public ReservationsController(RageRoomsContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        [Authorize]
        public async Task<IActionResult> Index(string roomName, int participants, int totalPrice, string emailAddress)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            var allReservations = (from res in _context.Reservations
                                   join r in _context.RageRooms
                                   on res.RageRoom.Id equals r.Id
                                   join u in _context.Users
                                   on res.User.Id equals u.Id

                                   select new Reservation
                                   {
                                       Id = res.Id,
                                       RageRoom = r,
                                       Participants = res.Participants,
                                       StartingTime = res.StartingTime,
                                       TotalPrice = res.TotalPrice,
                                       User = u
                                   });

            if (!User.IsInRole("Admin"))
            {
                allReservations = allReservations.Where(r => r.User.Id == user.Id);
            }
            else if (!String.IsNullOrEmpty(emailAddress))
            {
                allReservations = allReservations.Where(s => s.User.Email.Contains(emailAddress));
            }

            if (!String.IsNullOrEmpty(roomName))
            {
                allReservations = allReservations.Where(r => r.RageRoom.Name.Contains(roomName));
            }

            if (participants > 0)
            {
                allReservations = allReservations.Where(r => r.Participants == participants);
            }

            if (totalPrice > 0)
            {
                allReservations = allReservations.Where(r => r.TotalPrice <= totalPrice);
            }

            return View(await allReservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Reservation was not found");
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return new NotFoundViewResult("NotFoundError", "Reservation was not found");
            }
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            // Checks if the user is system administrator
            if (User.IsInRole("Admin"))
            {
                return View(reservation);
            }
            // Checks if the user wants to view his own reservation
            else if (reservation.User.Id == user.Id)
            {
                return View(reservation);
            }
            else
            //return Json("access denied");
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        // GET: Reservations/Create/1
        [Authorize]
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Room to reserve was not found");
            }
            else
            {
                var room = _context.RageRooms.FirstOrDefault(r => r.Id == id);
                if (room == null)
                {
                    return new NotFoundViewResult("NotFoundError", "Room to reserve was not found");
                }
                else
                {
                    ViewBag.room = room;
                    return View();
                }
            }

        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(int participants, string date, string time, int roomId)
        {
            RageRoom room = _context.RageRooms.Find(roomId);
            string[] dateSplited = date.Split(".");
            int day = int.Parse(dateSplited[0]);
            int month = int.Parse(dateSplited[1]);
            int year = int.Parse(dateSplited[2]);

            string[] timeSplitted = time.Split(":");
            int hour = int.Parse(timeSplitted[0]);
            int minute = int.Parse(timeSplitted[1]);

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Reservation newReservation = new Reservation()
            {
                Participants = participants,
                StartingTime = new DateTime(year, month, day, hour, minute, 0),
                TotalPrice = participants * room.PricePerParticipant,
                RageRoom = room,
                User = user
            };

            var a = _context.Add(newReservation);
            await _context.SaveChangesAsync();
            return Json(newReservation.Id);
        }

        // GET: Reservations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Reservation was not found");
            }

            var reservation = await _context.Reservations.Include(r => r.RageRoom).Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return new NotFoundViewResult("NotFoundError", "Reservation was not found");
            }

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (reservation.User.Id == user.Id || User.IsInRole("Admin"))
            {
                ViewBag.room = reservation.RageRoom;
                return View(reservation);
            }
            else
            {
                return new ForbiddenViewResult("PermissionDeniedError");
            }
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int participants, string date, string time, int reservationId)
        {
            if (reservationId == null || _context.Reservations.Find(reservationId) == null)
            {
                return new NotFoundViewResult("NotFoundError", "Reservation was not found");
            }

            var reservation = _context.Reservations.Include(r => r.RageRoom).Include(r => r.User).First(r => r.Id == reservationId);

            string[] dateSplited = date.Split(".");
            int day = int.Parse(dateSplited[0]);
            int month = int.Parse(dateSplited[1]);
            int year = int.Parse(dateSplited[2]);

            string[] timeSplitted = time.Split(":");
            int hour = int.Parse(timeSplitted[0]);
            int minute = int.Parse(timeSplitted[1]);

            reservation.Participants = participants;
            reservation.StartingTime = new DateTime(year, month, day, hour, minute, 0);
            reservation.TotalPrice = participants * reservation.RageRoom.PricePerParticipant;

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.Id == reservation.User.Id || User.IsInRole("Admin"))
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    return Json(reservationId);
                }
                catch (DbUpdateConcurrencyException)
                {

                    return new NotFoundViewResult("NotFoundError", "Reservation was not found");
                }
            }
            else
            {
                return new ForbiddenViewResult("PermissionDeniedError");
            }
        }

        // GET: Reservations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return new NotFoundViewResult("NotFoundError", "Reservation was not found");
            }

            var reservation = await _context.Reservations.Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return new NotFoundViewResult("NotFoundError", "Reservation was not found");
            }
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (reservation.User.Id == user.Id || User.IsInRole("Admin"))
            {
                return View(reservation);
            }
            else
            {
                return new ForbiddenViewResult("PermissionDeniedError");
            }

        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var reservation = await _context.Reservations.FindAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            // Checks if the user is system administrator
            if (User.IsInRole("Admin"))
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); ;
            }
            // Checks if the user wants to delete his own reservation
            else if (reservation.User.Id == user.Id)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
                return Json("Permission denied");
        }


        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="day">for example 13.10.2018</param>
        /// <returns></returns>
        public List<DateTime> GetAvailableTimes(int roomId, string day, int? reservationId)
        {
            // Gets the room object
            RageRoom room = _context.RageRooms.Find(roomId);

            List<DateTime> hours = GenerateHoursList(room.Duration, day);
            List<Reservation> existingReservations = _context.Reservations.Where(r => r.RageRoom.Id == roomId).ToList();

            if (reservationId.HasValue)
            {
                existingReservations = existingReservations.Where(r => r.Id != reservationId.Value).ToList();
            }

            // TODO check if works when we add data
            existingReservations.ForEach(rs => hours.Remove(rs.StartingTime));

            return hours;
        }

        [Authorize]
        private List<DateTime> GenerateHoursList(int gameLength, string day)
        {
            List<DateTime> output = new List<DateTime>();
            string[] daySplitted = day.Split('.');

            // 9 is the opening hour
            DateTime curr = new DateTime(int.Parse(daySplitted[2]), int.Parse(daySplitted[1]), int.Parse(daySplitted[0]), 9, 0, 0);
            while (curr.Day == int.Parse(daySplitted[0]))
            {
                output.Add(curr);
                curr = curr.AddHours(gameLength);
            }

            return output;
        }

    }
}
