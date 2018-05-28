using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class UserDetails
    {

        public UserDetails()
        {
            this.Reservation = new HashSet<Reservation>();
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
        public virtual ICollection<Reservation> Reservation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelPackage> CreatedPackages { get; set; }


    }
}