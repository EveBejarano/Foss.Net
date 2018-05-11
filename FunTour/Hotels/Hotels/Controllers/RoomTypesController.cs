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
    public class RoomTypesController : Controller
    {
        private HotelsContext db = new HotelsContext();

        // GET: RoomTypes
        public async Task<ActionResult> Index()
        {
            var roomType = db.RoomType.Include(r => r.Hotel);
            return View(await roomType.ToListAsync());
        }

        // GET: RoomTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomType roomType = await db.RoomType.FindAsync(id);
            if (roomType == null)
            {
                return HttpNotFound();
            }
            return View(roomType);
        }

        // GET: RoomTypes/Create
        public ActionResult Create()
        {
            ViewBag.HotelID = new SelectList(db.Hotel, "HotelID", "HotelName");
            return View();
        }

        // POST: RoomTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RoomTypeID,,HotelID,RoomDescription,StandardRate")] RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                db.RoomType.Add(roomType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.HotelID = new SelectList(db.Hotel, "HotelID", "HotelName", roomType.HotelID);
            return View(roomType);
        }

        // GET: RoomTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomType roomType = await db.RoomType.FindAsync(id);
            if (roomType == null)
            {
                return HttpNotFound();
            }

            ViewBag.HotelID = new SelectList(db.Hotel, "HotelID", "HotelName", roomType.HotelID);
            return View(roomType);
        }

        // POST: RoomTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RoomTypeID,HotelID,RoomDescription,StandardRate")] RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.HotelID = new SelectList(db.Hotel, "HotelID", "HotelName", roomType.HotelID);
            return View(roomType);
        }

        // GET: RoomTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomType roomType = await db.RoomType.FindAsync(id);
            if (roomType == null)
            {
                return HttpNotFound();
            }
            return View(roomType);
        }

        // POST: RoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RoomType roomType = await db.RoomType.FindAsync(id);
            db.RoomType.Remove(roomType);
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
