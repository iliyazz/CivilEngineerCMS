namespace CivilEngineerCMS.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Common;

using static CivilEngineerCMS.Common.EntityValidationConstants.Project;

/// <summary>
/// Project entity.
/// </summary>
public class Project
{
    public Project()
    {
        this.Id = Guid.NewGuid();
        this.ProjectsEmployees = new HashSet<ProjectEmployee>();
        this.Interactions = new HashSet<Interaction>();
    }
    /// <summary>
    /// Primary key of the Project entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the Project entity.
    /// </summary>
    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Description of the Project entity.
    /// </summary>
    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Guid ClientId { get; set; }
    public virtual Client Client { get; set; } = null!;


    [Required]
    public Guid ManagerId { get; set; }
    public virtual Employee Manager { get; set; } = null!;

    public virtual ICollection<ProjectEmployee> ProjectsEmployees { get; set; }

    [MaxLength(UrlMaxLength)]
    public string? UrlPicturePath { get; set; }

    [Required]
    public ProjectStatusEnums Status { get; set; }
    public DateTime ProjectCreatedDate { get; set; }
    public DateTime ProjectEndDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Interaction> Interactions { get; set; }

}