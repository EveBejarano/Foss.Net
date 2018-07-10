using System;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.BusCompany;

namespace FunTourBusinessLayer.Service
{

    public class AuxBus
    {

        [Key]
        public int IdAPI_Bus { get; set; }

        public int TripID { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DateTimeDeparture { get; set; }
        public DateTime DateTimeArrival { get; set; }
        public string Company { get; set; }
        public string Class { get; set; }
        public int Capacity { get; set; }
        public float Price { get; set; }
        public int NotReservedSeats { get; set; }


        public virtual BusCompany BusCompany { get; set; }
    }
}