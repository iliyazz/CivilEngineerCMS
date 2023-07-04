using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class RemoveForiegnkeyProjectApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DropForeignKey(migrationBuilder, "Projects", "ApplicationUserId", "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
        private void DropForeignKey(MigrationBuilder migrationBuilder, string table, string foreignKey,
            string principalTable)
        {
            migrationBuilder.DropForeignKey(
                name: $"FK_{table}_{principalTable}_{foreignKey}",
                table: table);

            migrationBuilder.DropIndex(
                name: $"IX_{table}_{foreignKey}",
                table: table);
        }
    }
}
