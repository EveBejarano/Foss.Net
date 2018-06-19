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
using BusAPI.DAL;
using BusAPI.Models;

namespace BusAPI.Controllers
{
    public class TripsAPIController : ApiController
    {
        private BusContext db = new BusContext();

        // GET: api/TripsAPI
        public IQueryable<Trip> GetTrips()
        {
            return db.Trips;
        }

        // GET: api/TripsAPI/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult GetTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        // PUT: api/TripsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrip(int id, Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trip.TripID)
            {
                return BadRequest();
            }

            db.Entry(trip).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
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

        // POST: api/TripsAPI
        [ResponseType(typeof(Trip))]
        public IHttpActionResult PostTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trips.Add(trip);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trip.TripID }, trip);
        }

        // DELETE: api/TripsAPI/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult DeleteTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            db.Trips.Remove(trip);
            db.SaveChanges();

            return Ok(trip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TripExists(int id)
        {
            return db.Trips.Count(e => e.TripID == id) > 0;
        }
        
        [HttpPost]
        [Route("api/Trips")]
        public List<TripDetails> GetTripsAvailable(Parameters parameters) //post que devuelve datos de viajes a partir de una fecha, con origen y destino
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query =
                from data in db.Trips.Include(t => t.OriginCity).Include(t => t.DestinationCity).Include(t => t.Bus)
                where (data.DateTimeTrip >= parameters.Date) && (data.OriginCity.CityName.Equals(parameters.Origin)) && (data.DestinationCity.CityName.Equals(parameters.Destination))
                select data;
                if (query == null)
                { return null; }
                var list = query.ToList();
            var Availability = new List<TripDetails>();
            var i = 0;
                if (!list.Count.Equals(0))
                {
                    while (i < list.Count())
                    {
                        var newTripDetails = (CreateTripDetails(list.ElementAt(i)));
                        Availability.Add(newTripDetails);
                        i++;
                    };
                };
            return Availability;
        }
        
        public TripDetails CreateTripDetails (Trip trip)
        {
            var newTD = new TripDetails
            {
                TripID = trip.TripID,
                Origin = trip.OriginCity.CityName,
                Destination = trip.DestinationCity.CityName,
                DateTimeDeparture = trip.DateTimeTrip,
                DateTimeArrival = trip.DateTimeArrival,
                Company = trip.Bus.Company,
                Class = trip.Bus.Class,
                Capacity = trip.Bus.Capacity,
                Price = trip.Price,
                AvailableSeats = (AvSeats(trip.TripID).Count())
            };
            return newTD;
        }
        [HttpPost]
        [Route("api/CreateBookings")]
        public List<BookData> CreateBookings(BookDTO book)
        {
            var seatList = AvSeats(book.TripID);
            var bookList = new List<Booking>();
            var i = 0;
            if (seatList.Count() < book.SeatCount) // si la cantidad de asientos que se quieren reservar son más que los disponibles
            {
                return null;
            }

            while (i < book.SeatCount) //bucle para crear n reservas
            {
                var booking = new Booking
                {
                    TripID = book.TripID,
                    BusID = (db.Trips.Find(book.TripID).BusID),
                    SeatID = (seatList.ElementAt(i).SeatID),
                    ClientID = 1
                };
                db.Bookings.Add(booking);
                db.SaveChanges();
                bookList.Add(booking);
                i++;
            }

            var BDlist = new List<BookData>();
            i = 0;
            if (!bookList.Count.Equals(0)) //si hay asientos disponibles
            {
                while (i < (bookList.Count()))
                {
                    var newBookData = new BookData
                    {
                        BookingID = bookList.ElementAt(i).BookingID,
                        TripID = (db.Bookings.Find(bookList.ElementAt(i).BookingID).TripID)
                    };
                    BDlist.Add(newBookData);
                    i++;
                };
            }
            return BDlist; //devuelve los ids de las reservas creadas
        }

        public List<Seat> AvSeats(int tripID) //devuelve asientos disponibles en un viaje
        {
            var v =
                (from book in db.Bookings
                 where (book.TripID == tripID)
                 select book.SeatID).ToList();
            var Bus = (db.Trips.Find(tripID).BusID);
            var query =
            (from data in db.Seats
            where ((data.BusID == (Bus))
            && (!v.Any(b => (b == data.SeatID))))
            select data).ToList();

            if (query == null)
            { return null; }

            return query;
        }
    }
}