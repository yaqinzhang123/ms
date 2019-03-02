using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class rfidgroupcompanyid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RFIDGroup_Company_CompanyID",
                table: "RFIDGroup");

            migrationBuilder.DropIndex(
                name: "IX_RFIDGroup_CompanyID",
                table: "RFIDGroup");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "RFIDGroup",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "RFIDGroup",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_RFIDGroup_CompanyID",
                table: "RFIDGroup",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_RFIDGroup_Company_CompanyID",
                table: "RFIDGroup",
                column: "CompanyID",
                principalTable: "Company",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
