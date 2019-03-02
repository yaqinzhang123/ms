using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class _20181019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControlInfo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    DI1 = table.Column<bool>(nullable: false),
                    DI2 = table.Column<bool>(nullable: false),
                    DI3 = table.Column<bool>(nullable: false),
                    DI4 = table.Column<bool>(nullable: false),
                    DI5 = table.Column<bool>(nullable: false),
                    DI6 = table.Column<bool>(nullable: false),
                    DI7 = table.Column<bool>(nullable: false),
                    DI8 = table.Column<bool>(nullable: false),
                    ProductionLineID = table.Column<int>(nullable: false),
                    Sync = table.Column<bool>(nullable: false),
                    Flag = table.Column<bool>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlInfo", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControlInfo");
        }
    }
}
