using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunTourDataLayer.FlightCompany;

namespace FunTourDataLayer.Reservation
{
    public class FlightReservedSeat : ReservedSeat
    {
        public virtual Reservation Reservation { get; set; }
        public virtual Flight Flight { get; set; }

    }
}
