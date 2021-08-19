using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using internetiot.Data;
using internetiot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace internetiot.Controllers
{
    public class AdminController : Controller
    {
        private readonly RageRoomsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(RageRoomsContext context, UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
            this._context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult MostProfitable()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public JsonResult RoomOrdersCountByMonth(int? id)
        {
            var result = this._context.Reservations.Where(r=>r.RageRoom.Id == id).AsEnumerable().GroupBy(x => x.StartingTime.Month).Select(s => 
            new { Month = s.First().StartingTime.Month, Count = s.Count(), Profit = s.Sum(p => p.TotalPrice) }).ToList();

            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetData()
        {
            _context.Reservations.RemoveRange(_context.Reservations);
            _context.RageRooms.RemoveRange(_context.RageRooms);
            _context.Genre.RemoveRange(_context.Genre);
            _context.Users.RemoveRange(_context.Users.Where(u=>u.Email != "Admin@gmail.com"));

            _context.SaveChanges();

            return Json("Data was successfuly saved!");
        }
    }
}