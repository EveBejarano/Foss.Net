using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class Permission
    {
        public Permission()
        {
            Roles = new HashSet<RoleDetails>();
        }

        [Key]
        public int Id_Permission { get; set; }

        [Required]
        [StringLength(30)]
        public string PermissionDescription { get; set; }

        public ICollection<RoleDetails> Roles { get; set; }
    }
}