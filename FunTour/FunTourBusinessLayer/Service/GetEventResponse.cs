using System;

namespace FunTourBusinessLayer.Service
{
    internal class GetEventResponse
    {
        public int EventWithTicketID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CityName { get; set; }
        public string TrasportType { get; set; }
        public DateTime EventDate { get; set; }
        public string Address { get; set; }
        public float Price { get; set; }
        public int MaxTicket { get; set; }
    }
}