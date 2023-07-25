﻿namespace CivilEngineerCMS.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    public class LoginFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}