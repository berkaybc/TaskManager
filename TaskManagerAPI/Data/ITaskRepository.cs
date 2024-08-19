using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApi.Data
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Task>> GetAllTasksAsync();
        Task<Task?> GetTaskByIdAsync(int id);
        Task<Task> CreateTaskAsync(Task task);
        Task<Task?> UpdateTaskAsync(Task task);
        Task<bool> DeleteTaskAsync(int id);
    }
}