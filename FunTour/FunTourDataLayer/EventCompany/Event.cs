using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.TravelPackage = new HashSet<TravelPackage>();
            this.ReservedTicket = new HashSet<ReservedTicket>();
        }

        [Key]
        public int Id_Event { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string APIURLToTickets { get; set; }
        public string APIURLToReserveTicket { get; set; }
        public string APIURLToCancelReservation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelPackage> TravelPackage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservedTicket> ReservedTicket { get; set; }
    }
}