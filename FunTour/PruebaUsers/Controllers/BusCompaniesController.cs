using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PruebaUsers.Models;

namespace PruebaUsers.Controllers
{
    public class BusCompaniesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BusCompanies
        public IQueryable<BusCompany> GetBusCompanies()
        {
            return db.BusCompanies;
        }

        // GET: api/BusCompanies/5
        [ResponseType(typeof(BusCompany))]
        public IHttpActionResult GetBusCompany(int id)
        {
            BusCompany busCompany = db.BusCompanies.Find(id);
            if (busCompany == null)
            {
                return NotFound();
            }

            return Ok(busCompany);
        }

        // PUT: api/BusCompanies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBusCompany(int id, BusCompany busCompany)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != busCompany.Id_BusCompany)
            {
                return BadRequest();
            }

            db.Entry(busCompany).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusCompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BusCompanies
        [ResponseType(typeof(BusCompany))]
        public IHttpActionResult PostBusCompany(BusCompany busCompany)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BusCompanies.Add(busCompany);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = busCompany.Id_BusCompany }, busCompany);
        }

        // DELETE: api/BusCompanies/5
        [ResponseType(typeof(BusCompany))]
        public IHttpActionResult DeleteBusCompany(int id)
        {
            BusCompany busCompany = db.BusCompanies.Find(id);
            if (busCompany == null)
            {
                return NotFound();
            }

            db.BusCompanies.Remove(busCompany);
            db.SaveChanges();

            return Ok(busCompany);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusCompanyExists(int id)
        {
            return db.BusCompanies.Count(e => e.Id_BusCompany == id) > 0;
        }


        public class Model
        {
            public string city { get; set; }
            public DateTime date_start { get; set; }
            public DateTime date_end { get; set; }

            public List<SubModel> submodelos { get; set; }
        }


        public class SubModel
        {
            public string city { get; set; }
            public DateTime date_start { get; set; }
            public DateTime date_end { get; set; }

        }

        // GET: api/HotelsAPI/5
        [ResponseType(typeof(Model))]
        [System.Web.Http.Route("api/HotelsAPI/{id}")]
        [System.Web.Http.HttpGet, System.Web.Http.HttpPost]
        public IHttpActionResult GetHotel(int id)
        {
            var model = new Model
            {
                city = "3500",
                date_start = System.DateTime.Now,
                date_end = DateTime.Now,
                submodelos = new List<SubModel>(),
            };

            for (int i = 0; i < 4; i++)
            {

                var Modelito = new SubModel
                {
                    city = "3563",
                    date_start = System.DateTime.Now,
                    date_end = DateTime.Now

                };

                model.submodelos.Add(Modelito);
            }

            return Ok(model);
        }

        [ResponseType(typeof(Model))]
        [System.Web.Http.Route("api/HotelsAPI/Set/{id}")]
        [HttpGet]
        public IHttpActionResult SetHotel(Model model)
            {

            return Ok(model);
        }
    }
}