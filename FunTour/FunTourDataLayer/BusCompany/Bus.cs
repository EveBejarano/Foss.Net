using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.Reservation;

namespace FunTourDataLayer.BusCompany
{
    public class Bus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bus()
        {
            this.BusReservedSeat = new HashSet<BusReservedSeat>();
            this.TravelPackage = new HashSet<TravelPackage>();
        }

        [Key]
        public int IdAPI_Bus { get; set; }

        public int TripID { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DateTimeDeparture { get; set; }
        public DateTime DateTimeArrival { get; set; }
        public string Company { get; set; }
        public string Class { get; set; }
        public int Capacity { get; set; }
        public float Price { get; set; }
        public int NotReservedSeats { get; set; }


        public virtual BusCompany BusCompany { get; set; }
        public virtual ICollection<BusReservedSeat> BusReservedSeat { get; set; }
  
        public virtual ICollection<TravelPackage> TravelPackage { get; set; }

    }
}