using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FunTour.Models;
using FunTourBusinessLayer.Service;
using FunTourBusinessLayer.UnitOfWorks;

namespace FunTour.Models
{
    public class UserRole
    {
        public int Id_UserRole { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
        private readonly DataService Service = new DataService();
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

                var rol = Service.UnitOfWork.RolesRepository.GetRoleByID(UserRole);
                IsSysAdmin = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(rol.Id).IsSysAdmin;

                return (IsSysAdmin);
            }

            catch (Exception)
            {
                return false;
            }

        }

        public void SetUserRolePermissions()
        {
            var UserRole = this.Id_UserRole;


            var rol = Service.UnitOfWork.RolesRepository.GetRoleByID(UserRole.ToString());
            var Permissions = Service.UnitOfWork.RolesRepository.GetRoleDetails(filter: p => p.Id_Role.ToString() == rol.Id, includeProperties: "Permissions").FirstOrDefault().Permissions;

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
