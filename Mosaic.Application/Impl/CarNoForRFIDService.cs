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
    public class CarNoForRFIDService : Service<CarNoForRFIDDataObject, CarNoForRFID>, ICarNoForRFIDService
    {
        public CarNoForRFIDService(ICarNoForRFIDRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
