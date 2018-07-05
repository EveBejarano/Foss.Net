using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using FunTour.ActualModels;
using FunTourBusinessLayer.UnitOfWorks;
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

        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        #region FunTour

        // Muestra en el index el listado de todos los usuarios activos
        // GET: Admin
        public ActionResult Index()
        {
            // lista los usuarios activos
            // verifica si el usuario esta activo o no. los ordena primero por apellido y despues por nombre
            // List<UserDetails> UserList = _context.Database.SqlQuery<UserDetails>(string.Format("SELECT * FROM IdentityUsers p, UserDetails d WHERE p.UserName = d.UserName And(d.Inactive != 'true')", new UserDetails().LastName)).ToList();


            //List<UserDetails> UserList = (from d in _context.UserDetails
            //                              join u in _context.Users
            //                              on d.UserName equals u.UserName
            //                              where d.Inactive != true
            //                              select d).ToList();

            IEnumerable<UserDetails> UserList = UnitOfWork.UserRepository.GetUserDetails(filter : p=> p.Inactive == false);

            List < UserModel > UserModelList = new List<UserModel>();

            foreach (var item in UserList)
            {
                var auxUser = UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
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


        //Busca un usuario y muestra los datos
        public ViewResult UserDetails(string  IdUser)
        {
            var user = UnitOfWork.UserRepository.GetUserByID(IdUser);
                //_context.Users.FirstOrDefault(p => p.Id == IdUser);

            var User = UserModel.GetDataUserModel(user.UserName, UnitOfWork); //Ver
            SetViewBagData(IdUser);
            return View(User);
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
            ViewBag.RoleId = new SelectList(UnitOfWork.RolesRepository.GetRoles(orderBy: q => q.OrderBy(p => p.Name) ), "Id_Role", "RoleName");
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
                UserModel.SetDataUserModel(User, UnitOfWork);
            }
            return View(User);
        }

        public ActionResult UserCreate()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> UserCreate(UserModel NewUser)
        {
            // controla que el usuario no haya ingresado un nombre NO valido
            if (NewUser.UserModelName == "" || NewUser.UserModelName == null)
            {
                ModelState.AddModelError(string.Empty, "El nombre de usuario es obligatorio.");
            }

            try
            {
                if (ModelState.IsValid)
                {

                    // busca una lista de nombres de usuario que coincidan con el ingresado
                    //List<string> results = (from d in _context.UserDetails
                    //                        join u in _context.Users
                    //                        on d.UserName equals u.UserName
                    //                        where d.UserName == NewUser.UserModelName
                    //                        || (d.UserName == u.UserName && u.Email == NewUser.Email)
                    //                        select d.UserName).ToList();

                    List<string> results = UnitOfWork.UserRepository.GetStringUserNames(NewUser.UserModelName, NewUser.Email);

                    bool _UserExistsInTable = (results.Count > 0);

                    UserDetails _User = null;

                    // si hay usuarios con el mismo nombre, busca el primero y si esta activo devuelve error
                    // Si el usuario no esta activo, lo activa
                    if (_UserExistsInTable)
                    {
                        
                        var Users = UnitOfWork.UserRepository.GetUserDetailsByNameEmail(NewUser.UserModelName, NewUser.Email);


                            var band = false;
                            foreach (var item in Users)
                            {
                                if (item.Inactive == false)
                                {
                                    ModelState.AddModelError(string.Empty, "El usuario ya existe!");

                                    band = true;
                                    break;
                                }
                            }
                            if (!band)
                            {
                                _User = Users.First(p => p.Inactive == true);
                                UnitOfWork.UserRepository.ActivateUserDetails(_User);
                                UnitOfWork.Save();
                                return RedirectToAction("Index");
                            }

                    }


                    // Si no hay usuarios con el mismo nombre, crea el usuario y lo almacena
                    else
                    {

                        var user = new ApplicationUser
                        {
                            UserName = NewUser.UserModelName,
                            Email = NewUser.Email
                        };


                        var result = await UserManager.CreateAsync(user, NewUser.Password);
                        if (result.Succeeded)
                        {

                            var userDetails = new UserDetails
                            {
                                FirstName = NewUser.FirstName,
                                LastName = NewUser.LastName,
                            };

                            UnitOfWork.UserRepository.CreateUserDetails(userDetails);
                            try
                            {

                                UnitOfWork.Save();
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }

                        
                        //{
                        //    using (var _context = new ApplicationDbContext())
                        //    {

                        //        var userDetails = new UserDetails
                        //        {
                        //            UserName = _context.Users.FirstOrDefault(p => p.UserName == NewUser.UserModelName).UserName,

                        //            FirstName = NewUser.FirstName,
                        //            LastName = NewUser.LastName,
                        //            LastModified = System.DateTime.Now,
                        //            Inactive = false,
                        //            isSysAdmin = true
                        //        };

                        //        try
                        //        {

                        //            _context.UserDetails.Add(userDetails);
                        //            _context.SaveChanges();
                        //        }
                        //        catch (Exception ex)
                        //        {

                        //            throw;
                        //        }

                        //    }

                        return RedirectToAction("Index");
                        }
                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //return base.ShowError(ex);
            }

            return View(NewUser);
        }

        [HttpGet]
        public ActionResult UserEdit(string UserName)
        {
            
            var User = UserModel.GetDataUserModel(UserName, UnitOfWork);
            SetViewBagData(UserName);

            return View(User);
        }

        [HttpPost]
        public ActionResult UserEdit(UserModel User)
        {
            var _User = UnitOfWork.UserRepository.GetUserByID(User.Id_UserModel);
                /*_context.Users.Where(p => p.Id == User.Id_UserModel).FirstOrDefault()*/;
            if (_User != null)
            {
                try
                {
                    UserModel.SetDataUserModel( User, UnitOfWork);
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("UserDetails", new RouteValueDictionary(new { IdUser = _User.Id }));
        }

        [HttpGet]
        public ActionResult DeleteUserRole(string id, string UserId)
        {
            //var role = _context.Roles.Find(id);
            //var user = _context.Users.Find(UserId);

            var role = UnitOfWork.RolesRepository.GetRoleByID(id);
            var user = UnitOfWork.UserRepository.GetUserByID(UserId);

            if (UnitOfWork.UserRepository.DeleteRoleFromUser(user, role))
            {
                UnitOfWork.Save();
            };
               
            
            return RedirectToAction("Details", "User", new { id = UserId });
        }

        [HttpGet]
        public PartialViewResult filter4FunTour(string _surname)
        {
            return PartialView("_ListUserTable", GetFilteredUserList(_surname));
        }

        [HttpGet]
        public PartialViewResult filterReset()
        {
            IEnumerable<UserDetails> usersdetails = UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<UserModel> users = new List<UserModel>();
            foreach (var user in usersdetails)
            {
                var newuser = new UserModel
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Id_UserModel = UnitOfWork.UserRepository.GetUserByUserName(user.UserName).Id,
                    UserModelName = user.UserName

                };

                users.Add(newuser);
            }

            return PartialView("_ListUserTable", users);
        }

        [HttpGet]
        public PartialViewResult DeleteUserReturnPartialView(string UserId)
        {
            try
            {
                if (UnitOfWork.UserRepository.DeleteUserDetail(UserId))
                {
                    UnitOfWork.Save();
                };
                //var User = _context.UserDetails.Find(UserId);
                //if (User != null)
                //{
                //    User.Inactive = true;
                //    User.LastModified = System.DateTime.Now;

                //    var User1 = _context.UserDetails.Find(UserId);
                //    _context.Entry(User1).CurrentValues.SetValues(User);
                //    _context.SaveChanges();
                    
                //}
            }
            catch
            {
            }
            return this.filterReset();
        }

        private ICollection<UserModel> GetFilteredUserList(string _surname)
        {
            ICollection<UserModel> _ret = null;
            try
            {
                if (string.IsNullOrEmpty(_surname))
                {
                    var userDetails = UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

                    foreach (var user in userDetails)
                    {
                        var userModel = new UserModel
                        {

                            Id_UserModel = UnitOfWork.UserRepository.GetUserByUserName(user.UserName).Id,
                            UserModelName = user.UserName,
                            Inactive = user.Inactive,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            IsSysAdmin = user.isSysAdmin

                        };

                        var users = UnitOfWork.UserRepository.GetUserByUserName(_surname);

                        userModel.UserModelName = users.UserName;
                        userModel.Email = users.Email;
                        userModel.PhoneNumber = users.PhoneNumber;

                        _ret.Add(userModel);

                    }
                }
                else
                {
                    IEnumerable<IdentityUser> users = UnitOfWork.UserRepository.GetUsers(filter: p => p.UserName == _surname);

                    foreach (var user in users)
                    {
                        var userModel = new UserModel
                        {
                            Id_UserModel = user.Id,
                            UserModelName = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber
                        };

                        var userDetail = UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);

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
            var role = UnitOfWork.RolesRepository.GetRoleByID(id);
            var user = UnitOfWork.UserRepository.GetUserByID(UserId);

            if (UnitOfWork.UserRepository.DeleteRoleFromUser(user, role))
            {
                UnitOfWork.Save();
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
            return View(UnitOfWork.RolesRepository.GetRoleDetails(orderBy: q=> q.OrderBy(r => r.RoleDescription)));
        }

        public ViewResult RoleDetails(int id)
        {

            IdentityUser User = UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
            var roleDetails = UnitOfWork.RolesRepository.GetRoleDetails(filter:r => r.Id_Role == id, includeProperties : "Permissions")
                   .FirstOrDefault();

            var role = UnitOfWork.RolesRepository.GetRoles(filter: p => p.Id == roleDetails.Id_Role.ToString(), includeProperties: "Users").FirstOrDefault();
            foreach (var item in role.Users)
            {
                var user = UnitOfWork.UserRepository.GetUserByID(item.UserId);
                var auxuser = UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);

                if (user!= null && (auxuser.Inactive == false || auxuser.Inactive == null))
                {
                    roleDetails.Users.Add(user);
                }
            }


            var auxuserlist = UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
                UserList.Add(auxuser);
            }

            // FunTour combo
            ViewBag.UserId = new SelectList(UserList, "Id_User", "UserName");


            // Rights combo
            ViewBag.PermissionId = new SelectList(UnitOfWork.PermissionRepository.Get(orderBy: p=> p.OrderBy(a => a.PermissionDescription)), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(roleDetails);
        }

        public ActionResult RoleCreate()
        {
           IdentityUser User = UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
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

            IdentityUser User = UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                if (UnitOfWork.RolesRepository.CreateRol(_role))
                {
                    UnitOfWork.Save();
                };
                return RedirectToAction("RoleIndex");
            }
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult RoleEdit(int id)
        {
        //    IdentityUser User = _context.Users.Where(r => r.UserName == this.User.Identity.Name).FirstOrDefault();
            var roleDetails = UnitOfWork.RolesRepository.GetRoleDetails(filter: r => r.Id_Role == id, includeProperties : "Permissions").FirstOrDefault();

            var role = UnitOfWork.RolesRepository.GetRoles(filter: p => p.Name == roleDetails.RoleName, includeProperties: "Users").FirstOrDefault();
            foreach (var item in role.Users)
            {
                var user = UnitOfWork.UserRepository.GetUserByID(item.UserId);
                var auxuser = UnitOfWork.UserRepository.GetUserDetailByUserName(user.UserName);

                if (user != null && (auxuser.Inactive == false || auxuser.Inactive == null))
                {
                    roleDetails.Users.Add(user);
                }
            }

            var auxuserlist = UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
                UserList.Add(auxuser);
            }

            // FunTour combo
            ViewBag.UserList = new SelectList(UserList, "Id", "UserName");


            // Rights combo
            ViewBag.PermissionId = new SelectList(UnitOfWork.PermissionRepository.Get(orderBy: p=> p.OrderBy(a => a.Id_Permission)), "Id_Permission", "PermissionDescription");
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
            
            IdentityUser User = UnitOfWork.UserRepository.GetUserByUserName(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                
                if (UnitOfWork.RolesRepository.Update(_role))
                {
                    UnitOfWork.Save();
                }
                return RedirectToAction("RoleDetails", new RouteValueDictionary(new { id = _role.Id_Role }));
            }

            var auxuserlist = UnitOfWork.UserRepository.GetUserDetails(filter: r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = UnitOfWork.UserRepository.GetUserByUserName(item.UserName);
                UserList.Add(auxuser);
            }

            // Users combo-
            ViewBag.UserList = new SelectList( UserList, "Id", "UserName");

            // Rights combo
            ViewBag.PermissionId = new SelectList(UnitOfWork.PermissionRepository.Get(orderBy: p=>p.OrderBy(a => a.Id_Permission)), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult RoleDelete(int id)
        {
            RoleDetails roleDetails = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);

            IdentityRole _role = UnitOfWork.RolesRepository.GetRoles(filter: p => p.Name == roleDetails.RoleName).FirstOrDefault();


            if (UnitOfWork.RolesRepository.DeleteRole(_role))
            {
                UnitOfWork.Save();
            }
            return RedirectToAction("RoleIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserFromRoleReturnPartialView(int id, int UserId)
        {
            IdentityRole role = UnitOfWork.RolesRepository.GetRoleByID(id);
            IdentityUser user = UnitOfWork.UserRepository.GetUserByID(UserId);
            

            if (UnitOfWork.RolesRepository.DeleteUserFromRole(user,role))
            {
                UnitOfWork.Save();
            }
            return PartialView("_ListUsersTable4Role", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUser2RoleReturnPartialView(int id, string UserId)
        {
            IdentityRole role = UnitOfWork.RolesRepository.GetRoleByID(id);
            IdentityUser User = UnitOfWork.UserRepository.GetUserByID(UserId);

            if (UnitOfWork.UserRepository.AddRolesToUser(User, role))
            {
                UnitOfWork.Save();
            }
            return PartialView("_ListUsersTable4Role", role);
        }

        #endregion

        #region Permissions

        public ViewResult PermissionIndex()
        {
            IEnumerable<Permission> _permissions = UnitOfWork.PermissionRepository.Get(orderBy : p=>p.OrderBy(wn => wn.PermissionDescription), includeProperties :"Roles");
            return View(_permissions);
        }

        public ViewResult PermissionDetails(int id)
        {
            Permission _permission = UnitOfWork.PermissionRepository.GetByID(id);
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
                UnitOfWork.PermissionRepository.Insert(_permission);
                UnitOfWork.Save();
                return RedirectToAction("PermissionIndex");
            }
            return View(_permission);
        }

        public ActionResult PermissionEdit(int id)
        {
            Permission _permission = UnitOfWork.PermissionRepository.GetByID(id);
            ViewBag.RoleId = new SelectList(UnitOfWork.RolesRepository.GetRoleDetails(orderBy: q=> q.OrderBy(p => p.RoleDescription)), "Id_Role", "RoleDescription");
            return View(_permission);
        }

        [HttpPost]
        public ActionResult PermissionEdit(Permission _permission)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.PermissionRepository.Update(_permission);
                UnitOfWork.Save();
                return RedirectToAction("PermissionDetails", new RouteValueDictionary(new { id = _permission.Id_Permission }));
            }
            return View(_permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult PermissionDelete(int id)
        {
            Permission permission = UnitOfWork.PermissionRepository.GetByID(id);
            if (permission.Roles.Count > 0)
                permission.Roles.Clear();

            UnitOfWork.PermissionRepository.Delete(permission);
            UnitOfWork.Save();
            return RedirectToAction("PermissionIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddPermission2RoleReturnPartialView(int id, int permissionId)
        {
            if (UnitOfWork.RolesRepository.AddPermissionToRole(id, permissionId))
            {
                UnitOfWork.Save();
            }
            RoleDetails role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView("_ListPermissions", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddAllPermissions2RoleReturnPartialView(string id)
        {

            if (UnitOfWork.RolesRepository.AddAllPermissions2Role(id))
            {
                UnitOfWork.Save();
            }

            RoleDetails _role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            return PartialView("_ListPermissions", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeletePermissionFromRoleReturnPartialView(int id, int permissionId)
        {
            RoleDetails _role = UnitOfWork.RolesRepository.GetRoleDetailsByID(id);
            if (UnitOfWork.RolesRepository.DeletePermissionFromRole(id, permissionId))
            {
                UnitOfWork.Save();
            }
            return PartialView("_ListPermissions", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteRoleFromPermissionReturnPartialView(int id, int permissionId)
        {
            Permission permission= UnitOfWork.PermissionRepository.GetByID(permissionId);
            if (UnitOfWork.RolesRepository.DeleteRoleFromPermission(id, permissionId))
            {
                UnitOfWork.Save();
            }
            return PartialView("_ListRolesTable4Permission", permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddRole2PermissionReturnPartialView(int permissionId, int roleId)
        {
            Permission _permission = UnitOfWork.PermissionRepository.GetByID(permissionId);

            if (UnitOfWork.RolesRepository.AddRole2Permission(permissionId, roleId))
            {
                UnitOfWork.Save();
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
                    Permission _permission = UnitOfWork.PermissionRepository.Get(filter : p => p.PermissionDescription == _permissionDescription).FirstOrDefault();
                    if (_permission == null)
                    {
                        if (ModelState.IsValid)
                        {
                            Permission _perm = new Permission();
                            _perm.PermissionDescription = _permissionDescription;

                            UnitOfWork.PermissionRepository.Insert(_perm);
                            UnitOfWork.Save();
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
    }
}