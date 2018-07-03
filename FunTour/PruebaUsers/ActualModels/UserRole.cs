using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PruebaUsers.Models;

namespace PruebaUsers.ActualModels
{
    public class UserRole
    {
        public int Id_UserRole { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }

        public string RoleDescription { get; set; }

        public UserRole()
        {
            this.UserPermissions = new HashSet<UserPermission>();
        }

        public Boolean IsSysAdmin()
        {

            try
            {
                string UserRole = this.Id_UserRole.ToString();

                Boolean IsSysAdmin = false;

                using (var _context = new ApplicationDbContext())
                {
                    var rol = _context.Roles.FirstOrDefault(p => p.Id == UserRole);
                    IsSysAdmin = _context.RoleDetails.FirstOrDefault(p => p.Id_Role.ToString() == rol.Id).IsSysAdmin;
                }
                return (IsSysAdmin);
            }
            catch (Exception)
            {
                return false;
            }

        }

        public void SetUserRolePermissions()
        {

            using( var _context = new ApplicationDbContext())
            {

                var UserRole = this.Id_UserRole;


                var rol = _context.Roles.FirstOrDefault(p => p.Id == UserRole.ToString());
                var Permissions = _context.RoleDetails.FirstOrDefault(p => p.Id_Role.ToString() == rol.Id).Permissions;

                foreach (var permission in Permissions)
                {
                    UserPermission TempPermission = new UserPermission
                    {
                        Id_UserPermission = permission.Id_Permission,
                        PermissionDescription = permission.PermissionDescription
                    };

                    this.UserPermissions.Add(TempPermission);
                }

            }
        }

        public Boolean HasPermissionTo(string permission)
        {
            UserPermissions = this.UserPermissions;

            foreach (var Permission in UserPermissions)
            {
                if (Permission.PermissionDescription.ToLower() == permission)
                {
                    return true;
                }
            }

            return false;

        }
    }
}