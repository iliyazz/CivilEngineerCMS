namespace CivilEngineerCMS.Common
{
    public static class GeneralApplicationConstants
    {
        public const int ReleaseYear = 2023;
        public const int DefaultPageNumber = 1;
        public const int EntitiesPerPage = 3;
        public const string AdministratorRoleName = "Administrator";
        public const string DevelopmentAdminEmail = "iliyaz.softuni@gmail.com";
        public const string AdminAreaName = "Admin";
        public const string OnLineUsersCookieName = "OnLineUser";
        public const int OnLineUsersLastActivityInMinutes = 15;
        public const string OnLineClientsCacheKey = "OnLineClientsCache";
        public const string OnLineEmployeesCacheKey = "OnLineEmployeesCache";
        public const int OnLineUsersCacheExpirationInMinutes = 5;
        public const int ImageGeneratedSufixLength = 9;
        public const int ImageMaxSizeInBytes = 5 * 1024 * 1024;
    }
}