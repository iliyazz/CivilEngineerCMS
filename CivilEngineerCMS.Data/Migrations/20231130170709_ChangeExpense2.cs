using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class ChangeExpense2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "InvoiceNumber",
                table: "Expenses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDzsPqxQQMQAEMeEC/Dr100vtZJN8e5bXFf9vtPudSfmFi+7gzAHOSpVa4w4s2qtnQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InvoiceNumber",
                table: "Expenses",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEE/VPxSOZtOZbsDB8it0ODM5Mf+CNGMg+6E54xeb4AfQYXhX5lY4VMOcXYJ+S75jjw==");
        }
    }
}
