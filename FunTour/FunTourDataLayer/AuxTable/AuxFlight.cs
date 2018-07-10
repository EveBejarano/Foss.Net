using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.FlightCompany;

namespace FunTourBusinessLayer.Service
{

    public class Destination
    {
        public string nameAirport { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class AuxFlight
    {
        [Key]
        public int Id_Flight { get; set; }

        public int TripID { get; set; }
        public string Description { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivedDate { get; set; }
        public float Price { get; set; }
        public int NotReservedSeats { get; set; }
        public virtual FlightCompany FlightCompany { get; set; }
        
    }
}