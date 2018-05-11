using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Agent
    {
        [Key]
        public int AgentID { get; set; }

        public string AgentName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string AgentEmail { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}