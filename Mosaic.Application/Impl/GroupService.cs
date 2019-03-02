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
    public class GroupService : Service<GroupDataObject, Group>, IGroupService
    {
        public GroupService(IGroupRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public void AddGroup(int sum, QRCodeDataObject dataObject)
        {
            //产线上设置的分包数量
            //int sum = this.repository.Context.Get<Operation>(p => p.ProductionLineID == dataObject.ProductionLineID).OrderByDescending(p=>p.LastUpdateTime).FirstOrDefault().Sum;
            IList<QRCode> qrcodelist = this.repository.Context.Get<QRCode>(k => k.ProductionLineID == dataObject.ProductionLineID && k.GID == 0).OrderBy(p => p.Time).Take(sum).ToList();
            Group group = this.repository.Create();
            group.ProductionLineID = dataObject.ProductionLineID;
            this.repository.Add(group);
            this.repository.Commit();
            for (int i = 0; i < qrcodelist.Count(); i++)
            {
                qrcodelist[i].GID = group.ID;
                this.repository.Context.Update<QRCode>(qrcodelist[i]);
            }
            this.repository.Commit();
        }
        ////更新分组
        //public void UpdateGroup(int GID, QRCodeDataObject dataObject)
        //{
        //    //看离此二维码最近的之后有多少个分组
        //    IList<Group> groupList = this.repository.Get(p => p.ID >= GID).OrderBy(p=>p.ID).ToList();
        //    QRCode qRCode = Mapper.Map<QRCodeDataObject, QRCode>(dataObject);
        //    //把二维码数据增到组list中
        //    qRCode.GID = GID;
        //    this.repository.Context.Update<QRCode>(qRCode);
        //    this.repository.Commit();
        //    for (int i = 0; i < groupList.Count(); i++)
        //    {
        //        QRCode qR = this.repository.Context.Get<QRCode>(p=>p.GID==groupList[i].ID).OrderByDescending(p => p.Time).First();
        //        if (i == groupList.Count() - 1)
        //        {
        //            qR.GID =0;
        //        }
        //        else
        //        {
        //            qR.GID = groupList[i+1].ID;
        //        }
        //        this.repository.Context.Update<QRCode>(qR);
        //        this.repository.Context.Commit();
        //    }
        //    this.repository.Commit();
        //}

        //读取天线加rfid ,若最后分组不到分组数量，把新建分组，把产线上全部剩余个数分为一组，加上RFID
        public override GroupDataObject Update(GroupDataObject dataObject)
        {
            Group group = this.repository.Context.GetUpdateEntity<Group>().Where(p => p.ProductionLineID == dataObject.ProductionLineID && p.RFID == null).FirstOrDefault();
            if (group == null || group.ID == 0)
            {
                Group newGroup = this.repository.Create();
                newGroup.ProductionLineID = dataObject.ProductionLineID;
                newGroup.RFID = dataObject.RFID;
                newGroup.Time = DateTime.Now;
                this.repository.Context.Add<Group>(newGroup);
                this.repository.Context.Commit();
                IList<QRCode> qrcodelist = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => p.ProductionLineID == dataObject.ProductionLineID && p.GID == 0).OrderBy(p => p.Time).ToList();
                int groupid = newGroup.ID;
                for (int i = 0; i < qrcodelist.Count(); i++)
                {
                    qrcodelist[i].GID = groupid;
                    this.repository.Context.Update(qrcodelist[i]);
                    this.repository.Context.Commit();
                }
                return mapper.Map<Group, GroupDataObject>(newGroup);
            }
            else
            {
                group.RFID = dataObject.RFID;
                this.repository.Update(group);
                this.repository.Commit();
                return mapper.Map<Group, GroupDataObject>(group);
            }
        }



        //通过二维码查看公司及发货单信息
        public GroupDataObject GetGroupByQRCode(string qrcode)
        {
            int groupID = this.repository.Context.Get<QRCode>(p => !p.Deleted&&p.Content == qrcode.Trim()).FirstOrDefault().GID;
            if (groupID == 0)
                return new GroupDataObject();
            return this.GetByID(groupID);
        }
        public override GroupDataObject GetByID(int id)
        {
            GroupDataObject group= base.GetByID(id);
            group.QRCodeList = Mapper.Map<IList<QRCode>, IList<QRCodeDataObject>>(this.repository.Context.Get<QRCode>(p=>p.GID==id&&!p.Deleted).ToList());
            return group;
        }

        public void UpdateRFID(RFIDRecordDataObject rfid, int i)
        {
            IList<Group> groupList = this.repository.Context.GetUpdateEntity<Group>().Where(p => p.ProductionLineID == rfid.LineID && string.IsNullOrWhiteSpace(p.RFID)).OrderBy(p=>p.LastUpdateTime).ToList();
            if (groupList.Count <= 0)
            {
                Group newGroup = this.repository.Create();
                newGroup.ProductionLineID = rfid.LineID;
                newGroup.RFID = rfid.RFID;
                newGroup.Time = DateTime.Now;
                this.repository.Context.Add<Group>(newGroup);
                this.repository.Context.Commit();
                IList<QRCode> qrcodelist = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => p.ProductionLineID == rfid.LineID && p.GID == 0).OrderBy(p => p.Time).ToList();
                int groupid = newGroup.ID;
                for (int k = 0; k < qrcodelist.Count(); k++)
                {
                    qrcodelist[k].GID = groupid;
                    this.repository.Context.Update(qrcodelist[k]);
                    this.repository.Context.Commit();
                }
                return;
            }
            Group group = groupList[i];
            group.RFID = rfid.RFID;
            group.Time = DateTime.Now;
            this.repository.Update(group);
            this.repository.Commit();
        }

        public bool ManualAddGroup(GroupDataObject dataObject)
        {
            Group group = this.repository.Context.GetUpdateEntity<Group>().Where(p => p.ProductionLineID == dataObject.ProductionLineID && p.RFID == null).FirstOrDefault();
            if (group == null || group.ID == 0)
            {
                Group newGroup = this.repository.Create();
                newGroup.ProductionLineID = dataObject.ProductionLineID;
                newGroup.Time = DateTime.Now;
                this.repository.Context.Add<Group>(newGroup);
                this.repository.Context.Commit();
                IList<QRCode> qrcodelist = this.repository.Context.GetUpdateEntity<QRCode>().Where(p => p.ProductionLineID == dataObject.ProductionLineID && p.GID == 0).OrderBy(p => p.Time).ToList();
                int groupid = newGroup.ID;
                for (int i = 0; i < qrcodelist.Count(); i++)
                {
                    qrcodelist[i].GID = groupid;
                    this.repository.Context.Update(qrcodelist[i]);
                    this.repository.Context.Commit();
                }
                return true;
            }
            else
            {
                //group.RFID = dataObject.RFID;
                //newGroup.ProductionLineID = dataObject.ProductionLineID;
                this.repository.Update(group);
                this.repository.Commit();
                return true;
            } 
        }
        //不用
        public int UpdateRFIDList(CarRFIDReceiverDataObject car)
        {
            IList<Group> groupList = this.repository.Context.GetUpdateEntity<Group>().Where(p => p.ProductionLineID == car.ProductionLineID && string.IsNullOrWhiteSpace(p.RFID)).OrderBy(p => p.LastUpdateTime).ToList();
            if (groupList.Count() == 0)
                return 0;
            Group group = groupList[0];
            group.RFID = car.RFIDList;
            group.RFIDGroupNo = car.Location;
            group.Time = DateTime.Now;
            this.repository.Update(group);
            this.repository.Commit();
            return 1;
        }
    
    }
}