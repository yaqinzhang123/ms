using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mosaic.Api.Msg;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/DeviceManage/[action]")]
    public class DeviceManageController : Controller
    {
        private IDeviceManageService deviceManageService;

        public ILogger<DeviceManageController> Logger { get; }

        public DeviceManageController(IDeviceManageService deviceManageService,ILogger<DeviceManageController> logger)
        {
            this.deviceManageService = deviceManageService;
            Logger = logger;
        }
        [HttpGet]
        public IList<DeviceManageDataObject> GetDeviceManageList()
        {

            return this.deviceManageService.GetList();
        }
        [HttpGet]
        public IList<DeviceManageDataObject> GetListByCompanyID(int id,string memo)
        {
            return this.deviceManageService.GetListByCompanyID(id,memo);
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.deviceManageService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public DeviceManageDataObject Update([FromBody]DeviceManageDataObject deviceManage)
        {
            return this.deviceManageService.Update(deviceManage);
        }
        [HttpPost]
        public DTOMessage<DeviceManageDataObject> Add([FromBody]DeviceManageDataObject deviceManage)
        {
            if (this.deviceManageService.Exists(deviceManage.Name))
            {
                return new Msg.DTOMessage<DeviceManageDataObject>() { Code = 1, Message = "设备名称已存在!" };
            }
            DeviceManageDataObject deviceManageDataObject = deviceManageService.Add(deviceManage);
            return new Msg.DTOMessage<DeviceManageDataObject>() { Code = 2, Data = deviceManageDataObject };
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.deviceManageService.RemoveByID(id);
        }
    }
}