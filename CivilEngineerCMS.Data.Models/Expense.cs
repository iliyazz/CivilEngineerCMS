namespace CivilEngineerCMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityValidationConstants.Expense;
    /// <summary>
    /// Expense entity.
    /// </summary>
    public class Expense
    {
        public Expense()
        {
            Id = Guid.NewGuid();
        }
        /// <summary>
        /// Primary key of the Expense entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key of the Expense entity.
        /// </summary>
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        /// <summary>
        /// Amount of the Expense entity.
        /// </summary>
        [Range(AmountMinValue, AmountMaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Total amount of the Expense entity.
        /// </summary>
        [Range(TotalAmountMinValue, TotalAmountMaxValue)]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Date of create or edit the Expense entity.
        /// </summary>
        public DateTime Date { get; set; }


        /// <summary>
        /// Invoice number of the Expense entity.
        /// </summary>
        [Range(InvoiceNumberMinValue, InvoiceNumberMaxValue)]
        public long InvoiceNumber { get; set; }



    }
}