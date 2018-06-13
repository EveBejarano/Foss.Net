using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.UnitOfWorks;
using FunTour.Models;
using FunTourDataLayer.Models;
using FuntourBusinessLayer.Service;
using FunTourDataLayer.Models;

namespace PruebaUsers.Controllers
{
    public class TravelPackagesController : Controller
    {
        private readonly DataService Service = new DataService();

        // GET: TravelPackages
        public ActionResult Index()
        {
            return View(Service.UnitOfWork.TravelPackageRepository.Get());
        }

        // GET: TravelPackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(id);
            if (travelPackage == null)
            {
                return HttpNotFound();
            }
            return View(travelPackage);
        }

        // GET: TravelPackages/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == Id).FirstOrDefault();

            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = travelPackage.FlightOrBus
            };
            return View(travelPackageViewModel);
        }

        // POST: TravelPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_TravelPackage")] TravelPackageViewModel travelPackageViewModel)
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
            {
                Service.UnitOfWork.TravelPackageRepository.Insert(travelPackage);
                Service.UnitOfWork.Save();
                return RedirectToAction("AddPlaces", routeValues: new {travelPackageViewModel});
            }

            return View(travelPackageViewModel);
        }

        public ActionResult AddPlaces(TravelPackageViewModel travelPackageView)
        {
            IEnumerable<City> ListOfCityToGo = Service.UnitOfWork.CityRepository.Get(includeProperties: "Province, Country");
            IEnumerable<City> ListOfCityToStay = Service.UnitOfWork.CityRepository.Get(includeProperties: "Province, Country");


            ViewBag.FromCities = new SelectList(ListOfCityToGo, "Id_City", "Name" + "Province.Name" + "Province.Country.Name");
            ViewBag.ToCities = new SelectList(ListOfCityToStay, "Id_City", "Name" + "Province.Name" + "Province.Country.Name");

            return View();
        }

        public ActionResult AddPlacesReturn(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "FromPlace, ToPlace").FirstOrDefault();

            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = travelPackage.FlightOrBus
            };


            if (travelPackage.FromPlace != null && travelPackage.ToPlace != null && travelPackage.ToPlace != travelPackage.FromPlace)
            {
                travelPackage.ToPlace = travelPackage.ToPlace;
                travelPackage.FromPlace = travelPackage.FromPlace;
                return RedirectToAction("AddServicesToTravel", routeValues: new { travelPackageViewModel });
            }
            return RedirectToAction("AddPlaces", routeValues: new { travelPackageViewModel }); ;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void AddToPlaceReturnPartialView(int CityId, int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            travelPackage.ToPlace = Service.UnitOfWork.CityRepository.GetByID(CityId);
            Service.UnitOfWork.TravelPackageRepository.Update(travelPackage);
            Service.UnitOfWork.Save();

        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void AddFromPlaceReturnPartialView(int CityId, int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            travelPackage.FromPlace = Service.UnitOfWork.CityRepository.GetByID(CityId);
            Service.UnitOfWork.TravelPackageRepository.Update(travelPackage);
            Service.UnitOfWork.Save();

        }






        public ActionResult AddServicesToTravel(TravelPackageViewModel travelPackageViewModel)
        {
            if (travelPackageViewModel.FlightOrBus)
            {
                IEnumerable<Flight> ListOfFlightsToGo = Service.GetFlights(travelPackageViewModel.FromDay, travelPackageViewModel.FromPlace, travelPackageViewModel.ToPlace);
                IEnumerable<Flight> ListOfFlightsToBack = Service.GetFlights( travelPackageViewModel.ToDay, travelPackageViewModel.FromPlace, travelPackageViewModel.ToPlace);


                ViewBag.FlightsToGo = new SelectList(ListOfFlightsToGo, "Id_Flight", "Id_Flight");
                ViewBag.FlightsToBack = new SelectList(ListOfFlightsToBack, "Id_Flight", "Id_Flight");
                ViewBag.FlighsToGoToShow = ListOfFlightsToGo;
                ViewBag.FlightsToBackToShow = ListOfFlightsToBack;
            }
            else
            {
                IEnumerable<Bus> ListOfBusesToGo = Service.GetBuses(travelPackageViewModel.FromDay, travelPackageViewModel.FromPlace.CP, travelPackageViewModel.ToPlace.CP);
                IEnumerable<Bus> ListOfBusesToBack = Service.GetBuses(travelPackageViewModel.FromDay, travelPackageViewModel.FromPlace.CP, travelPackageViewModel.ToPlace.CP);

                ViewBag.BusesToGo = new SelectList(ListOfBusesToGo, "Id_Bus", "Id_Bus");
                ViewBag.BusesToBack = new SelectList(ListOfBusesToBack, "Id_Bus", "Id_Bus");
                ViewBag.BusesToGoToShow = ListOfBusesToGo;
                ViewBag.BusesToBackToShow = ListOfBusesToBack;
            }

            return View(travelPackageViewModel);
        }

        public ActionResult AddServicesToTravelReturn(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties:"Flight, Bus, City").FirstOrDefault();

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
                if (travelPackage.ToGoFlight != null && travelPackage.ToBackFlight != null)
                {
                    travelPackageViewModel.ToGoFlight = travelPackage.ToGoFlight;
                    travelPackageViewModel.ToBackFlight = travelPackage.ToBackFlight;
                    return RedirectToAction("AddServicesInPlace", routeValues: new { travelPackageViewModel });
                }

                if (travelPackage.ToGoBus != null && travelPackage.ToBackBus != null)
                {
                    travelPackageViewModel.ToGoBus = travelPackage.ToGoBus;
                    travelPackageViewModel.ToBackBus = travelPackage.ToBackBus;
                    return RedirectToAction("AddServicesInPlace", routeValues: new { travelPackageViewModel });
                }
            }
            return RedirectToAction("AddServicesToTravel", routeValues: new { travelPackageViewModel }); ;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void AddFlightToTravelPackagePartialView(int? ToGoFlightId, int? ToBackFlightId, int? TravelPackageId)
        {
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (ToBackFlightId != null && ToGoFlightId != null)
            {
                travelPackage.ToGoFlight = Service.UnitOfWork.FlightRepository.GetByID(ToGoFlightId);
                travelPackage.ToBackFlight = Service.UnitOfWork.FlightRepository.GetByID(ToBackFlightId);
                Service.UnitOfWork.Save();
            }

        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void AddBusToTravelPackagePartialView(int? ToGoBusId, int? ToBackBusId, int? TravelPackageId)
        {
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (ToBackBusId != null && ToGoBusId != null)
            {
                travelPackage.ToGoBus = Service.UnitOfWork.BusRepository.GetByID(ToGoBusId);
                travelPackage.ToBackBus = Service.UnitOfWork.BusRepository.GetByID(ToBackBusId);
                Service.UnitOfWork.Save();
            }
        }






        public ActionResult AddServicesInPlace(TravelPackageViewModel travelPackageViewModel)
        {
            DateTime FromDay;
            DateTime ToDay;
            if (travelPackageViewModel.FlightOrBus)
            {
                FromDay = travelPackageViewModel.ToGoFlight.ArrivedDate;
                ToDay = travelPackageViewModel.ToBackFlight.DepartureDate;
            }
            else
            {
                FromDay = travelPackageViewModel.ToGoBus.ArrivedDate;
                ToDay = travelPackageViewModel.ToBackBus.DepartureDate;
            }

            IEnumerable<Hotel> ListOfHotels = Service.GetHotels(travelPackageViewModel.ToPlace.CP, FromDay, ToDay);
            IEnumerable<Event> ListOfEvents = Service.GetEvents(travelPackageViewModel.ToPlace.CP, FromDay, ToDay);

            ViewBag.Hotels = new SelectList(Service.UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Events = new SelectList(Service.UnitOfWork.EventRepository.Get(), "Id_Event", "Name");

            ViewBag.HotelsToShow = ListOfHotels;
            ViewBag.EventsToShow = ListOfEvents;

            return View(travelPackageViewModel);
        }

        public ActionResult AddServicesInPlaceReturn(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "Flight, Hotel, City, Event, Hotel").FirstOrDefault();

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

            if (travelPackage.Hotel != null && travelPackage.Event != null)
            {
                return RedirectToAction("ShowToConfirmCreation", routeValues: new { travelPackageViewModel });
            }

            if (travelPackageViewModel.FlightOrBus)
            {
                if (travelPackage.ToGoFlight != null && travelPackage.ToBackFlight != null)
                {
                    travelPackageViewModel.ToGoFlight = travelPackage.ToGoFlight;
                    travelPackageViewModel.ToBackFlight = travelPackage.ToBackFlight;
                }

                if (travelPackage.ToGoBus != null && travelPackage.ToBackBus != null)
                {
                    travelPackageViewModel.ToGoBus = travelPackage.ToGoBus;
                    travelPackageViewModel.ToBackBus = travelPackage.ToBackBus;
                }
            }

            return RedirectToAction("AddServicesInPlace", routeValues: new { travelPackageViewModel }); ;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void AddHotelToTravelPackagePartialView(int? HotelId, int TravelPackageId)
        {
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (HotelId != null)
            {
                travelPackage.Hotel = Service.UnitOfWork.HotelRepository.GetByID(HotelId);
                Service.UnitOfWork.Save();
            }
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void AddEventToTravelPackagePartialView(int? EventId, int TravelPackageId)
        {
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (EventId != null)
            {
                travelPackage.Event = Service.UnitOfWork.EventRepository.GetByID(EventId);
                Service.UnitOfWork.Save();
            }
        }



        public ActionResult ShowToConfirmCreation(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "Flight, Hotel, City, Event, Hotel").FirstOrDefault();

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
                Event = travelPackage.Event
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

        public ActionResult ConfirmCreation(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == TravelPackageId, includeProperties: "Flight, Hotel, City, Event, Hotel").FirstOrDefault();

            travelPackage.Creator = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(ControllerContext.HttpContext.User.Identity.Name);
            Service.UnitOfWork.TravelPackageRepository.Update(travelPackage);

            Service.UnitOfWork.Save();

            Service.UnitOfWork.ManageNewTravelPackage(travelPackage);
            return RedirectToAction("Index");
        }

        public ActionResult CancelCreation(int TravelPackageId)
        {
            var travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);
            Service.UnitOfWork.TravelPackageRepository.Delete(travelPackage);
            Service.UnitOfWork.Save();
            return RedirectToAction("Index");
        }

        // GET: TravelPackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(id);
            if (travelPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hotels = new SelectList(Service.UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Flights = new SelectList(Service.UnitOfWork.FlightRepository.Get(), "Id_FlightCompany", "Name");
            ViewBag.Events = new SelectList(Service.UnitOfWork.EventRepository.Get(), "Id_Event", "Name");
            ViewBag.Buses = new SelectList(Service.UnitOfWork.BusRepository.Get(), "Id_Bus", "Name");


            return View(travelPackage);
        }

        // POST: TravelPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Package")] TravelPackage travelPackage)
        {
            if (ModelState.IsValid)
            {
                Service.UnitOfWork.TravelPackageRepository.Update(travelPackage);
                Service.UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(travelPackage);
        }








        // GET: TravelPackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(id);
            if (travelPackage == null)
            {
                return HttpNotFound();
            }
            return View(travelPackage);
        }

        // POST: TravelPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(id);
            Service.UnitOfWork.TravelPackageRepository.Delete(travelPackage);
            Service.UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
