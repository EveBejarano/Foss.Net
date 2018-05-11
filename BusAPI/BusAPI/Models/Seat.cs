using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusAPI.Models
{
    public class Seat
    {
        public int SeatID { get; set; }
        public int BusID { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}