﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mosaic.Repositories.Dao;

namespace Mosaic.Repositories.Migrations
{
    [DbContext(typeof(MosaicContext))]
    [Migration("20181130084328_relationadduser")]
    partial class relationadduser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mosaic.Domain.Models.Agency", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int>("CompanyID");

                    b.Property<string>("Contact");

                    b.Property<string>("ContactTel");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Grade");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.Property<string>("SuperAgency");

                    b.Property<string>("Tel");

                    b.Property<string>("Zone");

                    b.HasKey("ID");

                    b.ToTable("Agency");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.CarData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("Enter");

                    b.Property<bool?>("Handled")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<DateTime>("Leave");

                    b.Property<int>("Line");

                    b.Property<string>("Location");

                    b.Property<string>("RFID1");

                    b.Property<string>("RFID2");

                    b.Property<string>("RFID3");

                    b.Property<bool>("SecondFilterd");

                    b.Property<bool>("Uploaded");

                    b.HasKey("ID");

                    b.ToTable("CarData");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.CarInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Flag");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Location");

                    b.Property<int>("ProductionLineID");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("TriggerTime");

                    b.HasKey("ID");

                    b.ToTable("CarInfo");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.CarNoForRFID", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CarNo");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("RFID");

                    b.HasKey("ID");

                    b.ToTable("CarNoForRFID");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.CarRFIDReceiver", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("EnterTime");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<DateTime>("LeaveTime");

                    b.Property<string>("Location");

                    b.Property<string>("RFIDList");

                    b.HasKey("ID");

                    b.ToTable("CarRFIDReceiver");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Describe");

                    b.Property<string>("Img");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Manufacturer");

                    b.Property<int>("ManufacturerNo");

                    b.Property<string>("MaterialNo");

                    b.Property<string>("Yieldly");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("Code");

                    b.Property<string>("Contact");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email");

                    b.Property<string>("Fax");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.Property<string>("PostNo");

                    b.Property<string>("RegistrationCode");

                    b.Property<string>("ShortCode");

                    b.Property<string>("SoftList");

                    b.Property<string>("Tel");

                    b.HasKey("ID");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.ControlInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("DI1");

                    b.Property<bool>("DI2");

                    b.Property<bool>("DI3");

                    b.Property<bool>("DI4");

                    b.Property<bool>("DI5");

                    b.Property<bool>("DI6");

                    b.Property<bool>("DI7");

                    b.Property<bool>("DI8");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Flag");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("ProductionLineID");

                    b.Property<bool>("Sync");

                    b.Property<DateTime>("Time");

                    b.HasKey("ID");

                    b.ToTable("ControlInfo");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.DeviceManage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("IP");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("ManageUrl");

                    b.Property<string>("Memo");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("DeviceManage");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.DYLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Key");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Memo");

                    b.HasKey("ID");

                    b.ToTable("DYLog");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Group", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Flag");

                    b.Property<string>("InvoiceNo");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Location");

                    b.Property<int>("ProductionLineID");

                    b.Property<string>("RFID");

                    b.Property<string>("RFIDGroupNo");

                    b.Property<DateTime>("Time");

                    b.HasKey("ID");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Invoice", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BatchNumber");

                    b.Property<string>("CodeList");

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("CustomerNo");

                    b.Property<string>("CustomerOrderNo");

                    b.Property<string>("CustomerOrderTime");

                    b.Property<string>("DealerName");

                    b.Property<string>("DealerPlace");

                    b.Property<string>("DealerPostcord");

                    b.Property<bool>("Deleted");

                    b.Property<string>("DeliveryMode");

                    b.Property<string>("DriverName");

                    b.Property<string>("DriverPhoneNo");

                    b.Property<string>("ErrGroupNoList");

                    b.Property<string>("ErrRFIDList");

                    b.Property<bool>("Flag");

                    b.Property<string>("GroupNoList");

                    b.Property<string>("InvoiceTime");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("No");

                    b.Property<string>("OrderNo");

                    b.Property<string>("OrderTime");

                    b.Property<string>("PlateNumber");

                    b.Property<string>("Quantity");

                    b.Property<string>("ShipmentMode");

                    b.Property<string>("img");

                    b.HasKey("ID");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.InvoiceShipment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Describe");

                    b.Property<int>("InvoiceID");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("MaterialNo");

                    b.Property<string>("Project");

                    b.Property<string>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("InvoiceID");

                    b.ToTable("InvoiceShipment");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.LocationLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.Property<int>("ProductionLineID");

                    b.Property<DateTime>("Time");

                    b.HasKey("ID");

                    b.ToTable("LocationLog");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Module", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.Property<int?>("SoftWareID");

                    b.HasKey("ID");

                    b.HasIndex("SoftWareID");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Operation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BatchNo");

                    b.Property<int>("CategoryID");

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<int>("GroupingMethod");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("ProductionLineID");

                    b.Property<int?>("ProductionLineID1");

                    b.Property<int>("Rule");

                    b.Property<bool>("State");

                    b.Property<int>("Sum");

                    b.Property<string>("Time");

                    b.Property<int>("UserInfoID");

                    b.Property<int>("Weight");

                    b.HasKey("ID");

                    b.HasIndex("ProductionLineID1");

                    b.ToTable("Operation");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.ProductionLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Flag");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.Property<int>("OperationID");

                    b.Property<string>("RFIDDevice");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("ProductionLine");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.QRCode", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AutoSkip");

                    b.Property<int>("CID");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("EndRoot");

                    b.Property<int>("GID");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Location");

                    b.Property<bool>("Lock");

                    b.Property<bool>("ManualSkip");

                    b.Property<int>("OperationID");

                    b.Property<int>("OperationRule");

                    b.Property<int>("ProductionLineID");

                    b.Property<bool>("StartRoot");

                    b.Property<DateTime>("Time");

                    b.Property<int>("VirtualAdd");

                    b.HasKey("ID");

                    b.ToTable("QRCode");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.RelationRFIDQRCode", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("RFID");

                    b.Property<DateTime>("TimeQRCode");

                    b.Property<DateTime>("TimeRFID");

                    b.Property<int?>("userInfoID");

                    b.HasKey("ID");

                    b.HasIndex("userInfoID");

                    b.ToTable("RelationRFIDQRCode");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.RFIDBackup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CodeList");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<int>("InvoiceID");

                    b.Property<DateTime>("LastUpdateTime");

                    b.HasKey("ID");

                    b.ToTable("RFIDBackup");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.RFIDGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("GroupNoList");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("OldInvoiceID");

                    b.Property<string>("OldInvoiceNo");

                    b.Property<int>("OldInvoiceShipmentID");

                    b.Property<string>("OldInvoiceShipmentNo");

                    b.Property<DateTime>("OldTime");

                    b.Property<string>("PlateNumber");

                    b.Property<string>("QRCodeList");

                    b.HasKey("ID");

                    b.ToTable("RFIDGroup");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.RFIDRecord", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Flag");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("LineID");

                    b.Property<string>("Location");

                    b.Property<string>("RFID");

                    b.Property<bool>("Sync");

                    b.Property<DateTime>("Time");

                    b.Property<int>("Times");

                    b.HasKey("ID");

                    b.ToTable("RFIDRecord");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Rights", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Add");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Delete");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Edit");

                    b.Property<string>("Factory");

                    b.Property<int>("FactoryID");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int?>("ModuleID");

                    b.Property<int>("RoleID");

                    b.Property<string>("SoftName");

                    b.HasKey("ID");

                    b.HasIndex("ModuleID");

                    b.HasIndex("RoleID");

                    b.ToTable("Rights");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.SoftWare", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Flag");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("SoftWare");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.UserInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<string>("JobNumber");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Tel");

                    b.Property<string>("TrueName");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.UserRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("RoleID");

                    b.Property<int>("UserInfoID");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.HasIndex("UserInfoID");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.VirtualPrinterData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Flag");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("PSFileContent");

                    b.Property<DateTime>("PrintTime");

                    b.Property<string>("TxtFileContent");

                    b.HasKey("ID");

                    b.ToTable("VirtualPrinterData");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Category", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mosaic.Domain.Models.InvoiceShipment", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.Invoice")
                        .WithMany("InvoiceShipmentList")
                        .HasForeignKey("InvoiceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Module", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.SoftWare", "SoftWare")
                        .WithMany()
                        .HasForeignKey("SoftWareID");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Operation", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.ProductionLine", "ProductionLine")
                        .WithMany()
                        .HasForeignKey("ProductionLineID1");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.ProductionLine", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.Company", "Company")
                        .WithMany("ProductionLineList")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mosaic.Domain.Models.RelationRFIDQRCode", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.UserInfo", "userInfo")
                        .WithMany()
                        .HasForeignKey("userInfoID");
                });

            modelBuilder.Entity("Mosaic.Domain.Models.Rights", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleID");

                    b.HasOne("Mosaic.Domain.Models.Role")
                        .WithMany("rightsList")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mosaic.Domain.Models.UserInfo", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mosaic.Domain.Models.UserRole", b =>
                {
                    b.HasOne("Mosaic.Domain.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mosaic.Domain.Models.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserInfoID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
