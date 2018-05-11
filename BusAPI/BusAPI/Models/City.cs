using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusAPI.Models
{
    public class City
    {
        public int CityID { get; set; }
        public string CityName { get; set; }

        public ICollection<Trip> OriginTrips { get; set; }
        public ICollection<Trip> DestinationTrips { get; set; }

    }
}