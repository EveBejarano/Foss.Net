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
using BusAPI.DAL;
using BusAPI.Models;

namespace BusAPI.Controllers
{
    public class TripsAPIController : ApiController
    {
        private BusContext db = new BusContext();

        // GET: api/TripsAPI
        public IQueryable<Trip> GetTrips()
        {
            return db.Trips;
        }

        // GET: api/TripsAPI/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult GetTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        // PUT: api/TripsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrip(int id, Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trip.TripID)
            {
                return BadRequest();
            }

            db.Entry(trip).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
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

        // POST: api/TripsAPI
        [ResponseType(typeof(Trip))]
        public IHttpActionResult PostTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trips.Add(trip);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trip.TripID }, trip);
        }

        // DELETE: api/TripsAPI/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult DeleteTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            db.Trips.Remove(trip);
            db.SaveChanges();

            return Ok(trip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TripExists(int id)
        {
            return db.Trips.Count(e => e.TripID == id) > 0;
        }
        
        [HttpPost]
        [Route("api/Trips")]
        public IQueryable GetTripsAvailable(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Trips
                where (data.DateTimeTrip >= parameters.Date) && (data.OriginCity.CityName.Equals(parameters.Origin)) && (data.DestinationCity.CityName.Equals(parameters.Destination))
                select new
                {
                    data.TripID,
                    Origin = data.OriginCity.CityName,
                    Destination = data.DestinationCity.CityName,
                    DateTimeDeparture = data.DateTimeTrip,
                    data.DateTimeArrival,
                    data.Bus.Company,
                    data.Bus.Class,
                    data.Bus.Capacity,
                    data.Price,

                };

            return query;
        }
        
    }
}