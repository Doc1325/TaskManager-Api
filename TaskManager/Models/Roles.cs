using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models;

[Index("RoleName", Name = "AK_Roles_RoleName", IsUnique = true)]
public partial class Roles
{
    [Key]
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    [InverseProperty("RoleNameNavigation")]
    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
