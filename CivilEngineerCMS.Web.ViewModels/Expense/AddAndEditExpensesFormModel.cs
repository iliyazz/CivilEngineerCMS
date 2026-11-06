namespace CivilEngineerCMS.Web.ViewModels.Expenses
{
    using System.ComponentModel.DataAnnotations;

    using static CivilEngineerCMS.Common.EntityValidationConstants.Expense;

    public class AddAndEditExpensesFormModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        [Required]
        [Range(AmountMinValue, AmountMaxValue)]
        [Display(Name = "Paid Amount (€)")]
        public decimal Amount { get; set; }

        [Required]
        [Range(TotalAmountMinValue, TotalAmountMaxValue)]
        [Display(Name = "Total Required (€)")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        //[Range(InvoiceNumberMinValue, InvoiceNumberMaxValue)]
        //public long InvoiceNumber { get; set; }

        //decimal remainingPaiment = 0;
    }
}


