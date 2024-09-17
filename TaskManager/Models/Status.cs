using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models;

[Table("Status")]
public partial class Status
{
    [Key]
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<TaskItems> Tasks { get; set; } = new List<TaskItems>();
}
