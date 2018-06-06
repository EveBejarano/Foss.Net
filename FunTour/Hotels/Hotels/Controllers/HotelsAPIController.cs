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
    public class HotelsAPIController : ApiController
    {
        private HotelsContext db = new HotelsContext();


        // GET: api/HotelsAPI
        public IQueryable GetHotel()
        {
            var query =
                    from data in db.Hotel
                    select new { data.HotelID, data.HotelName, data.HotelAddress, data.StarRating.RatingImage };
            return query;
        }

        // GET: api/HotelsAPI/5
        [ResponseType(typeof(Hotel))]
        public IHttpActionResult GetHotel(int id)
        {
            var hotel = db.Hotel
                .Include(h => h.RoomTypes)
                .SingleOrDefault(h => h.HotelID == id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        // PUT: api/HotelsAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHotel(int id, Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotel.HotelID)
            {
                return BadRequest();
            }

            db.Entry(hotel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        // POST: api/HotelsAPI
        [ResponseType(typeof(Hotel))]
        public async Task<IHttpActionResult> PostHotel(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Hotel.Add(hotel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = hotel.HotelID }, hotel);
        }

        // DELETE: api/HotelsAPI/5
        [ResponseType(typeof(Hotel))]
        public async Task<IHttpActionResult> DeleteHotel(int id)
        {
            Hotel hotel = await db.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            db.Hotel.Remove(hotel);
            await db.SaveChangesAsync();

            return Ok(hotel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HotelExists(int id)
        {
            return db.Hotel.Count(e => e.HotelID == id) > 0;
        }

        [Route("api/HotelsbyCity/{city}")]
        public IQueryable GetHotelsbyCity(string city)
        {
            var query =
                from data in db.Hotel
                where data.HotelCity.Equals(city)
                select new { data.HotelID, data.HotelName, data.HotelAddress, data.StarRating.RatingImage };

            return query;

        }

        [Route("api/Availability/{city}/{date_start}/{date_end}")]
        public IQueryable Availability(string city, DateTime date_start, DateTime date_end)
        {
           var v =
                from book in db.Booking
                 where (book.Room.RoomType.Hotel.HotelCity == city) &&
                       (book.StartDate >= date_start) &&
                       (book.EndDate <= date_end)
                 select book;

           var query =
                from data in db.Room
                where (data.RoomType.Hotel.HotelCity == city) && (!v.Any(book=>(book.RoomID == data.RoomID)))
                select new { data.RoomID, data.RoomType.RoomDescription, data.RoomType.StandardRate,
                             data.RoomType.HotelID, data.RoomType.Hotel.HotelName, data.RoomType.Hotel.HotelCity,
                             data.RoomType.Hotel.Country.CountryName};
            return query; 

        }
    }
}