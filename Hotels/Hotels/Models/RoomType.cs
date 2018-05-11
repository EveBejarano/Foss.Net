using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeID { get; set; }

        [Required]
        public int HotelID { get; set; }

        [Required]
        public string RoomDescription { get; set; }

        [Required]
        public decimal StandardRate { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public Hotel Hotel { get; set; }


    }
}
