using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FunTour.Models;
using FunTourBusinessLayer.Service;
using FunTourDataLayer.BusCompany;
using FunTourDataLayer.EventCompany;
using FunTourDataLayer.FlightCompany;
using FunTourDataLayer.Hotel;
using FunTourDataLayer.Locality;
using FunTourDataLayer.Reservation;

namespace PruebaUsers.Controllers
{
    public class TravelPackagesController : Controller
    {
        private const string AddServicesInPlaceString = "AddServicesInPlace";
        private const string AddServicesToTravelString = "AddServicesToTravel";
        private const string AddPlacesString = "AddPlaces";
        private readonly DataService Service = new DataService();

        // GET: TravelPackages
        public ActionResult Index()
        {
            if (Service.UnitOfWork.TravelPackageRepository.Get() == null)
            {

                List<TravelPackage> listOfPackages = new List<TravelPackage>();

                return View(listOfPackages);
            }
            else
            {
                var listOfPackages = Service.UnitOfWork.TravelPackageRepository.Get();

                return View(listOfPackages);
            }

        }
        // GET: TravelPackages/Details/5
        public ActionResult Details(int? TravelPackageId)
        {
            if (TravelPackageId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPLace,ToPlace,ToGoBus,ToBackBus,ToGoFlight,ToBackFlight, Event, Hotel, Reservations").FirstOrDefault();
            if (travelPackage != null)
            {
                var travelPackageViewModel = new TravelPackageViewModel
                {
                    Id_TravelPackage = travelPackage.Id_TravelPackage,
                    PackageName = travelPackage.PackageName,
                    Description = travelPackage.Description,
                    FromDay = travelPackage.FromDay,
                    ToDay = travelPackage.ToDay,
                    FlightOrBus = travelPackage.FlightOrBus,
                    FromPlace = travelPackage.FromPlace,
                    ToPlace = travelPackage.ToPlace,
                    Hotel = travelPackage.Hotel,
                    Event = travelPackage.Event,
                    ReservationAmount = travelPackage.ReservationAmount,
                    TotalPrice = travelPackage.TotalPrice,
                    Reservations = travelPackage.Reservations

                };

                if (travelPackageViewModel.FlightOrBus)
                {
                    travelPackageViewModel.ToGoFlight = travelPackage.ToGoFlight;
                    travelPackageViewModel.ToBackFlight = travelPackage.ToBackFlight;
                }
                else
                {
                    travelPackageViewModel.ToGoBus = travelPackage.ToGoBus;
                    travelPackageViewModel.ToBackBus = travelPackage.ToBackBus;
                }
                return View(travelPackageViewModel);
            }
            return HttpNotFound();
    }



        #region Create Package

        [UserAuthorization]
        [HttpGet]
        // GET: TravelPackages/Create
        public ActionResult Create()
        {
            return View();
        }


        [UserAuthorization]
        // POST: TravelPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_TravelPackage,PackageName,Description,FromDay,ToDay,FlightOrBus")] TravelPackageViewModel travelPackageViewModel)
        {
            var travelPackage = new TravelPackage
            {
                Id_TravelPackage = travelPackageViewModel.Id_TravelPackage,
                PackageName = travelPackageViewModel.PackageName,
                Description = travelPackageViewModel.Description,
                FromDay = travelPackageViewModel.FromDay,
                ToDay = travelPackageViewModel.ToDay,
                FlightOrBus = travelPackageViewModel.FlightOrBus
            };

            if (ModelState.IsValid)
            {   //validación de fechas
                if (travelPackageViewModel.FromDay > System.DateTime.Now & travelPackageViewModel.ToDay > System.DateTime.Now & travelPackage.FromDay != travelPackageViewModel.ToDay)
                {
                    Service.UnitOfWork.TravelPackageRepository.Insert(travelPackage); //se crea y guarda el paquete. se busca el id del creado
                    Service.UnitOfWork.Save();
                    travelPackageViewModel.Id_TravelPackage =
                        Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.PackageName == travelPackage.PackageName && p.Description == travelPackage.Description).FirstOrDefault().Id_TravelPackage;
                    return RedirectToAction(AddPlacesString, routeValues: new{ TravelPackageId = travelPackageViewModel.Id_TravelPackage}); //redirección a la siguiente vista
                }
            }

            return View(travelPackageViewModel);
        }


        #region Places

        [UserAuthorization]
        public ActionResult AddPlaces(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPlace, ToPlace").FirstOrDefault();
            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = travelPackage.FlightOrBus,
                FromPlace = travelPackage.FromPlace,
                ToPlace = travelPackage.ToPlace
            };

            IEnumerable<City> ListOfCityToGo = Service.UnitOfWork.CityRepository.Get(includeProperties: "Province");
            IEnumerable<City> ListOfCityToStay = Service.UnitOfWork.CityRepository.Get(includeProperties: "Province");

            //se cargan listas para elegir ciudad origen y destino
            ViewBag.FromCities = new SelectList(ListOfCityToGo, "Id_City", "Name");
            ViewBag.ToCities = new SelectList(ListOfCityToStay, "Id_City", "Name");

            return View(travelPackageViewModel);
        }
        [UserAuthorization]
        public ActionResult AddPlacesReturn(int TravelPackageId, int ToCityId, int FromCityId)
        {

            var travelPackage = Service.UnitOfWork.TravelPackageRepository
                .Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPlace, ToPlace")
                .FirstOrDefault();

            //se agregan origen y destino al paquete
            travelPackage.ToPlace = Service.UnitOfWork.CityRepository.GetByID(ToCityId);
            travelPackage.FromPlace = Service.UnitOfWork.CityRepository.GetByID(FromCityId);
            Service.UnitOfWork.TravelPackageRepository.Update(travelPackage);
            Service.UnitOfWork.Save();

            return RedirectToAction(AddServicesToTravelString, routeValues: new { TravelPackageId = TravelPackageId });

        }


        #endregion

        #region Bus&Flights
        [UserAuthorization]
        //[HttpGet]
        //[ValidateAntiForgeryToken]       //llena las listas con vuelos o viajes
        public ActionResult AddServicesToTravel(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPlace, ToPlace").FirstOrDefault();
            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = travelPackage.FlightOrBus,
                FromPlace = travelPackage.FromPlace,
                ToPlace = travelPackage.ToPlace
            };
            if (travelPackageViewModel.FlightOrBus)
            {
                IEnumerable<AuxFlight> ListOfFlightsToGo = Service.GetFlights(travelPackageViewModel.FromDay, travelPackageViewModel.FromPlace, travelPackageViewModel.ToPlace);
                IEnumerable<AuxFlight> ListOfFlightsToBack = Service.GetFlights(travelPackageViewModel.ToDay, travelPackageViewModel.FromPlace, travelPackageViewModel.ToPlace);


                ViewBag.FlightsToGo = new SelectList(ListOfFlightsToGo, "Id_Flight", "Id_Flight");
                ViewBag.FlightsToBack = new SelectList(ListOfFlightsToBack, "Id_Flight", "Id_Flight");
                ViewBag.FlighsToGoToShow = ListOfFlightsToGo;
                ViewBag.FlightsToBackToShow = ListOfFlightsToBack;
            }
            else
            {
                IEnumerable<AuxBus> ListOfBusesToGo = Service.GetBuses(travelPackageViewModel.FromDay, travelPackageViewModel.FromPlace, travelPackageViewModel.ToPlace);
                IEnumerable<AuxBus> ListOfBusesToBack = Service.GetBuses(travelPackageViewModel.FromDay, travelPackageViewModel.FromPlace, travelPackageViewModel.ToPlace);

                ViewBag.BusesToGo = new SelectList(ListOfBusesToGo, "IdAPI_Bus", "IdaPI_Bus");
                ViewBag.BusesToBack = new SelectList(ListOfBusesToBack, "IdAPI_Bus", "IdAPI_Bus");
                ViewBag.BusesToGoToShow = ListOfBusesToGo;
                ViewBag.BusesToBackToShow = ListOfBusesToBack;
            }

            return View(travelPackageViewModel);

        }
        [UserAuthorization]  //asigna vuelos o viajes al paquete
        public ActionResult AddServicesToTravelReturn(int TravelPackageId, int ToGoId, int ToBackId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPLace,ToPlace").FirstOrDefault();

            if (travelPackage.FlightOrBus)
            {
                Service.SetAuxFlightToPackage(TravelPackageId, ToGoId, ToBackId);


            }

            else
            {
                Service.SetAuxBusToPackage(TravelPackageId, ToGoId, ToBackId);

            }

            return RedirectToAction(AddServicesInPlaceString, routeValues: new { TravelPackageId });
        }


        #endregion

        #region Hotels&Events
        [UserAuthorization]

        public ActionResult AddServicesInPlace(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPLace,ToPlace,ToGoBus,ToBackBus,ToGoFlight,ToBackFlight").FirstOrDefault();

            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = travelPackage.FlightOrBus,
                FromPlace = travelPackage.FromPlace,
                ToPlace = travelPackage.ToPlace

            };

            DateTime FromDay;
            DateTime ToDay;
            if (travelPackageViewModel.FlightOrBus)
            {
                FromDay = travelPackage.ToGoFlight.ArrivedDate;
                ToDay = travelPackage.ToBackFlight.DepartureDate;
            }
            else
            {
                FromDay = travelPackage.ToGoBus.DateTimeArrival;
                ToDay = travelPackage.ToBackBus.DateTimeDeparture;
            }

            IEnumerable<AuxHotel> ListOfHotels = Service.GetHotels(travelPackage.ToPlace, FromDay, ToDay);
            IEnumerable<AuxEvent> ListOfEvents = Service.GetEvents(travelPackage.ToPlace, FromDay, ToDay);

            ViewBag.Hotels = new SelectList(ListOfHotels, "Id_Hotel", "Name");
            ViewBag.Events = new SelectList(ListOfEvents, "Id_Event", "Name");

            ViewBag.HotelsToShow = ListOfHotels;
            ViewBag.EventsToShow = ListOfEvents;

            return View(travelPackageViewModel);
        }
        [UserAuthorization]
        public ActionResult AddServicesInPlaceReturn(int TravelPackageId, int EventId, int HotelId)
        {

            Service.SetAuxHotelToPackage(TravelPackageId, HotelId);

            Service.SetAuxEventToPackage(TravelPackageId, EventId);

            return RedirectToAction("ShowToConfirmCreation", routeValues: new { TravelPackageId });

        }


        #endregion
        [UserAuthorization]
        public ActionResult ShowToConfirmCreation(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPLace,ToPlace,ToGoBus,ToBackBus,ToGoFlight,ToBackFlight, Event, Hotel").FirstOrDefault();

            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = travelPackage.FlightOrBus,
                FromPlace = travelPackage.FromPlace,
                ToPlace = travelPackage.ToPlace,
                Hotel = travelPackage.Hotel,
                Event = travelPackage.Event,
                Reservations = new List<Reservation>()
            };

            travelPackage.SetReservationAmount();
            travelPackageViewModel.ReservationAmount = travelPackage.ReservationAmount;

            travelPackage.SetPrice();
            travelPackageViewModel.TotalPrice = travelPackage.TotalPrice;

            Service.UnitOfWork.TravelPackageRepository.Update(travelPackage);
            Service.UnitOfWork.Save();

            if (travelPackageViewModel.FlightOrBus)
            {
                travelPackageViewModel.ToGoFlight = travelPackage.ToGoFlight;
                travelPackageViewModel.ToBackFlight = travelPackage.ToBackFlight;
            }
            else
            {
                travelPackageViewModel.ToGoBus = travelPackage.ToGoBus;
                travelPackageViewModel.ToBackBus = travelPackage.ToBackBus;
            }
            return View(travelPackageViewModel);
        }
        [UserAuthorization]
        public ActionResult ConfirmCreation(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPLace,ToPlace,ToGoBus,ToBackBus,ToGoFlight,ToBackFlight, Event, Hotel").FirstOrDefault();

            travelPackage.Creator = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(ControllerContext.HttpContext.User.Identity.Name);
            travelPackage.Activate = true;
            Service.UnitOfWork.TravelPackageRepository.Update(travelPackage);
            Service.UnitOfWork.Save();
            
            Service.SetHotelReservationToNewTravelPackage(travelPackage);
            Service.SetEventReservationToNewTravelPackage(travelPackage);
            if (travelPackage.FlightOrBus)
            {
                Service.SetToGoFlightReservationToNewTravelPackage(travelPackage);
                Service.SetToBackFlightReservationToNewTravelPackage(travelPackage);

            }
            else
            {
                Service.SetToGoBusReservationToNewTravelPackage(travelPackage);
                Service.SetToBackBusReservationToNewTravelPackage(travelPackage);
            }

            return RedirectToAction("Index");
        }
        [UserAuthorization]
        public ActionResult CancelCreation(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);
            Service.UnitOfWork.TravelPackageRepository.Delete(travelPackage);
            Service.UnitOfWork.Save();
            return RedirectToAction("Index");
        }
        #endregion


        [UserAuthorization]
        // GET: TravelPackages/Delete/5
        public ActionResult Activate(int? TravelPackageId)
        {
            Service.ActivatePackage(TravelPackageId);
            return RedirectToAction("Index");
        }

        [UserAuthorization]
        // GET: TravelPackages/Delete/5
        public ActionResult Inactivate(int? TravelPackageId)
        {
            Service.InactivatePackage(TravelPackageId);
            return RedirectToAction("Index");
        }



        [UserAuthorization]
        // GET: TravelPackages/Delete/5
        public ActionResult Delete(int? TravelPackageId)
        {
            if (TravelPackageId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPLace,ToPlace,ToGoBus,ToBackBus,ToGoFlight,ToBackFlight, Event, Hotel").FirstOrDefault();
            if (travelPackage == null)
            {
                return HttpNotFound();
            }

            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = travelPackage.FlightOrBus,
                FromPlace = travelPackage.FromPlace,
                ToPlace = travelPackage.ToPlace,
                Hotel = travelPackage.Hotel,
                Event = travelPackage.Event,
                Reservations = travelPackage.Reservations,
                ReservationAmount = travelPackage.ReservationAmount,
                TotalPrice = travelPackage.TotalPrice
        };
            
            if (travelPackageViewModel.FlightOrBus)
            {
                travelPackageViewModel.ToGoFlight = travelPackage.ToGoFlight;
                travelPackageViewModel.ToBackFlight = travelPackage.ToBackFlight;
            }
            else
            {
                travelPackageViewModel.ToGoBus = travelPackage.ToGoBus;
                travelPackageViewModel.ToBackBus = travelPackage.ToBackBus;
            }
            return View(travelPackageViewModel);
        }

        // POST: TravelPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int TravelPackageId)
        {
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);
            Service.UnitOfWork.TravelPackageRepository.Delete(travelPackage);
            Service.UnitOfWork.Save();
            return RedirectToAction("Index");
        }



    }
}

