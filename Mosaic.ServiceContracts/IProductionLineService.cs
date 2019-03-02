using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IProductionLineService:IService<ProductionLineDataObject>
    {
        IList<ProductionLineDataObject> GetProductionLinesByCompanyID(int CompanyID);
        bool Exists(string name);
        ProductionLineDataObject UpdateOperation(ProductionLineDataObject productionLine);
    }
}
