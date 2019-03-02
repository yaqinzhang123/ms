using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
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
    public class QRCodeService : Service<QRCodeDataObject, QRCode>, IQRCodeService
    {

        public QRCodeService(IQRCodeRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public class Production
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int OperationID { get; set; }
            public int CID { get; set; }
            public double ReadRate { get; set; }//读码率预值
        }
        public int AddList(IList<QRCodeDataObject> list)
        {
            var idList = list.Select(p => p.ID).ToList();
            IList<int> existsID = this.repository.Context.Get<QRCode>(p => idList.Contains(p.VirtualAdd))
                                               .Select(p => p.VirtualAdd).ToList();
            idList = idList.Except(existsID).ToList();//去除已经存在的二维码
            list = list.Where(p => idList.Contains(p.ID)).ToList();
            if (list.Count() == 0)
                return 0;
            IList<int> lineList = list.GroupBy(p => p.ProductionLineID).Select(p => p.Key).ToList();//取列表中的所有产线ID
            Dictionary<int, int> lineDict = this.repository.Context.Get<ProductionLine>(p => lineList.Contains(p.ID))
                                                                     .ToDictionary(p => p.ID, k => k.OperationID);

            Dictionary<int, Operation> operationDict = new Dictionary<int, Operation>();
            foreach (var item in lineDict.Keys)
            {
                Operation operation = this.repository.Context.Get<Operation>(p => p.ID == lineDict[item]).FirstOrDefault();
                operationDict.Add(item, operation);
            }
            foreach (var line in lineList)//按产线处理数据
            {
                Operation op;
                operationDict.TryGetValue(line, out op);//取当前产线设置的品类信息
                IList<QRCodeDataObject> qrList = list.Where(p => p.ProductionLineID == line).ToList();//当前产线的所有二维码
                IList<QRCode> qRList = Mapper.Map<IList<QRCodeDataObject>, IList<QRCode>>(qrList);
                foreach (var qr in qRList)
                {
                    if (op == null)//如果产线没有设置的品类信息，就置为0
                    {
                        QRCode code = this.repository.Get(p => !p.Deleted && p.ProductionLineID == line && p.Time < qr.Time).OrderByDescending(p => p.Time).FirstOrDefault();//找到此二维码相邻的二维码
                        if (code != null)//相邻的二维码不为空
                        {
                            qr.OperationID = code.OperationID;
                            qr.OperationRule = code.OperationRule;
                            qr.CID = code.CID;
                        }
                    }
                    else
                    {
                        if (op.CreateTime > qr.Time && op.ID != 286)//品类设置的时间晚于二维码上传的时间并且排除是废料
                        {
                            QRCode code = this.repository.Get(p => !p.Deleted && p.ProductionLineID == line && p.Time < qr.Time).OrderByDescending(p => p.Time).FirstOrDefault();//找到此二维码相邻的二维码
                            if (code != null)//相邻的二维码不为空
                            {
                                qr.OperationID = code.OperationID;
                                qr.OperationRule = code.OperationRule;
                                qr.CID = code.CID;
                            }
                            else
                            {
                                qr.OperationID = op.ID;
                                qr.OperationRule = op.Rule;
                                qr.CID = op.CategoryID;

                            }
                        }
                        else
                        {
                            qr.OperationID = op.ID;
                            qr.OperationRule = op.Rule;
                            qr.CID = op.CategoryID;
                        }
                    }
                    qr.VirtualAdd = qr.ID;
                    this.repository.Add(qr);//添加
                }
            }
            return this.repository.Commit();
        }
        public override QRCodeDataObject Add(QRCodeDataObject dataObject)
        {
            dataObject.ID = 0;
            dataObject.Content = dataObject.Content.Trim();
            ProductionLine line = this.repository.Context.Get<ProductionLine>(p => p.ID == dataObject.ProductionLineID).FirstOrDefault();
            Operation op = this.repository.Context.Get<Operation>(p => p.ID == line.OperationID).FirstOrDefault();
            if (op == null)
            {
                dataObject.OperationID = 0;
                dataObject.OperationRule = 0;
            }
            else
            {
                if (op.CreateTime > DateTime.MinValue.AddTicks(dataObject.Time) && op.ID != 286)
                {
                    QRCode code = this.repository.Get(p => !p.Deleted && p.ProductionLineID == line.ID && p.Time < DateTime.MinValue.AddTicks(dataObject.Time)).OrderByDescending(p => p.Time).FirstOrDefault();
                    //Operation operation = this.repository.Context.Get<Operation>(p => p.ProductionLineID == line && !p.Deleted&&p.CreateTime< op.CreateTime).OrderByDescending(p=>p.CreateTime).FirstOrDefault();
                    if (code != null)
                    {
                        dataObject.OperationID = code.OperationID;
                        dataObject.OperationRule = code.OperationRule;
                        dataObject.CID = code.CID;
                    }
                    else
                    {
                        dataObject.OperationID = op.ID;
                        dataObject.OperationRule = op.Rule;
                        dataObject.CID = op.CategoryID;

                    }
                }
                else
                {
                    dataObject.OperationID = op.ID;
                    dataObject.OperationRule = op.Rule;
                    dataObject.CID = op.CategoryID;
                }
            }
            // QRCode qrnext = this.repository.Get(p => !p.Deleted && p.ProductionLineID == dataObject.ProductionLineID).OrderByDescending(p => p.Time).FirstOrDefault();
            if (dataObject.Content != "noread")
            {
                QRCodeDataObject contentQR = Mapper.Map<QRCode, QRCodeDataObject>(this.repository.Get(p => !p.Deleted && p.Content == dataObject.Content).FirstOrDefault());
                if (contentQR != null && contentQR.CID == dataObject.CID)
                    return new QRCodeDataObject();
                else if (contentQR != null && contentQR.CID != dataObject.CID)
                {
                    contentQR.CID = dataObject.CID;
                    contentQR.OperationID = dataObject.OperationID;
                    contentQR.OperationRule = dataObject.OperationRule;
                    contentQR.GID = dataObject.GID;
                    contentQR.Lock = false;
                    return base.Update(contentQR);
                }
                else
                {
                    QRCodeDataObject qrcode = base.Add(dataObject);
                    return qrcode;
                }
            }
            else
            {
                QRCodeDataObject qrcode = base.Add(dataObject);
                return qrcode;
            }
        }
        public void AddGroup(int sum, QRCodeDataObject dataObject)
        {
            //产线上设置的分包数量
            //int sum = this.repository.Context.Get<Operation>(p => p.ProductionLineID == dataObject.ProductionLineID).OrderByDescending(p=>p.LastUpdateTime).FirstOrDefault().Sum;
            IList<QRCode> qrcodelist = this.repository.Context.GetUpdateEntity<QRCode>().Where(k => k.ProductionLineID == dataObject.ProductionLineID && k.GID == 0).OrderBy(p => p.Time).Take(sum).ToList();
            Group group = this.repository.Context.Create<Group>();
            group.ProductionLineID = dataObject.ProductionLineID;
            group.Time = DateTime.Now;
            //group.Rule = Int32.Parse(qrcodelist[0].Operation.Rule);
            this.repository.Context.Add<Group>(group);
            this.repository.Context.Commit();
            int groupid = group.ID;
            for (int i = 0; i < qrcodelist.Count(); i++)
            {
                QRCode qRCode = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => p.ID == qrcodelist[i].ID).FirstOrDefault();
                qRCode.GID = groupid;
                this.repository.Update(qRCode);
                this.repository.Commit();
            }

        }
        public void UpdateGID(int GID, int id)
        {
            //把刚刚上传的二维码插到应该的分组中
            QRCode qRCode = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => p.ID == id).FirstOrDefault();
            qRCode.GID = GID;
            qRCode.Lock = true;
            this.repository.Context.Update<QRCode>(qRCode);
            this.repository.Commit();
            //看离此二维码最近的二维码分组之后还有多少个分组
            IList<Group> groupList = this.repository.Context.Get<Group>(p => p.ID > GID && p.ProductionLineID == qRCode.ProductionLineID).OrderBy(p => p.ID).ToList();
            var tempGid = GID;
            foreach (var group in groupList)
            {
                QRCode lastQR = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => p.GID == tempGid).OrderByDescending(p => p.Time).FirstOrDefault();
                tempGid = group.ID;
                lastQR.GID = tempGid;
                this.repository.Context.Update<QRCode>(lastQR);
            }
            QRCode last = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => p.GID == tempGid).OrderByDescending(p => p.Time).FirstOrDefault();
            last.GID = 0;
            last.Lock = false;
            this.repository.Context.Update<QRCode>(last);
            this.repository.Context.Commit();
        }
        public IList<QRCodeDataObject> GetListByProductionID(int id)
        {
            return Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Get(p => p.ProductionLineID == id && !p.Deleted).Take(20).ToList());
        }
        public QRCodeDataObject GetContent(string content)
        {
            QRCode qr = this.repository.Get(p => !p.Deleted && p.Content.Replace("\r", "") == content.Replace("\r", "")).OrderBy(p => p.CreateTime).FirstOrDefault();
            QRCodeDataObject qRCode = Mapper.Map<QRCode, QRCodeDataObject>(qr);
            if (qRCode != null)
            {
                var qrList = this.repository.Get(p => !p.Deleted && p.CID == qRCode.CID && p.ProductionLineID == qRCode.ProductionLineID && p.Time > qr.Time.Date)
                                            .OrderBy(p => p.Time)
                                            .GroupBy(p => p.GID)
                                            .Select(p => p.Key)
                                            .ToList();
                int gidtrue = qrList.IndexOf(qr.GID) + 1;
                qRCode.GNO = gidtrue;
                ProductionLineDataObject productionLine = Mapper.Map<ProductionLine, ProductionLineDataObject>(this.repository.Context.Get<ProductionLine>(p => p.ID == qRCode.ProductionLineID).FirstOrDefault());
                if (productionLine != null)
                    qRCode.ProductionLine = productionLine;
                OperationDataObject operation = Mapper.Map<Operation, OperationDataObject>(this.repository.Context.Get<Operation>(p => p.ID == qRCode.OperationID).FirstOrDefault());
                if (operation != null)
                    qRCode.Operation = operation;
                GroupDataObject group = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == qRCode.GID).FirstOrDefault());
                if (group != null)
                {
                    //qRCode.Group = group;
                    var list = this.repository.Get(p => !p.Deleted && p.GID == group.ID).OrderBy(p => p.Time).ToList();
                    qRCode.GroupSum = list.Count();
                    var index = list.Select((x, i) => new
                    {
                        qRCode = x,
                        i
                    }).First(x => x.qRCode.ID == qRCode.ID).i + 1;
                    qRCode.Index = index;
                }
                CategoryDataObject category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == qRCode.CID).FirstOrDefault());
                if (category != null)
                    qRCode.Category = category;
                CompanyDataObject company = Mapper.Map<Company, CompanyDataObject>(this.repository.Context.Get<Company>(p => p.ID == qRCode.Operation.CompanyID).FirstOrDefault());
                if (company != null)
                    qRCode.Company = company;
                //按组号查询发货单有问题，查5组会把组号包括5的全部查到
                //InvoiceDataObject invoice = Mapper.Map<Invoice, InvoiceDataObject>(this.repository.Context.Get<Invoice>(p => p.GroupNoList.Contains(qRCode.GID.ToString())).FirstOrDefault());
                Invoice invoice = this.repository.Context.Get<Invoice>(p => !p.Deleted && p.Flag && p.GroupNoArray.Where(t => !string.IsNullOrWhiteSpace(t)).Select(k => int.Parse(k)).Contains(qRCode.GID)).FirstOrDefault();
                if (invoice == null)
                    return qRCode;
                InvoiceDataObject sss = Mapper.Map<Invoice, InvoiceDataObject>(invoice);
                sss.InvoiceShipmentList = Mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == invoice.ID).ToList());
                qRCode.Invoice = sss;
            }
            return qRCode;
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p => p.Content == name);
        }

        public IList<QRCodeDataObject> GetListByTime(QRCodeDataObject qrcode)
        {
            if (qrcode == null)
                return null;
            IList<QRCodeDataObject> list = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Get(p => !p.Deleted && p.ProductionLineID == qrcode.ProductionLineID && p.Time > DateTime.Parse(qrcode.CreateTime) && p.Time < DateTime.Parse(qrcode.LastUpdateTime)).ToList());
            foreach (var qr in list)
            {
                qr.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == qr.CID).FirstOrDefault());
            }
            return list;
        }

        public IList<QRCodeDataObject> Query(string qrcode)
        {
            IList<QRCodeDataObject> list = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Get(p => !p.Deleted && p.Content.Contains(qrcode)).ToList());
            foreach (var qr in list)
            {
                qr.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == qr.CID).FirstOrDefault());
            }
            return list;
        }

        public IList<QRCodeDataObject> UpdateList(IList<QRCodeDataObject> qrList)
        {
            IList<QRCodeDataObject> list = new List<QRCodeDataObject>();
            for (int i = 0; i < qrList.Count(); i++)
            {
                QRCodeDataObject qRCode = this.GetByID(qrList[i].ID);
                qRCode.Lock = false;
                qRCode.ManualSkip = qrList[i].ManualSkip;
                qRCode.StartRoot = qrList[i].StartRoot;
                qRCode.EndRoot = qrList[i].EndRoot;
                qRCode.AutoSkip = qrList[i].AutoSkip;
                list.Add(this.Update(qRCode));
            }
            foreach (var qr in list)
            {
                qr.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == qr.CID).FirstOrDefault());
            }
            return list;
        }

        public QRCodeDataObject UpdateSingle(QRCodeDataObject qrcode)
        {
            QRCodeDataObject qRCode = this.GetByID(qrcode.ID);
            qRCode.Lock = false;
            qRCode.ManualSkip = qrcode.ManualSkip;
            qRCode.StartRoot = qrcode.StartRoot;
            qRCode.EndRoot = qrcode.EndRoot;
            qRCode.AutoSkip = qrcode.AutoSkip;
            return this.Update(qRCode);
        }

        public IList<QRCodeDataObject> Unlock(QRCodeDataObject qr)
        {
            //QRCodeDataObject qRCode = this.GetByID(qr.ID);
            //IList<QRCodeDataObject> list = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Get(p => !p.Deleted&&p.ProductionLineID==qr.ProductionLineID&&p.Time >= DateTime.MinValue.AddTicks(qRCode.Time)).ToList());
            //foreach(var )
            return null;
        }

        public IList<QRCodeDataObject> GetQRCodeList()
        {
            IList<QRCodeDataObject> qrList = this.GetList().OrderByDescending(p => p.CreateTime).Take(20).ToList();
            foreach (var qr in qrList)
            {
                qr.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == qr.CID).FirstOrDefault());
            }
            return qrList;
        }

        public QRCodeDataObject UpdateStartRoot(QRCodeDataObject qrcode)
        {
            QRCodeDataObject qRCode = this.GetByID(qrcode.ID);
            qRCode.Lock = false;
            qRCode.StartRoot = qrcode.StartRoot;
            return this.Update(qRCode);
        }

        public QRCodeDataObject UpdateManualSkip(QRCodeDataObject qrcode)
        {
            QRCodeDataObject qRCode = this.GetByID(qrcode.ID);
            qRCode.Lock = false;
            qRCode.ManualSkip = qrcode.ManualSkip;
            return this.Update(qRCode);
        }

        public QRCodeDataObject UpdateVirtualAdd(QRCodeDataObject qrcode)
        {
            QRCodeDataObject qRCode = this.GetByID(qrcode.ID);
            qRCode.Lock = false;
            qRCode.VirtualAdd = qrcode.VirtualAdd;
            return this.Update(qRCode);
        }

        public QRCodeDataObject UpdateEndRoot(QRCodeDataObject qrcode)
        {
            QRCodeDataObject qRCode = this.GetByID(qrcode.ID);
            qRCode.Lock = false;
            qRCode.EndRoot = qrcode.EndRoot;
            return this.Update(qRCode);
        }

        public QRCodeDataObject GetEndQRByProductionLineID(int productionLineID)
        {
            return Mapper.Map<QRCode, QRCodeDataObject>(this.repository.Get(p => p.ProductionLineID == productionLineID).OrderByDescending(p => p.Time).FirstOrDefault());
        }

        public QRCodeDataObject GetByContent(QRCodeDataObject qrcode)
        {
            return Mapper.Map<QRCode, QRCodeDataObject>(this.repository.Get(p => p.Content.Contains(qrcode.Content.Trim())).FirstOrDefault());
        }

        public IList<TotalQRCode> GetTotal(QRCodeDataObject dataObject)
        {
            if (dataObject.CompanyID == 0)
                return null;
            if (string.IsNullOrWhiteSpace(dataObject.CreateTime) || string.IsNullOrWhiteSpace(dataObject.LastUpdateTime))
                return null;
            IList<QRCode> qRList = this.repository.Get(p => p.Time >= DateTime.Parse(dataObject.CreateTime) && p.Time <= DateTime.Parse(dataObject.LastUpdateTime)).ToList();
            if (qRList == null || qRList.Count() == 0)
                return null;
            var productionLine = this.repository.Context.Get<ProductionLine>(p => !p.Deleted && p.CompanyID == dataObject.CompanyID)
                                                       .Select(p => new
                                                       {
                                                           p.ID,
                                                           p.Name
                                                       }).ToList();
            var lineIDList = new List<int>();
            List<TotalQRCode> totalList = new List<TotalQRCode>();
            List<int> cids = qRList.GroupBy(p => p.CID).Select(p => p.Key).ToList();
            var category = this.repository.Context.Get<Category>(p => cids.Contains(p.ID))
                                        .Select(p => new
                                        {
                                            cateid = p.ID,
                                            materialNo = p.MaterialNo,
                                            describe = p.Describe
                                        }).ToList();
            if (dataObject.CompanyID != 0 && dataObject.ProductionLineID == 0)
            {
                lineIDList = productionLine.Select(p => p.ID).ToList();
            }
            else
            {
                lineIDList.Add(dataObject.ProductionLineID);
            }
            if (dataObject.CID != 0)
            {
                qRList = qRList.Where(p => p.CID == dataObject.CID).ToList();
                foreach (var id in lineIDList)
                {
                    var list = qRList.Where(p => p.ProductionLineID == id).OrderBy(p => p.Time).ToList();
                    var gid0List = list.Where(p => p.GID == 0).ToList();
                    var groupIDList = list.Where(p => p.GID != 0).GroupBy(p => p.GID).Select(p => p.Key).ToList();
                    TotalQRCode totalQRCode = new TotalQRCode();
                    totalQRCode.Total = list.Count();
                    if (groupIDList.Count() == 0)
                    {
                        totalQRCode.GroupCountTrue = 0;
                        totalQRCode.RemainderTrue = gid0List.Count();
                    }
                    else
                    {
                        totalQRCode.GroupCountTrue = groupIDList.Count();
                        totalQRCode.RemainderTrue = list.Where(p => p.GID == groupIDList.Last()).Count() + gid0List.Count();
                    }
                    totalQRCode.ProductionLineName = productionLine.Where(p => p.ID == id).Select(p => p.Name).FirstOrDefault();
                    totalQRCode.ProductionLineID = id;
                    totalQRCode.MaterialNo = category.Where(p => p.cateid == dataObject.CID).FirstOrDefault().materialNo;
                    totalQRCode.Describe = category.Where(p => p.cateid == dataObject.CID).FirstOrDefault().describe;
                    totalQRCode.CID = dataObject.CID;
                    totalList.Add(totalQRCode);
                }
                return totalList;
            }
            else
            {
                foreach (var categroyid in cids)
                {
                    qRList = qRList.Where(p => !p.Deleted && p.CID == categroyid).ToList();
                    foreach (var id in lineIDList)
                    {
                        var list = qRList.Where(p => p.ProductionLineID == id).OrderBy(p => p.Time).ToList();
                        var gid0List = list.Where(p => p.GID == 0).ToList();
                        var groupIDList = list.Where(p => p.GID != 0).GroupBy(p => p.GID).Select(p => p.Key).ToList();
                        TotalQRCode totalQRCode = new TotalQRCode();
                        totalQRCode.Total = list.Count();
                        if (groupIDList.Count() == 0)
                        {
                            totalQRCode.GroupCountTrue = 0;
                            totalQRCode.RemainderTrue = gid0List.Count();
                        }
                        else
                        {
                            totalQRCode.GroupCountTrue = groupIDList.Count();
                            totalQRCode.RemainderTrue = list.Where(p => p.GID == groupIDList.Last()).Count() + gid0List.Count();
                        }
                        totalQRCode.ProductionLineName = productionLine.Where(p => p.ID == id).Select(p => p.Name).FirstOrDefault();
                        totalQRCode.ProductionLineID = id;
                        totalQRCode.MaterialNo = category.Where(p => p.cateid == categroyid).FirstOrDefault().materialNo;
                        totalQRCode.Describe = category.Where(p => p.cateid == categroyid).FirstOrDefault().describe;
                        totalQRCode.CID = categroyid;
                        totalList.Add(totalQRCode);
                    }
                }
                return totalList;
            }
        }

        public bool IsThree(string content)
        {
            QRCode qRCode = this.repository.Get(p => p.Content == content).FirstOrDefault();
            if (qRCode == null)
                return false;
            return true;
        }
        public List<ReadRateQRCode> GetReadRate(QRCodeDataObject dataObject)
        {
            if (dataObject.CompanyID == 0 || dataObject.CreateTime == null || dataObject.LastUpdateTime == null)
                return null;
            var productionLine = this.repository.Context.Get<ProductionLine>(p => !p.Deleted && p.CompanyID == dataObject.CompanyID)
                                                        .Select(p => new
                                                        {
                                                            p.ID,
                                                            p.Name
                                                        }).ToList();
            IList<QRCode> qRList = this.repository.Get(p => p.Time > DateTime.Parse(dataObject.CreateTime) && p.Time < DateTime.Parse(dataObject.LastUpdateTime)).ToList();
            if (qRList == null || qRList.Count() == 0)
                return null;
            List<int> lineList = new List<int>();
            List<int> cidList = new List<int>();
            List<ReadRateQRCode> readRateList = new List<ReadRateQRCode>();
            if (dataObject.CompanyID != 0 && dataObject.ProductionLineID == 0)
            {
                lineList = productionLine.Select(p => p.ID).ToList();
            }
            if (dataObject.ProductionLineID != 0)
            {
                lineList.Add(dataObject.ProductionLineID);
            }
            qRList = qRList.Where(p => lineList.Contains(p.ProductionLineID)).ToList();
            List<int> cids = qRList.GroupBy(p => p.CID).Select(p => p.Key).ToList();
            var category = this.repository.Context.Get<Category>(p => cids.Contains(p.ID))
                                        .Select(p => new
                                        {
                                            cateid = p.ID,
                                            materialNo = p.MaterialNo,
                                            describe = p.Describe
                                        }).ToList();
            if (dataObject.CID != 0)
            {
                qRList = qRList.Where(p => !p.Deleted && p.CID == dataObject.CID).ToList();
                foreach (var id in lineList)
                {
                    var list = qRList.Where(p => p.ProductionLineID == id).ToList();
                    ReadRateQRCode readRate = new ReadRateQRCode();
                    var falseList = list.Where(p => p.Content == "noread").ToList();
                    var trueList = list.Where(p => p.Content != "noread").ToList();
                    readRate.QRCodeCount = list.Count();
                    readRate.FalseQRCodeCount = falseList.Count();
                    readRate.TrueQRCodeCount = trueList.Count();
                    readRate.ProductionLineID = id;
                    readRate.ProductionLineName = productionLine.Where(p => p.ID == id).Select(p => p.Name).FirstOrDefault();
                    readRate.ReadRate = list.Count() == 0 ? "0%" : (Math.Truncate(((trueList.Count() * 1.0 / list.Count()) * 100) * 100) / 100) + "%";
                    readRate.MaterialNo = category.Where(p => p.cateid == dataObject.CID).FirstOrDefault().materialNo;
                    readRate.Describe = category.Where(p => p.cateid == dataObject.CID).FirstOrDefault().describe;
                    readRate.CID = dataObject.CID;
                    readRateList.Add(readRate);
                }
                return readRateList;
            }
            else
            {
                foreach (var categroyid in cids)
                {
                    var cateList = qRList.Where(p => !p.Deleted && p.CID == categroyid).ToList();
                    if (cateList != null)
                    {
                        foreach (var id in lineList)
                        {
                            var list = cateList.Where(p => p.ProductionLineID == id).ToList();
                            ReadRateQRCode readRate = new ReadRateQRCode();
                            var falseList = list.Where(p => p.Content == "noread").ToList();
                            var trueList = list.Where(p => p.Content != "noread").ToList();
                            readRate.QRCodeCount = list.Count();
                            readRate.FalseQRCodeCount = falseList.Count();
                            readRate.TrueQRCodeCount = trueList.Count();
                            readRate.ProductionLineName = productionLine.Where(p => p.ID == id).Select(p => p.Name).FirstOrDefault();
                            readRate.ProductionLineID = id;
                            readRate.ReadRate = list.Count() == 0 ? "0%" : (Math.Truncate(((trueList.Count() * 1.0 / list.Count()) * 100) * 100) / 100) + "%";
                            readRate.MaterialNo = category.Where(p => p.cateid == categroyid).FirstOrDefault().materialNo;
                            readRate.Describe = category.Where(p => p.cateid == dataObject.CID).FirstOrDefault().describe;
                            readRate.CID = categroyid;
                            readRateList.Add(readRate);
                        }
                    }

                }
                return readRateList;
            }
        }

        public IList<CategoryDataObject> GetCategoryByTime(QRCodeDataObject dataObject)
        {
            if (string.IsNullOrWhiteSpace(dataObject.CreateTime))
                return null;
            if (string.IsNullOrWhiteSpace(dataObject.LastUpdateTime))
                dataObject.LastUpdateTime = DateTime.Parse(dataObject.CreateTime).AddDays(1).Date.ToString();
            var lineIDList = this.repository.Context.Get<ProductionLine>(p => !p.Deleted && p.CompanyID == dataObject.CompanyID).Select(p => p.ID).ToList();
            IList<QRCode> qRList = this.repository.Get(p => !p.Deleted && p.Time > DateTime.Parse(dataObject.CreateTime)
                                                        && p.Time < DateTime.Parse(dataObject.LastUpdateTime) && lineIDList.Contains(p.ProductionLineID))
                                                        .OrderBy(p => p.Time).ToList();
            if (qRList == null || qRList.Count() == 0)
                return null;
            var cate = qRList.GroupBy(p => p.CID).Select(p => p.Key).ToList();
            IList<CategoryDataObject> result = this.repository.Context.Get<Category>(p => !p.Deleted && cate.Contains(p.ID))
                                                            .Select(p => new CategoryDataObject()
                                                            {
                                                                ID = p.ID,
                                                                MaterialNo = p.MaterialNo
                                                            }).ToList();
            return result;
        }
        public List<ReadRateQRCode> GetRateAndTotal(QRCodeDataObject dataObject)
        {
            if (dataObject.CompanyID == 0)
                return null;
            var productionLine = this.repository.Context.Get<ProductionLine>(p => !p.Deleted && p.CompanyID == dataObject.CompanyID)
                                                        .Select(p => new Production
                                                        {
                                                            ID = p.ID,
                                                            Name = p.Name,
                                                            OperationID = p.OperationID,
                                                            CID = 0,
                                                            ReadRate = 0.00
                                                        }).ToList();
            IList<Operation> opList = new List<Operation>();
            foreach (var production in productionLine)
            {
                Operation op = this.repository.Context.Get<Operation>(p => p.ID == production.OperationID).FirstOrDefault();
                opList.Add(op);
                production.CID = op.CategoryID;
                production.ReadRate = op.ReadRate;
            }
            IList<int> cids = productionLine.Select(p => p.CID).ToList();
            List<int> lineList = productionLine.Select(p => p.ID).ToList();
            IList<QRCode> qRList = this.repository.Get(p => p.Time > DateTime.Now.Date && p.Time < DateTime.Now && lineList.Contains(p.ProductionLineID)).ToList();
            if (qRList == null || qRList.Count() == 0)
                return null;
            List<ReadRateQRCode> readRateList = new List<ReadRateQRCode>();
            var category = this.repository.Context.Get<Category>(p => cids.Contains(p.ID))
                                        .Select(p => new
                                        {
                                            cateid = p.ID,
                                            materialNo = p.MaterialNo,
                                            describe = p.Describe
                                        }).ToList();
            foreach (var id in lineList)
            {
                var pro = productionLine.Where(p => p.ID == id).FirstOrDefault();
                int cid = pro.CID;
                double rate = pro.ReadRate;
                var list = qRList.Where(p => !p.Deleted && p.CID == cid && p.ProductionLineID == id).OrderBy(p => p.Time).ToList();
                var gid0List = list.Where(p => p.GID == 0).ToList();
                var groupIDList = list.Where(p => p.GID != 0).GroupBy(p => p.GID).Select(p => p.Key).ToList();
                ReadRateQRCode readRate = new ReadRateQRCode();
                var falseList = list.Where(p => p.Content == "noread").ToList();
                var trueList = list.Where(p => p.Content != "noread").ToList();
                readRate.QRCodeCount = list.Count();
                readRate.FalseQRCodeCount = falseList.Count();
                readRate.TrueQRCodeCount = trueList.Count();
                readRate.ProductionLineID = id;
                readRate.ProductionLineName = productionLine.Where(p => p.ID == id).Select(p => p.Name).FirstOrDefault();
                readRate.MaterialNo = category.Where(p => p.cateid == cid).FirstOrDefault().materialNo;
                readRate.Describe = category.Where(p => p.cateid == cid).FirstOrDefault().describe;
                readRate.CID = cid;
                if (groupIDList.Count() == 0)
                {
                    readRate.GroupCountTrue = 0;
                    readRate.RemainderTrue = gid0List.Count();
                }
                else
                {
                    readRate.GroupCountTrue = groupIDList.Count();
                    readRate.RemainderTrue = list.Where(p => p.GID == groupIDList.Last()).Count() + gid0List.Count();
                }
                var iList = list.Where(p => p.Time > DateTime.Now.AddMinutes(-10)).OrderBy(p => p.Time).ToList();
                var itrueList = iList.Where(p => p.Content != "noread").ToList();
                double a = 0;
                if (list.Count() == 0)
                {
                    readRate.ReadRate = "0%";
                    readRate.RateFlag = true;
                    readRate.InstantReadRate = "0%";
                    readRate.RateFlag1 = true;
                }
                else
                {
                    a = Math.Truncate(((trueList.Count() * 1.0 / list.Count()) * 100) * 100) / 100;
                    readRate.ReadRate = a.ToString("N") + "%";
                    readRate.RateFlag = a < rate ? true : false;
                    if (iList.Count() == 0)
                    {
                        readRate.InstantReadRate = "0%";
                        readRate.RateFlag1 = true;
                    }
                    else
                    {
                        double b = Math.Truncate(((itrueList.Count() * 1.0 / iList.Count()) * 100) * 100) / 100;
                        readRate.InstantReadRate = b.ToString("N") + "%";
                        readRate.RateFlag1 = b < rate ? true : false;
                    }
                }
                readRateList.Add(readRate);
            }
            return readRateList;

        }

        public IList<ReadRateQRCode> TotalDate(QRCodeDataObject dataObject)
        {
            if (dataObject.CompanyID == 0)//公司id为0，退出
                return null;
            var productionLine = this.repository.Context.Get<ProductionLine>(p => !p.Deleted && p.CompanyID == dataObject.CompanyID)
                                                        .Select(p => new
                                                        {
                                                            p.ID,
                                                            p.Name
                                                        }).ToList();
            IList<QRCode> qRList = new List<QRCode>();
            if (dataObject.CreateTime == null)
            {
                //拿到此公司id下的所有产线及名称
                qRList = this.repository.Get(p => p.Time > DateTime.Now.Date && p.Time < DateTime.Now).ToList();//查询今天生产的所有二维码
            }
            else
            {
                qRList = this.repository.Get(p => p.Time >DateTime.Parse(dataObject.CreateTime).Date && p.Time < DateTime.Parse(dataObject.CreateTime).AddDays(1).Date).ToList();//查询今天生产的所有二维码
            }
            if (qRList == null || qRList.Count() == 0)//若为空，则退出
                return null;
            IList<int> cids = qRList.GroupBy(p => p.CID).Select(p => p.Key).ToList();
            List<int> lineList = new List<int>();
            if (dataObject.CompanyID != 0 && dataObject.ProductionLineID == 0)//若无筛选产线，则拿到所有
            {
                lineList = productionLine.Select(p => p.ID).ToList();
            }
            if (dataObject.ProductionLineID != 0)//若有产线信息，则传入产线信息
            {
                lineList.Add(dataObject.ProductionLineID);
            }
            List<ReadRateQRCode> readRateList = new List<ReadRateQRCode>();
            var category = this.repository.Context.Get<Category>(p => cids.Contains(p.ID))
                                        .Select(p => new
                                        {
                                            cateid = p.ID,
                                            materialNo = p.MaterialNo,
                                            describe = p.Describe
                                        }).ToList();
            if (dataObject.CID != 0)
            {
                qRList = qRList.Where(p => !p.Deleted && p.CID == dataObject.CID).ToList();
                foreach (var id in lineList)
                {
                    var list = qRList.Where(p => p.ProductionLineID == id).OrderBy(p => p.Time).ToList();
                    var gid0List = list.Where(p => p.GID == 0).ToList();//获得未分组的二维码数组
                    var groupIDList = list.Where(p => p.GID != 0).GroupBy(p => p.GID).Select(p => p.Key).ToList();//已分组的gid
                    ReadRateQRCode readRate = new ReadRateQRCode();
                    var falseList = list.Where(p => p.Content == "noread").ToList();//失败二维码
                    var trueList = list.Where(p => p.Content != "noread").ToList();//成功二维码
                    readRate.QRCodeCount = list.Count();//总数
                    readRate.FalseQRCodeCount = falseList.Count();//失败数量
                    readRate.TrueQRCodeCount = trueList.Count();//成功数量
                    readRate.ProductionLineID = id;//产线
                    readRate.ProductionLineName = productionLine.Where(p => p.ID == id).Select(p => p.Name).FirstOrDefault();//产线名
                    readRate.MaterialNo = category.Where(p => p.cateid == dataObject.CID).FirstOrDefault().materialNo;//物料码
                    readRate.Describe = category.Where(p => p.cateid == dataObject.CID).FirstOrDefault().describe;//物料描述
                    readRate.CID = dataObject.CID;
                    if (groupIDList.Count() == 0)//没有已分组的gid
                    {
                        readRate.GroupCountTrue = 0;//分组兜数0
                        readRate.RemainderTrue = gid0List.Count();//尾数
                    }
                    else
                    {
                        readRate.GroupCountTrue = groupIDList.Count;//分组兜数已经分好组的兜数
                        readRate.RemainderTrue = list.Where(p => p.GID == groupIDList.Last()).Count() + gid0List.Count();//尾数最后一兜的数量加上未分组的
                    }
                    double a = 0;
                    if (list.Count() == 0)
                        readRate.ReadRate = "0%";
                    else
                    {
                        a = Math.Truncate(((trueList.Count() * 1.0 / list.Count()) * 100) * 100) / 100;
                        readRate.ReadRate = a.ToString("N") + "%";
                    }
                    readRateList.Add(readRate);
                }
                return readRateList;
            }
            else
            {
                foreach (var categroyid in cids)
                {
                    var cateList = qRList.Where(p => !p.Deleted && p.CID == categroyid).ToList();
                    if (cateList != null)
                    {
                        foreach (var id in lineList)
                        {
                            var list = cateList.Where(p => p.ProductionLineID == id).OrderBy(p => p.Time).ToList();
                            var gid0List = list.Where(p => p.GID == 0).ToList();//获得未分组的二维码数组
                            var groupIDList = list.Where(p => p.GID != 0).GroupBy(p => p.GID).Select(p => p.Key).ToList();//已分组的gid
                            ReadRateQRCode readRate = new ReadRateQRCode();
                            var falseList = list.Where(p => p.Content == "noread").ToList();//失败二维码
                            var trueList = list.Where(p => p.Content != "noread").ToList();//成功二维码
                            readRate.QRCodeCount = list.Count();//总数
                            readRate.FalseQRCodeCount = falseList.Count();//失败数量
                            readRate.TrueQRCodeCount = trueList.Count();//成功数量
                            readRate.ProductionLineID = id;//产线
                            readRate.ProductionLineName = productionLine.Where(p => p.ID == id).Select(p => p.Name).FirstOrDefault();//产线名
                            readRate.MaterialNo = category.Where(p => p.cateid == categroyid).FirstOrDefault().materialNo;//物料码
                            readRate.Describe = category.Where(p => p.cateid == categroyid).FirstOrDefault().describe;//物料描述
                            readRate.CID = categroyid;
                            if (groupIDList.Count() == 0)//没有已分组的gid
                            {
                                readRate.GroupCountTrue = 0;//分组兜数0
                                readRate.RemainderTrue = gid0List.Count();//尾数
                            }
                            else
                            {
                                readRate.GroupCountTrue = groupIDList.Count;//分组兜数已经分好组的兜数
                                readRate.RemainderTrue = list.Where(p => p.GID == groupIDList.Last()).Count() + gid0List.Count();//尾数最后一兜的数量加上未分组的
                            }
                            double a = 0;
                            if (list.Count() == 0)
                                readRate.ReadRate = "0%";
                            else
                            {
                                a = Math.Truncate(((trueList.Count() * 1.0 / list.Count()) * 100) * 100) / 100;
                                readRate.ReadRate = a.ToString("N") + "%";
                            }
                            readRateList.Add(readRate);
                        }
                    }
                }
                return readRateList;
            }
        }
    }
}

