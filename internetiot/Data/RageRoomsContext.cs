using internetiot.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace internetiot.Data
{
    public class RageRoomsContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<RageRoom> RageRooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Genre> Genre { get; set; }

        public RageRoomsContext(DbContextOptions<RageRoomsContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
       
    }
}
