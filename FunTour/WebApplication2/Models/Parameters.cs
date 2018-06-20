using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class Parameters
    {
        public string City { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public bool HasTickets { get; set; }
    }
}