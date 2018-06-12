using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Country
    {
        [Key]
        public string CountryID { get; set; }

        [Required]
        public string CountryName { get; set; }

        public string Currency { get; set; }
        public float ExchangeRate { get; set; }

        public ICollection<Hotel> Hotels { get; set; }
        public ICollection<Region> Regions { get; set; }
    }
}