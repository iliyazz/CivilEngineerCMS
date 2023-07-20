namespace CivilEngineerCMS.Data;

using System.Reflection;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Models;

public class CivilEngineerCmsDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public CivilEngineerCmsDbContext(DbContextOptions<CivilEngineerCmsDbContext> options)
        : base(options)
    {
    }

    //public DbSet<Board> Boards { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Employee> Employees { set; get; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<Interaction> Interactions { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectEmployee> ProjectsEmployees { get; set; } = null!;
    //public DbSet<Task> Tasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Assembly configurationsAssembly = Assembly.GetAssembly(typeof(CivilEngineerCmsDbContext)) ?? Assembly.GetExecutingAssembly();
        builder.ApplyConfigurationsFromAssembly(configurationsAssembly);

        base.OnModelCreating(builder);

    }
}