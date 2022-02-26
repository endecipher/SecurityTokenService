using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Access.Migrations
{
    public partial class MinorChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: "QXlhbkNsaWVudA==");

            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_FriendlyName",
                table: "Clients",
                column: "FriendlyName")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_FriendlyName",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "FriendlyName", "Password" },
                values: new object[] { "QXlhbkNsaWVudA==", "AyanClient", "AyanClient" });
        }
    }
}
