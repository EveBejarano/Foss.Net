using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FunTourBusinessLayer.Service;
using FunTourBusinessLayer.UnitOfWorks;
using FunTourDataLayer;
using FunTourDataLayer.AccountManagement;

namespace FunTour.Models
{
    public class UserModel
    {
        private static readonly DataService Service = new DataService();

        public string Id_UserModel { get; set; }

        public bool? Inactive { get; set; }
        public ICollection<UserRole> UserModelRoles { get; set; }


        [Display(Name = "Número de Teléfono")]
        public string PhoneNumber { get; set; }



        [Required]
        [Display(Name = "¿Es Administrador?")]
        public Boolean IsSysAdmin { get; set; }


        [Required]
        [Display(Name = "Nombre de Usuario")]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        public string UserModelName { get; set; }

        [Required]
        [Display(Name = "Nombres")]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }


        public IList<UserModelRolElement> SelectedRoles { get; set; }


        public UserModel()
        {
            SelectedRoles = new List<UserModelRolElement>();
        }

        public static UserModel GetDataUserModel(string UserName)
        {
                
                try
                {
                    UserModel userModel = new UserModel();

                    var user = Service.UnitOfWork.UserRepository.GetUserByUserName(UserName);
                    userModel.Id_UserModel = user.Id;
                    userModel.Email = user.Email;
                    userModel.PhoneNumber = user.PhoneNumber;
                    userModel.UserModelName = user.UserName;
                    userModel.UserModelRoles = new List<UserRole>();

                    foreach (var item in user.Roles)
                    {
                        var auxdescriptionrole = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(item.RoleId);

                        var auxrole = new UserRole
                        {
                            Id_UserRole = auxdescriptionrole.Id_Role,
                            RoleDescription = auxdescriptionrole.RoleDescription,
                            RoleName = auxdescriptionrole.RoleName
                            
                        };

                        userModel.UserModelRoles.Add(auxrole);
                    }

                    var userDetails = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);
                    userModel.FirstName = userDetails.FirstName;
                    userModel.LastName = userDetails.LastName;
                    userModel.Inactive = false ;
                    userModel.IsSysAdmin = userDetails.isSysAdmin;

                    userModel.GetAvailablesRoles();

                    

                    return userModel;

                }
                catch (Exception ex)
                {
                    //manejar error
                    throw;
                }

            }
        

        public static void SetDataUserModel(UserModel user)
        {
            //user.GetRolesToUserFromList();
            try
            {

                var userModel = Service.UnitOfWork.UserRepository.GetUserByID(user.Id_UserModel);
                userModel.Id = user.Id_UserModel;
                userModel.Email = user.Email;
                userModel.PhoneNumber = user.PhoneNumber;
                userModel.UserName = user.UserModelName;

                var userDetails = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserModelName);
                userDetails.FirstName = user.FirstName;
                userDetails.LastName = user.LastName;
                userDetails.Inactive = user.Inactive;

                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new ApplicationUserManager(store);


                var userModel1 = Service.UnitOfWork.UserRepository.GetUserByID(user.Id_UserModel);
                Service.UnitOfWork.UserRepository.UpdateUser(userModel);
                Service.UnitOfWork.Save();


                var userDetails1 = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserModelName);
                Service.UnitOfWork.UserRepository.UpdateUserDetails(userDetails);
                Service.UnitOfWork.Save();

                user.AddRolesToUser();

            }
            catch (Exception ex)
            {
                //manejar error
                throw;
            }
            //}


        }
        
        //public void GetRolesToUserFromList()
        //{
        //    using (var _context = new ApplicationDbContext())
        //    {
        //        foreach (var item in this.SelectedRoles)
        //        {
        //            var auxRol = _context.RoleDetails.FirstOrDefault(p => p.RoleName == item);
        //            this.Roles.Add(auxRol);
        //        }
        //    }

        //}

        private void GetAvailablesRoles()
        {
                var auxRoles = Service.UnitOfWork.RolesRepository.GetRoleDetails(filter: p => p.IsDeleted == false).ToList();
                foreach (var item in auxRoles)
                {
                    var newRole = new UserModelRolElement
                    {
                        isSelected = false,
                        RoleDetails = item
                    };

                    this.SelectedRoles.Add(newRole);
                }
    
                //List<SelectListItem> selectedRolesList = new List<SelectListItem>();

                //        foreach (var item in auxRoles)
                //        {
                //            this.AvailableRoles.Add(new SelectListItem { Text = item.RoleDescription, Value = item.RoleName });
                //        }


        }

        private void AddRolesToUser()
        {
            //this.GetRolesToUserFromList();

            foreach (var item in this.SelectedRoles)
            {
                if (item.isSelected == true)
                {
                    IdentityRole role = Service.UnitOfWork.RolesRepository.GetRoleByName(item.RoleDetails.RoleName);
                    IdentityUser User = Service.UnitOfWork.UserRepository.GetUserByID(this.Id_UserModel);

                    if (Service.UnitOfWork.UserRepository.AddRolesToUser(User, role))
                    {
                        Service.UnitOfWork.Save();
                    }

                }
            }

        }
    }

    public class UserModelRolElement
    {

        public Boolean isSelected { get; set; }
        public RoleDetails RoleDetails { get; set; }

    }
}