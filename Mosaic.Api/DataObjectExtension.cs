using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mosaic.Api
{
    public static class DataObjectExtension
    {
        public static string ToJson(this DataObject dataObject)
        {
            PropertyInfo[] properties = dataObject.GetType().GetProperties();
            List<string> list = new List<string>();
            foreach(var item in properties)
            {
                list.Add($"{item.Name}:{item.GetValue(dataObject)}");
            }
            return "{" + string.Join(",", list) + "}";
        }
    }
}
