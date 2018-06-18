using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hotels.Models;

namespace Hotels.Data
{
    public interface IHotelRepository : IDisposable
    {
        IEnumerable<Hotel> GetHotels();
        IQueryable GetHotelsByZip(string city);
        IQueryable getCities();
        IQueryable getHotelChains();
        IQueryable getStarRatings();
        bool BookingExists(int id);

        Booking GetBookingByID(int id);
        Hotel GetHotelByID(int? hotelID);

        IEnumerable<Hotel> HotelList(Parameters parameters);
        RoomCount CreateRoomCount(Hotel hotel, Parameters parameters);
        IEnumerable<Room> AvRooms(int RoomTypeID, Parameters parameters);
        IEnumerable<Room> AvRooms(BookDTO parameters);
        int CountRooms(int RoomTypeID, Parameters parameters);
        List<RoomCount> Availability(Parameters parameters);
        List<BookData> CreateBookings(BookDTO book);
        bool UpdateBooking(GuestData guestData);

        void InsertHotel(Hotel hotel);
        void DeleteHotel(int hotelID);
        void UpdateHotel(Hotel hotel);
        void Save();
    }
}
