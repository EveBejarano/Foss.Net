using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Flights.Models
{
    public class FlightPlace
    {
        [Key][Column(Order = 1)]
        public int numPlace { get; set; }
        public string Place_Owner_Name { get; set; }
        public int Place_Owner_DNI { get; set; }
        public System.DateTime FP_Date { get; set; }
        [ForeignKey ("CommercialFlight")][Key][Column(Order = 2)]
        public string idFlight { get; set; }

      
        public  CommercialFlight CommercialFlight { get; set; }
    }
}