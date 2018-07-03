using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.FlightCompany;

namespace FunTourDataLayer.Reservation
{
    public partial class ReservedSeat
    {

        [Key]
        public int Id_ReservedSeat { get; set; }

        public int Id_Flight { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual Flight Flight { get; set; }
    }
}