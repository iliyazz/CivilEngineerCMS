using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class AddPublicIdInProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEOZrXaOAqlFTwdwAwh0LCRcXOgokagZgtR9j1TNvt0XbVqohf74c1tcpkSbUzHMTfw==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEGgEQwJPgs8rE1zowMlQztjtsgWiB2BCeonOeW9uXq12UIIJW3aUrmk2KXFq4BqN3A==");
        }
    }
}
