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
    public class DYLogService: Service<DYLogDataObject, DYLog>, IDYLogService
    {
        public DYLogService(IDYLogRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public bool ExistsMemo(string memo)
        {
            return this.repository.Exists(p => !p.Deleted && memo.Equals(p.Memo)&&p.CreateTime>DateTime.Now.AddHours(-1));
        }
    }
}
