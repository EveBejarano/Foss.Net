using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class TicketDTO
    {
        public int ReservaID { get; set; }
        public int EventWithTicketID { get; set; }
        public int PersonID { get; set; }
        public int DNI { get; set; }
        public double Price { get; set; }
    }
}