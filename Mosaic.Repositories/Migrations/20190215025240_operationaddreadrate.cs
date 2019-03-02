using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class operationaddreadrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ReadRate",
                table: "Operation",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadRate",
                table: "Operation");
        }
    }
}
