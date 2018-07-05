namespace Flights.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Flights.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Flights.Models.FlightsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Flights.Models.FlightsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            context.Destinations.AddOrUpdate(d => d.ZipCode,
                new Destination() { ZipCode = "9410",idAirport="USH",NameAirport= "Aeropuerto Internacional Malvinas Argentinas", City="Ushuaia",State="Tierra del Fuego",Country="Argentina" },
                new Destination() { ZipCode = "1804", idAirport = "EZE", NameAirport = "Aeropuerto Internacional Ministro Pistarini", City = "Ezeiza", State = "Buenos Aires", Country = "Argentina" },
                new Destination() { ZipCode = "10000", idAirport = "JFK", NameAirport = "Aeropuerto Internacional John F. Kennedy", City = "New York", State = "New York", Country = "United States" }
                );
            context.CommercialFlights.AddOrUpdate(c => c.IdFlight,
                new CommercialFlight() { IdFlight="AU0000",Distance=34556,Deport= new DateTime(2015, 8, 18, 13, 31, 17), Arrive= new DateTime(2015, 8, 18, 21, 31, 17), Price=3000,Flight_Plane="1",FlightFrom= "9410", FlightTo= "1804" }
                );

            context.Planes.AddOrUpdate(p => p.idPlane,
                new Plane() { idPlane = "1", namePlane = "Boeing 737", CrossingRange = 4567, PlanePlaces = 85 },
                new Plane() { idPlane = "2", namePlane = "Boeing 747", CrossingRange = 8709, PlanePlaces = 415 }
                );
            context.FlightPlaces.AddOrUpdate( f => f.numPlace,
                new FlightPlace() { numPlace = 1, FP_Date = new DateTime(2015, 3, 18, 13, 31, 17), idFlight = "AU0000", Place_Owner_DNI = 30898756, Place_Owner_Name = "Roberto Bolanios" }               
                );

            
        }
    }
}
