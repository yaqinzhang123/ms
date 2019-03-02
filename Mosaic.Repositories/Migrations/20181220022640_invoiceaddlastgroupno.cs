using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class invoiceaddlastgroupno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrQRCodeList",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastGroupNoList",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNo",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemoveGroupNoList",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemoveQRCodeList",
                table: "Invoice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrQRCodeList",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "LastGroupNoList",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "LastNo",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "RemoveGroupNoList",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "RemoveQRCodeList",
                table: "Invoice");
        }
    }
}
