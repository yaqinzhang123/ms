using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYFramework.ServiceContract
{
    public interface IService<TDataObject> where TDataObject:DataObject
    {
        IList<TDataObject> GetList();
        TDataObject GetByID(int id);
        TDataObject Add(TDataObject dataObject);
        TDataObject Update(TDataObject dataObject);
        int RemoveByID(int id);
        int GetCount();
        bool Exists(int id);
    }
}
