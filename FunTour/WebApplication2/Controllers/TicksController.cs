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
    public class TicksController : Controller
    {
        private EventsContext db = new EventsContext();

        // GET: Ticks
        public async Task<ActionResult> Index()
        {
            var ticks = db.Ticks.Include(t => t.EventWithTicket);
            return View(await ticks.ToListAsync());
        }

        // GET: Ticks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tick tick = await db.Ticks.FindAsync(id);
            if (tick == null)
            {
                return HttpNotFound();
            }
            return View(tick);
        }

        // GET: Ticks/Create
        public ActionResult Create()
        {
            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Name");
            return View();
        }

        // POST: Ticks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TicketID,EventWithTicketID,Description")] Tick tick)
        {
            if (ModelState.IsValid)
            {
                db.Ticks.Add(tick);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Name", tick.EventWithTicketID);
            return View(tick);
        }

        // GET: Ticks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tick tick = await db.Ticks.FindAsync(id);
            if (tick == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Name", tick.EventWithTicketID);
            return View(tick);
        }

        // POST: Ticks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TicketID,EventWithTicketID,Description")] Tick tick)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tick).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventWithTicketID = new SelectList(db.EventsWithTickets, "EventWithTicketID", "Name", tick.EventWithTicketID);
            return View(tick);
        }

        // GET: Ticks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tick tick = await db.Ticks.FindAsync(id);
            if (tick == null)
            {
                return HttpNotFound();
            }
            return View(tick);
        }

        // POST: Ticks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tick tick = await db.Ticks.FindAsync(id);
            db.Ticks.Remove(tick);
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
