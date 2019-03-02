using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface ICompanyService : IService<CompanyDataObject>
    {
        CompanyDataObject GetCompanyByQRCode(string qrCode);
        bool Exists(string name);
        IList<string> GetShortCode();
    }
}
