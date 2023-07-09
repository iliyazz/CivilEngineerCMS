namespace CivilEngineerCMS.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;

using static Common.EntityValidationConstants.Employee;

public class Employee
{
    public Employee()
    {
        Id = Guid.NewGuid();
        this.ProjectsEmployees = new HashSet<ProjectEmployee>();
        this.Projects = new HashSet<Project>();
    }
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    [Phone]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [MaxLength(AddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(JobTitleMaxLength)]
    public string JobTitle { get; set; } = null!;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public virtual ICollection<ProjectEmployee> ProjectsEmployees { get; set; }// = new HashSet<ProjectEmployee>();

    public virtual ICollection<Project> Projects { get; set; }// = new HashSet<Project>();


}