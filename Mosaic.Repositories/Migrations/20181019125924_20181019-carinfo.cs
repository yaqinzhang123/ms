using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class _20181019carinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enter",
                table: "CarInfo");

            migrationBuilder.RenameColumn(
                name: "Leave",
                table: "CarInfo",
                newName: "TriggerTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TriggerTime",
                table: "CarInfo",
                newName: "Leave");

            migrationBuilder.AddColumn<DateTime>(
                name: "Enter",
                table: "CarInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
