using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.Reservation;

namespace FunTourDataLayer.EventCompany
{
    public class EventCompany
    {

        public EventCompany()
        {
            this.Event = new HashSet<Event>();
        }

        [Key]
        public int Id_EventCompany { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string APIURLToGetEvents { get; set; }
        public string APIURLToReserveTickets { get; set; }

        public string APIURLToCancelReservation { get; set; }

        public string APIURLToReserveTicketByClient { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Event { get; set; }

    }
}
