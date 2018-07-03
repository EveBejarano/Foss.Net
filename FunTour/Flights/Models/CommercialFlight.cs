using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Flights.Models
{
    public class CommercialFlight
    {

        [Key]
        public string IdFlight { get; set; }

        [JsonIgnore]
        public decimal Distance { get; set; }
        public System.DateTime Deport { get; set; }
        public System.DateTime Arrive { get; set; }
        public float Price { get; set; }
        public int Disponible_Places { get; set; }
        [ForeignKey("To")]
        public string FlightTo { get; set; }
        [ForeignKey("From")]
        public string FlightFrom { get; set; }
        [ForeignKey("Plane")]
        public string Flight_Plane { get; set; }

        public Destination To { get; set; }        
        public Destination From { get; set; }        
        public  Plane Plane { get; set; }


        [JsonIgnore]
        public ICollection<FlightPlace> FlightPlaces { get; set; }



    }
}