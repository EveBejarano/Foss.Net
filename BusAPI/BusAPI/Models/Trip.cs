using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusAPI.Models
{
    public class Trip
    {
        [Key]
        [Required]
        public int TripID { get; set; }

        [ForeignKey("Bus")]
        public int BusID { get; set; }
        public Bus Bus { get; set; }

        public int OriginID { get; set; }
        public City OriginCity { get; set; }

        public int DestinationID { get; set; }
        public City DestinationCity { get; set; }

        public double Price { get; set; }

        public DateTime DateTimeTrip { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}