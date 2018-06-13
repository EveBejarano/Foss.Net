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
        

        //Metodo para pedir Lista de Vuelos de Ida o Vuelta
        //ejemplillo: localhost:59596/api/GetCositas/2017-01-18/SAZN%20NQN%20NEU/SAEZ%20EZE%20EZE

        [Route("api/GetCositas/{date}/{FromDest}/{ToDest}")] 
        [ResponseType(typeof(CommercialFlight))]

        public IHttpActionResult GetCositas (DateTime date, string fromDest, string toDest)
            {

            List<CommercialFlight> commercialFlight =
                db.CommercialFlights.Where(p =>
                                                p.Flight_To == toDest &&
                                                p.Flight_From == fromDest &&
                                                p.Deport == date)
         
                                    .ToList();
         

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

        [HttpPost]
        [Route("api/GetEvesFlights")]
        [ResponseType(typeof(CommercialFlight))]

        public IHttpActionResult GetEvesFlights1 (FlyingFlight flight)
        {
            List<CommercialFlight> commercialFlight =
           db.CommercialFlights.Where(p =>
                                           p.Flight_To == flight.toDest &&
                                           p.Flight_From == flight.fromDest &&
                                           p.Deport == flight.date)

                               .ToList();


            if (commercialFlight == null)
            {
                return NotFound();
            }

            return Ok(commercialFlight);
        }


        //Obtener Vuelos por Ciudad

        [ResponseType(typeof(CommercialFlight))]
        [Route("api/GetCommercialFlightToCity/{city}")]
        public IHttpActionResult GetCommercialFlightToCity(string city)
        {
            List<CommercialFlight> commercialFlight = db.CommercialFlights.Where(p => p.Flight_To == city).ToList();

            if (commercialFlight == null)
            {
                return NotFound();
            }

            return Ok(commercialFlight);
        }

        //Obtener Vuelos por Fecha

        [ResponseType(typeof(CommercialFlight))]
        public IHttpActionResult GetCommercialFlightToDeportDate(DateTime date)
        {
            List<CommercialFlight> CommercialFlight = db.CommercialFlights
                                                        .Where(p => p.Deport == date).ToList();

            if (CommercialFlight == null)
            {
                return NotFound();
            }

            return Ok(CommercialFlight);
        }


        //Obtener Vuelos por Destino/Partida


        [ResponseType(typeof(CommercialFlight))]
        public IHttpActionResult GetCommercialFlightToOriginDestination(string origin, string destination)
        {
            List<CommercialFlight> CommercialFlight = db.CommercialFlights
                                                        .Where(p => p.Flight_To == origin
                                                        && p.Flight_From == destination).ToList();

            if (CommercialFlight == null)
            {
                return NotFound();
            }

            return Ok(CommercialFlight);
        }




        // PUT: api/CommercialFlights/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommercialFlight(string id, CommercialFlight commercialFlight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


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
        
        //2 PUT que pre-reserva todos los lugares que eve pide para un vuelo

        [ResponseType(typeof(void))]
        [Route("api/PutEvesPreBooks2/{id}/{cant}")]
        public IHttpActionResult PutEvesPreBooks2(string id, int cant)

        {
            CommercialFlight vuelo = db.CommercialFlights.Find(id);

            if (id != vuelo.idFlight)
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

        //PUT 


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