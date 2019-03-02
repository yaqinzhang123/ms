using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class addinvoiceuserinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "InvoiceShipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverPhoneNo",
                table: "InvoiceShipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupNoList",
                table: "InvoiceShipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlateNumber",
                table: "InvoiceShipment",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvoiceUserInfo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    InvoiceID = table.Column<int>(nullable: false),
                    UserInfoID = table.Column<int>(nullable: false),
                    GroupNoList = table.Column<string>(nullable: true),
                    CodeList = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceUserInfo", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceUserInfo");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "InvoiceShipment");

            migrationBuilder.DropColumn(
                name: "DriverPhoneNo",
                table: "InvoiceShipment");

            migrationBuilder.DropColumn(
                name: "GroupNoList",
                table: "InvoiceShipment");

            migrationBuilder.DropColumn(
                name: "PlateNumber",
                table: "InvoiceShipment");
        }
    }
}
