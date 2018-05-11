using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class Transport
    {

        [Key]
        public int TransportID { get; set; }
        public string TransportType { get; set; }
        public int Capacity { get; set; }
        public bool Bathroom { get; set; }

    }
}