using System;
using System.Collections.Generic;
using System.Linq;
using FunTourBusinessLayer.UnitOfWorks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FunTour.ActualModels
{
    public class ActualUser
    {
        public string Id_ActualUser { get; set; }
        public string ActualUserName { get; set; }
        public Boolean IsSysAdmin { get; set; }
        public ICollection<UserRole> ActualUserRoles { get; set; }


        public ActualUser(string UserName, UnitOfWork unitOfWork)
        {
                try
                {
                    this.Id_ActualUser = unitOfWork.UserRepository.GetUserByUserName(UserName).Id;

                    this.ActualUserName = UserName;
                    this.IsSysAdmin = unitOfWork.UserRepository.GetUserDetailByUserName(this.ActualUserName).isSysAdmin;
                    this.SetUserRoles(unitOfWork);
            }
                catch (Exception ex)
                {
                
                }


        }


        // 1. Extrae de la base de datos los Roles de un usuario
        // 2. verifica si es Sysadmin o no
        // 3. Almacena en cada rol los permisos que este posee
        public void SetUserRoles(UnitOfWork unitOfWork)
        {
                
                List<IdentityUserRole> Roles = unitOfWork.UserRepository.GetUsers(filter: p => p.UserName == this.ActualUserName, includeProperties: "Roles").FirstOrDefault().Roles.ToList();
                foreach (var rol in Roles)
                {
                    UserRole userRole = new UserRole
                    {
                        
                        RoleName = unitOfWork.RolesRepository.GetRoleByID(rol.RoleId).Name
                    };

                    int a;
                    int.TryParse(rol.RoleId, out a);

                    userRole.Id_UserRole = a;

                    userRole.SetUserRolePermissions(unitOfWork);

                    if (IsSysAdmin == false)
                    {
                        this.IsSysAdmin = userRole.IsSysAdmin(unitOfWork);
                    }

                    ActualUserRoles.Add(userRole);
                }
            }
        


        // Verifica que un Usuario tenga un rol especifico
        public Boolean HasRole(string Rol)
        {
            if (this.ActualUserRoles != null)
            {
                foreach (var rol in this.ActualUserRoles)
                {
                    if (rol.RoleName.ToLower() == Rol.ToLower())
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        // Verifica que el usuario tiene alguno de los Roles dados.
        public bool HasRoles(string Roles)
        {
            bool bFound = false;
            string[] _Roles = Roles.ToLower().Split(';');

            if (this.ActualUserRoles != null)
            {
                foreach (UserRole Role in this.ActualUserRoles)
                {
                    try
                    {
                        bFound = _Roles.Contains(Role.RoleName.ToLower());
                        if (bFound)
                            return bFound;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return bFound;
        }

        // Verifica si un Usuario tiene un permiso especifico

        public Boolean HasPermission(string Permission)
        {
            if (this.ActualUserRoles != null)
            {
                foreach (var rol in this.ActualUserRoles)
                {
                    foreach (var RolPermission in rol.UserPermissions)
                    {
                        if (Permission.ToLower() == RolPermission.PermissionDescription.ToLower())
                        {
                            return true;
                        }
                    }


                }
            }

            return false;
        }

    }
}