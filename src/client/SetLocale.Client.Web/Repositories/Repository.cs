using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using SetLocale.Client.Web.Entities;

namespace SetLocale.Client.Web.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected SetLocaleDbContext Context;

        public Repository()
        {
            Context = new SetLocaleDbContext();
        }

        public virtual TEntity Create(TEntity entity)
        {
            return Context.Set<TEntity>().Add(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual void Delete(long id)
        {
            var entity = Context.Set<TEntity>().Find(id);
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            var objects = Context.Set<TEntity>().Where(where).AsEnumerable();
            foreach (var item in objects)
            {
                Context.Set<TEntity>().Remove(item);
            }
        }

        public virtual void SoftDelete(int id, int deletedBy)
        {
            var entity = Context.Set<TEntity>().Find(id);
            entity.DeletedAt = DateTime.Now;
            entity.DeletedBy = deletedBy;
            entity.IsDeleted = true;
        }

        public void SoftDelete(Expression<Func<TEntity, bool>> where, int deletedBy)
        {
            var objects = Context.Set<TEntity>().Where(where).AsEnumerable();
            foreach (var item in objects)
            {
                item.DeletedAt = DateTime.Now;
                item.DeletedBy = deletedBy;
                item.IsDeleted = true;
            }
        }
        public virtual TEntity FindById(long id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where = null)
        {
            return FindAll(where).FirstOrDefault();
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return Context.Set<T>();
        }

        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> where = null)
        {
            return null != where ? Context.Set<TEntity>().Where(s => !s.IsDeleted).Where(where) : Context.Set<TEntity>().Where(s => !s.IsDeleted);
        }

        public virtual bool SaveChanges()
        {
            return 0 < Context.SaveChanges();
        }

        public void Dispose()
        {
            if (null != Context)
            {
                Context.Dispose();
            }
        }
    }
}