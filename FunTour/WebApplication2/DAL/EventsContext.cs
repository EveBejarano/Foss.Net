using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Events.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Events.DAL
{
    public class EventsContext : DbContext
    {
        public EventsContext() : base("EventsCOntext")
        {
        }

        public DbSet<EventWithTicket> EventsWithTickets { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Person> Persons { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}