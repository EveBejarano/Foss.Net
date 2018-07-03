using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Flights.Models;

namespace Flights.Controllers
{
    public class DestinationsController : ApiController
    {
        private FlightsContext db = new FlightsContext();

        // GET: api/Destinations
        public IQueryable<Destination> GetDestinations()
        {
            return db.Destinations;
        }

        // GET: api/Destinations/5
        [ResponseType(typeof(Destination))]
        public async Task<IHttpActionResult> GetDestination(string id)
        {
            Destination destination = await db.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            return Ok(destination);
        }

        // PUT: api/Destinations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDestination(string id, Destination destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != destination.ZipCode)
            {
                return BadRequest();
            }

            db.Entry(destination).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(id))
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

        // POST: api/Destinations
        [ResponseType(typeof(Destination))]
        public async Task<IHttpActionResult> PostDestination(Destination destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Destinations.Add(destination);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DestinationExists(destination.ZipCode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = destination.ZipCode }, destination);
        }

        // DELETE: api/Destinations/5
        [ResponseType(typeof(Destination))]
        public async Task<IHttpActionResult> DeleteDestination(string id)
        {
            Destination destination = await db.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            db.Destinations.Remove(destination);
            await db.SaveChangesAsync();

            return Ok(destination);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DestinationExists(string id)
        {
            return db.Destinations.Count(e => e.ZipCode == id) > 0;
        }
    }
}