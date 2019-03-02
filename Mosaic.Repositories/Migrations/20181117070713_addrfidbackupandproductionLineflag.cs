using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class addrfidbackupandproductionLineflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operation_ProductionLine_ProductionLineID",
                table: "Operation");

            migrationBuilder.DropIndex(
                name: "IX_Operation_ProductionLineID",
                table: "Operation");

            migrationBuilder.AddColumn<bool>(
                name: "Flag",
                table: "ProductionLine",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OperationID",
                table: "ProductionLine",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductionLineID1",
                table: "Operation",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RFIDBackup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    CodeList = table.Column<string>(nullable: true),
                    InvoiceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFIDBackup", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operation_ProductionLineID1",
                table: "Operation",
                column: "ProductionLineID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Operation_ProductionLine_ProductionLineID1",
                table: "Operation",
                column: "ProductionLineID1",
                principalTable: "ProductionLine",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operation_ProductionLine_ProductionLineID1",
                table: "Operation");

            migrationBuilder.DropTable(
                name: "RFIDBackup");

            migrationBuilder.DropIndex(
                name: "IX_Operation_ProductionLineID1",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "ProductionLine");

            migrationBuilder.DropColumn(
                name: "OperationID",
                table: "ProductionLine");

            migrationBuilder.DropColumn(
                name: "ProductionLineID1",
                table: "Operation");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_ProductionLineID",
                table: "Operation",
                column: "ProductionLineID");

            migrationBuilder.AddForeignKey(
                name: "FK_Operation_ProductionLine_ProductionLineID",
                table: "Operation",
                column: "ProductionLineID",
                principalTable: "ProductionLine",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
