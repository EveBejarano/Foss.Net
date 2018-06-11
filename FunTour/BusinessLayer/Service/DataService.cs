using BusinessLayer.UnitOfWorks;
using FunTourDataLayer.Locality;
using FunTourDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuntourBusinessLayer.Service
{
    public class DataService
    {
        public UnitOfWork UnitOfWork { get; set; }

        public IEnumerable<Flight> GetFlights(DateTime fromDay, string cP1, string cP2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetBuses(DateTime fromDay, string cP1, string cP2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hotel> GetHotels(string cP, DateTime fromDay, DateTime toDay)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetEvents(string cP, DateTime fromDay, DateTime toDay)
        {
            throw new NotImplementedException();
        }
    }
}
