namespace CivilEngineerCMS.Web.ViewModels.Administrator
{
    /// <summary>
    /// SelectEmployeesForAdministratorFormModel is used to bind the data from the form for selecting employees for an administrator.
    /// </summary>
    public class SelectEmployeesForAdministratorFormModel
    {
        /// <summary>
        /// This property is used to get or set the Id of the employee.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// This property is used to get or set the first name of the employee.
        /// </summary>
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set the last name of the employee.
        /// </summary>
        public string LastName { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set the job title of the employee.
        /// </summary>
        public string JobTitle { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set the email of the employee.
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set if the employee is selected for administrator.
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// This property is used to get or set the UserId of the user.
        /// </summary>
        public Guid UserId { get; set; }
    }
}