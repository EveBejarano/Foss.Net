using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.AccountManagement;
using FunTourDataLayer.BusCompany;
using FunTourDataLayer.EventCompany;
using FunTourDataLayer.FlightCompany;
using FunTourDataLayer.Locality;

namespace FunTourDataLayer.Reservation
{
    public partial class TravelPackage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TravelPackage()
        {
            this.Reservations = new HashSet<Reservation>();
            this.ReservationAmount =0;

            this.TotalPrice = 0;
        }

        [Key]
        public int Id_TravelPackage { get; set; }

        public string PackageName { get; set; }

        public string Description { get; set; }

        public DateTime FromDay { get; set; }
        public DateTime ToDay { get; set; }

        public virtual City FromPlace { get; set; }
        public virtual City ToPlace { get; set; }

        public bool FlightOrBus { get; set; }

        public float TotalPrice { get; set; }

        public int ReservationAmount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual UserDetails Creator { get; set; }
        public virtual Hotel.Hotel Hotel { get; set; }
        public virtual Flight ToGoFlight { get; set; }
        public virtual Flight ToBackFlight { get; set; }
        public virtual Bus ToGoBus { get; set; }
        public virtual Bus ToBackBus { get; set; }
        public virtual Event Event { get; set; }

        public void SetReservationAmount()
        {
            this.ReservationAmount = this.Hotel.NotReservedRooms;
            if (this.ReservationAmount > this.Event.AvailableTickets)
            {
                this.ReservationAmount = this.Event.AvailableTickets;
            }
            if (this.FlightOrBus)
            {
                if (this.ReservationAmount > this.ToGoFlight.NotReservedSeats)
                {
                    this.ReservationAmount = this.ToGoFlight.NotReservedSeats;
                }

                if (this.ReservationAmount > this.ToBackFlight.NotReservedSeats)
                {
                    this.ReservationAmount = this.ToBackFlight.NotReservedSeats;
                }
            }
            else
            {
                if (this.ReservationAmount > this.ToGoBus.NotReservedSeats)
                {
                    this.ReservationAmount = this.ToGoBus.NotReservedSeats;
                }

                if (this.ReservationAmount > this.ToBackBus.NotReservedSeats)
                {
                    this.ReservationAmount = this.ToBackBus.NotReservedSeats;
                }
            }
        }

        public void SetPrice()
        {
            this.TotalPrice += this.Hotel.Price + this.Event.Price;

            if (this.FlightOrBus)
            {
                this.TotalPrice += this.ToGoFlight.Price + this.ToBackFlight.Price;
            }
            else
            {
                this.TotalPrice += this.ToGoBus.Price + this.ToBackBus.Price;
            }
        }
    }
}