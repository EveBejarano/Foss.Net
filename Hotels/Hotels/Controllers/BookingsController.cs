using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotels.Data;
using Hotels.Models;

namespace Hotels.Controllers
{
    public class BookingsController : Controller
    {
        private HotelsContext db = new HotelsContext();

        // GET: Bookings
        public async Task<ActionResult> Index()
        {
            var booking = db.Booking.Include(b => b.Agent).Include(b => b.BookingStatus).Include(b => b.Guest).Include(b => b.Room);
            return View(await booking.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Booking.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.AgentID = new SelectList(db.Agent, "AgentID", "AgentName");
            ViewBag.StatusID = new SelectList(db.BookingStatus, "StatusID", "StatusDescription");
            ViewBag.GuestID = new SelectList(db.Guest, "GuestID", "GuestName");
            ViewBag.RoomID = new SelectList(db.Room, "RoomID", "RoomNumber");
            return View();
        }

        // POST: Bookings/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookingID,AgentID,StatusID,GuestID,RoomID,StartDate,EndDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Booking.Add(booking);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AgentID = new SelectList(db.Agent, "AgentID", "AgentName", booking.AgentID);
            ViewBag.StatusID = new SelectList(db.BookingStatus, "StatusID", "StatusDescription", booking.StatusID);
            ViewBag.GuestID = new SelectList(db.Guest, "GuestID", "GuestName", booking.GuestID);
            ViewBag.RoomID = new SelectList(db.Room, "RoomID", "RoomNumber", booking.RoomID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Booking.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentID = new SelectList(db.Agent, "AgentID", "AgentName", booking.AgentID);
            ViewBag.StatusID = new SelectList(db.BookingStatus, "StatusID", "StatusDescription", booking.StatusID);
            ViewBag.GuestID = new SelectList(db.Guest, "GuestID", "GuestName", booking.GuestID);
            ViewBag.RoomID = new SelectList(db.Room, "RoomID", "RoomNumber", booking.RoomID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookingID,AgentID,StatusID,GuestID,RoomID,StartDate,EndDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agent, "AgentID", "AgentName", booking.AgentID);
            ViewBag.StatusID = new SelectList(db.BookingStatus, "StatusID", "StatusDescription", booking.StatusID);
            ViewBag.GuestID = new SelectList(db.Guest, "GuestID", "GuestName", booking.GuestID);
            ViewBag.RoomID = new SelectList(db.Room, "RoomID", "RoomNumber", booking.RoomID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Booking.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Booking booking = await db.Booking.FindAsync(id);
            db.Booking.Remove(booking);
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
