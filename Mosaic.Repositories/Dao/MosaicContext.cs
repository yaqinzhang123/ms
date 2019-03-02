using DYFramework.Dao;
using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Mosaic.Domain.Models;
using Mosaic.Repositories.TypeConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.Dao
{
    public class MosaicContext : DyContext
    {
        public MosaicContext(DbContextOptions options) : base(options)
        {
        }

        protected override void AddTypeConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyTypeConfiguration())
                .ApplyConfiguration(new CategoryTypeConfiguration())
                .ApplyConfiguration(new RightsTypeConfiguration())
                .ApplyConfiguration(new RoleTypeConfiguration())
                .ApplyConfiguration(new UserInfoTypeConfiguration())
                .ApplyConfiguration(new AgencyTypeConfiguration())
                .ApplyConfiguration(new InvoiceTypeConfiguration())
                .ApplyConfiguration(new QRCodeTypeConfiguration())
                .ApplyConfiguration(new ProductionLineTypeConfiguration())
                .ApplyConfiguration(new GroupTypeConfiguration())
                .ApplyConfiguration(new RFIDGroupTypeConfiguration())
                .ApplyConfiguration(new DeviceManageTypeConfiguration())
                .ApplyConfiguration(new OperationTypeConfiguration())
                .ApplyConfiguration(new ModuleypeConfiguration())
                .ApplyConfiguration(new SoftWareTypeConfiguration())
                .ApplyConfiguration(new UserRoleTypeConfiguration())
                .ApplyConfiguration(new InvoiceShipmentTypeConfiguration())
                .ApplyConfiguration(new DYLogTypeConfiguration())
                .ApplyConfiguration(new RFIDRecordTypeConfiguartion())
                .ApplyConfiguration(new CarInfoTypeConfiguration())
                .ApplyConfiguration(new LocationLogTypeConfiguration())
                .ApplyConfiguration(new ControlInfoTypeConfiguration())
                .ApplyConfiguration(new VirtualPrinterDataTypeConfiguration())
                .ApplyConfiguration(new RFIDBackupTypeConfiguartion())
                .ApplyConfiguration(new CarDataTypeConfiguration())
                .ApplyConfiguration(new CarNoForRFIDTypeConfiguration())
                .ApplyConfiguration(new CarRFIDReceiverTypeConfiguration())
                .ApplyConfiguration(new RelationRFIDQRCodeTypeConfiguration())
                .ApplyConfiguration(new InvoiceUserInfoTypeConfiguration());
        }
    }
}
