using AutoMapper;
using DYFramework.Application;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class CarRFIDReceiverService : Service<CarRFIDReceiverDataObject, CarRFIDReceiver>, ICarRFIDReceiverService
    {
        public CarRFIDReceiverService(ICarRFIDReceiverRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public IList<RFIDUpdateDataObject> GetListByTime(GroupDataObject group)
        {
            IList<RFIDUpdateDataObject> rFIDUpdateList = new List<RFIDUpdateDataObject>();
            IList<CarRFIDReceiverDataObject> carList = Mapper.Map<IList<CarRFIDReceiver>, IList<CarRFIDReceiverDataObject>>(this.repository.Get(p => !p.Deleted && p.EnterTime > DateTime.Parse(group.CreateTime) && p.LeaveTime < DateTime.Parse(group.LastUpdateTime)).ToList());
            //IList<string> rfidList = carList.Select(p => p.RFIDList).ToList();
            //IList<GroupDataObject> groupList = Mapper.Map<IList<Group>, IList<GroupDataObject>>(this.repository.Context.Get<Group>(p => !p.Deleted && rfidList.Contains(p.RFID)).ToList());
            //foreach(GroupDataObject g in groupList)
            //{
            //g.QRCodeList = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Context.Get<QRCode>(p => p.GID == g.ID).ToList());
            //if (g.QRCodeList.Count > 0)
            //{
            //    g.Sum = g.QRCodeList.Count();
            //    g.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => !p.Deleted && p.ID == g.QRCodeList[g.QRCodeList.Count - 1].CID).FirstOrDefault());
            //} }
            foreach (CarRFIDReceiverDataObject car in carList)
            {
                RFIDUpdateDataObject rFIDUpdate = new RFIDUpdateDataObject();
                GroupDataObject g=Mapper.Map<Group,GroupDataObject>(this.repository.Context.Get<Group>(p => !p.Deleted && car.RFIDList.Contains(p.RFID)).FirstOrDefault());
                g.QRCodeList = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Context.Get<QRCode>(p => p.GID == g.ID).ToList());
                if (g.QRCodeList.Count > 0)
                {
                    g.Sum = g.QRCodeList.Count();
                    g.Category = Mapper.Map<Category, CategoryDataObject>(this.repository.Context.Get<Category>(p => !p.Deleted && p.ID == g.QRCodeList[g.QRCodeList.Count - 1].CID).FirstOrDefault());
                }
                rFIDUpdate.CarRFIDReceiverDataObject = car;
                rFIDUpdate.Category = g.Category;
                rFIDUpdate.FirstQR = g.QRCodeList.First();
                rFIDUpdate.EndQR = g.QRCodeList.Last();
                rFIDUpdate.FirstQRTime = DateTime.MinValue.AddTicks(g.QRCodeList.First().Time);
                rFIDUpdate.EndQRTime = DateTime.MinValue.AddTicks(g.QRCodeList.Last().Time);
                rFIDUpdate.QRSum = g.Sum;
                rFIDUpdateList.Add(rFIDUpdate);
            }
            return rFIDUpdateList;
        }
    }
}
