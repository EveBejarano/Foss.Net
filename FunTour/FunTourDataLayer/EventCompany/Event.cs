using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class Event: IEntityToReload
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.TravelPackage = new HashSet<TravelPackage>();
            this.ReservedTicket = new HashSet<ReservedTicket>();
        }

        [Key]
        public int Id_Event { get; set; }

        public string Name { get; set; }


        public float Price { get; set; }

        public string Description { get; set; }

        public int AvailableTickets{ get; set; }

        public string APIURLToTickets { get; set; }
        public string APIURLToReserveTicket { get; set; }
        public string APIURLToCancelReservation { get; set; }

        public virtual EventCompany EventCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelPackage> TravelPackage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservedTicket> ReservedTicket { get; set; }

        IEnumerable<object> IEntityToReload.DesearializeJson(string data)
        {
            throw new NotImplementedException();
        }

        string IEntityToReload.GenerateParameters()
        {
            throw new NotImplementedException();
        }

        IEnumerable<object> IEntityToReload.MappingJson(object APIResponse)
        {
            throw new NotImplementedException();
        }

        object IEntityToReload.NewEntity(string _parameters)
        {
            throw new NotImplementedException();
        }

        void IEntityToReload.ReLoadTable()
        {
            throw new NotImplementedException();
        }
    }
}