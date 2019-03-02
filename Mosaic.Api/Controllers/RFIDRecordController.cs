using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Api.Msg;
using Mosaic.Application.Impl;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/RFIDRecord/[action]")]
    public class RFIDRecordController : Controller
    {
        private IRFIDRecordService rFIDRecordService;
        private IOperationService operationService;
        private readonly IMapper mapper;

        public RFIDRecordController(IRFIDRecordService rFIDRecordService,IOperationService operationService,IMapper mapper)
        {
            this.rFIDRecordService = rFIDRecordService;
            this.operationService = operationService;
            this.mapper = mapper;
        }
        [HttpGet]
        public IList<RFIDRecordDataObject> GetRFIDRecordList()
        {

            return this.rFIDRecordService.GetList();
        }
       
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.rFIDRecordService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public RFIDRecordDataObject Update([FromBody]RFIDRecordDataObject rFIDRecord)
        {
            return this.rFIDRecordService.Update(rFIDRecord);
        }
        [HttpPost]
        public string AddList([FromBody]IList<RFIDRecordTransfer> rFIDRecordTransferList)
        {
            if (rFIDRecordTransferList == null || rFIDRecordTransferList.Count <= 0)
                return "0";

            var rfidList = this.mapper.Map<IList<RFIDRecordTransfer>, IList<RFIDRecordDataObject>>(rFIDRecordTransferList);
            int n = this.rFIDRecordService.AddList(rfidList);
            if (n <= 0)
                return "0";
            return "1";

        }
        [HttpPost]
        public string Add([FromBody]RFIDRecordTransfer rFIDRecordTransfer)
        {
            //加入状态判断
            //OperationDataObject operation = this.operationService.GetOperationByProductionLine(rFIDRecordTransfer.ProductionLineID);
            //if (!operation.State)
            //    return "1";
            try
            {
                RFIDRecordDataObject rFIDRecord = new RFIDRecordDataObject();
                rFIDRecord.RFID = rFIDRecordTransfer.RFID;
                rFIDRecord.LineID = rFIDRecordTransfer.ProductionLineID;
                rFIDRecord.Location = rFIDRecordTransfer.Location;
                rFIDRecord.Sync = rFIDRecordTransfer.Sync;
                rFIDRecord.Time = DateTime.MinValue.AddTicks(rFIDRecordTransfer.Time);
                rFIDRecord.Times = rFIDRecordTransfer.Times;
                rFIDRecord.Flag = rFIDRecordTransfer.Flag;
                RFIDRecordDataObject rFID= this.rFIDRecordService.Add(rFIDRecord);
                if(rFID==null)
                    return "0";
                return "1";
            }catch(Exception e)
            {
                return e.ToString();
            }
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.rFIDRecordService.RemoveByID(id);
        }
    }
}