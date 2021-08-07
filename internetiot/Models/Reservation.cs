using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace internetiot.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [Display(Name = "Participants")]
        public int Participants { get; set; }
        [Display(Name = "Total price")]
        public float TotalPrice { get; set; }
        [Display(Name = "Starting time")]
        public DateTime StartingTime { get; set; }
        public RageRoom RageRoom { get; set; }
        public ApplicationUser User { get; set; }
    }
}
