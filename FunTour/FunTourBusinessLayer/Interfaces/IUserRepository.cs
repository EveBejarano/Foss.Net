using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FunTourBusinessLayer.Interfaces
{
    interface IUserRepository<TEntity>
    {
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        bool Equals(object obj);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        TEntity GetByID(object id);
        int GetHashCode();
        void Insert(TEntity entity);
        string ToString();
        void Update(TEntity entityToUpdate);
    }
}