﻿namespace CivilEngineerCMS.Web.ViewModels.Client
{
    public class ProjectSelectClientFormModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => FirstName + " " + LastName;
    }
}
