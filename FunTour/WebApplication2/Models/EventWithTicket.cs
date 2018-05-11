using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class EventWithTicket
    {
        [Key]
        [Required]
        public int EventWithTicketID { get; set; }

        [ForeignKey("City")]
        public int CityID { get; set; }
        public virtual City City { get; set; }

        [ForeignKey("Transport")]
        public int TransportID { get; set; }
        public virtual Transport Transport { get; set; }

        public DateTime EventDate { get; set; }

        [Required]
        public string Description { get; set; }
  
        public string Addres { get; set; }
        public bool HasTickets { get; set; }
        public int MaxTicket { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}