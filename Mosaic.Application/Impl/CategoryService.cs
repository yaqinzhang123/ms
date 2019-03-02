using AutoMapper;
using DYFramework.Application;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.Repositories.Dao;
using Mosaic.ServiceContracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Application.Impl
{
    public class CategoryService : Service<CategoryDataObject, Category>,ICategoryService
    {
        private static Dictionary<string, string> headerProperty = new Dictionary<string, string>
        {
            {"物料编号","MaterialNo" },
            {"物料描述","Describe" }
   
        };

        public CategoryService(ICategoryRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public IList<CategoryDataObject> Query(string content,int id)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return this.GetListByCompanyID(id);
            }
            var query = this.repository.Get(p => p.Describe.Contains(content.Trim()) || p.MaterialNo.Contains(content.Trim())).ToList();
            return Mapper.Map<IList<Category>, IList<CategoryDataObject>>(query);
        }
        public IList<CategoryDataObject> GetListByCompanyID(int id)
        {
            return Mapper.Map<IList<Category>, IList<CategoryDataObject>>(this.repository.Get(p => p.CompanyID == id).ToList());
        }
        public override CategoryDataObject Add(CategoryDataObject dataObject)
        {
            Category category = this.repository.Create();
            category = Mapper.Map(dataObject, category);
            Company company = this.repository.Context.Get<Company>(p => p.ID == dataObject.CompanyID).FirstOrDefault();
            this.repository.Add(category);
            this.repository.Commit();
            category.Company = company;
            return Mapper.Map<Category, CategoryDataObject>(category);
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p =>!p.Deleted&&p.MaterialNo == name);
        }

        public async Task<int> Import(DataTable dt)
        {
            foreach (string header in headerProperty.Keys)
            {
                dt.Columns[header].ColumnName = headerProperty[header];
            }
            JsonSerializerSettings mJsonSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            string str = JsonConvert.SerializeObject(dt, Formatting.None, mJsonSettings);
            JsonSerializer serializer = new JsonSerializer();
           // serializer.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyyMMdd" });
            serializer.NullValueHandling = NullValueHandling.Ignore;
            var list = JArray.Parse(str).ToObject<IList<CategoryDataObject>>(serializer);
            foreach (CategoryDataObject dto in list)
            {
                if (this.Exists(dto.MaterialNo))
                {
                    Category category = this.repository.Get(p => p.MaterialNo == dto.MaterialNo).FirstOrDefault();
                    category = this.mapper.Map(dto, category);
                }
                else
                {
                   this.Add(dto);
                }
            }
            return this.repository.Commit();
        }
    }
}
