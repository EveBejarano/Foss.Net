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
            ViewBag.Hotels = new SelectList(UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Flights = new SelectList(UnitOfWork.FlightRepository.Get(), "Id_FlightCompany", "Name");
            ViewBag.Event = new SelectList(UnitOfWork.EventRepository.Get(), "Id_Event", "Name");
            ViewBag.BusCompany = new SelectList(UnitOfWork.BusRepository.Get(), "Id_BusCompany", "Name");

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
                return RedirectToAction("AddServices", new {travelPackage, travelPackageViewModel.FlightOrBus});
            }

            return View(travelPackage);
        }

        public ActionResult AddServices(TravelPackage travelPackage, bool FlightOrBus)
        {
            if (FlightOrBus)
            {
                ViewBag.Flights = new SelectList(UnitOfWork.FlightRepository.Get(), "Id_FlightCompany", "Name");
            }
            else
            {
                ViewBag.Buses = new SelectList(UnitOfWork.BusRepository.Get(), "Id_Bus", "Name");
            }
            ViewBag.Hotels = new SelectList(UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Events = new SelectList(UnitOfWork.EventRepository.Get(), "Id_Event", "Name");
            ViewBag.FlightOrBus = FlightOrBus;

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


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddFlightToTravelPackagePartialView(int FlightId, int TravelPackageId)
        {
            //if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
            //{
            //    UnitOfWork.Save();
            //}
            //RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView();
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

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddBusToTravelPackagePartialView(int BusId, int TravelPackageId)
        {
            //if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
            //{
            //    UnitOfWork.Save();
            //}
            //RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView();
        }
    }
}
