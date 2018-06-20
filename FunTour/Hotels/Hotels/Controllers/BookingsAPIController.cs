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
using Hotels.Data;
using Hotels.Models;

namespace Hotels.Controllers
{
    public class BookingsAPIController : ApiController
    {
        private HotelsContext db = new HotelsContext();

        // GET: api/BookingsAPI
        public IQueryable<Booking> GetBookings()
        {
            return db.Booking;
        }

        // GET: api/BookingsAPI/5
        [ResponseType(typeof(Booking))]
        public async Task<IHttpActionResult> GetBooking(int id)
        {
            Booking booking = await db.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // PUT: api/BookingsAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/BookingsAPI
        [ResponseType(typeof(Booking))]
        public async Task<IHttpActionResult> PostBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Booking.Add(booking);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = booking.BookingID }, booking);
        }

        [HttpPost]
        [Route("api/CreateBookings")]
        public List<Booking> CreateBookings(BookDTO book)
        {
            var roomList = AvRooms(book);
            var bookList = new List<Booking>();
            var i = 0;
            while (i < book.RoomCount)
            {
                var booking = new Booking
                {
                    AgentID = book.AgentID,
                    GuestID = 1,
                    RoomID = (roomList.ElementAt(i).RoomID),
                    StartDate = book.Date_start,
                    EndDate = book.Date_end,
                    StatusID = "RES"
                };
                db.Booking.Add(booking);
                db.SaveChanges();
                bookList.Add(booking);
                i++;
            }
            return bookList;
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/UpdateBooking")]
        public Booking UpdateBooking(GuestData guestData)
        {
            var guest = db.Guest.Where((g => g.GuestName.Equals(guestData.GuestName) && g.GuestEmail.Equals(guestData.GuestEmail)))
                        .Single();
            if (guest == null)
            {
                var newGuest = new Guest
                {
                    GuestName = guestData.GuestName,
                    GuestEmail = guestData.GuestEmail
                };
                db.Guest.Add(newGuest);
                db.SaveChanges();
                guest = newGuest;
            };

            var booking = db.Booking.First(b => b.BookingID.Equals(guestData.BookingID));
            booking.GuestID = guest.GuestID;
            booking.StatusID = "PAG";
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();

            /*
             var booking =
                 (from b in db.Booking
                 where (b.BookingID.Equals(guestData.BookingID))
                 select b).ToList();

             var updatedBooking = new Booking
             {
                 BookingID = booking.Last().BookingID,
                 RoomID = booking.Last().RoomID,
                 AgentID = booking.Last().AgentID,
                 StartDate = booking.Last().StartDate,
                 EndDate = booking.Last().EndDate,
                 GuestID = guest.GuestID,
                 StatusID = "PAG",
             };

             return await PutBooking(updatedBooking.BookingID, updatedBooking);*/
            return booking;
        }



        // DELETE: api/BookingsAPI/5
        [ResponseType(typeof(Booking))]
        public async Task<IHttpActionResult> DeleteBooking(int id)
        {
            Booking booking = await db.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Booking.Remove(booking);
            await db.SaveChangesAsync();

            return Ok(booking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Booking.Count(e => e.BookingID == id) > 0;
        }

        public IEnumerable<Room> AvRooms(BookDTO parameters)
        {
            var v =
                from book in db.Booking
                where ((book.Room.RoomTypeID == parameters.RoomTypeID) &&
                (book.StartDate >= parameters.Date_start) &&
                (book.EndDate <= parameters.Date_end))
                select book;
            var query =
                from data in db.Room
                where (data.RoomTypeID == parameters.RoomTypeID) && (!v.Any(book => (book.RoomID == data.RoomID)))
                select data;

            if (query == null)
            { return null; }

            return query.ToList();

        }
    }
}