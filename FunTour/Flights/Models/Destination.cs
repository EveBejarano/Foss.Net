using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Flights.Models
{
    public class Destination
    {
        [Key]
        public string ZipCode { get; set; }
        public string idAirport { get; set; }        
        public string NameAirport { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        

        [JsonIgnore]
        [InverseProperty("To")]
        public ICollection<CommercialFlight> Froms { get; set; }
        [JsonIgnore]
        [InverseProperty("From")]
        public ICollection<CommercialFlight> Tos { get; set; }
    }
}