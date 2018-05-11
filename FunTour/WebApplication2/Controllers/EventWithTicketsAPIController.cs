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
using Events.DAL;
using Events.Models;

namespace Events.Controllers
{
    public class EventWithTicketsAPIController : ApiController
    {
        private EventsContext db = new EventsContext();

        // GET: api/EventWithTicketsAPI
        public IQueryable<EventWithTicket> GetEventsWithTickets()
        {
            return db.EventsWithTickets;
        }

        // GET: api/EventWithTicketsAPI/5
        [ResponseType(typeof(EventWithTicket))]
        public IHttpActionResult GetEventWithTicket(int id)
        {
            EventWithTicket eventWithTicket = db.EventsWithTickets.Find(id);
            if (eventWithTicket == null)
            {
                return NotFound();
            }

            return Ok(eventWithTicket);
        }

        // PUT: api/EventWithTicketsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEventWithTicket(int id, EventWithTicket eventWithTicket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventWithTicket.EventWithTicketID)
            {
                return BadRequest();
            }

            db.Entry(eventWithTicket).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventWithTicketExists(id))
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

        // POST: api/EventWithTicketsAPI
        [ResponseType(typeof(EventWithTicket))]
        public IHttpActionResult PostEventWithTicket(EventWithTicket eventWithTicket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventsWithTickets.Add(eventWithTicket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = eventWithTicket.EventWithTicketID }, eventWithTicket);
        }

        // DELETE: api/EventWithTicketsAPI/5
        [ResponseType(typeof(EventWithTicket))]
        public IHttpActionResult DeleteEventWithTicket(int id)
        {
            EventWithTicket eventWithTicket = db.EventsWithTickets.Find(id);
            if (eventWithTicket == null)
            {
                return NotFound();
            }

            db.EventsWithTickets.Remove(eventWithTicket);
            db.SaveChanges();

            return Ok(eventWithTicket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventWithTicketExists(int id)
        {
            return db.EventsWithTickets.Count(e => e.EventWithTicketID == id) > 0;
        }

        [Route("api/EventsWithTickets")]
        public IQueryable GetAvailable()
        {
            var query =
                from data in db.EventsWithTickets
                where data.HasTickets == true
                select new { data.City, data.Addres, data.EventDate, data.Transport, data.MaxTicket, data.Description };

            return query;
        }

        [Route("api/EventsInCity/{city}")]
        public IQueryable GetCityEvents(string cityName)
        {
            var query =
                from data in db.EventsWithTickets
                where data.City.CityName.Equals(cityName)
                select new { data.City, data.Addres, data.EventDate, data.Transport, data.MaxTicket, data.Description };

            return query;
        }

        [Route("api/EventsByDate/{date}")]
        public IQueryable GetCityByDate(DateTime date)
        {
            var query =
                from data in db.EventsWithTickets
                where data.EventDate == date
                select new { data.City, data.Addres, data.EventDate, data.Transport, data.MaxTicket, data.Description };

            return query;
        }

        [Route("api/Events/{date}/{date2}")]
        public IQueryable GetEventsBetweenDates(DateTime date, DateTime date2)
        {
            var query =
                from data in db.EventsWithTickets
                where (data.EventDate >= date) && (data.EventDate <= date2)
                select new { data.City, data.Addres, data.EventDate, data.Transport, data.MaxTicket, data.Description };

            return query;
        }

        [Route("api/Events/{maxPrice}")]
        public IQueryable GetEventsWithPriceLowerThan(double price)
        {
            var query =
                from data in db.Tickets
                where (data.Price <= price)
                select new { data.EventWithTicket.City, data.EventWithTicket.Addres, data.EventWithTicket.EventDate, data.EventWithTicket.Transport, data.EventWithTicket.MaxTicket, data.EventWithTicket.Description };

            return query;

       }
    }
}