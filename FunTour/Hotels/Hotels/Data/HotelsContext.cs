using Hotels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hotels.Data
{
    public class HotelsContext : DbContext
    {
        public HotelsContext() : base("HotelsContext")
        {
        }

        public DbSet<Agent> Agent { get; set; }
        public DbSet<HotelChain> HotelChain { get; set; }
        public DbSet<BookingStatus> BookingStatus { get; set; }
        public DbSet<StarRating> StarRating { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<RoomType> RoomType { get; set; }
    }
}