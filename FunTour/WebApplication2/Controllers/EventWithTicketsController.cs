using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events.DAL;
using Events.Models;

namespace Events.Controllers
{
    public class EventWithTicketsController : Controller
    {
        private EventsContext db = new EventsContext();

        // GET: EventWithTickets
        public ActionResult Index()
        {
            var eventsWithTickets = db.EventsWithTickets.Include(e => e.City).Include(e => e.Transport).Include(e => e.Tickets);
            return View(eventsWithTickets.ToList());
        }

        // GET: EventWithTickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventWithTicket eventWithTicket = db.EventsWithTickets.Find(id);
            if (eventWithTicket == null)
            {
                return HttpNotFound();
            }
            return View(eventWithTicket);
        }

        // GET: EventWithTickets/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.TransportID = new SelectList(db.Transports, "TransportID", "TransportType");
            return View();
        }

        // POST: EventWithTickets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventWithTicketID,CityID,TransportID,EventDate,Description,Addres,HasTickets,MaxTicket")] EventWithTicket eventWithTicket)
        {
            if (ModelState.IsValid)
            {
                    if (eventWithTicket.EventDate >= DateTime.Now)
                    {
                        db.EventsWithTickets.Add(eventWithTicket);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                return View("DateError") ;
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", eventWithTicket.CityID);
            ViewBag.TransportID = new SelectList(db.Transports, "TransportID", "TransportType", eventWithTicket.TransportID);
            return View(eventWithTicket);
        }

        // GET: EventWithTickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventWithTicket eventWithTicket = db.EventsWithTickets.Find(id);
            if (eventWithTicket == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", eventWithTicket.CityID);
            ViewBag.TransportID = new SelectList(db.Transports, "TransportID", "TransportType", eventWithTicket.TransportID);
            return View(eventWithTicket);
        }

        // POST: EventWithTickets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventWithTicketID,CityID,TransportID,EventDate,Description,Addres,HasTickets,MaxTicket")] EventWithTicket eventWithTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventWithTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", eventWithTicket.CityID);
            ViewBag.TransportID = new SelectList(db.Transports, "TransportID", "TransportType", eventWithTicket.TransportID);
            return View(eventWithTicket);
        }

        // GET: EventWithTickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventWithTicket eventWithTicket = db.EventsWithTickets.Find(id);
            if (eventWithTicket == null)
            {
                return HttpNotFound();
            }
            return View(eventWithTicket);
        }

        // POST: EventWithTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventWithTicket eventWithTicket = db.EventsWithTickets.Find(id);
            db.EventsWithTickets.Remove(eventWithTicket);
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
