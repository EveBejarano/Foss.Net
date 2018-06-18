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
        private IHotelRepository hotelRepository;

        public HotelsAPIController()
        {
            this.hotelRepository = new HotelRepository(new HotelsContext());
        }

        public HotelsAPIController(IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        [HttpGet]
        [Route("api/Hotel/{id}")]
        [ResponseType(typeof(Hotel))]
        public IHttpActionResult GetHotel(int id)
        {
            var hotel = hotelRepository.GetHotelByID(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        [HttpGet]
        [Route("api/HotelsByZipCode/{zip}")]
        public IQueryable GetHotelsByZipCode(string zip)
        {
            var query = hotelRepository.GetHotelsByZip(zip);
            return query;
        }

        [HttpPost]
        [Route("api/Availability")]
        public List<RoomCount> GetAvailability(Parameters parameters)
        {
            var Availability = hotelRepository.Availability(parameters);
            return Availability;
        }

        [HttpGet]
        [Route("api/Booking/{id}")]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult GetBooking(int id)
        {
            Booking booking = hotelRepository.GetBookingByID(id);
            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpPost]
        [Route("api/CreateBookings")]
        public List<BookData> CreateBookings(BookDTO book)
        {
          return hotelRepository.CreateBookings(book);
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/UpdateBooking")]
        public  IHttpActionResult UpdateBooking(GuestData guestData)
        {
            var update = hotelRepository.UpdateBooking(guestData);
            if (update)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

           
    }
} 

/*
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
                var hotelList = HotelList(parameters).ToList();
                var i = 0;
                var Availability = new List<RoomCount>();
                if (!hotelList.Count.Equals(0))
                {
                    while (i < (hotelList.Count()))
                    {
                        var newRoomCount = (CreateRoomCount(hotelList.ElementAt(i), parameters));
                        Availability.Add(newRoomCount);
                        i++;
                    };
                }
                return Availability;
            }

            public IEnumerable<Hotel> HotelList(Parameters parameters)
            {
                var query =
                    from data in db.Hotel.Include(h => h.RoomTypes)
                                            .Include(h => h.City).Include(h => h.City.Region).Include(h => h.City.Region.Country)
                    where ((data.City.CityName == parameters.City)
                            && (data.City.Region.RegionName == parameters.Region)
                            && (data.City.Region.Country.CountryName == parameters.Country))
                    select data;
                if (query == null)
                { return null; }
                var list = query.ToList();
                var hotelList = new List<Hotel>();
                var i = 0;
                if ((list.Count) > 0)
                {
                    while (i < list.Count())
                    {
                        if ((CountRooms((list.ElementAt(i).RoomTypes.ElementAt(0)).RoomTypeID, parameters)) > 0)
                        {
                            hotelList.Add(list.ElementAt(i));
                        }
                        i++;
                    }
                }
                return hotelList;
            }

            public RoomCount CreateRoomCount(Hotel hotel, Parameters parameters)
            {

                    var roomCount  =  new RoomCount
                    {
                        HotelID = hotel.HotelID,
                        HotelName = hotel.HotelName,
                        HotelAddress = hotel.HotelAddress,
                        HotelCity = hotel.City.CityName,
                        HotelRegion = hotel.City.Region.RegionName,
                        HotelCountry = hotel.City.Region.Country.CountryName,
                        StandardRate = (hotel.RoomTypes.ElementAt(0)).StandardRate,
                        FreeRoomCount = CountRooms((hotel.RoomTypes.ElementAt(0)).RoomTypeID, parameters)
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
                        AgentID = 1,
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

                 return await PutBooking(updatedBooking.BookingID, updatedBooking);
                return booking;
            }

            public IEnumerable<Room> AvRooms(BookDTO parameters)
            {
                var v =
                    from book in db.Booking
                    where ((book.Room.RoomType.HotelID == parameters.HotelID) &&
                    (book.StartDate >= parameters.Date_start) &&
                    (book.EndDate <= parameters.Date_end))
                    select book;
                var query =
                    from data in db.Room
                    where (data.RoomType.HotelID == parameters.HotelID) && (!v.Any(book => (book.RoomID == data.RoomID)))
                    select data;

                if (query == null)
                { return null; }

                return query.ToList();
            }

            private bool BookingExists(int id)
            {
                return db.Booking.Count(e => e.BookingID == id) > 0;
            }
            */