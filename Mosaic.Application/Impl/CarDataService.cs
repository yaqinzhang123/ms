using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class CarDataService : Service<CarDataDataObject, CarData>, ICarDataService
    {
        public CarDataService(ICarDataRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public void Add(CarDataTransfer carDataTransfer)
        {
            DateTime enter = DateTime.MinValue.AddTicks(carDataTransfer.Enter);
            DateTime leave = DateTime.MinValue.AddTicks(carDataTransfer.Leave);
            if (this.repository.Exists(p => p.Enter == enter && p.Leave == leave))
            {
                var entity = this.repository.Get(p => p.Enter == enter && p.Leave == leave).FirstOrDefault();
                entity = this.mapper.Map(carDataTransfer, entity);
                this.repository.Update(entity);
            }
            else
            {
                var entity = this.mapper.Map<CarDataTransfer, CarData>(carDataTransfer);
                this.repository.Add(entity);
            }
            this.repository.Commit();
        }
    }
}
