using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class addUpdaterfidPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "QRCode",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Group",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarNoForRFID",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    CarNo = table.Column<string>(nullable: true),
                    RFID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarNoForRFID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CarRFIDReceiver",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    RFIDList = table.Column<string>(nullable: true),
                    EnterTime = table.Column<DateTime>(nullable: false),
                    LeaveTime = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRFIDReceiver", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarNoForRFID");

            migrationBuilder.DropTable(
                name: "CarRFIDReceiver");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Group");
        }
    }
}
