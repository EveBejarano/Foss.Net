﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using FunTour.Models;
using FunTourBusinessLayer.Service;
using FunTourDataLayer;
using FunTourDataLayer.AccountManagement;

namespace FunTour.Controllers
{
    [UserAuthorization]
    public class AdminController : Controller
    { 
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private readonly DataService Service = new DataService();

        #region FunTour

        // Muestra en el index el listado de todos los usuarios activos
        // GET: Admin
        public ActionResult Index()
        {


            IEnumerable<UserDetails> UserList = Service.UnitOfWork.UserRepository.GetUserDetails();

            List < UserModel > UserModelList = new List<UserModel>();

            foreach (var item in UserList)
            {
                var auxUser = Service.UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
                    //_context.Users.FirstOrDefault(p => p.UserName == item.UserName);

                var UserModel = new UserModel
                {
                    Id_UserModel = item.Id_UserDetails.ToString(),
                    UserModelName = item.UserName,
                    LastName = item.LastName,
                    FirstName = item.FirstName,
                    IsSysAdmin = item.isSysAdmin,
                    Inactive = item.Inactive,
                    Email = auxUser.Email,
                    PhoneNumber = auxUser.PhoneNumber
                };

                UserModelList.Add(UserModel);
            }

            return View(UserModelList);
        }


        public ActionResult UserCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserCreate(UserModel NewUser)
        {
            // controla que el usuario no haya ingresado un nombre NO valido
            if (NewUser.UserModelName == "" || NewUser.UserModelName == null)
            {
                ModelState.AddModelError(string.Empty, "El nombre de usuario es obligatorio.");
            }

            if (ModelState.IsValid)
            {
                List<string> results = Service.UnitOfWork.UserRepository.GetStringUserNames(NewUser.UserModelName, NewUser.Email);

                bool _UserExistsInTable = (results.Count > 0);

                UserDetails _User = null;

                // si hay usuarios con el mismo nombre, busca el primero y si esta activo devuelve error
                // Si el usuario no esta activo, lo activa
                if (_UserExistsInTable)
                {

                    var Users = Service.UnitOfWork.UserRepository.GetUserDetailsByNameEmail(NewUser.UserModelName, NewUser.Email);


                    var band = false;
                    foreach (var item in Users)
                    {
                        if (item.Inactive == false)
                        {
                            ModelState.AddModelError(string.Empty, "El usuario ya existe!"); band = true;
                            break;
                        }
                    }
                    if (!band)
                    {
                        _User = Users.First(p => p.Inactive == true);
                        Service.UnitOfWork.UserRepository.ActivateUserDetails(_User);
                        Service.UnitOfWork.Save();
                        return RedirectToAction("Index");
                    }

                }


                // Si no hay usuarios con el mismo nombre, crea el usuario y lo almacena
                else
                {

                    var user = new ApplicationUser
                    {
                        UserName = NewUser.UserModelName,
                        Email = NewUser.Email,
                        PhoneNumber = NewUser.PhoneNumber,

                    };


                    var result = UserManager.CreateAsync(user, NewUser.Password).Result;
                    if (result.Succeeded)
                    {

                        var userDetails = new UserDetails
                        {
                            UserName = user.UserName,
                            FirstName = NewUser.FirstName,
                            LastName = NewUser.LastName,
                            isSysAdmin = NewUser.IsSysAdmin
                        };

                        Service.UnitOfWork.UserRepository.CreateUserDetails(userDetails);

                        Service.UnitOfWork.Save();
                        return RedirectToAction("Index");
                    }

                }

            }


            return View(NewUser);
        }

        public List<SelectListItem> List_boolNullYesNo()
        {
            var _retVal = new List<SelectListItem>();
            try
            {
                _retVal.Add(new SelectListItem { Text = "Not Set", Value = null });
                _retVal.Add(new SelectListItem { Text = "Yes", Value = bool.TrueString });
                _retVal.Add(new SelectListItem { Text = "No", Value = bool.FalseString });
            }
            catch { }
            return _retVal;
        }

        private void SetViewBagData(string _UserId)
        {
            ViewBag.UserId = _UserId;
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            ViewBag.RoleId = new SelectList(Service.UnitOfWork.RolesRepository.GetRoles(orderBy: q => q.OrderBy(p => p.Name)), "Id_Role", "RoleName");
        }


        //Busca un usuario y muestra los datos
        public ViewResult UserDetails(string  IdUser)
        {
            string user;
            string _idUser;
            try
            {
                var aux = Service.UnitOfWork.UserRepository.GetUserByID(IdUser);
                user = aux.UserName;
                _idUser = aux.Id;
            }
            catch (Exception e)
            {
                user = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Id_UserDetails.ToString() == IdUser).FirstOrDefault().UserName;
                var aux = Service.UnitOfWork.UserRepository.GetUserByUserName(user);
                _idUser = aux.Id;
            }

            var User = UserModel.GetDataUserModel(user); //Ver
            SetViewBagData(_idUser);
            return View(User);
        }


        // Toma un Model de Usuario
        [HttpPost]
        public ActionResult UserDetails(UserModel User)
        {
            if (User.UserModelName == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid User Name");
            }

            if (ModelState.IsValid)
            {
                UserModel.SetDataUserModel(User);
            }
            return View(User);
        }

        [HttpGet]
        public ActionResult UserEdit(string IdUser)
        {
            string user;
            try
            {
               user = Service.UnitOfWork.UserRepository.GetUserByID(IdUser).UserName;
            }
            catch (Exception e)
            {
                user = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Id_UserDetails.ToString() == IdUser).FirstOrDefault().UserName;
            }
            


            var User = UserModel.GetDataUserModel(user);
            SetViewBagData(IdUser);

            return View(User);
        }

        [HttpPost]
        public ActionResult UserEdit(UserModel User)
        {
            var _User = Service.UnitOfWork.UserRepository.GetUserByID(User.Id_UserModel);
                /*_context.Users.Where(p => p.Id == User.Id_UserModel).FirstOrDefault()*/;
            if (_User != null)
            {
                try
                {
                    UserModel.SetDataUserModel( User);
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("UserDetails", new RouteValueDictionary(new { IdUser = User.Id_UserModel}));
        }

        [HttpGet]
        public ActionResult DeleteUserRole(string id, string UserId)
        {
            //var role = _context.Roles.Find(id);
            //var user = _context.Users.Find(UserId);

            var role = Service.UnitOfWork.RolesRepository.GetRoleByID(id);
            var user = Service.UnitOfWork.UserRepository.GetUserByID(UserId);

            if (Service.UnitOfWork.UserRepository.DeleteRoleFromUser(user, role))
            {
                Service.UnitOfWork.Save();
            };
               
            
            return RedirectToAction("UserDetails", new { id = UserId });
        }

        [HttpGet]
        public PartialViewResult filter4FunTour(string _surname)
        {
            return PartialView("_ListUserTable", GetFilteredUserList(_surname));
        }

        [HttpGet]
        public PartialViewResult filterReset()
        {
            IEnumerable<UserDetails> usersdetails = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<UserModel> users = new List<UserModel>();
            foreach (var user in usersdetails)
            {
                var newuser = new UserModel
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Id_UserModel = Service.UnitOfWork.UserRepository.GetUserByUserName(user.UserName).Id,
                    UserModelName = user.UserName

                };

                users.Add(newuser);
            }

            return PartialView("_ListUserTable", users);
        }

        [HttpGet]
        public PartialViewResult DeleteUserReturnPartialView(string UserId)
        {
            var aux = Service.UnitOfWork.UserRepository.GetUserByID(UserId);
            var user = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.UserName == aux.UserName).FirstOrDefault().Id_UserDetails;

            if (Service.UnitOfWork.UserRepository.DeleteUserDetail(user))
                {
                    Service.UnitOfWork.Save();
                };


            return this.filterReset();
        }

        private ICollection<UserModel> GetFilteredUserList(string _surname)
        {
            ICollection<UserModel> _ret = null;
            try
            {
                if (string.IsNullOrEmpty(_surname))
                {
                    var userDetails = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

                    foreach (var user in userDetails)
                    {
                        var userModel = new UserModel
                        {

                            Id_UserModel = Service.UnitOfWork.UserRepository.GetUserByUserName(user.UserName).Id,
                            UserModelName = user.UserName,
                            Inactive = user.Inactive,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            IsSysAdmin = user.isSysAdmin

                        };

                        var users = Service.UnitOfWork.UserRepository.GetUserByUserName(_surname);

                        userModel.UserModelName = users.UserName;
                        userModel.Email = users.Email;
                        userModel.PhoneNumber = users.PhoneNumber;

                        _ret.Add(userModel);

                    }
                }
                else
                {
                    IEnumerable<IdentityUser> users = Service.UnitOfWork.UserRepository.GetUsers(filter: p => p.UserName == _surname);

                    foreach (var user in users)
                    {
                        var userModel = new UserModel
                        {
                            Id_UserModel = user.Id,
                            UserModelName = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber
                        };

                        var userDetail = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);

                        userModel.Inactive = userDetail.Inactive;
                        userModel.FirstName = userDetail.FirstName;
                        userModel.LastName = userDetail.LastName;
                        userModel.IsSysAdmin = userDetail.isSysAdmin;

                    _ret.Add(userModel);

                }
            }
            }
            catch
            {
            }
            return _ret;
        }
        

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserRoleReturnPartialView(int id, string UserId)
        {
            var role = Service.UnitOfWork.RolesRepository.GetRoleByID(id);
            var user = Service.UnitOfWork.UserRepository.GetUserByID(UserId);

            if (Service.UnitOfWork.UserRepository.DeleteRoleFromUser(user, role))
            {
                Service.UnitOfWork.Save();
            };
            //IdentityRole role = _context.Roles.Find(id);
            //IdentityUser User = _context.Users.Find(UserId);
            //IdentityUserRole user_role = new IdentityUserRole
            //{
            //    RoleId = role.Id,
            //    UserId = User.Id
            //};

            //if (role.Users.Contains(user_role))
            //{
            //    role.Users.Remove(user_role);
            //    _context.SaveChanges();
            //}
            SetViewBagData(UserId.ToString());

            return PartialView("_ListUserRoleTable", user.UserName);
        }

        //[HttpGet]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public PartialViewResult AddUserRoleReturnPartialView(int id, int UserId)
        //{

        //    IdentityRole role = _context.Roles.Find(id);
        //    IdentityUser User = _context.Users.Find(UserId);
        //    IdentityUserRole user_role = new IdentityUserRole
        //    {
        //        RoleId = role.Id,
        //        UserId = User.Id
        //    };

        //    if (!role.Users.Contains(user_role))
        //    {
        //        role.Users.Add(user_role);
        //        _context.SaveChanges();
        //    }
        //    SetViewBagData(UserId.ToString());

        //    return PartialView("_ListUserRoleTable", _context.Users.Find(UserId));
        //}

        #endregion

        #region Roles
        public ActionResult RoleIndex()
        {
            //Get rolesrepository 
            return View(Service.UnitOfWork.RolesRepository.GetRoleDetails(orderBy: q=> q.OrderBy(r => r.RoleDescription)));
        }

        public ViewResult RoleDetails(int id)
        {

            IdentityUser User = Service.UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
            var roleDetails = Service.UnitOfWork.RolesRepository.GetRoleDetails(filter:r => r.Id_Role == id, includeProperties : "Permissions")
                   .FirstOrDefault();

            var role = Service.UnitOfWork.RolesRepository.GetRoles(filter: p => p.Id == roleDetails.Id_Role.ToString(), includeProperties: "Users").FirstOrDefault();
            if (role.Users != null)
            {
                foreach (var item in role.Users)
                {
                    var user = Service.UnitOfWork.UserRepository.GetUserByID(item.UserId);
                    var auxuser = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);

                    if (user != null && (auxuser.Inactive == false || auxuser.Inactive == null))
                    {
                        roleDetails.Users.Add(user);
                    }
                }
            }



            var auxuserlist = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = Service.UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
                UserList.Add(auxuser);
            }

            // FunTour combo
            ViewBag.UserId = new SelectList(UserList, "Id_User", "UserName");


            // Rights combo
            ViewBag.PermissionId = new SelectList(Service.UnitOfWork.PermissionRepository.Get(orderBy: p=> p.OrderBy(a => a.PermissionDescription)), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(roleDetails);
        }

        public ActionResult RoleCreate()
        {
           IdentityUser User = Service.UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View();
        }

        [HttpPost]
        public ActionResult RoleCreate(RoleDetails _role)
        {
            if (_role.RoleDescription == null)
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
            }

            IdentityUser User = Service.UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                if (Service.UnitOfWork.RolesRepository.CreateRol(_role))
                {
                    Service.UnitOfWork.Save();
                };
                return RedirectToAction("RoleIndex");
            }
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult RoleEdit(int id)
        {
        //    IdentityUser User = _context.Users.Where(r => r.UserName == this.User.Identity.Name).FirstOrDefault();
            var roleDetails = Service.UnitOfWork.RolesRepository.GetRoleDetails(filter: r => r.Id_Role == id, includeProperties : "Permissions").FirstOrDefault();

            var role = Service.UnitOfWork.RolesRepository.GetRoles(filter: p => p.Name == roleDetails.RoleName, includeProperties: "Users").FirstOrDefault();
            foreach (var item in role.Users)
            {
                var user = Service.UnitOfWork.UserRepository.GetUserByID(item.UserId);
                var auxuser = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);

                if (user != null && (auxuser.Inactive == false || auxuser.Inactive == null))
                {
                    roleDetails.Users.Add(user);
                }
            }

            var auxuserlist = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = Service.UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
                UserList.Add(auxuser);
            }

            // FunTour combo
            ViewBag.UserList = new SelectList(UserList, "Id", "UserName");


            // Rights combo
            ViewBag.PermissionId = new SelectList(Service.UnitOfWork.PermissionRepository.Get(orderBy: p=> p.OrderBy(a => a.Id_Permission)), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(roleDetails);
        }

        [HttpPost]
        public ActionResult RoleEdit(RoleDetails _role)
        {
            if (string.IsNullOrEmpty(_role.RoleDescription))
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
            }
            
            IdentityUser User = Service.UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                
                if (Service.UnitOfWork.RolesRepository.Update(_role))
                {
                    Service.UnitOfWork.Save();
                }
                return RedirectToAction("RoleDetails", new RouteValueDictionary(new { id = _role.Id_Role }));
            }

            var auxuserlist = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = Service.UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
                UserList.Add(auxuser);
            }

            // Users combo-
            ViewBag.UserList = new SelectList( UserList, "Id", "UserName");

            // Rights combo
            ViewBag.PermissionId = new SelectList(Service.UnitOfWork.PermissionRepository.Get(orderBy: p=>p.OrderBy(a => a.Id_Permission)), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult RoleDelete(int id)
        {
            RoleDetails roleDetails = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);

            IdentityRole _role = Service.UnitOfWork.RolesRepository.GetRoles(filter: p => p.Name == roleDetails.RoleName).FirstOrDefault();


            if (Service.UnitOfWork.RolesRepository.DeleteRole(_role))
            {
                Service.UnitOfWork.Save();
            }
            return RedirectToAction("RoleIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserFromRoleReturnPartialView(int id, int UserId)
        {
            IdentityRole role = Service.UnitOfWork.RolesRepository.GetRoleByID(id);
            IdentityUser user = Service.UnitOfWork.UserRepository.GetUserByID(UserId);
            

            if (Service.UnitOfWork.RolesRepository.DeleteUserFromRole(user,role))
            {
                Service.UnitOfWork.Save();
            }
            return PartialView("_ListUsersTable4Role", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUser2RoleReturnPartialView(int id, string UserId)
        {
            IdentityRole role = Service.UnitOfWork.RolesRepository.GetRoleByID(id);
            IdentityUser User = Service.UnitOfWork.UserRepository.GetUserByID(UserId);

            if (Service.UnitOfWork.UserRepository.AddRolesToUser(User, role))
            {
                Service.UnitOfWork.Save();
            }
            return PartialView("_ListUsersTable4Role", role);
        }

        #endregion

        #region Permissions

        public ViewResult PermissionIndex()
        {
            IEnumerable<Permission> _permissions = Service.UnitOfWork.PermissionRepository.Get(orderBy : p=>p.OrderBy(wn => wn.PermissionDescription), includeProperties :"Roles");
            return View(_permissions);
        }

        public ViewResult PermissionDetails(int id)
        {
            Permission _permission = Service.UnitOfWork.PermissionRepository.GetByID(id);
            return View(_permission);
        }

        public ActionResult PermissionCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PermissionCreate(Permission _permission)
        {
            if (_permission.PermissionDescription == null)
            {
                ModelState.AddModelError("Permission Description", "Permission Description must be entered");
            }

            if (ModelState.IsValid)
            {
                Service.UnitOfWork.PermissionRepository.Insert(_permission);
                Service.UnitOfWork.Save();
                return RedirectToAction("PermissionIndex");
            }
            return View(_permission);
        }

        public ActionResult PermissionEdit(int id)
        {
            Permission _permission = Service.UnitOfWork.PermissionRepository.GetByID(id);
            ViewBag.RoleId = new SelectList(Service.UnitOfWork.RolesRepository.GetRoleDetails(orderBy: q=> q.OrderBy(p => p.RoleDescription)), "Id_Role", "RoleDescription");
            return View(_permission);
        }

        [HttpPost]
        public ActionResult PermissionEdit(Permission _permission)
        {
            if (ModelState.IsValid)
            {
                Service.UnitOfWork.PermissionRepository.Update(_permission);
                Service.UnitOfWork.Save();
                return RedirectToAction("PermissionDetails", new RouteValueDictionary(new { id = _permission.Id_Permission }));
            }
            return View(_permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult PermissionDelete(int id)
        {
            Permission permission = Service.UnitOfWork.PermissionRepository.GetByID(id);
            if (permission.Roles.Count > 0)
                permission.Roles.Clear();

            Service.UnitOfWork.PermissionRepository.Delete(permission);
            Service.UnitOfWork.Save();
            return RedirectToAction("PermissionIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddPermission2RoleReturnPartialView(int id, int permissionId)
        {
            if (Service.UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
            {
                Service.UnitOfWork.Save();
            }
            RoleDetails role = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView("_ListPermissions", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddAllPermissions2RoleReturnPartialView(string id)
        {

            if (Service.UnitOfWork.RolesRepository.AddAllPermissions2Role(id))
            {
                Service.UnitOfWork.Save();
            }

            RoleDetails _role = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView("_ListPermissions", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeletePermissionFromRoleReturnPartialView(int id, int permissionId)
        {
            RoleDetails _role = Service.UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            if (Service.UnitOfWork.RolesRepository.DeletePermissionFromRole(id, permissionId))
            {
                Service.UnitOfWork.Save();
            }
            return PartialView("_ListPermissions", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteRoleFromPermissionReturnPartialView(int id, int permissionId)
        {
            Permission permission= Service.UnitOfWork.PermissionRepository.GetByID(permissionId);
            if (Service.UnitOfWork.RolesRepository.DeleteRoleFromPermission(id, permissionId))
            {
                Service.UnitOfWork.Save();
            }
            return PartialView("_ListRolesTable4Permission", permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddRole2PermissionReturnPartialView(int permissionId, int roleId)
        {
            Permission _permission = Service.UnitOfWork.PermissionRepository.GetByID(permissionId);

            if (Service.UnitOfWork.RolesRepository.AddRole2Permission(permissionId, roleId))
            {
                Service.UnitOfWork.Save();
            }
            return PartialView("_ListRolesTable4Permission", _permission);
        }

        public ActionResult PermissionsImport()
        {
            var _controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t != null
                    && t.IsPublic
                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    && !t.IsAbstract
                    && typeof(IController).IsAssignableFrom(t));

            var _controllerMethods = _controllerTypes.ToDictionary(controllerType => controllerType,
                    controllerType => controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType)));

            foreach (var _controller in _controllerMethods)
            {
                string _controllerName = _controller.Key.Name;
                foreach (var _controllerAction in _controller.Value)
                {
                    string _controllerActionName = _controllerAction.Name;
                    if (_controllerName.EndsWith("Controller"))
                    {
                        _controllerName = _controllerName.Substring(0, _controllerName.LastIndexOf("Controller"));
                    }

                    string _permissionDescription = string.Format("{0}-{1}", _controllerName, _controllerActionName);
                    Permission _permission = Service.UnitOfWork.PermissionRepository.Get(filter : p => p.PermissionDescription == _permissionDescription).FirstOrDefault();
                    if (_permission == null)
                    {
                        if (ModelState.IsValid)
                        {
                            Permission _perm = new Permission();
                            _perm.PermissionDescription = _permissionDescription;

                            Service.UnitOfWork.PermissionRepository.Insert(_perm);
                            Service.UnitOfWork.Save();
                        }
                    }
                }
            }
            return RedirectToAction("PermissionIndex");
        }
        #endregion

        //public static SelectList RoleList(int UserId)
        //{

        //        List<SelectListItem> RolesList = new List<SelectListItem>();
        //    using (var _context = new ApplicationDbContext())
        //    {
        //        var auxRoleList = 

        //        items.Add(new SelectListItem { Text = "Action", Value = "0" });


        //    };
        //        ViewBag.Roles = RolesList;


        //    }

        [HttpGet]
        public ActionResult UserDelete(string IdUser)
        {
            var user = Service.UnitOfWork.UserRepository.GetUserDetails(filter: p => p.Id_UserDetails.ToString() == IdUser).FirstOrDefault();
            //_context.Users.FirstOrDefault(p => p.Id == IdUser);

            var User = UserModel.GetDataUserModel(user.UserName); //Ver
            SetViewBagData(IdUser);
            return View(User);
        }

        
        [HttpGet]
        public ActionResult UserDeleteConfirmed(string UserId)
        {
            
            var aux = Service.UnitOfWork.UserRepository.GetUserByID(UserId);
            var user = Service.UnitOfWork.UserRepository.GetUserDetails(filter: r => r.UserName == aux.UserName).FirstOrDefault().Id_UserDetails;
            
            if (Service.UnitOfWork.UserRepository.DeleteUserDetail(user))
            {
                Service.UnitOfWork.Save();
            };
            return RedirectToAction("Index");
        }
    }
}