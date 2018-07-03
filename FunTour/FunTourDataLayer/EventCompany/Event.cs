using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.Reservation;

namespace FunTourDataLayer.EventCompany
{
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.ReservedTicket = new HashSet<ReservedTicket>();
            this.TravelPackage = new HashSet<TravelPackage>();
        }

        [Key]
        public int Id_Event { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int AvailableTickets{ get; set; }
        

        public virtual EventCompany EventCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelPackage> TravelPackage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservedTicket> ReservedTicket { get; set; }
        
    }
}