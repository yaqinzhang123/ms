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
    public class InvoiceUserInfoService : Service<InvoiceUserInfoDataObject, InvoiceUserInfo>, IInvoiceUserInfoService
    {
        public InvoiceUserInfoService(IInvoiceUserInfoRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        // 按照invoiceID、UserInfoID查找是否存在。
        public bool Exist(InvoiceUserInfoDataObject invoiceUserInfo)
        {
            return this.repository.Exists(p => p.InvoiceID == invoiceUserInfo.InvoiceID && p.UserInfoID == invoiceUserInfo.UserInfoID);
        }
        //codelist后添加code
        public bool UpdateQRCode(InvoiceUserInfoDataObject invoiceUserInfo)
        {
            string code = null;
            if (invoiceUserInfo.CodeList.Count() == 1)
                code = invoiceUserInfo.CodeList.First();
            if (Exist(invoiceUserInfo))
            {
                var invoiceUser= mapper.Map<InvoiceUserInfo, InvoiceUserInfoDataObject>(this.repository.Get(p => p.InvoiceID == invoiceUserInfo.InvoiceID
                                    && p.UserInfoID == invoiceUserInfo.UserInfoID)
                               .FirstOrDefault());
                int length1 = invoiceUser.CodeList.Count();
                invoiceUser.CodeList.Add(code);
                invoiceUser.CodeList=invoiceUser.CodeList.Distinct().ToList();
                int length2 = invoiceUser.CodeList.Count();
                var result=this.Update(invoiceUser);
                return result.CodeList.Contains(code) ? true : false;
                
            }
            else
            {
                var username = this.repository.Context.Get<UserInfo>(p => p.ID == invoiceUserInfo.UserInfoID).Select(p => p.Name).FirstOrDefault();
                invoiceUserInfo.UserInfoName = username;
                var result=this.Add(invoiceUserInfo);
                return result == null || result.ID == 0 ? false : true;
            }
            
        }
    }
}