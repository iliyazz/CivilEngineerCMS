namespace CivilEngineerCMS.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Common;

using static CivilEngineerCMS.Common.EntityValidationConstants.Project;

public class Project
{
    public Project()
    {
        //this.ProjectCreatedDate = DateTime.UtcNow;
        this.Id = Guid.NewGuid();
        this.ProjectsEmployees = new HashSet<ProjectEmployee>();
        this.Interactions = new HashSet<Interaction>();
    }

    [Key]
    public Guid Id { get; set; }


    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    //[ForeignKey(nameof(Client))]
    public Guid ClientId { get; set; }
    public virtual Client Client { get; set; } = null!;


    [Required]
    //[ForeignKey(nameof(Manager))]
    public Guid ManagerId { get; set; }
    public virtual Employee Manager { get; set; } = null!;

    public virtual ICollection<ProjectEmployee> ProjectsEmployees { get; set; }// = new HashSet<ProjectEmployee>();

    [MaxLength(UrlMaxLength)]
    public string? UrlPicturePath { get; set; }

    [Required]
    public ProjectStatusEnums Status { get; set; }
    public DateTime ProjectCreatedDate { get; set; }
    public DateTime ProjectEndDate { get; set; }

    public virtual ICollection<Interaction> Interactions { get; set; }// = new HashSet<Interaction>();

    //public Guid UserId { get; set; }
    //public virtual ApplicationUser User { get; set; }

}