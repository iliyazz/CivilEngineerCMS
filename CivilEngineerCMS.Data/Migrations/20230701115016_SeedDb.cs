using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivilEngineerCMS.Data.Migrations
{
    public partial class SeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "FirstName", "LastName", "PhoneNumber", "UserId" },
                values: new object[] { new Guid("1ae4584f-5611-4870-baa7-cb0e7edcc572"), "Plovdiv1", "Gosho", "Goshev", "+359123456787", new Guid("4f318431-568e-4cdf-9a58-08db7a22ebd9") });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "FirstName", "LastName", "PhoneNumber", "UserId" },
                values: new object[] { new Guid("8058bda4-c0fb-44d3-b3b6-e66619cec1ab"), "Plovdiv1", "Pesho", "Peshev", "+359123456788", new Guid("60376974-e414-4277-1d75-08db7a21c396") });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "FirstName", "JobTitle", "LastName", "PhoneNumber", "UserId" },
                values: new object[] { new Guid("8c0629b6-b564-4a2c-a6c2-73408d4878e5"), "Sofia", "Ivan", "Surveyor", "Ivanov", "+395123456781", new Guid("5544bb21-be62-44ac-1d76-08db7a21c396") });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ClientId", "Description", "ManagerId", "Name", "ProjectCreatedDate", "ProjectEndDate", "Status", "UserId" },
                values: new object[] { new Guid("082258d8-b2ec-410f-acd7-4bde06d025d7"), new Guid("1ae4584f-5611-4870-baa7-cb0e7edcc572"), "Project 2 Description", new Guid("ec5497aa-1b1c-44ac-8259-7b7b95b07b12"), "Project 2", new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new Guid("5544bb21-be62-44ac-1d76-08db7a21c396") });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ClientId", "Description", "ManagerId", "Name", "ProjectCreatedDate", "ProjectEndDate", "Status", "UserId" },
                values: new object[] { new Guid("cfc9ab51-c431-4624-98ff-4da7be50762d"), new Guid("8058bda4-c0fb-44d3-b3b6-e66619cec1ab"), "Project 1 Description", new Guid("8c0629b6-b564-4a2c-a6c2-73408d4878e5"), "Project 1", new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new Guid("e9830393-05a5-4069-1d74-08db7a21c396") });

            migrationBuilder.InsertData(
                table: "ProjectsEmployees",
                columns: new[] { "EmployeeId", "ProjectId" },
                values: new object[] { new Guid("ec5497aa-1b1c-44ac-8259-7b7b95b07b12"), new Guid("082258d8-b2ec-410f-acd7-4bde06d025d7") });

            migrationBuilder.InsertData(
                table: "ProjectsEmployees",
                columns: new[] { "EmployeeId", "ProjectId" },
                values: new object[] { new Guid("ec5497aa-1b1c-44ac-8259-7b7b95b07b12"), new Guid("cfc9ab51-c431-4624-98ff-4da7be50762d") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectsEmployees",
                keyColumns: new[] { "EmployeeId", "ProjectId" },
                keyValues: new object[] { new Guid("ec5497aa-1b1c-44ac-8259-7b7b95b07b12"), new Guid("082258d8-b2ec-410f-acd7-4bde06d025d7") });

            migrationBuilder.DeleteData(
                table: "ProjectsEmployees",
                keyColumns: new[] { "EmployeeId", "ProjectId" },
                keyValues: new object[] { new Guid("ec5497aa-1b1c-44ac-8259-7b7b95b07b12"), new Guid("cfc9ab51-c431-4624-98ff-4da7be50762d") });

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("082258d8-b2ec-410f-acd7-4bde06d025d7"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("cfc9ab51-c431-4624-98ff-4da7be50762d"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("1ae4584f-5611-4870-baa7-cb0e7edcc572"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("8058bda4-c0fb-44d3-b3b6-e66619cec1ab"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("8c0629b6-b564-4a2c-a6c2-73408d4878e5"));
        }
    }
}
