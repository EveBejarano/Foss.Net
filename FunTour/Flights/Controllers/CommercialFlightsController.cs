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
    public class CommercialFlightsController : ApiController
    {
        private FlightsContext db = new FlightsContext();

        // GET: api/CommercialFlights
        public IQueryable<CommercialFlight> GetCommercialFlights()
        {
            db.CommercialFlights.Include(b => b.From).Include(b => b.To).Include(b => b.Plane);
            return db.CommercialFlights;
            
        }

        // GET: api/CommercialFlights/5
        [ResponseType(typeof(CommercialFlight))]
        public async Task<IHttpActionResult> GetCommercialFlight(string id)
        {
            CommercialFlight commercialFlight = await db.CommercialFlights.FindAsync(id);
            if (commercialFlight == null)
            {
                return NotFound();
            }

            return Ok(commercialFlight);
        }

        //1 Clase y Método para Que Eve pida cosas del vuelo con un cute Json 
        public class FlyingFlight
        {
            public string fromDest { get; set; }
            public string toDest { get; set; }
            public DateTime date { get; set; }
        }

        [HttpGet]
        [Route("api/GetFlightsToChoose")]
        [ResponseType(typeof(CommercialFlight))]

        /* var query =
            from data in context.Hotel.Include(h => h.RoomTypes)
                                    .Include(h => h.City).Include(h => h.City.Region).Include(h => h.City.Region.Country)
            where ((data.City.CityName == parameters.City)
                    && (data.City.Region.RegionName == parameters.Region)
                    && (data.City.Region.Country.CountryName == parameters.Country))
            select data
               if (query == null)
            { return null; }
            var list = query.ToList();
            var hotelList = new List<Hotel>();
            var i = 0;
            if ((list.Count) > 0)
            {
                while (i < list.Count())
                {
                    if ((CountRooms((list.ElementAt(i).RoomTypes.ElementAt(0)).RoomTypeID, parameters)) > 0)
                    {
                        hotelList.Add(list.ElementAt(i));
                    }
                    i++;
                }
            }
            return hotelList;*/

        public IHttpActionResult GetFlightsToChoose(FlyingFlight flight)
        {
           
            var query = from data in db.CommercialFlights.Include(p=> p.Plane).Include(p=> p.To).Include(p=> p.From)
                        where (
                            data.FlightTo == flight.toDest &&
                            data.FlightFrom == flight.fromDest &&
                            data.Deport == flight.date )
                        select data;

            if (query == null)
            {
                return NotFound();
            }

            var flightsList = new List<CommercialFlight>();
            if (query != null)
            {
                var list = query.ToList();

                
                var i = 0;
                if ((list.Count) > 0)
                {
                    while (i < list.Count())
                    {
                        flightsList.Add(list.ElementAt(i));
                        i++;
                    }
                }                
            }
            return Ok(flightsList);
        }


        // PUT: api/CommercialFlights/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCommercialFlight(string id, CommercialFlight commercialFlight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commercialFlight.IdFlight)
            {
                return BadRequest();
            }

            db.Entry(commercialFlight).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommercialFlightExists(id))
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

        //2 PUT que pre-reserva todos los lugares que eve pide para un vuelo

        [ResponseType(typeof(void))]
        [Route("api/PutEvesPreBooks/{id}/{cant}")]
        public IHttpActionResult PutPreBooks(string id, int cant)

        {
            CommercialFlight vuelo = db.CommercialFlights.Find(id);

            if (id != vuelo.IdFlight)
            {
                return BadRequest();
            }

            vuelo.Disponible_Places = vuelo.Disponible_Places - cant;


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(vuelo).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommercialFlightExists(id))
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

        // POST: api/CommercialFlights
        [ResponseType(typeof(CommercialFlight))]
        public async Task<IHttpActionResult> PostCommercialFlight(CommercialFlight commercialFlight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CommercialFlights.Add(commercialFlight);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CommercialFlightExists(commercialFlight.IdFlight))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = commercialFlight.IdFlight }, commercialFlight);
        }

        // DELETE: api/CommercialFlights/5
        [ResponseType(typeof(CommercialFlight))]
        public async Task<IHttpActionResult> DeleteCommercialFlight(string id)
        {
            CommercialFlight commercialFlight = await db.CommercialFlights.FindAsync(id);
            if (commercialFlight == null)
            {
                return NotFound();
            }

            db.CommercialFlights.Remove(commercialFlight);
            await db.SaveChangesAsync();

            return Ok(commercialFlight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommercialFlightExists(string id)
        {
            return db.CommercialFlights.Count(e => e.IdFlight == id) > 0;
        }
    }
}