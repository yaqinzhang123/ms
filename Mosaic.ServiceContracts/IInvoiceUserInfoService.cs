using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IInvoiceUserInfoService : IService<InvoiceUserInfoDataObject>
    {
        bool UpdateQRCode(InvoiceUserInfoDataObject invoiceUserInfo);
        bool Exist(InvoiceUserInfoDataObject invoiceUserInfo);
    }
}
