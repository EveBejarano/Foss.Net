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
    public class FilghtPlacesController : ApiController
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: api/FilghtPlaces
        public IQueryable<FilghtPlace> GetFilghtPlaces()
        {
            return db.FilghtPlaces;
        }

        // GET: api/FilghtPlaces/5
        [ResponseType(typeof(FilghtPlace))]
        public IHttpActionResult GetFilghtPlace(int id)
        {
            FilghtPlace filghtPlace = db.FilghtPlaces.Find(id);
            if (filghtPlace == null)
            {
                return NotFound();
            }

            return Ok(filghtPlace);
        }
        //GET para que Eve encuentre la info del asiento del cliente con el DNI
        [ResponseType(typeof(FilghtPlace))]
        [Route("api/GetEvesPlace4/{DNI}")]
        public IHttpActionResult GetEvesPlace4(int DNI)
        {
            IQueryable<FilghtPlace> filghtPlace = db.FilghtPlaces.Where(e => e.Place_Owner_DNI == DNI);

            if (filghtPlace == null)
            {
                return NotFound();
            }

            return Ok(filghtPlace);
        }



        // PUT: api/FilghtPlaces/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFilghtPlace(int id, FilghtPlace filghtPlace)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            if (id != filghtPlace.numPlace)
            {
                return BadRequest();
            }

            db.Entry(filghtPlace).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilghtPlaceExists(id))
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

        // POST: api/FilghtPlaces
        [ResponseType(typeof(FilghtPlace))]
        public IHttpActionResult PostFilghtPlace(FilghtPlace filghtPlace)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            db.FilghtPlaces.Add(filghtPlace);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FilghtPlaceExists(filghtPlace.numPlace))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = filghtPlace.numPlace }, filghtPlace);
        }

        // POST que le deja a Eve crear un asiento con los datos de su cliente en un vuelo

        public class PlaceData
        {
            public string ClientName { get; set; }
            public int ClientDNI { get; set; }
            public string idFlight { get; set; }

        }


        [ResponseType(typeof(FilghtPlace))]
        [Route("api/PostEvesPlace3")]
        public IHttpActionResult PostEvesPlace3(PlaceData data)
        {
            FilghtPlace place = new FilghtPlace();
            DateTime Actual = DateTime.Now;

            place.Place_Owner_DNI = data.ClientDNI;
            place.Place_Owner_Name = data.ClientName;
            place.idFlight = data.idFlight;
            place.FP_Date = Actual;


            db.FilghtPlaces.Add(place);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FilghtPlaceExists(place.numPlace))
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



        // DELETE: api/FilghtPlaces/5
        [ResponseType(typeof(FilghtPlace))]
        public IHttpActionResult DeleteFilghtPlace(int id)
        {
            FilghtPlace filghtPlace = db.FilghtPlaces.Find(id);
            if (filghtPlace == null)
            {
                return NotFound();
            }

            db.FilghtPlaces.Remove(filghtPlace);
            db.SaveChanges();

            return Ok(filghtPlace);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FilghtPlaceExists(int id)
        {
            return db.FilghtPlaces.Count(e => e.numPlace == id) > 0;
        }
    }
}