namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ProjectEmployeeEntityConfiguration : IEntityTypeConfiguration<ProjectEmployee>
    {
        public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
        {
            builder
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });
            builder
                .HasOne(p => p.Employee)
                .WithMany(e => e.ProjectsEmployees)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.Project)
                .WithMany(p => p.ProjectsEmployees)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(this.GenerateProjectEmployees());

        }

        private ProjectEmployee[] GenerateProjectEmployees()
        {
            ICollection<ProjectEmployee> projectEmployees = new HashSet<ProjectEmployee>();
            ProjectEmployee projectEmployee;
            projectEmployee = new ProjectEmployee()
            {
                ProjectId = Guid.Parse("CFC9AB51-C431-4624-98FF-4DA7BE50762D"),
                EmployeeId = Guid.Parse("EC5497AA-1B1C-44AC-8259-7B7B95B07B12"),
            };
            projectEmployees.Add(projectEmployee);
            projectEmployee = new ProjectEmployee()
            {
                ProjectId = Guid.Parse("082258D8-B2EC-410F-ACD7-4BDE06D025D7"),
                EmployeeId = Guid.Parse("EC5497AA-1B1C-44AC-8259-7B7B95B07B12"),
            };
            projectEmployees.Add(projectEmployee);


            return projectEmployees.ToArray();
        }

    }
}
