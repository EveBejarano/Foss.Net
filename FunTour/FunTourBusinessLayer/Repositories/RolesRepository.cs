using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FunTourDataLayer;
using FunTourDataLayer.AccountManagement;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FunTourBusinessLayer.Repositories
{
    public class RolesRepository<TEntity, TEntity2>
        where TEntity : IdentityRole
    {

        internal ApplicationDbContext _context = new ApplicationDbContext();
        internal DbSet<RoleDetails> RoleDetails { get; set; }
        internal DbSet<IdentityRole> Roles { get; set; }
        internal DbSet<Permission> Permission { get; set; }

        public RolesRepository(ApplicationDbContext context)
        {

            _context = context;
            this.Roles = context.Set<IdentityRole>();
            this.RoleDetails = context.Set<RoleDetails>();
            this.Permission = context.Set<Permission>();
        }

        public IEnumerable<IdentityRole> GetRoles(
                                        Expression<Func<IdentityRole, bool>> filter = null,
                                        Func<IQueryable<IdentityRole>, IOrderedQueryable<IdentityRole>> orderBy = null,
                                        string includeProperties = "")
        {
            IQueryable<IdentityRole> query = Roles;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        public IEnumerable<RoleDetails> GetRoleDetails(
                                        Expression<Func<RoleDetails, bool>> filter = null,
                                        Func<IQueryable<RoleDetails>, IOrderedQueryable<RoleDetails>> orderBy = null,
                                        string includeProperties = "")
        {
            IQueryable<RoleDetails> query = RoleDetails;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        public virtual IdentityRole GetRoleByID(object id)
        {
            return Roles.Find(id);
        }

        public virtual RoleDetails GetRoleDetailsByID(string id)
        {
            return RoleDetails.FirstOrDefault(p => p.Id_Role.ToString() == id);
        }

        public virtual RoleDetails GetRoleDetailsByID(int id)
        {
            return RoleDetails.FirstOrDefault(p => p.Id_Role == id);
        }

        public bool CreateRol(RoleDetails _role)
        {

            IdentityRole role = new IdentityRole
            {
                Id = _role.Id_Role.ToString(),
                Name = _role.RoleName
            };

            try
            {
                Roles.Add(role);
                RoleDetails.Add(_role);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


        public virtual bool Update(RoleDetails _role)
        {
            try
            {
                RoleDetails.Attach(_role);
                _context.Entry(_role).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public virtual bool DeleteRole(IdentityRole _role)
        {

            if (_role != null)
            {
                RoleDetails roleDetails = RoleDetails.FirstOrDefault(p => p.RoleName == _role.Name);
                _role.Users.Clear();
                roleDetails.Permissions.Clear();

                _context.Entry(_role).State = EntityState.Deleted;
                _context.Entry(roleDetails).State = EntityState.Deleted;

                return true;
            }

            return false;
        }

        public bool DeleteUserFromRole(IdentityUser user, IdentityRole role)
        {

            IdentityUserRole userrole = new IdentityUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };


            if (role.Users.Contains(userrole))
            {
                role.Users.Remove(userrole);
                user.Roles.Remove(userrole);
                return true;
            }
            return false;
        }

        public bool AddPermissionToRole(int id, int permissionId)
        {
            RoleDetails role = RoleDetails.FirstOrDefault(p => p.Id_Role == id);
            Permission _permission = Permission.Find(permissionId);

            if (!role.Permissions.Contains(_permission))
            {
                role.Permissions.Add(_permission);
                return true;
            }
            return false;
        }

        public bool AddRole2Permission(int permissionId, int roleId)
        {
            RoleDetails role = _context.RoleDetails.Find(roleId);
            Permission _permission = _context.Permissions.Find(permissionId);

            if (!role.Permissions.Contains(_permission))
            {
                role.Permissions.Add(_permission);
                return true;
            }
            return false;
        }

        public bool AddAllPermissions2Role(string id)
        {
            RoleDetails _role = RoleDetails.Where(p=> p.Id_Role.ToString() == id).Include("Permissions").FirstOrDefault();
            IEnumerable<Permission> _permissions = Permission.ToList();

            try
            {
                foreach (Permission _permission in _permissions)
                {
                    if (!_role.Permissions.Contains(_permission))
                    {
                        _role.Permissions.Add(_permission);

                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool DeletePermissionFromRole(int id, int permissionId)
        {

            RoleDetails _role = RoleDetails.FirstOrDefault(p => p.Id_Role == id);
            Permission _permission = Permission.Find(permissionId); 

            if (_role.Permissions.Contains(_permission))
            {
                _role.Permissions.Remove(_permission);
                return true;
            }

            return false;
        }

        public bool DeleteRoleFromPermission(int id, int permissionId)
        {
            RoleDetails role = _context.RoleDetails.Find(id);
            Permission permission = _context.Permissions.Find(permissionId);

            if (role.Permissions.Contains(permission))
            {
                role.Permissions.Remove(permission);
                return true;
            }
            return false;
        }

        public IdentityRole GetRoleByName(string roleName)
        {
            return Roles.FirstOrDefault(p => p.Name == roleName);
        }
    }
}
