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

            //builder.HasData(this.GenerateProjectEmployees());

        }


        //private ProjectEmployee[] GenerateProjectEmployees()
        //{
        //    ICollection<ProjectEmployee> projectEmployees = new HashSet<ProjectEmployee>();
        //    ProjectEmployee projectEmployee;
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("28766CD4-25BA-4B4C-8B68-366197404EE5"),
        //        EmployeeId = Guid.Parse("41A1BDAB-D519-4C95-AE73-0425A31D9B6D"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("5B9750F4-6AD9-43A8-9E11-4CCE433D929D"),
        //        EmployeeId = Guid.Parse("41A1BDAB-D519-4C95-AE73-0425A31D9B6D"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        EmployeeId = Guid.Parse("41A1BDAB-D519-4C95-AE73-0425A31D9B6D"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("C54BC710-EFA4-4A2C-8160-812D276D5F3F"),
        //        EmployeeId = Guid.Parse("41A1BDAB-D519-4C95-AE73-0425A31D9B6D"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("D1615808-4CC0-4C32-AD90-9FBE2DE229C3"),
        //        EmployeeId = Guid.Parse("41A1BDAB-D519-4C95-AE73-0425A31D9B6D"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("28766CD4-25BA-4B4C-8B68-366197404EE5"),
        //        EmployeeId = Guid.Parse("15779B34-FF5A-4DB1-B6D2-0B3626F251B8"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("5B9750F4-6AD9-43A8-9E11-4CCE433D929D"),
        //        EmployeeId = Guid.Parse("15779B34-FF5A-4DB1-B6D2-0B3626F251B8"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("C54BC710-EFA4-4A2C-8160-812D276D5F3F"),
        //        EmployeeId = Guid.Parse("15779B34-FF5A-4DB1-B6D2-0B3626F251B8"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("28766CD4-25BA-4B4C-8B68-366197404EE5"),
        //        EmployeeId = Guid.Parse("47FFC1E5-F628-4575-9813-5EFB9198FF63"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        EmployeeId = Guid.Parse("47FFC1E5-F628-4575-9813-5EFB9198FF63"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("38CF7267-BA70-442B-896F-A41F1A4189BC"),
        //        EmployeeId = Guid.Parse("47FFC1E5-F628-4575-9813-5EFB9198FF63"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("BF07487F-B5EA-47A7-820C-B5B19353A104"),
        //        EmployeeId = Guid.Parse("47FFC1E5-F628-4575-9813-5EFB9198FF63"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("38CF7267-BA70-442B-896F-A41F1A4189BC"),
        //        EmployeeId = Guid.Parse("FCC5A05E-0958-4D7B-8429-6215B8EE0B30"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("5B9750F4-6AD9-43A8-9E11-4CCE433D929D"),
        //        EmployeeId = Guid.Parse("B7C8C9B6-880E-487D-9531-AA523A08A8C5"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        EmployeeId = Guid.Parse("B7C8C9B6-880E-487D-9531-AA523A08A8C5"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("D1615808-4CC0-4C32-AD90-9FBE2DE229C3"),
        //        EmployeeId = Guid.Parse("B7C8C9B6-880E-487D-9531-AA523A08A8C5"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("38CF7267-BA70-442B-896F-A41F1A4189BC"),
        //        EmployeeId = Guid.Parse("B7C8C9B6-880E-487D-9531-AA523A08A8C5"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("BF07487F-B5EA-47A7-820C-B5B19353A104"),
        //        EmployeeId = Guid.Parse("B7C8C9B6-880E-487D-9531-AA523A08A8C5"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("28766CD4-25BA-4B4C-8B68-366197404EE5"),
        //        EmployeeId = Guid.Parse("5DEA13AC-A918-4A9E-9147-CF1211F73381"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("5B9750F4-6AD9-43A8-9E11-4CCE433D929D"),
        //        EmployeeId = Guid.Parse("5DEA13AC-A918-4A9E-9147-CF1211F73381"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        EmployeeId = Guid.Parse("5DEA13AC-A918-4A9E-9147-CF1211F73381"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("C54BC710-EFA4-4A2C-8160-812D276D5F3F"),
        //        EmployeeId = Guid.Parse("5DEA13AC-A918-4A9E-9147-CF1211F73381"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("28766CD4-25BA-4B4C-8B68-366197404EE5"),
        //        EmployeeId = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("5B9750F4-6AD9-43A8-9E11-4CCE433D929D"),
        //        EmployeeId = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        EmployeeId = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("C54BC710-EFA4-4A2C-8160-812D276D5F3F"),
        //        EmployeeId = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //    };
        //    projectEmployees.Add(projectEmployee);
        //    projectEmployee = new ProjectEmployee
        //    {
        //        ProjectId = Guid.Parse("D1615808-4CC0-4C32-AD90-9FBE2DE229C3"),
        //        EmployeeId = Guid.Parse("3C8A8219-5FF4-486D-B162-FB52D3DE87C4"),
        //    };
        //    projectEmployees.Add(projectEmployee);

        //    return projectEmployees.ToArray();
        //}
    }
}
