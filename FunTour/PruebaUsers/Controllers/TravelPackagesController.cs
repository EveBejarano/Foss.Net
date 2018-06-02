using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.UnitOfWorks;
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
        public ActionResult Create([Bind(Include = "Id_Package")] TravelPackage travelPackage)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.TravelPackageRepository.Insert(travelPackage);
                UnitOfWork.Save();
                return RedirectToAction("AddServices", travelPackage);
            }

            return View(travelPackage);
        }

        public ActionResult AddServices(TravelPackage travelPackage)
        {

            ViewBag.Hotels = new SelectList(UnitOfWork.HotelRepository.Get(), "Id_Hotel", "Name");
            ViewBag.Flights = new SelectList(UnitOfWork.FlightRepository.Get(), "Id_FlightCompany", "Name");
            ViewBag.Event = new SelectList(UnitOfWork.EventRepository.Get(), "Id_Event", "Name");
            ViewBag.BusCompany = new SelectList(UnitOfWork.BusRepository.Get(), "Id_BusCompany", "Name");


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
            ViewBag.Event = new SelectList(UnitOfWork.EventRepository.Get(), "Id_Event", "Name");
            ViewBag.BusCompany = new SelectList(UnitOfWork.BusRepository.Get(), "Id_BusCompany", "Name");


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
        public PartialViewResult AddPermission2RoleReturnPartialView(int id, int permissionId)
        {
            if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
            {
                UnitOfWork.Save();
            }
            RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView("_ListPermissions", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddFlightToTravelPackagePartialView(int id, int flightId)
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
        public PartialViewResult AddHotelToTravelPackagePartialView(int id, int HotelId)
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
        public PartialViewResult AddEventToTravelPackagePartialView(int id, int TravelPackageId)
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
        public PartialViewResult AddBusToTravelPackagePartialView(int id, int BusId)
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
