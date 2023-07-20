using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class RemoveBoardsAndTaskEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Boards_BoardId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Boards",
                table: "Boards");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameTable(
                name: "Boards",
                newName: "Board");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Task",
                newName: "IX_Task_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_BoardId",
                table: "Task",
                newName: "IX_Task_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Board",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Board_BoardId",
                table: "Task",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Projects_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Board_BoardId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Projects_ProjectId",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Board");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "Board",
                newName: "Boards");

            migrationBuilder.RenameIndex(
                name: "IX_Task_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_BoardId",
                table: "Tasks",
                newName: "IX_Tasks_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boards",
                table: "Boards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Boards_BoardId",
                table: "Tasks",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
