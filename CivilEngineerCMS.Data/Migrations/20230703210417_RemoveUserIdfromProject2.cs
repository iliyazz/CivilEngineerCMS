using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class RemoveUserIdfromProject2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DropForeignKeyCostraint(migrationBuilder, "Projects", "UserId", "AspNetUsers");
            //DropKey(migrationBuilder, "Projects", "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
        private void DropForeignKeyCostraint(MigrationBuilder migrationBuilder, string table, string foreignKey,
            string principalTable)
        {
            migrationBuilder.DropForeignKey(
                name: $"FK_{table}_{principalTable}_{foreignKey}",
                table: table);

            migrationBuilder.DropIndex(
                name: $"IX_{table}_{foreignKey}",
                table: table);
        }

        //private void DropKey(MigrationBuilder migrationBuilder, string table, string key)
        //{
        //    migrationBuilder.DropIndex(
        //                       name: $"IX_{table}_{key}",
        //                                      table: table);
        //}
    }
}
