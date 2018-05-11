using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusAPI.DAL;
using BusAPI.Models;

namespace BusAPI.Controllers
{
    public class TripsController : Controller
    {
        private BusContext db = new BusContext();

        // GET: Trips
        public ActionResult Index()
        {
            var trips = db.Trips.Include(t => t.Bus).Include(t => t.DestinationCity).Include(t => t.OriginCity);
            return View(trips.ToList());
        }

        // GET: Trips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // GET: Trips/Create
        public ActionResult Create()
        {
            ViewBag.BusID = new SelectList(db.Buses, "BusID", "Company");
            ViewBag.DestinationID = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.OriginID = new SelectList(db.Cities, "CityID", "CityName");
            return View();
        }

        // POST: Trips/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TripID,BusID,OriginID,DestinationID,Price,DateTimeTrip")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Trips.Add(trip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BusID = new SelectList(db.Buses, "BusID", "Company", trip.BusID);
            ViewBag.DestinationID = new SelectList(db.Cities, "CityID", "CityName", trip.DestinationID);
            ViewBag.OriginID = new SelectList(db.Cities, "CityID", "CityName", trip.OriginID);
            return View(trip);
        }

        // GET: Trips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusID = new SelectList(db.Buses, "BusID", "Company", trip.BusID);
            ViewBag.DestinationID = new SelectList(db.Cities, "CityID", "CityName", trip.DestinationID);
            ViewBag.OriginID = new SelectList(db.Cities, "CityID", "CityName", trip.OriginID);
            return View(trip);
        }

        // POST: Trips/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TripID,BusID,OriginID,DestinationID,Price,DateTimeTrip")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BusID = new SelectList(db.Buses, "BusID", "Company", trip.BusID);
            ViewBag.DestinationID = new SelectList(db.Cities, "CityID", "CityName", trip.DestinationID);
            ViewBag.OriginID = new SelectList(db.Cities, "CityID", "CityName", trip.OriginID);
            return View(trip);
        }

        // GET: Trips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trip trip = db.Trips.Find(id);
            db.Trips.Remove(trip);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
