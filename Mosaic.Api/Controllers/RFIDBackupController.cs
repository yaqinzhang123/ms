using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/RFIDBackup/[action]")]
    public class RFIDBackupController : Controller
    {
        private IRFIDBackupService rFIDBackupService;
        private IOperationService operationService;

        public RFIDBackupController(IRFIDBackupService rFIDBackupService,IOperationService operationService)
        {
            this.rFIDBackupService = rFIDBackupService;
            this.operationService = operationService;
        }
        [HttpGet]
        public IList<RFIDBackupDataObject> GetRFIDBackupList()
        {

            return this.rFIDBackupService.GetList();
        }
       
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.rFIDBackupService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public RFIDBackupDataObject Update([FromBody] RFIDBackupDataObject rFIDBackup)
        {
            return this.rFIDBackupService.Update(rFIDBackup);
        }
        [HttpPost]
        public RFIDBackupDataObject Add([FromBody]RFIDBackupDataObject rFIDBackup)
        {
            //加入状态判断
            //OperationDataObject operation = this.operationService.GetOperationByProductionLine(rFIDBackupTransfer.ProductionLineID);
            //if (!operation.State)
            //    return "1";
           return this.rFIDBackupService.Add(rFIDBackup);
           
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.rFIDBackupService.RemoveByID(id);
        }
    }
}