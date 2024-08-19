using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TaskManagementApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Seed data for initial setup
            modelBuilder.Entity<Task>().HasData(
                new Task { TaskId = 1, Title = "Initial Task", Description = "This is the first task", Completed = false }
            );
        }
    }
}
