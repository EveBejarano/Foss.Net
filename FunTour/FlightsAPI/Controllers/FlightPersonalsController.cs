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
using FlightsAPI.Models;

namespace FlightsAPI.Controllers
{
    public class FlightPersonalsController : ApiController
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: api/FlightPersonals
        public IQueryable<FlightPersonal> GetFlightPersonals()
        {
            return db.FlightPersonals;
        }

        // GET: api/FlightPersonals/5
        [ResponseType(typeof(FlightPersonal))]
        public IHttpActionResult GetFlightPersonal(string id)
        {
            FlightPersonal flightPersonal = db.FlightPersonals.Find(id);
            if (flightPersonal == null)
            {
                return NotFound();
            }

            return Ok(flightPersonal);
        }

        // PUT: api/FlightPersonals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFlightPersonal(string id, FlightPersonal flightPersonal)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            if (id != flightPersonal.Personal_Rol)
            {
                return BadRequest();
            }

            db.Entry(flightPersonal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightPersonalExists(id))
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

        // POST: api/FlightPersonals
        [ResponseType(typeof(FlightPersonal))]
        public IHttpActionResult PostFlightPersonal(FlightPersonal flightPersonal)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            db.FlightPersonals.Add(flightPersonal);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FlightPersonalExists(flightPersonal.Personal_Rol))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = flightPersonal.Personal_Rol }, flightPersonal);
        }

        // DELETE: api/FlightPersonals/5
        [ResponseType(typeof(FlightPersonal))]
        public IHttpActionResult DeleteFlightPersonal(string id)
        {
            FlightPersonal flightPersonal = db.FlightPersonals.Find(id);
            if (flightPersonal == null)
            {
                return NotFound();
            }

            db.FlightPersonals.Remove(flightPersonal);
            db.SaveChanges();

            return Ok(flightPersonal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FlightPersonalExists(string id)
        {
            return db.FlightPersonals.Count(e => e.Personal_Rol == id) > 0;
        }
    }
}