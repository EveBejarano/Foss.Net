using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class BookDTO
    {
        public int HotelID { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public int RoomCount { get; set; }
    }
}