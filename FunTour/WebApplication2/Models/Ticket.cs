using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public double Price { get; set; }

        [ForeignKey("EventWithTicket")]
        public int EventWithTicketID { get; set; }
        public virtual EventWithTicket EventWithTicket { get; set; }

        [ForeignKey("Person")]
        public string DNI { get; set; }
        public virtual Person Person { get; set; }

    }
}