using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class TicketDTO
    {
        public int TicketID { get; set; }
        public double Price { get; set; }
        public int EventWithTicketID { get; set; }
        public int PersonID { get; set; }
    }
}