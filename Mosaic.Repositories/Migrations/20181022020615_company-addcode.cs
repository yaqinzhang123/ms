using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class companyaddcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Company_CompanyID",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_CompanyID",
                table: "Invoice");

            migrationBuilder.AddColumn<bool>(
                name: "Flag",
                table: "Group",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "Company",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CompanyID",
                table: "Invoice",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Company_CompanyID",
                table: "Invoice",
                column: "CompanyID",
                principalTable: "Company",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
