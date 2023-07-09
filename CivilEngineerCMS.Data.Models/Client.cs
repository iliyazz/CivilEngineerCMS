namespace CivilEngineerCMS.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Common.EntityValidationConstants.Client;

public class Client
{
    public Client()
    {
        this.Projects = new HashSet<Project>();
        this.Id = Guid.NewGuid();
    }

    [Key] public Guid Id { get; set; }

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

    public virtual ICollection<Project> Projects { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}