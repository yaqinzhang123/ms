using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class addqrcodeoperation1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCode_Operation_OperationID",
                table: "QRCode");

            migrationBuilder.AlterColumn<int>(
                name: "OperationID",
                table: "QRCode",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_QRCode_Operation_OperationID",
                table: "QRCode",
                column: "OperationID",
                principalTable: "Operation",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCode_Operation_OperationID",
                table: "QRCode");

            migrationBuilder.AlterColumn<int>(
                name: "OperationID",
                table: "QRCode",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QRCode_Operation_OperationID",
                table: "QRCode",
                column: "OperationID",
                principalTable: "Operation",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
