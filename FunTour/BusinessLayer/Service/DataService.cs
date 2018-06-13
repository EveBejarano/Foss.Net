using BusinessLayer.UnitOfWorks;
using FunTourDataLayer.Models;
using FunTourDataLayer.Models;
using FunTourDataLayer.Services;
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


        public IEnumerable<Bus> GetBuses(DateTime toDay, City fromPlace, City toPlace)
        {
            var getBusRequest = new GetBusRequest
            {
                fromPlace = fromPlace.Name,
                toPlace = toPlace.Name,
                toDay = toDay

            };

            BusCompany BusCompany = UnitOfWork.BusCompanyRepository.Get().FirstOrDefault();

            List<Bus> ListOfBus = new List<Bus>();

            var consumerBuss = new Consumer<GetBusResponse>();

            GetBusResponse getBusResponse = consumerBuss.ReLoadEntities(BusCompany.APIURLToGetSeats, "POST", getBusRequest).Result;

            foreach (var item in getBusResponse)
            {
                var auxBus = new Bus
                {

                };
                ListOfBus.Add(auxBus);
            }
            return ListOfBus;
        }

        public IEnumerable<Hotel> GetHotels(string cP, DateTime fromDay, DateTime toDay)
        {
            var getHotelsRequest = new GetHotelsRequest
            {

            };

            HotelCompany HotelsCompany = UnitOfWork.HotelCompanyRepository.Get().FirstOrDefault();

            List<Hotel> ListOfHotels = new List<Hotel>();

            var consumerHotelss = new Consumer<GetHotelsResponse>();

            GetHotelsResponse getHotelsResponse = consumerHotelss.ReLoadEntities(HotelsCompany.APIURLToGetHotel, "POST", getHotelsRequest).Result;

            foreach (var item in getHotelsResponse)
            {
                var auxHotels = new Hotel
                {

                };
                ListOfHotels.Add(auxHotels);
            }
            return ListOfHotels;
        }


    public IEnumerable<Event> GetEvents(string cP, DateTime fromDay, DateTime toDay)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Flight> GetFlights(DateTime toDay, City fromPlace, City toPlace)
        {
            var getFlightRequest = new GetFlightRequest
            {
                fromPlace = fromPlace.CP,
                toPlace = toPlace.CP,
                toDay = toDay

            };

            FlightCompany FlightCompany = UnitOfWork.FlightCompanyRepository.Get().FirstOrDefault();

            List<Flight> ListOfFlights = new List<Flight>();

            var consumerFlights = new Consumer<GetFlightResponse>();

            GetFlightResponse getFlightResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToGetSeats, "POST" , getFlightRequest).Result;

            foreach (var item in getFlightResponse)
            {
                var auxFlight = new Flight
                {

                };
                ListOfFlights.Add(auxFlight);
            }
            return ListOfFlights;
        }
    }
}
