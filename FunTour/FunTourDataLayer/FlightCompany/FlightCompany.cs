using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.FlightCompany
{
    public partial class FlightCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FlightCompany()
        {
            this.Flight = new HashSet<Flight>();
        }

        [Key]
        public int Id_FlightCompany { get; set; }


        public string Name { get; set; }

        public string Information { get; set; }

        public string APIURLToGetFlights { get; set; }
        public string APIURLToReserveSeatsToTravelPackage { get; set; }
        public string APIURLToReserveSeatByClient { get; set; }
        public string APIURLToCancelReservation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Flight> Flight { get; set; }
    }
}