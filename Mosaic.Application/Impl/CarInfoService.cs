using AutoMapper;
using DYFramework.Application;
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
    public class CarInfoService : Service<CarInfoDataObject, CarInfo>, ICarInfoService
    {
        public CarInfoService(ICarInfoRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        //public IList<CarInfoDataObject> GetListByLineId(int lineId, int v)
        //{
        //    var list = this.repository.Get(p => !p.Flag && p.ProductionLineID == lineId)
        //                            .OrderBy(p => p.Leave).Take(v).ToList();
        //    return this.mapper.Map<IList<CarInfo>,IList<CarInfoDataObject>>(list);

        //}
    }
}
