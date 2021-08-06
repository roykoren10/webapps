using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace internetiot.Models
{
    public class RageRoom
    {
        public int Id { get; set; }

        [StringLength(30, MinimumLength = 1)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [MinLength(2)]
        [MaxLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Range(1, 100)]
        [Display(Name = "Min participants")]
        public int MinParticipants { get; set; }

        [Range(1, 100)]
        [Display(Name = "Max participants")]
        public int MaxParticipants { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Price per participant")]
        public int PricePerParticipant { get; set; }

        [Range(1, 100)]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [DataType(DataType.Url)]
        public string ImgUrl { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public Genre Genre { get; set; }
    }
}
