using AutoMapper;
using DYFramework.Application;
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
    public class RFIDGroupService : Service<RFIDGroupDataObject, RFIDGroup>, IRFIDGroupService
    {
        private readonly IInvoiceService invoiceService;

        public RFIDGroupService(IRFIDGroupRepository repository, IMapper mapper,IInvoiceService invoiceService) : base(repository, mapper)
        {
            this.invoiceService = invoiceService;
        }
        //按公司查已经取消关联的网兜并按最新时间排序
        public IList<RFIDGroupDataObject> GetListByCompanyID(int id)
        {
            IList < RFIDGroupDataObject > rFIDGroupList= Mapper.Map<IList<RFIDGroup>, IList<RFIDGroupDataObject>>(this.repository.Get(p => p.CompanyID == id ).OrderByDescending(p => p.LastUpdateTime).ToList());
            //IList<GroupDataObject> groupList = new List<GroupDataObject>();
            //for (int i = 0; i < rFIDGroupList.Count(); i++)
            //{
            //    for(int j = 0; j < rFIDGroupList[i].GroupNoList.Count(); j++)
            //    {
            //        GroupDataObject group = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == Int32.Parse(rFIDGroupList[i].GroupNoList[j])).FirstOrDefault());
            //        group.QRCodeList= Mapper.Map<IList<QRCode>,IList<QRCodeDataObject>>(this.repository.Context.Get<QRCode>(p => p.GID == group.ID).ToList());
            //        groupList.Add(group);
            //    }
            //    rFIDGroupList[i].GroupList = groupList;
            //}
            return rFIDGroupList;
        }
        //按原交货单查未关联网兜
        public IList<RFIDGroupDataObject> GetListByInvoice(int id)
        {
            IList<RFIDGroupDataObject> rFIDGroupList = Mapper.Map<IList<RFIDGroup>, IList<RFIDGroupDataObject>>(this.repository.Get(p => p.OldInvoiceID == id).ToList());
            IList<GroupDataObject> groupList = new List<GroupDataObject>();
            for (int i = 0; i < rFIDGroupList.Count(); i++)
            {
                for (int j = 0; j < rFIDGroupList[i].GroupNoList.Count(); j++)
                {
                    GroupDataObject group = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == Int32.Parse(rFIDGroupList[i].GroupNoList[j])).FirstOrDefault());
                    group.QRCodeList = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Context.Get<QRCode>(p => p.GID == group.ID).ToList());
                    groupList.Add(group);
                }
                rFIDGroupList[i].GroupList = groupList;
            }
            return rFIDGroupList;
        }
        public override RFIDGroupDataObject Add(RFIDGroupDataObject dataObject)
        {
            IList<string> groupNoList = new List<string>();
            IList<Group> groupList = new List<Group>();
            dataObject.OldInvoiceID = 0;
            dataObject.OldInvoiceShipmentID = 0;
            dataObject.OldInvoiceNo = "空单发运";
            if (dataObject.QRCodeList.Count() == 1&& string.Equals(dataObject.QRCodeList[0].Substring(0,4),"http"))
            {
                string qr = dataObject.QRCodeList[0];
                string code = qr.Replace("\n", "").Replace("\r", ""); 
                QRCode qRCode = this.repository.Context.Get<QRCode>(p => code.Contains(p.Content)).FirstOrDefault();
                if (qRCode == null)
                    return new RFIDGroupDataObject();
                Group group = this.repository.Context.Get<Group>(p => p.ID == qRCode.GID).FirstOrDefault();
                groupList.Add(group);
            }
            else
            {
                groupList = this.repository.Context.Get<Group>(p => dataObject.QRCodeList.Contains(p.RFID)).ToList();

            }
            if (groupList != null)
            {
                for (int i=0;i<groupList.Count();i++)
                {
                    string gNo = groupList[i].ID.ToString();
                    groupNoList.Add(gNo);
                }
                dataObject.GroupNoList = groupNoList;
            }
            else
            {
                    // dataObject.QRCodeList.Remove(dataObject.QRCodeList[i].Trim());
            }
            return base.Add(dataObject);
        }
        public  RFIDGroupDataObject GetRFIDGroup(int id)
        {
            RFIDGroupDataObject rFIDGroup = base.GetByID(id);
            if (rFIDGroup == null)
                return rFIDGroup;
            IList<GroupDataObject> groupList = new List<GroupDataObject>();
            for (int i = 0; i < rFIDGroup.GroupNoList.Count(); i++)
            {
                GroupDataObject group = Mapper.Map<Group, GroupDataObject>(this.repository.Context.Get<Group>(p => p.ID == Int32.Parse(rFIDGroup.GroupNoList[i])).FirstOrDefault());
                if (group != null)
                {
                    group.QRCodeList = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Context.Get<QRCode>(p => p.GID == group.ID).ToList());
                    groupList.Add(group);
                }
            }
            rFIDGroup.GroupList = groupList;
            return rFIDGroup;
        }
        //更新rfid
        public RFIDGroupDataObject UpdateRFID(RFIDGroupDataObject dataObject)
        {
            IList<string> groupNoList = new List<string>();
            RFIDGroup rFIDGroup = this.repository.Context.GetUpdateEntity<RFIDGroup>().Where(p => p.ID == dataObject.ID).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(rFIDGroup.QRCodeList))
            {
                for (int i = 0; i < dataObject.QRCodeList.Count(); i++)
                {
                    Group group = this.repository.Context.Get<Group>(p => p.RFID == dataObject.QRCodeList[i].Trim()).FirstOrDefault();
                    if (group != null)
                    {
                        string gNo = group.ID.ToString();
                        groupNoList.Add(gNo);
                    }
                    else
                    {
                       // dataObject.QRCodeList.Remove(dataObject.QRCodeList[i].Trim());
                    }

                }
                rFIDGroup.QRCodeList = String.Join(",", dataObject.QRCodeList).ToString();
                rFIDGroup.GroupNoList = String.Join(",", groupNoList).ToString();
            }
            else
            {
                IList<string> qrcodelist = rFIDGroup.QRCodeList.Split(',', StringSplitOptions.None).ToList();
                var list1 = dataObject.QRCodeList.Except(qrcodelist, new Utils.Utils.StringComparer());
                if (list1 == null || list1.Count() == 0)
                    return mapper.Map<RFIDGroup, RFIDGroupDataObject>(rFIDGroup);
                List<string> newCodeList = qrcodelist.Union(list1).ToList();
                for (int j = 0; j < newCodeList.Count(); j++)
                {
                    Group group = this.repository.Context.Get<Group>(p => p.RFID == newCodeList[j].Trim()).FirstOrDefault();
                    if (group != null)
                    {
                        string gNo = group.ID.ToString();
                        groupNoList.Add(gNo);
                    }
                    else
                    {
                      //  newCodeList.Remove(newCodeList[j].Trim());
                    }
                }
                rFIDGroup.QRCodeList = String.Join(",", newCodeList).ToString();
                rFIDGroup.GroupNoList = String.Join(",", groupNoList).ToString();
            }
            this.repository.Update(rFIDGroup);
            this.repository.Commit();
            return mapper.Map<RFIDGroup, RFIDGroupDataObject>(rFIDGroup);
        }
        //更新qrcode
        public RFIDGroupDataObject UpdateQRCode(RFIDGroupDataObject dataObject)
        {
            IList<string> groupNoList = new List<string>();
            RFIDGroup rFIDGroup = this.repository.Get(p => p.ID == dataObject.ID).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(rFIDGroup.QRCodeList))
            {

                for (int i = 0; i < dataObject.QRCodeList.Count(); i++)
                {
                    QRCode qrcode = this.repository.Context.Get<QRCode>(p => p.Content == dataObject.QRCodeList[i].Trim()).OrderByDescending(p => p.LastUpdateTime).FirstOrDefault();
                    if (qrcode != null)
                    {
                        string gNo = qrcode.GID.ToString();
                        groupNoList.Add(gNo);
                    }
                    else
                    {
                       // dataObject.QRCodeList.Remove(dataObject.QRCodeList[i].Trim());
                    }
                }
                rFIDGroup.QRCodeList = String.Join(",", dataObject.QRCodeList).ToString();
                rFIDGroup.GroupNoList = String.Join(",", groupNoList).ToString();
            }
            else
            {
                IList<string> qrcodelist = rFIDGroup.QRCodeList.Split(',', StringSplitOptions.None).ToList();
                var list1 = dataObject.QRCodeList.Except(qrcodelist, new Utils.Utils.StringComparer());
                if (list1 == null || list1.Count() == 0)
                    return mapper.Map<RFIDGroup, RFIDGroupDataObject>(rFIDGroup);
                List<string> newCodeList = qrcodelist.Union(list1).ToList();
                // invoice.CodeList = String.Join(",", newCodeList).ToString();
                for (int j = 0; j < newCodeList.Count(); j++)
                {

                    QRCode qrcode = this.repository.Context.Get<QRCode>(p => p.Content == newCodeList[j].Trim()).FirstOrDefault();
                    if (qrcode != null)
                    {
                        string gNo = qrcode.GID.ToString();
                        groupNoList.Add(gNo);
                    }
                    else
                    {
                       // newCodeList.Remove(newCodeList[j].Trim());
                    }
                }
                rFIDGroup.QRCodeList = String.Join(",", newCodeList).ToString();
                rFIDGroup.GroupNoList = String.Join(",", groupNoList).ToString();
            }
            this.repository.Update(rFIDGroup);
            this.repository.Commit();
            return mapper.Map<RFIDGroup, RFIDGroupDataObject>(rFIDGroup);
        }
        public RFIDGroupDataObject Get(int id)
        {
            RFIDGroupDataObject rFIDGroup = this.GetByID(id);
            if (rFIDGroup.QRCodeList.Count() == 0)
                return rFIDGroup;
            InvoiceDataObject invoice = new InvoiceDataObject();
            invoice.CodeList = rFIDGroup.QRCodeList;
            if (String.Equals(invoice.CodeList[0].Substring(0, 1), "h")){
                IList<GroupDataObject> groups = this.invoiceService.GetGroupByCode(invoice);
                rFIDGroup.GroupList = groups;
                return rFIDGroup;
            }
            IList<GroupDataObject> groupList = this.invoiceService.GetGroupByRFID(invoice);
            rFIDGroup.GroupList = groupList;
            return rFIDGroup;
        }
      
    }
}
