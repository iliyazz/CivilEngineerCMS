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


            builder.HasData(this.GenerateProjects());
        }


        private Project[] GenerateProjects()
        {
            ICollection<Project> projects = new HashSet<Project>();
            Project project;
            project = new Project
            {
                Id = Guid.Parse("CFC9AB51-C431-4624-98FF-4DA7BE50762D"),
                Name = "Project 1",
                Description = "Project 1 Description",
                ClientId = Guid.Parse("8058BDA4-C0FB-44D3-B3B6-E66619CEC1AB"),
                ManagerId = Guid.Parse("8C0629B6-B564-4A2C-A6C2-73408D4878E5"),
                Status = ProjectStatusEnums.InProgress,
                ProjectCreatedDate = new DateTime(2023, 3, 10),
                ProjectEndDate = new DateTime(2023, 8, 12),
            };
            projects.Add(project);
            project = new Project
            {
                Id = Guid.Parse("082258D8-B2EC-410F-ACD7-4BDE06D025D7"),
                Name = "Project 2",
                Description = "Project 2 Description",
                ClientId = Guid.Parse("1AE4584F-5611-4870-BAA7-CB0E7EDCC572"),
                ManagerId = Guid.Parse("EC5497AA-1B1C-44AC-8259-7B7B95B07B12"),
                Status = ProjectStatusEnums.NotYetStarted,
                ProjectCreatedDate = new DateTime(2023, 1, 5),
                ProjectEndDate = new DateTime(2023, 2, 8),
            };
            projects.Add(project);
            return projects.ToArray();
        }
    }
}