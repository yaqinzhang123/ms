//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.IO;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Mosaic.Domain.Repository;

//namespace Mosaic.Api.FileLog
//{
//    public static class LoggerExtensions
//    {
//        public static ILoggerFactory AddDatabase(this ILoggerFactory factory,string connStr)
//        {
//             factory.AddProvider(new DatabaseLoggerProvider(connStr));
//            return factory;
//        }
//    }
//}
