using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using internetiot.Models;
using internetiot.Data;
using Microsoft.AspNetCore.Authorization;

namespace internetiot.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private RageRoomsContext _context;

        public HomeController(RageRoomsContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Displays a way to contact the company
        public IActionResult Contact()
        {
            ViewData["Message"] = "How can you contact us?\n";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? id)
        {
            if (id == 404)
            {
                return View("NotFound");
            }   
            
            return View();
        }
    }
}
