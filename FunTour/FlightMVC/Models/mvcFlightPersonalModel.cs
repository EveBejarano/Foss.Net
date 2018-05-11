using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightMVC.Models
{
    public class mvcFlightPersonalModel
    {
        public string Personal_Rol { get; set; }
        public string Flight { get; set; }
        public string Employ { get; set; }

        public virtual mvcCommercialFlightModel CommercialFlight { get; set; }
        public virtual mvcEmployeeModel Employee { get; set; }
    }
}
