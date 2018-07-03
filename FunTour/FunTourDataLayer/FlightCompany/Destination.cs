using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.FlightCompany
{
    public class Destination
    {
        [Key] public int Id { get; set; }
        public string nameAirport { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}