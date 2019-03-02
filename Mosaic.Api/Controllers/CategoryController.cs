using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Mosaic.Utils.Utils;
using Newtonsoft.Json;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Category/[action]")]
    public class CategoryController : Controller
    {
        private ICategoryService categoryService;
        private readonly IDYLogService dYLogService;

        public ILogger<CategoryController> Logger { get; }

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger,IDYLogService dYLogService)
        {
            this.categoryService = categoryService;
            Logger = logger;
            this.dYLogService = dYLogService;
        }
        [HttpGet]
        public IList<CategoryDataObject> GetCategoryList()
        {
            IList<CategoryDataObject> result = new List<CategoryDataObject>();
            try
            {
                result =this.categoryService.GetList();
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/GetList,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<CategoryDataObject> GetListByCompanyID(int id)
        {
            IList<CategoryDataObject> result = new List<CategoryDataObject>();
            try
            {
                result = this.categoryService.GetListByCompanyID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/GetListByCompanyID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.categoryService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public CategoryDataObject Update([FromBody]CategoryDataObject category)
        {
            CategoryDataObject result = new CategoryDataObject();
            try
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/update,更新，categoryid："+category.ID  });
                result = this.categoryService.Update(category);
            }catch(Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/Get,错误信息：" + e.ToString() });
            }
            return result;
        }

        [HttpOptions("add")]
        public void Option()
        {

        }
        [HttpPost]
        public DTOMessage<CategoryDataObject> Add([FromBody] CategoryDataObject category)
        {
            CategoryDataObject result = new CategoryDataObject();
            if (this.categoryService.Exists(category.MaterialNo))
            {
                return new Msg.DTOMessage<CategoryDataObject>() { Code = 1, Message = "物料编码已存在!" };
            }
            try
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/add,添加物料：" + category.MaterialNo });
                result = this.categoryService.Add(category);
            }catch(Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/add,错误信息：" + e.ToString() });
            }
            return new Msg.DTOMessage<CategoryDataObject>() { Code = 2, Data = result };
        }
        [HttpPost]
        public async Task<DyResult> Import(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            DataTable dt = ExcelHelper.ImportExcel(stream);
            //var str = JsonConvert.SerializeObject(dt);
            int n = await this.categoryService.Import(dt);
            return new DyResult("");
        }

        [HttpGet]
        public int Remove(int id)
        {
            int result = 0;
            try
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/RemoveByID,categoryid：" + id });
                result = this.categoryService.RemoveByID(id);
            }catch(Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/RemoveByID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<CategoryDataObject> Query(string content,int id)
        {
            IList<CategoryDataObject> result = new List<CategoryDataObject>();
            try
            {
                this.Logger.LogInformation("模糊查询");
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/query,查询内容：" + content});
                result = this.categoryService.Query(content, id);
            }catch(Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：Category/Query,错误信息：" + e.ToString() });
            }
            return result;
        }
      
    }
}