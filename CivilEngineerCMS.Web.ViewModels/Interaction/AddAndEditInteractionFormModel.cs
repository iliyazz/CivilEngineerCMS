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
        [Display(Name = "Type")]
        public string Type { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(MessageMaxLength)]
        [MinLength(MessageMinLength)]
        [Display(Name = "Message")]
        public string Message { get; set; } = null!;

        [MaxLength(UrlPathMaxLength)]
        [Display(Name = "File Link (optional)")]
        public string? UrlPath { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }
}

