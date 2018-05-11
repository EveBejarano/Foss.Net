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
    public class StarRatingsController : Controller
    {
        private HotelsContext db = new HotelsContext();

        // GET: StarRatings
        public async Task<ActionResult> Index()
        {
            return View(await db.StarRating.ToListAsync());
        }

        // GET: StarRatings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StarRating starRating = await db.StarRating.FindAsync(id);
            if (starRating == null)
            {
                return HttpNotFound();
            }
            return View(starRating);
        }

        // GET: StarRatings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StarRatings/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RatingID,RatingImage")] StarRating starRating)
        {
            if (ModelState.IsValid)
            {
                db.StarRating.Add(starRating);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(starRating);
        }

        // GET: StarRatings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StarRating starRating = await db.StarRating.FindAsync(id);
            if (starRating == null)
            {
                return HttpNotFound();
            }
            return View(starRating);
        }

        // POST: StarRatings/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RatingID,RatingImage")] StarRating starRating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(starRating).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(starRating);
        }

        // GET: StarRatings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StarRating starRating = await db.StarRating.FindAsync(id);
            if (starRating == null)
            {
                return HttpNotFound();
            }
            return View(starRating);
        }

        // POST: StarRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StarRating starRating = await db.StarRating.FindAsync(id);
            db.StarRating.Remove(starRating);
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
