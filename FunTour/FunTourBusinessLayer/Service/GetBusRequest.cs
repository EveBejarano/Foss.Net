using System;

namespace FunTourBusinessLayer.Service
{
    internal class GetBusRequest
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}