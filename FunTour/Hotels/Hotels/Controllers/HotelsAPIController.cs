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

