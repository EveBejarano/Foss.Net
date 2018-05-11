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
    public class RoomsController : Controller
    {
        private HotelsContext db = new HotelsContext();

        // GET: Rooms
        public async Task<ActionResult> Index()
        {
            var room = db.Room.Include(r => r.RoomType);
            return View(await room.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await db.Room.FindAsync(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {

            ViewBag.RoomTypeID = new SelectList(db.RoomType, "RoomTypeID", "RoomDescription");
            return View();
        }

        // POST: Rooms/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RoomID,RoomTypeID,RoomNumber,RoomDetails")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Room.Add(room);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            ViewBag.RoomTypeID = new SelectList(db.RoomType, "RoomTypeID", "RoomDescription", room.RoomTypeID);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await db.Room.FindAsync(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoomTypeID = new SelectList(db.RoomType, "RoomTypeID", "RoomDescription", room.RoomTypeID);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RoomID,RoomTypeID,RoomNumber,RoomDetails")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RoomTypeID = new SelectList(db.RoomType, "RoomTypeID", "RoomDescription", room.RoomTypeID);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await db.Room.FindAsync(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Room room = await db.Room.FindAsync(id);
            db.Room.Remove(room);
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
