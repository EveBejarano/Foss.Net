using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Hotel
    {
        [Key]
        public int HotelID { get; set; }
           
        public string ChainId { get; set; }
        public string CountryID { get; set; }
        public int RatingID { get; set; }

        [Required]
        public string HotelName { get; set; }

        public string HotelAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        public string HotelEmail { get; set; }

        public string HotelWebsite { get; set; }
        public string HotelDetails { get; set; }

        [Required]
        public string HotelCity { get; set; }

        public HotelChain HotelChain { get; set; }
        public StarRating StarRating { get; set; }
        public Country Country { get; set; }
        public virtual ICollection<RoomType> RoomTypes { get; set; }

    }
}