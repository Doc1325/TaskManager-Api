using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class Status
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<TaskItems> Tasks { get; set; } = new List<TaskItems>();
}
