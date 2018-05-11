using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Required]
        public int RoomTypeID { get; set; }

        [Required]
        public string RoomNumber { get; set; }
        
        public string RoomDetails { get; set; }

        public RoomType RoomType { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }
}