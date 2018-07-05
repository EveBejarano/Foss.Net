using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.Hotel
{
    public class HotelCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HotelCompany()
        {
            this.Hotels = new HashSet<Hotel>();
        }

        [Key]
        public int Id_HotelCompany { get; set; }


        public string Name { get; set; }

        public string Information { get; set; }

        public string APIURLToGetHotels { get; set; }
        public string APIURLToReserveRoomsToTravelPackage { get; set; }
        public string APIURLToReserveRoomByClient { get; set; }
        public string APIURLToCancelReservation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
