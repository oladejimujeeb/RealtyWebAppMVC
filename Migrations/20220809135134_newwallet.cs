using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtyWebApp.Migrations
{
    public partial class newwallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_Realtors_RealtorId",
                table: "Wallet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallet",
                table: "Wallet");

            migrationBuilder.RenameTable(
                name: "Wallet",
                newName: "Wallets");

            migrationBuilder.RenameIndex(
                name: "IX_Wallet_RealtorId",
                table: "Wallets",
                newName: "IX_Wallets_RealtorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HRRee8yLSwrGIo8Whu9NC.s4oQ7M.tQqfKTiOPHjXSS8PeGM60Pc6");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Realtors_RealtorId",
                table: "Wallets",
                column: "RealtorId",
                principalTable: "Realtors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Realtors_RealtorId",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets");

            migrationBuilder.RenameTable(
                name: "Wallets",
                newName: "Wallet");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_RealtorId",
                table: "Wallet",
                newName: "IX_Wallet_RealtorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallet",
                table: "Wallet",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$akSASxX6B8kOq34o4wLUYeMOmzMmp58Hsad1zLXAsoN8e.hD7Z9k2");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_Realtors_RealtorId",
                table: "Wallet",
                column: "RealtorId",
                principalTable: "Realtors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
