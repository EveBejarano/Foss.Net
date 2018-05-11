using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class StarRating
    {
        [Key]
        public int RatingID { get; set; }

        public byte[] RatingImage { get; set; }

        public ICollection<Hotel> Hotels { get; set; }
    }
}