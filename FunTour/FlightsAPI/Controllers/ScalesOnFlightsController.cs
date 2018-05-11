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
    public class ScalesOnFlightsController : ApiController
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: api/ScalesOnFlights
        public IQueryable<ScalesOnFlight> GetScalesOnFlights()
        {
            return db.ScalesOnFlights;
        }

        // GET: api/ScalesOnFlights/5
        [ResponseType(typeof(ScalesOnFlight))]
        public IHttpActionResult GetScalesOnFlight(string id)
        {
            ScalesOnFlight scalesOnFlight = db.ScalesOnFlights.Find(id);
            if (scalesOnFlight == null)
            {
                return NotFound();
            }

            return Ok(scalesOnFlight);
        }

        // PUT: api/ScalesOnFlights/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutScalesOnFlight(string id, ScalesOnFlight scalesOnFlight)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            if (id != scalesOnFlight.idScale)
            {
                return BadRequest();
            }

            db.Entry(scalesOnFlight).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScalesOnFlightExists(id))
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

        // POST: api/ScalesOnFlights
        [ResponseType(typeof(ScalesOnFlight))]
        public IHttpActionResult PostScalesOnFlight(ScalesOnFlight scalesOnFlight)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            db.ScalesOnFlights.Add(scalesOnFlight);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ScalesOnFlightExists(scalesOnFlight.idScale))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = scalesOnFlight.idScale }, scalesOnFlight);
        }

        // DELETE: api/ScalesOnFlights/5
        [ResponseType(typeof(ScalesOnFlight))]
        public IHttpActionResult DeleteScalesOnFlight(string id)
        {
            ScalesOnFlight scalesOnFlight = db.ScalesOnFlights.Find(id);
            if (scalesOnFlight == null)
            {
                return NotFound();
            }

            db.ScalesOnFlights.Remove(scalesOnFlight);
            db.SaveChanges();

            return Ok(scalesOnFlight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ScalesOnFlightExists(string id)
        {
            return db.ScalesOnFlights.Count(e => e.idScale == id) > 0;
        }
    }
}