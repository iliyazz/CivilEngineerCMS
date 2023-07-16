//namespace CivilEngineerCMS.Web.ViewModels.Client
//{
//    using System.ComponentModel.DataAnnotations;
//    using CivilEngineerCMS.Web.ViewModels.Project.Enums;
//    using Enums;

//    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

//    public class AllClientQueryModel
//    {
//        public AllClientQueryModel()
//        {
//            this.CurrentPage = DefaultPageNumber;
//            this.ClientPerPage = EntitiesPerPage;
//            this.Emails = new HashSet<string>();
//            this.Clients = new HashSet<AllClientViewModel>();
//        }

//        public string? Email { get; set; }
//        [Display(Name = "Search by word")]
//        public string? SearchString{ get; set; }
//        public ProjectSorting ClientSorting { get; set; }
//        public int CurrentPage { get; set; }
//        public int ClientPerPage { get; set; }
//        public IEnumerable<string> Emails { get; set; }
//        public IEnumerable<AllClientViewModel> Clients { get; set; }
//    }
//}
