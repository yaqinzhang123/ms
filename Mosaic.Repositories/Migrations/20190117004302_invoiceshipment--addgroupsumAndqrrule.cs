using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class invoiceshipmentaddgroupsumAndqrrule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupSum",
                table: "InvoiceShipment",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupSum",
                table: "InvoiceShipment");
        }
    }
}
