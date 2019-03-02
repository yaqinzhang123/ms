using AutoMapper;

using Mosaic.Domain.Models;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic.Api
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //  Model to DTO
            CreateMap<Company, CompanyDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.SoftList, src => src.MapFrom(p => p.SoftList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.SoftList.Split(',', StringSplitOptions.None).ToList())); ;
            CreateMap<QRCode, QRCodeDataObject>()
                .ForMember(dest => dest.Time, src => src.MapFrom(p => p.Time.Ticks))
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<Group, GroupDataObject>()
                .ForMember(dest => dest.Time, src => src.MapFrom(p => p.Time.Ticks))
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<Category, CategoryDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<DeviceManage, DeviceManageDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<Operation, OperationDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<ProductionLine, ProductionLineDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<RFIDGroup, RFIDGroupDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.OldTime, src => src.MapFrom(p => p.OldTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.QRCodeList, src => src.MapFrom(p => p.QRCodeList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.QRCodeList.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => p.GroupNoList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.GroupNoList.Split(',', StringSplitOptions.None).ToList()));
            CreateMap<Rights, RightsDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<Role, RoleDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<UserInfo, UserInfoDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<UserRole, UserRoleDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<Agency, AgencyDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<Invoice, InvoiceDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.SubmitTime, src => src.MapFrom(p => p.SubmitTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => p.GroupNoList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.GroupNoList.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.CodeList, src => src.MapFrom(p => p.CodeList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.CodeList.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.ErrRFIDList, src => src.MapFrom(p => p.ErrRFIDList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.ErrRFIDList.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.ErrGroupNoList, src => src.MapFrom(p => p.ErrGroupNoList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.ErrGroupNoList.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.LastGroupNoList, src => src.MapFrom(p => p.LastGroupNoList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.LastGroupNoList.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.RemoveGroupNoList, src => src.MapFrom(p => p.RemoveGroupNoList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.RemoveGroupNoList.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.RemoveQRCodeList, src => src.MapFrom(p => p.RemoveQRCodeList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.RemoveQRCodeList.Split(',', StringSplitOptions.None).ToList()));
            CreateMap<InvoiceShipment, InvoiceShipmentDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => p.GroupNoList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.GroupNoList.Split(',', StringSplitOptions.None).ToList()));
            CreateMap<Module, ModuleDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<SoftWare, SoftWareDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<Invoice, InvoiceAndShipment>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<InvoiceShipment, InvoiceAndShipment>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<DYLog, DYLogDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<RFIDRecord, RFIDRecordDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<CarInfo, CarInfoDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<LocationLog, LocationLogDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<ControlInfo, ControlInfoDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<VirtualPrinterData, VirtualPrinterDataDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<RFIDBackup, RFIDBackupDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.CodeList, src => src.MapFrom(p => p.CodeList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.CodeList.Split(',', StringSplitOptions.None).ToList()));
            CreateMap<CarData, CarDataDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<CarNoForRFID, CarNoForRFIDDataObject>()
                .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<CarRFIDReceiver, CarRFIDReceiverDataObject>()
               .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
               .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<RelationRFIDQRCode, RelationRFIDQRCodeDataObject>()
               .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
               .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<InvoiceUserInfo, InvoiceUserInfoDataObject>()
               .ForMember(dest => dest.CreateTime, src => src.MapFrom(p => p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
               .ForMember(dest => dest.LastUpdateTime, src => src.MapFrom(p => p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss")))
               .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => p.GroupNoList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.GroupNoList.Split(',', StringSplitOptions.None).ToList()))
               .ForMember(dest => dest.CodeList, src => src.MapFrom(p => p.CodeList.Split(',', StringSplitOptions.None).ToList().Any(k => k == "") ? null : p.CodeList.Split(',', StringSplitOptions.None).ToList()));


            //  DTO to Model
            CreateMap<CompanyDataObject, Company>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore())
                .ForMember(dest => dest.SoftList, src => src.MapFrom(p => string.Join(",", p.SoftList)));
            CreateMap<QRCodeDataObject, QRCode>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                // .ForMember(dest => dest.Content,src => src.MapFrom(p=>p.Content.Replace("/r","").Trim()))
                .ForMember(dest => dest.Time, src => src.MapFrom(p => DateTime.MinValue.AddTicks(p.Time)))
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<GroupDataObject, Group>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.Time, src => src.MapFrom(p => DateTime.MinValue.AddTicks(p.Time)))
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<CategoryDataObject, Category>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<OperationDataObject, Operation>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<DeviceManageDataObject, DeviceManage>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<ProductionLineDataObject, ProductionLine>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<RFIDGroupDataObject, RFIDGroup>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore())
                .ForMember(dest => dest.OldTime, src => src.Ignore())
                .ForMember(dest => dest.QRCodeList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.QRCodeList)) ? null : string.Join(",", p.QRCodeList)))
                .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.GroupNoList)) ? null : string.Join(",", p.GroupNoList)));
            CreateMap<RightsDataObject, Rights>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<RoleDataObject, Role>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<UserInfoDataObject, UserInfo>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<UserRoleDataObject, UserRole>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<AgencyDataObject, Agency>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<InvoiceDataObject, Invoice>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore())
                .ForMember(dest => dest.SubmitTime, src => src.Ignore())
                .ForMember(dest => dest.ErrQRCodeList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.ErrQRCodeList)) ? null : string.Join(",", p.ErrQRCodeList)))
                .ForMember(dest => dest.LastGroupNoList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.LastGroupNoList)) ? null : string.Join(",", p.LastGroupNoList)))
                .ForMember(dest => dest.RemoveQRCodeList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.RemoveQRCodeList)) ? null : string.Join(",", p.RemoveQRCodeList)))
                .ForMember(dest => dest.RemoveGroupNoList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.RemoveGroupNoList)) ? null : string.Join(",", p.RemoveGroupNoList)))
                .ForMember(dest => dest.CodeList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.CodeList)) ? null : string.Join(",", p.CodeList)))
                .ForMember(dest => dest.ErrRFIDList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.ErrRFIDList)) ? null : string.Join(",", p.ErrRFIDList)))
                .ForMember(dest => dest.ErrGroupNoList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.ErrGroupNoList)) ? null : string.Join(",", p.ErrGroupNoList)))
                .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.GroupNoList))?null: string.Join(",", p.GroupNoList)));
            CreateMap<InvoiceShipmentDataObject, InvoiceShipment>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore())
                .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.GroupNoList)) ? null : string.Join(",", p.GroupNoList)));
            CreateMap<InvoiceAndShipment, Invoice>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<InvoiceAndShipment, InvoiceShipment>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<ModuleDataObject, Module>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<SoftWareDataObject, SoftWare>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<DYLogDataObject, DYLog>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<RFIDRecordDataObject, RFIDRecord>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<CarInfoDataObject, CarInfo>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<LocationLogDataObject, LocationLog>()
                .ForMember(dest=>dest.ID,src=>src.Ignore())
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<ControlInfoDataObject, ControlInfo>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<VirtualPrinterDataDataObject, VirtualPrinterData>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<RFIDBackupDataObject, RFIDBackup>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore())
                .ForMember(dest => dest.CodeList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.CodeList)) ? null : string.Join(",", p.CodeList)));
            CreateMap<CarNoForRFIDDataObject, CarNoForRFID>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<CarRFIDReceiverDataObject, CarRFIDReceiver>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<CarDataDataObject, CarData>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            CreateMap<RelationRFIDQRCodeDataObject, RelationRFIDQRCode>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.CreateTime, src => src.Ignore())
                .ForMember(dest => dest.LastUpdateTime, src => src.Ignore());
            //Transfer To Entity
            CreateMap<LocationLogTransfer, LocationLog>()
                .ForMember(dest => dest.ID, src => src.Ignore())
                .ForMember(dest => dest.Time, src => src.MapFrom(p => DateTime.MinValue.AddTicks(p.Time)));
            CreateMap<CarDataTransfer, CarData>()
                .ForMember(dest => dest.Enter, src => src.MapFrom(p => DateTime.MinValue.AddTicks(p.Enter)))
                .ForMember(dest => dest.Leave, src => src.MapFrom(p => DateTime.MinValue.AddTicks(p.Leave)));
            CreateMap<RFIDRecordTransfer, RFIDRecordDataObject>()
                .ForMember(dest=>dest.LineID,src=>src.MapFrom(p=>p.ProductionLineID))
                .ForMember(dest => dest.Time, src => src.MapFrom(p => DateTime.MinValue.AddTicks(p.Time)));

            CreateMap<InvoiceUserInfoDataObject, InvoiceUserInfo>()
               .ForMember(dest => dest.ID, src => src.Ignore())
               .ForMember(dest => dest.CreateTime, src => src.Ignore())
               .ForMember(dest => dest.LastUpdateTime, src => src.Ignore())
               .ForMember(dest => dest.CodeList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.CodeList)) ? null : string.Join(",", p.CodeList)))
               .ForMember(dest => dest.GroupNoList, src => src.MapFrom(p => String.IsNullOrWhiteSpace(string.Join(",", p.GroupNoList)) ? null : string.Join(",", p.GroupNoList)));

        }
    }
}
