﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class TravelPackage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TravelPackage()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        [Key]
        public int Id_TravelPackage { get; set; }

        public string PackageName { get; set; }

        public string Description { get; set; }

        public DateTime FromDay { get; set; }
        public DateTime ToDay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual UserDetails Creator { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual Flight Flight { get; set; }
        public virtual Bus Bus { get; set; }
        public virtual Event Event { get; set; }
    }
}