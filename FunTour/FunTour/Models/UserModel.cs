using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FunTourBusinessLayer.UnitOfWorks;
using FunTourDataLayer;
using FunTourDataLayer.AccountManagement;

namespace FunTour.ActualModels
{
    public class UserModel
    {
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

        public static UserModel GetDataUserModel(string UserName, UnitOfWork unitOfWork)
        {
                
                try
                {
                    UserModel userModel = new UserModel();

                    var user = unitOfWork.UserRepository.GetUserByUserName(UserName);
                    userModel.Id_UserModel = user.Id;
                    userModel.Email = user.Email;
                    userModel.PhoneNumber = user.PhoneNumber;
                    userModel.UserModelName = user.UserName;
                    userModel.UserModelRoles = new List<UserRole>();

                    foreach (var item in user.Roles)
                    {
                        var auxdescriptionrole = unitOfWork.RolesRepository.GetRoleDetailsByID(item.RoleId);

                        var auxrole = new UserRole
                        {
                            Id_UserRole = auxdescriptionrole.Id_Role,
                            RoleDescription = auxdescriptionrole.RoleDescription,
                            RoleName = auxdescriptionrole.RoleName
                            
                        };

                        userModel.UserModelRoles.Add(auxrole);
                    }

                    var userDetails = unitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);
                    userModel.FirstName = userDetails.FirstName;
                    userModel.LastName = userDetails.LastName;
                    userModel.Inactive = false ;
                    userModel.IsSysAdmin = userDetails.isSysAdmin;

                    userModel.GetAvailablesRoles(unitOfWork);

                    

                    return userModel;

                }
                catch (Exception ex)
                {
                    //manejar error
                    throw;
                }

            }
        

        public static void SetDataUserModel(UserModel user, UnitOfWork unitOfWork)
        {
            //user.GetRolesToUserFromList();
            try
            {

                var userModel = unitOfWork.UserRepository.GetUserByID(user.Id_UserModel);
                userModel.Id = user.Id_UserModel;
                userModel.Email = user.Email;
                userModel.PhoneNumber = user.PhoneNumber;
                userModel.UserName = user.UserModelName;

                var userDetails = unitOfWork.UserRepository.GetUserDetailByUserName(user.UserModelName);
                userDetails.FirstName = user.FirstName;
                userDetails.LastName = user.LastName;
                userDetails.Inactive = user.Inactive;

                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new ApplicationUserManager(store);


                var userModel1 = unitOfWork.UserRepository.GetUserByID(user.Id_UserModel);
                unitOfWork.UserRepository.UpdateUser(userModel);
                unitOfWork.Save();


                var userDetails1 = unitOfWork.UserRepository.GetUserDetailByUserName(user.UserModelName);
                unitOfWork.UserRepository.UpdateUserDetails(userDetails);
                unitOfWork.Save();

                user.AddRolesToUser(unitOfWork);

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

        private void GetAvailablesRoles(UnitOfWork unitOfWork)
        {
                var auxRoles = unitOfWork.RolesRepository.GetRoleDetails(filter: p => p.IsDeleted == false).ToList();
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

        private void AddRolesToUser(UnitOfWork unitOfWork)
        {
            //this.GetRolesToUserFromList();

            foreach (var item in this.SelectedRoles)
            {
                if (item.isSelected == true)
                {
                    IdentityRole role = unitOfWork.RolesRepository.GetRoleByName(item.RoleDetails.RoleName);
                    IdentityUser User = unitOfWork.UserRepository.GetUserByID(this.Id_UserModel);

                    if (unitOfWork.UserRepository.AddRolesToUser(User, role))
                    {
                        unitOfWork.Save();
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