namespace CivilEngineerCMS.Web.ViewModels.Client
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// This class is used to bind the data from the form for all clients.
    /// </summary>
    public class AllClientViewModel
    {
        /// <summary>
        /// This property is used to get or set the Id of the client.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// This property is used to get or set the first name of the client.
        /// </summary>
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set the last name of the client.
        /// </summary>
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set the phone number of the client.
        /// </summary>
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set the email of the client.
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// This property is used to get or set the address of the client.
        /// </summary>
        public string Address { get; set; } = null!;
        /// <summary>
        /// This property is used to check if client is soft deleted.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// This property is used to get or set the UserId of the user.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
