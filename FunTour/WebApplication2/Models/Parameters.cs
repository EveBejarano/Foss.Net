using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class Parameters
    {
        public int EventCompany { get; set; }
        public string City { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public int EventWithTicketsID { get; set; }
        public int TicketAmount { get; set; }
        public int DNI { get; set; }
        public double Price_Start { get; set; }
        public double Price_End { get; set; }
    }
}