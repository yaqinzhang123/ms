
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using System.IO;
//using System.Text;
//using System.Threading;
//using Mosaic.Domain.Repository;
//using Mosaic.Domain.Models;
//using Mosaic.Repositories.Dao;
//using Microsoft.EntityFrameworkCore;

//namespace Mosaic.Api.FileLog
//{
//    public class DatabaseLogger : ILogger
//    {
//        public DatabaseLogger(string categoryName)
//        {
//            this.Name = categoryName;
//        }
//        class Disposable : IDisposable
//        {
//            public void Dispose()
//            {
//            }
//        }
//        Disposable _DisposableInstance = new Disposable();
//        public IDisposable BeginScope<TState>(TState state)
//        {
//            return _DisposableInstance;
//        }
//        public bool IsEnabled(LogLevel logLevel)
//        {
//            return true;
//        }

//        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
//        {
//            using (MosaicContext context = new MosaicContext((new DbContextOptionsBuilder()).UseSqlServer(this.ConnStr).Options))
//            {
//                var entity = context.Set<DYLog>();
//                string msg = formatter(state, exception);
//                DYLog log = new DYLog();
//                log.Memo = msg;
//                entity.Add(log);
//                context.SaveChanges();
//            }
//        }

//        public string Name { get; }
//        public string ConnStr { get; internal set; }
//    }
//}
    
