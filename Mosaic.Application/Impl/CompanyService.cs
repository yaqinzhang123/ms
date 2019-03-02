using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using DYFramework.ServiceContract;
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
    public class CompanyService :Service<CompanyDataObject,Company>, ICompanyService
    {
        public CompanyService(ICompanyRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public CompanyDataObject GetCompanyByQRCode(string qrCode)
        {
            int productionLineID = this.repository.Context.Get<QRCode>(p => p.Content == qrCode.Trim()).FirstOrDefault().ProductionLineID;
            int companyID =this.repository.Context.Get<ProductionLine>(p => p.ID == productionLineID).FirstOrDefault().Company.ID;
            return  this.GetByID(companyID);
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p =>!p.Deleted&& p.Name == name);
        }
        public override CompanyDataObject Add(CompanyDataObject dataObject)
        {
            CompanyDataObject company = base.Add(dataObject);
            Role role = this.repository.Context.Get<Role>(p => p.Name == "超级管理员").FirstOrDefault();
            //增加权限
            Rights rights = this.repository.Context.Create<Rights>();
            rights.SoftName = "多工厂云平台";
            rights.RoleID = role.ID;
            rights.FactoryID = company.ID;
            rights.Factory = company.Name;
            this.repository.Context.Add<Rights>(rights);
            this.repository.Context.Commit();
            return company;
        }
        public override int RemoveByID(int id)
        {
            base.RemoveByID(id);
            IList<Role> roleList = this.repository.Context.GetUpdateEntity<Role>().Where(p => p.CompanyID == id).ToList();
            if (roleList.Count() > 0)
            {
                for(int i = 0; i < roleList.Count(); i++)
                {
                    this.repository.Context.Remove<Role>(roleList[i]);
                }
                this.repository.Context.Commit();
            }
            IList<Rights> rightsList = this.repository.Context.GetUpdateEntity<Rights>().Where(p => p.FactoryID == id).ToList();
            if (rightsList.Count() > 0)
            {
                for (int i = 0; i < rightsList.Count(); i++)
                {
                    this.repository.Context.Remove<Rights>(rightsList[i]);
                }
                this.repository.Context.Commit();
            }
            return 1;
        }

        public IList<string> GetShortCode()
        {
            IList<CompanyDataObject> list = this.GetList();
            IList<string> codeList = new List<string>();
            foreach(var company in list)
            {
                codeList.Add(company.ShortCode);
            }
            return codeList;
        }
    }
}
