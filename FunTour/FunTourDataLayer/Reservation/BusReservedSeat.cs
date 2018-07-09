using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.BusCompany;

namespace FunTourDataLayer.Reservation
{
        public class BusReservedSeat: FlightReservedSeat
        {

            public int TripID { get; set; }
            public virtual Bus Bus { get; set; }
            public virtual Reservation Reservation { get; set; }

        }

}