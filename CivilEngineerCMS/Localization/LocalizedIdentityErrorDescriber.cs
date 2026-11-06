namespace CivilEngineerCMS.Web.Localization
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// Translates the ASP.NET Core Identity errors shown on the registration page.
    /// </summary>
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<SharedResource> localizer;

        public LocalizedIdentityErrorDescriber(IStringLocalizer<SharedResource> localizer)
        {
            this.localizer = localizer;
        }

        public override IdentityError DefaultError()
            => this.Error(nameof(DefaultError), "An unknown failure has occurred.");

        public override IdentityError PasswordMismatch()
            => this.Error(nameof(PasswordMismatch), "Incorrect password.");

        public override IdentityError InvalidUserName(string? userName)
            => this.Error(nameof(InvalidUserName), "Username '{0}' is invalid, can only contain letters or digits.", userName);

        public override IdentityError InvalidEmail(string? email)
            => this.Error(nameof(InvalidEmail), "Email '{0}' is invalid.", email);

        public override IdentityError DuplicateUserName(string userName)
            => this.Error(nameof(DuplicateUserName), "Username '{0}' is already taken.", userName);

        public override IdentityError DuplicateEmail(string email)
            => this.Error(nameof(DuplicateEmail), "Email '{0}' is already taken.", email);

        public override IdentityError PasswordTooShort(int length)
            => this.Error(nameof(PasswordTooShort), "Passwords must be at least {0} characters.", length);

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => this.Error(nameof(PasswordRequiresUniqueChars), "Passwords must use at least {0} different characters.", uniqueChars);

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => this.Error(nameof(PasswordRequiresNonAlphanumeric), "Passwords must have at least one non alphanumeric character.");

        public override IdentityError PasswordRequiresDigit()
            => this.Error(nameof(PasswordRequiresDigit), "Passwords must have at least one digit ('0'-'9').");

        public override IdentityError PasswordRequiresLower()
            => this.Error(nameof(PasswordRequiresLower), "Passwords must have at least one lowercase ('a'-'z').");

        public override IdentityError PasswordRequiresUpper()
            => this.Error(nameof(PasswordRequiresUpper), "Passwords must have at least one uppercase ('A'-'Z').");

        private IdentityError Error(string code, string message, params object?[] arguments)
            => new IdentityError
            {
                Code = code,
                Description = this.localizer[message, arguments!].Value
            };
    }
}
