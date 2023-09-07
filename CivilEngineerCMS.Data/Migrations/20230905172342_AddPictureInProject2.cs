using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class AddPictureInProject2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMGIXzyootB0xpstvsmoU5zXgX4Nm7n4HT7syAlxh8d2s9q6TPz9ko5ZcZQLRMjUIg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEPX2QhRhCyR4wg4gKC8pVxSI1yyuIfPI1a1WxlE3s2m8XTzf5W+Mym6X9iByphAEgw==");
        }
    }
}
