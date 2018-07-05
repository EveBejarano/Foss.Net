using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using FunTourDataLayer.AccountManagement;
using FunTourDataLayer.BusCompany;
using FunTourDataLayer.EventCompany;
using FunTourDataLayer.FlightCompany;
using FunTourDataLayer.Locality;
using FunTourDataLayer.Reservation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FunTourDataLayer
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<RoleDetails> RoleDetails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusCompany.BusCompany> BusCompanies { get; set; }
        public DbSet<BusReservedSeat> BusReservedSeats { get; set; }
        public DbSet<EventCompany.EventCompany> EventCompanies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightCompany.FlightCompany> FlightCompanies { get; set; }
        public DbSet<Hotel.Hotel> Hotels { get; set; }
        public DbSet<Reservation.Reservation> Reservations { get; set; }
        public DbSet<ReservedRoom> ReservedRooms { get; set; }
        public DbSet<ReservedSeat> ReservedSeats { get; set; }
        public DbSet<ReservedTicket> ReservedTickets { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }

        public DbSet<Destination> Destinations { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // many to many relationship
            modelBuilder.Entity<Permission>()
                .HasMany(p => p.Roles)
                .WithMany(p => p.Permissions)

                // If you want to specify the join table name and the names of the columns in the table you need to do additional configuration by using the Map method.
                .Map(m =>
                   m.ToTable("Roles_Permissions")
                   .MapLeftKey("Id_Permission")
                   .MapRightKey("Id_Role")
                );
            modelBuilder.Entity<Reservation.Reservation>()
                .HasOptional(s => s.BusReservedSeat)
                .WithRequired(a => a.Reservation);


            modelBuilder.Entity<Reservation.Reservation>()
                .HasOptional(s => s.ReservedSeat)
                .WithRequired(a => a.Reservation);


            modelBuilder.Entity<Reservation.Reservation>()
                .HasOptional(s => s.ReservedRoom)
                .WithRequired(a => a.Reservation);

            modelBuilder.Entity<Reservation.Reservation>()
                .HasOptional(s => s.ReservedTicket)
                .WithRequired(a => a.Reservation);


        }
    }
}