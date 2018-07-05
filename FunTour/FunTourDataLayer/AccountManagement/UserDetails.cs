using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.Reservation;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FunTourDataLayer.AccountManagement
{
    public partial class UserDetails
    {

        public UserDetails()
        {
            this.Reservation = new HashSet<Reservation.Reservation>();
            this.CreatedPackages = new HashSet<TravelPackage>();
        }


        [Key]
        public int Id_UserDetails { get; set; }


        public string UserName { get; set; }

        public DateTime? LastModified { get; set; }
        
        public bool? Inactive { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        public bool isSysAdmin { get; set; }


        public virtual IdentityUser IdentityUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation.Reservation> Reservation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelPackage> CreatedPackages { get; set; }


    }
}