using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Hotels.Models;

namespace Hotels.Data
{
    public class HotelRepository : IHotelRepository, IDisposable
    {
        private HotelsContext context;

        public HotelRepository(HotelsContext context)
        {
            this.context = context;
        }

        public IEnumerable<Hotel> GetHotels()
        {
            return context.Hotel.ToList();
        }

        public IQueryable GetHotelsByZip(string zipcode)
        {
            var query =
                from data in context.Hotel
                where data.ZipCode.Equals(zipcode)
                select new
                {
                    data.HotelID,
                    data.HotelName,
                    data.HotelAddress,
                    data.ZipCode,
                    data.StarRating.RatingImage,
                    data.City.CityName,
                    data.City.Region.RegionName,
                    data.City.Region.Country.CountryName,
                };
            return query;
        }

        public Hotel GetHotelByID(int? id)
        {
            var hotel = context.Hotel
            .Include(h => h.RoomTypes)
            .SingleOrDefault(h => h.HotelID == id);
            return hotel;
        }

        public Booking GetBookingByID(int id)
        {
            var book = context.Booking
            .Include(b => b.Room)
            .SingleOrDefault(b => b.BookingID == id);
            return book;
        }


        public void InsertHotel(Hotel hotel)
        {
            context.Hotel.Add(hotel);
        }

        public void UpdateHotel(Hotel hotel)
        {
            context.Entry(hotel).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void DeleteHotel(int id)
        {
            Hotel hotel = context.Hotel.Find(id);
            context.Hotel.Remove(hotel);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Hotel> HotelList(Parameters parameters)
        {
            var query =
            from data in context.Hotel.Include(h => h.RoomTypes)
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

            var roomCount = new RoomCount
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

        public List<RoomCount> Availability(Parameters parameters)
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

        public IEnumerable<Room> AvRooms(int RoomTypeID, Parameters parameters)
        {
            var v =
            from book in context.Booking
            where ((book.Room.RoomTypeID == RoomTypeID) &&
            (book.StartDate >= parameters.Date_start) &&
            (book.EndDate <= parameters.Date_end))
            select book;
            var query =
            from data in context.Room
            where (data.RoomTypeID == RoomTypeID) && (!v.Any(book => (book.RoomID == data.RoomID)))
            select data;

            if (query == null)
            { return null; }

            return query.ToList();

        }

        public IEnumerable<Room> AvRooms(BookDTO parameters)
        {
            var v =
            from book in context.Booking
            where ((book.Room.RoomType.HotelID == parameters.HotelID) &&
            (book.StartDate >= parameters.Date_start) &&
            (book.EndDate <= parameters.Date_end))
            select book;
            var query =
            from data in context.Room
            where (data.RoomType.HotelID == parameters.HotelID) && (!v.Any(book => (book.RoomID == data.RoomID)))
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


        public IQueryable getCities()
        {
            var citiesQuery = context.City;
            return citiesQuery;
        }

        public IQueryable getHotelChains()
        {
            var chainsQuery = context.HotelChain;
            return chainsQuery;
        }

        public IQueryable getStarRatings()
        {
            var ratingsQuery = context.StarRating;
            return ratingsQuery;
        }

        public List<BookData> CreateBookings(BookDTO book)
        {
            var roomList = AvRooms(book);
            var bookList = new List<Booking>();
            var i = 0; 
            if (roomList.Count() < book.RoomCount)
            {
                return null;
            }

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
                context.Booking.Add(booking);
                context.SaveChanges();
                bookList.Add(booking);
                i++;
            }

            var BDlist = new List<BookData>();
            i = 0;
            if (!bookList.Count.Equals(0))
            {
                while (i < (bookList.Count()))
                {
                    var newBookData = new BookData
                    {
                        BookingID = bookList.ElementAt(i).BookingID,
                        HotelID = ((context.RoomType.Find((context.Room.Find(bookList.ElementAt(i).RoomID)).RoomTypeID)).HotelID)
                    };
                    BDlist.Add(newBookData);
                    i++;
                };
            }
            return BDlist;

        }

        public bool UpdateBooking(GuestData guestData)
        {
            var guest = context.Guest.Where((g => g.GuestName.Equals(guestData.GuestName) && g.GuestEmail.Equals(guestData.GuestEmail)))
                        .Single();
            if (guest == null)
            {
                var newGuest = new Guest
                {
                    GuestName = guestData.GuestName,
                    GuestEmail = guestData.GuestEmail
                };
                context.Guest.Add(newGuest);
                context.SaveChanges();
                guest = newGuest;
            };

            if (!BookingExists(guestData.BookingID))
            {
                return false;
            }
            var booking = context.Booking.First(b => b.BookingID.Equals(guestData.BookingID));
            booking.GuestID = guest.GuestID;  
            booking.StatusID = "PAG";
            context.Entry(booking).State = EntityState.Modified;
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(booking.BookingID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public bool BookingExists(int id)
        {
            return context.Booking.Count(e => e.BookingID == id) > 0;
        }
    }
}