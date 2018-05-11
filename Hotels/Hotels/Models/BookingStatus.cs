using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class BookingStatus
    {
        [Key]
        public string StatusID { get; set; }
        
        public string StatusDescription { get; set; }

        public ICollection<Booking> Bookings { get; set; }

    }
}