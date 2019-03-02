using AutoMapper;
using DYFramework.Application;
using Mosaic.Domain;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using Mosaic.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class InvoiceService : Service<InvoiceDataObject, Invoice>, IInvoiceService
    {
        private readonly IInvoiceUserInfoRepository invoiceUserInfoRepository;

        public InvoiceService(IInvoiceRepository repository,IInvoiceUserInfoRepository invoiceUserInfoRepository, IMapper mapper) : base(repository, mapper)
        {
            this.invoiceUserInfoRepository = invoiceUserInfoRepository;
            this.repository = repository;
        }
        public  class GroupInt{

            public int Gid { get; set; }
            public int CID { get; set; }
            public float Quantity { get; set; }
            public int UserInfoID { get; set; }

        }
        //记录扫描二维码，更新
        public InvoiceDataObject UpdateQRCode(InvoiceDataObject dataObject,out string message)
        {
            InvoiceDataObject invoice = this.Get(dataObject.ID);
            message = "";
            string codeList = dataObject.CodeList[0].Trim();
            QRCode qR = this.repository.Context
                              .Get<QRCode>(p => !p.Deleted && codeList.Contains(p.Content.Trim())).FirstOrDefault();
            if (qR == null)
            {
                message = "此二维码不存在！";
                return invoice;
            }
            if (invoice.GroupNoList.Contains(qR.GID.ToString()))
            {
                message = "网兜重复扫描！";
                return invoice;
            }
            bool trueFlag = this.invoiceUserInfoRepository.Exists(p => p.GroupNoArray.Contains(qR.GID.ToString()));
            if (trueFlag)
            {
                message = "网兜已被扫描！";
                return invoice;
            }
            var shipmentList= invoice.InvoiceShipmentList.Select(p => new { p.MaterialNo, p.ID,p.Flag }).ToList();
            var materialNoList = shipmentList.Select(p => p.MaterialNo).ToList();
            if (materialNoList == null || materialNoList.Count() <= 0 || dataObject.CodeList == null || dataObject.CodeList.Count() == 0)
            {
                message = "此交货单没有装运物料！";
                return invoice;
            }
            var CategoryList = this.repository.Context.Get<Category>(p => materialNoList.Contains(p.MaterialNo)).Select(p => new { p.ID, p.MaterialNo }).ToList();
            IList<int> CIDList = CategoryList.Select(p => p.ID).ToList();
            int gidSum = this.repository.Context.Get<QRCode>(p => !p.Deleted && p.GID == qR.GID && p.CID == qR.CID).Count();
            IList<string> groupNoList = CIDList.Contains(qR.CID) ? qR.GID.ToString().Split(""):null;
            if (groupNoList != null)
            {
                var materialno = CategoryList.Where(p => p.ID == qR.CID).FirstOrDefault().MaterialNo;
                var shipment = shipmentList.Where(p => p.MaterialNo == materialno && !p.Flag).FirstOrDefault();
                if (shipment == null)
                {
                    message = "该品类已经发运完成，请勿多扫！";
                    return invoice;
                }
                var shipmentid = shipment.ID;
                InvoiceShipment invoiceShipment = this.repository.Context.GetUpdateEntity<InvoiceShipment>()
                                                .Where(p => p.ID == shipmentid).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(invoiceShipment.GroupNoList))
                {
                    invoiceShipment.GroupNoList = string.Join(",", groupNoList);
                }
                else
                {
                    var list = invoiceShipment.GroupNoList.Split(",", StringSplitOptions.None).ToList();
                    list = list.Union(groupNoList).Distinct().ToList();
                    invoiceShipment.GroupNoList = string.Join(",", list);
                }
                invoiceShipment.CID = qR.CID;
                invoiceShipment.QRRule = qR.OperationRule;
                invoiceShipment.GroupSum = invoiceShipment.GroupSum + gidSum;
                this.repository.Context.Update<InvoiceShipment>(invoiceShipment);
                InvoiceShipmentDataObject invoiceShip = mapper.Map<InvoiceShipmentDataObject>(invoiceShipment);
                if (invoiceShip.GroupQuantitySum >= invoiceShip.Quantity)
                {
                    message = "该品类已经发运完成！";
                }
                invoice.LastGroupNoList = invoice.GroupNoList;//上一次分组
                invoice.GroupNoList = invoice.GroupNoList.Union(groupNoList).Distinct().ToList();//相加去重之后
                invoice.LastNo = invoice.GroupNoList == null || invoice.GroupNoList.Count() > 0 ? invoice.GroupNoList.Last() : null;//最后一个组号
                //查找invoiceUserInfo,添加
                InvoiceUserInfoDataObject invoiceUserInfo = new InvoiceUserInfoDataObject()
                {
                    InvoiceID = dataObject.ID,
                    UserInfoID = dataObject.UserInfoID,
                    CodeList = dataObject.CodeList,
                    GroupNoList = groupNoList
                };
               // bool dd = this.UserInvoiceAdd(invoiceUserInfo);
                Group group = this.repository.Context.GetUpdateEntity<Group>().Where(p => p.ID == qR.GID).FirstOrDefault();
                group.CID = qR.CID;
                this.repository.Context.Update<Group>(group);
                this.repository.Context.Commit();
                if (string.IsNullOrWhiteSpace(message))
                    message = "网兜扫描成功";
            }
            else
            {
                message = "此网兜所属物料与交货单的装运项目不符！";
            }
            invoice.CodeList = invoice.CodeList == null ? null : invoice.CodeList.Union(dataObject.CodeList).Distinct().ToList() ;
            invoice.SubmitTime = DateTime.Now.ToString();
            invoice.InvoiceShipmentList = null;
            var result= this.Update(invoice);
         //   result.InvoiceUserInfoList = Mapper.Map<IList<InvoiceUserInfo>, IList<InvoiceUserInfoDataObject>>(this.repository.Context.Get<InvoiceUserInfo>(p => p.InvoiceID == result.ID).ToList());
            return result;
        }
        public bool UserInvoiceAdd(InvoiceUserInfoDataObject invoiceUser)
        {
            var invoiceUserInfo = this.repository.Context.GetUpdateEntity<InvoiceUserInfo>().Where(p =>!p.Deleted&& p.InvoiceID == invoiceUser.InvoiceID && p.UserInfoID == invoiceUser.UserInfoID).FirstOrDefault();
            if (invoiceUserInfo != null)
            {
                IList<string> list = new List<string>();
                List<string> qrlist = new List<string>();
                if (invoiceUserInfo.GroupNoList==null|| invoiceUserInfo.GroupNoList.Count()==0)
                    list = invoiceUser.GroupNoList;
                else
                {
                    list = invoiceUserInfo.GroupNoList.Split(",").Union(invoiceUser.GroupNoList).ToList();
                }
                invoiceUserInfo.GroupNoList = string.Join(",", list.Distinct());
                if (invoiceUser.CodeList == null || invoiceUser.CodeList.Count() == 0)
                    qrlist = invoiceUserInfo.CodeList.Split(",").ToList();
                qrlist = invoiceUserInfo.CodeList.Split(",").Union(invoiceUser.CodeList).Distinct().ToList();
                qrlist.RemoveAll(p => string.IsNullOrWhiteSpace(p));
                invoiceUserInfo.CodeList = qrlist.Count() == 0 || qrlist == null ? null : string.Join(",", qrlist);
                this.repository.Context.Update<InvoiceUserInfo>(invoiceUserInfo);
            }
            else
            {
                InvoiceUserInfo result = Mapper.Map<InvoiceUserInfo>(invoiceUser);
                this.repository.Context.Add<InvoiceUserInfo>(result);
            }
            return this.repository.Context.Commit() > 0;
        }
            //记录扫描RFID，更新
            public InvoiceDataObject UpdateRFIDCode(InvoiceDataObject dataObject)
        {
            Invoice invoice = this.repository.Context.GetUpdateEntity<Invoice>().Where(p => p.ID == dataObject.ID).FirstOrDefault();
            var materialNoList = this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == dataObject.ID).Select(p => p.MaterialNo).ToList();
            if (materialNoList == null || materialNoList.Count() <= 0 || dataObject.CodeList == null)
                return new InvoiceDataObject();
            List<int> CIDList = this.repository.Context.Get<Category>(p => materialNoList.Contains(p.MaterialNo)).Select(p => p.ID).ToList();
            IList<string> errRFIDList = new List<string>();
            IList<string> errGroupNoList = new List<string>();
            IList<GroupDataObject> errgroupList = new List<GroupDataObject>();
            IList<GroupDataObject> grouplist = new List<GroupDataObject>();
            IList<string> rfidList = new List<string>();
            if (invoice.CodeList != null)
                rfidList = dataObject.CodeList.Union(invoice.CodeList.Split(",")).ToList();
            else
                rfidList = dataObject.CodeList;
            var groupList = this.repository.Context
                                            .Get<Group>(p => rfidList.Contains(p.RFID)).ToList();

            var groupNoList =   groupList.Select(p =>new { id=p.ID.ToString(),p.Flag,p.RFID })
                                            .ToList();
            errRFIDList = rfidList.Except(groupNoList.Select(l => l.RFID).ToList()).ToList();//未查到分组的标签
            errGroupNoList =groupNoList.Where(k => k.Flag == true).Select(k=>k.id).ToList();//已经发运的
            var NoshipGroupNoList= groupNoList.Where(k => k.Flag == false).Select(k => k.id).ToList();//未发运的
            var qrList = this.repository.Context
                                      .Get<QRCode>(p => NoshipGroupNoList.Contains(p.GID.ToString()))
                                      .ToList();
            var rightsGNo= qrList.Where(p => CIDList.Contains(p.CID)).GroupBy(p => p.GID).Select(p => p.Key.ToString()).ToList();
            var errNo = NoshipGroupNoList.Except(rightsGNo).ToList();
            errGroupNoList=errGroupNoList.Union(errNo).ToList();
            invoice.GroupNoList = rightsGNo.Count > 0 ? String.Join(",", rightsGNo).ToString():null;
            invoice.CodeList= String.Join(",", rfidList).ToString();
            invoice.ErrGroupNoList = errGroupNoList.Count > 0 ? String.Join(",", errGroupNoList).ToString():null;//错误的分组号
            invoice.ErrRFIDList = errRFIDList.Count()>0? String.Join(",", errRFIDList).ToString():null;
            invoice.SubmitTime = DateTime.Now;
            this.repository.Update(invoice);
            this.repository.Commit();
            IList<InvoiceShipment> invoiceShipment =this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == invoice.ID).ToList();
            invoice.InvoiceShipmentList = invoiceShipment;
            InvoiceDataObject dataobject = mapper.Map<Invoice, InvoiceDataObject>(invoice);
            grouplist = Mapper.Map(groupList, grouplist);
            foreach (var group in grouplist)
            {
                int CID = this.repository.Context.Get<QRCode>(p => p.GID == group.ID).FirstOrDefault().CID;
                group.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == CID).FirstOrDefault());
            }
            dataobject.GroupList = grouplist;
            dataobject.ErrGroupList = errgroupList;
            return dataobject;
        }
        public InvoiceDataObject GetInfomation(int id)
        {
            InvoiceDataObject invoice = this.GetByID(id);
            var invoiceShipmentList = Mapper.Map <IList< InvoiceShipment>, IList<InvoiceShipmentDataObject>>( this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == id).ToList());
            invoice.InvoiceShipmentList = invoiceShipmentList;
            IList<GroupDataObject> groupList = new List<GroupDataObject>();
            if (invoice.GroupNoList.Count() > 0)
            {
                foreach(var gid in invoice.GroupNoList)
                {
                    GroupDataObject group = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == int.Parse(gid)).FirstOrDefault());
                    int cid = this.repository.Context.Get<QRCode>(p=>p.GID==int.Parse(gid)).OrderBy(p=>p.Time).FirstOrDefault().CID;
                    group.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == cid).FirstOrDefault());
                    groupList.Add(group);
                }
            }
            invoice.GroupList = groupList;
            IList<GroupDataObject> errGroupList = new List<GroupDataObject>();
            if (invoice.ErrGroupNoList.Count() > 0)
            {
                foreach (var gid in invoice.ErrGroupNoList)
                {
                    GroupDataObject errGroup = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == int.Parse(gid)).FirstOrDefault());
                    int cid = this.repository.Context.Get<QRCode>(p => p.GID == int.Parse(gid)).OrderBy(p => p.Time).FirstOrDefault().CID;
                    errGroup.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == cid).FirstOrDefault());
                    errGroupList.Add(errGroup);
                }
            }
            invoice.ErrGroupList = errGroupList;
            return invoice;

        }
        //取消关联二维码，并将二维码移动到RFIDGroup中
        public bool RemoveCode(InvoiceDataObject dataObject)
        {
            Invoice invoice = this.repository.Get(p => p.ID == dataObject.ID).FirstOrDefault();
            string qrcodeList = invoice.CodeList;
            string groupnoList = invoice.GroupNoList;
            invoice.CodeList = null;
            invoice.GroupNoList = null;
            invoice.ErrGroupNoList = null;
            invoice.ErrRFIDList = null;
            invoice.Flag = false;
            this.repository.Update(invoice);
            int one = this.repository.Commit();
            RFIDGroup rFIDGroup = this.repository.Context.Create<RFIDGroup>();
            rFIDGroup.GroupNoList = groupnoList;
            rFIDGroup.QRCodeList = qrcodeList;
            rFIDGroup.CompanyID = invoice.CompanyID;
            rFIDGroup.OldInvoiceID = invoice.ID;
            rFIDGroup.OldInvoiceNo = invoice.No;
            rFIDGroup.OldTime = invoice.LastUpdateTime;
            this.repository.Context.Add<RFIDGroup>(rFIDGroup);
            int two = this.repository.Context.Commit();
            //group flag=false
            List<string> list = groupnoList.Split(',', StringSplitOptions.None).ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                if (String.IsNullOrWhiteSpace(list[i]))
                    continue;
                Group group = this.repository.Context.GetUpdateEntity<Group>().Where(p => p.ID == Int32.Parse(list[i])).FirstOrDefault();
                group.Flag = false;
                this.repository.Context.Update<Group>(group);
            }
            int three=this.repository.Context.Commit();
            if (one > 0 && two > 0 && three>0)
                return true;
            return false;

        }
        //重新关联
        public InvoiceDataObject RelationCode(int id, int rfid)
        {
            Invoice invoice = this.repository.Context.GetUpdateEntity<Invoice>().Where(p => p.ID == id).FirstOrDefault();
            RFIDGroup rFIDGroup = this.repository.Context.GetUpdateEntity<RFIDGroup>().Where(p => p.ID == rfid).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(invoice.CodeList) && string.IsNullOrWhiteSpace(invoice.GroupNoList))
            {
                invoice.CodeList = rFIDGroup.QRCodeList;
                invoice.GroupNoList = rFIDGroup.GroupNoList;
            }
            else
            {
                List<string> invoicecodeList = invoice.CodeList.Split(',', StringSplitOptions.None).ToList();
                List<string> qrCodeList = rFIDGroup.QRCodeList.Split(',', StringSplitOptions.None).ToList();
                List<string> invoiceGroupNoList = invoice.GroupNoList.Split(',', StringSplitOptions.None).ToList();
                List<string> groupList = rFIDGroup.GroupNoList.Split(',', StringSplitOptions.None).ToList();
                //去重之后的结果
                var qrcode = qrCodeList.Except(invoicecodeList);//, new Utils.Utils.StringComparer()
                var groupNo = groupList.Except(invoiceGroupNoList);//, new Utils.Utils.StringComparer()
                if (qrcode.Count() == 0 || qrcode == null || groupNo.Count() == 0 || groupNo == null)
                    return mapper.Map<Invoice, InvoiceDataObject>(invoice);
                List<string> code = invoicecodeList.Union(qrcode).ToList();
                invoice.CodeList = String.Join(",", code).ToString();
                List<string> group = invoiceGroupNoList.Union(groupNo).ToList();
                invoice.GroupNoList = String.Join(",", group).ToString();
            }
            this.repository.Update(invoice);
            this.repository.Commit();
            this.repository.Context.Remove<RFIDGroup>(rFIDGroup);
            this.repository.Context.Commit();
            return mapper.Map<Invoice, InvoiceDataObject>(invoice);
        }
        //查询所有公司的所有交货单
        public IList<InvoiceDataObject> GetInvoices(InvoiceDataObject dataObject)
        {
            IList<InvoiceDataObject> invoiceList = Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.GetByDescending(p => !p.Deleted&&p.CompanyID==dataObject.CompanyID, p => p.LastUpdateTime, 1, 20 * dataObject.Page).ToList());
            return invoiceList;
        }
       
        //查看一个
        public InvoiceDataObject Get(int id)
        {
            InvoiceDataObject dataObject = this.GetByID(id);
            if (dataObject == null)
                return dataObject;
            IList<InvoiceShipmentDataObject> invoiceShipmentList = mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == id).ToList());
            dataObject.InvoiceShipmentList = invoiceShipmentList;
            IList<InvoiceUserInfoDataObject> invoiceUserInfoList = mapper.Map<IList<InvoiceUserInfo>, IList<InvoiceUserInfoDataObject>>(this.repository.Context.Get<InvoiceUserInfo>(p => p.InvoiceID == id).ToList());
            dataObject.InvoiceUserInfoList = invoiceUserInfoList;
            return dataObject;
        }
        //查看一个
        public InvoiceDataObject GetOne(int id)
        {
            InvoiceDataObject dataObject = this.GetByID(id);
            if (dataObject == null)
                return dataObject;
            IList<InvoiceShipmentDataObject> invoiceShipmentList = mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == id).ToList());
            dataObject.InvoiceShipmentList = invoiceShipmentList;
            IList<InvoiceUserInfoDataObject> invoiceUserInfoList = mapper.Map<IList<InvoiceUserInfo>, IList<InvoiceUserInfoDataObject>>(this.repository.Context.Get<InvoiceUserInfo>(p => p.InvoiceID == id).ToList());
            dataObject.InvoiceUserInfoList = invoiceUserInfoList;
            if (dataObject.GroupNoList == null)
                return dataObject;
            IList<GroupDataObject> grouplist = new List<GroupDataObject>();
            IList<CategoryDataObject> catelist = new List<CategoryDataObject>();
            for (int i = 0; i < dataObject.GroupNoList.Count(); i++)
            {
                if (!String.IsNullOrWhiteSpace(dataObject.GroupNoList[i]))
                {
                    GroupDataObject group = mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == Int32.Parse(dataObject.GroupNoList[i])).FirstOrDefault());
                    if (group != null)
                    {
                        QRCode qr = this.repository.Context.Get<QRCode>(p => p.GID == group.ID).FirstOrDefault();
                        if (qr == null)
                            continue;
                        Operation operation = this.repository.Context.Get<Operation>(p => p.ID == qr.OperationID).FirstOrDefault();
                        if (operation != null)
                        {
                            CategoryDataObject cate = mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == operation.CategoryID).FirstOrDefault());
                            catelist.Add(cate);
                        }
                    }
                    grouplist.Add(group);
                }

            }
            dataObject.CategoryList = catelist;
            dataObject.GroupList = grouplist;
            return dataObject;
        }
        //分公司查看invoice
        public IList<InvoiceDataObject> GetListByCompanyID(int id)
        {
            List<InvoiceDataObject> list = new List<InvoiceDataObject>();
            IList<InvoiceDataObject> sumdai = this.GetListNoGroupSum(id);
            IList<InvoiceDataObject> wei = this.GetListGroup(id);
            IList<InvoiceDataObject> yi = this.QueryFlagTrueByToday(id);
            list.AddRange(sumdai);
            list.AddRange(wei);
            list.AddRange(yi);
            return list;
        }
        //分公司查看invoice,电脑端
        public IList<InvoiceDataObject> GetList(int id)
        {
            List<InvoiceDataObject> list = Mapper.Map<List<Invoice>, List<InvoiceDataObject>>(
                                            this.repository.Get(p => !p.Deleted && p.CompanyID == id &&p.CreateTime>DateTime.Now.Date.AddDays(-7))
                                            .OrderByDescending(p => p.LastUpdateTime).ToList());
            IList<UserInfoDataObject> userList = Mapper.Map<List<UserInfo>, List<UserInfoDataObject>>(this.repository.Context.Get<UserInfo>(p => p.CompanyID == id).ToList());
            foreach(var invoice in list)
            {
                invoice.UserInfo = invoice.UserInfoID == 0 ? null : userList.Where(p => p.ID == invoice.UserInfoID).FirstOrDefault();
            }
            return list;
        }
        //未勾选的待发运
        public IList<InvoiceDataObject> GetListNoGroup(int id)
        {
            IList<InvoiceDataObject> invoiceList = Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted &&p.Checked==false&& p.CompanyID == id &&p.Flag==false&& string.IsNullOrWhiteSpace(p.CodeList)).OrderByDescending(p=>p.LastUpdateTime).ToList());
            IList<UserInfoDataObject> userList = Mapper.Map<List<UserInfo>, List<UserInfoDataObject>>(this.repository.Context.Get<UserInfo>(p => p.CompanyID == id).ToList());
            foreach (var invoice in invoiceList)
            {
                invoice.UserInfo = invoice.UserInfoID == 0 ? null : userList.Where(p => p.ID == invoice.UserInfoID).FirstOrDefault();
            }
            return invoiceList;
        } 
        //全部待发运
        public IList<InvoiceDataObject> GetListNoGroupSum(int id)
        {
            IList<InvoiceDataObject> invoiceList = Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted &&  p.CompanyID == id && p.Flag == false && string.IsNullOrWhiteSpace(p.GroupNoList)).OrderByDescending(p => p.LastUpdateTime).ToList());
            IList<UserInfoDataObject> userList = Mapper.Map<List<UserInfo>, List<UserInfoDataObject>>(this.repository.Context.Get<UserInfo>(p => p.CompanyID == id).ToList());
            foreach (var invoice in invoiceList)
            {
                invoice.UserInfo = invoice.UserInfoID == 0 ? null : userList.Where(p => p.ID == invoice.UserInfoID).FirstOrDefault();
            }
            return invoiceList;
        }
        //全部未完成
        public IList<InvoiceDataObject> GetListGroup(int id)
        {
            IList<InvoiceDataObject> invoiceList = Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p =>!p.Deleted && p.CompanyID == id && p.Flag==false&&!string.IsNullOrWhiteSpace(p.GroupNoList)).OrderByDescending(p=>p.LastUpdateTime).ToList());
            IList<UserInfoDataObject> userList = Mapper.Map<List<UserInfo>, List<UserInfoDataObject>>(this.repository.Context.Get<UserInfo>(p => p.CompanyID == id).ToList());
            foreach (var invoice in invoiceList)
            {
                invoice.UserInfo = invoice.UserInfoID == 0 ? null : userList.Where(p => p.ID == invoice.UserInfoID).FirstOrDefault();
            }
            return invoiceList;
        }
        //没有认领的未完成
        public IList<InvoiceDataObject> GetListGroupNoUser(int id)
        {
            IList<InvoiceDataObject> invoiceList = Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted && p.CompanyID == id && p.Flag == false && !string.IsNullOrWhiteSpace(p.GroupNoList)&&p.UserInfoID==0&&!p.Checked).OrderByDescending(p => p.LastUpdateTime).ToList());
            return invoiceList;
        }
        public override InvoiceDataObject Add(InvoiceDataObject dataObject)
        {
            Invoice invoice = this.repository.Create();
            invoice = Mapper.Map(dataObject, invoice);
            invoice.OrderNo = String.IsNullOrWhiteSpace(invoice.OrderNo)?null:invoice.OrderNo.Replace("_", "").Trim();
            for (int i = 0; i < invoice.InvoiceShipmentList.Count(); i++)
            {
                invoice.InvoiceShipmentList[i].CreateTime = DateTime.Now;
                invoice.InvoiceShipmentList[i].LastUpdateTime = DateTime.Now;
                
            }
            this.repository.Add(invoice);
            this.repository.Commit();
            return mapper.Map<Invoice, InvoiceDataObject>(invoice);
        }
        public InvoiceDataObject AddInvoice(InvoiceAndShipment dataObject)
        {
            Invoice invoice = this.repository.Create();
            InvoiceShipment invoiceShipment = this.repository.Context.Create<InvoiceShipment>();
            IList<InvoiceShipment> invoiceShipmentList = new List<InvoiceShipment>();
            invoice = Mapper.Map(dataObject, invoice);
            invoiceShipment = Mapper.Map(dataObject, invoiceShipment);
            Company company = this.repository.Context.Get<Company>(p => p.ID == dataObject.CompanyID).FirstOrDefault();
            this.repository.Add(invoice);
            this.repository.Commit();
            invoiceShipment.ID = 0;
            invoiceShipment.InvoiceID = invoice.ID;
            this.repository.Context.Add<InvoiceShipment>(invoiceShipment);
            this.repository.Commit();
            invoiceShipmentList.Add(invoiceShipment);
            invoice.InvoiceShipmentList = invoiceShipmentList;
            return mapper.Map<Invoice, InvoiceDataObject>(invoice);
        }
        //查询
        public IList<InvoiceDataObject> Query(string query)
        {
            return Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => p.No.Contains(query.Trim())).ToList());
        }
        public IList<CategoryDataObject> GetCategoryByCode(InvoiceDataObject dataObject)
        {
            List<Category> categoryList = new List<Category>();
            string CodeList = String.Join(",", dataObject.CodeList).ToString();
            for (int i = 0; i < dataObject.CodeList.Count(); i++)
            {
                QRCode qrcode = this.repository.Context.Get<QRCode>(p => p.Content == dataObject.CodeList[i].Trim()).FirstOrDefault();
                if (qrcode != null)
                {
                    Operation operation = this.repository.Context.Get<Operation>(p => p.ProductionLineID == qrcode.ProductionLineID).OrderByDescending(p => p.LastUpdateTime).FirstOrDefault();
                    Category category = this.repository.Context.Get<Category>(p => p.ID == operation.CategoryID).FirstOrDefault();
                    categoryList.Add(category);
                }
            }
            return mapper.Map<IList<Category>, IList<CategoryDataObject>>(categoryList);
        }
        //一组二维码查到分组信息
        public IList<GroupDataObject> GetGroupByCode(InvoiceDataObject dataObject)
        {
            IList<GroupDataObject> groupList = new List<GroupDataObject>();
            IList<string> groupNoList = new List<string>();
            groupNoList = this.repository.Context
                              .Get<QRCode>(p => dataObject.CodeList.Contains(p.Content.Trim()))
                              .Select(p => p.GID.ToString())
                              .Distinct()
                              .ToList();
            if (groupNoList == null)
                return groupList;
            dataObject.GroupNoList = groupNoList;
            groupList = UpdateGroup(dataObject);
            return groupList;
        }
        public IList<GroupDataObject> GetGroupByID(InvoiceDataObject dataObject, out string Quantity)
        {
            InvoiceDataObject invoice = this.GetByID(dataObject.ID);
            IList<InvoiceUserInfoDataObject> invoiceUserInfoList = Mapper.Map<IList<InvoiceUserInfo>, IList<InvoiceUserInfoDataObject>>(
                            this.repository.Context.Get<InvoiceUserInfo>(p => !p.Deleted && p.InvoiceID == dataObject.ID).ToList());
            invoice.InvoiceUserInfoList = invoiceUserInfoList;
            Quantity = "";
            IList<GroupDataObject> newList = new List<GroupDataObject>();
            if (invoice.GroupNoList == null)
                return newList;
            newList= UpdateGroup(invoice);
            Quantity = string.Format("{0:0.00}", newList.Sum(p => p.Rule));
            return newList;
        }
        //一组Rfid查到所属品类
        public IList<CategoryDataObject> GetCategoryByRFID(InvoiceDataObject dataObject)
        {
            IList<Group> groupList = this.repository.Context.Get<Group>(p => dataObject.CodeList.Contains(p.RFID)).ToList();
            List<Category> categoryList = new List<Category>();
            string CodeList = String.Join(",", dataObject.CodeList).ToString();
            for (int i = 0; i < groupList.Count(); i++)
            {
                Operation operation = this.repository.Context.Get<Operation>(p => p.ProductionLineID == groupList[i].ProductionLineID).OrderByDescending(p => p.LastUpdateTime).FirstOrDefault();
                if (operation != null)
                {
                    Category category = this.repository.Context.Get<Category>(p => p.ID == operation.CategoryID).FirstOrDefault();
                    categoryList.Add(category);
                }
            }
            return mapper.Map<IList<Category>, IList<CategoryDataObject>>(categoryList);
        }

        public IList<GroupDataObject> GetGroupByRFID(InvoiceDataObject dataObject)
        {

            IList<GroupDataObject> groupList = mapper.Map<IList<Group>, IList<GroupDataObject>>(this.repository.Context.Get<Group>(p => dataObject.CodeList.Contains(p.RFID)).ToList());
            IList<string> groupNoList = groupList.Select(p => p.ID.ToString()).ToList();
            IList<GroupDataObject> newList = new List<GroupDataObject>();
            if (groupNoList == null)
                return newList;
            dataObject.GroupNoList = groupNoList;
            newList = UpdateGroup(dataObject);
            return newList;
        }
        //品类及吨数信息查看
        public IList<GroupDataObject> GetGroupByNo(InvoiceDataObject dataObject)
        {
            InvoiceDataObject invoice = this.GetByID(dataObject.ID);
            IList<InvoiceUserInfoDataObject> invoiceUserInfoList = Mapper.Map<IList<InvoiceUserInfo>, IList<InvoiceUserInfoDataObject>>(
                            this.repository.Context.Get<InvoiceUserInfo>(p => !p.Deleted && p.InvoiceID == dataObject.ID).ToList());
            invoice.InvoiceUserInfoList = invoiceUserInfoList;
            IList<GroupDataObject> groupList = mapper.Map<IList<Group>, IList<GroupDataObject>>(this.repository.Context.Get<Group>(p => invoice.GroupNoList.Contains(p.ID.ToString())).ToList());
            IList<GroupDataObject> newList = new List<GroupDataObject>();
            if (invoice.GroupNoList == null)
                return newList;
            newList = UpdateGroup(invoice);
            return newList;
        }
        //标记交货完成
        public bool AddFlag(int id)
        {
            Invoice invoice = this.repository.Get(p => p.ID == id).FirstOrDefault();
            if (!String.IsNullOrWhiteSpace(invoice.GroupNoList))
            {
                List<string> list = invoice.GroupNoList.Split(',', StringSplitOptions.None).ToList();
                var groupList = this.repository.Context.GetUpdateEntity<Group>().Where(p => list.Contains(p.ID.ToString())).ToList();
                foreach(var group in groupList)
                {
                    group.Flag = true;
                    group.InvoiceNo = invoice.ID.ToString();
                    this.repository.Context.Update<Group>(group);
                }
                this.repository.Context.Commit();
            }
            invoice.Checked = true;
            invoice.Flag = true;
            invoice.SubmitTime=DateTime.Now;
            this.repository.Update(invoice);
            return this.repository.Commit()>0;
        }
        //有限制的标记交货完成：理货员专用
        public bool AddFlagLimit(int id,out string message)
        {
            InvoiceDataObject invoice = this.Get(id);
            if (invoice.Flag)
            {
                message = "此交货单已经发运完成，请勿重新提交！";
                return false;
            }
            if (invoice.InvoiceShipmentList.Count() > 0)
            {
                int i = 0;
                foreach (var ship in invoice.InvoiceShipmentList)
                {
                    if (ship.Quantity == ship.GroupQuantitySum)
                    {
                        i++;
                    }
                }
                if (i != invoice.InvoiceShipmentList.Count())
                {
                    message = "数量不符，不能提交！";
                    return false;
                }
                if (invoice.GroupNoList.Count() > 0)
                {
                    IList<string> list = invoice.GroupNoList;
                    var groupList = this.repository.Context.GetUpdateEntity<Group>().Where(p => list.Contains(p.ID.ToString())).ToList();
                    foreach (var group in groupList)
                    {
                        group.Flag = true;
                        group.InvoiceNo = invoice.No;
                        this.repository.Context.Update<Group>(group);
                    }
                    this.repository.Context.Commit();
                }
                invoice.Checked = true;
                invoice.Flag = true;
                invoice.SubmitTime = DateTime.Now.ToString();
                invoice.InvoiceShipmentList = null;
                invoice.InvoiceUserInfoList = null;
                this.Update(invoice);
                message = invoice.Flag ? "提交成功" : "提交失败";
                return invoice.Flag;
            }
            message = "提交的交货单没有物料！";
            return false;
        }
        //查询已经发货完成的交货单
        public IList<InvoiceDataObject> QueryFlagTrue(int companyID)
        {
            IList<InvoiceDataObject> invoiceList = mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p =>!p.Deleted && p.CompanyID == companyID && p.Flag == true).OrderByDescending(p => p.LastUpdateTime).ToList());
            IList<UserInfoDataObject> userList = Mapper.Map<List<UserInfo>, List<UserInfoDataObject>>(this.repository.Context.Get<UserInfo>(p => p.CompanyID == companyID).ToList());
            foreach (var invoice in invoiceList)
            {
                invoice.UserInfo = invoice.UserInfoID == 0 ? null : userList.Where(p => p.ID == invoice.UserInfoID).FirstOrDefault();
            }
            return invoiceList;
        }
        //已完成发货单今天数据
        public IList<InvoiceDataObject> QueryFlagTrueByToday(int companyID)
        {
            IList<InvoiceDataObject> invoiceList = mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted && p.CompanyID == companyID && p.Flag == true && p.SubmitTime > DateTime.Today).OrderByDescending(p => p.LastUpdateTime).ToList());
            return invoiceList;
        }
        public bool Exists(string no)
        {
            return this.repository.Exists(p =>!p.Deleted&& p.No == no.Trim());
        }
       
        public IList<GroupDataObject> UpdateGroup (InvoiceDataObject invoice)
        {
            IList<GroupDataObject> newList = new List<GroupDataObject>();
            if (invoice.GroupNoList == null||invoice.GroupNoList.Count()==0)
                return newList;
            var groupInts = this.repository.Context.Get<QRCode>(p => invoice.GroupNoList.Contains(p.GID.ToString()))
                                        .GroupBy(p => p.GID)
                                        .Select(p => new GroupInt
                                        {
                                            Gid = p.Key,
                                            Quantity = (float)p.Sum(k => k.OperationRule) / 1000,
                                            CID = p.Select(k => k.CID).FirstOrDefault(),
                                        })
                                        .ToList();

            foreach (var item in groupInts)
            {
                GroupDataObject group = new GroupDataObject();
                group.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == item.CID).FirstOrDefault());
                group.Rule = item.Quantity;
                if (invoice.InvoiceUserInfoList != null|| invoice.InvoiceUserInfoList.Count() > 0)
                {
                    var invUserInfo = invoice.InvoiceUserInfoList.Where(p => p.GroupNoList.Contains(item.Gid.ToString())).FirstOrDefault();
                    group.UserInfoID = invUserInfo == null ? 0 : invUserInfo.UserInfoID;
                    group.UserInfoName = invUserInfo == null ? null : invUserInfo.UserInfoName;
                }
                newList.Add(group);
            }
            return newList;
        }
        public override InvoiceDataObject Update(InvoiceDataObject dataObject)
        {
            Invoice invoice = this.repository.Context.GetUpdateEntity<Invoice>().Where(p => p.ID == dataObject.ID).FirstOrDefault();
            IList<InvoiceShipment> invoiceShipmentList = this.repository.Context.GetUpdateEntity<InvoiceShipment>().Where(p => p.InvoiceID == invoice.ID).ToList();
            if (dataObject.InvoiceShipmentList == null || dataObject.InvoiceShipmentList.Count() == 0)
                dataObject.InvoiceShipmentList = Mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(invoiceShipmentList);
            if (dataObject.CompanyID == 0)
                dataObject.CompanyID = invoice.CompanyID;
            //if (dataObject.InvoiceShipmentList != null || dataObject.InvoiceShipmentList.Count() > 0)
            //{
            //    for (int i = 0; i < dataObject.InvoiceShipmentList.Count(); i++)
            //    {
            //        dataObject.InvoiceShipmentList[i].LastUpdateTime = DateTime.Now.ToString();
            //    }
            //}
            invoice = mapper.Map(dataObject, invoice);
            if (invoice.InvoiceShipmentList != null || invoice.InvoiceShipmentList.Count() > 0)
            {
                for (int i = 0; i < invoice.InvoiceShipmentList.Count(); i++)
                {
                    invoice.InvoiceShipmentList[i].CreateTime = invoice.CreateTime;
                    invoice.InvoiceShipmentList[i].LastUpdateTime = DateTime.Now;
                }
            }

            this.repository.Update(invoice);
            this.repository.Commit();
            return Mapper.Map<Invoice, InvoiceDataObject>(invoice);
        }
        public IList<int> GetSumList(int id,int userInfoID)
        {
            InvoiceDataObject invoice = new InvoiceDataObject() { UserInfoID = userInfoID };
            IList<int> sumList = new List<int>();
            //按公司查所有
            int sum = this.GetListByCompanyID(id).Count();
            sumList.Add(sum);//全部
            int dai = this.GetListNoGroup(id).Count();
            sumList.Add(dai);//待发运
            int wei = this.GetListGroupNoUser(id).Count(); 
            sumList.Add(wei);//未完成发运
            int yi = this.QueryFlagTrueByToday(id).Count();
            sumList.Add(yi);//已发运
            int kong=this.repository.Context.Get<RFIDGroup>(p=> !p.Deleted&&p.CompanyID==id).OrderByDescending(p => p.LastUpdateTime).ToList().Count();
            sumList.Add(kong);//空单
            int medai = this.GetListNoGroupByUserInfoID(invoice).Count();
            sumList.Add(medai);//我的待发运
            int mewei = this.GetListGroupByUserInfoID(invoice).Count();
            sumList.Add(mewei);//我的未完成
            int meyi = this.QueryFlagTrueTodayByUserInfoID(invoice).Count();
            sumList.Add(meyi);//我的已完成            
            int mequan = this.GetListByUserInfoID(invoice).Count();
            sumList.Add(mequan);//我的全部
            return sumList;
        }
        //监控软件用的拿数
        public IList<int> GetSum(int id, int userInfoID)
        {
            InvoiceDataObject invoice = new InvoiceDataObject() { UserInfoID = userInfoID };
            IList<int> sumList = new List<int>();
            //按公司查所有
            int sum = this.GetListByCompanyID(id).Count();
            sumList.Add(sum);//全部
            int dai = this.GetListNoGroupSum(id).Count();
            sumList.Add(dai);//全部待发运
            int wei = this.GetListGroup(id).Count();
            sumList.Add(wei);//未完成发运
            int yi = this.QueryFlagTrueByToday(id).Count();
            sumList.Add(yi);//已发运
            return sumList;
        }
        //手持机取消关联
        public InvoiceDataObject DeleteCode(int id)
        {
            InvoiceDataObject invoice = this.Get(id);
            if (invoice.Flag)
                return invoice;
            var groupNo = invoice.GroupNoList;
            invoice.GroupNoList = null;
            invoice.LastGroupNoList = null;
            invoice.LastNo = null;
            invoice.ErrGroupNoList = null;
            invoice.ErrRFIDList = null;
            invoice.CodeList = null;
            foreach (var ship in invoice.InvoiceShipmentList)
            {
                if (ship.GroupNoList != null)
                {
                    ship.GroupNoList = null;
                    ship.GroupSum = 0;
                }
                    
            }
            var invoiceUserInfoIDList = invoice.InvoiceUserInfoList.Select(p=>p.ID).ToList();
            var invoiceUserInfoList = this.repository.Context.GetUpdateEntity<InvoiceUserInfo>().Where(p => invoiceUserInfoIDList.Contains(p.ID)).ToList();
            foreach (var user in invoiceUserInfoList)
            {
                if (user.GroupNoList != null||user.CodeList!=null)
                {
                    user.GroupNoList = null;
                    user.CodeList = null;
                    this.repository.Context.Update<InvoiceUserInfo>(user);
                }
            }
            this.repository.Context.Commit();
            InvoiceDataObject inv= this.Update(invoice);
            var groupList = this.repository.Context.GetUpdateEntity<Group>().Where(p => groupNo.Contains(p.ID.ToString())).ToList();
            foreach (var group in groupList)
            {
                group.Flag = false;
                group.InvoiceNo = null;
                this.repository.Context.Update<Group>(group);
            }
            this.repository.Context.Commit();
            return inv;
        }
        //手持机取消关联，用户
        public InvoiceDataObject DeleteCodeByUserInfoID(int id,int userInfoID,out string message)
        {
            InvoiceDataObject invoice = this.Get(id);
            if (invoice.Flag)
            {
                message = "此交货单已经发运，无法清除！";
                return invoice;
            } 
            InvoiceUserInfoDataObject invoiceUserInfo = invoice.InvoiceUserInfoList.Where(p => p.InvoiceID == id && p.UserInfoID == userInfoID).FirstOrDefault();
            if (invoiceUserInfo == null || invoiceUserInfo.ID == 0)
            {
                message = "此理货员下没有扫码，无法清除！";
                return invoice;
            }
            var codelist = invoiceUserInfo.CodeList.ToList();
            var deletestr = invoiceUserInfo.GroupNoList.ToList();
            if (deletestr.Count()==0)
            {
                message = "此理货员下没有网兜，无法清除！";
                return invoice;
               
            }
            bool userFlag = this.RevokeUser(invoiceUserInfo.ID, deletestr);
            bool flag = this.RevokeShip(invoice.ID, deletestr);
            var groupNo = invoice.GroupNoList;
            invoice.GroupNoList = invoice.GroupNoList.Except(deletestr).Distinct().ToList();
            invoice.LastGroupNoList = invoice.GroupNoList.Except(deletestr).Distinct().ToList();
            invoice.LastNo = invoice.GroupNoList.Count()==0?null: invoice.GroupNoList.Last();
            invoice.CodeList = invoice.CodeList.Except(codelist).ToList();
            invoice.InvoiceShipmentList = null;
            invoice.InvoiceUserInfoList = null;
            InvoiceDataObject inv = this.Update(invoice);
            var groupList = this.repository.Context.GetUpdateEntity<Group>().Where(p => groupNo.Contains(p.ID.ToString())).ToList();
            foreach (var group in groupList)
            {
                group.Flag = false;
                group.InvoiceNo = null;
                this.repository.Context.Update<Group>(group);
            }
            this.repository.Context.Commit();
            message =flag && userFlag?"清除成功！":"清除失败!";
            return inv;
        }
        //按照交货单号查询
        public InvoiceDataObject GetByNo(string no)
        {
            return Mapper.Map<Invoice, InvoiceDataObject>(this.repository.Get(p=>!p.Deleted && p.No==no.Trim()).FirstOrDefault());
        }

        //更正为错误
        public InvoiceDataObject UpdateErrNo(int id, int groupID)
        {
            InvoiceDataObject invoice = this.GetByID(id);
            invoice.InvoiceShipmentList = Mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == id).ToList());
            IList<string> gNo = new List<string>() { groupID.ToString() };
            invoice.GroupNoList=invoice.GroupNoList.Except(gNo).ToList();
            invoice.ErrGroupNoList=invoice.ErrGroupNoList.Union(gNo).ToList();
            invoice = this.Update(invoice);
            IList<GroupDataObject> groupList = new List<GroupDataObject>();
            if (invoice.GroupNoList.Count() > 0)
            {
                foreach (var gid in invoice.GroupNoList)
                {
                    GroupDataObject group = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == int.Parse(gid)).FirstOrDefault());
                    int cid = this.repository.Context.Get<QRCode>(p => p.GID == int.Parse(gid)).OrderBy(p => p.Time).FirstOrDefault().CID;
                    group.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == cid).FirstOrDefault());
                    groupList.Add(group);
                }
            }
            invoice.GroupList = groupList;
            IList<GroupDataObject> errGroupList = new List<GroupDataObject>();
            if (invoice.ErrGroupNoList.Count() > 0)
            {
                foreach (var gid in invoice.ErrGroupNoList)
                {
                    GroupDataObject errGroup = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == int.Parse(gid)).FirstOrDefault());
                    int cid = this.repository.Context.Get<QRCode>(p => p.GID == int.Parse(gid)).OrderBy(p => p.Time).FirstOrDefault().CID;
                    errGroup.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == cid).FirstOrDefault());
                    errGroupList.Add(errGroup);
                }
            }
            invoice.ErrGroupList = errGroupList;
            return invoice;
        }
        //更正为正确分组
        public InvoiceDataObject UpdateRightNo(int id, int groupID)
        {
            InvoiceDataObject invoice = this.GetByID(id);
            IList<string> gNo = new List<string>() { groupID.ToString() };
            invoice.ErrGroupNoList=invoice.ErrGroupNoList.Except(gNo).ToList();
            invoice.GroupNoList=invoice.GroupNoList.Union(gNo).ToList();
            invoice = this.Update(invoice);
            var invoiceShipmentList = Mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == id).ToList());
            invoice.InvoiceShipmentList = invoiceShipmentList;
            IList<GroupDataObject> groupList = new List<GroupDataObject>();
            if (invoice.GroupNoList.Count() > 0)
            {
                foreach (var gid in invoice.GroupNoList)
                {
                    GroupDataObject group = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == int.Parse(gid)).FirstOrDefault());
                    int cid = this.repository.Context.Get<QRCode>(p => p.GID == int.Parse(gid)).OrderBy(p => p.Time).FirstOrDefault().CID;
                    group.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == cid).FirstOrDefault());
                    groupList.Add(group);
                }
            }
            invoice.GroupList = groupList;
            IList<GroupDataObject> errGroupList = new List<GroupDataObject>();
            if (invoice.ErrGroupNoList.Count() > 0)
            {
                foreach (var gid in invoice.ErrGroupNoList)
                {
                    GroupDataObject errGroup = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == int.Parse(gid)).FirstOrDefault());
                    int cid = this.repository.Context.Get<QRCode>(p => p.GID == int.Parse(gid)).OrderBy(p => p.Time).FirstOrDefault().CID;
                    errGroup.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => p.ID == cid).FirstOrDefault());
                    errGroupList.Add(errGroup);
                }
            }
            invoice.ErrGroupList = errGroupList;
            return invoice;
        }
        //发货单分配到人员
        public bool AddUserInfo(InvoiceDataObject invoice)
        {
            InvoiceDataObject dataObject = this.Get(invoice.ID);
            if (dataObject == null)
                return false;
            dataObject.UserInfoID = invoice.UserInfoID;
            dataObject.Checked = true;
            dataObject.InvoiceShipmentList = null;
            InvoiceDataObject result = this.Update(dataObject);
            if (result.UserInfoID != 0)
                return true;
            return false;
        }
        //我的全部
        public IList<InvoiceDataObject> GetListByUserInfoID(InvoiceDataObject invoice)
        {
            List<InvoiceDataObject> list = new List<InvoiceDataObject>();
            IList<InvoiceDataObject> dai = this.GetListNoGroupByUserInfoID(invoice);
            list.AddRange(dai);
            IList<InvoiceDataObject> wei = this.GetListGroupByUserInfoID(invoice);
            list.AddRange(wei);
            //IList<InvoiceDataObject> yi = this.QueryFlagTrueTodayByUserInfoID(invoice);
            //list.AddRange(yi);
            return list;
        }
        //我的未完成（所有）
        public IList<InvoiceDataObject> GetListGroupByUserInfoID(InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> invoiceList = Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted && p.UserInfoID == invoice.UserInfoID && p.Flag == false && !string.IsNullOrWhiteSpace(p.CodeList)).OrderByDescending(p => p.LastUpdateTime).ToList());
            return invoiceList;
        }
       
        //我的已完成
        public IList<InvoiceDataObject> QueryFlagTrueByUserInfoID(InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> invoiceList = mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted && p.UserInfoID == invoice.UserInfoID && p.Flag == true).OrderByDescending(p => p.LastUpdateTime).ToList());
            return invoiceList;
        }
        //我的已完成(今天)
        public IList<InvoiceDataObject> QueryFlagTrueTodayByUserInfoID(InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> invoiceList = mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted && p.UserInfoID == invoice.UserInfoID && p.Flag == true && p.SubmitTime > DateTime.Today).OrderByDescending(p => p.LastUpdateTime).ToList());
            return invoiceList;
        }
        //我的待发运(所有)
        public IList<InvoiceDataObject> GetListNoGroupByUserInfoID(InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> invoiceList = Mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted &&p.Checked==true&& p.UserInfoID == invoice.UserInfoID && p.Flag == false && string.IsNullOrWhiteSpace(p.CodeList)).OrderByDescending(p => p.LastUpdateTime).ToList());
            return invoiceList;
        }
        //删除用户
        public bool RemoveUserInfo(InvoiceDataObject invoice)
        {
            InvoiceDataObject dataObject = this.Get(invoice.ID);
            if (dataObject == null)
                return false;
            //if (dataObject.CodeList.Count>0)
            //    return false;f
            dataObject.UserInfoID = 0;
            dataObject.Checked = false;
            dataObject.InvoiceShipmentList = null;
            InvoiceDataObject result = this.Update(dataObject);
            if (result.UserInfoID == 0|| result.Checked==false)
                return true;
            return false;
        }

        public InvoiceDataObject QuantitySum(InvoiceDataObject dataObject)
        {
            InvoiceDataObject invoice = this.GetByID(dataObject.ID);
            invoice.Memo = dataObject.Memo;
            var memo = dataObject.Memo.Replace("-", "+-").Split("+");
            //var memo = dataObject.Memo.Split(",");
            int[] intArray;
            intArray = Array.ConvertAll<string, int>(memo, s => int.Parse(s));
            invoice.Quantity = String.IsNullOrWhiteSpace(intArray.Sum().ToString())?"0": intArray.Sum().ToString();
            invoice.InvoiceShipmentList = Mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == dataObject.ID).ToList());
            var result = this.Update(invoice);
            return result;
        }

        public bool TransferGNo(InvoiceDataObject dataObject, out string message)
        {
            InvoiceDataObject invoice = this.Get(dataObject.ID);
            InvoiceUserInfo invoiceUserInfo = this.repository.Context.GetUpdateEntity<InvoiceUserInfo>().Where(p => p.InvoiceID == dataObject.ID && p.UserInfoID == dataObject.UserInfoID).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(invoice.LastNo))
            {
                message = "此单没有最后一兜，无法转移！";
                return false;
            }
            if (String.IsNullOrWhiteSpace(invoiceUserInfo.GroupNoList))
            {
                message = "此理货员名下没有最后一兜，无法转移！";
                return false;
            }
            var lastGNoList = invoiceUserInfo.GroupNoList.Split(',', StringSplitOptions.None).ToList();
            var last = lastGNoList.Last();
            InvoiceDataObject NewInvoice = this.Get(dataObject.NewID);
            if (NewInvoice.Flag)
            {
                message = "新交货单已经发运，无法转移！";
                return false;
            }
            //invoice.InvoiceShipmentList = Mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == dataObject.ID).ToList());
            //NewInvoice.InvoiceShipmentList = Mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => p.InvoiceID == dataObject.NewID).ToList());
            var shipmentList = this.repository.Context.Get<InvoiceShipment>(p => !p.Deleted && p.InvoiceID == dataObject.NewID).Select(p => new { p.MaterialNo, p.ID }).ToList();
            var materialNoList = shipmentList.Select(p => p.MaterialNo).ToList();
            if (materialNoList == null || materialNoList.Count() <= 0)
            {
                message = "新交货单没有发运物料信息！";
                return false;
            }
            var clist = this.repository.Context.Get<Category>(p => !p.Deleted && materialNoList.Contains(p.MaterialNo)).Select(p => new { p.MaterialNo,p.ID}).ToList();
            List<int> CIDList = clist.Select(p => p.ID).ToList();
            var qRCode = this.repository.Context
                              .Get<QRCode>(p => !p.Deleted && p.GID.ToString() == last).FirstOrDefault();
            if (qRCode == null)
            {
                message = "最后一个网兜没有二维码信息！";
                return false;
            }
            if (!CIDList.Contains(qRCode.CID))
            {
                message = "最后一个网兜物料与新交货单不符！";
                return false;
            }
            string materialNo = clist.Where(p => p.ID == qRCode.CID).FirstOrDefault().MaterialNo;
            NewInvoice.LastNo = last;
            bool revokeShipFlag = this.RevokeShip(invoice.ID,last.Split("").ToList());
            invoice.GroupNoList = invoice.GroupNoList.Except(NewInvoice.LastNo.Split("")).ToList();
            invoice.LastNo= invoice.GroupNoList.Count()==0 ? null : invoice.GroupNoList.Last();
            invoice.LastGroupNoList= invoice.LastGroupNoList.Except(NewInvoice.LastNo.Split("")).ToList();
            invoice.InvoiceShipmentList = null;
            invoice.InvoiceUserInfoList = null;
            var result1 = this.Update(invoice);
            bool one = result1.LastNo != NewInvoice.LastNo;
            int newshipid = shipmentList.Where(p => p.MaterialNo == materialNo).Select(p=>p.ID).FirstOrDefault();
            bool addShipFlag = this.AddShip(newshipid,last);
            NewInvoice.GroupNoList = NewInvoice.GroupNoList.Union(NewInvoice.LastNo.Split("")).ToList();
            NewInvoice.LastGroupNoList= NewInvoice.LastGroupNoList.Union(NewInvoice.LastNo.Split("")).ToList();
            NewInvoice.InvoiceShipmentList = null;
            NewInvoice.InvoiceUserInfoList = null;
            var result2 = this.Update(NewInvoice);
            bool two = result2.GroupNoList.Contains(NewInvoice.LastNo);
            var invUserGNo= lastGNoList.Except(last.Split("")).ToList();
            invoiceUserInfo.GroupNoList= string.IsNullOrWhiteSpace(string.Join(",", invUserGNo))?null: string.Join(",", invUserGNo);
            this.repository.Context.Update<InvoiceUserInfo>(invoiceUserInfo);
            this.repository.Context.Commit();
            InvoiceUserInfo newInvoiceUserInfo = this.repository.Context.GetUpdateEntity<InvoiceUserInfo>().Where(p => p.InvoiceID == dataObject.NewID && p.UserInfoID == dataObject.UserInfoID).FirstOrDefault();
            if (newInvoiceUserInfo == null)
            {
                InvoiceUserInfo invoiceUser = this.repository.Context.Create<InvoiceUserInfo>();
                invoiceUser.GroupNoList = last;
                invoiceUser.InvoiceID = dataObject.NewID;
                invoiceUser.UserInfoID = dataObject.UserInfoID;
                this.repository.Context.Update<InvoiceUserInfo>(invoiceUser);
                this.repository.Context.Commit();
            }
            else if(string.IsNullOrWhiteSpace(newInvoiceUserInfo.GroupNoList))
            {
                newInvoiceUserInfo.GroupNoList = last;
                this.repository.Context.Update<InvoiceUserInfo>(newInvoiceUserInfo);
                this.repository.Context.Commit();
            }
            else
            {
                var gnoList = newInvoiceUserInfo.GroupNoList.Split(",", StringSplitOptions.None).ToList();
                var newList = gnoList.Union(last.Split("")).ToList();
                newInvoiceUserInfo.GroupNoList = string.Join(",",newList);
                this.repository.Context.Update<InvoiceUserInfo>(newInvoiceUserInfo);
                this.repository.Context.Commit();
            }
            bool flag =invoiceUserInfo.GroupNoList.Split(',', StringSplitOptions.None).ToList().Contains(last);
            message = (!flag && one && two && revokeShipFlag && addShipFlag) ? "转移最后一兜成功！" : "转移最后一兜失败！";
            if (!flag && one && two && revokeShipFlag && addShipFlag)
                return true;
            return false;
        }
        //撤销方法
        public bool RevokeGNo(InvoiceDataObject dataObject, out string message)
        {
            InvoiceDataObject invoice = this.Get(dataObject.ID);
            InvoiceUserInfo invoiceUserInfo = this.repository.Context.GetUpdateEntity<InvoiceUserInfo>().Where(p => p.InvoiceID == dataObject.ID && p.UserInfoID == dataObject.UserInfoID).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(invoice.LastNo)|| invoice==null||invoice.GroupNoList==null|| invoice.GroupNoList.Count()==0)
            {
                message = "此发货单下没有网兜，无法撤销！";
                return false;
            }
            else if (invoiceUserInfo == null|| invoiceUserInfo.GroupNoList==null||invoiceUserInfo.GroupNoList.Count()==0)
            {
                message = "此理货员名下没有网兜，无法撤销！";
                return false;
            }
            else
            {
                var groupnolist = invoiceUserInfo.GroupNoList.Split(',', StringSplitOptions.None).ToList();
                string lastgno = groupnolist.Last();
                bool shipFlag = this.RevokeShip(dataObject.ID, lastgno.Split("").ToList());
                var list = invoice.GroupNoList.Except(lastgno.Split("")).ToList();
                invoice.GroupNoList = list ==null||list.Count()==0?null: list;
                var newGroupNoList= groupnolist.Except(lastgno.Split("")).ToList();
                invoiceUserInfo.GroupNoList = string.Join(",", newGroupNoList);
                invoice.LastGroupNoList = invoice.GroupNoList; 
                invoice.LastNo = invoice.GroupNoList==null?null: invoice.GroupNoList.Last();
                invoice.InvoiceShipmentList = null;
                invoice.InvoiceUserInfoList = null;
                var result1 = this.Update(invoice);
                this.repository.Context.Update<InvoiceUserInfo>(invoiceUserInfo);
                this.repository.Context.Commit();
                var last= invoiceUserInfo.GroupNoList.Split(',', StringSplitOptions.None).ToList().Last();
                bool state = last != lastgno;
                message = state && shipFlag ? "撤销最后一兜成功！":"撤销最后一兜失败！";
                return state;
            }
        }
        //id：InvoiceUserInfo的id ，list：撤销的组号list
        public bool RevokeUser(int id, List<string> list)
        {
            var invoiceUsers = this.repository.Context.GetUpdateEntity<InvoiceUserInfo>().Where(p => p.ID == id).FirstOrDefault();
            var gnolist = invoiceUsers.GroupNoList.Split(",", StringSplitOptions.None).Except(list).ToList();

            if(gnolist==null|| gnolist.Count() == 0)
            {
                invoiceUsers.GroupNoList = null;
                invoiceUsers.CodeList = null;
            }
            else
            {
                invoiceUsers.GroupNoList = string.Join(",", gnolist);
            }
            this.repository.Context.Update<InvoiceUserInfo>(invoiceUsers);  
            int result = this.repository.Context.Commit();
            return result > 0;
        }
        //id：InvoiceShipment的id ，list：撤销的组号list
        public bool RevokeShip(int id, List<string> list)
        {
            if (list.Count() > 0)
            {
                InvoiceShipment ship = this.repository.Context.GetUpdateEntity<InvoiceShipment>().Where(p => p.InvoiceID == id && p.GroupNoList.Contains(list.First())).FirstOrDefault();
                if (ship == null)
                    return false;
                foreach (var group in list)
                {
                    var sum = this.repository.Context.Get<QRCode>(p => !p.Deleted && p.GID == Int32.Parse(group))
                                             .GroupBy(p => new { p.CID, p.OperationRule })
                                             .Select(p => new { CID = p.Key.CID,Rule=p.Key.OperationRule, Count = p.Count() })
                                             .FirstOrDefault();
                    ship.GroupSum = ship.GroupSum-sum.Count<0?0: ship.GroupSum - sum.Count;
                    if (ship.CID == 0 || ship.QRRule == 0)
                    {
                        ship.CID = sum.CID;
                        ship.QRRule = sum.Rule;
                    }
                        
                    var strlist = ship.GroupNoList.Split(",", StringSplitOptions.None).Except(group.Split("")).ToList();
                    if(strlist == null || strlist.Count() == 0)
                    {
                        ship.GroupNoList = null;
                        ship.GroupSum = 0;
                    }
                    else
                    {
                        ship.GroupNoList = string.Join(",", strlist);
                    }
                    this.repository.Context.Update<InvoiceShipment>(ship);
                }
                int result = this.repository.Context.Commit();
                return result > 0;
            }
            else
            {
                return false;
            }
        }
        //shipment里新增组号  id:新增组号，gno增加的组号
        public bool AddShip(int id, string gNo)
        {
            var groupNoList = gNo.Split("").ToList();
            InvoiceShipment invoiceShipment = this.repository.Context.GetUpdateEntity<InvoiceShipment>()
                                                .Where(p => p.ID == id).FirstOrDefault();
            var sum = this.repository.Context.Get<QRCode>(p => !p.Deleted && p.GID == Int32.Parse(gNo))
                                            .GroupBy(p => new { p.CID, p.OperationRule })
                                            .Select(p => new { CID = p.Key.CID, Rule = p.Key.OperationRule, Count = p.Count() })
                                            .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(invoiceShipment.GroupNoList))
            {
                invoiceShipment.GroupNoList = string.Join(",", groupNoList);
                invoiceShipment.CID = sum.CID;
                invoiceShipment.QRRule = sum.Rule;
                invoiceShipment.GroupSum = sum.Count;
            }
            else
            {
                if(invoiceShipment.CID==0|| invoiceShipment.QRRule == 0)
                {
                    invoiceShipment.CID = sum.CID;
                    invoiceShipment.QRRule = sum.Rule;
                }
                invoiceShipment.GroupSum = invoiceShipment.GroupSum + sum.Count;
                var list = invoiceShipment.GroupNoList.Split(",", StringSplitOptions.None).ToList();
                list = list.Union(groupNoList).Distinct().ToList();
                invoiceShipment.GroupNoList = string.Join(",", list);
            }
            this.repository.Context.Update<InvoiceShipment>(invoiceShipment);
            return this.repository.Context.Commit() > 0;
        }

        public bool RevokeQRCode(InvoiceDataObject dataObject, out string message)
        {
            InvoiceDataObject invoice = this.Get(dataObject.ID);
            if (invoice.Flag)
            {
                message = "此交货单已发运，无法撤销!";
                return false;
            }
            if (invoice.InvoiceUserInfoList == null|| invoice.InvoiceUserInfoList.Count()==0)
            {
                message = "此交货单下没有网兜，无法撤销!";
                return false;
            }
            var qrcode= this.repository.Context.Get<QRCode>(p => p.Content.Replace("\r", "") == dataObject.RevokeQRCode.Replace("\r", "")).FirstOrDefault();
            if (qrcode == null)
            {
                message = "二维码未找到，无法撤销!";
                return false;
            }                
            else
            {
                string lastgno = qrcode.GID.ToString();
                if (!invoice.GroupNoList.Contains(lastgno))
                {
                    message = "二维码网兜并不在此发货单里，无法撤销！";
                    return false;
                }
                int id = 0;
                foreach(var invoiceUserInfo in invoice.InvoiceUserInfoList)
                {
                    if (invoiceUserInfo.GroupNoList.Contains(lastgno))
                    {
                        id = invoiceUserInfo.ID;
                    }
                }
                bool userFlag = false;
                if (id != 0)
                {
                    userFlag = this.RevokeUser(id, lastgno.Split("").ToList());
                }
                int gNoSum = invoice.GroupNoList.Count();
                invoice.GroupNoList = invoice.GroupNoList.Except(lastgno.Split("")).ToList();
                invoice.InvoiceShipmentList = null;
                invoice.InvoiceUserInfoList = null;
                var result = this.Update(invoice);
                bool shipflag = this.RevokeShip(invoice.ID, lastgno.Split("").ToList());
                bool state=result.GroupNoList.Count() < gNoSum;
                message = shipflag  && state ? "撤销指定二维码成功！": "撤销指定二维码失败！";
                return state;
            }
        }

        public bool AlreadyShiped(int id)
        {
            return this.repository.Exists(p => !p.Deleted&&p.ID ==id&&p.Flag);
        }

        public bool UpdateFlag(InvoiceDataObject dataObject, out string message)
        {
            InvoiceDataObject invoice = this.Get(dataObject.ID);
            if (!invoice.Flag)
            {
                message = "此交货单未提交，不能更改！";
                return false;
            }
            if (invoice.GroupNoList.Count()>0)
            {
                IList<string> list = invoice.GroupNoList;
                var groupList = this.repository.Context.GetUpdateEntity<Group>().Where(p => list.Contains(p.ID.ToString())).ToList();
                foreach (var group in groupList)
                {
                    group.Flag = false;
                    group.InvoiceNo = null;
                    this.repository.Context.Update<Group>(group);
                }
                this.repository.Context.Commit();
            }
            invoice.Flag = false;
            invoice.UserInfoID = 0;
            invoice.Checked = false;
            invoice.InvoiceShipmentList = null;
            var result=this.Update(invoice);
            if (result.Flag)
            {
                message = "此交货单更改失败！";
                return false;
            }
            message = "此交货单已更改为未完成单！";
            return true;
        }

        public IList<InvoiceDataObject> GetListByQuery(InvoiceDataObject dataObject)
        {
            if (dataObject.CompanyID == 0)
                return null;
            IList<InvoiceDataObject> list = this.GetList(dataObject.CompanyID);
            if (!string.IsNullOrWhiteSpace(dataObject.SubmitTime))
                list = list.Where (p=>DateTime.Parse(p.SubmitTime).Date==DateTime.Parse(dataObject.SubmitTime).Date).OrderByDescending(p => p.LastUpdateTime).ToList();
            if (!string.IsNullOrWhiteSpace(dataObject.PlateNumber))
                list = list.Where(p => p.PlateNumber.Contains(dataObject.PlateNumber)).ToList();
            if (!string.IsNullOrWhiteSpace(dataObject.DriverName))
                list = list.Where(p => p.DriverName.Contains(dataObject.DriverName)).ToList();
            if(!string.IsNullOrWhiteSpace(dataObject.DealerName))
                list= list.Where(p => p.DealerName.Contains(dataObject.DealerName)).ToList();
            if(!string.IsNullOrWhiteSpace(dataObject.No))
                list = list.Where(p => p.No.Contains(dataObject.No)).ToList();
            if (list == null || list.Count() == 0)
                return null;
            IList<UserInfoDataObject> userList = Mapper.Map<List<UserInfo>, List<UserInfoDataObject>>(this.repository.Context.Get<UserInfo>(p => p.CompanyID == dataObject.CompanyID).ToList());
            foreach (var invoice in list)
            {
                invoice.UserInfo = invoice.UserInfoID == 0 ? null : userList.Where(p => p.ID == invoice.UserInfoID).FirstOrDefault();
            }
            return list;
        }

        public IList<InvoiceDataObject> QueryShip(InvoiceDataObject dataObject)
        {
            if (dataObject.CompanyID == 0)
                return null;
            IList<InvoiceDataObject> list = mapper.Map<IList<Invoice>, IList<InvoiceDataObject>>(this.repository.Get(p => !p.Deleted && p.CompanyID == dataObject.CompanyID && p.SubmitTime > DateTime.Parse(dataObject.CreateTime) && p.SubmitTime < DateTime.Parse(dataObject.LastUpdateTime)).ToList());
            var newList = new List<InvoiceDataObject>();
            foreach (var invoice in list)
            {
                invoice.InvoiceShipmentList = mapper.Map<IList<InvoiceShipment>, IList<InvoiceShipmentDataObject>>(this.repository.Context.Get<InvoiceShipment>(p => !p.Deleted && p.InvoiceID == invoice.ID).ToList());
            }
            if (!string.IsNullOrWhiteSpace(dataObject.DealerName))
                list = list.Where(p => p.DealerName == dataObject.DealerName).ToList();
            if (dataObject.CID != 0)
            {
                string material = this.repository.Context.Get<Category>(p => p.ID == dataObject.CID).FirstOrDefault().MaterialNo;
                foreach(var inv in list)
                {
                    inv.InvoiceShipmentList = inv.InvoiceShipmentList.Where(p => p.MaterialNo == material).ToList();
                    if (inv.InvoiceShipmentList == null || inv.InvoiceShipmentList.Count() == 0)
                        continue;
                    newList.Add(inv);
                }
                return newList;
            }
            return list;
        }

        public IList<InvoiceShipmentDataObject> TotalDate(InvoiceDataObject dataObject)
        {
            if (dataObject.CompanyID == 0)//公司id为0，退出
                return null;
            if (String.IsNullOrWhiteSpace(dataObject.CreateTime))
                return null;
            var invoiceList = this.repository.Get(p => p.CompanyID == dataObject.CompanyID && p.Flag && p.SubmitTime > DateTime.Parse(dataObject.CreateTime).Date
                                      && p.SubmitTime < DateTime.Parse(dataObject.CreateTime).Date.AddDays(1))
                                     .OrderBy(p => p.CreateTime).Select(p=>new
                                     {
                                         id=p.ID,
                                         no=p.No
                                     });
            IList<int> ids = invoiceList.Select(p => p.id).ToList();
            return null;
        }
        public override int RemoveByID(int id)
        {

            return base.RemoveByID(id);
        }
    } 
}
