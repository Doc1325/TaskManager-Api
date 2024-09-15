using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

namespace TaskManager.Models
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext>options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }

        public DbSet<TaskItems> Tasks { get; set; }
        public DbSet<Status> Status { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasOne<Roles>() 
                .WithMany()
                .HasForeignKey(t => t.RoleName)
                .HasPrincipalKey(s => s.RoleName); 
        }

    }

   

}
