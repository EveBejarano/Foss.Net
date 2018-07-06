using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.AccountManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunTourDataLayer.Reservation
{
    public partial class Reservation
    {
        [Key]
        public int Id_Reservation { get; set; }

        [ForeignKey("TravelPackage")]
        public int Id_TravelPackage { get; set; }
        public virtual TravelPackage TravelPackage { get; set; }
        public virtual UserDetails Client { get; set; }
        public virtual ReservedRoom ReservedRoom { get; set; }
        public virtual ReservedSeat ReservedSeat { get; set; }
        public virtual BusReservedSeat BusReservedSeat { get; set; }
        public virtual ReservedTicket ReservedTicket { get; set; }
        public bool Paid { get; set; }
    }
}

