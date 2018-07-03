using System;

namespace FunTourBusinessLayer.Service
{
    internal class GetEventRequest
    {
        public string City { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
    }
}