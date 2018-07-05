using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class Tick //ESTO ES UN TICKET
    {
        [Key]
        [Required]
        public int TicketID { get; set; }

        [ForeignKey("EventWithTicket")]
        public int EventWithTicketID { get; set; }
        public virtual EventWithTicket EventWithTicket { get; set; }

        [Required]
        public string Description { get; set; }
    }
}