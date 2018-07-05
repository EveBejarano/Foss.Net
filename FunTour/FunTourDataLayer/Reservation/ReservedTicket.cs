using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.EventCompany;

namespace FunTourDataLayer.Reservation
{
    public partial class ReservedTicket
    {

        [Key]
        public int Id_ReservedTicket { get; set; }

        public int Id_Event { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Event Event { get; set; }
    }
}