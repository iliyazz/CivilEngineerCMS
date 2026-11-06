namespace CivilEngineerCMS.Common
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public enum ProjectStatusEnums
    {
        [EnumMember(Value = "Not yet started")]
        [Display(Name = "Not yet started")]
        NotYetStarted = 1,
        [EnumMember(Value = "Close to starting")]
        [Display(Name = "Close to starting")]
        CloseToStarting = 2,
        [EnumMember(Value = "In progress")]
        [Display(Name = "In progress")]
        InProgress = 3,
        [EnumMember(Value = "In progress with a pending action")]
        [Display(Name = "In progress with a pending action")]
        InProgressWithPendingAction = 4,
        [EnumMember(Value = "In progress with a blocker or ended")]
        [Display(Name = "In progress with a blocker or ended")]
        InProgressWithBlockerOrEnded = 5,
        [EnumMember(Value = "Finished")]
        [Display(Name = "Finished")]
        Finished = 6
    }
}