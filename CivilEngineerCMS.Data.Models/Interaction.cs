namespace CivilEngineerCMS.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Common.EntityValidationConstants.Interaction;

public class Interaction
{
    public Interaction()
    {
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(TypeMaxLength)]
    public string Type { get; set; } = null!;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    [MaxLength(MessageMaxLength)]
    public string Message { get; set; } = null!;

    [MaxLength(UrlPathMaxLength)]
    public string? UrlPath { get; set; }

    [Required]
    public DateTime Date { get; set; }

}