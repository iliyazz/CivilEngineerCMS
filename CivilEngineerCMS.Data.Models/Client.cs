namespace CivilEngineerCMS.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Common.EntityValidationConstants.Client;

/// <summary>
/// Client entity.
/// </summary>
public class Client
{
    public Client()
    {
        this.Projects = new HashSet<Project>();
        this.Id = Guid.NewGuid();
    }
    /// <summary>
    /// Primary key of the Client entity.
    /// </summary>
    [Key] public Guid Id { get; set; }

    /// <summary>
    /// First name of the Client entity.
    /// </summary>
    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of the Client entity.
    /// </summary>
    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Phone number of the Client entity.
    /// </summary>
    [Required]
    [Phone]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Address of the Client entity.
    /// </summary>
    [Required]
    [MaxLength(AddressMaxLength)]
    public string Address { get; set; } = null!;

    /// <summary>
    /// Soft delete of the Client entity.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Projects of the Client entity.
    /// </summary>
    public virtual ICollection<Project> Projects { get; set; }

    /// <summary>
    /// Foreign key of the Client entity.
    /// </summary>
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;


}