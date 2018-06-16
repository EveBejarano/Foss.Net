
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


        public IEnumerable<Bus> GetBuses(DateTime toDay, string fromPlace, string toPlace)
        {
            var getBusRequest = new GetBusRequest
            {
                fromPlace = fromPlace,
                toPlace = toPlace,
                toDay = toDay

            };

            BusCompany BusCompany = new BusCompany
            {
                Name = "Nombre",
                APIURLToGetSeats = "http://demo4736431.mockable.io/EvePost"
            };

            List<Bus> ListOfBus = new List<Bus>();

            List<BusResponse> ListOfBusesResponse = new List<BusResponse>();
            var consumerBuss = new Consumer<List<BusResponse>>();

            List<BusResponse> getBusResponse = consumerBuss.ReLoadEntities(BusCompany.APIURLToGetSeats, "GET", getBusRequest).Result;

            foreach (var item in getBusResponse)
            {
                var auxBus = new Bus
                {
                    IdAPI_Bus = item.Id,
                    Destination = item.Destination,
                    Origin = item.Origin,
                    DateTimeArrival = item.DateTimeArrival,
                    DateTimeDeparture = item.DateTimeDeparture,
                    Company = item.Company,
                    BusCompany = BusCompany,
                    Class = item.Class,
                    Capacity = item.Capacity,
                    Price = item.Price,
                    NotReservedSeats = item.Capacity
                };
                ListOfBus.Add(auxBus);
            }
            return ListOfBus;
        }

        //public IEnumerable<Hotel> GetHotels(string cP, DateTime fromDay, DateTime toDay)
        //{
        //    var getHotelsRequest = new GetHotelsRequest
        //    {

        //    };

        //    HotelCompany HotelsCompany = UnitOfWork.HotelCompanyRepository.Get().FirstOrDefault();

        //    List<Hotel> ListOfHotels = new List<Hotel>();

        //    var consumerHotelss = new Consumer<GetHotelsResponse>();

        //    GetHotelsResponse getHotelsResponse = consumerHotelss.ReLoadEntities(HotelsCompany.APIURLToGetHotel, "POST", getHotelsRequest).Result;

        //    foreach (var item in getHotelsResponse)
        //    {
        //        var auxHotels = new Hotel
        //        {

        //        };
        //        ListOfHotels.Add(auxHotels);
        //    }
        //    return ListOfHotels;
        //}



        //public IEnumerable<Flight> GetFlights(DateTime toDay, City fromPlace, City toPlace)
        //{
        //    var getFlightRequest = new GetFlightRequest
        //    {
        //        fromPlace = fromPlace,
        //        toPlace = toPlace,
        //        toDay = toDay

        //    };

        //    FlightCompany FlightCompany = UnitOfWork.FlightCompanyRepository.Get().FirstOrDefault();

        //    List<Flight> ListOfFlights = new List<Flight>();

        //    var consumerFlights = new Consumer<GetFlightResponse>();

        //    GetFlightResponse getFlightResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToGetSeats, "POST" , getFlightRequest).Result;

        //    foreach (var item in getFlightResponse)
        //    {
        //        var auxFlight = new Flight
        //        {

        //        };
        //        ListOfFlights.Add(auxFlight);
        //    }
        //    return ListOfFlights;
        //}
    }
}
