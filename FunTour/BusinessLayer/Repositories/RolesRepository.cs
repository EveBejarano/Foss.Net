using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FunTourDataLayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BusinessLayer.Repositories
{
    public class RolesRepository<TEntity,TEntity2> : GenericRepository<TEntity, TEntity2>
        where TEntity : IdentityRole where TEntity2 : RoleDetails
    {

        internal ApplicationDbContext _context = new ApplicationDbContext();
        internal DbSet<RoleDetails> RoleDetails { get; set; }
        internal DbSet<IdentityRole> Roles { get; set; }

        public RolesRepository(ApplicationDbContext context) : base(context)
        {

            _context = context;
            this.Roles = context.Set<IdentityRole>();
            this.RoleDetails = context.Set<RoleDetails>();
        }

        public IEnumerable<IdentityRole> GetRoles(
                                        Expression<Func<IdentityRole, bool>> filter = null,
                                        Func<IQueryable<IdentityRole>, IOrderedQueryable<IdentityRole>> orderBy = null,
                                        string includeProperties = "")
        {
            IQueryable<IdentityRole> query = dbSet;

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

        public virtual RoleDetails GetRoleDetailsByID(object id)
        {
            return RoleDetails.Find(id);
        }

        public  bool CreateRol(RoleDetails _role)
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
                context.Entry(_role).State = EntityState.Modified;
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
                RoleDetails roleDetails = RoleDetails.FirstOrDefault(p=> p.RoleName == _role.Name);
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
    }
}
