# C# Web - May 2023

**Version: 1.1.0** (defined in `Directory.Build.props`)
🎓 Repository for My course project [*C# Web - May 2023 @ SoftUni*]
***

## ASP.NET Advanced
[**Civil Engineer Customer Management System - Course Project**](https://github.com/iliyazz/CivilEngineerCMS)
***


Application is hosted on Azure: https://cmsprojects.azurewebsites.net/ 


This is educational project for SoftUni's ASP.NET Advanced course, built using ASP.NET MVC, Entity framework core, C#, SQLite, JavaScript, HTML, CSS, Bootstrap.

The application is a Civil Engineer Customer Management System for consulting and design firms. The purpose of the application is to help with the organization of work of the company and at the same time to enable the client to monitor the work process.
There are three types of application users: employees, customers, and administrators.
Only those registered as employees can be administrators.
The application has one seed account for admin with username: iliyaz.softuni@gmail.com and initial password: 123456Admin-+.

The sequence of operation when initializing the application:
Any user can freely register in the application with email and password.
After registration, the site administrator creates an employee or customer.
The administrator creates a project. When creating the project, one of the employees is added as the project manager, and the client connects to the project to monitor the work progress.
The administrator can assign employees to the project or leave this work to the manager.

Customers can monitor the work process.

A manager, who is also an employee, is assigned to each project.
The project manager can do the following:
-adds and removes employees to the project, depending on its specifics.
-edit the data to project
-read, create and edit edit comments to the project
-reads, creates and edits data on payments made to the project

The employee involved in the project can add comments and read other comments.

The administrator can do the following:
-after user registration, the administrator enters additional data and creates a client or employee from the respective user.
-remove customers and employees from the system
-creates projects, appoints a project manager and connects the client to the project
-edit the data of all projects
-edit the data of all customers
-edit the data of all employees
-to grant administrative rights to employees

Customers can only access the projects they are joined to.
Employees can only access and work on the projects they are attached to.
Managers only have access to and can work with the projects they are managers of.
Administrators have access to and can work with all projects, employees and customers.

## Running locally

1. `dotnet run --project CivilEngineerCMS`
2. On startup the app creates `CivilEngineerCMS/civilengineercms.db` (SQLite) and applies all EF Core migrations automatically. The database file is not stored in the repository.
3. In Development the seed admin account gets the Administrator role.

The Web API (`CivilEngineerCMS.WebApi`) uses the same database file (`../CivilEngineerCMS/civilengineercms.db`).

In the main folder there is a legacy script for seeding the database: `CivilEngineerCMS-seed data in database.sql`. It was written for the earlier SQL Server version and needs adapting before use with SQLite.

## Changelog

### 1.1.0
- Removed the old SQL Server migrations; a single SQLite `InitialCreate` migration remains.
- Replaced `GETDATE()` (SQL Server only) with `CURRENT_TIMESTAMP` for default dates.
- Database is created/migrated automatically on startup; the `.db` file and test images are no longer committed.
- Fixed Production lockout settings (they were under the `Password` section and were ignored).
- Web API: switched to SQLite, added a connection string, fixed CORS middleware order, returns 403 instead of 200 for users without access.
- Removed obsolete ASP.NET Core 2.2 packages and unused packages (SQL Server provider, Microsoft.Graph, Swashbuckle.Core, dotenv.net, IE/Opera drivers, MSTest, unofficial QualityTools).


Registered users in application are:

iliyaz.softuni@gmail.com




<br>
client1@abv.bg
<br>
client2@abv.bg
<br>
client3@abv.bg
<br>
client4@abv.bg
<br>
client5@abv.bg
<br>
client6@abv.bg
<br>
client7@abv.bg
<br>
client8@abv.bg
<br>
employee1@abv.bg
<br>
employee2@abv.bg
<br>
employee3@abv.bg
<br>
employee4@abv.bg
<br>
employee5@abv.bg
<br>
employee6@abv.bg
<br>
employee7@abv.bg
<br>
employee8@abv.bg
<br>
employee9@abv.bg
<br>
forRecapture@abv.bg
