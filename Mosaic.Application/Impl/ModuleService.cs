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
    public class ModuleService : Service<ModuleDataObject, Module>,IModuleService
    {
        public ModuleService(IModuleRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
      
      
    }
}
