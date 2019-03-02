using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class addqrcodeoperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Page",
                table: "Operation");

            migrationBuilder.CreateIndex(
                name: "IX_QRCode_OperationID",
                table: "QRCode",
                column: "OperationID");

            migrationBuilder.AddForeignKey(
                name: "FK_QRCode_Operation_OperationID",
                table: "QRCode",
                column: "OperationID",
                principalTable: "Operation",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCode_Operation_OperationID",
                table: "QRCode");

            migrationBuilder.DropIndex(
                name: "IX_QRCode_OperationID",
                table: "QRCode");

            migrationBuilder.AddColumn<int>(
                name: "Page",
                table: "Operation",
                nullable: false,
                defaultValue: 0);
        }
    }
}
