using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class ChangeExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContent",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNumber",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEE/VPxSOZtOZbsDB8it0ODM5Mf+CNGMg+6E54xeb4AfQYXhX5lY4VMOcXYJ+S75jjw==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Expenses");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageContent",
                table: "Projects",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEOZrXaOAqlFTwdwAwh0LCRcXOgokagZgtR9j1TNvt0XbVqohf74c1tcpkSbUzHMTfw==");
        }
    }
}
