namespace CivilEngineerCMS.Web.ViewModels.Expenses
{
    using System.ComponentModel.DataAnnotations;

    using static CivilEngineerCMS.Common.EntityValidationConstants.Expense;

    public class AddAndEditExpensesFormModel
    {
        public Guid ProjectId { get; set; }

        [Range(AmountMinValue, AmountMaxValue)]
        public decimal Amount { get; set; }

        [Range(TotalAmountMinValue, TotalAmountMaxValue)]
        public decimal TotalAmount { get; set; }

        public DateTime Date { get; set; }

    }
}


