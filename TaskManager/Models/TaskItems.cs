using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models;

[Index("StatusId", Name = "IX_Tasks_StatusId")]
public partial class TaskItems
{
    [Key]
    public int TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int StatusId { get; set; }

    public int CreatorId { get; set; }

    public int AsignnedId { get; set; }

    [ForeignKey("AsignnedId")]
    [InverseProperty("TaskAsignneds")]
    public virtual Users Asignned { get; set; } = null!;

    [ForeignKey("CreatorId")]
    [InverseProperty("TaskCreators")]
    public virtual Users Creator { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Tasks")]
    public virtual Status Status { get; set; } = null!;
}
