using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaic.Repositories.Migrations
{
    public partial class relationadduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userInfoID",
                table: "RelationRFIDQRCode",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelationRFIDQRCode_userInfoID",
                table: "RelationRFIDQRCode",
                column: "userInfoID");

            migrationBuilder.AddForeignKey(
                name: "FK_RelationRFIDQRCode_UserInfo_userInfoID",
                table: "RelationRFIDQRCode",
                column: "userInfoID",
                principalTable: "UserInfo",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelationRFIDQRCode_UserInfo_userInfoID",
                table: "RelationRFIDQRCode");

            migrationBuilder.DropIndex(
                name: "IX_RelationRFIDQRCode_userInfoID",
                table: "RelationRFIDQRCode");

            migrationBuilder.DropColumn(
                name: "userInfoID",
                table: "RelationRFIDQRCode");
        }
    }
}
