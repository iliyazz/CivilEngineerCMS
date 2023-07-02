namespace CivilEngineerCMS.Common;

using System.Runtime.Serialization;

public enum ProjectStatusEnums
{
    [EnumMember(Value = "Not yet started")]
    NotYetStarted = 1,
    [EnumMember(Value = "Close to starting")]
    CloseToStarting = 2,
    [EnumMember(Value = "In progress")]
    InProgress = 3,
    [EnumMember(Value = "In progress with a pending action")]
    InProgressWithPendingAction = 4,
    [EnumMember(Value = "In progress with a blocker or ended")]
    InProgressWithBlockerOrEnded = 5,
    [EnumMember(Value = "Finished")]
    Finished = 6
}