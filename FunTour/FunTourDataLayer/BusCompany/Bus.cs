using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public class Bus: IEntityToReload
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bus()
        {
            this.BusReservedSeat = new HashSet<BusReservedSeat>();
            this.TravelPackage = new HashSet<TravelPackage>();
        }

        [Key]
        public int Id_Bus { get; set; }

        public string Description { get; set; }
        public DateTime DepartureDate{ get; set; }
        public DateTime ArrivedDate { get; set; }
        public float Price { get; set; }
        public int NotReservedSeats { get; set; }


        public virtual BusCompany BusCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusReservedSeat> BusReservedSeat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelPackage> TravelPackage { get; set; }

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