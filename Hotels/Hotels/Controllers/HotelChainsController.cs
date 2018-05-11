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
    public class HotelChainsController : Controller
    {
        private HotelsContext db = new HotelsContext();

        // GET: HotelChains
        public async Task<ActionResult> Index()
        {
            return View(await db.HotelChain.ToListAsync());
        }

        // GET: HotelChains/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelChain hotelChain = await db.HotelChain.FindAsync(id);
            if (hotelChain == null)
            {
                return HttpNotFound();
            }
            return View(hotelChain);
        }

        // GET: HotelChains/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HotelChains/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ChainID,ChainName")] HotelChain hotelChain)
        {
            if (ModelState.IsValid)
            {
                db.HotelChain.Add(hotelChain);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hotelChain);
        }

        // GET: HotelChains/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelChain hotelChain = await db.HotelChain.FindAsync(id);
            if (hotelChain == null)
            {
                return HttpNotFound();
            }
            return View(hotelChain);
        }

        // POST: HotelChains/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ChainID,ChainName")] HotelChain hotelChain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotelChain).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hotelChain);
        }

        // GET: HotelChains/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelChain hotelChain = await db.HotelChain.FindAsync(id);
            if (hotelChain == null)
            {
                return HttpNotFound();
            }
            return View(hotelChain);
        }

        // POST: HotelChains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            HotelChain hotelChain = await db.HotelChain.FindAsync(id);
            db.HotelChain.Remove(hotelChain);
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
