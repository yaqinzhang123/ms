using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class addrfid123Filed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RFID",
                table: "CarData",
                newName: "RFID3");

            migrationBuilder.AddColumn<string>(
                name: "RFID1",
                table: "CarData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RFID2",
                table: "CarData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RFID1",
                table: "CarData");

            migrationBuilder.DropColumn(
                name: "RFID2",
                table: "CarData");

            migrationBuilder.RenameColumn(
                name: "RFID3",
                table: "CarData",
                newName: "RFID");
        }
    }
}
