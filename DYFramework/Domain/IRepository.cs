using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DYFramework.Domain
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        IRepositoryContext Context { get; set; }
        IQueryable<TAggregateRoot> GetAll();
        IQueryable<TAggregateRoot> Get(Expression<Func<TAggregateRoot, bool>> expression);
        IQueryable<TAggregateRoot> Get(int pageNo, int pageSize);
        IQueryable<TAggregateRoot> Get<TKey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, TKey>> keySelector, int pageNo, int pageSize);
        IQueryable<TAggregateRoot> GetByDescending<TKey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, TKey>> keySelector);
        IQueryable<TAggregateRoot> GetByDescending<TKey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, TKey>> keySelector, int pageNo, int pageSize);
        void Add(TAggregateRoot aggregateRoot);
        void AddList(IList<TAggregateRoot> list);
        void Update(TAggregateRoot aggregateRoot);
        void Remove(TAggregateRoot aggregateRoot);
        int Commit();
        TAggregateRoot Create();
        bool Exists(Func<TAggregateRoot, bool> condition);
        int GetCount();
    }
}
