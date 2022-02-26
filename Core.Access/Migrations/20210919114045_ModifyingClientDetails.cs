using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Access.Migrations
{
    public partial class ModifyingClientDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCodeRequest_AspNetUsers_UserId",
                table: "ClientCodeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientCodeRequest_Client_ClientId",
                table: "ClientCodeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientTokenRequest_Client_ClientId",
                table: "ClientTokenRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientTokenRequest",
                table: "ClientTokenRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientCodeRequest",
                table: "ClientCodeRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Client",
                table: "Client");

            migrationBuilder.RenameTable(
                name: "ClientTokenRequest",
                newName: "ClientTokenRequests");

            migrationBuilder.RenameTable(
                name: "ClientCodeRequest",
                newName: "ClientCodeRequests");

            migrationBuilder.RenameTable(
                name: "Client",
                newName: "Clients");

            migrationBuilder.RenameIndex(
                name: "IX_ClientTokenRequest_ClientId",
                table: "ClientTokenRequests",
                newName: "IX_ClientTokenRequests_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientCodeRequest_UserId",
                table: "ClientCodeRequests",
                newName: "IX_ClientCodeRequests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientCodeRequest_ClientId",
                table: "ClientCodeRequests",
                newName: "IX_ClientCodeRequests_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientTokenRequests",
                table: "ClientTokenRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientCodeRequests",
                table: "ClientCodeRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCodeRequests_AspNetUsers_UserId",
                table: "ClientCodeRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCodeRequests_Clients_ClientId",
                table: "ClientCodeRequests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTokenRequests_Clients_ClientId",
                table: "ClientTokenRequests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCodeRequests_AspNetUsers_UserId",
                table: "ClientCodeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientCodeRequests_Clients_ClientId",
                table: "ClientCodeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientTokenRequests_Clients_ClientId",
                table: "ClientTokenRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientTokenRequests",
                table: "ClientTokenRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientCodeRequests",
                table: "ClientCodeRequests");

            migrationBuilder.RenameTable(
                name: "ClientTokenRequests",
                newName: "ClientTokenRequest");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Client");

            migrationBuilder.RenameTable(
                name: "ClientCodeRequests",
                newName: "ClientCodeRequest");

            migrationBuilder.RenameIndex(
                name: "IX_ClientTokenRequests_ClientId",
                table: "ClientTokenRequest",
                newName: "IX_ClientTokenRequest_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientCodeRequests_UserId",
                table: "ClientCodeRequest",
                newName: "IX_ClientCodeRequest_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientCodeRequests_ClientId",
                table: "ClientCodeRequest",
                newName: "IX_ClientCodeRequest_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientTokenRequest",
                table: "ClientTokenRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Client",
                table: "Client",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientCodeRequest",
                table: "ClientCodeRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCodeRequest_AspNetUsers_UserId",
                table: "ClientCodeRequest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCodeRequest_Client_ClientId",
                table: "ClientCodeRequest",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTokenRequest_Client_ClientId",
                table: "ClientTokenRequest",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
