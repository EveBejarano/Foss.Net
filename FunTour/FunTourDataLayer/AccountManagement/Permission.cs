using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.AccountManagement
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
        [StringLength(100)]
        public string PermissionDescription { get; set; }

        public ICollection<RoleDetails> Roles { get; set; }
    }
}