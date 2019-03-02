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
    [Route("api/Operation/[action]")]
    public class OperationController : Controller
    {
        private IOperationService operationService;
        private readonly IDYLogService dYLogService;

        public OperationController(IOperationService operationService, IDYLogService dYLogService)
        {
            this.operationService = operationService;
            this.dYLogService = dYLogService;
        }
        [HttpGet]
        public IList<OperationDataObject> GetOperationList()
        {
            return this.operationService.GetList();
        }
        [HttpGet("{id}")]
        IList<OperationDataObject> GetListByCompanyID(int id)
        {
            IList<OperationDataObject> result = new List<OperationDataObject>();
            try
            {
                result = this.operationService.GetListByCompanyID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Operation/GetListByCompanyID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.operationService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public DTOMessage<OperationDataObject> Update([FromBody]OperationDataObject operation)
        {
            OperationDataObject result = new OperationDataObject();
            if (operation == null)
                return new DTOMessage<OperationDataObject>() {Code=1,Message = "提交的全部为空值！", Data = result };
            try
            {
                result= this.operationService.Update(operation);
            }catch(Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：operation/update,错误信息：" + e.ToString() });
            }
            if (result.CategoryID == 0)
                return new DTOMessage<OperationDataObject>() { Code=2,Message = "物料编码不存在！", Data = result };
            return new DTOMessage<OperationDataObject>() {   Code=0,Data = result };

        }
        [HttpPost]
        public DTOMessage<OperationDataObject> UpdateOperation([FromBody]OperationDataObject operation)
        {
            OperationDataObject result = new OperationDataObject();
            if (operation == null)
                return new DTOMessage<OperationDataObject>() { Code = 1, Message = "提交的全部为空值！", Data = result };
            try
            {
                result = this.operationService.UpdateOperation(operation);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：operation/updateoperation,错误信息：" + e.ToString() });
            }
            if (result.CategoryID == 0)
                return new DTOMessage<OperationDataObject>() { Code = 2, Message = "物料编码不存在！", Data = result };
            return new DTOMessage<OperationDataObject>() { Code = 0, Data = result };

        }
        [HttpPost]
        public DTOMessage<OperationDataObject> UpdateSum([FromBody]OperationDataObject operation)
        {
            OperationDataObject result = new OperationDataObject();
            if (operation == null|| operation.UserInfoID==0||operation.Sum==0)
                return new DTOMessage<OperationDataObject>() { Code = 1, Message = "提交的分组数量不正确或人员id为空！", Data = result };
            try
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "修改operationid为" + operation.ID + "的分组数量为"+operation.Sum+"，操作人id：" + operation.UserInfoID + "." });
                result = this.operationService.UpdateSum(operation);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：operation/UpdateSum,错误信息：" + e.ToString() });
            }
            if (result.CategoryID == 0)
                return new DTOMessage<OperationDataObject>() { Code = 2, Message = "物料编码不存在！", Data = result };
            return new DTOMessage<OperationDataObject>() { Code = 0, Data = result };

        }
        [HttpPost]
        public OperationDataObject Add([FromBody]OperationDataObject operation)
        {
            OperationDataObject result = new OperationDataObject();
            if (operation == null)
                return new OperationDataObject();
            try
            {
                result= this.operationService.Add(operation);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：operation/Add,错误信息：" + e.ToString() });
            }
            return result;
        }
        //[HttpPost]
        //public OperationDataObject AddUpdateQRCode([FromBody]OperationDataObject operation)
        //{
        //    OperationDataObject result = new OperationDataObject();
        //    if (operation == null)
        //        return result;
        //    try
        //    {
        //        result = this.operationService.AddUpdateQRCode(operation);
        //    }
        //    catch (Exception e)
        //    {
        //        this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：operation/AddUpdateQRCode,错误信息：" + e.ToString() });
        //    }
        //    return result;
        //}
        [HttpGet]
        public int Remove(int id)
        {
            int result = 0;
            try
            {
                result = this.operationService.RemoveByID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：operation/remove,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<OperationDataObject> GetOperationByCompanyID(int id)
        {

            IList<OperationDataObject> result = new List<OperationDataObject>();
            try
            {
                result =this.operationService.GetOperationByCompanyID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Operation/GetOperationByCompanyID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public OperationDataObject GetLatest(int id)
        {
            OperationDataObject result = new OperationDataObject();
            try
            {
                result = this.operationService.GetLatest(id); 
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Operation/GetLatest,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public bool UpdateState([FromBody]OperationDataObject dataObject)
        {
            bool result = false;
            try
            {
                result = this.operationService.UpdateState(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Operation/UpdateState,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public OperationDataObject GetOperationByProductionLine(int id)
        {
            return this.operationService.GetOperationByProductionLine(id);
        }
        [HttpGet]
        public DTOMessage<IList<OperationDataObject>> GetHistoryList(int companyID,int pageNo)
        {
            pageNo = pageNo == 0 ? 1 : pageNo;
            IList<OperationDataObject>  opList=this.operationService.GetHistoryList(companyID,pageNo, out int pageCount);
            return new DTOMessage<IList<OperationDataObject>>() { PageCount = pageCount, PageNo= pageNo,Data = opList };
        }
        [HttpPost]
        public DTOMessage<IList<OperationDataObject>> Query([FromBody]OperationDataObject operation)
        {
            operation.PageNo = operation.PageNo == 0 ? 1 : operation.PageNo;
            int pageNo = operation.PageNo;
            try
            {
                IList<OperationDataObject> opList = this.operationService.Query(operation, out int pageCount);
                return new DTOMessage<IList<OperationDataObject>>() { PageCount = pageCount, PageNo = pageNo, Data = opList };
            }
            catch(Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Operation/query,错误信息：" + e.ToString() });
                return new DTOMessage<IList<OperationDataObject>>() { Data = null };
            }
            
        }
        [HttpPost]
        public DTOMessage<IList<OperationDataObject>> QueryState([FromBody]OperationDataObject operation)
        {

            IList<OperationDataObject> result = new List<OperationDataObject>();
            try
            {
                result = this.operationService.QueryState(operation);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Operation/query,错误信息：" + e.ToString() });
            }
            return new DTOMessage<IList<OperationDataObject>>() { Data = result };
        }
        [HttpGet]
        public bool EndGroup(string opID)
        {
            bool result = false;
            try
            {
                List<string> IDStrList = opID.Split(',', StringSplitOptions.None).ToList();
                List<int> IDList = new List<int>();
                foreach(string id in IDStrList)
                {
                    int i = Convert.ToInt32(id);
                    IDList.Add(i);
                }
                result= this.operationService.EndGroup(IDList);
            }catch(Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Operation/endgroup,错误信息：" + e.ToString() });
            }
            return result;
        }
    }
}