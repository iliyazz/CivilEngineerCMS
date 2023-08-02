namespace CivilEngineerCMS.Data.Configurations
{
    using Common;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .Property(p => p.ProjectCreatedDate)
                .HasDefaultValueSql("GETDATE()");
            builder
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.Manager)
                .WithMany(e => e.Projects)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);


            //builder.HasData(this.GenerateProjects());
        }
        //private Project[] GenerateProjects()
        //{
        //    ICollection<Project> projects = new HashSet<Project>();
        //    Project project;

        //    project = new Project
        //    {
        //        Id = Guid.Parse("9936B8B0-8165-49DE-A574-21957390D5BC"),
        //        Name = "Forest House",
        //        Description = "Design Very small house in forest",
        //        ClientId = Guid.Parse("F1E783E9-5932-429E-B755-E9115E506DC6"),
        //        ManagerId = Guid.Parse("15779B34-FF5A-4DB1-B6D2-0B3626F251B8"),
        //        UrlPicturePath = "https://archello.com/thumbs/images/2021/04/15/81.waw.pl-forest-house-apartments-archello.1618468040.4446.jpg",
        //        Status = Enum.Parse<ProjectStatusEnums>("3"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-16 21:38:12.7566667"),
        //        ProjectEndDate = DateTime.Parse("2023-09-22 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);
        //    project = new Project
        //    {
        //        Id = Guid.Parse("28766CD4-25BA-4B4C-8B68-366197404EE5"),
        //        Name = "Designing administrative building",
        //        Description = "2550sq.m. 5 floors",
        //        ClientId = Guid.Parse("F1E783E9-5932-429E-B755-E9115E506DC6"),
        //        ManagerId = Guid.Parse("41A1BDAB-D519-4C95-AE73-0425A31D9B6D"),
        //        UrlPicturePath = "https://localhost:7255/images/5.jpg",
        //        Status = Enum.Parse<ProjectStatusEnums>("3"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-12 20:50:15.3200000"),
        //        ProjectEndDate = DateTime.Parse("2023-12-15 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);
        //    project = new Project
        //    {
        //        Id = Guid.Parse("5B9750F4-6AD9-43A8-9E11-4CCE433D929D"),
        //        Name = "Test Design house",
        //        Description = "Test description design house",
        //        ClientId = Guid.Parse("EAD2D779-0A8A-4615-BF42-34B4B33F76A5"),
        //        ManagerId = Guid.Parse("B7C8C9B6-880E-487D-9531-AA523A08A8C5"),
        //        UrlPicturePath = "",
        //        Status = Enum.Parse<ProjectStatusEnums>("1"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-19 19:31:45.3866667"),
        //        ProjectEndDate = DateTime.Parse("2023-12-15 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);
        //    project = new Project
        //    {
        //        Id = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Name = "Designing small House",
        //        Description = "Two floors, 266sq.m",
        //        ClientId = Guid.Parse("5C1C5BD8-C162-4246-BFAE-B381DA3B22B0"),
        //        ManagerId = Guid.Parse("5DEA13AC-A918-4A9E-9147-CF1211F73381"),
        //        UrlPicturePath = "https://localhost:7255/images/3.jpg",
        //        Status = Enum.Parse<ProjectStatusEnums>("3"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-09 18:10:44.4700000"),
        //        ProjectEndDate = DateTime.Parse("2023-12-16 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);
        //    project = new Project
        //    {
        //        Id = Guid.Parse("C54BC710-EFA4-4A2C-8160-812D276D5F3F"),
        //        Name = "Supervision winter house",
        //        Description = "250sq.m. 2 floors",
        //        ClientId = Guid.Parse("5C1C5BD8-C162-4246-BFAE-B381DA3B22B0"),
        //        ManagerId = Guid.Parse("5DEA13AC-A918-4A9E-9147-CF1211F73381"),
        //        UrlPicturePath = "https://empire-s3-production.bobvila.com/articles/wp-content/uploads/2020/12/iStock-1195793605-home-winter.jpg",
        //        Status = Enum.Parse<ProjectStatusEnums>("3"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-17 17:11:21.8100000"),
        //        ProjectEndDate = DateTime.Parse("2024-06-13 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);
        //    project = new Project
        //    {
        //        Id = Guid.Parse("D1615808-4CC0-4C32-AD90-9FBE2DE229C3"),
        //        Name = "Construction supervision",
        //        Description = "Administrative building, 3550sq.m",
        //        ClientId = Guid.Parse("BE032944-AD41-4DE3-81F8-DAAE394FA777"),
        //        ManagerId = Guid.Parse("15779B34-FF5A-4DB1-B6D2-0B3626F251B8"),
        //        UrlPicturePath = "https://localhost:7255/images/2.jpg",
        //        Status = Enum.Parse<ProjectStatusEnums>("1"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-09 18:12:40.4500000"),
        //        ProjectEndDate = DateTime.Parse("2024-05-15 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);
        //    project = new Project
        //    {
        //        Id = Guid.Parse("38CF7267-BA70-442B-896F-A41F1A4189BC"),
        //        Name = "House design",
        //        Description = "150 sq.m, 2 floors",
        //        ClientId = Guid.Parse("B90D4A57-7046-4201-80DE-ED5A41D7B6A1"),
        //        ManagerId = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //        UrlPicturePath = "https://doqannr4o843x.cloudfront.net/5032ce49f6dde667d6e56f8c80bb85f92fbb59ca-1-original.jpeg",
        //        Status = Enum.Parse<ProjectStatusEnums>("5"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-17 16:48:55.0966667"),
        //        ProjectEndDate = DateTime.Parse("2023-12-21 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);
        //    project = new Project
        //    {
        //        Id = Guid.Parse("BF07487F-B5EA-47A7-820C-B5B19353A104"),
        //        Name = "Supervision of Small House",
        //        Description = "220 sq.m, 2 floors",
        //        ClientId = Guid.Parse("B90D4A57-7046-4201-80DE-ED5A41D7B6A1"),
        //        ManagerId = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //        UrlPicturePath = "https://2.bp.blogspot.com/-7fDorXHTy5g/Vnf88kKF4II/AAAAAAAA06Y/PKa5ri8YB5A/s0/neat-simple-home-design.jpg",
        //        Status = Enum.Parse<ProjectStatusEnums>("3"),
        //        ProjectCreatedDate = DateTime.Parse("2023-07-17 16:50:21.2100000"),
        //        ProjectEndDate = DateTime.Parse("2023-11-30 00:00:00.0000000"),
        //        IsActive = true,
        //    };
        //    projects.Add(project);

        //    return projects.ToArray();
        //}
    }
}