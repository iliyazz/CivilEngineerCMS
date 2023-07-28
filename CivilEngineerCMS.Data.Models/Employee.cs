namespace CivilEngineerCMS.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;

using static Common.EntityValidationConstants.Employee;

/// <summary>
/// Employee entity.
/// </summary>
public class Employee
{
    public Employee()
    {
        Id = Guid.NewGuid();
        this.ProjectsEmployees = new HashSet<ProjectEmployee>();
        this.Projects = new HashSet<Project>();
    }
    /// <summary>
    /// Primary key of the Employee entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// First name of the Employee entity.
    /// </summary>
    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of the Employee entity.
    /// </summary>
    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Phone number of the Employee entity.
    /// </summary>
    [Required]
    [Phone]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = null!;
    /// <summary>
    /// Address of the Employee entity.
    /// </summary>
    [Required]
    [MaxLength(AddressMaxLength)]
    public string Address { get; set; } = null!;

    /// <summary>
    /// Job title of the Employee entity.
    /// </summary>
    [Required]
    [MaxLength(JobTitleMaxLength)]
    public string JobTitle { get; set; } = null!;

    /// <summary>
    /// Soft delete of the Employee entity.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Foreign key of the Employee entity.
    /// </summary>
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// many-to-many relationship between Employee and Project entities.
    /// </summary>
    public virtual ICollection<ProjectEmployee> ProjectsEmployees { get; set; }

    /// <summary>
    /// Projects of the Employee entity.
    /// </summary>
    public virtual ICollection<Project> Projects { get; set; }


}