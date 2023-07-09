namespace CivilEngineerCMS.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Common.EntityValidationConstants.Board;


public class Board
{
    public Board()
    {
        Id = Guid.NewGuid();
        this.Tasks = new HashSet<Task>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; }
}