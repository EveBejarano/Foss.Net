using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.Reservation
{
    public partial class ReservedRoom
    {

        [Key]
        public int Id_ReservedRoom { get; set; }
        public int HotelID { get; set; }
        public int BookingID { get; set; }

        public virtual Hotel.Hotel Hotel { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}