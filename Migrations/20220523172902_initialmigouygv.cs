using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtyWebApp.Migrations
{
    public partial class initialmigouygv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyRegNo",
                table: "PropertyImages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PropertyRegNo",
                table: "PropertyDocuments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyRegNo",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "PropertyRegNo",
                table: "PropertyDocuments");
        }
    }
}
