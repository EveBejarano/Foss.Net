using System;
using System.Collections.Generic;

namespace FuntourBusinessLayer.Service
{
    internal class GetBusResponse
    {
        public List<BusResponse> Buses { get; set; }
    }

    public class BusResponse
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DateTimeDeparture { get; set; }
        public DateTime DateTimeArrival { get; set; }
        public string Company { get; set; }
        public string Class { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
    }
}