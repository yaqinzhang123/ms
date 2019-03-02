using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
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
    public class OperationService : Service<OperationDataObject, Operation>, IOperationService
    {
        public OperationService(IOperationRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        //传参：时间、公司id
        public IList<OperationDataObject> QueryByTime(OperationDataObject dataObject)
        {
             DateTime date = DateTime.Parse(dataObject.LastUpdateTime);
             DateTime endDate = date.AddDays(1);
            var query = this.repository.Get(p=>p.CompanyID==dataObject.CompanyID);
            query = query.Where(p => p.CreateTime >= date && p.CreateTime < endDate);
            return Mapper.Map<IList<Operation>, IList<OperationDataObject>>(query.ToList());
        }
        //分公司查看
        public IList<OperationDataObject> GetListByCompanyID(int id)
        {
            return Mapper.Map<IList<Operation>, IList<OperationDataObject>>(this.repository.Get(p => p.CompanyID == id&&p.CreateTime>DateTime.Now.Date.AddDays(-7)).ToList());
        }
        //更新历史数据，更新二维码

        public  OperationDataObject UpdateOperation(OperationDataObject dataObject)
        {
            OperationDataObject operation = this.GetByID(dataObject.ID);
            operation.BatchNo = dataObject.BatchNo;
            operation.Rule = dataObject.Rule;
            operation.Time = dataObject.Time;
            operation.ReadRate = dataObject.ReadRate;
            OperationDataObject updateOperation = this.Update(operation);
            bool qrFlag = dataObject.Rule == operation.Rule;
            if (!qrFlag)
            {
                IList<QRCode> qRCodes = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => !p.Deleted && p.OperationID == dataObject.ID).ToList();
                foreach (var qr in qRCodes)
                {
                    qr.OperationRule = updateOperation.Rule;
                    this.repository.Context.Update<QRCode>(qr);
                }
                this.repository.Context.Commit();
            }
            return updateOperation;
        }

        public override OperationDataObject Add(OperationDataObject dataObject)
        {
            IList<ProductionLine> lineList = new List<ProductionLine>();
            if (dataObject.ProductionLineID == 0)
            {
                lineList = this.repository.Context.GetUpdateEntity<ProductionLine>().Where(p => p.Company.ID == dataObject.CompanyID).ToList();
            }
            else
            {
                lineList = this.repository.Context.GetUpdateEntity<ProductionLine>().Where(p => p.ID == dataObject.ProductionLineID).ToList();
            }
            Category category = this.repository.Context.Get<Category>(p => p.MaterialNo == dataObject.CategoryID.ToString().Trim()).FirstOrDefault();
            if (category == null || category.ID == 0)
                return new OperationDataObject();
            dataObject.CategoryID = category.ID;
            int lineCount = lineList.Count();
            IList<OperationDataObject> list = new List<OperationDataObject>();
            foreach (var line in lineList)
            {
                dataObject.ProductionLineID = line.ID;
                OperationDataObject operation = base.Add(dataObject);
                line.Flag = true;
                line.OperationID = operation.ID;
                this.repository.Context.Update(line);
                list.Add(operation);
            }           
            int flag=this.repository.Context.Commit();
            var result=list.FirstOrDefault();
            result.Flag = flag == lineCount ? true : false;
            return result;
        }
        public IList<OperationDataObject> GetOperationByCompanyID(int id)
        {
            IList<ProductionLine> productionLineList = this.repository.Context.Get<ProductionLine>(p => p.Company.ID == id).ToList();
            IList<OperationDataObject> operationList = new List<OperationDataObject>();
            for (int i=0;i< productionLineList.Count(); i++)
            {
                OperationDataObject operation = Mapper.Map<Operation,OperationDataObject>( this.repository.Get(p => p.ProductionLineID == productionLineList[i].ID).OrderByDescending(p => p.CreateTime).FirstOrDefault());
                if (operation == null || operation.ID == 0)
                {
                    
                   // operation.ProductionLineID = productionLineList[i].ID;
                    //operationList.Add(operation);
                }
                else
                {
                    CategoryDataObject category = Mapper.Map<Category,CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == operation.CategoryID).FirstOrDefault());
                    operation.Category = category;
                    operationList.Add(operation);
                }
            }
            return operationList;
        }
        public OperationDataObject GetLatest(int id)
        {
            OperationDataObject operation= Mapper.Map<Operation, OperationDataObject>(this.repository.Get(p => !p.Deleted && p.CompanyID == id).OrderByDescending(p => p.CreateTime).FirstOrDefault());
            operation.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == operation.CategoryID).FirstOrDefault());
            return operation;
        }
        //修改分组和品种，更新二维码 
        public OperationDataObject UpdateSum(OperationDataObject dataObject)
        {
            OperationDataObject operation = this.GetByID(dataObject.ID);
            operation.Sum = dataObject.Sum;
            Category category = this.repository.Context.Get<Category>(p => p.MaterialNo == dataObject.CategoryMaterialNo.Trim() && !p.Deleted).FirstOrDefault();
            if (category == null)
                return null;
            bool cFlag = operation.CategoryID == category.ID;
            operation.CategoryID = category.ID;
            OperationDataObject updateOperation = this.Update(operation);
            bool qrFlag = dataObject.Rule == operation.Rule;
            IList<QRCode> qRCodes = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => !p.Deleted && p.OperationID == operation.ID).ToList();
            if (qRCodes == null || qRCodes.Count() == 0)
                return updateOperation;
            if (!cFlag)
            {
                foreach (var qr in qRCodes)
                {
                    qr.CID = updateOperation.CategoryID;
                    qr.GID = 0;
                    this.repository.Context.Update<QRCode>(qr);
                }
            }
            QRCode firstQR = qRCodes.FirstOrDefault();
            firstQR.Lock = false;
            this.repository.Context.Update<QRCode>(firstQR);
            this.repository.Context.Commit();
            return updateOperation;
        }

        public bool UpdateState(OperationDataObject dataObject)
        {
            OperationDataObject operation = this.GetByID(dataObject.ID);
            if (operation.State == dataObject.State)
                return dataObject.State;
            operation.State = dataObject.State;
            OperationDataObject operationData= this.Update(operation);
            return operationData.State;            
        }

        public OperationDataObject GetOperationByProductionLine(int id)
        {
            OperationDataObject operation = Mapper.Map<Operation, OperationDataObject>(this.repository.Get(p => !p.Deleted && p.ProductionLineID == id).OrderByDescending(p => p.LastUpdateTime).FirstOrDefault());
            return operation;
        }

        public IList<OperationDataObject> GetHistoryList(int companyID, int pageNo,out int pageCount)
        {
            //IList<OperationDataObject>  list=Mapper.Map<IList<Operation>, IList<OperationDataObject>>(this.repository.Get(p => !p.Deleted && p.CompanyID == companyID,p=>p.LastUpdateTime, pageNo, 10).ToList());
            IList<OperationDataObject> list = Mapper.Map<IList<Operation>, IList<OperationDataObject>>(this.repository.Get(p => !p.Deleted && p.CompanyID == companyID).ToList());
            pageCount = list.Count();
            var list1=list.OrderByDescending(p=>p.CreateTime).Skip((pageNo - 1) * 10).Take(10).ToList();
            foreach (var op in list1)
            {
                op.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == op.CategoryID).FirstOrDefault());
                op.ProductionLineName = this.repository.Context.Get<ProductionLine>(p => p.ID == op.ProductionLineID).FirstOrDefault().Name;
            }
            return list1;
        }

        public IList<OperationDataObject> Query(OperationDataObject operation,out int pageCount)
        {
            IList<OperationDataObject> list = new List<OperationDataObject>();
            pageCount = 0;
            if (operation.CompanyID == 0)
                return null;
            if (operation.ProductionLineID == 0 && !String.IsNullOrWhiteSpace(operation.CreateTime) && !String.IsNullOrWhiteSpace(operation.LastUpdateTime))
                list = Mapper.Map<IList<Operation>, IList<OperationDataObject>>(this.repository.Get(p => !p.Deleted && p.CompanyID==operation.CompanyID && p.CreateTime > DateTime.Parse(operation.CreateTime) && p.CreateTime < DateTime.Parse(operation.LastUpdateTime)).ToList());
            else if (operation.ProductionLineID != 0 && !String.IsNullOrWhiteSpace(operation.CreateTime) && !String.IsNullOrWhiteSpace(operation.LastUpdateTime))
                list = Mapper.Map<IList<Operation>, IList<OperationDataObject>>(this.repository.Get(p => !p.Deleted && p.ProductionLineID == operation.ProductionLineID && p.CreateTime > DateTime.Parse(operation.CreateTime) && p.CreateTime < DateTime.Parse(operation.LastUpdateTime)).ToList());
            else
                list= Mapper.Map<IList<Operation>, IList<OperationDataObject>>(this.repository.Get(p => !p.Deleted && p.ProductionLineID == operation.ProductionLineID).ToList());
            pageCount = list.Count();
            list = list.OrderBy(p=>p.CreateTime).Skip((operation.PageNo - 1) * 10).Take(10).ToList();
            foreach (var op in list)
            {
                op.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p=>p.ID==op.CategoryID).FirstOrDefault());
                op.ProductionLineName = this.repository.Context.Get<ProductionLine>(p => p.ID == op.ProductionLineID).FirstOrDefault().Name;
            }
            return list;
        }

        public IList<OperationDataObject> QueryState(OperationDataObject operation)
        {

            
            IList<int> lineList = new List<int>();
            IList<OperationDataObject> resultList = new List<OperationDataObject>();
            IList<ProductionLine> productionLines = this.repository.Context.Get<ProductionLine>(p => p.CompanyID == operation.CompanyID).ToList();
            if (operation.ProductionLineID == 0)
            {
                 if (productionLines != null)
                {
                    List<int> lines = productionLines.Select(p => p.ID).ToList();
                    lineList=lines;
                }
            }
            else
            {
                lineList.Add(operation.ProductionLineID);
            }
            Category category = this.repository.Context.Get<Category>(p => !p.Deleted && p.MaterialNo == operation.CategoryID.ToString()).FirstOrDefault();
            if (category != null)
            {
               // var oplist = this.repository.Get(p => !p.Deleted && p.CategoryID == category.ID).OrderByDescending(p => p.CreateTime).GroupBy(p => p.ProductionLineID).ToList();

                foreach (int lineID in lineList)
                {
                    OperationDataObject oplast = Mapper.Map<Operation, OperationDataObject>(this.repository.Get(p => !p.Deleted && p.ProductionLineID == lineID && p.CategoryID == category.ID).OrderByDescending(p => p.CreateTime).FirstOrDefault());
                    if (oplast == null)
                        continue;
                    QRCode qr = this.repository.Context.Get<QRCode>(p => !p.Deleted&&p.OperationID == oplast.ID).OrderByDescending(p => p.Time).FirstOrDefault();
                    if (qr == null)
                        continue;
                    if (qr.EndRoot == false)
                    {
                        int qrsum = this.repository.Context.Get<QRCode>(p => p.GID == qr.GID).Count();
                        if (qrsum == oplast.Sum)
                        {
                            oplast.GroupState = "已封包";
                        }
                        else
                        {
                            oplast.GroupState = "未封包";
                        }
                    }
                    else
                    {
                        oplast.GroupState = "已封包";
                    }
                    oplast.ProductionLineName = productionLines.Where(P => P.ID == oplast.ProductionLineID).FirstOrDefault().Name;
                    resultList.Add(oplast);
                }
            }
            return resultList;
        }

        public bool EndGroup(IList<int> opID)
        {
            bool result = false;
            foreach(int id in opID)
            {
                QRCode qR = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => !p.Deleted && p.OperationID == id).OrderByDescending(p => p.CreateTime).FirstOrDefault();
                if (qR == null)
                    continue;
                qR.EndRoot = true;
                qR.Lock = false;
                this.repository.Context.Update<QRCode>(qR);
            }
            result = this.repository.Context.Commit()>0;
            return result;
        }
    }
}
