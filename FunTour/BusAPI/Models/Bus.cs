using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusAPI.Models
{
    public class Bus
    {
        [Key]
        [Required]
        public int BusID { get; set; }
        public int Capacity { get; set; }
        public string Company { get; set; }
        public string Class { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}