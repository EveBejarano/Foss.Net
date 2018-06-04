using FunTourDataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunTourDataLayer.Models
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

        public string APIURLToGetTickets { get; set; }
        public string APIURLToReserveTicket { get; set; }
        public string APIURLToCancelReservation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Event { get; set; }

    }
}
