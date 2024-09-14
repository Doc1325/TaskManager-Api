using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext>options) : base(options)
        {

        }

        public DbSet<TaskItems> Tasks { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
