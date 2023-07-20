//namespace CivilEngineerCMS.Data.Models;

//using System;
//using System.ComponentModel.DataAnnotations.Schema;

//public class Task
//{

//    public Task()
//    {
//        this.Id = Guid.NewGuid();
//    }
//    public Guid Id { get; set; }

//    [ForeignKey(nameof(Project))]
//    public Guid ProjectId { get; set; }
//    public Project Project { get; set; } = null!;


//    public Guid BoardId { get; set; }
//    public Board Board { get; set; } = null!;
//}