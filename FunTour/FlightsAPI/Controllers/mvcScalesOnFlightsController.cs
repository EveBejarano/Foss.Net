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
    public class mvcScalesOnFlightsController : Controller
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: mvcScalesOnFlights1
        public ActionResult Index()
        {
            var scalesOnFlights = db.ScalesOnFlights.Include(s => s.CommercialFlight).Include(s => s.Destination);
            return View(scalesOnFlights.ToList());
        }

        // GET: mvcScalesOnFlights1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScalesOnFlight scalesOnFlight = db.ScalesOnFlights.Find(id);
            if (scalesOnFlight == null)
            {
                return HttpNotFound();
            }
            return View(scalesOnFlight);
        }

        // GET: mvcScalesOnFlights1/Create
        public ActionResult Create()
        {
            ViewBag.idFligth = new SelectList(db.CommercialFlights, "idFlight", "Flight_To");
            ViewBag.idAirport = new SelectList(db.Destinations, "idAirport", "nameAirport");
            return View();
        }

        // POST: mvcScalesOnFlights1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idScale,Deport,Arrive,idAirport,idFligth")] ScalesOnFlight scalesOnFlight)
        {
            if (ModelState.IsValid)
            {
                db.ScalesOnFlights.Add(scalesOnFlight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idFligth = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", scalesOnFlight.idFligth);
            ViewBag.idAirport = new SelectList(db.Destinations, "idAirport", "nameAirport", scalesOnFlight.idAirport);
            return View(scalesOnFlight);
        }

        // GET: mvcScalesOnFlights1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScalesOnFlight scalesOnFlight = db.ScalesOnFlights.Find(id);
            if (scalesOnFlight == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFligth = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", scalesOnFlight.idFligth);
            ViewBag.idAirport = new SelectList(db.Destinations, "idAirport", "nameAirport", scalesOnFlight.idAirport);
            return View(scalesOnFlight);
        }

        // POST: mvcScalesOnFlights1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idScale,Deport,Arrive,idAirport,idFligth")] ScalesOnFlight scalesOnFlight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scalesOnFlight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idFligth = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", scalesOnFlight.idFligth);
            ViewBag.idAirport = new SelectList(db.Destinations, "idAirport", "nameAirport", scalesOnFlight.idAirport);
            return View(scalesOnFlight);
        }

        // GET: mvcScalesOnFlights1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScalesOnFlight scalesOnFlight = db.ScalesOnFlights.Find(id);
            if (scalesOnFlight == null)
            {
                return HttpNotFound();
            }
            return View(scalesOnFlight);
        }

        // POST: mvcScalesOnFlights1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ScalesOnFlight scalesOnFlight = db.ScalesOnFlights.Find(id);
            db.ScalesOnFlights.Remove(scalesOnFlight);
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
