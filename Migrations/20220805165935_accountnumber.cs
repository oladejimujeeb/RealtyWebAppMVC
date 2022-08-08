using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtyWebApp.Migrations
{
    public partial class accountnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNo",
                table: "Wallet",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Wallet",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$akSASxX6B8kOq34o4wLUYeMOmzMmp58Hsad1zLXAsoN8e.hD7Z9k2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNo",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Wallet");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$C4G2nfIl/s0codCwdYDGvuwKIJnDB44RGtnWM3xFR/Oxft1OA0TI.");
        }
    }
}
