using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class Users
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public virtual ICollection<TaskItems> TaskAssigneds { get; set; } = new List<TaskItems>();

    public virtual ICollection<TaskItems> TaskCreators { get; set; } = new List<TaskItems>();
}
