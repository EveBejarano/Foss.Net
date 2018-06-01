using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class BusCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BusCompany()
        {
            this.Bus = new HashSet<Bus>();
        }

        [Key]
        public int Id_BusCompany { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string APIURLToGetSeats { get; set; }
        public string APIURLToReserveSeat { get; set; }
        public string APIURLToCancelReservation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bus> Bus { get; set; }
    }
}