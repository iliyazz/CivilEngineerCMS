namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .Property(e => e.IsActive)
                .HasDefaultValue(true);
            //builder.HasData(GenerateEmployees());

        }


        //private Employee[] GenerateEmployees()
        //{
        //    ICollection<Employee> employees = new HashSet<Employee>();
        //    Employee employee;
        //    employee = new Employee
        //    {
        //        Id = Guid.Parse("41A1BDAB-D519-4C95-AE73-0425A31D9B6D"),
        //        FirstName = "Gergana",
        //        LastName = "Gerganova-employee3",
        //        PhoneNumber = "+359123456777",
        //        Address = "Town Silistra, Osvobojdenie Street 18",
        //        JobTitle = "Surveyor",
        //        IsActive = true,
        //        UserId = Guid.Parse("811CD974-5562-4D33-B491-08DB82505C39"),
        //    };
        //    employees.Add(employee);
        //    employee = new Employee
        //    {
        //        Id = Guid.Parse("15779B34-FF5A-4DB1-B6D2-0B3626F251B8"),
        //        FirstName = "Iliya",
        //        LastName = "Iliyev",
        //        PhoneNumber = "+359123456788",
        //        Address = "Varna, Kraibrejna Street",
        //        JobTitle = "Hydraulic Engineer",
        //        IsActive = true,
        //        UserId = Guid.Parse("669FEF1E-980F-41B0-84B1-08DB807C5EDB"),
        //    };
        //    employees.Add(employee);
        //    employee = new Employee
        //    {
        //        Id = Guid.Parse("47FFC1E5-F628-4575-9813-5EFB9198FF63"),
        //        FirstName = "Nedialka",
        //        LastName = "Nedialkova",
        //        PhoneNumber = "+359123456755",
        //        Address = "Town Kaspichan, Iskar 154",
        //        JobTitle = "Electrical engineer",
        //        IsActive = true,
        //        UserId = Guid.Parse("4FA32242-0E4F-406E-A7C9-37BA26018B8F"),
        //    };
        //    employees.Add(employee);
        //    employee = new Employee
        //    {
        //        Id = Guid.Parse("FCC5A05E-0958-4D7B-8429-6215B8EE0B30"),
        //        FirstName = "Krastio",
        //        LastName = "Krastev",
        //        PhoneNumber = "+359123456782",
        //        Address = "Sofia, Vitosha Street 235",
        //        JobTitle = "Architect",
        //        IsActive = true,
        //        UserId = Guid.Parse("0E7115FA-D727-4A28-B2AD-62F815F2D899"),
        //    };
        //    employees.Add(employee);
        //    employee = new Employee
        //    {
        //        Id = Guid.Parse("B7C8C9B6-880E-487D-9531-AA523A08A8C5"),
        //        FirstName = "Decho",
        //        LastName = "Dechev",
        //        PhoneNumber = "+359123456792",
        //        Address = "Town Lovech, Klokotnica Street 12",
        //        JobTitle = "Highway engineer",
        //        IsActive = true,
        //        UserId = Guid.Parse("1039301D-024F-4F1A-48FD-08DB86CAD14A"),
        //    };
        //    employees.Add(employee);
        //    employee = new Employee
        //    {
        //        Id = Guid.Parse("5DEA13AC-A918-4A9E-9147-CF1211F73381"),
        //        FirstName = "Ivan",
        //        LastName = "Ivanov",
        //        PhoneNumber = "+359123456779",
        //        Address = "Strelcha Banana Street 1",
        //        JobTitle = "Architect",
        //        IsActive = true,
        //        UserId = Guid.Parse("1E7D4F19-E822-4630-84B0-08DB807C5EDB"),
        //    };
        //    employees.Add(employee);
        //    employee = new Employee
        //    {
        //        Id = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //        FirstName = "Marina",
        //        LastName = "Marinova",
        //        PhoneNumber = "+359123456791",
        //        Address = "Town Pleven, Bulgaria Street 182",
        //        JobTitle = "Geologist",
        //        IsActive = true,
        //        UserId = Guid.Parse("5B8F3E1A-D68B-4E89-48FC-08DB86CAD14A"),
        //    };
        //    employees.Add(employee);

        //    return employees.ToArray();
        //}
    }
}