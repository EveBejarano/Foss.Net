using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.BusCompany;

namespace FunTourDataLayer.Reservation
{
        public partial class BusReservedSeat
        {
            [Key]   
            public int Id_BusReservedSeat { get; set; }
            public int IdAPI_Bus { get; set; }

            public int TripID { get; set; }
            public virtual Bus Bus { get; set; }
            public virtual Reservation Reservation { get; set; }
        }

}