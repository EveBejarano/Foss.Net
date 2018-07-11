using System;
using System.Collections.Generic;
using System.Linq;
using FunTourBusinessLayer.UnitOfWorks;
using FunTourDataLayer.BusCompany;
using FunTourDataLayer.EventCompany;
using FunTourDataLayer.FlightCompany;
using FunTourDataLayer.Hotel;
using FunTourDataLayer.Locality;
using FunTourDataLayer.Reservation;
using FunTourDataLayer.Services;
using FunTourDataLayer;
using FunTourDataLayer.Payment;


namespace FunTourBusinessLayer.Service
{
    public class DataService
    {
        public UnitOfWork UnitOfWork { get; set; }
        public static List<Flight> AuxFlights { get; set; }
        public static List<Bus> AuxBus { get; set; }
        public static List<Event> AuxEvents { get; set; }

        public DataService()
        {
            UnitOfWork = new UnitOfWork();
        }

        #region Flights
        public IEnumerable<AuxFlight> GetFlights(DateTime toDay, City fromPlace, City toPlace)
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
            List<AuxFlight> ListOfFlights = new List<AuxFlight>();

            var consumerFlights = new Consumer<List<GetFlightResponse>>();

            List<GetFlightResponse> getFlightResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToGetFlights, "GET", getFlightRequest).Result;
            

            foreach (var item in getFlightResponse)
            {
                var auxFlight = new AuxFlight
                {
                    Id_Flight = item.idFlight,
                    DepartureDate = item.Deport,
                    ArrivedDate = item.Arrive,
                    Price = item.Price,
                    NotReservedSeats = item.Disponible_Places,
                    FlightCompany = FlightCompany
                };

                UnitOfWork.AuxFlightRepository.Insert(auxFlight);
                ListOfFlights.Add(auxFlight);
            }
            UnitOfWork.Save();

            return ListOfFlights;
        }

        public void SetAuxFlightToPackage(int travelPackageId, int toGoId, int toBackId)
        {
            var travelpackage = UnitOfWork.TravelPackageRepository.GetByID(travelPackageId);

            var auxflight = UnitOfWork.AuxFlightRepository.GetByID(toGoId);
            var Flight = new Flight
            {
                Id_Flight = auxflight.Id_Flight,
                DepartureDate = auxflight.DepartureDate,
                ArrivedDate = auxflight.ArrivedDate,
                Price = auxflight.Price,
                NotReservedSeats = auxflight.NotReservedSeats,
                FlightCompany = auxflight.FlightCompany

            };

            UnitOfWork.FlightRepository.Insert(Flight);
            travelpackage.ToGoFlight = Flight;

            auxflight = UnitOfWork.AuxFlightRepository.GetByID(toGoId);
            var ToBackFlight = new Flight
            {
                Id_Flight = auxflight.Id_Flight,
                DepartureDate = auxflight.DepartureDate,
                ArrivedDate = auxflight.ArrivedDate,
                Price = auxflight.Price,
                NotReservedSeats = auxflight.NotReservedSeats,
                FlightCompany = auxflight.FlightCompany

            };

            UnitOfWork.FlightRepository.Insert(ToBackFlight);

            travelpackage.ToBackFlight = ToBackFlight;

            UnitOfWork.AuxFlightRepository.DeleteAllEntities();
            UnitOfWork.TravelPackageRepository.Update(travelpackage);
            UnitOfWork.Save();
        }



        public void SetToGoFlightReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationFlightRequest = new FlightReservationsToTravelPackageRequest
            {
                FlightID = travelPackage.ToGoFlight.Id_Flight,
                SeatCount = travelPackage.ReservationAmount
            };

            //FlightCompany FlightCompany = UnitOfWork.FlightCompanyRepository.GetByID(travelPackage.ToGoFlight.Id_Flight);

            FlightCompany FlightCompany = new FlightCompany
            {
                APIURLToReserveSeatsToTravelPackage = "http://demo4736431.mockable.io/ReserveFlightSeat"
            };

            var consumerFlights = new Consumer<List<FlightReservationsToTravelPackageResponse>>();

            List<FlightReservationsToTravelPackageResponse> getSeatsResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToReserveSeatsToTravelPackage, "GET", reservationFlightRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new FlightReservedSeat()
                {
                    Id_ReservedSeat = item.Id_Seat,
                    Flight = UnitOfWork.FlightRepository.GetByID(item.Id_Flight),
                    TravelPackage = travelPackage,
                    Available = true
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

            //FlightCompany FlightCompany = UnitOfWork.FlightCompanyRepository.GetByID(travelPackage.ToGoFlight.Id_Flight);

            FlightCompany FlightCompany = new FlightCompany
            {
                APIURLToReserveSeatsToTravelPackage = "http://demo4736431.mockable.io/ReserveFlightSeat"
            };
            var consumerFlights = new Consumer<List<FlightReservationsToTravelPackageResponse>>();

            List<FlightReservationsToTravelPackageResponse> getSeatsResponse = consumerFlights.ReLoadEntities(FlightCompany.APIURLToReserveSeatsToTravelPackage, "GET", reservationFlightRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new FlightReservedSeat()
                {
                    Id_ReservedSeat = item.Id_Seat,
                    Flight = UnitOfWork.FlightRepository.GetByID(item.Id_Flight),
                    TravelPackage = travelPackage,
                    Available = true
                };
                auxSeat.Flight.ReservedSeat.Add(auxSeat);
                UnitOfWork.ReservedSeatRepository.Insert(auxSeat);
                UnitOfWork.FlightRepository.Update(auxSeat.Flight);
            }
            UnitOfWork.Save();
        }

        #endregion

        #region Payments
        public bool DoPayment(string Name, string creditCardNumber, string expirationDate, string securityNumber)
        {
            var GetPaymentRequest = new GetPaymentRequest
            {
                Name = Name,
                CreditCardNumber =creditCardNumber,
                ExpirationDate = expirationDate,
                SecurityNumber = securityNumber
	    };

            PaymentService PaymentService = new PaymentService
            {
                Name = "Payments",
                APIURLToPay = "http://localhost:3040/payments"
            };

            var consumerPayment = new Consumer<GetPaymentResponse>();

		GetPaymentResponse getPaymentRespons = consumerPayment.ReLoadEntities(PaymentService.APIURLToPay, "POST", GetPaymentRequest).Result; 
		
		return getPaymentRespons.stateOfPayment;
	}
	#endregion

        #region Buses
        public IEnumerable<AuxBus> GetBuses(DateTime toDay, City fromPlace, City toPlace)
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

            List<AuxBus> listOfBus = new List<AuxBus>();

            foreach (var item in getBusResponse)
            {
                var auxBus = new AuxBus
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

                UnitOfWork.AuxBusRepository.Insert(auxBus);

                listOfBus.Add(auxBus);
            }
            UnitOfWork.Save();
            return listOfBus;
        }

        public void SetAuxBusToPackage(int travelPackageId, int toGoId, int toBackId)
        {
            var travelpackage = UnitOfWork.TravelPackageRepository.GetByID(travelPackageId);

            var auxbus = UnitOfWork.AuxBusRepository.GetByID(toGoId);
            var Bus = new Bus
            {
                IdAPI_Bus = auxbus.IdAPI_Bus,
                DateTimeDeparture = auxbus.DateTimeDeparture,
                DateTimeArrival = auxbus.DateTimeArrival,
                Price = auxbus.Price,
                NotReservedSeats = auxbus.NotReservedSeats,
                BusCompany = auxbus.BusCompany

            };

            UnitOfWork.BusRepository.Insert(Bus);
            travelpackage.ToGoBus = Bus;

            auxbus = UnitOfWork.AuxBusRepository.GetByID(toGoId);
            var ToBackBus = new Bus
            {
                IdAPI_Bus = auxbus.IdAPI_Bus,
                DateTimeDeparture = auxbus.DateTimeDeparture,
                DateTimeArrival = auxbus.DateTimeArrival,
                Price = auxbus.Price,
                NotReservedSeats = auxbus.NotReservedSeats,
                BusCompany = auxbus.BusCompany

            };

            UnitOfWork.BusRepository.Insert(ToBackBus);

            travelpackage.ToBackBus = ToBackBus;

            UnitOfWork.AuxBusRepository.DeleteAllEntities();
            UnitOfWork.TravelPackageRepository.Update(travelpackage);
            UnitOfWork.Save();
        }


        public void SetToGoBusReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationBusRequest = new BusReservationsToTravelPackageRequest
            {
                TripID = travelPackage.ToGoBus.TripID,
                SeatCount = travelPackage.ReservationAmount
            };

            //BusCompany BusCompany = UnitOfWork.BusCompanyRepository.GetByID(travelPackage.ToGoBus.IdAPI_Bus);

            BusCompany BusCompany = new BusCompany
            {
                APIURLToReserveSeatToTravelPackage = "http://demo4736431.mockable.io/ReserverBusSeat"
            };

            var consumerBuss = new Consumer<List<BusReservationsToTravelPackageResponse>>();

            List<BusReservationsToTravelPackageResponse> getSeatsResponse = consumerBuss.ReLoadEntities(BusCompany.APIURLToReserveSeatToTravelPackage, "GET", reservationBusRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new BusReservedSeat()
                {
                    TripID = item.TripID,
                    Id_ReservedSeat = item.BookingID,
                    Bus = UnitOfWork.BusRepository.GetByID(item.TripID),
                    TravelPackage = travelPackage,
                    Available = true
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

            //BusCompany BusCompany = UnitOfWork.BusCompanyRepository.GetByID(travelPackage.ToBackBus.IdAPI_Bus);

            BusCompany BusCompany = new BusCompany
            {
                APIURLToReserveSeatToTravelPackage = "http://demo4736431.mockable.io/ReserverBusSeat",
            };

            var consumerBuss = new Consumer<List<BusReservationsToTravelPackageResponse>>();

            List<BusReservationsToTravelPackageResponse> getSeatsResponse = consumerBuss.ReLoadEntities(BusCompany.APIURLToReserveSeatToTravelPackage, "GET", reservationBusRequest).Result;

            foreach (var item in getSeatsResponse)
            {
                var auxSeat = new BusReservedSeat()
                {
                    TripID = item.TripID,
                    Id_ReservedSeat = item.BookingID,
                    Bus = UnitOfWork.BusRepository.GetByID(item.TripID),
                    TravelPackage = travelPackage,
                    Available = true
                };
                auxSeat.Bus.BusReservedSeat.Add(auxSeat);
                UnitOfWork.BusRepository.Update(auxSeat.Bus);
            }
            UnitOfWork.Save();
        }

        #endregion

        #region Hotels
        public IEnumerable<AuxHotel> GetHotels(City Place, DateTime fromDay, DateTime toDay)
        {
            var city = UnitOfWork.CityRepository.Get(filter: p => p.Id_City == Place.Id_City,
                includeProperties: "Province").FirstOrDefault();
            var province = UnitOfWork.ProvinceRepository.Get(filter: p=> p.Id_Province == Place.Province.Id_Province, includeProperties: "Country").FirstOrDefault();
            var getHotelsRequest = new GetHotelsRequest
            {
                City = Place.Name,
                Region = Place.Province.Name,
                Country = province.Country.Name,
                Date_start = fromDay,
                Date_end = toDay
            };

            //HotelCompany HotelsCompany = UnitOfWork.HotelCompanyRepository.Get().FirstOrDefault();
            HotelCompany HotelsCompany = new HotelCompany
            {
                APIURLToGetHotels = "http://demo4736431.mockable.io/GetHotels"
            };

            List<AuxHotel> ListOfHotels = new List<AuxHotel>();

            var consumerHotelss = new Consumer<List<GetHotelsResponse>>();

            List<GetHotelsResponse> getHotelsResponse = consumerHotelss.ReLoadEntities(HotelsCompany.APIURLToGetHotels, "GET", getHotelsRequest).Result;

            foreach (var item in getHotelsResponse)
            {
                var auxHotels = new AuxHotel
                {
                    Id_Hotel = item.HotelID,
                    Name = item.HotelName,
                    Price = item.StandardRate,
                    NotReservedRooms = item.FreeRoomCount,
                    HotelCompany = HotelsCompany
                };
                UnitOfWork.AuxHotelRepository.Insert(auxHotels);
                ListOfHotels.Add(auxHotels);
            }
            UnitOfWork.Save();
            return ListOfHotels;
        }

        public void SetAuxHotelToPackage(int travelPackageId, int hotelId)
        {
            var travelpackage = UnitOfWork.TravelPackageRepository.GetByID(travelPackageId);

            var auxhotel = UnitOfWork.AuxHotelRepository.GetByID(hotelId);
            var Hotel = new Hotel
            {
                Id_Hotel = auxhotel.Id_Hotel,
                Name = auxhotel.Name,
                Description = auxhotel.Description,
                Price = auxhotel.Price,
                NotReservedRooms = auxhotel.NotReservedRooms,
                HotelCompany = auxhotel.HotelCompany

            };

            UnitOfWork.HotelRepository.Insert(Hotel);
            travelpackage.Hotel = Hotel;
            UnitOfWork.AuxHotelRepository.DeleteAllEntities();
            UnitOfWork.TravelPackageRepository.Update(travelpackage);
            UnitOfWork.Save();
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

            //HotelCompany HotelsCompany = UnitOfWork.HotelCompanyRepository.GetByID(travelPackage.Hotel.Id_Hotel);
            HotelCompany HotelsCompany = new HotelCompany
            {
                APIURLToReserveRoomsToTravelPackage = "http://demo4736431.mockable.io/ReserveRoom"
            };
            var consumerHotels = new Consumer<List<SetReservationsToTravelPackageResponse>>();

            List<SetReservationsToTravelPackageResponse> getBookingsResponse = consumerHotels.ReLoadEntities(HotelsCompany.APIURLToReserveRoomsToTravelPackage, "GET", SetReservationRequest).Result;

            foreach (var item in getBookingsResponse)
            {
                var auxHotels = new ReservedRoom
                {
                    HotelID = item.HotelID,
                    BookingID = item.BookingID,
                    Hotel = UnitOfWork.HotelRepository.GetByID(item.HotelID),
                    TravelPackage = travelPackage,
                    Available = true
                };
                
               UnitOfWork.ReservedRoomRepository.Insert(auxHotels);
            }
            UnitOfWork.Save();

        }

        #endregion

        #region Events

        public IEnumerable<AuxEvent> GetEvents(City Place, DateTime fromDay, DateTime toDay)
        {
            var getEventRequest = new GetEventRequest
            {
                City = Place.Name,
                Date_start = fromDay,
                Date_end = toDay

            };

            //EventCompany EventCompany = UnitOfWork.EventCompanyRepository.Get().FirstOrDefault();

            var EventCompany = new EventCompany
            {
                APIURLToGetEvents = "http://demo4736431.mockable.io/GetEvents"
            };

            List<AuxEvent> ListOfEvents = new List<AuxEvent>();

            var consumerEvents = new Consumer<List<GetEventResponse>>();

            List<GetEventResponse> getEventResponse = consumerEvents.ReLoadEntities(EventCompany.APIURLToGetEvents, "GET", getEventRequest).Result;

            foreach (var item in getEventResponse)
            {
                var auxEvent = new AuxEvent
                {
                    Id_Event = item.EventWithTicketID,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    AvailableTickets = item.MaxTicket,

                };
                UnitOfWork.AuxEventRepository.Insert(auxEvent);
                ListOfEvents.Add(auxEvent);
            }

            UnitOfWork.Save();
            return ListOfEvents;

        }


        public void SetAuxEventToPackage(int travelPackageId, int? eventId)
        {
            var travelpackage = UnitOfWork.TravelPackageRepository.GetByID(travelPackageId);

            var auxevent = UnitOfWork.AuxEventRepository.GetByID(eventId);
            var Event = new Event
            {
                Id_Event = auxevent.Id_Event,
                Name = auxevent.Name,
                Description = auxevent.Description,
                Price = auxevent.Price,
                AvailableTickets = auxevent.AvailableTickets,
                EventCompany = auxevent.EventCompany

            };
            UnitOfWork.AuxEventRepository.DeleteAllEntities();
            UnitOfWork.EventRepository.Insert(Event);
            travelpackage.Event = Event;

            UnitOfWork.TravelPackageRepository.Update(travelpackage);
            UnitOfWork.Save();
        }


        public void SetEventReservationToNewTravelPackage(TravelPackage travelPackage)
        {
            var reservationEventRequest = new EventReservationsToTravelPackageRequest
            {
                EventID = travelPackage.Event.Id_Event,
                TicketsAmount = travelPackage.ReservationAmount
            };

            //EventCompany EventsCompany = UnitOfWork.EventCompanyRepository.GetByID(travelPackage.Event.Id_Event);

            EventCompany EventsCompany = new EventCompany
            {
                APIURLToReserveTickets = "http://demo4736431.mockable.io/ReserveEvent"
            };

            var consumerEvents = new Consumer<List<EventReservationsToTravelPackageResponse>>();

            List<EventReservationsToTravelPackageResponse> getTicketsResponse = consumerEvents.ReLoadEntities(EventsCompany.APIURLToReserveTickets, "GET", reservationEventRequest).Result;

            foreach (var item in getTicketsResponse)
            {
                var auxTicket = new ReservedTicket()
                {
                    Id_Event = item.EventID,
                    Id_ReservedTicket = item.TicketID,
                    Event = UnitOfWork.EventRepository.GetByID(item.EventID),
                    TravelPackage = travelPackage,
                    Available = true
                };
                auxTicket.Event.ReservedTicket.Add(auxTicket);
                UnitOfWork.ReservedTicketRepository.Insert(auxTicket);
                UnitOfWork.EventRepository.Update(auxTicket.Event);
            }
            UnitOfWork.Save();
        }

        #endregion

        public void InactivatePackage(int? travelPackageId)
        {
            var package = UnitOfWork.TravelPackageRepository.GetByID(travelPackageId);
            package.Activate = false;
            UnitOfWork.TravelPackageRepository.Update(package);
            UnitOfWork.Save();
        }

        public void ActivatePackage(int? travelPackageId)
        {
            var package = UnitOfWork.TravelPackageRepository.GetByID(travelPackageId);
            package.Activate = true;
            UnitOfWork.TravelPackageRepository.Update(package);
            UnitOfWork.Save();
        }
    }


}
