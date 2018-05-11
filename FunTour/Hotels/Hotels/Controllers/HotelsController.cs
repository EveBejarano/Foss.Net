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
    public class HotelsController : Controller
    {
        private HotelsContext db = new HotelsContext();

        // GET: Hotels
        public async Task<ActionResult> Index()
        {
            var hotel = db.Hotel.Include(h => h.Country).Include(h => h.HotelChain).Include(h => h.StarRating);
            return View(await hotel.ToListAsync());
        }

        // GET: Hotels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hotel = await db.Hotel
            .Include(h => h.RoomTypes)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.HotelID == id);

            if (hotel == null)
            {
                return HttpNotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Country, "CountryID", "CountryName");
            ViewBag.ChainId = new SelectList(db.HotelChain, "ChainID", "ChainName");
            ViewBag.RatingID = new SelectList(db.StarRating, "RatingID", "RatingID");
            return View();
        }

        // POST: Hotels/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HotelID,ChainId,CountryID,RatingID,HotelName,HotelAddress,HotelEmail,HotelWebsite,HotelDetails,HotelCity")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotel.Add(hotel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(db.Country, "CountryID", "CountryName", hotel.CountryID);
            ViewBag.ChainId = new SelectList(db.HotelChain, "ChainID", "ChainName", hotel.ChainId);
            ViewBag.RatingID = new SelectList(db.StarRating, "RatingID", "RatingID", hotel.RatingID);
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = await db.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Country, "CountryID", "CountryName", hotel.CountryID);
            ViewBag.ChainId = new SelectList(db.HotelChain, "ChainID", "ChainName", hotel.ChainId);
            ViewBag.RatingID = new SelectList(db.StarRating, "RatingID", "RatingID", hotel.RatingID);
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HotelID,ChainId,CountryID,RatingID,HotelName,HotelAddress,HotelEmail,HotelWebsite,HotelDetails,HotelCity")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.Country, "CountryID", "CountryName", hotel.CountryID);
            ViewBag.ChainId = new SelectList(db.HotelChain, "ChainID", "ChainName", hotel.ChainId);
            ViewBag.RatingID = new SelectList(db.StarRating, "RatingID", "RatingID", hotel.RatingID);
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = await db.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Hotel hotel = await db.Hotel.FindAsync(id);
            db.Hotel.Remove(hotel);
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
