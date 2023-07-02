namespace CivilEngineerCMS.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Common.EntityValidationConstants.Expense;

public class Expense
{
    public Expense()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }

    [Range(AmountMinValue, AmountMaxValue)]
    public decimal Amount { get; set; }

    [Range(TotalAmountMinValue, TotalAmountMaxValue)]
    public decimal TotalAmount { get; set; }

    public DateTime Date { get; set; }
}