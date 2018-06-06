using BusinessLayer.UnitOfWorks;
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

        public IEnumerable<Flight> GetFlights(DateTime fromDay, string fromPlace, string toPlace)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetBuses(DateTime fromDay, string fromPlace, string toPlace)
        {
            throw new NotImplementedException();
        }
    }
}
