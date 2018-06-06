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
                ToDay = travelPackageViewModel.ToDay
            };

            if (ModelState.IsValid)
            {
                Service.UnitOfWork.TravelPackageRepository.Insert(travelPackage);
                Service.UnitOfWork.Save();
                return RedirectToAction("AddServicesToTravel", routeValues: new {travelPackageViewModel});
            }

            return View(travelPackage);
        }

        public ActionResult AddServicesToTravel(TravelPackageViewModel travelPackage)
        {
            string FromPlace;
            string ToPlace;
            if (travelPackage.FlightOrBus)
            {
                IEnumerable<Flight> ListOfFlightsToGo = Service.GetFlights(travelPackage.FromDay, FromPlace, ToPlace);
                IEnumerable<Flight> ListOfFlightsToBack = Service.GetFlights( travelPackage.ToDay, ToPlace, FromPlace);


                ViewBag.FlightsToGo = new SelectList(ListOfFlightsToGo, "Id_FlightCompany", "Name");
                ViewBag.FlightsToBack = new SelectList(ListOfFlightsToBack, "Id_FlightCompany", "Name");
            }
            else
            {
                IEnumerable<Bus> ListOfBusesToGo = Service.GetBuses(travelPackage.FromDay, FromPlace, ToPlace);
                IEnumerable<Bus> ListOfBusesToBack = Service.GetBuses(travelPackage.ToDay, ToPlace, FromPlace);

                ViewBag.BusesToGo = new SelectList(ListOfBusesToGo, "Id_Bus", "Name");
                ViewBag.BusesToBack = new SelectList(ListOfBusesToBack, "Id_Bus", "Name");
            }

            return View(travelPackage);
        }

        public ActionResult AddServicesInPlace(TravelPackageViewModel travelPackage)
        {
            if (travelPackage.Flight != null && travelPackage.Bus == null)
            {
                var place= Service.GetHotel(travelPackage.FromDay, travelPackage.ToDay, travelPackage.)
            }
            else
            {
                if (travelPackage.Flight == null && travelPackage.Bus != null)
                {

                }
                else
                {
                    return RedirectToAction("AddServicesToTravel", routeValues: new { travelPackage, travelPackageViewModel.FlightOrBus });
                }
            }

            IEnumerable<Bus> ListOfBusesToGo = Service.GetBuses(travelPackage.FromDay, FromPlace, ToPlace);
            IEnumerable<Bus> ListOfBusesToBack = Service.GetBuses(travelPackage.ToDay, ToPlace, FromPlace);

            ViewBag.Hotels = new SelectList(Service.UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Events = new SelectList(Service.UnitOfWork.EventRepository.Get(), "Id_Event", "Name");

            return View(travelPackage);
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




        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddFlightToTravelPackagePartialView(int? ToGoFlightId, int? ToBackFlightId, int? TravelPackageId)
        {
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (ToBackFlightId != null && ToGoFlightId != null && TravelPackageId != null)
            {
                //if (Service.UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
                //{
                //    Service.UnitOfWork.Save();
                //}
                //RoleDetails role = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);

                return RedirectToAction("AddServicesInPlace", travelPackage);
            }

            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = true
            };
            return RedirectToAction("AddServicesToTravel", travelPackage);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddBusToTravelPackagePartialView(int? BusId, int? TravelPackageId)
        {
            // revisar que services to travel recibe un travelpackageviewmodel
            TravelPackage travelPackage = Service.UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (BusId != null && TravelPackageId != null)
            {
                //if (Service.UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
                //{
                //    Service.UnitOfWork.Save();
                //}
                //RoleDetails role = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);

                return RedirectToAction("AddServicesInPlace", travelPackage);
            }

            var travelPackageViewModel = new TravelPackageViewModel
            {
                Id_TravelPackage = travelPackage.Id_TravelPackage,
                PackageName = travelPackage.PackageName,
                Description = travelPackage.Description,
                FromDay = travelPackage.FromDay,
                ToDay = travelPackage.ToDay,
                FlightOrBus = false
            };
            return RedirectToAction("AddServicesToTravel", travelPackage);
        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddHotelToTravelPackagePartialView(int HotelId, int TravelPackageId)
        {
        //    if (Service.UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
        //    {
        //        Service.UnitOfWork.Save();
        //    }
        //    RoleDetails role = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddEventToTravelPackagePartialView(int EventId, int TravelPackageId)
        {
            //if (Service.UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
            //{
            //    Service.UnitOfWork.Save();
            //}
            //RoleDetails role = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView();
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
