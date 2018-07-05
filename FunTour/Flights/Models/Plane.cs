using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flights.Models
{
    public class Plane
    {
        [Key]
        public string idPlane { get; set; }
        public string namePlane { get; set; }
        [JsonIgnore]
        public float CrossingRange { get; set; }
        public int PlanePlaces { get; set; }

        [JsonIgnore]
        public ICollection<CommercialFlight> CommercialFlights { get; set; }
    }
}