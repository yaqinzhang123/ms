using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IDYLogService : IService<DYLogDataObject>
    {
        bool ExistsMemo(string memo);
    }
}
