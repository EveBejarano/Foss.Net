using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.BusCompany
{
    public partial class BusCompany
    {
        
        public BusCompany()
        {
            this.Bus = new HashSet<Bus>();
        }

        [Key]
        public int Id_BusCompany { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string APIURLToGetBuses { get; set; }
        public string APIURLToReserveSeatToTravelPackage { get; set; }
        public string APIURLToCancelReservation { get; set; }
        public string APIURLToReserveSeatByClient { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bus> Bus { get; set; }
    }
}