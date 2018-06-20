using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class ParametersTicket
    {
        public int EventWithTicketID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Price_Start { get; set; }
        public double Price_End { get; set; }
    }
}