using DYFramework.Dao;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mosaic.Domain.Models;
using Mosaic.Repositories.Dao;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mosaic.SingletonService
{
    public class GroupingService
    {
        private string connStr;
        List<int> lineIdList = new List<int>();
        List<ProductionLine> productionLines = new List<ProductionLine>();
        private DbContextOptions options;

        private static object sync = new object();
        private readonly ILogger logger;

        public GroupingService(IConfiguration configuration,ILogger<GroupingService> logger)
        {
            this.connStr = configuration.GetConnectionString("SqlServer");
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(this.connStr, p => p.UseRowNumberForPaging());
            this.options = builder.Options;
            this.logger = logger;
        }

        public void Start()
        {

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    lock (this.lineIdList)
                    {
                        List<int> ids = new List<int>();
                        using (MosaicContext context = new MosaicContext(this.options))
                        {

                            ids = context.Set<ProductionLine>().Where(p => !p.Deleted).Select(p => p.ID).ToList();
                        }
                        this.logger.Log(LogLevel.Information, "Get the ProductionLine Configuration Information!");
                        var added = ids.Except(this.lineIdList).ToList();
                        this.lineIdList.Clear();
                        this.lineIdList.AddRange(ids);
                        foreach (int id in added)
                        {
                            this.logger.Log(LogLevel.Information, $"Start The ProductionLineID of {id}");
                            this.startGrouping(id);
                        }
                    }
                    Thread.Sleep(1000 * 60 * 10);
                }
            });
            thread.IsBackground = true;
            thread.Start();
            this.logger.Log(LogLevel.Information, "Start AutoGrouping!");

        }

        //public void Start()
        //{
        //    this.lineIdList.Add(1);
        //    startGrouping(1);
        //}

        private void startGrouping(int id)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(autoGrouping));
            thread.IsBackground = true;
            thread.Start(id);
        }

        private void autoGrouping(object id)
        {
            int lineId = int.Parse(id.ToString());
            while (true)
            {

                Thread.Sleep(5000);
                lock (this.lineIdList)
                {
                    if (!this.lineIdList.Contains(lineId))
                    {
                        this.logger.Log(LogLevel.Information, $"Stop The ProductionLineID of {lineId}");
                        break;
                    }
                }
                this.logger.Log(LogLevel.Information, $"preHandle: {lineId}");
                this.preHandle(lineId);
                //int cid=this.preHandle(lineId);
                //if (cid != 0)
                //{
                //    this.logger.Log(LogLevel.Information, $"manualHandle: {lineId}");
                //    this.manualHandle(lineId, cid);
                //}
                    
            }
        }


        /// <summary>
        /// Automatic Grouping
        /// </summary>
        /// <param name="lineId">Production Line ID</param>
        private void preHandle(int lineId)
        {
            Stopwatch watch = Stopwatch.StartNew();
            int cid = 0;
            DateTime now = DateTime.Now;
            using (MosaicContext context = new MosaicContext(this.options))
            {
                var firstUnlock = context.Set<QRCode>()
                                       .Where(p => !p.Deleted && !p.Lock&&p.ProductionLineID==lineId)
                                       .OrderBy(p => p.Time)
                                       .ThenBy(p=>p.ID)
                                       .FirstOrDefault();
                if (firstUnlock == null)
                    return;
                this.logger.Log(LogLevel.Information, $"First unlocked qrcode,productionLineID:{lineId},{firstUnlock.ID}");
                cid = firstUnlock.CID;
                var previous = context.Set<QRCode>().Where(p => !p.Deleted && p.Time < firstUnlock.Time&&p.ProductionLineID==lineId&&p.CID==firstUnlock.CID)
                                                  .OrderByDescending(p => p.Time)
                                                  .FirstOrDefault();
                //this.logger.Log(LogLevel.Information, $"Find previous qrcode before first unlock at productionline {lineId},{previous.ID}");
                List<QRCode> list = new List<QRCode>();
                if (previous == null)
                {
                    list = context.Set<QRCode>()
                                              .Where(p => p.Time >= firstUnlock.Time && !p.Deleted && p.ProductionLineID == lineId && p.CID == firstUnlock.CID)
                                              .ToList();
                    previous = firstUnlock;
                }
                this.logger.Log(LogLevel.Information, $"Find previous qrcode before first unlock at productionline {lineId},{previous.ID}");
                firstUnlock.GID = previous.GID;
                QRCode firstNodeInGroup = context.Set<QRCode>().Where(p => !p.Deleted && p.GID == previous.GID && p.ProductionLineID == lineId)
                                                              .OrderBy(p => p.Time)
                                                              .ThenBy(p => p.ID)
                                                              .FirstOrDefault();
                //}
                //else
                //{
                //    firstNodeInGroup = firstUnlock;
                //}
                //var firstNodeInGroup = context.Set<QRCode>().Where(p => !p.Deleted && p.GID == previous.GID&&p.ProductionLineID==lineId)
                //                                          .OrderBy(p => p.Time)
                //                                          .FirstOrDefault();
                this.logger.Log(LogLevel.Information, $"First node in group:{previous.GID},{firstNodeInGroup.ID}");

                list = context.Set<QRCode>().Where(p => !p.Deleted && p.Time >= firstNodeInGroup.Time&&p.ProductionLineID==lineId&&p.CID==firstNodeInGroup.CID)
                                              .OrderBy(p => p.Time)
                                              .ThenBy(p => p.ID)
                                              .ToList();
                this.logger.Log(LogLevel.Information, $"Curent unlocked qrcode:{lineId},{list.Count()}");
                foreach (var item in list)
                {
                    item.Lock = false;
                }
                manualHandle(list,lineId,cid,context);
                context.SaveChanges();
            }
            //return cid;
        }

        /// <summary>
        /// Manual Handle Qrcode Regrouping
        /// </summary>
        /// <param name="lineId">Production Line ID</param>
        private void manualHandle(List<QRCode> list,int lineId,int cid,MosaicContext context)
        {
            this.logger.Log(LogLevel.Information, $"Start manualHandle productionLineID: {lineId} , CID: {cid}");
            try
            {
                    if (list == null || list.Count <= 0)
                        return;
                    this.logger.Log(LogLevel.Information, $"Get all of unlock qrcode in productionline {lineId},list length:{list.Count()}");
                    var groupCounter = list.Where(p=>p.GID!=0).GroupBy(p => p.GID).ToDictionary(p => p.Key, k => 0);
                    var gidQueue = new Queue<int>(groupCounter.Keys.ToList());
                    this.logger.Log(LogLevel.Information, $"productionline {lineId},gidQueue length:{groupCounter.Keys.Count()}");
                    var gid = 0;
                    if(!gidQueue.TryDequeue(out gid))
                    {
                        gid =getGid(lineId);
                        groupCounter.Add(gid, 0);
                        this.logger.Log(LogLevel.Information, $"productionline {lineId},new group id:{gid}");
                    }
                    int processed = 0;
                    foreach(var qrCode in list)
                    {
                        processed++;
                        qrCode.Lock = true;

                        #region cancel startroot
                        //取消开始节点的判断
                        /*
                        if(qrCode.StartRoot&&qrCode.EndRoot&&(qrCode.AutoSkip||qrCode.ManualSkip))
                        {
                            this.logger.Log(LogLevel.Information, "This qrcode is a start and autoskip or manualskip node!");
                            if (!gidQueue.TryDequeue(out gid))
                            {
                                gid = getGid(lineId);
                                this.logger.Log(LogLevel.Information, $"StartRoot and Endroot.Gid={gid},ProductionLine={lineId},qrcodeid={qrCode.ID}");
                                groupCounter.Add(gid, 0);
                            }
                            continue;
                        }
                        
                        //如果上一分组的Endroot为true,则执行完该条语句将会在数据库中产生一条不关联二维码的分组记录。
                        if (qrCode.StartRoot)
                        {
                            this.logger.Log(LogLevel.Information, $"This qrcode id {qrCode.ID} is a start node.");
                            if (!gidQueue.TryDequeue(out gid))
                            {
                                gid = getGid(lineId);
                                this.logger.Log(LogLevel.Information, $"StartRoot.  Gid={gid},ProductionLine={lineId},qrcodeid={qrCode.ID}");
                                groupCounter.Add(gid, 0);
                            }
                        }
                        */
                        #endregion

                        qrCode.GID = gid;
                        groupCounter[gid]++;

                        if (qrCode.AutoSkip || qrCode.ManualSkip)
                        {
                            this.logger.Log(LogLevel.Information, $"autoskip or manualskip node,{qrCode.ID}");
                            groupCounter[gid]--;
                        }


                        if(qrCode.EndRoot&&processed<list.Count)
                        {
                            this.logger.Log(LogLevel.Information, $"end node,{qrCode.ID}");
                            if (!gidQueue.TryDequeue(out gid))
                            {
                                gid = getGid(lineId);
                                this.logger.Log(LogLevel.Information, $"Endroot. Gid={gid},ProductionLine={lineId},qrcodeid={qrCode.ID}");
                                groupCounter.Add(gid, 0);
                            }
                            continue;
                        }

                        #region cancel virtualadd
                        /*
                         * 取消虚增的判断
                         * groupCounter[gid] += qrCode.VirtualAdd;
                         */
                        #endregion

                        var operation = context.Set<Operation>().Find(qrCode.OperationID);
                        var standard = operation.GroupingMethod == 0 ? operation.Sum : operation.Weight / operation.Rule;

                        if (processed < list.Count && groupCounter[gid] >= standard)
                        {
                            this.logger.Log(LogLevel.Information, $"Qrcodeid={qrCode.ID},the group with id {gid} has full,The quantity is :{groupCounter[gid]},");
                            if (!gidQueue.TryDequeue(out gid))
                            {
                                gid = getGid(lineId);
                                this.logger.Log(LogLevel.Information, $"Group is full.New Gid={gid},ProductionLine={lineId},after end qrcodeid={qrCode.ID}");
                                groupCounter.Add(gid, 0);
                            }
                        }


                    }          
            }
            catch(Exception ex)
            {
                this.logger.Log(LogLevel.Error, ex.ToString());
            }

            #region the code before 20181126
            /*
            try
            {
                Stopwatch watch = Stopwatch.StartNew();
                using (MosaicContext context = new MosaicContext(this.options))
                {
                    var qRCode = context.Set<QRCode>()
                                    .Where(p => !p.Deleted && !p.Lock && p.ProductionLineID == lineId)
                                    .OrderBy(p => p.Time)
                                    .FirstOrDefault();
                    if (qRCode == null)
                        return;

                    var lastGroupID = context.Set<QRCode>()
                                    .Where(p => !p.Deleted && p.CID == qRCode.CID && p.ProductionLineID == lineId && p.Time<=qRCode.Time)
                                    .Max(p => p.GID);


                    var beginTime = context.Set<QRCode>()
                                    .Where(p => p.GID == lastGroupID && !p.Deleted)
                                    .Min(p => p.Time);

                     var qrcodeList = context.Set<QRCode>()
                                        .Where(p => !p.Deleted&&p.ProductionLineID==lineId&&p.Time>=beginTime)
                                        .ToList();

                    var qrcodeDict = qrcodeList.GroupBy(p => p.CID).ToDictionary(p => p.Key, k =>k.OrderBy(t=>t.Time).ToList());

                    foreach (var cid in qrcodeDict.Keys)
                    {
                        var list = qrcodeDict[cid];
                        var gidDict = list.Where(p => p.GID != 0).GroupBy(p => p.GID).OrderBy(p => p.Key).ToDictionary(p => p.Key, k => 0);
                        var gidQ = new Queue<int>(list.Where(p => p.GID != 0).GroupBy(p => p.GID).OrderBy(p => p.Key).Select(p => p.Key));
                        int operationID = list[0].OperationID;
                        Operation operation = context.Set<Operation>().Find(operationID);
                        List<int> lockedGid = new List<int>();
                        int gid = 0;
                        if (!gidQ.TryDequeue(out gid))
                        {
                            gid = this.getGid(gid,lineId);
                            gidDict.Add(gid, 0);
                        }
                        int index = 0;
                        foreach (var code in list)
                        {

                            if (code.StartRoot&&index!=0)
                            {
                                lockedGid.Add(gid);
                                if (!gidQ.TryDequeue(out gid))
                                {
                                    gid = getGid(gid,lineId);
                                    gidDict.Add(gid, 0);
                                }
                            }

                            if (code.VirtualAdd > 0)
                            {
                                gidDict[gid] += code.VirtualAdd;
                            }


                            if (code.AutoSkip || code.ManualSkip)
                            {
                                gidDict[gid]--;
                            }
                            if (gid == 0)
                            {
                                if (!gidQ.TryDequeue(out gid))
                                {
                                    gid = this.getGid(gid,lineId);
                                    gidDict.Add(gid, 0);
                                }
                            }

                            code.GID = gid;
                            gidDict[gid]++;

                            if (code.EndRoot)
                            {
                                lockedGid.Add(gid);
                                if (!gidQ.TryDequeue(out gid))
                                {
                                    gid = this.getGid(gid,lineId);
                                    if (!gidDict.ContainsKey(gid))
                                        gidDict.Add(gid, 0);
                                    else
                                        gidDict[gid]++;
                                }
                            }

                            operation = context.Set<Operation>().Find(code.OperationID);
                            var pageSize = operation?.GroupingMethod == 0 ? operation.Sum : operation.Weight / operation.Rule;
                            if (gidDict[code.GID] == pageSize)
                            {
                                lockedGid.Add(gid);
                                if (!gidQ.TryDequeue(out gid))
                                {
                                    gid = this.getGid(gid,lineId);
                                    if (!gidDict.ContainsKey(gid))
                                        gidDict.Add(gid, 0);
                                    else
                                        gidDict[gid]++;
                                }
                            }
                        }
                        var qrList = list.Where(p => lockedGid.Contains(p.GID)).ToList();
                        foreach (var item in qrList)
                        {
                            item.Lock = true;
                        }
                        int n = context.SaveChanges();
                        //var strList = gidDict.Select(p => "{ Gid : "+ p.Key.ToString() +", Count : "+p.Value.ToString()+"}").ToList();
                        index++;
                        watch.Stop();
                        if (n > 0)
                            Console.WriteLine($"Production Line {lineId} Process {n} Items Completed! Duration:{watch.Elapsed}");
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Production Line {lineId}  Exception: {ex.ToString()}");
            }
            */
            #endregion
        }

        private int getGid(int lineId)
        {
            lock (sync)
            {
                DateTime now = DateTime.Now;
                using (MosaicContext context = new MosaicContext(this.options))
                {

                    Group group = new Group();
                    group.ProductionLineID = lineId;
                    group.Time = now;
                    group.CreateTime = now;
                    group.LastUpdateTime = now;
                    context.Set<Group>().Add(group);
                    context.SaveChanges();
                    return group.ID;
                }
            }
        }
    }
}
