using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlightsAPI.Models;

namespace FlightsAPI.Controllers
{
    public class mvcCommercialFlightsController : Controller
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: mvcCommercialFlights
        public ActionResult Index()
        {
            var commercialFlights = db.CommercialFlights.Include(c => c.Destination).Include(c => c.Destination1).Include(c => c.Plane);
            return View(commercialFlights.ToList());
        }

        // GET: mvcCommercialFlights/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommercialFlight commercialFlight = db.CommercialFlights.Find(id);
            if (commercialFlight == null)
            {
                return HttpNotFound();
            }
            return View(commercialFlight);
        }

        // GET: mvcCommercialFlights/Create
        public ActionResult Create()
        {
            ViewBag.Flight_From = new SelectList(db.Destinations, "idAirport", "nameAirport");
            ViewBag.Flight_To = new SelectList(db.Destinations, "idAirport", "nameAirport");
            ViewBag.Flight_Plane = new SelectList(db.Planes, "idPlane", "namePlane");
            return View();
        }

        // POST: mvcCommercialFlights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idFlight,Distance,Deport,Arrive,Price,Disponible_Places,Flight_To,Flight_From,Flight_Plane")] CommercialFlight commercialFlight)
        {
            if (ModelState.IsValid)
            {
                db.CommercialFlights.Add(commercialFlight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Flight_From = new SelectList(db.Destinations, "idAirport", "nameAirport", commercialFlight.Flight_From);
            ViewBag.Flight_To = new SelectList(db.Destinations, "idAirport", "nameAirport", commercialFlight.Flight_To);
            ViewBag.Flight_Plane = new SelectList(db.Planes, "idPlane", "namePlane", commercialFlight.Flight_Plane);
            return View(commercialFlight);
        }

        // GET: mvcCommercialFlights/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommercialFlight commercialFlight = db.CommercialFlights.Find(id);
            if (commercialFlight == null)
            {
                return HttpNotFound();
            }
            ViewBag.Flight_From = new SelectList(db.Destinations, "idAirport", "nameAirport", commercialFlight.Flight_From);
            ViewBag.Flight_To = new SelectList(db.Destinations, "idAirport", "nameAirport", commercialFlight.Flight_To);
            ViewBag.Flight_Plane = new SelectList(db.Planes, "idPlane", "namePlane", commercialFlight.Flight_Plane);
            return View(commercialFlight);
        }

        // POST: mvcCommercialFlights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idFlight,Distance,Deport,Arrive,Price,Disponible_Places,Flight_To,Flight_From,Flight_Plane")] CommercialFlight commercialFlight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commercialFlight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Flight_From = new SelectList(db.Destinations, "idAirport", "nameAirport", commercialFlight.Flight_From);
            ViewBag.Flight_To = new SelectList(db.Destinations, "idAirport", "nameAirport", commercialFlight.Flight_To);
            ViewBag.Flight_Plane = new SelectList(db.Planes, "idPlane", "namePlane", commercialFlight.Flight_Plane);
            return View(commercialFlight);
        }

        // GET: mvcCommercialFlights/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommercialFlight commercialFlight = db.CommercialFlights.Find(id);
            if (commercialFlight == null)
            {
                return HttpNotFound();
            }
            return View(commercialFlight);
        }

        // POST: mvcCommercialFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CommercialFlight commercialFlight = db.CommercialFlights.Find(id);
            db.CommercialFlights.Remove(commercialFlight);
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
