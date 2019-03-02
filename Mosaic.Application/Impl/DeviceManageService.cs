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
    public class DeviceManageService : Service<DeviceManageDataObject, DeviceManage>, IDeviceManageService
    {
        public DeviceManageService(IDeviceManageRepository repository, IMapper mapper) : base(repository, mapper)
        {

        }
        public IList<DeviceManageDataObject> GetListByCompanyID(int  id,string memo)
        {
            IList<DeviceManageDataObject> devList = Mapper.Map<IList<DeviceManage>, IList<DeviceManageDataObject>>(this.repository.Get(p => p.CompanyID == id && p.Memo == memo).ToList());
            CompanyDataObject companyDataObject = Mapper.Map<Company, CompanyDataObject>(this.repository.Context.Get<Company>(p => p.ID == id).FirstOrDefault());
            for (int i = 0; i < devList.Count(); i++)
                devList[i].Company = companyDataObject;
            return devList;
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p =>!p.Deleted&& p.Name == name);
        }
    }
}
