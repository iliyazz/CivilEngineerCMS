namespace CivilEngineerCMS.Web.ViewModels.Interaction
{
    using System.ComponentModel.DataAnnotations;
    using static CivilEngineerCMS.Common.EntityValidationConstants.Interaction;

    public class AddAndEditInteractionFormModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        [Required]
        [MaxLength(TypeMaxLength)]
        [MinLength(TypeMinLength)]
        public string Type { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(MessageMaxLength)]
        [MinLength(MessageMinLength)]
        public string Message { get; set; } = null!;

        [MaxLength(UrlPathMaxLength)]
        public string? UrlPath { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}

