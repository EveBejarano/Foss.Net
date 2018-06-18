using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class RoomCount
    {
            public int HotelID { get; set; }
            public string HotelName { get; set; }
            public string HotelAddress { get; set; }
            public string HotelCity { get; set; }
            public string HotelRegion { get; set; }
            public string HotelCountry { get; set; }
            public decimal StandardRate { get; set; }
            public int FreeRoomCount { get; set; }
    }

}