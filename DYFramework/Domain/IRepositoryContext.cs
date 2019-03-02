using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DYFramework.Domain
{
    public interface IRepositoryContext
    {
        IQueryable<TAggregateRoot> GetReadEntity<TAggregateRoot>() where TAggregateRoot : AggregateRoot;
        DbSet<TAggregateRoot> GetUpdateEntity<TAggregateRoot>() where TAggregateRoot : AggregateRoot;
        IQueryable<TAggregateRoot> GetAll<TAggregateRoot>() where TAggregateRoot : AggregateRoot;
        IQueryable<TAggregateRoot> Get<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> expression) where TAggregateRoot : AggregateRoot;
        IQueryable<TAggregateRoot> Get<TAggregateRoot>(int pageNo, int pageSize) where TAggregateRoot : AggregateRoot;
        IQueryable<TAggregateRoot> Get<TAggregateRoot, Tkey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, Tkey>> keySelector, int pageNo, int pageSize) where TAggregateRoot : AggregateRoot;
        IQueryable<TAggregateRoot> GetByDescending<TAggregateRoot, Tkey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, Tkey>> keySelector) where TAggregateRoot : AggregateRoot;
        IQueryable<TAggregateRoot> GetByDescending<TAggregateRoot, Tkey>(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, Tkey>> keySelector, int pageNo, int pageSize) where TAggregateRoot : AggregateRoot;
        TAggregateRoot Create<TAggregateRoot>() where TAggregateRoot : AggregateRoot, new();       
        void Add<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot;
        void Update<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot;
        void Remove<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot;
        int Commit();
        void AddList<TAggregateRoot>(IList<TAggregateRoot> list) where TAggregateRoot : AggregateRoot, new();
    }
}
