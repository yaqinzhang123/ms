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
    public class SoftWareService : Service<SoftWareDataObject, SoftWare>,ISoftWareService
    {
        public SoftWareService(ISoftWareRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
      
      
    }
}
