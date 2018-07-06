using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class EventWithTicketDTO
    {
        public int EventWithTicketID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Transport { get; set; }
        public DateTime EventDate { get; set; }
        public string Adress { get; set; }
        public double Price { get; set; }
        public int MaxTickets { get; set; }
    }
}