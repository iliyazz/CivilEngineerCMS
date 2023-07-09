namespace CivilEngineerCMS.Common;

public static class EntityValidationConstants
{
    public static class Project
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 50;
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 1000;
        public const int UrlMaxLength = 2048;
    }

    public static class Client
    {
        public const int FirstNameMinLength = 3;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMinLength = 3;
        public const int LastNameMaxLength = 50;
        public const int EmailMaxLength = 320;
        public const int PhoneNumberMinLength = 15;
        public const int PhoneNumberMaxLength = 15;
        public const int AddressMinLength = 10;
        public const int AddressMaxLength = 500;
    }

    public static class Employee
    {
        public const int FirstNameMinLength = 3;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMinLength = 3;
        public const int LastNameMaxLength = 50;
        public const int EmailMaxLength = 320;
        public const int PhoneNumberMinLength = 7;
        public const int PhoneNumberMaxLength = 15;
        public const int AddressMinLength = 10;
        public const int AddressMaxLength = 500;
        public const int JobTitleMinLength = 3;
        public const int JobTitleMaxLength = 50;
    }

    public static class Expense
    {
        public const int AmountMinValue = 0;
        public const int AmountMaxValue = 100000000;
        public const int TotalAmountMinValue = 0;
        public const int TotalAmountMaxValue = 100000000;
    }

    public static class Interaction
    {
        public const int TypeMinLength = 3;
        public const int TypeMaxLength = 50;
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 1000;
        public const int MessageMinLength = 10;
        public const int MessageMaxLength = 1000;
        public const int UrlPathMaxLength = 2048;
    }

    public static class Board
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 50;
    }

}