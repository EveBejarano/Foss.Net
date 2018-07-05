using System;
using System.Collections.Generic;
using System.Linq;
using FunTourBusinessLayer.UnitOfWorks;
using FunTourDataLayer;
using FunTourDataLayer.BusCompany;
using FunTourDataLayer.EventCompany;
using FunTourDataLayer.FlightCompany;
using FunTourDataLayer.Hotel;
using FunTourDataLayer.Locality;
using FunTourDataLayer.Reservation;
using FunTourDataLayer.Services;

namespace FunTourBusinessLayer.Service
{
    public class DataService
    {
        public UnitOfWork UnitOfWork { get; set; }

        public DataService()
        {
            UnitOfWork = new UnitOfWork();
        }

        #region Flights
        public IEnumerable<Flight> GetFlights(DateTime toDay, City fromPlace, City toPlace)
        {
            var getFlightRequest = new GetFlightRequest
            {
                fromPlace = fromPlace.Name,
                toPlace = toPlace.Name,
                toDay = toDay

            };

            //FlightCompany FlightCompany = UnitOfWork.FlightCompanyRepository.Get().FirstOrDefault();
            FlightCompany FlightCompany = new FlightCompany
            {
                APIURLToGetFlights = "http://demo4736431.mockable.io/GetFlights"
            };
            List<Flight> ListOfFlights = new List<Flight>();

            var consumerFlights = new Consumer<GetFlightResponse>();

            GetFlightResponse getFlightResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToGetFlights, "POST", getFlightRequest).Result;

            foreach (var item in getFlightResponse.CommercialFlights)
            {
                var auxFlight = new Flight
                {
                    Id_Flight = item.idFlight,
                    DepartureDate = item.Deport,
                    ArrivedDate = item.Arrive,
                    Price = item.Price,
                    NotReservedSeats = item.Disponible_Places,

                };
                ListOfFlights.Add(auxFlight);
            }
            return ListOfFlights;
        }
        public void SetToGoFlightReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationFlightRequest = new FlightReservationsToTravelPackageRequest
            {
                FlightID = travelPackage.ToGoFlight.Id_Flight,
                SeatCount = travelPackage.ReservationAmount
            };

            FlightCompany FlightCompany = UnitOfWork.FlightCompanyRepository.GetByID(travelPackage.ToGoFlight.Id_Flight);

            var consumerFlights = new Consumer<List<FlightReservationsToTravelPackageResponse>>();

            List<FlightReservationsToTravelPackageResponse> getSeatsResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToReserveSeatsToTravelPackage, "POST", reservationFlightRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new ReservedSeat()
                {
                    Id_Flight = item.Id_Flight,
                    Id_ReservedSeat = item.Id_Seat,
                    Flight = UnitOfWork.FlightRepository.GetByID(item.Id_Flight)
                };
                auxSeat.Flight.ReservedSeat.Add(auxSeat);
                UnitOfWork.ReservedSeatRepository.Insert(auxSeat);
                UnitOfWork.FlightRepository.Update(auxSeat.Flight);
            }
            UnitOfWork.Save();
        }


        public void SetToBackFlightReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationFlightRequest = new FlightReservationsToTravelPackageRequest
            {
                FlightID = travelPackage.ToBackFlight.Id_Flight,
                SeatCount = travelPackage.ReservationAmount
            };

            FlightCompany FlightCompany = UnitOfWork.FlightCompanyRepository.GetByID(travelPackage.ToBackFlight.Id_Flight);

            var consumerFlights = new Consumer<List<FlightReservationsToTravelPackageResponse>>();

            List<FlightReservationsToTravelPackageResponse> getSeatsResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToReserveSeatsToTravelPackage, "POST", reservationFlightRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new ReservedSeat()
                {
                    Id_Flight = item.Id_Flight,
                    Id_ReservedSeat = item.Id_Seat,
                    Flight = UnitOfWork.FlightRepository.GetByID(item.Id_Flight)
                };
                auxSeat.Flight.ReservedSeat.Add(auxSeat);
                UnitOfWork.ReservedSeatRepository.Insert(auxSeat);
                UnitOfWork.FlightRepository.Update(auxSeat.Flight);
            }
            UnitOfWork.Save();
        }

        #endregion

        #region Buses
        public IEnumerable<Bus> GetBuses(DateTime toDay, City fromPlace, City toPlace)
        {
            var getBusRequest = new GetBusRequest
            {
                Origin = fromPlace.Name,
                Destination = toPlace.Name,
                Date = toDay

            };

            //BusCompany BusCompany = UnitOfWork.BusCompanyRepository.Get().FirstOrDefault();
            BusCompany BusCompany = new BusCompany
            {
                Name = "Nombre",
                APIURLToGetBuses = "http://demo4736431.mockable.io/GetBuses"
            };


            var consumerBuss = new Consumer<List<BusResponse>>();

            List<BusResponse> getBusResponse = consumerBuss.ReLoadEntities(BusCompany.APIURLToGetBuses, "GET", getBusRequest).Result;

            List<Bus> listOfBus = new List<Bus>();

            foreach (var item in getBusResponse)
            {
                var auxBus = new Bus
                {
                    IdAPI_Bus = item.TripID,
                    TripID = item.TripID,
                    Destination = item.Destination,
                    Origin = item.Origin,
                    DateTimeArrival = item.DateTimeArrival,
                    DateTimeDeparture = item.DateTimeDeparture,
                    Company = item.Company,
                    BusCompany = BusCompany,
                    Class = item.Class,
                    Capacity = item.Capacity,
                    Price = (float)item.Price,
                    NotReservedSeats = item.AvailableSeats
                };
                listOfBus.Add(auxBus);
            }
            return listOfBus;
        }

        public void SetToGoBusReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationBusRequest = new BusReservationsToTravelPackageRequest
            {
                TripID = travelPackage.ToGoBus.TripID,
                SeatCount = travelPackage.ReservationAmount
            };

            BusCompany BusCompany = UnitOfWork.BusCompanyRepository.GetByID(travelPackage.ToGoBus.IdAPI_Bus);

            var consumerBuss = new Consumer<List<BusReservationsToTravelPackageResponse>>();

            List<BusReservationsToTravelPackageResponse> getSeatsResponse = consumerBuss.ReLoadEntities(BusCompany.APIURLToReserveSeatToTravelPackage, "POST", reservationBusRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new BusReservedSeat()
                {
                    IdAPI_Bus = item.TripID,
                    TripID = item.TripID,
                    Id_BusReservedSeat = item.BookingID,
                    Bus = UnitOfWork.BusRepository.GetByID(item.TripID)
                };
                auxSeat.Bus.BusReservedSeat.Add(auxSeat);
                UnitOfWork.BusReservedSeatRepository.Insert(auxSeat);
                UnitOfWork.BusRepository.Update(auxSeat.Bus);
            }
            UnitOfWork.Save();
        }


        public void SetToBackBusReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationBusRequest = new BusReservationsToTravelPackageRequest
            {
                TripID = travelPackage.ToBackBus.TripID,
                SeatCount = travelPackage.ReservationAmount
            };

            BusCompany BusCompany = UnitOfWork.BusCompanyRepository.GetByID(travelPackage.ToBackBus.IdAPI_Bus);

            var consumerBuss = new Consumer<List<BusReservationsToTravelPackageResponse>>();

            List<BusReservationsToTravelPackageResponse> getSeatsResponse = consumerBuss.ReLoadEntities(BusCompany.APIURLToReserveSeatToTravelPackage, "POST", reservationBusRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new BusReservedSeat()
                {
                    IdAPI_Bus = item.TripID,
                    TripID = item.TripID,
                    Id_BusReservedSeat = item.BookingID,
                    Bus = UnitOfWork.BusRepository.GetByID(item.TripID)
                };
                auxSeat.Bus.BusReservedSeat.Add(auxSeat);
                UnitOfWork.BusReservedSeatRepository.Insert(auxSeat);
                UnitOfWork.BusRepository.Update(auxSeat.Bus);
            }
            UnitOfWork.Save();
        }

        #endregion

        #region Hotels
        public IEnumerable<Hotel> GetHotels(City Place, DateTime fromDay, DateTime toDay)
        {
            var getHotelsRequest = new GetHotelsRequest
            {
                City = Place.Name,
                Region = Place.Province.Name,
                Country = Place.Province.Country.Name,
                Date_start = fromDay,
                Date_end = toDay
            };

            //HotelCompany HotelsCompany = UnitOfWork.HotelCompanyRepository.Get().FirstOrDefault();
            HotelCompany HotelsCompany = new HotelCompany
            {
                APIURLToGetHotels = "http://demo4736431.mockable.io/GetHotels"
            };

            List<Hotel> ListOfHotels = new List<Hotel>();

            var consumerHotelss = new Consumer<List<GetHotelsResponse>>();

            List<GetHotelsResponse> getHotelsResponse = consumerHotelss.ReLoadEntities(HotelsCompany.APIURLToGetHotels, "POST", getHotelsRequest).Result;

            foreach (var item in getHotelsResponse)
            {
                var auxHotels = new Hotel
                {
                    Id_Hotel = item.HotelID,
                    Name = item.HotelName,
                    Price = item.StandardRate,
                    NotReservedRooms = item.FreeRoomCount
                };
                ListOfHotels.Add(auxHotels);
            }
            return ListOfHotels;
        }

        public void SetHotelReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            DateTime fromDay;
            DateTime toDay;
            if (travelPackage.FlightOrBus)
            {
                fromDay = travelPackage.ToGoFlight.ArrivedDate;
                toDay = travelPackage.ToBackFlight.DepartureDate;
            }
            else
            {

                fromDay = travelPackage.ToGoBus.DateTimeArrival;
                toDay = travelPackage.ToBackBus.DateTimeDeparture;
            }
            var SetReservationRequest = new ReservationsToTravelPackageRequest
            {
                HotelID = travelPackage.Hotel.Id_Hotel,
                Date_start = fromDay,
                Date_end = toDay,
                RoomCount = travelPackage.ReservationAmount

            };

            HotelCompany HotelsCompany = UnitOfWork.HotelCompanyRepository.GetByID(travelPackage.Hotel.Id_Hotel);

            var consumerHotels = new Consumer<List<SetReservationsToTravelPackageResponse>>();

            List<SetReservationsToTravelPackageResponse> getBookingsResponse = consumerHotels.ReLoadEntities(HotelsCompany.APIURLToReserveRoomsToTravelPackage, "POST", SetReservationRequest).Result;

            foreach (var item in getBookingsResponse)
            {
                var auxHotels = new ReservedRoom
                {
                    HotelID = item.HotelID,
                    BookingID = item.BookingID,
                    Hotel = UnitOfWork.HotelRepository.GetByID(item.HotelID)
                };
                auxHotels.Hotel.ReservedRoom.Add(auxHotels);
                UnitOfWork.ReservedRoomRepository.Insert(auxHotels);
                UnitOfWork.HotelRepository.Update(auxHotels.Hotel);
            }
            UnitOfWork.Save();

        }

        #endregion

        #region Events

        public IEnumerable<Event> GetEvents(City Place, DateTime fromDay, DateTime toDay)
        {
            var getEventRequest = new GetEventRequest
            {
                City = Place.Name,
                Date_start = fromDay,
                Date_end = toDay

            };

            EventCompany EventCompany = UnitOfWork.EventCompanyRepository.Get().FirstOrDefault();

            List<Event> ListOfEvents = new List<Event>();

            var consumerEvents = new Consumer<List<GetEventResponse>>();

            List<GetEventResponse> getEventResponse = consumerEvents.ReLoadEntities(EventCompany.APIURLToGetEvents, "POST", getEventRequest).Result;

            foreach (var item in getEventResponse)
            {
                var auxEvent = new Event
                {
                    Id_Event = item.EventWithTicketID,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    AvailableTickets = item.MaxTicket,

                };
                ListOfEvents.Add(auxEvent);
            }
            return ListOfEvents;

        }


        public void SetEventReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationEventRequest = new EventReservationsToTravelPackageRequest
            {
                EventID = travelPackage.Event.Id_Event,
                TicketsAmount = travelPackage.ReservationAmount
            };

            EventCompany EventsCompany = UnitOfWork.EventCompanyRepository.GetByID(travelPackage.Event.Id_Event);

            var consumerEvents = new Consumer<List<EventReservationsToTravelPackageResponse>>();

            List<EventReservationsToTravelPackageResponse> getTicketsResponse = consumerEvents.ReLoadEntities(EventsCompany.APIURLToReserveTickets, "POST", reservationEventRequest).Result;

            foreach (var item in getTicketsResponse)
            {
                var auxTicket = new ReservedTicket()
                {
                    Id_Event = item.EventID,
                    Id_ReservedTicket = item.TicketID,
                    Event = UnitOfWork.EventRepository.GetByID(item.EventID)
                };
                auxTicket.Event.ReservedTicket.Add(auxTicket);
                UnitOfWork.ReservedTicketRepository.Insert(auxTicket);
                UnitOfWork.EventRepository.Update(auxTicket.Event);
            }
            UnitOfWork.Save();
        }

        #endregion





    }
    
}
