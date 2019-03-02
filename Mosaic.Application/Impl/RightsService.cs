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
    public class RightsService: Service<RightsDataObject, Rights>, IRightsService
    {
        private readonly ICompanyService companyService;

        public RightsService(IRightsRepository repository, IMapper mapper,ICompanyService companyService) : base(repository, mapper)
        {
            this.companyService = companyService;
        }
        
        public override RightsDataObject Add(RightsDataObject dataObject)
        {
            int companyID = this.repository.Context.Get<Company>(p => p.Name == dataObject.Factory.Trim()).FirstOrDefault().ID;
            if (!this.Exists(companyID, dataObject.SoftName,dataObject.RoleID)){ 
                Rights rights = this.repository.Create();
                rights = Mapper.Map(dataObject, rights);
                rights.FactoryID = companyID;
                this.repository.Add(rights);
                this.repository.Commit();
                return Mapper.Map<Rights, RightsDataObject>(rights);
            }
            else
            {
                Rights rights = this.repository.Get(p => p.SoftName == dataObject.SoftName && p.FactoryID == companyID).FirstOrDefault();
                return Mapper.Map<Rights, RightsDataObject>(rights);
            }
        }

        public RightsDataObject GetByRoleID(int id)
        {
            return Mapper.Map<Rights, RightsDataObject>(this.repository.Get(p => p.RoleID == id).FirstOrDefault());

        }
        public bool Exists(int factoryID,string softName,int roleID)
        {
            return this.repository.Exists(p => p.FactoryID == factoryID&&p.SoftName==softName&&p.RoleID==roleID);
        }

        public CompanyDataObject RemoveRights(RightsDataObject dataObject)
        {
            IList<RightsDataObject> rights = Mapper.Map<IList<Rights>, IList<RightsDataObject>>(this.repository.Get(p => p.SoftName==dataObject.SoftName&&p.FactoryID==dataObject.FactoryID).ToList());
            CompanyDataObject company = Mapper.Map<Company, CompanyDataObject>(this.repository.Context.GetUpdateEntity<Company>().Where(p => p.ID == dataObject.FactoryID).FirstOrDefault());
            company.SoftList.Remove(dataObject.SoftName);
            if (rights == null)
                return null;
            int l = 0;
            for(int i = 0; i < rights.Count(); i++)
            {
                l = this.RemoveByID(rights[i].ID);
                l++;
            }
            //CompanyDataObject company = Mapper.Map<Company,CompanyDataObject>(this.repository.Context.GetUpdateEntity<Company>().Where(p => p.ID == dataObject.FactoryID).FirstOrDefault());
            company.SoftList.Remove(dataObject.SoftName);
            return this.companyService.Update(company);
        }
    }
}
