using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class invoiceerrgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrGroupNoList",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrRFIDList",
                table: "Invoice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrGroupNoList",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ErrRFIDList",
                table: "Invoice");
        }
    }
}
