using AutoMapper;
using DYFramework.Application;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.Repositories.Dao;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class LocationLogService : Service<LocationLogDataObject, LocationLog>, ILocationLogService
    {
        public LocationLogService(ILocationLogRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public int AddList(IList<LocationLogTransfer> list)
        {
            var dataList = this.mapper.Map<IList<LocationLogTransfer>, IList<LocationLog>>(list);
            this.repository.AddList(dataList);
            int result = this.repository.Commit();
            return result;
        }

        public LocationLogDataObject LastLocation(int lineID, DateTime leave)
        {
            var locationLog = this.repository.Get(p => p.Time < leave && p.ProductionLineID == lineID).FirstOrDefault();
            return this.mapper.Map<LocationLog, LocationLogDataObject>(locationLog);
        }
    }
}
