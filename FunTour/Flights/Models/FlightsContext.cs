using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Flights.Models
{
    public class FlightsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public FlightsContext() : base("name=FlightsContext")
        {
            // New code:
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<Destination> Destinations { get; set; }

        public DbSet<FlightPlace> FlightPlaces { get; set; }

        public DbSet<CommercialFlight> CommercialFlights { get; set; }

        public DbSet<Plane> Planes { get; set; }
    }
}
