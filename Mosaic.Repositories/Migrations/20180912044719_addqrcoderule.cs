using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class addqrcoderule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCode_Operation_OperationID",
                table: "QRCode");

            migrationBuilder.DropIndex(
                name: "IX_QRCode_OperationID",
                table: "QRCode");

            migrationBuilder.AlterColumn<int>(
                name: "OperationID",
                table: "QRCode",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationRule",
                table: "QRCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Rule",
                table: "Operation",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationRule",
                table: "QRCode");

            migrationBuilder.AlterColumn<int>(
                name: "OperationID",
                table: "QRCode",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Rule",
                table: "Operation",
                nullable: true,
                oldClrType: typeof(int));

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
