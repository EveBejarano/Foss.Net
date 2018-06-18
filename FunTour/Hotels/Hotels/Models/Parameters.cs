using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Parameters
    {
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public DateTime Date_start { get; set; }
            public DateTime Date_end { get; set; }
    }
}