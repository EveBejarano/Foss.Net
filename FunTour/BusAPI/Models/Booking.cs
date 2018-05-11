using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusAPI.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }

        public int SeatID { get; set; }
        public int BusID { get; set; }
        public Seat Seat { get; set; }

        [ForeignKey("Trip")]
        public int TripID { get; set; }
        public virtual Trip Trip { get; set; }
    }
}