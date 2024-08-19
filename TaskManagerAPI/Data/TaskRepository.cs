using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApi.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Task?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Task?> UpdateTaskAsync(Task task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.TaskId);
            if (existingTask == null) return null;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Completed = task.Completed;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
