using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DYFramework.Repository
{
    public class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot,new()
    {
        public IRepositoryContext Context { get; set; }

        public Repository(IRepositoryContext context)
        {
            this.Context = context;
        }
        public virtual void Add(TAggregateRoot aggregateRoot)
        {
            this.Context.Add(aggregateRoot);
        }

        public virtual int Commit()
        {
            return this.Context.Commit();
        }

        public virtual TAggregateRoot Create()
        {
            return this.Context.Create<TAggregateRoot>();
        }

        public virtual bool Exists(Func<TAggregateRoot, bool> condition)
        {
            return this.Context.GetReadEntity<TAggregateRoot>().Where(p => !p.Deleted).Any(condition);
        }

        public virtual IQueryable<TAggregateRoot> Get(Expression<Func<TAggregateRoot, bool>> expression)
        {
            return this.Context.Get(expression);
        }

        public virtual IQueryable<TAggregateRoot> Get(int pageNo, int pageSize)
        {
            return this.Context.Get<TAggregateRoot>(pageNo, pageSize);
        }

        public virtual IQueryable<TAggregateRoot> Get<TKey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, TKey>> keySelector, int pageNo, int pageSize)
        {
            return this.Context.Get(expression, keySelector, pageNo, pageSize);
        }

        public virtual IQueryable<TAggregateRoot> GetByDescending<TKey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, TKey>> keySelector, int pageNo, int pageSize)
        {
            return this.Context.GetByDescending(expression, keySelector, pageNo, pageSize);
        }

        public virtual IQueryable<TAggregateRoot> GetByDescending<TKey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, TKey>> keySelector)
        {
            return this.Context.GetByDescending(expression, keySelector);
        }

        public virtual IQueryable<TAggregateRoot> GetAll()
        {
            return this.Context.GetAll<TAggregateRoot>();
        }

        public virtual int GetCount()
        {
            return this.Context.GetReadEntity<TAggregateRoot>().Count();
        }

        public virtual void Remove(TAggregateRoot aggregateRoot)
        {
            this.Context.Remove(aggregateRoot);
        }

        public virtual void Update(TAggregateRoot aggregateRoot)
        {
            this.Context.Update(aggregateRoot);
        }

        public void AddList(IList<TAggregateRoot> list)
        {
            this.Context.AddList(list);
        }
    }
}
