using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace internetiot.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Display(Name ="Genre Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name="Minimum Age")]
        public int MinAge { get; set; } //incase we want to add minimum age for certain Rage rooms
    }
}
