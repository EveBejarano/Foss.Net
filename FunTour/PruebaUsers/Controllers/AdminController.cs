using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PruebaUsers.ActualModels;
using PruebaUsers.Models;

namespace PruebaUsers.Controllers
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

        private ApplicationDbContext _context = new ApplicationDbContext();

        #region PruebaUsers

        // Muestra en el index el listado de todos los usuarios activos
        // GET: Admin
        public ActionResult Index()
        {
            // lista los usuarios activos
            // verifica si el usuario esta activo o no. los ordena primero por apellido y despues por nombre
            // List<UserDetails> UserList = _context.Database.SqlQuery<UserDetails>(string.Format("SELECT * FROM IdentityUsers p, UserDetails d WHERE p.UserName = d.UserName And(d.Inactive != 'true')", new UserDetails().LastName)).ToList();

            List<UserDetails> UserList = (from d in _context.UserDetails
                                          join u in _context.Users
                                          on d.UserName equals u.UserName
                                          where d.Inactive != true
                                          select d).ToList();


            List < UserModel > UserModelList = new List<UserModel>();

            foreach (var item in UserList)
            {
                var auxUser = _context.Users.FirstOrDefault(p => p.UserName == item.UserName);

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
            var user = _context.Users.FirstOrDefault(p => p.Id == IdUser);
            var User = UserModel.GetDataUserModel(user.UserName);
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
            ViewBag.RoleId = new SelectList(_context.Roles.OrderBy(p => p.Name), "Id_Role", "RoleName");
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
                    List<string> results = (from d in _context.UserDetails
                                            join u in _context.Users
                                            on d.UserName equals u.UserName
                                            where d.UserName == NewUser.UserModelName
                                            || (d.UserName == u.UserName && u.Email == NewUser.Email)
                                            select d.UserName).ToList();

                    bool _UserExistsInTable = (results.Count > 0);

                    UserDetails _User = null;

                    // si hay usuarios con el mismo nombre, busca el primero y si esta activo devuelve error
                    // Si el usuario no esta activo, lo activa
                    if (_UserExistsInTable)
                    {
                        //_User = _context.UserDetails.Where(p => p.UserName == NewUser.UserModelName).FirstOrDefault();

                         var Users =
                            from d in _context.UserDetails
                            join u in _context.Users
                            on d.UserName equals u.UserName
                            where d.UserName == NewUser.UserModelName
                            || (d.UserName == u.UserName && u.Email == NewUser.Email)
                            select d;

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
                                _context.Entry(_User).Entity.Inactive = false;
                                _context.Entry(_User).Entity.LastModified = System.DateTime.Now;
                                _context.Entry(_User).State = EntityState.Modified;
                                _context.SaveChanges();
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
                            using (var _context = new ApplicationDbContext())
                            {

                                var userDetails = new UserDetails
                                {
                                    UserName = _context.Users.FirstOrDefault(p => p.UserName == NewUser.UserModelName).UserName,

                                    FirstName = NewUser.FirstName,
                                    LastName = NewUser.LastName,
                                    LastModified = System.DateTime.Now,
                                    Inactive = false,
                                    isSysAdmin = true
                                };

                                try
                                {

                                    _context.UserDetails.Add(userDetails);
                                    _context.SaveChanges();
                                }
                                catch (Exception ex)
                                {

                                    throw;
                                }

                            }

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
            
            var User = UserModel.GetDataUserModel(UserName);
            SetViewBagData(UserName);

            return View(User);
        }

        [HttpPost]
        public ActionResult UserEdit(UserModel User)
        {
            var _User = _context.Users.Where(p => p.Id == User.Id_UserModel).FirstOrDefault();
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
            return RedirectToAction("UserDetails", new RouteValueDictionary(new { IdUser = _User.Id }));
        }






        [HttpGet]
        public ActionResult DeleteUserRole(string id, string UserId)
        {
            var role = _context.Roles.Find(id);
            var user = _context.Users.Find(UserId);

            IdentityUserRole userrole = new IdentityUserRole {
                UserId = user.Id,
                RoleId = role.Id
            }; 


            if (role.Users.Contains(userrole))
            {
                role.Users.Remove(userrole);
                user.Roles.Remove(userrole);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "User", new { id = UserId });
        }

        [HttpGet]
        public PartialViewResult filter4PruebaUsers(string _surname)
        {
            return PartialView("_ListUserTable", GetFilteredUserList(_surname));
        }

        [HttpGet]
        public PartialViewResult filterReset()
        {
            List<UserDetails> usersdetails = _context.UserDetails.Where(r => r.Inactive == false || r.Inactive == null).ToList();

            List<UserModel> users = new List<UserModel>();
            foreach (var user in usersdetails)
            {
                var newuser = new UserModel
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Id_UserModel = _context.Users.FirstOrDefault(p=> p.UserName == user.UserName).Id,
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
                var User = _context.UserDetails.Find(UserId);
                if (User != null)
                {
                    User.Inactive = true;
                    User.LastModified = System.DateTime.Now;

                    var User1 = _context.UserDetails.Find(UserId);
                    _context.Entry(User1).CurrentValues.SetValues(User);
                    _context.SaveChanges();
                    
                }
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
                    var userDetails = _context.UserDetails.Where(r => r.Inactive == false || r.Inactive == null).ToList();

                    foreach (var user in userDetails)
                    {
                        var userModel = new UserModel
                        {

                            Id_UserModel = _context.Users.FirstOrDefault(p => p.UserName == user.UserName).Id,
                            UserModelName = user.UserName,
                            Inactive = user.Inactive,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            IsSysAdmin = user.isSysAdmin

                        };

                        var users = _context.Users.FirstOrDefault(p => p.UserName == _surname);

                        userModel.UserModelName = users.UserName;
                        userModel.Email = users.Email;
                        userModel.PhoneNumber = users.PhoneNumber;

                        _ret.Add(userModel);

                    }
                }
                else
                {
                    ICollection<ApplicationUser> users = _context.Users.Where(p => p.UserName == _surname).ToList();

                    foreach (var user in users)
                    {
                        var userModel = new UserModel
                        {
                            Id_UserModel = user.Id,
                            UserModelName = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber
                        };

                        var userDetail = _context.UserDetails.FirstOrDefault(p => p.UserName == user.UserName);

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

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserRoleReturnPartialView(int id, string UserId)
        {
            IdentityRole role = _context.Roles.Find(id);
            IdentityUser User = _context.Users.Find(UserId);
            IdentityUserRole user_role = new IdentityUserRole
            {
                RoleId = role.Id,
                UserId = User.Id
            };

            if (role.Users.Contains(user_role))
            {
                role.Users.Remove(user_role);
                _context.SaveChanges();
            }
            SetViewBagData(UserId.ToString());

            return PartialView("_ListUserRoleTable", User.UserName);
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
            return View(_context.RoleDetails.OrderBy(r => r.RoleDescription).ToList());
        }

        public ViewResult RoleDetails(int id)
        {

            IdentityUser User = _context.Users.Where(r => r.UserName == this.User.Identity.Name).FirstOrDefault();
            var roleDetails = _context.RoleDetails.Where(r => r.Id_Role == id)
                   .Include(a => a.Permissions)
                   .FirstOrDefault();

            var role = _context.Roles.Where(p => p.Id == roleDetails.Id_Role.ToString()).Include("Users").FirstOrDefault();
            foreach (var item in role.Users)
            {
                var user = _context.Users.FirstOrDefault(p => p.Id == item.UserId);
                var auxuser = _context.UserDetails.FirstOrDefault(r => r.UserName == user.UserName);

                if (user!= null && (auxuser.Inactive == false || auxuser.Inactive == null))
                {
                    roleDetails.Users.Add(user);
                }
            }


            var auxuserlist = _context.UserDetails.Where(r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = _context.Users.FirstOrDefault(p => p.UserName == item.UserName);
                UserList.Add(auxuser);
            }

            // PruebaUsers combo
            ViewBag.UserId = new SelectList(UserList, "Id_User", "UserName");


            // Rights combo
            ViewBag.PermissionId = new SelectList(_context.Permissions.OrderBy(a => a.PermissionDescription), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(roleDetails);
        }

        public ActionResult RoleCreate()
        {
           IdentityUser User = _context.Users.Where(r => r.UserName == this.User.Identity.Name).FirstOrDefault();
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

            IdentityUser User = _context.Users.Where(r => r.UserName == this.User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Id = _role.Id_Role.ToString(),
                    Name = _role.RoleName
                };

                _context.Roles.Add(role);
                _context.RoleDetails.Add(_role);
                _context.SaveChanges();
                return RedirectToAction("RoleIndex");
            }
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult RoleEdit(int id)
        {
        //    IdentityUser User = _context.Users.Where(r => r.UserName == this.User.Identity.Name).FirstOrDefault();
            var roleDetails = _context.RoleDetails.Where(r => r.Id_Role == id)
                   .Include(a => a.Permissions)
                   .FirstOrDefault();

            var role = _context.Roles.Where(p => p.Name == roleDetails.RoleName).Include("Users").FirstOrDefault();
            foreach (var item in role.Users)
            {
                var user = _context.Users.FirstOrDefault(p => p.Id == item.UserId);
                var auxuser = _context.UserDetails.FirstOrDefault(r => r.UserName == user.UserName);

                if (user != null && (auxuser.Inactive == false || auxuser.Inactive == null))
                {
                    roleDetails.Users.Add(user);
                }
            }



            var auxuserlist = _context.UserDetails.Where(r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = _context.Users.FirstOrDefault(p => p.UserName == item.UserName);
                UserList.Add(auxuser);
            }

            // PruebaUsers combo
            ViewBag.UserId = new SelectList(UserList, "Id_User", "UserName");


            // Rights combo
            ViewBag.PermissionId = new SelectList(_context.Permissions.OrderBy(a => a.Id_Permission), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(role);
        }

        [HttpPost]
        public ActionResult RoleEdit(RoleDetails _role)
        {
            if (string.IsNullOrEmpty(_role.RoleDescription))
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
            }

            //EntityState state = _context.Entry(_role).State;
            IdentityUser User = _context.Users.Where(r => r.UserName == this.User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                IdentityRole role = _context.Roles.FirstOrDefault(p => p.Id == _role.Id_Role.ToString());

                role.Name = _role.RoleName;

                _context.Entry(role).CurrentValues.SetValues(role);

                _context.Entry(_role).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("RoleDetails", new RouteValueDictionary(new { id = _role.Id_Role }));
            }

            var auxuserlist = _context.UserDetails.Where(r => r.Inactive == false || r.Inactive == null);

            List<IdentityUser> UserList = new List<IdentityUser>();

            foreach (var item in auxuserlist)
            {
                var auxuser = _context.Users.FirstOrDefault(p => p.UserName== item.UserName);
                UserList.Add(auxuser);
            }

            // PruebaUsers combo
            ViewBag.UserId = new SelectList( UserList, "Id_User", "UserName");

            // Rights combo
            ViewBag.PermissionId = new SelectList(_context.Permissions.OrderBy(a => a.Id_Permission), "Id_Permission", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult RoleDelete(int id)
        {
            RoleDetails roleDetails = _context.RoleDetails.Find(id);

            IdentityRole _role = _context.Roles.FirstOrDefault( p => p.Name == roleDetails.RoleName);


            if (_role != null)
            {
                _role.Users.Clear();
                roleDetails.Permissions.Clear();

                _context.Entry(_role).State = EntityState.Deleted;

                _context.Entry(roleDetails).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            return RedirectToAction("RoleIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserFromRoleReturnPartialView(int id, int UserId)
        {
            IdentityRole role = _context.Roles.Find(id);
            IdentityUser User = _context.Users.Find(UserId);

            IdentityUserRole auxuserRole = new IdentityUserRole
            {
                RoleId = role.Id,
                UserId = User.Id
            };
            

            if (role.Users.Contains(auxuserRole))
            {
                role.Users.Remove(auxuserRole);
                _context.SaveChanges();
            }
            return PartialView("_ListPruebaUsersTable4Role", role);
        }

        //[HttpGet]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public PartialViewResult AddUser2RoleReturnPartialView(int id, int UserId)
        //{
        //    IdentityRole role = _context.Roles.Find(id);
        //    IdentityUser User = _context.Users.Find(UserId);

        //    IdentityUserRole auxuserRole = new IdentityUserRole
        //    {
        //        RoleId = role.Id,
        //        UserId = User.Id
        //    };

        //    if (!role.Users.Contains(auxuserRole))
        //    {
        //        role.Users.Add(auxuserRole);
        //        _context.SaveChanges();
        //    }
        //    return PartialView("_ListPruebaUsersTable4Role", role);
        //}

        #endregion

        #region Permissions

        public ViewResult PermissionIndex()
        {
            List<Permission> _permissions = _context.Permissions
                               .OrderBy(wn => wn.PermissionDescription)
                               .Include(a => a.Roles)
                               .ToList();
            return View(_permissions);
        }

        public ViewResult PermissionDetails(int id)
        {
            Permission _permission = _context.Permissions.Find(id);
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
                _context.Permissions.Add(_permission);
                _context.SaveChanges();
                return RedirectToAction("PermissionIndex");
            }
            return View(_permission);
        }

        public ActionResult PermissionEdit(int id)
        {
            Permission _permission = _context.Permissions.Find(id);
            ViewBag.RoleId = new SelectList(_context.RoleDetails.OrderBy(p => p.RoleDescription), "Id_Role", "RoleDescription");
            return View(_permission);
        }

        [HttpPost]
        public ActionResult PermissionEdit(Permission _permission)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(_permission).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("PermissionDetails", new RouteValueDictionary(new { id = _permission.Id_Permission }));
            }
            return View(_permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult PermissionDelete(int id)
        {
            Permission permission = _context.Permissions.Find(id);
            if (permission.Roles.Count > 0)
                permission.Roles.Clear();

            _context.Entry(permission).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("PermissionIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddPermission2RoleReturnPartialView(int id, int permissionId)
        {
            RoleDetails role = _context.RoleDetails.Find(id);
            Permission _permission = _context.Permissions.Find(permissionId);

            if (!role.Permissions.Contains(_permission))
            {
                role.Permissions.Add(_permission);
                _context.SaveChanges();
            }
            return PartialView("_ListPermissions", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddAllPermissions2RoleReturnPartialView(string id)
        {
            RoleDetails _role = _context.RoleDetails.Where(p => p.Id_Role.ToString() == id).FirstOrDefault();
            List<Permission> _permissions = _context.Permissions.ToList();
            foreach (Permission _permission in _permissions)
            {
                if (!_role.Permissions.Contains(_permission))
                {
                    _role.Permissions.Add(_permission);

                }
            }
            _context.SaveChanges();
            return PartialView("_ListPermissions", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeletePermissionFromRoleReturnPartialView(int id, int permissionId)
        {
            RoleDetails _role = _context.RoleDetails.Find(id);
            Permission _permission = _context.Permissions.Find(permissionId);

            if (_role.Permissions.Contains(_permission))
            {
                _role.Permissions.Remove(_permission);
                _context.SaveChanges();
            }
            return PartialView("_ListPermissions", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteRoleFromPermissionReturnPartialView(int id, int permissionId)
        {
            RoleDetails role = _context.RoleDetails.Find(id);
            Permission permission = _context.Permissions.Find(permissionId);

            if (role.Permissions.Contains(permission))
            {
                role.Permissions.Remove(permission);
                _context.SaveChanges();
            }
            return PartialView("_ListRolesTable4Permission", permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddRole2PermissionReturnPartialView(int permissionId, int roleId)
        {
            RoleDetails role = _context.RoleDetails.Find(roleId);
            Permission _permission = _context.Permissions.Find(permissionId);

            if (!role.Permissions.Contains(_permission))
            {
                role.Permissions.Add(_permission);
                _context.SaveChanges();
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
                    Permission _permission = _context.Permissions.Where(p => p.PermissionDescription == _permissionDescription).FirstOrDefault();
                    if (_permission == null)
                    {
                        if (ModelState.IsValid)
                        {
                            Permission _perm = new Permission();
                            _perm.PermissionDescription = _permissionDescription;

                            _context.Permissions.Add(_perm);
                            _context.SaveChanges();
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