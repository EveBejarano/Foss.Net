using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusAPI.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BusAPI.Migrations;


namespace BusAPI.DAL
{
    public class BusContext : DbContext
    {
        public BusContext() : base("BusContext")
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* Para que funcione lo de la clave compuesta en Seats */

            modelBuilder.Entity<Seat>()
                .HasKey(s => new { s.SeatID, s.BusID });

            modelBuilder.Entity<Booking>()
                .HasRequired(b => b.Seat)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => new { b.SeatID, b.BusID });

            /* Para poder tener dos claves foráneas en Trip que apunten a la misma tabla (City) */

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.OriginCity)
                .WithMany(c => c.OriginTrips)
                .HasForeignKey(t => t.OriginID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.DestinationCity)
                .WithMany(c => c.DestinationTrips)
                .HasForeignKey(t => t.DestinationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer<BusContext>(
            new MigrateDatabaseToLatestVersion<BusContext, Configuration>());
        }
    }
}