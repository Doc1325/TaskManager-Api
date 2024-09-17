using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models;

[Index("RoleName", Name = "IX_Users_RoleName")]
public partial class Users
{
    [Key]
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    [ForeignKey("RoleName")]
    [InverseProperty("Users")]
    public virtual Roles RoleNameNavigation { get; set; } = null!;

    [InverseProperty("Asignned")]
    public virtual ICollection<TaskItems> TaskAsignneds { get; set; } = new List<TaskItems>();

    [InverseProperty("Creator")]
    public virtual ICollection<TaskItems> TaskCreators { get; set; } = new List<TaskItems>();
}
