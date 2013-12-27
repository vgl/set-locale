using System;
using System.Linq;
using System.Linq.Expressions;

using SetLocale.Client.Web.Entities;

namespace SetLocale.Client.Web.Repositories
{
    public interface IRepository<TEntity>
           where TEntity : BaseEntity
    {
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);

        void SoftDelete(int id);
        void SoftDelete(Expression<Func<TEntity, bool>> where);

        void Delete(long id);
        void Delete(Expression<Func<TEntity, bool>> where);

        TEntity FindOne(Expression<Func<TEntity, bool>> where = null);
        TEntity FindById(long id);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> where = null);
        IQueryable<T> Set<T>() where T : class;

        bool SaveChanges();
    }
}