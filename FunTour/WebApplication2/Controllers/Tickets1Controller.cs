using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events.DAL;
using Events.Models;

namespace WebApplication2.Controllers
{
    public class Tickets1Controller : Controller
    {
        private EventsContext db = new EventsContext();

        // GET: Tickets1
        public async Task<ActionResult> Index()
        {
            var tickets = db.Tickets.Include(t => t.EventWithTicket).Include(t => t.Person);
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets1/Create
        public ActionResult Create()
        {
            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Description");
            ViewBag.DNI = new SelectList(db.Persons, "DNI", "DNI");
            return View();
        }

        // POST: Tickets1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TicketID,Price,EventWithTicketID,DNI")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Description", ticket.EventWithTicketID);
            ViewBag.DNI = new SelectList(db.Persons, "DNI", "DNI", ticket.DNI);
            return View(ticket);
        }

        // GET: Tickets1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Description", ticket.EventWithTicketID);
            ViewBag.DNI = new SelectList(db.Persons, "DNI", "DNI", ticket.DNI);
            return View(ticket);
        }

        // POST: Tickets1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TicketID,Price,EventWithTicketID,DNI")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Description", ticket.EventWithTicketID);
            ViewBag.DNI = new SelectList(db.Persons, "DNI", "DNI", ticket.DNI);
            return View(ticket);
        }

        // GET: Tickets1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();
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
