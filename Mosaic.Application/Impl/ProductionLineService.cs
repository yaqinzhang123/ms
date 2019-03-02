using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using DYFramework.ServiceContract;
using Microsoft.EntityFrameworkCore;
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
    public class ProductionLineService : Service<ProductionLineDataObject, ProductionLine>, IProductionLineService
    {
        public ProductionLineService(IProductionLineRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public IList<ProductionLineDataObject> GetProductionLinesByCompanyID(int CompanyID)
        {
            IList < ProductionLineDataObject > LineList= Mapper.Map<IList<ProductionLine>, IList<ProductionLineDataObject>>(this.repository.Get(p => p.Company.ID == CompanyID).ToList());
            foreach(ProductionLineDataObject line in LineList)
            {
                line.Operation= Mapper.Map<Operation, OperationDataObject>(this.repository.Context.Get<Operation>(p => p.ID == line.OperationID).FirstOrDefault());
                if(line.Operation!=null)
                    line.Category= Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == line.Operation.CategoryID).FirstOrDefault());
            }
            return LineList;
        }
        public override ProductionLineDataObject Add(ProductionLineDataObject dataObject)
        {
            ProductionLine line = this.repository.Create();
            line = Mapper.Map(dataObject,line);
            Company company = this.repository.Context.Get<Company>(p => p.ID == dataObject.CompanyID).FirstOrDefault();
            this.repository.Add(line);
            this.repository.Commit();
            line.Company = company;
            return Mapper.Map<ProductionLine, ProductionLineDataObject>(line);
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p => p.Name == name);
        }

        public ProductionLineDataObject UpdateOperation(ProductionLineDataObject dataObject)
        {
            ProductionLine productionLine = this.repository.Get(p => p.ID == dataObject.ID).FirstOrDefault();
            Category waste= this.repository.Context.Get<Category>(p => p.Describe.Contains("废料")).FirstOrDefault();
            Operation operation = this.repository.Context.Get<Operation>(p => !p.Deleted && p.ProductionLineID == dataObject.ID && p.CategoryID != waste.ID).OrderByDescending(p => p.LastUpdateTime).FirstOrDefault();
            if (dataObject.Flag && operation !=null)//正常
            {
                productionLine.OperationID = operation.ID;
                productionLine.Flag = dataObject.Flag;
            }
            else if(dataObject.Flag && operation == null)//第一次没有operation
            {
                productionLine.Flag = dataObject.Flag;
            }
            else if(!dataObject.Flag )//设置废料
            {
                Category categoryWaste = this.repository.Context.Get<Category>(p => p.Describe.Contains("废料")).FirstOrDefault();
                Operation operationWaste = this.repository.Context.Get<Operation>(p => !p.Deleted && p.CategoryID == categoryWaste.ID).FirstOrDefault();
                if (operationWaste != null)
                {
                    productionLine.OperationID = operationWaste.ID;
                    productionLine.Flag = dataObject.Flag;
                }else if (operation != null)
                {
                    Operation newOW = operation;
                    newOW.CategoryID = categoryWaste.ID;
                    newOW.ID = 0;
                    newOW.LastUpdateTime = DateTime.Now;
                    newOW.CreateTime = DateTime.Now;
                    this.repository.Context.Add<Operation>(newOW);
                    this.repository.Context.Commit();
                    productionLine.OperationID = newOW.ID;
                    productionLine.Flag = dataObject.Flag;
                }
                else
                {
                    productionLine.Flag = dataObject.Flag;
                }
            }
            this.repository.Update(productionLine);
            this.repository.Commit();
            ProductionLineDataObject lineDataObject= Mapper.Map<ProductionLine, ProductionLineDataObject>(productionLine);
            lineDataObject.Operation = Mapper.Map<Operation, OperationDataObject>(this.repository.Context.Get<Operation>(p => p.ID == productionLine.OperationID).FirstOrDefault());
            if(lineDataObject.Operation!=null)
                lineDataObject.Category= Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == lineDataObject.Operation.CategoryID).FirstOrDefault());
            return lineDataObject;
        }
    }
}
