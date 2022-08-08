using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtyWebApp.Migrations
{
    public partial class accountdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Wallet",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ReceipientCode",
                table: "Wallet",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$C4G2nfIl/s0codCwdYDGvuwKIJnDB44RGtnWM3xFR/Oxft1OA0TI.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "ReceipientCode",
                table: "Wallet");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$.ZqLNYKP/zO0mgplJhObseUiOAn3PSG8F11X2hy7zRU9pTTQPloXq");
        }
    }
}
