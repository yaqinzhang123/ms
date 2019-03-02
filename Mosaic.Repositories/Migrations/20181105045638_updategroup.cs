using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class updategroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RFIDList",
                table: "RFIDGroup",
                newName: "PlateNumber");

            migrationBuilder.AddColumn<bool>(
                name: "AutoSkip",
                table: "QRCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EndRoot",
                table: "QRCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Lock",
                table: "QRCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ManualSkip",
                table: "QRCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StartRoot",
                table: "QRCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VirtualAdd",
                table: "QRCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupingMethod",
                table: "Operation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Operation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Operation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoSkip",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "EndRoot",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "Lock",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "ManualSkip",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "StartRoot",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "VirtualAdd",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "GroupingMethod",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Operation");

            migrationBuilder.RenameColumn(
                name: "PlateNumber",
                table: "RFIDGroup",
                newName: "RFIDList");
        }
    }
}
