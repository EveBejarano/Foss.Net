using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.Reservation;

namespace FunTourDataLayer.FlightCompany
{
    public partial class Flight 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Flight()
        {
            this.ReservedSeat = new HashSet<ReservedSeat>();
            this.TravelPackage = new HashSet<TravelPackage>();
        }

        [Key]
        public int Id_Flight { get; set; }

        public int TripID { get; set; }
        public string Description { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivedDate { get; set; }
        public float Price { get; set; }
        public int NotReservedSeats { get; set; }
        public string Flight_To { get; set; }
        public string Flight_From { get; set; }


        public virtual Destination Destination { get; set; }
        public virtual Destination Origin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservedSeat> ReservedSeat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelPackage> TravelPackage { get; set; }
        public virtual FlightCompany FlightCompany { get; set; }

        public IEnumerable<object> DesearializeJson(string data)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> MappingJson(object APIResponse)
        {
            throw new NotImplementedException();
        }

        public void ReLoadTable()
        {
            throw new NotImplementedException();
        }
    }
}