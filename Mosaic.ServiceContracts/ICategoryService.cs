using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.ServiceContracts
{
    public interface ICategoryService : IService<CategoryDataObject>
    {
        IList<CategoryDataObject> Query(string content,int id);
        IList<CategoryDataObject> GetListByCompanyID(int id);
        bool Exists(string name);
        Task<int> Import(DataTable dt);
    }
}
