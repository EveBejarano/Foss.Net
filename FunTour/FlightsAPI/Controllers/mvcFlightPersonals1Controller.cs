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
    public class mvcFlightPersonals1Controller : Controller
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: mvcFlightPersonals1
        public ActionResult Index()
        {
            var flightPersonals = db.FlightPersonals.Include(f => f.CommercialFlight).Include(f => f.Employee);
            return View(flightPersonals.ToList());
        }

        // GET: mvcFlightPersonals1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightPersonal flightPersonal = db.FlightPersonals.Find(id);
            if (flightPersonal == null)
            {
                return HttpNotFound();
            }
            return View(flightPersonal);
        }

        // GET: mvcFlightPersonals1/Create
        public ActionResult Create()
        {
            ViewBag.Flight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To");
            ViewBag.Employ = new SelectList(db.Employees, "idEmploy", "Employ_Name");
            return View();
        }

        // POST: mvcFlightPersonals1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Personal_Rol,Flight,Employ")] FlightPersonal flightPersonal)
        {
            if (ModelState.IsValid)
            {
                db.FlightPersonals.Add(flightPersonal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Flight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", flightPersonal.Flight);
            ViewBag.Employ = new SelectList(db.Employees, "idEmploy", "Employ_Name", flightPersonal.Employ);
            return View(flightPersonal);
        }

        // GET: mvcFlightPersonals1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightPersonal flightPersonal = db.FlightPersonals.Find(id);
            if (flightPersonal == null)
            {
                return HttpNotFound();
            }
            ViewBag.Flight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", flightPersonal.Flight);
            ViewBag.Employ = new SelectList(db.Employees, "idEmploy", "Employ_Name", flightPersonal.Employ);
            return View(flightPersonal);
        }

        // POST: mvcFlightPersonals1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Personal_Rol,Flight,Employ")] FlightPersonal flightPersonal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flightPersonal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Flight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", flightPersonal.Flight);
            ViewBag.Employ = new SelectList(db.Employees, "idEmploy", "Employ_Name", flightPersonal.Employ);
            return View(flightPersonal);
        }

        // GET: mvcFlightPersonals1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightPersonal flightPersonal = db.FlightPersonals.Find(id);
            if (flightPersonal == null)
            {
                return HttpNotFound();
            }
            return View(flightPersonal);
        }

        // POST: mvcFlightPersonals1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            FlightPersonal flightPersonal = db.FlightPersonals.Find(id);
            db.FlightPersonals.Remove(flightPersonal);
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
