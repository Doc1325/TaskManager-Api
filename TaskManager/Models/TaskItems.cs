using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class TaskItems
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int StatusId { get; set; }

    public int CreatorId { get; set; }

    public int AssignedId { get; set; }

    public virtual Users Assigned { get; set; } = null!;

    public virtual Users Creator { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
