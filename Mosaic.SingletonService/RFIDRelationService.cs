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
    public class RFIDRelationService
    {
        private string connStr;
        private DbContextOptions options;
        private readonly ILogger<RFIDRelationService> logger;
        private DateTime date = DateTime.MinValue;
        private List<RFIDInfo> rfidList;

        public RFIDRelationService(IConfiguration configuration,ILogger<RFIDRelationService> logger)
        {
            this.connStr = configuration.GetConnectionString("SqlServer");
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(this.connStr, p => p.UseRowNumberForPaging());
            this.options = builder.Options;
            this.logger = logger;
        }
        public void Start()
        {
            this.startRelation();
        }

        private void startRelation()
        {
            Thread thread = new Thread(new ThreadStart(autoRelation));
            thread.IsBackground = true;
            thread.Start();
        }

        private void autoRelation()
        {
            while (true)
            {
                Thread.Sleep(5000);
                this.relationRFID();
            }
        }

        private void relationRFID()
        {
            //this.logger.LogInformation($"Production Line {lineID} Relation.");
            try
            {
                using (MosaicContext context = new MosaicContext(this.options))
                {

                    var groupList = (from qrCodeGroup in context.Set<Group>()
                                     join qrcode in context.Set<QRCode>() on qrCodeGroup.ID equals qrcode.GID
                                     where string.IsNullOrWhiteSpace(qrCodeGroup.RFID)&&!qrCodeGroup.Deleted&&!qrcode.Deleted&&qrcode.CID!=40&&qrcode.Time.Date>new DateTime(2018,12,2)
                                     group new { qrCodeGroup,qrcode } by new { qrCodeGroup.ID,qrCodeGroup.ProductionLineID } into qrcodeList
                                     select new  GroupInfo
                                     {
                                         GID=qrcodeList.Key.ID,
                                         ProductionLineID=qrcodeList.Key.ProductionLineID,
                                         Count=qrcodeList.Count(),
                                         FirstTime=qrcodeList.Min(p=>p.qrcode.Time),
                                         LastTime=qrcodeList.Max(p=>p.qrcode.Time)
                                     })
                                    .ToList();
                    this.logger.LogInformation($"GroupList Count : {groupList.Count}");
                    var groupDict = new Dictionary<int, string>();
                    foreach (var item in groupList)
                    {
                        var rfid = getRFID(item,context);
                        groupDict.Add(item.GID, rfid);
                    }
                    var list = context.Set<Group>().Where(p => groupDict.Keys.Contains(p.ID)).ToList();
                    foreach(var item in groupDict)
                    {
                        var qrGroup = list.Where(p => p.ID == item.Key).FirstOrDefault();
                        qrGroup.RFID = item.Value;
                    }
                    context.SaveChanges();
                                    

                }
            }catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }

        private string getRFID(GroupInfo groupInfo, MosaicContext context)
        {
            if(this.date!=groupInfo.LastTime.Date)
            {
                this.date = groupInfo.LastTime.Date;
                this.rfidList = this.getRFIDList(context);
            }
            this.logger.LogInformation(groupInfo.ToString());

            var rfidFiltered = this.rfidList.
                               Where(p => p.ProductionLineID==groupInfo.ProductionLineID &&p.LastTime < groupInfo.FirstTime&&p.LastTime>groupInfo.FirstTime.AddMinutes(-6))
                               .ToList();
            
            this.logger.LogInformation($"GID : {groupInfo.GID} ,RFIDList Count : {rfidFiltered.Count}");
            var location = context.Set<LocationLog>()
                                    .Where(p => p.Time >= groupInfo.FirstTime.AddSeconds(5)
                                               && p.Time <= groupInfo.LastTime.AddSeconds(5)
                                               && p.ProductionLineID == groupInfo.ProductionLineID)
                                    .GroupBy(p => p.Name)
                                    .Select(p =>
                                    new
                                    {
                                        Name = p.Key,
                                        groupInfo.ProductionLineID,
                                        Count = p.Count(),
                                        FirstTime = p.Min(k => k.Time),
                                        LastTime = p.Max(k => k.Time)
                                    })
                                    .OrderByDescending(p => p.Count)
                                    .FirstOrDefault();

            var query = rfidFiltered.Where(p => p.Location == location.Name)
                                    .OrderByDescending(p => p.Times)
                                    .FirstOrDefault();

            this.logger.LogInformation($"GID : {groupInfo.GID} ,Location Count : {location.Count}");
            if (query == null )
                return "N/A";
            return query?.RFID;
                                
        }

        private List<RFIDInfo> getRFIDList(MosaicContext context)
        {
            var list = context.Set<RFIDRecord>().Where(p => p.Time.Date == this.date).ToList();
            #region filter diffrence location in same productionline
            //var sameRFIDonLine = list.GroupBy(p => new { p.RFID, p.LineID })
            //                         .Select(p => new
            //                         {
            //                             p.Key.RFID,
            //                             p.Key.LineID,
            //                             Count=p.GroupBy(t=>t.Location).Count()
            //                         })
            //                         .Where(p=>p.Count>1)
            //                         .ToList();

            //foreach(var item in sameRFIDonLine)
            //{
            //    list.RemoveAll(p => p.RFID == item.RFID && p.LineID == item.LineID && p.Location == "B");
            //}
            #endregion

            var temp = list.GroupBy(p => p.RFID)
                         .Select(p => new
                         {
                             RFID=p.Key,
                             LineCount=p.GroupBy(k=>k.LineID).Count(),
                             LineID=p.GroupBy(k=>k.LineID)
                                     .Select(t=>new { LineID=t.Key, Times=t.Sum(y => y.Times) })
                                     .OrderByDescending(k=>k.Times)
                                     .FirstOrDefault()?.LineID
                         })
                         .Where(p=>p.LineCount>1)
                         .ToList();

            foreach(var item in temp)
            {
                list.RemoveAll(p => p.RFID == item.RFID && p.LineID != item.LineID);
            }

            var lineRfidList = list .GroupBy(p=>new { p.RFID,p.LineID,p.Location })
                                    .Select(p => new RFIDInfo
                                   {
                                        RFID=p.Key.RFID,
                                        Location=p.Key.Location,
                                        ProductionLineID=p.Key.LineID,
                                        LastTime=p.Max(t=>t.Time),
                                        Times=p.Sum(k=>k.Times)
                                   })
                                   .ToList();
            var recycle = new string[]  { "E200001A350E012422605D96", "E2000019570F01920860A590", "E200001A920F02721230EDA0" };
            lineRfidList.RemoveAll(p => recycle.Contains(p.RFID));

            return lineRfidList;
        }
    }
    public class GroupInfo
    {
        public int GID { get; set; }
        public int ProductionLineID { get; set; }
        public int Count { get; set; }        
        public DateTime FirstTime { get; set; }
        public DateTime LastTime { get; set; }

        public override string ToString()
        {
            Type type = this.GetType();
            var properties = type.GetProperties();
            var result = new List<string>();
            foreach(var property in properties)
            {
                result.Add($"'{property.Name}:'{property.GetValue(this)}'");
            }
            return string.Join(",", result);
        }
    }
    public class RFIDInfo
    {
        public string RFID { get; set; }
        public int Times { get; set; }
        public int ProductionLineID { get; set; }
        public DateTime LastTime { get; set; }
        public string Location { get; set; }
    }
}
