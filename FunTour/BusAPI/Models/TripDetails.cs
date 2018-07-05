﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusAPI.Models
{
    public class TripDetails
    {
        public int TripID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DateTimeDeparture { get; set; }
        public DateTime DateTimeArrival { get; set; }
        public string Company { get; set; }
        public string Class { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
        public int AvailableSeats { get; set; }

    }
}