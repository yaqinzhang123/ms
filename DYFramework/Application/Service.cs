using AutoMapper;
using DYFramework.DataObjects;
using DYFramework.Domain;
using DYFramework.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DYFramework.Application
{
    public class Service<TDataObject, TAggregateRoot> : IService<TDataObject> where TDataObject : DataObject where TAggregateRoot : AggregateRoot
    {
        protected IRepository<TAggregateRoot> repository;
        protected IMapper mapper;

        public Service(IRepository<TAggregateRoot> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public virtual TDataObject Add(TDataObject dataObject)
        {
            TAggregateRoot entity = this.repository.Create();
            entity = mapper.Map(dataObject, entity);
            this.repository.Add(entity);
            this.repository.Commit();
            return mapper.Map<TAggregateRoot, TDataObject>(entity);
        }

        public virtual bool Exists(int id)
        {
            return this.repository.Exists(p => p.ID == id);
        }

        public virtual TDataObject GetByID(int id)
        {
            return this.mapper.Map<TAggregateRoot, TDataObject>(this.repository.Get(p => p.ID == id).FirstOrDefault());
        }

        public virtual int GetCount()
        {
            return this.repository.GetCount();
        }

        public virtual IList<TDataObject> GetList()
        {
            return mapper.Map<IList<TAggregateRoot>, IList<TDataObject>>(this.repository.GetAll().ToList());
        }

        public virtual int RemoveByID(int id)
        {
            TAggregateRoot entity = this.repository.Get(p => p.ID == id).FirstOrDefault();
            if (entity == null)
                return 0;
            this.repository.Remove(entity);
            return this.repository.Commit();
        }

        public virtual TDataObject Update(TDataObject dataObject)
        {
            TAggregateRoot entity = this.repository.Get(p => p.ID == dataObject.ID).FirstOrDefault();
            if (entity == null)
                return dataObject;
            entity = mapper.Map(dataObject, entity);
            this.repository.Update(entity);
            this.repository.Commit();
            return mapper.Map<TAggregateRoot, TDataObject>(entity);
        }
    }
}
