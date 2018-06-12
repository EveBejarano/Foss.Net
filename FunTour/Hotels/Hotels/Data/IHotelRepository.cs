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
        IQueryable GetHotelsByCity(string city);
        IQueryable getCities();
        IQueryable getHotelChains();
        IQueryable getStarRatings();

        Hotel GetHotelByID(int? hotelID);
        void InsertHotel(Hotel hotel);
        void DeleteHotel(int hotelID);
        void UpdateHotel(Hotel hotel);
        void Save();
    }
}
