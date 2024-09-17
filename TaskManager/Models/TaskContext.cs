using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models;

public partial class TaskContext : DbContext
{
    public TaskContext()
    {
    }

    public TaskContext(DbContextOptions<TaskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Status> Status { get; set; }

    public virtual DbSet<TaskItems> Tasks { get; set; }

    public virtual DbSet<Users> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItems>(entity =>
        {
            entity.HasOne(d => d.Asignned).WithMany(p => p.TaskAsignneds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Users1");

            entity.HasOne(d => d.Creator).WithMany(p => p.TaskCreators)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Users");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasOne(d => d.RoleNameNavigation).WithMany(p => p.Users)
                .HasPrincipalKey(p => p.RoleName)
                .HasForeignKey(d => d.RoleName);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
