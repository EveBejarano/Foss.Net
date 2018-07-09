using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.FlightCompany;

namespace FunTourDataLayer.Reservation
{
    public class ReservedSeat
    {

        [Key]
        public int Id_ReservedSeat { get; set; }

        public bool Available { get; set; }

        public virtual TravelPackage TravelPackage { get; set; }
    }
}