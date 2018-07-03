using System.Collections.Generic;

namespace FunTourBusinessLayer.Service
{
    internal class GetFlightResponse
    {
        public List<CommercialFlight> CommercialFlights { get; set; }
    }


    public class Destination
    {
        public string nameAirport { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class CommercialFlight
    {

        public virtual Destination Destination { get; set; }
        public virtual Destination Destination1 { get; set; }

        public int idFlight { get; set; }

        public System.DateTime Deport { get; set; }
        public System.DateTime Arrive { get; set; }
        public float Price { get; set; }
        public int Disponible_Places { get; set; }
        public string Flight_To { get; set; }
        public string Flight_From { get; set; }


    }
}