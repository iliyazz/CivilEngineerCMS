namespace CivilEngineerCMS.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? UrlPicturePath { get; set; }
        public byte[]? ImageContent { get; set; }
        public string? ImageName { get; set; }
    }
}