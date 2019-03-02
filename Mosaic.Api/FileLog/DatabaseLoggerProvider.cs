//using Microsoft.Extensions.Logging;
//using Mosaic.Domain.Repository;

//namespace Mosaic.Api.FileLog
//{
//    internal class DatabaseLoggerProvider : ILoggerProvider
//    {
//        private string connStr;

//        public DatabaseLoggerProvider(string connStr)
//        {
//            this.connStr = connStr;
//        }

//        public ILogger CreateLogger(string categoryName)
//        {
//            DatabaseLogger logger = new DatabaseLogger(categoryName);
//            logger.ConnStr = this.connStr;

//            return logger;
//        }

//        public void Dispose()
//        {
            
//        }
//    }
//}