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

namespace PruebaUsers.Controllers
{
    public class TravelPackagesController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        // GET: TravelPackages
        public ActionResult Index()
        {
            return View(UnitOfWork.TravelPackageRepository.Get());
        }

        // GET: TravelPackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelPackage travelPackage = UnitOfWork.TravelPackageRepository.GetByID(id);
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
                UnitOfWork.TravelPackageRepository.Insert(travelPackage);
                UnitOfWork.Save();
                return RedirectToAction("AddServicesToTravel", routeValues: new {travelPackage, travelPackageViewModel.FlightOrBus});
            }

            return View(travelPackage);
        }

        public ActionResult AddServicesToTravel(TravelPackage travelPackage, bool FlightOrBus)
        {
            if (FlightOrBus)
            {
                // tratar fechas y lugar y cargar tablas
                //tiene que tener en cuenta el lugar de llegada, el de salida, la fecha de llegada y la de salida
                // traer todos los vuelos de todas las companias que cumplan con eso
                //https://www.w3schools.com/howto/howto_css_custom_checkbox.asp
                ViewBag.FlightsToGo = new SelectList(UnitOfWork.FlightRepository.Get(), "Id_FlightCompany", "Name");
                ViewBag.FlightsToBack = new SelectList(UnitOfWork.FlightRepository.Get(), "Id_FlightCompany", "Name");
            }
            else
            {
                ViewBag.BusesToGo = new SelectList(UnitOfWork.BusRepository.Get(), "Id_Bus", "Name");
                ViewBag.BusesToBack = new SelectList(UnitOfWork.BusRepository.Get(), "Id_Bus", "Name");
            }
            ViewBag.FlightOrBus = FlightOrBus;

            return View(travelPackage);
        }

        public ActionResult AddServicesInPlace(TravelPackage travelPackage)
        {
            if ()
            {

            }
            ViewBag.Hotels = new SelectList(UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Events = new SelectList(UnitOfWork.EventRepository.Get(), "Id_Event", "Name");

            return View(travelPackage);
        }

        // GET: TravelPackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelPackage travelPackage = UnitOfWork.TravelPackageRepository.GetByID(id);
            if (travelPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hotels = new SelectList(UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Flights = new SelectList(UnitOfWork.FlightRepository.Get(), "Id_FlightCompany", "Name");
            ViewBag.Events = new SelectList(UnitOfWork.EventRepository.Get(), "Id_Event", "Name");
            ViewBag.Buses = new SelectList(UnitOfWork.BusRepository.Get(), "Id_Bus", "Name");


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
                UnitOfWork.TravelPackageRepository.Update(travelPackage);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(travelPackage);
        }




        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddFlightToTravelPackagePartialView(int? ToGoFlightId, int? ToBackFlightId, int? TravelPackageId)
        {
            TravelPackage travelPackage = UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (ToBackFlightId != null && ToGoFlightId != null && TravelPackageId != null)
            {
                //if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
                //{
                //    UnitOfWork.Save();
                //}
                //RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);

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
            TravelPackage travelPackage = UnitOfWork.TravelPackageRepository.GetByID(TravelPackageId);

            if (BusId != null && TravelPackageId != null)
            {
                //if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
                //{
                //    UnitOfWork.Save();
                //}
                //RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);

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
        //    if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
        //    {
        //        UnitOfWork.Save();
        //    }
        //    RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddEventToTravelPackagePartialView(int EventId, int TravelPackageId)
        {
            //if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
            //{
            //    UnitOfWork.Save();
            //}
            //RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView();
        }





        // GET: TravelPackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelPackage travelPackage = UnitOfWork.TravelPackageRepository.GetByID(id);
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
            TravelPackage travelPackage = UnitOfWork.TravelPackageRepository.GetByID(id);
            UnitOfWork.TravelPackageRepository.Delete(travelPackage);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
