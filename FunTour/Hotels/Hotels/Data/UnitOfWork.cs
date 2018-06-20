using Hotels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotels.Data
{
    public class UnitOfWork : IDisposable
    {
        private HotelsContext context = new HotelsContext();
        private GenericRepository<Agent> agentRepository;
        private GenericRepository<Country> countryRepository;
        private GenericRepository<HotelChain> hotelchainRepository;
        private GenericRepository<Room> roomRepository;
        private GenericRepository<RoomType> roomtypeRepository;
        private GenericRepository<StarRating> starratingRepository;
        private GenericRepository<BookingStatus> bookingstatusRepository;

        public GenericRepository<Agent> AgentRepository
        {
            get
            {

                if (this.agentRepository == null)
                {
                    this.agentRepository = new GenericRepository<Agent>(context);
                }
                return agentRepository;
            }
        }

        public GenericRepository<BookingStatus> BookingStatusRepository
        {
            get
            {

                if (this.bookingstatusRepository == null)
                {
                    this.bookingstatusRepository = new GenericRepository<BookingStatus>(context);
                }
                return bookingstatusRepository;
            }
        }

        public GenericRepository<StarRating> StarRatingRepository
        {
            get
            {

                if (this.starratingRepository == null)
                {
                    this.starratingRepository = new GenericRepository<StarRating>(context);
                }
                return starratingRepository;
            }
        }

        public GenericRepository<RoomType> RoomTypeRepository
        {
            get
            {

                if (this.roomtypeRepository == null)
                {
                    this.roomtypeRepository = new GenericRepository<RoomType>(context);
                }
                return roomtypeRepository;
            }
        }

        public GenericRepository<Room> RoomRepository
        {
            get
            {

                if (this.roomRepository == null)
                {
                    this.roomRepository = new GenericRepository<Room>(context);
                }
                return roomRepository;
            }
        }

        public GenericRepository<HotelChain> HotelChainRepository
        {
            get
            {

                if (this.hotelchainRepository == null)
                {
                    this.hotelchainRepository = new GenericRepository<HotelChain>(context);
                }
                return hotelchainRepository;
            }
        }

        public GenericRepository<Country> CountryRepository
        {
            get
            {

                if (this.countryRepository == null)
                {
                    this.countryRepository = new GenericRepository<Country>(context);
                }
                return countryRepository;
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