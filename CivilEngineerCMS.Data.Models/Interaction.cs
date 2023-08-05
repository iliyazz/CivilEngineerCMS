namespace CivilEngineerCMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityValidationConstants.Interaction;

    /// <summary>
    /// Interaction entity.
    /// </summary>
    public class Interaction
    {
        public Interaction()
        {
            Id = Guid.NewGuid();
            Date = DateTime.UtcNow;
        }
        /// <summary>
        /// Primary key of the Interaction entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign key of the Interaction entity.
        /// </summary>
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        /// <summary>
        /// Type of the Interaction entity.
        /// </summary>
        [Required]
        [MaxLength(TypeMaxLength)]
        public string Type { get; set; } = null!;
        /// <summary>
        /// Description of the Interaction entity.
        /// </summary>
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;
        /// <summary>
        /// Message of the Interaction entity.
        /// </summary>
        [Required]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; } = null!;
        /// <summary>
        /// Picture Url of the Interaction entity.
        /// </summary>
        [MaxLength(UrlPathMaxLength)]
        public string? UrlPath { get; set; }
        /// <summary>
        /// Date of create or edit the Interaction entity.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

    }
}