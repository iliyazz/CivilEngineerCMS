using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class AddPictureInProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageContent",
                table: "Projects",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Projects",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEPX2QhRhCyR4wg4gKC8pVxSI1yyuIfPI1a1WxlE3s2m8XTzf5W+Mym6X9iByphAEgw==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELA8jWQDQZbvZ1tx24tdgEqJwbUIEC02CK6EFqT+27eGWgVa+/in9HxzhwEdNuLk2A==");
        }
    }
}
