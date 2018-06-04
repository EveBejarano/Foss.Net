using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
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

            public IQueryable GetHotelsByCity(string city)
            {
                var query =
                    from data in context.Hotel
                    where data.HotelCity.Equals(city)
                    select new { data.HotelID, data.HotelName, data.HotelAddress, data.Country.CountryName, data.StarRating.RatingImage };
                return query;

            }

            public Hotel GetHotelByID(int? id)
            {
                var hotel = context.Hotel
                .Include(h => h.RoomTypes)
                .SingleOrDefault(h => h.HotelID == id);
                return hotel;
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

            public IQueryable getCountries()
            {
                var countriesQuery = context.Country;
                return countriesQuery;
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

    }
}
