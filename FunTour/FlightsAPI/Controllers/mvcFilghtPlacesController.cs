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
    public class mvcFilghtPlacesController : Controller
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: mvcFilghtPlaces
        public ActionResult Index()
        {
            var filghtPlaces = db.FilghtPlaces.Include(f => f.CommercialFlight);
            return View(filghtPlaces.ToList());
        }

        // GET: mvcFilghtPlaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilghtPlace filghtPlace = db.FilghtPlaces.Find(id);
            if (filghtPlace == null)
            {
                return HttpNotFound();
            }
            return View(filghtPlace);
        }

        // GET: mvcFilghtPlaces/Create
        public ActionResult Create()
        {
            ViewBag.idFlight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To");
            return View();
        }

        // POST: mvcFilghtPlaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "numPlace,Place_Owner_Name,Place_Owner_DNI,FP_Date,idFlight")] FilghtPlace filghtPlace)
        {
            if (ModelState.IsValid)
            {
                db.FilghtPlaces.Add(filghtPlace);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idFlight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", filghtPlace.idFlight);
            return View(filghtPlace);
        }

        // GET: mvcFilghtPlaces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilghtPlace filghtPlace = db.FilghtPlaces.Find(id);
            if (filghtPlace == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFlight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", filghtPlace.idFlight);
            return View(filghtPlace);
        }

        // POST: mvcFilghtPlaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "numPlace,Place_Owner_Name,Place_Owner_DNI,FP_Date,idFlight")] FilghtPlace filghtPlace)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filghtPlace).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idFlight = new SelectList(db.CommercialFlights, "idFlight", "Flight_To", filghtPlace.idFlight);
            return View(filghtPlace);
        }

        // GET: mvcFilghtPlaces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilghtPlace filghtPlace = db.FilghtPlaces.Find(id);
            if (filghtPlace == null)
            {
                return HttpNotFound();
            }
            return View(filghtPlace);
        }

        // POST: mvcFilghtPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilghtPlace filghtPlace = db.FilghtPlaces.Find(id);
            db.FilghtPlaces.Remove(filghtPlace);
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
