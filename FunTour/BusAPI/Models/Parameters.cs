using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusAPI.Models
{
    public class Parameters
    {
        public DateTime Date { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}