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
    public class AgencyService : Service<AgencyDataObject, Agency>, IAgencyService
    {
        public AgencyService(IAgencyRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public IList<AgencyDataObject> GetListByCompanyID(int id, string grade)
        {
            return Mapper.Map<IList<Agency>, IList<AgencyDataObject>>(this.repository.Get(p => p.CompanyID == id && p.Grade == grade).ToList());
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p =>!p.Deleted&& p.Name == name);
        }
    }
}
