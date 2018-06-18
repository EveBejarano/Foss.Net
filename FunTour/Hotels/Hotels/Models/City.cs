using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public string RegionID { get; set; }


        public Region Region { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
    }
}
