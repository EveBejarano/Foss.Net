using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FunTourDataLayer;
using FunTourDataLayer.AccountManagement;

namespace FunTourBusinessLayer.Repositories
{
    public class UserRepository<TEntity, TEntity2> 
        where TEntity : IdentityUser
    {

        internal ApplicationDbContext _context = new ApplicationDbContext();
        internal DbSet<UserDetails> UserDetails { get; set; }
        internal DbSet<IdentityUser> Users { get; set; }
        internal DbSet<IdentityUserRole> UserRoles { get; set; }


        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            this.Users = context.Set<IdentityUser>();
            this.UserDetails = context.Set<UserDetails>();
            this.UserRoles = context.Set<IdentityUserRole>();
        }

        public virtual IEnumerable<UserDetails> GetUserDetails(
                                        Expression<Func<UserDetails, bool>> filter = null,
                                        Func<IQueryable<UserDetails>, IOrderedQueryable<UserDetails>> orderBy = null,
                                        string includeProperties = "")
        {
            IQueryable<UserDetails> query = UserDetails;

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


        public virtual IEnumerable<IdentityUser> GetUsers(
                                        Expression<Func<IdentityUser, bool>> filter = null,
                                        Func<IQueryable<IdentityUser>, IOrderedQueryable<IdentityUser>> orderBy = null,
                                        string includeProperties = "")
        {
            IQueryable<IdentityUser> query = Users;

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

        public IdentityUser GetUserByUserName(string userName)
        {
            return Users.FirstOrDefault(p => p.UserName == userName);
        }


        public UserDetails GetUserDetailByUserName(string userName)
        {
            return UserDetails.FirstOrDefault(p => p.UserName == userName);
        }


        public virtual IdentityUser GetUserByID(object id)
        {
            return Users.Find(id);
        }

        public List <String> GetStringUserNames(string UserName , string Email)
        {
            var aux = (from d in _context.UserDetails
                      join u in _context.Users
                      on d.UserName equals u.UserName
                      where d.UserName == UserName
                      || (d.UserName == u.UserName && u.Email == Email)
                      select d.UserName).ToList();
            return aux;
        }

        public  IEnumerable<UserDetails> GetUserDetailsByNameEmail(string UserName, string Email)
        {
           
            var aux = (from d in _context.UserDetails
                       join u in _context.Users
                       on d.UserName equals u.UserName
                       where d.UserName == UserName
                       || (d.UserName == u.UserName && u.Email == Email)
                       select d);
            return aux;
        }

        public  void ActivateUserDetails(UserDetails _User)
        {
            
            UserDetails.Attach(_User);
            _context.Entry(_User).Entity.Inactive = false;
            _context.Entry(_User).Entity.LastModified = System.DateTime.Now;

            _context.Entry(_User).State = EntityState.Modified;

            
        }

        public void CreateUserDetails(UserDetails NewUser)
        {

            var userDetails = new UserDetails
            {
                UserName = _context.Users.FirstOrDefault(p => p.UserName == NewUser.UserName).UserName,
                FirstName = NewUser.FirstName,
                LastName = NewUser.LastName,
                LastModified = System.DateTime.Now,
                Inactive = false,
                isSysAdmin = true
            };

            UserDetails.Add(userDetails);
        }

        public bool DeleteRoleFromUser(IdentityUser user, IdentityRole role)
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

        public virtual bool DeleteUserDetail(string userId)
        {

            var User = _context.UserDetails.Find(userId);
            if (User != null)
            {
                UserDetails.Attach(User);
                User.Inactive = true;
                User.LastModified = System.DateTime.Now;

                _context.Entry(User).State = EntityState.Modified;
                return true;

            }

            return false;
        }

        public void UpdateUser(IdentityUser user)
        {
            Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
        }

        public void UpdateUserDetails(UserDetails userDetails)
        {
            UserDetails.Attach(userDetails);
            _context.Entry(userDetails).State = EntityState.Modified;
        }

        public bool AddRolesToUser(IdentityUser User, IdentityRole role)
        {
            //this.GetRolesToUserFromList();

            IdentityUserRole auxuserRole = new IdentityUserRole
            {
                RoleId = role.Id,
                UserId = User.Id
            };

            if (!role.Users.Contains(auxuserRole))
            {
                role.Users.Add(auxuserRole);
                return true;
            }
            return false;
        }

    }
}
