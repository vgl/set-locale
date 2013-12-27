using System;
using System.Linq;
using System.Linq.Expressions;
using SetLocale.Client.Web.Entities;

namespace SetLocale.Client.Web.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public TEntity Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void SoftDelete(int id)
        {
            throw new NotImplementedException();
        }

        public void SoftDelete(Expression<Func<TEntity, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<TEntity, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> @where = null)
        {
            throw new NotImplementedException();
        }

        public TEntity FindById(long id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> @where = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Set<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}