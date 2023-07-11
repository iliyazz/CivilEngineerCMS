﻿namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    public class DetailsEmployeeViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
    }
}
