using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        public int AgentID { get; set; }

        [Required]
        public string StatusID { get; set; }

        [Required]
        public int GuestID { get; set; }

        [Required]
        public int RoomID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public Room Room { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public Guest Guest { get; set; }
        public Agent Agent { get; set; }



    }
}