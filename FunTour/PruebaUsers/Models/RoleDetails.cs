using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruebaUsers.Models
{
    public partial class RoleDetails
    {
        public RoleDetails()
        {
            this.Users = new HashSet<IdentityUser>();
            this.Permissions = new HashSet<Permission>();
        }

        [Key]
        public int Id_Role { get; set; }

        [Required]
        [StringLength(30)]
        public string RoleName { get; set; }

        [StringLength(50)]
        public string RoleDescription { get; set; }

        [Required]
        public bool IsSysAdmin { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Permission> Permissions {get;set;}

        public virtual ICollection<IdentityUser> Users { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}