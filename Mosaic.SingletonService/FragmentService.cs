using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mosaic.Domain.Models;
using Mosaic.Repositories.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mosaic.SingletonService
{
    public class FragmentService
    {
        private readonly ILogger<FragmentService> logger;
        private DbContextOptions options;

        public FragmentService(IConfiguration configuration, ILogger<FragmentService> logger)
        {
            this.logger = logger;
            var dbType = configuration.GetSection("DbType").Value;
            if (dbType.ToLower() == "sqlserver")
            {
                var connStr = configuration.GetConnectionString("SqlServer");
                this.options = new DbContextOptionsBuilder().UseSqlServer(connStr, p => p.UseRowNumberForPaging()).Options;
            }
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(this.handleFragment));
            thread.IsBackground = true;
            thread.Start();
        }

        private void handleFragment()
        {
            while (true)
            {
                Thread.Sleep(5000);
                using (MosaicContext context = new MosaicContext(this.options))
                {
                    var query = (from qrGroup in context.Set<Group>()
                                 join qrcode in context.Set<QRCode>() on qrGroup.ID equals qrcode.GID
                                 where !qrcode.Deleted
                                 group qrcode by new { qrcode.GID, qrGroup.ProductionLineID } into list
                                 where list.Count() <= 5 && list.Count() > 0
                                 select new GroupInfo
                                 {
                                     GID = list.Key.GID,
                                     ProductionLineID = list.Key.ProductionLineID,
                                     CID = list.First().CID,
                                     Count = list.Count(),
                                     EndRoot = list.OrderByDescending(p => p.Time).ThenByDescending(p => p.ID).First().EndRoot == true

                                 })
                                 .Where(p => p.EndRoot)
                                .ToList();
                    this.logger.LogInformation($"Found {query.Count} groups where member's count less than 5.{string.Join(",",query.Select(p=>p.GID))}");
                    foreach (var item in query)
                    {
                        this.process(item, context);
                    }
                }
            }
        }

        private void process(GroupInfo item, MosaicContext context)
        {
            this.logger.LogInformation($"Process group id is ： {item.GID}");
            if (!context.Set<QRCode>().Any(p => p.GID < item.GID && p.ProductionLineID == item.ProductionLineID && p.CID == item.CID))
                return;
            var qrcode = context.Set<QRCode>()
                                  .Where(p => p.GID < item.GID && p.CID == item.CID && p.ProductionLineID == item.ProductionLineID && !p.Deleted)
                                  .GroupBy(p => p.GID)
                                  .Select(p => new GroupInfo
                                  {
                                      GID = p.Key,
                                      ProductionLineID = item.ProductionLineID,
                                      CID = item.CID,
                                      Count = p.Count(),
                                      EndRoot = p.OrderByDescending(t => t.Time).ThenByDescending(t => t.ID).First().EndRoot
                                  })
                                  .OrderByDescending(p => p.GID)
                                  .First();
            this.logger.LogInformation($"The group id is {qrcode.GID},EndRoot is {qrcode.EndRoot} where id less than {item.GID} max one!");
            if (qrcode.EndRoot) 
                return;

            var list = context.Set<QRCode>().Where(p => !p.Deleted && p.GID == item.GID).ToList();
            foreach (var code in list)
            {
                code.GID = qrcode.GID;
                code.LastUpdateTime = DateTime.Now;
            }
            this.logger.LogInformation($"Modify Gid {item.GID} to {qrcode.GID}  Total {list.Count} items.");
            context.SaveChanges();
        }

        class GroupInfo
        {
            public int GID { get; set; }
            public int ProductionLineID { get; set; }
            public int CID { get; set; }
            public int Count { get; set; }
            public bool EndRoot { get; set; }
        }
    }
}
