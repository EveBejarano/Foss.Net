using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Region
    {
        [Key]
        public int RegionID { get; set; }

        [Required]
        public string RegionName { get; set; }

        [Required]
        public string CountryID { get; set; }

        public Country Country { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}