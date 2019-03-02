
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.Repositories.Dao;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mosaic.Application.Impl
{
    public class RFIDService: IRFIDService
    {
        //private readonly IRFIDRecordService rfidRecordService;
        //private readonly IMapper mapper;
        //private readonly IGroupService groupService;
        //private readonly ILocationLogService locationLogService;
        //private readonly ICarInfoService carInfoService;

        //public RFIDService(IRFIDRecordService rfidRecordService,IMapper mapper,IGroupService groupService,
        //    ILocationLogService locationLogService,ICarInfoService carInfoService)
        //{
        //    this.rfidRecordService = rfidRecordService;
        //    this.mapper = mapper;
        //    this.groupService = groupService;
        //    this.locationLogService = locationLogService;
        //    this.carInfoService = carInfoService;
        //}
        public RFIDService(IConfiguration configuration)
        {
            this.connStr = configuration.GetConnectionString("SqlServer");
        }
        
        
        private Dictionary<int, bool> threadStatus = new Dictionary<int, bool>();
        private string connStr;

        public void Start()
        {
            Thread thread = new Thread(() =>
              {
                  while (true)
                  {
                      Thread.Sleep(1000);
                      List<int> productionLineIDList = this.getProductionLineIDList();
                      foreach(int lineId in productionLineIDList)
                      {
                          if (!this.threadStatus.ContainsKey(lineId))
                          {
                              threadStatus.Add(lineId, false);
                          }
                          else if (this.threadStatus[lineId])
                          {
                              continue;
                          }

                         // this.startProductionLineThread(lineId);
                          
                      }
                  }
              });
            thread.IsBackground = true;
            thread.Start();
            
        }

        private List<int> getProductionLineIDList()
        {
            using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
                .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
            {
               return context.Set<RFIDRecord>().GroupBy(p => p.LineID).Select(p => p.Key).ToList();  
            }
        }

        //private void startProductionLineThread(int lineId)
        //{
        //    Thread thread = new Thread(() =>
        //    {
        //        Queue<CarInfoDataObject> carInfoQueue = new Queue<CarInfoDataObject>(20);
        //        var list = this.getCarInfoListByLineId(lineId,20);
        //        foreach(var carinfo in list)
        //        {
        //            carInfoQueue.Enqueue(carinfo);
        //        }
        //        this.threadStatus[lineId] = true;
        //        Thread.Sleep(2000);
        //        IAnalyzer analyzer = new Analyzer(lineId);
        //        analyzer.OnSelectedRFID += Analyzer_OnSelectedRFID;
        //        IList<RFIDRecordDataObject> rfidList = this.getUnhandleListByProduction(lineId);
        //        CarInfoDataObject carInfo = this.nextAction(carInfoQueue);

        //        DateTime previousTs = DateTime.MinValue;
        //        foreach (var record in rfidList)
        //        {
        //            DateTime readTime = record.Time;
        //            if (carInfo != null && carInfo.TimeIsUp(readTime, 90))
        //            {
        //                analyzer.LifterTrigger(carInfo);
        //                if (readTime != previousTs)
        //                {
        //                    carInfo = this.nextAction(carInfoQueue);
        //                    previousTs = readTime;
        //                }
        //                Thread.Sleep(1000);
        //            }
        //            analyzer.AddRecord(record);
        //            //record.Flag = true;
        //            this.rfidRecordUpdate(record);
        //        }
        //        analyzer.ShowRFIDInfo();


        //        this.threadStatus[lineId] = false;
        //    });
        //    thread.IsBackground = true;
        //    thread.Start();
        //}

        private void rfidRecordUpdate(RFIDRecordDataObject record)
        {
            using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
              .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
            {
                var rec = context.Set<RFIDRecord>().Find(record.ID);
                rec.Flag = true;
                context.SaveChanges();
                return;
            }
        }

        //private IList<CarInfoDataObject> getCarInfoListByLineId(int lineId, int v)
        //{
        //    using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
        //      .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
        //    {
        //        var list = context.Set<CarInfo>().Where(p => !p.Flag && p.ProductionLineID == lineId)
        //                            .OrderBy(p => p.Enter).Take(v).ToList();
        //        return Mapper.Map<IList<CarInfo>, IList<CarInfoDataObject>>(list);
        //    }
        //}

        private IList<RFIDRecordDataObject> getUnhandleListByProduction(int lineId)
        {
            using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
                .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
            {
                var list = context.Set<RFIDRecord>().Where(p => !p.Flag && p.LineID == lineId).ToList();
                return Mapper.Map<IList<RFIDRecord>, IList<RFIDRecordDataObject>>(list);
            }
        }

        //private void Analyzer_OnSelectedRFID(RFIDRecordDataObject rfid,CarInfoDataObject carInfo)
        //{   using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
        //        .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
        //    {
        //        int i = 0;
        //        LocationLogDataObject locationLog = this.lastLocation(rfid.LineID, carInfo.Leave);
        //        if (locationLog.Name.Trim() == carInfo.Location.Trim())
        //            i = 1;
        //        this.groupUpdateRFID(rfid, i);
        //        //carInfo.Flag = true;
        //        this.carInfoUpdate(carInfo);
        //    }
        //}

        private void carInfoUpdate(CarInfoDataObject carInfo)
        {
            using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
              .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
            {
                var rec = context.Set<CarInfo>().Find(carInfo.ID);
                rec.Flag = true;
                context.SaveChanges();
                return;
            }
        }

        private void groupUpdateRFID(RFIDRecordDataObject rfid, int i)
        {
            using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
             .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
            {
                IList<Group> groupList = context.Set<Group>().Where(p => p.ProductionLineID == rfid.LineID && string.IsNullOrWhiteSpace(p.RFID)).OrderBy(p => p.LastUpdateTime).ToList();
                if (groupList.Count <= 0)
                {
                    Group newGroup = new Group();
                    newGroup.ProductionLineID = rfid.LineID;
                    newGroup.RFID = rfid.RFID;
                    newGroup.Time = DateTime.Now;
                    context.Set<Group>().Add(newGroup);
                    context.SaveChanges();
                    IList<QRCode> qrcodelist = context.Set<QRCode>().Where(p => p.ProductionLineID == rfid.LineID && p.GID == 0).OrderBy(p => p.Time).ToList();
                    int groupid = newGroup.ID;
                    for (int k = 0; k < qrcodelist.Count(); k++)
                    {
                        var qrcode = context.Set<QRCode>().Find(qrcodelist[k].ID);
                        qrcodelist[k].GID = groupid;
                        context.SaveChanges();
                    }
                    return;
                }
                Group group = context.Set<Group>().Find(groupList[i].ID);
                group.RFID = rfid.RFID;
                group.Time = DateTime.Now;
                context.SaveChanges();
            }
        }
        private LocationLogDataObject lastLocation(int lineID, DateTime leave)
        {
            using (MosaicContext context = new MosaicContext(new DbContextOptionsBuilder()
               .UseSqlServer(this.connStr, p => p.UseRowNumberForPaging()).Options))
            {
                var location=context.Set<LocationLog>().Where(p => p.Time < leave && p.ProductionLineID == lineID).FirstOrDefault();
                return Mapper.Map<LocationLog, LocationLogDataObject> (location);
            }
            }

        private CarInfoDataObject nextAction(Queue<CarInfoDataObject> carInfoQueue)
        {
            carInfoQueue.TryDequeue(out var carInfo);
            return carInfo;

        }
    }
}
