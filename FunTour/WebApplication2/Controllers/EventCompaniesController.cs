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

namespace WebApplication2.Controllers
{
    public class EventCompaniesController : Controller
    {
        private EventsContext db = new EventsContext();

        // GET: EventCompanies
        public ActionResult Index()
        {
            return View(db.EventCompanies.ToList());
        }

        // GET: EventCompanies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCompany eventCompany = db.EventCompanies.Find(id);
            if (eventCompany == null)
            {
                return HttpNotFound();
            }
            return View(eventCompany);
        }

        // GET: EventCompanies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventCompanies/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyID,Name,FoundedYear,Description")] EventCompany eventCompany)
        {
            if (ModelState.IsValid)
            {
                db.EventCompanies.Add(eventCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventCompany);
        }

        // GET: EventCompanies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCompany eventCompany = db.EventCompanies.Find(id);
            if (eventCompany == null)
            {
                return HttpNotFound();
            }
            return View(eventCompany);
        }

        // POST: EventCompanies/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyID,Name,FoundedYear,Description")] EventCompany eventCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventCompany);
        }

        // GET: EventCompanies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCompany eventCompany = db.EventCompanies.Find(id);
            if (eventCompany == null)
            {
                return HttpNotFound();
            }
            return View(eventCompany);
        }

        // POST: EventCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventCompany eventCompany = db.EventCompanies.Find(id);
            db.EventCompanies.Remove(eventCompany);
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