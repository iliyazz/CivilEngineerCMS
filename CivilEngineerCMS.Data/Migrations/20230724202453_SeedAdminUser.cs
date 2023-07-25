using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class SeedAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"), 0, "31ca9820-e6fd-48dd-8483-68225bd2071c", "iliyaz.softuni@gmail.com", true, false, null, "ILIYAZ.SOFTUNI@GMAIL.COM", "ILIYAZ.SOFTUNI@GMAIL.COM", "AQAAAAEAACcQAAAAEMUBS2mCMeWPbeaAPzfizF8E1akOQplqw5TirNuaaHLc6mPHRg7dDuy1OVnir7e7yA==", "+359123456789", true, "LJW7J33EVBQOCAMXUDU6OLJIC5NFDMBG", false, "iliyaz.softuni@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b85227a0-4fb4-4b5e-84ad-08db807c5edb"));
        }
    }
}
