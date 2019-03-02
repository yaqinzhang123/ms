using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Api.Msg;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Group/[action]")]
    public class GroupController : Controller
    {
        private IGroupService groupService;
        private ICarRFIDReceiverService carRFIDReceiverService;

        public GroupController(IGroupService groupService, ICarRFIDReceiverService carRFIDReceiverService)
        {
            this.groupService = groupService;
            this.carRFIDReceiverService = carRFIDReceiverService;
        }
        [HttpGet]
        public IList<GroupDataObject> GetGroupList()
        {
            return this.groupService.GetList();
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.groupService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public GroupDataObject Update(GroupDataObject group)
        {
            return this.groupService.Update(group);
        }
        [HttpPost]
        public bool ManualAddGroup(GroupDataObject group)
        {
            return this.groupService.ManualAddGroup(group);
        }
        [HttpPost]
        public GroupDataObject Add([FromBody]GroupDataObject group)
        {
            return this.groupService.Add(group);
        }
        [HttpGet]
        public DTOMessage<GroupDataObject> GetGroupByQRCode(string qrcode)
        {
            GroupDataObject groupDataObject= this.groupService.GetGroupByQRCode(qrcode);
            if (groupDataObject == new GroupDataObject())
                return new Msg.DTOMessage<GroupDataObject> { Code = 1, Message = "未查到相关分组信息！", Data = groupDataObject };
            return new Msg.DTOMessage<GroupDataObject> { Code = 2,  Data = groupDataObject };
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.groupService.RemoveByID(id);
        }
        [HttpPost]
        public int UpdateRFIDList([FromBody]RFIDCarTransfer rFIDCar)
        {
            CarRFIDReceiverDataObject car = new CarRFIDReceiverDataObject();
            car.EnterTime = DateTime.MinValue.AddTicks(rFIDCar.Enter);
            car.LeaveTime = DateTime.MinValue.AddTicks(rFIDCar.Leave);
            car.RFIDList = rFIDCar.RFID;
            car.ProductionLineID = rFIDCar.Line;
            car.Location = rFIDCar.Location;
            this.carRFIDReceiverService.Add(car);
            return this.groupService.UpdateRFIDList(car);
        }

    }
}