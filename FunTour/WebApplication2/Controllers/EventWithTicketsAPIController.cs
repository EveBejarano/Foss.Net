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
using System.Threading.Tasks;
using Events.DAL;
using Events.Models;

namespace Events.Controllers
{
    public class EventWithTicketsAPIController : ApiController
    {
        private EventsContext db = new EventsContext();

        // GET: api/EventWithTicketsAPI
        public IQueryable<EventWithTicketDTO> GetEventsWithTickets()
        {
            var evento = from b in db.EventsWithTickets
                         select new EventWithTicketDTO()
                         {
                             EventWithTicketID = b.EventWithTicketID,
                             Name = b.Name,
                             Description = b.Description,
                             City = b.City.CityName,
                             Transport = b.Transport.TransportType,
                             EventDate = b.EventDate,
                             Adress = b.Addres,
                             Price = b.Price,
                             MaxTickets = b.MaxTicket
                         };

            return evento;
        }

        // GET: api/EventWithTicketsAPI/5
        [ResponseType(typeof(EventWithTicketDTO))]
        public async Task<IHttpActionResult> GetEvento(int id)
        {
            var evento = await db.EventsWithTickets.Include(b => b.City).Select(b =>
       new EventWithTicketDTO()
       {
           EventWithTicketID = b.EventWithTicketID,
           Name = b.Name,
           Description = b.Description,
           City = b.City.CityName,
           Transport = b.Transport.TransportType,
           EventDate = b.EventDate,
           Adress = b.Addres,
           Price = b.Price,
           MaxTickets = b.MaxTicket
       }).SingleOrDefaultAsync(b => b.EventWithTicketID == id);

            return Ok(evento);
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
        public async Task<IHttpActionResult> PostEventWithTicket(EventWithTicket eventWithTicket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventsWithTickets.Add(eventWithTicket);
            await db.SaveChangesAsync();

            // New code:
            // Load Evento
            db.Entry(eventWithTicket).Reference(x => x.City).Load();

            var dto = new EventWithTicketDTO()
            {
                EventWithTicketID = eventWithTicket.EventWithTicketID,
                Name = eventWithTicket.Name,
                Description = eventWithTicket.Description,
                City = eventWithTicket.City.CityName,
                Transport = eventWithTicket.Transport.TransportType,
                EventDate = eventWithTicket.EventDate,
                Adress = eventWithTicket.Addres,
                Price = eventWithTicket.Price,
                MaxTickets = eventWithTicket.MaxTicket
            };

            return CreatedAtRoute("DefaultApi", new { id = eventWithTicket.EventWithTicketID }, eventWithTicket);
        }

        // DELETE: api/EventWithTicketsAPI/5
        [ResponseType(typeof(EventWithTicket))]
        public async Task<IHttpActionResult> DeleteEventWithTicket(int id)
        {
            EventWithTicket eventWithTicket = await db.EventsWithTickets.FindAsync(id);
            if (eventWithTicket == null)
            {
                return NotFound();
            }

            db.EventsWithTickets.Remove(eventWithTicket);
            await db.SaveChangesAsync();

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

        [HttpPost]
        [Route("api/EventsByEventCompany")] //Muestra los eventos, ingresando sólo la compañia, y devolviendo el ID de ticket
                                    //la descripcion, la ciudad, el tipo de transporte, la fecha, direccion
                                    //y los tickets maximos
        public IQueryable GetEventsByEventCompany(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.EventsWithTickets
                where (data.EventCompany.CompanyID.Equals(parameters.EventCompany))
                select new
                {
                    data.EventWithTicketID,
                    data.Name,
                    data.Description,
                    data.City.CityName,
                    data.Transport.TransportType,
                    data.EventDate,
                    data.Addres,
                    data.Price,
                    data.MaxTicket

                };

            return query;

        }

        [HttpPost]
        [Route("api/EventsByPrice")] //Muestra los eventos por un rango de precios, ingresando el precio inicial y final,
                                      //y devolviendo los datos del evento
        public IQueryable EventsByPrice(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.EventsWithTickets
                where ((data.Price >= parameters.Price_Start) && (data.Price <= parameters.Price_End))
                select new
                {
                    data.EventWithTicketID,
                    data.Name,
                    data.Description,
                    data.City.CityName,
                    data.Transport.TransportType,
                    data.EventDate,
                    data.Addres,
                    data.Price,
                    data.MaxTicket

                };

            return query;

        }

        [HttpPost]
        [Route("api/TicketsByEventsAmount")] //Muestra los tickets por evento, y devolviendo el id del evento y del ticket
        public IQueryable GetTicketsByEventsAmount(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Ticks
                where ((data.EventWithTicketID.Equals(parameters.EventWithTicketsID)) && (data.EventWithTicket.MaxTicket >= parameters.TicketAmount))
                select new
                {
                    data.EventWithTicketID,
                    data.TicketID
                };

            return query;

        }

        [HttpPost]
        [Route("api/EventsByCity")] //Muestra los eventos, ingresando sólo la ciudad, y devolviendo el ID de ticket
                                    //la descripcion, la ciudad, el tipo de transporte, la fecha, direccion
                                    //y los tickets maximos
        public IQueryable GetEventsByCity(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.EventsWithTickets
                where (data.City.CityName.Equals(parameters.City))
                select new
                {
                    data.EventWithTicketID,
                    data.Name,
                    data.Description,
                    data.City.CityName,
                    data.Transport.TransportType,
                    data.EventDate,
                    data.Addres,
                    data.Price,
                    data.MaxTicket

                };

            return query;

        }

        [HttpPost]
        [Route("api/EventsByCityDate")] //Muestra los eventos, ingresando la ciudad y una fecha inicial y final, y devolviendo el ID de ticket
                                        //la descripcion, la ciudad, el tipo de transporte, la fecha, direccion
                                        //y los tickets maximos
        public IQueryable GetEventsByCityDate(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.EventsWithTickets
                where ((data.City.CityName.Equals(parameters.City)) && (data.EventDate >= parameters.Date_start) && (data.EventDate <= parameters.Date_end))
                select new
                {
                    data.EventWithTicketID,
                    data.Name,
                    data.Description,
                    data.City.CityName,
                    data.Transport.TransportType,
                    data.EventDate,
                    data.Addres,
                    data.Price,
                    data.MaxTicket

                };

            return query;

        }
    }
}