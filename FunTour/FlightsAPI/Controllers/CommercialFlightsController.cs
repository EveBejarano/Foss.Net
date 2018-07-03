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
    public class CommercialFlightsController : ApiController
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: api/CommercialFlights
        
        public IQueryable<CommercialFlight> GetCommercialFlights()
        {
            return db.CommercialFlights;
        }

        // GET: api/CommercialFlights/5
        [ResponseType(typeof(CommercialFlight))]
        public IHttpActionResult GetCommercialFlight(string id)
        {
            CommercialFlight commercialFlight = db.CommercialFlights.Find(id);
            if (commercialFlight == null)
            {
                return NotFound();
            }

            return Ok(commercialFlight);
        }

        //Obtener Vuelos por Ciudad

        [ResponseType(typeof(CommercialFlight))]
        public IHttpActionResult GetCommercialFlightToCity(string city)
        {
            List<CommercialFlight> commercialFlight = db.CommercialFlights.Where(p => p.Flight_To == city).ToList();

            if (commercialFlight == null)
            {
                return NotFound();
            }

            return Ok(commercialFlight);
        }

        //Obtener Vuelos por 

        // PUT: api/CommercialFlights/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommercialFlight(string id, CommercialFlight commercialFlight)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            //Estos metodos fueron suprimidos segun instrucciones de APU
            //Supuestamente en el Proyecto MVC es donde se deben tratar estos valores
            //En Todos los controladores se hizo lo meesmo

            if (id != commercialFlight.idFlight)
            {
                return BadRequest();
            }

            db.Entry(commercialFlight).State = EntityState.Modified;

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
        public IHttpActionResult PostCommercialFlight(CommercialFlight commercialFlight)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            db.CommercialFlights.Add(commercialFlight);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CommercialFlightExists(commercialFlight.idFlight))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = commercialFlight.idFlight }, commercialFlight);
        }

        // DELETE: api/CommercialFlights/5
        [ResponseType(typeof(CommercialFlight))]
        public IHttpActionResult DeleteCommercialFlight(string id)
        {
            CommercialFlight commercialFlight = db.CommercialFlights.Find(id);
            if (commercialFlight == null)
            {
                return NotFound();
            }

            db.CommercialFlights.Remove(commercialFlight);
            db.SaveChanges();

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
            return db.CommercialFlights.Count(e => e.idFlight == id) > 0;
        }
    }
}