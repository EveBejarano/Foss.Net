using System;
using System.Collections;
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
        public IQueryable GetHotels()
        {
            var query =
                    from data in db.Hotel
                    select new
                    {
                        data.HotelID,
                        data.HotelName,
                        data.HotelAddress,
                        data.StarRating.RatingImage,
                        data.ZipCode,
                        data.City.CityName,
                        data.City.Region.RegionName,
                        data.City.Region.Country.CountryName
                    };
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

        [HttpGet]
        [Route("api/GetHotelsByZipCode/{zip}")]
        public IQueryable GetHotelsByZipCode(string zip)
        {
            var query =
                from data in db.Hotel
                where data.ZipCode.Equals(zip)
                select new
                {
                    data.HotelID,
                    data.HotelName,
                    data.HotelAddress,
                    data.StarRating.RatingImage,
                    data.ZipCode,
                    data.City.CityName,
                    data.City.Region.RegionName,
                    data.City.Region.Country.CountryName
                };
            return query;
        }

        [HttpPost]
        [Route("api/GetAvailability")]
        public List<RoomCount> GetAvailability(Parameters parameters)
        {
            var roomTypeList = RoomTypeList(parameters).ToList();
            var i = 0;
            var Availability = new List<RoomCount>();
            if (!roomTypeList.Count.Equals(0))
            {
                while (i < (roomTypeList.Count()))
                {
                    var newRoomCount = (CreateRoomCount(roomTypeList.ElementAt(i), parameters));
                    Availability.Add(newRoomCount);
                    i++;
                };
            }
            return Availability;
        }

        public IEnumerable<RoomType> RoomTypeList(Parameters parameters)
        {
            var query =
                from data in db.RoomType.Include(h => h.Hotel)
                                        .Include(h => h.Hotel.City).Include(h => h.Hotel.City.Region).Include(h => h.Hotel.City.Region.Country)
                where ((data.Hotel.City.CityName == parameters.City)
                        && (data.Hotel.City.Region.RegionName == parameters.Region)
                        && (data.Hotel.City.Region.Country.CountryName == parameters.Country))
                select data;
            if (query == null)
            { return null; }
            var list = query.ToList();
            var roomTypeList = new List<RoomType>();
            var i = 0;
            if ((list.Count) > 0)
            {
                while (i < list.Count())
                {
                    if ((CountRooms(list.ElementAt(i).RoomTypeID, parameters)) > 0)
                    {
                        roomTypeList.Add(list.ElementAt(i));
                    }
                    i++;
                }
            }
            return roomTypeList;
        }

        public RoomCount CreateRoomCount(RoomType roomType, Parameters parameters)
        {

                var roomCount  =  new RoomCount
                {
                    HotelID = roomType.HotelID,
                    HotelName = roomType.Hotel.HotelName,
                    HotelAddress = roomType.Hotel.HotelAddress,
                    HotelCity = roomType.Hotel.City.CityName,
                    HotelRegion = roomType.Hotel.City.Region.RegionName,
                    HotelCountry = roomType.Hotel.City.Region.Country.CountryName,
                    RoomTypeID = roomType.RoomTypeID,
                    RoomDescription = roomType.RoomDescription,
                    StandardRate = roomType.StandardRate,
                    FreeRoomCount = CountRooms(roomType.RoomTypeID, parameters)
                };
            return roomCount;
        }

        public IEnumerable<Room> AvRooms(int RoomTypeID, Parameters parameters)
        {
            var v =
                from book in db.Booking
                where ((book.Room.RoomTypeID == RoomTypeID) &&
                (book.StartDate >= parameters.Date_start) &&
                (book.EndDate <= parameters.Date_end))
                select book;
            var query =
                from data in db.Room
                where (data.RoomTypeID == RoomTypeID) && (!v.Any(book => (book.RoomID == data.RoomID)))
                select data;

            if (query == null)
            { return null; }

            return query.ToList();

        }

        public int CountRooms(int RoomTypeID, Parameters parameters)
        {
            var list = AvRooms(RoomTypeID, parameters);
            return list.Count();
        }

    }
}