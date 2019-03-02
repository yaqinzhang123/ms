using DYFramework.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DYFramework.Repository
{
    public class RepositoryContext : IRepositoryContext
    {
        protected DbContext context;

        public RepositoryContext(DbContext context)
        {
            this.context = context;
            //this.context.Database.EnsureCreatedAsync();
        }

        public virtual IQueryable<TAggregateRoot> GetReadEntity<TAggregateRoot>() where TAggregateRoot:AggregateRoot
        {
            return this.context.Set<TAggregateRoot>().AsNoTracking();
        }

        public virtual DbSet<TAggregateRoot> GetUpdateEntity<TAggregateRoot>() where TAggregateRoot:AggregateRoot
        {
            return this.context.Set<TAggregateRoot>();
        }

        public virtual void Add<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot
        {
            aggregateRoot.CreateTime = DateTime.Now;
            aggregateRoot.LastUpdateTime = DateTime.Now;
            this.GetUpdateEntity<TAggregateRoot>().Add(aggregateRoot);
        }

        public virtual int Commit()
        {
            return this.context.SaveChanges();
        }

        public virtual IQueryable<TAggregateRoot> Get<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> expression) where TAggregateRoot : AggregateRoot
        { 
            return this.GetAll<TAggregateRoot>().Where(expression);
        }

        public virtual IQueryable<TAggregateRoot> Get<TAggregateRoot>(int pageNo, int pageSize) where TAggregateRoot : AggregateRoot
        {
            return this.GetAll<TAggregateRoot>().Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public virtual IQueryable<TAggregateRoot> Get<TAggregateRoot,Tkey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, Tkey>> keySelector, int pageNo, int pageSize) where TAggregateRoot : AggregateRoot
        {
            return this.GetAll<TAggregateRoot>().Where(expression).OrderBy(keySelector).Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking();
        }

        public virtual IQueryable<TAggregateRoot> GetByDescending<TAggregateRoot, Tkey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, Tkey>> keySelector) where TAggregateRoot : AggregateRoot
        {
            return this.GetAll<TAggregateRoot>().Where(expression).OrderByDescending(keySelector);
        }

        public virtual IQueryable<TAggregateRoot> GetByDescending<TAggregateRoot,Tkey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, Tkey>> keySelector, int pageNo, int pageSize) where TAggregateRoot : AggregateRoot
        {
            return this.GetAll<TAggregateRoot>().Where(expression).OrderByDescending(keySelector).Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking();
        }

        public virtual IQueryable<TAggregateRoot> GetAll<TAggregateRoot>() where TAggregateRoot : AggregateRoot
        {
            return this.GetReadEntity<TAggregateRoot>().Where(p => !p.Deleted);
        }

        public virtual void Remove<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot
        {
            TAggregateRoot agg = this.GetUpdateEntity<TAggregateRoot>().Find(aggregateRoot.ID);
            agg.Deleted = true;
            this.GetUpdateEntity<TAggregateRoot>().Update(agg);
        }

        public virtual void Update<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot
        {
            aggregateRoot.LastUpdateTime = DateTime.Now;
            this.GetUpdateEntity<TAggregateRoot>().Update(aggregateRoot);
        }

        public TAggregateRoot Create<TAggregateRoot>() where TAggregateRoot : AggregateRoot,new()
        {                    
            return new TAggregateRoot();
        }

        public void AddList<TAggregateRoot>(IList<TAggregateRoot> list) where TAggregateRoot : AggregateRoot, new()
        {
            foreach(var entity in list)
            {
                DateTime now = DateTime.Now;
                entity.CreateTime = now;
                entity.LastUpdateTime = now;
            }
            this.GetUpdateEntity<TAggregateRoot>().AddRange(list);
        }
    }
}
