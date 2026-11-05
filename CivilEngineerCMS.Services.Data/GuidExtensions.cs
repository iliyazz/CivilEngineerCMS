namespace CivilEngineerCMS.Services.Data
{
    internal static class GuidExtensions
    {
        /// <summary>
        /// Parses an id string into a Guid, or returns Guid.Empty when it is null or not a valid Guid.
        /// Used in queries so ids are compared as Guids rather than as strings: SQLite stores Guids
        /// as uppercase TEXT and compares strings case-sensitively, while Guid.ToString() is lowercase.
        /// Guid.Empty never matches a real row, so invalid input behaves as "not found".
        /// </summary>
        internal static Guid ToGuidOrEmpty(this string? value)
        {
            return Guid.TryParse(value, out Guid result) ? result : Guid.Empty;
        }
    }
}
