﻿using System;
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
    public class PlanesController : ApiController
    {
        private FlightsDBEntities db = new FlightsDBEntities();

        // GET: api/Planes
        public IQueryable<Plane> GetPlanes()
        {
            return db.Planes;
        }

        // GET: api/Planes/5
        [ResponseType(typeof(Plane))]
        public IHttpActionResult GetPlane(string id)
        {
            Plane plane = db.Planes.Find(id);
            if (plane == null)
            {
                return NotFound();
            }

            return Ok(plane);
        }

        // PUT: api/Planes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlane(string id, Plane plane)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            if (id != plane.idPlane)
            {
                return BadRequest();
            }

            db.Entry(plane).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaneExists(id))
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

        // POST: api/Planes
        [ResponseType(typeof(Plane))]
        public IHttpActionResult PostPlane(Plane plane)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            db.Planes.Add(plane);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PlaneExists(plane.idPlane))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = plane.idPlane }, plane);
        }

        // DELETE: api/Planes/5
        [ResponseType(typeof(Plane))]
        public IHttpActionResult DeletePlane(string id)
        {
            Plane plane = db.Planes.Find(id);
            if (plane == null)
            {
                return NotFound();
            }

            db.Planes.Remove(plane);
            db.SaveChanges();

            return Ok(plane);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlaneExists(string id)
        {
            return db.Planes.Count(e => e.idPlane == id) > 0;
        }
    }
}