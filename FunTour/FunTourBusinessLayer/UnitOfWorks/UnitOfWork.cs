using System;
using FunTourBusinessLayer.Repositories;
using FunTourDataLayer;
using FunTourDataLayer.AccountManagement;
using FunTourDataLayer.BusCompany;
using FunTourDataLayer.EventCompany;
using FunTourDataLayer.FlightCompany;
using FunTourDataLayer.Hotel;
using FunTourDataLayer.Locality;
using FunTourDataLayer.Reservation;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FunTourBusinessLayer.UnitOfWorks
{
    public class UnitOfWork : IDisposable                                                       
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private RolesRepository<IdentityRole, RoleDetails> rolesRepository;
        private UserRepository<IdentityUser, UserDetails> userRepository;
        private GenericRepository<Permission> permissionRepository;
        private GenericRepository<TravelPackage> travelPackageRepository;
        private GenericRepository<Hotel> hotelRepository;
        private GenericRepository<Flight> flightRepository;
        private GenericRepository<Bus> busRepository;
        private GenericRepository<Event> eventRepository;

        private GenericRepository<City> cityRepository;
        private GenericRepository<FlightCompany> flightCompanyRepository;
        private GenericRepository<BusCompany> busCompanyRepository;
        private GenericRepository<HotelCompany> hotelCompanyRepository;
        private GenericRepository<EventCompany> eventCompanyRepository;

        private GenericRepository<ReservedTicket> reservedTicketRepository { get; set; }
        private GenericRepository<ReservedRoom> reservedRoomRepository { get; set; }
        private GenericRepository<BusReservedSeat> busReservedSeatRepository { get; set; }
        private GenericRepository<ReservedSeat> reservedSeatRepository { get; set; }

        public RolesRepository<IdentityRole,RoleDetails> RolesRepository
        {
            get
            {

                if (this.rolesRepository == null)
                {
                    this.rolesRepository = new RolesRepository<IdentityRole,RoleDetails>(context);
                }
                return rolesRepository;
            }
        }

        public UserRepository<IdentityUser,UserDetails> UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository<IdentityUser,UserDetails>(context);
                }
                return userRepository;
            }
        }

        public GenericRepository<Permission> PermissionRepository
        {
            get
            {

                if (this.permissionRepository == null)
                {
                    this.permissionRepository = new GenericRepository<Permission>(context);
                }
                return permissionRepository;
            }
        }

        public GenericRepository<TravelPackage> TravelPackageRepository
        {
            get
            {
                if (this.travelPackageRepository == null)
                {
                    this.travelPackageRepository = new GenericRepository<TravelPackage>(context);
                }
                return travelPackageRepository;
            }
        }




        public GenericRepository<Hotel> HotelRepository
        {
            get
            {

                if (this.hotelRepository == null)
                {
                    this.hotelRepository = new GenericRepository<Hotel>(context);
                }
                return hotelRepository;
            }
        }

        public GenericRepository<Flight> FlightRepository
        {
            get
            {

                if (this.flightRepository == null)
                {
                    this.flightRepository = new GenericRepository<Flight>(context);
                }
                return flightRepository;
            }
        }

        public GenericRepository<Bus> BusRepository
        {
            get
            {

                if (this.busRepository == null)
                {
                    this.busRepository = new GenericRepository<Bus>(context);
                }
                return busRepository;
            }
        }

        public GenericRepository<Event> EventRepository
        {
            get
            {

                if (this.eventRepository == null)
                {
                    this.eventRepository = new GenericRepository<Event>(context);
                }
                return eventRepository;
            }
        }

        public GenericRepository<City> CityRepository
        {
            get
            {

                if (this.cityRepository == null)
                {
                    this.cityRepository = new GenericRepository<City>(context);
                }
                return cityRepository;
            }
        }


        public GenericRepository<FlightCompany> FlightCompanyRepository
        {
            get
            {

                if (this.flightCompanyRepository == null)
                {
                    this.flightCompanyRepository = new GenericRepository<FlightCompany>(context);
                }
                return flightCompanyRepository;
            }
        }

        public GenericRepository<BusCompany> BusCompanyRepository
        {
            get
            {

                if (this.busCompanyRepository == null)
                {
                    this.busCompanyRepository = new GenericRepository<BusCompany>(context);
                }
                return busCompanyRepository;
            }
        }


        public GenericRepository<HotelCompany> HotelCompanyRepository
        {
            get
            {

                if (this.hotelCompanyRepository == null)
                {
                    this.hotelCompanyRepository = new GenericRepository<HotelCompany>(context);
                }
                return hotelCompanyRepository;
            }
        }


        public GenericRepository<EventCompany> EventCompanyRepository
        {
            get
            {

                if (this.eventCompanyRepository == null)
                {
                    this.eventCompanyRepository = new GenericRepository<EventCompany>(context);
                }
                return eventCompanyRepository;
            }
        }

        public GenericRepository<ReservedRoom> ReservedRoomRepository
        {
            get
            {

                if (this.reservedRoomRepository == null)
                {
                    this.reservedRoomRepository = new GenericRepository<ReservedRoom>(context);
                }
                return reservedRoomRepository;
            }
        }

        public GenericRepository<ReservedTicket> ReservedTicketRepository
        {
            get
            {

                if (this.reservedTicketRepository == null)
                {
                    this.reservedTicketRepository = new GenericRepository<ReservedTicket>(context);
                }
                return reservedTicketRepository;
            }
        }

        public GenericRepository<BusReservedSeat> BusReservedSeatRepository
        {
            get
            {

                if (this.busReservedSeatRepository == null)
                {
                    this.busReservedSeatRepository = new GenericRepository<BusReservedSeat>(context);
                }
                return busReservedSeatRepository;
            }
        }

        public GenericRepository<ReservedSeat> ReservedSeatRepository
        {
            get
            {

                if (this.reservedSeatRepository == null)
                {
                    this.reservedSeatRepository = new GenericRepository<ReservedSeat>(context);
                }
                return reservedSeatRepository;
            }
        }



        public void Save()
        {
            context.SaveChanges();
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
        
    }
}
