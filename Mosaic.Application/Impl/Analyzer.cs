using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class Analyzer : IAnalyzer
    {
        private static int QUEUE_IN_MINUTES = 20;//队列中保存的20分钟数据
        private static int SAMPLING_PERIOD = 3;//每3秒取一次数据

        private Dictionary<string, Queue<RFIDRecordDataObject>> activeRFIDDict = new Dictionary<string, Queue<RFIDRecordDataObject>>();

        public int LineID { get; }

        public event SelectedRFID OnSelectedRFID;

        public Analyzer(int lineID)
        {
            LineID = lineID;
        }

        public void AddRecord(RFIDRecordDataObject record)
        {
            string rfid = record.RFID;
            if (activeRFIDDict.ContainsKey(rfid))
            {
                activeRFIDDict[rfid].Enqueue(record);
            }
            else
            {
                int len = 60 * QUEUE_IN_MINUTES / SAMPLING_PERIOD;
                Queue<RFIDRecordDataObject> queue = new Queue<RFIDRecordDataObject>(len);
                queue.Enqueue(record);
                activeRFIDDict.Add(rfid, queue);
            }
        }

        //public void LifterTrigger(CarInfoDataObject carInfo)
        //{
        //    List<string> rfids = new List<string>();
        //    foreach(var item in this.activeRFIDDict)
        //    {
        //        int cnt = 0;
        //        foreach(var record in item.Value)
        //        {
        //            if (record.Between(carInfo.Enter, carInfo.Leave))
        //            {
        //                cnt++;
        //            }

        //        }
        //        int len = item.Value.Count;
        //        Console.WriteLine($"cnt:{cnt}  len:{len},   key:{item.Key}");
        //        if (cnt >= 3 && item.Value.Last().BeforeExtendedTime(carInfo.Leave))
        //        {
        //            rfids.Add(item.Key);
        //        }
        //    }

        //    foreach(string item in rfids)
        //    {
        //        this.OnSelectedRFID?.Invoke(new RFIDRecordDataObject() { LineID = this.LineID, RFID = item },carInfo);
        //        Console.WriteLine($"RFID Tag Select: {item}");
        //        this.activeRFIDDict.Remove(item);
        //    }

        //}

        public List<string> RFIDTag()
        {
            throw new NotImplementedException();
        }

        public void ShowRFIDInfo()
        {
            foreach(var item in this.activeRFIDDict)
            {
                Console.WriteLine($"{item.Key}:{item.Value.Count}");
            }
        }
    }
}
