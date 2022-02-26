using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Access.Migrations
{
    public partial class AddingIsActiveForTokenRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ClientTokenRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ClientTokenRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientTokenRequests_UserId",
                table: "ClientTokenRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTokenRequests_AspNetUsers_UserId",
                table: "ClientTokenRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientTokenRequests_AspNetUsers_UserId",
                table: "ClientTokenRequests");

            migrationBuilder.DropIndex(
                name: "IX_ClientTokenRequests_UserId",
                table: "ClientTokenRequests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ClientTokenRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ClientTokenRequests");
        }
    }
}
