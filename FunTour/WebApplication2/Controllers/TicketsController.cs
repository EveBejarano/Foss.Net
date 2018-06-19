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
using Events.DAL;
using Events.Models;

namespace Events.Controllers
{
    public class TicketsController : ApiController
    {
        private EventsContext db = new EventsContext();

        // GET: api/Tickets
        public IQueryable<TicketDTO> GetTicketsDisponibles()
        {
            var ticket = from b in db.Tickets
                         select new TicketDTO()
                         {
                             TicketID = b.TicketID,
                             EventWithTicketID = b.EventWithTicketID,
                             Price = b.Price,
                             PersonID = b.DNI
                         };

            return ticket;
        }

        // GET: api/Tickets/5
        [ResponseType(typeof(TicketDTO))]
        public async Task<IHttpActionResult> GetTickets(int id)
        {
            var ticket = await db.Tickets.Include(b => b.EventWithTicket).Select(b =>
       new TicketDTO()
       {
           TicketID = b.TicketID,
           EventWithTicketID = b.EventWithTicketID,
           Price = b.Price,
           PersonID = b.DNI
       }).SingleOrDefaultAsync(b => b.EventWithTicketID == id);

            return Ok(ticket);
        }

        // PUT: api/Tickets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTicket(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.TicketID)
            {
                return BadRequest();
            }

            db.Entry(ticket).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        [ResponseType(typeof(Ticket))]
       public async Task<IHttpActionResult> PostTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EventWithTicket query =
                (from data in db.EventsWithTickets
                 where (data.EventWithTicketID == ticket.EventWithTicketID)
                 select data).SingleOrDefault();
            if (query.MaxTicket > 0)
            {
                try
                {
                    db.Tickets.Add(ticket);
                    query.MaxTicket--;
                    await db.SaveChangesAsync();

                    db.Entry(ticket).Reference(x => x.Person).Load();
                    var dto = new TicketDTO()
                    {
                        TicketID = ticket.TicketID,
                        Price = ticket.Price,
                        EventWithTicketID = ticket.EventWithTicketID,
                        PersonID = ticket.DNI
                    };

                    return CreatedAtRoute("DefaultApi", new { id = ticket.TicketID }, ticket);
                }
                catch{}
            }
            else if (query.MaxTicket == 0) { query.HasTickets = false; db.SaveChanges(); }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> DeleteTicket(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.TicketID == id) > 0;
        }

        [HttpPost]
        [Route("api/TicketsByEvents")] //Muestra los tickets por eventos, ingresando el ID de evento y devolviendo el ID de ticket
                                       //el precio, el ID de evento, el nombre de la persona, y el Apellido
        public IQueryable GetTicketsByEvents(ParametersTicket parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Tickets
                where ((data.EventWithTicketID.Equals(parameters.EventWithTicketID)))
                select new
                {
                    data.TicketID,
                    data.Price,
                    data.EventWithTicketID,
                    data.Person.Name,
                    data.Person.Surname

                };

            return query;

        }

        [HttpPost]
        [Route("api/TicketsByPerson")] //Muestra los tickets por persona, ingresando el nombre y el apellido, y devolviendo el id de ticket
                                       //el precio, el ID de evento, el nombre y el apellido
        public IQueryable GetTicketsByPerson(ParametersTicket parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Tickets
                where ((data.Person.Name.Equals(parameters.Name)) && (data.Person.Surname.Equals(parameters.Surname)))
                select new
                {
                    data.TicketID,
                    data.Price,
                    data.EventWithTicketID,
                    data.Person.Name,
                    data.Person.Surname

                };

            return query;

        }

        [HttpPost]
        [Route("api/TicketsByPrice")] //Muestra los tickets por un rango de precios, ingresando el precio inicial y final,
                                      //y devolviendo el id de ticket, el precio, el ID de evento, el nombre y el apellido
        public IQueryable GetTicketsByPrice(ParametersTicket parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Tickets
                where ((data.Price >= parameters.Price_Start) && (data.Price <= parameters.Price_End))
                select new
                {
                    data.TicketID,
                    data.Price,
                    data.EventWithTicketID,
                    data.Person.Name,
                    data.Person.Surname

                };

            return query;

        }
    }
}