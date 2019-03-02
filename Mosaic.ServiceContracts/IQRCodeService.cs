using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IQRCodeService : IService<QRCodeDataObject>
    {
        bool Exists(string count);
        IList<QRCodeDataObject> GetListByProductionID(int id);
        QRCodeDataObject GetContent(string content);
        IList<QRCodeDataObject> GetListByTime(QRCodeDataObject qrcode);
        IList<QRCodeDataObject> Query(string qrcode);
        IList<QRCodeDataObject> UpdateList(IList<QRCodeDataObject> qrList);
        QRCodeDataObject UpdateSingle(QRCodeDataObject qrcode);
        IList<QRCodeDataObject> Unlock(QRCodeDataObject qr);
        IList<QRCodeDataObject> GetQRCodeList();
        QRCodeDataObject UpdateStartRoot(QRCodeDataObject qrcode);
        QRCodeDataObject UpdateManualSkip(QRCodeDataObject qrcode);
        QRCodeDataObject UpdateVirtualAdd(QRCodeDataObject qrcode);
        QRCodeDataObject UpdateEndRoot(QRCodeDataObject qrcode);
        QRCodeDataObject GetEndQRByProductionLineID(int productionLineID);
        QRCodeDataObject GetByContent(QRCodeDataObject qrcode);
        IList<TotalQRCode> GetTotal(QRCodeDataObject dataObject);
        bool IsThree(string content);
        List<ReadRateQRCode> GetReadRate(QRCodeDataObject dataObject);
        IList<CategoryDataObject> GetCategoryByTime(QRCodeDataObject dataObject);
        List<ReadRateQRCode> GetRateAndTotal(QRCodeDataObject dataObject);
        int AddList(IList<QRCodeDataObject> qRCodeList);
        IList<ReadRateQRCode> TotalDate(QRCodeDataObject dataObject);
    }
}
