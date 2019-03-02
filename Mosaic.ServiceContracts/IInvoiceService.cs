using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IInvoiceService : IService<InvoiceDataObject>
    {
        InvoiceDataObject Get(int id);
        IList<InvoiceDataObject> GetListByCompanyID(int id);
        IList<InvoiceDataObject> GetList(int id);
        IList<InvoiceDataObject> GetListNoGroup(int id);
        IList<InvoiceDataObject> GetListGroup(int id);
        InvoiceDataObject UpdateQRCode(InvoiceDataObject dataObject, out string message);
        InvoiceDataObject UpdateRFIDCode(InvoiceDataObject dataObject);
        bool RemoveCode(InvoiceDataObject dataObject);
        IList<InvoiceDataObject> GetInvoices(InvoiceDataObject dataObject);//·ÖÒ³
        InvoiceDataObject RelationCode(int id, int rfid);
        IList<InvoiceDataObject> Query(string query);
        IList<CategoryDataObject> GetCategoryByCode(InvoiceDataObject dataObject);
        IList<CategoryDataObject> GetCategoryByRFID(InvoiceDataObject dataObject);
        IList<GroupDataObject> GetGroupByRFID(InvoiceDataObject dataObject);
        InvoiceDataObject DeleteCode(int id);
        IList<GroupDataObject> GetGroupByCode(InvoiceDataObject dataObject);
        InvoiceDataObject AddInvoice(InvoiceAndShipment invoiceAndShipment);
        bool AddFlag(int id);
        IList<InvoiceDataObject> QueryFlagTrue(int companyID);
        bool Exists(string no);
        IList<int> GetSumList(int id, int userInfoID);
        IList<int> GetSum(int id, int userInfoID);
        IList<InvoiceDataObject> QueryFlagTrueByToday(int companyID);
        InvoiceDataObject GetByNo(string no);
        IList<GroupDataObject> GetGroupByNo(InvoiceDataObject dataObject);
        InvoiceDataObject GetInfomation(int id);
        InvoiceDataObject UpdateErrNo(int id, int groupID);
        InvoiceDataObject UpdateRightNo(int id, int groupID);
        bool AddUserInfo(InvoiceDataObject invoice);
        IList<InvoiceDataObject> GetListByUserInfoID(InvoiceDataObject invoice);
        IList<InvoiceDataObject> GetListGroupByUserInfoID(InvoiceDataObject invoice);
        IList<InvoiceDataObject> QueryFlagTrueByUserInfoID(InvoiceDataObject invoice);
        IList<InvoiceDataObject> QueryFlagTrueTodayByUserInfoID(InvoiceDataObject invoice);
        IList<InvoiceDataObject> GetListNoGroupByUserInfoID(InvoiceDataObject invoice);
        bool RemoveUserInfo(InvoiceDataObject invoice);
        IList<InvoiceDataObject> GetListNoGroupSum(int id);
        IList<GroupDataObject> GetGroupByID(InvoiceDataObject dataObject, out string quantity);
        InvoiceDataObject GetOne(int id);
        InvoiceDataObject QuantitySum(InvoiceDataObject dataObject);
        bool AlreadyShiped(int id);
        bool TransferGNo(InvoiceDataObject dataObject, out string message);
        bool RevokeGNo(InvoiceDataObject dataObject, out string message);
        bool RevokeQRCode(InvoiceDataObject dataObject, out string message);
        bool UpdateFlag(InvoiceDataObject dataObject, out string message);
        IList<InvoiceDataObject> GetListByQuery(InvoiceDataObject dataObject);
        InvoiceDataObject DeleteCodeByUserInfoID(int id, int userInfoID, out string message);
        bool AddFlagLimit(int id,out string message);
        IList<InvoiceDataObject> QueryShip(InvoiceDataObject dataObject);
        IList<InvoiceDataObject> GetListGroupNoUser(int id);
        IList<InvoiceShipmentDataObject> TotalDate(InvoiceDataObject dataObject);
    }
}
