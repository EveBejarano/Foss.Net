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

        // GET: api/TicketsAPI
        public IQueryable<TicketDTO> GetTicketsDisponibles()
        {
            var ticket = from b in db.Tickets
                         select new TicketDTO()
                         {
                             ReservaID = b.ReservaID,
                             EventWithTicketID = b.Tick.EventWithTicketID,
                             PersonID = b.PersonID,
                             DNI = b.Person.DNI,
                             Price = b.Tick.EventWithTicketID
                         };

            return ticket;
        }

        // GET: api/TicketsAPI/5
        [ResponseType(typeof(TicketDTO))]
        public async Task<IHttpActionResult> GetTickets(int id)
        {
            var ticket = await db.Tickets.Include(b => b.Tick).Select(b =>
       new TicketDTO()
       {
           ReservaID = b.ReservaID,
           EventWithTicketID = b.Tick.EventWithTicketID,
           PersonID = b.PersonID,
           DNI = b.Person.DNI,
           Price = b.Tick.EventWithTicketID
       }).SingleOrDefaultAsync(b => b.EventWithTicketID == id);

            return Ok(ticket);
        }

        // PUT: api/TicketsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTicket(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.ReservaID)
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

        // POST: api/TicketsAPI
        [ResponseType(typeof(Tick))]
       public async Task<IHttpActionResult> PostTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tick query =
                (from data in db.Ticks
                 where (data.EventWithTicketID == ticket.Tick.EventWithTicketID)
                 select data).SingleOrDefault();
            if (query.EventWithTicket.MaxTicket > 0)
            {
                try
                {
                    db.Tickets.Add(ticket);
                    query.EventWithTicket.MaxTicket--;
                    await db.SaveChangesAsync();

                    db.Entry(ticket).Reference(x => x.Person).Load();
                    var dto = new TicketDTO()
                    {
                        ReservaID = ticket.ReservaID,
                        EventWithTicketID = ticket.Tick.EventWithTicketID,
                        PersonID = ticket.PersonID,
                        DNI = ticket.Person.DNI,
                        Price = ticket.Tick.EventWithTicket.Price
                    };

                    return CreatedAtRoute("DefaultApi", new { id = ticket.ReservaID }, ticket);
                }
                catch{}
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/TicketsAPI/5
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
            return db.Tickets.Count(e => e.ReservaID == id) > 0;
        }

        [HttpPost]
        [Route("api/TicketsByEvents")] //Muestra las reservas por eventos, ingresando el ID de evento y devolviendo el ID de ticket
                                       //el precio, el ID de evento, el nombre de la persona, y el Apellido
        public IQueryable GetTicketsByEvents(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Tickets
                where ((data.Tick.EventWithTicketID.Equals(parameters.EventWithTicketsID)))
                select new
                {
                    data.ReservaID,
                    data.Tick.EventWithTicketID,
                    data.Tick.EventWithTicket.Price,
                    data.Person.DNI,
                    data.Person.Name,
                    data.Person.Surname

                };

            return query;

        }

        [HttpPost]
        [Route("api/TicketsByPerson")] //Muestra las reservas por persona, ingresando el dni, y devolviendo el id de ticket
                                       //el precio, el ID de evento, el nombre y el apellido
        public IQueryable GetTicketsByPerson(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Tickets
                where ((data.Person.DNI.Equals(parameters.DNI)))
                select new
                {
                    data.ReservaID,
                    data.Tick.EventWithTicketID,
                    data.Tick.EventWithTicket.Price,
                    data.Person.DNI,
                    data.Person.Name,
                    data.Person.Surname

                };

            return query;

        }
    }
}