using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IOperationService:IService<OperationDataObject>
    {
        IList<OperationDataObject> GetListByCompanyID(int id);
        IList<OperationDataObject> GetOperationByCompanyID(int id);
        OperationDataObject GetLatest(int id);
       // OperationDataObject AddUpdateQRCode(OperationDataObject operation);
        bool UpdateState(OperationDataObject dataObject);
        OperationDataObject GetOperationByProductionLine(int id);
        IList<OperationDataObject> GetHistoryList(int companyID, int pageNo, out int pageCount);
        IList<OperationDataObject> Query(OperationDataObject operation, out int pageCount);
        OperationDataObject UpdateOperation(OperationDataObject operation);
        IList<OperationDataObject> QueryState(OperationDataObject operation);
        bool EndGroup(IList<int> opID);
        OperationDataObject UpdateSum(OperationDataObject dataObject);
    }
}
