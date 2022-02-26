using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Access.Migrations
{
    public partial class AddingClientDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "ClientCodeRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RedirectURI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCodeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientCodeRequest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientCodeRequest_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientTokenRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTokenRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTokenRequest_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "ClientId", "FriendlyName", "Password" },
                values: new object[] { "QXlhbkNsaWVudA==", "AyanClient", "AyanClient" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientCodeRequest_ClientId",
                table: "ClientCodeRequest",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCodeRequest_UserId",
                table: "ClientCodeRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTokenRequest_ClientId",
                table: "ClientTokenRequest",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientCodeRequest");

            migrationBuilder.DropTable(
                name: "ClientTokenRequest");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
