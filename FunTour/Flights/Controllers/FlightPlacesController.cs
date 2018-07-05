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
    public class FlightPlacesController : ApiController
    {
        private FlightsContext db = new FlightsContext();

        // GET: api/FlightPlaces
        public IQueryable<FlightPlace> GetFlightPlaces()
        {
            db.FlightPlaces.Include(b => b.CommercialFlight);
            return db.FlightPlaces;
        }

        // GET: api/FlightPlaces/5
        [ResponseType(typeof(FlightPlace))]
        public async Task<IHttpActionResult> GetFlightPlace(int id)
        {
            FlightPlace flightPlace = await db.FlightPlaces.FindAsync(id);
            if (flightPlace == null)
            {
                return NotFound();
            }

            return Ok(flightPlace);
        }

        //GET para que Eve encuentre la info del asiento del cliente con el DNI
        [ResponseType(typeof(FlightPlace))]
        [Route("api/GetClientPlaceInfo/{DNI}")]
        public IHttpActionResult GetClientPlaceInfo(int DNI)
        {
            IQueryable<FlightPlace> filghtPlace = db.FlightPlaces.Where(e => e.Place_Owner_DNI == DNI);

            if (filghtPlace == null)
            {
                return NotFound();
            }

            return Ok(filghtPlace);
        }


        // PUT: api/FlightPlaces/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFlightPlace(int id, FlightPlace flightPlace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flightPlace.numPlace)
            {
                return BadRequest();
            }

            db.Entry(flightPlace).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightPlaceExists(id))
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

        // POST: api/FlightPlaces
        [ResponseType(typeof(FlightPlace))]
        public async Task<IHttpActionResult> PostFlightPlace(FlightPlace flightPlace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FlightPlaces.Add(flightPlace);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FlightPlaceExists(flightPlace.numPlace))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = flightPlace.numPlace }, flightPlace);
        }

        // 3 -POST que le deja a Eve crear un asiento con los datos de su cliente en un vuelo

        public class PlaceData
        {
            public string ClientName { get; set; }
            public int ClientDNI { get; set; }
            public string idFlight { get; set; }

        }


        [ResponseType(typeof(FlightPlace))]
        [Route("api/PostPlace")]
        public IHttpActionResult PostPlace(PlaceData data)
        {
            FlightPlace place = new FlightPlace();
            DateTime Actual = DateTime.Now;

            place.Place_Owner_DNI = data.ClientDNI;
            place.Place_Owner_Name = data.ClientName;
            place.idFlight = data.idFlight;
            place.FP_Date = Actual;


            db.FlightPlaces.Add(place);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FlightPlaceExists(place.numPlace))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = place.numPlace }, place);
        }


        // DELETE: api/FlightPlaces/5
        [ResponseType(typeof(FlightPlace))]
        public async Task<IHttpActionResult> DeleteFlightPlace(int id)
        {
            FlightPlace flightPlace = await db.FlightPlaces.FindAsync(id);
            if (flightPlace == null)
            {
                return NotFound();
            }

            db.FlightPlaces.Remove(flightPlace);
            await db.SaveChangesAsync();

            return Ok(flightPlace);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FlightPlaceExists(int id)
        {
            return db.FlightPlaces.Count(e => e.numPlace == id) > 0;
        }
    }
}